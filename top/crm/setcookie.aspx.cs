using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;

public partial class top_crm_setcookie : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string nick = utils.NewRequest("nick", utils.RequestType.QueryString);
        string top_session = utils.NewRequest("session", utils.RequestType.QueryString);

        Common.Cookie cookie = new Common.Cookie();
        cookie.setCookie("top_session", top_session, 999999);
        cookie.setCookie("nick", nick, 999999);
        cookie.setCookie("iscrm", "1", 999999);

        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Decrypt(nick);

        //写入数据库
        string sql = "SELECT COUNT(*) FROM TCS_CrmConfig WHERE nick = '" + nick + "'";
        string count = utils.ExecuteString(sql);
        if (count == "0")
        {
            sql = "INSERT INTO TCS_CrmConfig (nick, session, updatedate) VALUES ('" + nick + "','" + top_session + "','" + DateTime.Now.AddMonths(-3).ToString() + "')";
            Response.Write(sql);
            utils.ExecuteNonQuery(sql);
        }
    }
}