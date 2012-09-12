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

public partial class visitor_addjs : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string id = Request.QueryString["id"];
            if (string.IsNullOrEmpty(id))
            {
                Response.Write("请传入id");
                Response.End();
            }
            else
            {
                //string url = "qijia.7fshop.com/getvisit.aspx";
                //string js = "<script type='text/javascript'>var lasturl = document.referrer;jQuery.post('" + url + "?lasturl='+lasturl));</script>";

                string url = "getvisit.aspx?id=" + id;
                string js = "var lasturl = document.referrer;jQuery.post('" + url + "&lasturl='+lasturl);";

                Response.Write(js);
                Response.End();
            }
        }
    }
}
