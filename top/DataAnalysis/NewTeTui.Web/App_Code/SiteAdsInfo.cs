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
/// 网站调用广告信息
/// </summary>
public class SiteAdsInfo
{
    public Guid Id { set; get; }

    public string SiteUrl { set; get; }

    public string AdsPosition { set; get; }

    public string AdsCode { set; get; }

    public SiteAdsType AdsType { set; get; }
}

/// <summary>
/// 广告类型
/// </summary>
public enum SiteAdsType
{
    /// <summary>
    /// 站内
    /// </summary>
    Inner = 0,

    /// <summary>
    /// 站外
    /// </summary>
    Out = 5,

    /// <summary>
    /// 淘宝
    /// </summary>
    TaoBao = 10
}
