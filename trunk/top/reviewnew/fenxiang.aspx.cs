using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;
using System.Net;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Data;

public partial class top_reviewnew_fenxiang : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        string session = cookie.getCookie("top_sessiongroupbuy");
        Rijndael_ encode = new Rijndael_("tetesoft");
        string nick = encode.Decrypt(taobaoNick);
        string sql = string.Empty;
        string giftMsg = "10";

        string act = utils.NewRequest("act", utils.RequestType.QueryString);

        if (act == "suc") {
            //每天最多赠送一次
            sql = "SELECT COUNT(*) FROM TCS_PayLog WHERE typ = '分享服务赠送10条短信' AND nick = '"+nick+"' AND DATEDIFF(d, enddate, GETDATE()) = 0";
            string count = utils.ExecuteString(sql);
            if (count == "0")
            {
                //插入充值记录并更新短信条数
                sql = "INSERT INTO TCS_PayLog (" +
                                "typ, " +
                                "enddate, " +
                                "nick, " +
                                "count " +
                            " ) VALUES ( " +
                                " '分享服务赠送10条短信', " +
                                " GETDATE(), " +
                                " '" + nick + "', " +
                                " '" + giftMsg + "' " +
                          ") ";
                utils.ExecuteNonQuery(sql);

                sql = "UPDATE TCS_ShopConfig SET total = total + " + giftMsg + " WHERE nick = '" + nick + "'";
                utils.ExecuteNonQuery(sql);

                Response.Write("<script>alert('分享成功，系统赠送您10条短信，每天都可以来分享获取一次免费短信哦！：）');window.location.href='msgaddlist.aspx';</script>");
                Response.End();
            }
            else
            {
                Response.Write("<script>alert('免费短信每天最多领取一次哦，您可以明天再来领取！：）'); history.go(-1);</script>");
                Response.End();
            }
        }
    }
}