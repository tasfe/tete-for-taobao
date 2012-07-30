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
/// 用户投放广告信息
/// </summary>
[Serializable]
public class UserAdsInfo
{
    public Guid Id { set; get; }

    public String AdsTitle { set; get; }

    public String AdsUrl { set; get; }

    public Guid AdsId { set; get; }

    /// <summary>
    /// 0：未投放；1：投放中;2:暂停投放
    /// </summary>
    public int UserAdsState { set; get; }

    public DateTime AdsShowStartTime { set; get; }

    public DateTime AdsShowFinishTime { set; get; }

    public String SellCateName { set; get; }

    public String AliWang { set; get; }

    /// <summary>
    /// 广告添加时间
    /// </summary>
    public DateTime AddTime { set; get; }

    /// <summary>
    /// 广告投放人
    /// </summary>
    public String Nick { set; get; }

    /// <summary>
    /// 分类级，主分类,子分类
    /// </summary>
    public String CateIds { set; get; }

    /// <summary>
    /// 使用的是哪种订购方式
    /// </summary>
    public Guid FeeId { set; get; }

    /// <summary>
    /// 广告图片
    /// </summary>
    public String AdsPic { set; get; }

    /// <summary>
    /// 价格,针对商品
    /// </summary>
    public decimal Price { set; get; }

    /// <summary>
    /// 是否是赠送的(0:非赠送，1:赠送的
    /// </summary>
    public int IsSend { set; get; }

    public override bool Equals(object obj)
    {
        UserAdsInfo info = obj as UserAdsInfo;
        if (info == null)
            return false;
        if (info.Id == this.Id)
            return true;
        return base.Equals(obj);
    }
}
