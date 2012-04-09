using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class SetCookie : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Btn_AddCookie_Click(object sender, EventArgs e)
    {
        string nick = Tb_cookie.Text;
        HttpCookie cookie = new HttpCookie("nick", nick);
        cookie.Expires = DateTime.Now.AddDays(1);
        Response.Cookies.Add(cookie);
        Response.Redirect("index.html");
    }
}
