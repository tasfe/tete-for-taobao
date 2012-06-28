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

public partial class getclick : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["id"]) && !string.IsNullOrEmpty(Request.QueryString["url"]))
            {
                string date = DateTime.Now.ToString("yyyyMMdd");
                Guid id = new Guid(Request.QueryString["id"]);
                int type = string.IsNullOrEmpty(Request.QueryString["type"]) ? 1 : 0;

                ClickInfo info = new ClickInfo();
                info.ClickType = type;
                info.ClickDate = date;
                info.UserAdsId = id;

                if (ClickService.SelectHasCount(info))
                    ClickService.UpdateClickInfo(info);
                else
                {
                    info.ClickCount = 1;
                    ClickService.InsertClickInfo(info);
                }

                Response.Redirect(Request.QueryString["url"]);
            }
        }
    }
}
