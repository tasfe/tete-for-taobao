using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;

public partial class iphoneapi_api_container : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string session = utils.NewRequest("session", utils.RequestType.QueryString);
        string nick = utils.NewRequest("nick", utils.RequestType.QueryString);

        Cookie cookie = new Cookie();
        cookie.setCookie("top_session", session, 999999);
        cookie.setCookie("nick", nick, 999999);

        Response.Redirect("http://haoping.7fshop.com/top/indexios.html");
    }
}