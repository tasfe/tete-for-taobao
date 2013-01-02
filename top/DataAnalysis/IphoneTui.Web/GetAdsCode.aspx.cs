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

public partial class GetAdsCode : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                try
                {
                    Guid gid = new Guid(Request.QueryString["id"]);
                    string adscode = new SiteAdsService().GetAdsCode(Request.QueryString["id"]);
                    Response.Write(adscode);
                    Response.End();
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
