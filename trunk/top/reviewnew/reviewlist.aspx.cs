using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;
using System.Text;
using System.IO;
using Taobao.Top.Api;
using Taobao.Top.Api.Domain;
using Taobao.Top.Api.Request;

public partial class top_review_reviewlist : System.Web.UI.Page
{
    public string session = string.Empty;
    public string nick = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string id = utils.NewRequest("id", utils.RequestType.QueryString);
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        session = cookie.getCookie("top_sessiongroupbuy");
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Decrypt(taobaoNick);

        //过期判断
        if (string.IsNullOrEmpty(taobaoNick))
        {
            string msg = "尊敬的淘宝卖家，非常抱歉的告诉您，您还没有购买此服务或者登录信息已失效，如需继续使用请<a href='http://fuwu.taobao.com/serv/detail.htm?service_id=4545' target='_blank'>进入该服务</a>，谢谢！";
            Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
            Response.End();
            return;
        }

        //判断VIP版本，只有VIP才能使用此功能
        string sql = "SELECT * FROM TCS_ShopSession WHERE nick = '" + nick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            string flag = dt.Rows[0]["version"].ToString();
            if (flag == "0")
            {
                Response.Redirect("xufei.aspx");
                Response.End();
                return;
            }
        }

        if (!IsPostBack)
        {
            BindData();
        }

        string t = utils.NewRequest("t", utils.RequestType.Form);
        string ids = utils.NewRequest("id", utils.RequestType.Form);

        if (t == "ok")
        {
            string[] idsArray = ids.Split(',');
            for (int i = 0; i < idsArray.Length; i++)
            {
                //try
                //{
                    UpdateReview(idsArray[i]);
                //}
                //catch { }
            }

            Response.Write("<script>alert('设置成功!');history.go(-1);</script>");
            Response.End();
            return;
        }
    }

    /// <summary>
    /// 更新展示评价
    /// </summary>
    private void UpdateReview(string id)
    {
        //根据商品ID获取商品详细信息
        string sql = "SELECT itemid FROM TCS_TradeRate WHERE orderid = '" + id + "' AND nick = '" + nick + "'";
        string itemid = utils.ExecuteString(sql);

        //发送请求获取
        string appkey = "12159997";
        string secret = "614e40bfdb96e9063031d1a9e56fbed5";
        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", appkey, secret);
        ItemGetRequest request = new ItemGetRequest();
        request.Fields = "title,price,pic_url";
        Response.Write(itemid);
        Response.End();
        return;
        request.NumIid = long.Parse(itemid);
        Item product = client.ItemGet(request, session);

        //获取最近30天商品售出数量
        sql = "SELECT COUNT(*) FROM TCS_TradeRate WHERE itemid = '" + itemid + "'";
        string sale = utils.ExecuteString(sql);

        //获取优惠券赠送信息
        sql = "SELECT * FROM TCS_Coupon WHERE guid = (SELECT couponid FROM TCS_ShopConfig WHERE nick = '" + nick + "')";
        string showcontent = string.Empty;

        try
        {
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                showcontent = "恭喜该用户获得本店送出的满" + dt.Rows[0]["condition"].ToString() + "减" + dt.Rows[0]["num"].ToString() + "元的优惠券！";
            }
        }
        catch
        {

        }

        //获取用户等级
        sql = "SELECT buynick FROM TCS_TradeRate WHERE orderid = '" + id + "' AND nick = '" + nick + "'";
        string buynick = utils.ExecuteString(sql);

        sql = "SELECT buyerlevel FROM TCS_Customer WHERE buynick = '" + buynick + "'";
        string userlevel = utils.ExecuteString(sql);

        sql = "UPDATE TCS_TradeRate SET isshow = 1,itemname='" + product.Title + "',itemsrc='" + product.PicUrl + "',price='" + product.Price + "',sale='" + sale + "',showcontent = '" + showcontent + "',userlevel='" + userlevel + "',showindex=100 WHERE orderid = '" + id + "' AND nick = '" + nick + "'";
        //Response.Write(sql);

        utils.ExecuteNonQuery(sql);
    }


    protected void Button2_Click(object sender, EventArgs e)
    {
        StringBuilder builder = new StringBuilder();
        //导出符合条件的评价列表
        string sql = "SELECT * FROM TCS_TradeRate WHERE nick = '" + nick + "' ORDER BY reviewdate DESC";
        DataTable dt = utils.ExecuteDataTable(sql);

        //获取卖家基本设置
        sql = "SELECT * FROM TCS_ShopConfig WHERE nick = '" + nick + "'";
        DataTable dtShop = utils.ExecuteDataTable(sql);
        if (dtShop.Rows.Count != 0)
        {
            builder.Append("买家昵称,评价内容,评价时间,订单号");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (CheckContent(dt.Rows[i], dtShop))
                {
                    builder.Append("\r\n");
                    builder.Append("\"" + dt.Rows[i]["buynick"].ToString() + "\",");
                    builder.Append("\"" + dt.Rows[i]["content"].ToString() + "\",");
                    builder.Append("\"" + dt.Rows[i]["reviewdate"].ToString() + "\",");
                    builder.Append("\"" + dt.Rows[i]["orderid"].ToString() + "\"");
                }
            }

            //生成excel文件
            string fileName = "tmp/" + nick + DateTime.Now.Ticks.ToString() + ".csv";
            File.WriteAllText(Server.MapPath(fileName), builder.ToString(), Encoding.Default);

            Response.Redirect(fileName);
        }
    }

    private bool CheckContent(DataRow dataRow, DataTable dtShop)
    {
        //是否好评
        if (dataRow["result"].ToString() != "good")
        {
            return false;
        }

        //自动内容审核
        if (dtShop.Rows[0]["iskeyword"].ToString() == "1")
        {
            if (CheckContent(dtShop.Rows[0]["keywordisbad"].ToString(), dtShop.Rows[0]["keyword"].ToString(), dtShop.Rows[0]["badkeyword"].ToString(), dtShop.Rows[0]["wordcount"].ToString(), dataRow["content"].ToString()))
            {
                return false;
            }
        }

        //默认好评判定
        if (dtShop.Rows[0]["iscancelauto"].ToString() == "1"
        && ((dataRow["content"].ToString() == "好评！" && dtShop.Rows[0]["cancel1"].ToString() == "1")
            || ((dataRow["content"].ToString() == "评价方未及时做出评价,系统默认好评!" || dataRow["content"].ToString() == "评价方未及时做出评价,系统默认好评！") && dtShop.Rows[0]["cancel2"].ToString() == "1")
        ))
        {
            return false;
        }

        string sql = "SELECT * FROM TCS_Trade WHERE orderid = '" + dataRow["orderid"].ToString() + "'";
        DataTable dtTrade = utils.ExecuteDataTable(sql);
        if (dtTrade.Rows.Count != 0)
        {
            //物流周期判定SELF
            if (dtTrade.Rows[0]["typ"].ToString() == "self" && (DateTime.Parse(dtTrade.Rows[0]["reviewdate"].ToString()) - DateTime.Parse(dtTrade.Rows[0]["senddate"].ToString())).TotalSeconds > int.Parse(dtShop.Rows[0]["maxdate"].ToString()) * 86400)
            {
                return false;
            }

            //物流周期判定SYSTEM
            if (dtTrade.Rows[0]["typ"].ToString() == "system" && (DateTime.Parse(dtTrade.Rows[0]["reviewdate"].ToString()) - DateTime.Parse(dtTrade.Rows[0]["shippingdate"].ToString())).TotalSeconds > int.Parse(dtShop.Rows[0]["mindate"].ToString()) * 86400)
            {
                return false;
            }
        }


        return true;
    }


    private bool CheckContent(string isbad, string goodkey, string badkey, string len, string content)
    {
        //长度判断
        if (content.Length < int.Parse(len))
        {
            return true;
        }

        //内容判断
        if (isbad == "1")
        {
            string[] keyArray = badkey.Split('|');
            for (int i = 0; i < keyArray.Length; i++)
            {
                if (keyArray[i] != "")
                {
                    if (content.IndexOf(keyArray[i].Trim()) != -1)
                    {
                        return true;
                    }
                }
            }
        }
        else
        {
            string[] keyArray = goodkey.Split('|');
            for (int i = 0; i < keyArray.Length; i++)
            {
                if (keyArray[i] != "")
                {
                    if (content.IndexOf(keyArray[i].Trim()) != -1)
                    {
                        return false;
                    }
                }
            }
        }

        return false;
    }


    protected void Button1_Click(object sender, EventArgs e)
    {
        if (search.Text.Trim() == "")
        {
            Response.Redirect("reviewlist.aspx");
            return;
        }

        string sqlNew = "SELECT * FROM TCS_TradeRate WHERE nick = '" + nick + "' AND buynick = '" + search.Text.Trim().Replace("'", "''") + "'";
        DataTable dt = utils.ExecuteDataTable(sqlNew);

        rptArticle.DataSource = dt;
        rptArticle.DataBind();

        lbPage.Text = "";
    }

    public static string getimg(string str)
    {
        return "images/" + str + ".jpg";
    }

    private void BindData()
    {
        string page = utils.NewRequest("page", utils.RequestType.QueryString);
        int pageNow = 1;
        if (page == "")
        {
            pageNow = 1;
        }
        else
        {
            pageNow = int.Parse(page);
        }
        int pageCount = 12;
        int dataCount = (pageNow - 1) * pageCount;

        string sqlNew = "SELECT TOP " + pageCount.ToString() + " * FROM (SELECT *,ROW_NUMBER() OVER (ORDER BY b.reviewdate DESC) AS rownumber FROM TCS_TradeRate b WHERE b.nick = '" + nick + "') AS a WHERE a.rownumber > " + dataCount.ToString() + " ORDER BY a.reviewdate DESC";
        DataTable dt = utils.ExecuteDataTable(sqlNew);

        rptArticle.DataSource = dt;
        rptArticle.DataBind();

        //分页数据初始化
        sqlNew = "SELECT COUNT(*) FROM TCS_TradeRate WHERE nick = '" + nick + "'";
        int totalCount = int.Parse(utils.ExecuteString(sqlNew));

        lbPage.Text = InitPageStr(totalCount, "reviewlist.aspx");
    }

    private string InitPageStr(int total, string url)
    {
        //分页数据初始化
        string str = string.Empty;
        int pageCount = 12;
        int pageSize = 0;
        int pageNow = 1;
        string page = utils.NewRequest("page", utils.RequestType.QueryString);
        if (page == "")
        {
            pageNow = 1;
        }
        else
        {
            pageNow = int.Parse(page);
        }

        //取总分页数
        if (total % pageCount == 0)
        {
            pageSize = total / pageCount;
        }
        else
        {
            pageSize = total / pageCount + 1;
        }

        //如果总页面大于20，则最大页面差不超过20
        int start = 1;
        int end = 20;

        if (pageSize < end)
        {
            end = pageSize;
        }
        else
        {
            if (pageNow > 15)
            {
                start = pageNow - 10;

                if (pageNow < (total - 10))
                {
                    end = pageNow + 10;
                }
                else
                {
                    end = total;
                }
            }
        }

        for (int i = start; i <= end; i++)
        {
            if (i.ToString() == pageNow.ToString())
            {
                str += i.ToString() + " ";
            }
            else
            {
                str += "<a href='" + url + "?page=" + i.ToString() + "'>[" + i.ToString() + "]</a> ";
            }
        }

        return str;
    }


    public static string left(string str)
    {
        string newstr = string.Empty;
        if (str.Length < 25)
        {
            newstr = str;
        }
        else
        {
            newstr = "<span title='" + str + "'>" + str.Substring(0, 25) + "..</span>";
        }
        return newstr;
    }
}