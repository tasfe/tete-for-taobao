using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

public partial class MasterPage : System.Web.UI.MasterPage
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
                    Response.Redirect("http://fuwu.taobao.com/ser/my_service.htm");
                }
                else
                {
                    nick = Session["snick"].ToString();
                }
            }
            if (nick != "")
            {
                Lbl_UserName.Text = nick;
            }
            else
            {
                Response.Redirect("http://fuwu.taobao.com/ser/my_service.htm");
            }
        }
    }
}
