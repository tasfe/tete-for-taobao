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

public partial class visitor_container : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                HttpCookie cookie = new HttpCookie("nick", Request.QueryString["id"]);
                cookie.Expires = DateTime.Now.AddDays(1);

                Response.Cookies.Add(cookie);

                Response.Redirect("index.html");
            }
        }
    }
}
