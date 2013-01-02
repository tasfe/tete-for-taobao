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
    /// 广告类型（1：瀑布流，5：混合类型)
    /// </summary>
    public int AdsType { set; get; }

    /// <summary>
    /// 瀑布流广告位个数
    /// </summary>
    public int AdsPubuCount { set; get; }

    /// <summary>
    /// 大广告位个数
    /// </summary>
    public int AdsPhoto { set; get; }

    /// <summary>
    /// 展示几天
    /// </summary>
    public int ShowDays { set; get; }

    /// <summary>
    /// 相应分数
    /// </summary>
    public int Score { set; get; }
}
