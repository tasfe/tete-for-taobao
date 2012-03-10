using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;

public partial class top_reviewnew_setcookie : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string nick = utils.NewRequest("nick", utils.RequestType.QueryString);
        string t = utils.NewRequest("t", utils.RequestType.QueryString);

        Common.Cookie cookie = new Common.Cookie();
        cookie.setCookie("nick", nick, 999999);

        if (t != "")
        {
            Response.Redirect("index.html?t=" + t);
        }
        else
        {
            Response.Redirect("index.html");
        }
    }
}