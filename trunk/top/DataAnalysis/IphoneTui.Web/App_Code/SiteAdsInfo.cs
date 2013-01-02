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
/// 网站广告信息
/// </summary>
public class SiteAdsInfo
{
    public Guid Id { set; get; }

    public Guid SiteId { set; get; }

    public Guid AdsId { set; get; }

    public string AdsPosition { set; get; }

    public string AdsPic { set; get; }

    public int AdsCount { set; get; }

    public int AdsCalCount { set; get; }

    public int PositionCode { set; get; }

    public int Score { set; get; }
}

