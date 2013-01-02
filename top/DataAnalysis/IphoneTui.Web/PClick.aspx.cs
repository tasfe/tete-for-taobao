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

public partial class PClick : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            System.Collections.Specialized.NameValueCollection s = Request.QueryString;

            if (s.Count != 1)
                return;
            string query = HttpUtility.UrlDecode(s.ToString());

            if (query.Contains("id=") && query.Contains("url="))
            {
                string date = DateTime.Now.ToString("yyyyMMdd");
                Guid id;
                try
                {
                    id = new Guid(query.Substring(query.IndexOf("=") + 1, query.IndexOf("&") - 3));
                }
                catch
                {
                    return;
                }
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

                string url = query.Substring(query.IndexOf("url=") + 4);
                url = url.Contains("?") ? url + "&" : url + "?";

                Response.Redirect(url);
            }
        }
    }
}
