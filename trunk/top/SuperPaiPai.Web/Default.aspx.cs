using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Cookie cookie = new Cookie();
        cookie.setCookie("visitorId", this.txtNick.Text, 999999);
        cookie.setCookie("nick", this.txtNick.Text, 999999);
    }
}
