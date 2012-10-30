using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;
using System.Text.RegularExpressions;
using System.Net;
using System.Text;
using System.IO;
using System.Security.Cryptography;

public partial class top_reviewnew_saletotal : System.Web.UI.Page
{
    public string session = string.Empty;
    public string nick = string.Empty;
    public string totalcount = string.Empty;
    public string totalprice = string.Empty;
    public string totalcount1 = string.Empty;
    public string totalprice1 = string.Empty;
    public string totalcount2 = string.Empty;
    public string totalprice2 = string.Empty;

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

        BindData();
    }

    private void BindData()
    {
        //优惠券带来的
        string sql = "SELECT COUNT(*) FROM TCS_Trade WHERE nick = '" + nick + "' AND iscoupon = 1 AND mobile <> ''";
        totalcount = utils.ExecuteString(sql);

        sql = "SELECT SUM(Convert(decimal,totalprice)) FROM TCS_Trade WHERE nick = '" + nick + "' AND iscoupon = 1 AND mobile <> ''";
        totalprice = utils.ExecuteString(sql);

        //免邮卡带来的
        sql = "SELECT COUNT(*) FROM TCS_Trade WHERE nick = '" + nick + "' AND orderid IN (SELECT orderid FROM TCS_FreeCardLog WHERE nick = '" + nick + "') AND mobile <> ''";
        totalcount1 = utils.ExecuteString(sql);

        sql = "SELECT SUM(Convert(decimal,totalprice)) FROM TCS_Trade WHERE nick = '" + nick + "' AND orderid IN (SELECT orderid FROM TCS_FreeCardLog WHERE nick = '" + nick + "') AND mobile <> ''";
        totalprice1 = utils.ExecuteString(sql);

        //催单有礼带来的
        sql = "SELECT COUNT(*) FROM TCS_Trade WHERE nick = '" + nick + "' AND orderid IN (SELECT orderid FROM TCS_MsgSend WHERE nick = '" + nick + "' AND typ = 'cui') AND mobile <> ''";
        totalcount2 = utils.ExecuteString(sql);

        sql = "SELECT SUM(Convert(decimal,totalprice)) FROM TCS_Trade WHERE nick = '" + nick + "' AND orderid IN (SELECT orderid FROM TCS_MsgSend WHERE nick = '" + nick + "' AND typ = 'cui') AND mobile <> ''";
        totalprice2 = utils.ExecuteString(sql);

        if (totalprice.Length == 0) totalprice = "0";
        if (totalprice1.Length == 0) totalprice1 = "0";
        if (totalprice2.Length == 0) totalprice2 = "0";

        totalprice = Math.Round(decimal.Parse(totalprice), 2).ToString("0.00");
        totalprice1 = Math.Round(decimal.Parse(totalprice1), 2).ToString("0.00");
        totalprice2 = Math.Round(decimal.Parse(totalprice2), 2).ToString("0.00");
    }
}