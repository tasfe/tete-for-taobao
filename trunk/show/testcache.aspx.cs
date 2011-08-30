using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.EnterpriseLibrary.Caching;

public partial class show_testcache : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string id = Request.QueryString["id"].ToString();

        CacheManager testcaching1 = CacheFactory.GetCacheManager();
        if (testcaching1.Contains("cache_1_" + id))
        {
            Response.Write(testcaching1.GetData("cache_1_" + id));
            Response.End();
            return;
        }
    }
}
