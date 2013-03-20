using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class msgbuy : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.Cookies["nick"] != null)
                Nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value); //"nick"; 
            else
                Nick = Session["snick"].ToString();
            if (Nick == "")
            {
                Response.Redirect("http://fuwu.paipai.com/appstore/ui/my/app/appdetail.xhtml?appId=234454");
            }
        }
    }

    protected string Nick
    {
        set;
        get;
    }
}