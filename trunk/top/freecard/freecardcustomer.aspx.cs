using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Common;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;
using System.Net;
using System.Security.Cryptography;

public partial class top_freecard_freecardcustomer : System.Web.UI.Page
{
    public string session = string.Empty;
    public string nick = string.Empty;
    public string id = string.Empty;
    public string act = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        id = utils.NewRequest("id", utils.RequestType.QueryString);
        act = utils.NewRequest("act", utils.RequestType.QueryString);
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

        //过期判断
        if (!IsBuy(nick))
        {
            string msg = "尊敬的" + nick + "，非常抱歉的告诉您，您尚未订购该功能，如需继续使用请购买该服务:<br><br>29元/月 【赠送短信100条】 <a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22904-11:1;' target='_blank'>立即购买</a><br><br>78元/季 【赠送短信300条】 <a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22904-11:3;' target='_blank'>立即购买</a><br><br>148元/半年 【赠送短信600条】<a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22904-11:6;' target='_blank'>立即购买</a><br><br>288元/年 【赠送短信1200条】<a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22904-11:12;' target='_blank'>立即购买</a><br>";
            Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
            Response.End();
            return;
        }

        if (act == "del")
        {
            string sql = "UPDATE TCS_FreeCard SET isdel = 1 WHERE nick = '" + nick + "' AND guid = '" + id + "'";
            utils.ExecuteNonQuery(sql);
            Response.Redirect("freecardcustomer.aspx");
            return;
        }

        BindData();
    }




    protected void Button3_Click(object sender, EventArgs e)
    {
        StringBuilder builder = new StringBuilder();
        string sql = "SELECT * FROM TCS_FreeCard b INNER JOIN TCS_FreeCardAction a ON a.guid = b.guid WHERE b.nick = '" + nick + "' AND b.isdel = 0";
        DataTable dt = utils.ExecuteDataTable(sql);
        builder.Append("名称,包邮地区,包邮次数,满金额,买家,订单号,赠送日期,有效期");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            builder.Append("\r\n");
            builder.Append(dt.Rows[i]["name"].ToString());
            builder.Append(",");
            builder.Append(dt.Rows[i]["arealist"].ToString());
            builder.Append(",");
            builder.Append(dt.Rows[i]["usecount"].ToString());
            builder.Append(",");
            builder.Append(dt.Rows[i]["price"].ToString());
            builder.Append(",");
            builder.Append(dt.Rows[i]["buynick"].ToString());
            builder.Append(",");
            builder.Append(dt.Rows[i]["orderid"].ToString());
            builder.Append(",");
            builder.Append(dt.Rows[i]["senddate"].ToString());
            builder.Append(",");
            builder.Append(dt.Rows[i]["carddate"].ToString());
        }
        //生成excel文件
        string fileName = "tmp/" + nick + DateTime.Now.Ticks.ToString() + ".csv";
        File.WriteAllText(Server.MapPath(fileName), builder.ToString(), Encoding.Default);

        Response.Redirect(fileName);
    }


    protected void Button2_Click(object sender, EventArgs e)
    {
        if (search.Text.Trim() == "")
        {
            Response.Redirect("freecardcustomer.aspx");
            return;
        }

        string sqlNew = "SELECT * FROM TCS_FreeCard WITH (NOLOCK) WHERE nick = '" + nick + "' AND buynick = '" + search.Text.Trim().Replace("'", "''") + "' AND isdel = 0";
        DataTable dt = utils.ExecuteDataTable(sqlNew);

        rptArticle.DataSource = dt;
        rptArticle.DataBind();

        lbPage.Text = "";
    }

    /// <summary>
    /// 判断该用户是否订购了该服务
    /// </summary>
    /// <param name="nick"></param>
    /// <returns></returns>
    private bool IsBuy(string nick)
    {
        string sql = "SELECT plus FROM TCS_ShopSession WHERE nick = '" + nick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            string plus = dt.Rows[0][0].ToString();
            if (plus.IndexOf("freecard") != -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    private void BindData()
    {
        string count = utils.NewRequest("count", utils.RequestType.QueryString);
        string isbirth = utils.NewRequest("isbirth", utils.RequestType.QueryString);
        string condition = string.Empty;
        string orderby = string.Empty;
        string pageUrl = "freecardcustomer.aspx?1=1";

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

        string sql = "SELECT TOP " + pageCount.ToString() + " * FROM (SELECT *,ROW_NUMBER() OVER (ORDER BY startdate DESC) AS rownumber FROM TCS_FreeCard b WITH (NOLOCK) WHERE b.nick = '" + nick + "' AND isdel = 0 ) AS a WHERE a.rownumber > " + dataCount.ToString() + " ORDER BY startdate DESC";
        DataTable dt = utils.ExecuteDataTable(sql);

        rptArticle.DataSource = dt;
        rptArticle.DataBind();

        //分页数据初始化
        sql = "SELECT COUNT(*) FROM TCS_FreeCard b WHERE b.nick = '" + nick + "' AND isdel = 0";
        int totalCount = int.Parse(utils.ExecuteString(sql));

        lbPage.Text = InitPageStr(totalCount, pageUrl);

        //string sql = "SELECT * FROM TCS_FreeCard WHERE nick = '" + nick + "' AND isdel = 0";
        //DataTable dt = utils.ExecuteDataTable(sql);

        //rptArticle.DataSource = dt;
        //rptArticle.DataBind();
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

                if (pageNow < (pageSize - 10))
                {
                    end = pageNow + 10;
                }
                else
                {
                    end = pageSize;
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
                str += "<a href='" + url + "&page=" + i.ToString() + "'>[" + i.ToString() + "]</a> ";
            }
        }

        return str;
    }
}