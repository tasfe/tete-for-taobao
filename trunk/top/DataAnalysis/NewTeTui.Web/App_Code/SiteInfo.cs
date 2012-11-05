using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// 网站信息
/// </summary>
public class SiteInfo
{
	public SiteInfo()
	{
        SiteId = Guid.NewGuid();
	}

    public SiteInfo(String sitename, String siteurl)
    {
        SiteName = sitename;
        SiteUrl = siteurl;
        SiteId = Guid.NewGuid();
    }

    public Guid SiteId { set; get; }

    public String SiteName { set; get; }

    public String SiteUrl { set; get; }
}
