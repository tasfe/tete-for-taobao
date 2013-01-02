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
/// 广告被点击信息
/// </summary>
public class ClickInfo
{
    /// <summary>
    /// 被点击的广告ID(UserAdsInfo中的ID)
    /// </summary>
    public Guid UserAdsId { set; get; }

    /// <summary>
    /// 被点击的次数
    /// </summary>
    public int ClickCount { set; get; }

    /// <summary>
    /// 具体哪一天yyyyMMdd
    /// </summary>
    public String ClickDate { set; get; }

    /// <summary>
    /// 来访类型1正常来访；0刷流量
    /// </summary>
    public int ClickType { set; get; }
}
