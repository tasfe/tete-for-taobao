using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Common;
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;
using System.Net;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

public partial class top_reviewnew_actadd : System.Web.UI.Page
{
    public string session = string.Empty;
    public string nick = string.Empty;

    public string couponstr = string.Empty;
    public string couponid = string.Empty;
    public string alipaystr = string.Empty;
    public string alipayid = string.Empty;
    public string freestr = string.Empty;
    public string freeid = string.Empty;
    public string itemlist = string.Empty;
    public string itemliststr = string.Empty;

    public string startdate = string.Empty;
    public string enddate = string.Empty;

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

        //过期判断
        if (!IsBuy(nick))
        {
            string msg = "尊敬的" + nick + "，非常抱歉的告诉您，只有VIP版本才能使用【多阶梯活动创建】功能，如需继续使用请<a href='http://fuwu.taobao.com/ser/detail.htm?service_code=service-0-22904&frm=haoping' target='_blank'>购买高级会员服务</a>，谢谢！";
            Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
            Response.End();
            return;
        }

        BindData();
    }
    
    //创建新优惠券
    protected void Button1_Click(object sender, EventArgs e)
    {
        string name = utils.NewRequest("name", utils.RequestType.Form);
        string startdate = utils.NewRequest("startdate", utils.RequestType.Form);
        string enddate = utils.NewRequest("enddate", utils.RequestType.Form);
        string condprice = utils.NewRequest("condprice", utils.RequestType.Form);
        string conditemlist = utils.NewRequest("productid", utils.RequestType.Form);

        string sql = "INSERT INTO TCS_Activity (" +
                        "nick, " +
                        "name, " +
                        "startdate, " +
                        "enddate, " +
                        "condprice, " +
                        "conditemlist " +
                    " ) VALUES ( " +
                        " '" + nick + "', " +
                        " '" + name + "', " +
                        " '" + startdate + "', " +
                        " '" + enddate + "', " +
                        " '" + condprice + "', " +
                        " '" + conditemlist + "' " +
                    ") ";
        utils.ExecuteNonQuery(sql);

        Response.Redirect("actlist.aspx");
    }

    private void BindData()
    {
        startdate = DateTime.Now.ToString("yyyy-MM-dd");
        enddate = DateTime.Now.AddMonths(3).ToString("yyyy-MM-dd");

        //数据绑定
        DataTable dtCoupon = utils.ExecuteDataTable("SELECT * FROM TCS_Coupon WHERE nick = '" + nick + "' AND isdel = 0");
        couponstr = "<select name='couponid'><option value=''>-请选择-</option>";
        for (int i = 0; i < dtCoupon.Rows.Count; i++)
        {
            if (dtCoupon.Rows[i]["guid"].ToString().Trim() == couponid.Trim())
            {
                couponstr += "<option value='" + dtCoupon.Rows[i]["guid"].ToString() + "' selected>" + dtCoupon.Rows[i]["name"].ToString() + " " + DateTime.Parse(dtCoupon.Rows[i]["enddate"].ToString()).ToString("yyyy-MM-dd") + " - " + dtCoupon.Rows[i]["num"].ToString() + "元</option>";
            }
            else
            {
                couponstr += "<option value='" + dtCoupon.Rows[i]["guid"].ToString() + "'>" + dtCoupon.Rows[i]["name"].ToString() + " " + DateTime.Parse(dtCoupon.Rows[i]["enddate"].ToString()).ToString("yyyy-MM-dd") + " - " + dtCoupon.Rows[i]["num"].ToString() + "元</option>";
            }
        }
        couponstr += "</select>";



        dtCoupon = utils.ExecuteDataTable("SELECT * FROM TCS_Alipay WHERE nick = '" + nick + "' AND isdel = 0");
        alipaystr = "<select name='alipayid'><option value=''>-请选择-</option>";
        for (int i = 0; i < dtCoupon.Rows.Count; i++)
        {
            if (dtCoupon.Rows[i]["guid"].ToString().Trim() == alipayid.Trim())
            {
                alipaystr += "<option value='" + dtCoupon.Rows[i]["guid"].ToString() + "' selected>" + dtCoupon.Rows[i]["name"].ToString() + " " + dtCoupon.Rows[i]["adddate"].ToString() + " - " + dtCoupon.Rows[i]["num"].ToString() + "元</option>";
            }
            else
            {
                alipaystr += "<option value='" + dtCoupon.Rows[i]["guid"].ToString() + "'>" + dtCoupon.Rows[i]["name"].ToString() + " " + dtCoupon.Rows[i]["adddate"].ToString() + " - " + dtCoupon.Rows[i]["num"].ToString() + "元</option>";
            }
        }
        alipaystr += "</select>";

        dtCoupon = utils.ExecuteDataTable("SELECT * FROM TCS_FreeCardAction WHERE nick = '" + nick + "' AND isdel = 0");
        freestr = "<select name='freeid'><option value=''>-请选择-</option>";
        for (int i = 0; i < dtCoupon.Rows.Count; i++)
        {
            if (dtCoupon.Rows[i]["guid"].ToString().Trim() == freeid.Trim())
            {
                freestr += "<option value='" + dtCoupon.Rows[i]["guid"].ToString() + "' selected>" + dtCoupon.Rows[i]["name"].ToString() + " " + dtCoupon.Rows[i]["adddate"].ToString() + " - " + dtCoupon.Rows[i]["price"].ToString() + "元</option>";
            }
            else
            {
                freestr += "<option value='" + dtCoupon.Rows[i]["guid"].ToString() + "'>" + dtCoupon.Rows[i]["name"].ToString() + " " + dtCoupon.Rows[i]["adddate"].ToString() + " - " + dtCoupon.Rows[i]["price"].ToString() + "元</option>";
            }
        }
        freestr += "</select>";


        //商品数据绑定
        if (itemlist.Length != 0)
        {
            string[] ary = itemlist.Split(',');
            for (int i = 0; i < ary.Length; i++)
            {
                itemliststr += GetItemHtml(ary[i]);
            }
        }
    }


    private string GetItemHtml(string itemid)
    {
        string str = string.Empty;

        string appkey = "12159997";
        string secret = "614e40bfdb96e9063031d1a9e56fbed5";
        List<Item> itemList = new List<Item>();
        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", appkey, secret);
        try
        {
            ItemGetRequest request = new ItemGetRequest();
            request.Fields = "num_iid,title,price,pic_url";
            request.NumIid = long.Parse(itemid);

            Item product = client.ItemGet(request);
            itemList.Add(product);


            //团购需要的商品数据
            for (int i = 0; i < itemList.Count; i++)
            {
                str = "<div id=item_" + itemList[i].NumIid.ToString() + " style=\"float:left;width:46px;border:solid 1px #ccc;padding:2px;margin:2px;\"><A href=\"http://item.taobao.com/item.htm?id=" + itemList[i].NumIid.ToString() + "\" target=\"_blank\"><IMG src=\"" + itemList[i].PicUrl + "_40x40.jpg\" border=0 title=\"" + itemList[i].Title + "\" /></A><br>" + itemList[i].Price + "<input type=\"hidden\" id=\"productid\" name=\"productid\" value=\"" + itemList[i].NumIid.ToString() + "\"><input type=\"hidden\" id=\"price\" name=\"price\" value=\"" + itemList[i].Price.ToString() + "\"><br><a href=\"javascript:delitem(" + itemList[i].NumIid.ToString() + ")\">删除</a></div>";
            }
        }
        catch { }

        return str;
    }

    public static string check(string str, string val)
    {
        if (str == val)
        {
            return "checked";
        }
        return "";
    }

    /// <summary>
    /// 判断该用户是否订购了该服务
    /// </summary>
    /// <param name="nick"></param>
    /// <returns></returns>
    private bool IsBuy(string nick)
    {
        string sql = "SELECT version FROM TCS_ShopSession WHERE nick = '" + nick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            string plus = dt.Rows[0][0].ToString();
            if (plus == "3")
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
}