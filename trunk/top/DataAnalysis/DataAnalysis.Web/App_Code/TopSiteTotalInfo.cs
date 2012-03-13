using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class TopSiteTotalInfo
{
    public string SiteNick { set; get; }

    public string SiteTotalDate { set; get; }

    public int SitePVCount { set; get; }

    public int SiteUVCount { set; get; }

    /// <summary>
    /// 订单总数
    /// </summary>
    public int SiteOrderCount { set; get; }

    /// <summary>
    /// 这里指当天下单的用户总数
    /// </summary>
    public int SiteBuyCustomTotal { set; get; }

    /// <summary>
    /// 二次浏览
    /// </summary>
    public int SiteUVBack { set; get; }

    public decimal SiteOrderPay { set; get; }

    /// <summary>
    /// 邮费
    /// </summary>
    public decimal PostFee { set; get; }

    /// <summary>
    /// 销售商品总数量
    /// </summary>
    public int GoodsCount { set; get; }

    /// <summary>
    /// 二次购买
    /// </summary>
    public int SiteSecondBuy { set; get; }

    #region

    /// <summary>
    /// 销售单价
    /// </summary>
    public decimal SellAvg
    {
        get
        {
            if (GoodsCount == 0 || SiteOrderCount == 0)
            {
                return 0;
            }

            return (SiteOrderPay / SiteOrderCount) / (SiteOrderPay / GoodsCount);
        }
    }

    /// <summary>
    /// 直通车流量
    /// </summary>
    public int ZhiTongFlow
    {
        set;
        get;
    }

    /// <summary>
    /// 丢单数
    /// </summary>
    public int LostOrder { set; get; }

    /// <summary>
    /// 询单数
    /// </summary>
    public int AskOrder { set; get; }

    public decimal CPC
    {
        set;
        get;
    }

    /// <summary>
    /// 单均价
    /// </summary>
    public decimal OneOrderPrice
    {
        get
        {
            if (SiteOrderCount == 0)
                return 0;
            return SiteOrderPay / SiteOrderCount;
        }
    }

    /// <summary>
    /// 客单价
    /// </summary>
    public decimal OneCustomerPrice
    {
        get
        {
            if (SiteUVCount == 0)
                return 0;
            return SiteOrderPay / SiteUVCount;
        }
    }

    /// <summary>
    /// 平均转化率
    /// </summary>
    public decimal CreateAVG
    {
        get
        {
            return 0;
        }
    }

    /// <summary>
    /// 退款率
    /// </summary>
    public decimal Refund
    {
        get { return 0; }
    }

    /// <summary>
    /// 收藏量
    /// </summary>
    public int Collection
    {
        set;
        get;
    }

    /// <summary>
    /// 浏览第一的商品ID
    /// </summary>
    public string SeeTop
    {
        get
        {
            return "";
        }
    }

    /// <summary>
    /// 销售第一的商品ID
    /// </summary>
    public string SellTop
    {
        get
        {
            return "";
        }
    }

    /// <summary>
    /// 浏览回头率
    /// </summary>
    public decimal BackSee
    {
        get
        {
            if (SiteUVBack == 0)
                return 0;
            return SiteUVCount / SiteUVBack;
        }
    }

    /// <summary>
    /// 平均访问深度
    /// </summary>
    public decimal SeeDeepAVG
    {
        get
        {
            if (SiteUVCount == 0)
                return 0;
            return (decimal)SitePVCount / SiteUVCount;
        }
    }

    #endregion

}
