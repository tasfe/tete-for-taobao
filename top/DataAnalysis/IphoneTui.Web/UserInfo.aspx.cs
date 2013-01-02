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

public partial class UserInfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string nick = "";
            if (Request.Cookies["nick"] != null)
                nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value); //"nick"; 
            else
            {
                if (Session["snick"] == null)
                {
                    Response.Write("0");
                    Response.End();
                }
                else
                {
                    nick = Session["snick"].ToString();
                }
            }
            if (nick != "")
            {
                Response.Write(nick);
                Response.End();
            }
        }
    }
}
