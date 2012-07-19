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
/// 费用类型
/// </summary>
public class FeeInfo
{
    public Guid FeeId { set; get; }

    public decimal Fee { set; get; }

    /// <summary>
    /// 网站个数
    /// </summary>
    public int SiteCount { set; get; }

    /// <summary>
    /// 广告类型（1：不限个数，5：只有一个(VIP版本)
    /// </summary>
    public int AdsType { set; get; }

    /// <summary>
    /// 广告位个数
    /// </summary>
    public int AdsCount { set; get; }

    /// <summary>
    /// 展示几天
    /// </summary>
    public int ShowDays { set; get; }
}
