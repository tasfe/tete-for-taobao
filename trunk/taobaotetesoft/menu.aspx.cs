using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;

public partial class menu : System.Web.UI.Page
{
    public string nick = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Common.Rijndael_ encode = new Rijndael_("tetetete");
        Common.Cookie cookie = new Common.Cookie();

        if (cookie.getCookie("nick") == "")
        {
            Response.Redirect("default.aspx");
            return;
        }
        
        nick = encode.Decrypt(cookie.getCookie("nick"));
    }
}