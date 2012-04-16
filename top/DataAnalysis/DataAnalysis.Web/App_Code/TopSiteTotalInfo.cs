using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CusServiceAchievements.DAL;
using Model;

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


    private string _SellAvg;
    /// <summary>
    /// 销售单价
    /// </summary>
    public string SellAvg
    {
        set { _SellAvg = value; }
        get
        {
            if (!string.IsNullOrEmpty(_SellAvg)) return _SellAvg;
            if (GoodsCount == 0 || SiteOrderCount == 0)
            {
                return "0";
            }

            return (SiteOrderPay / GoodsCount).ToString(".00");
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
    /// 钻展流量
    /// </summary>
    public int SiteZuanZhan
    {
        set;
        get;
    }

    public int FreeFlow
    {
        set { }
        get
        {
            return SitePVCount - ZhiTongFlow - SiteZuanZhan;
        }
    }


    private string _LostOrder;
    /// <summary>
    /// 丢单率
    /// </summary>
    public string LostOrder
    {
        set { _LostOrder = value; }
        get
        {
            if (!string.IsNullOrEmpty(_LostOrder)) return _LostOrder;
            if (AskOrder == 0) return "0";
            DateTime date = DateTime.Parse(SiteTotalDate.Substring(0, 4) + "-" + SiteTotalDate.Substring(4, 2) + "-" + SiteTotalDate.Substring(6));
            TalkRecodService trDal = new TalkRecodService();
            GoodsOrderService goDal = new GoodsOrderService();
            List<TaoBaoAPIHelper.GoodsOrderInfo> goodsOrderList = goDal.GetCustomerList(SiteNick, date, date.AddDays(1));

            IList<CustomerInfo> list = trDal.GetCustomerList(date, date.AddDays(1), SiteNick);

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    IList<TaoBaoAPIHelper.GoodsOrderInfo> thislist = goodsOrderList.Where(o => o.buyer_nick == list[i].CustomerNick).ToList();
                    if (thislist.Count > 0)
                    {
                        list[i].tid = thislist[0].tid;
                    }
                }
            }
            else return "0";
            return Math.Round(((decimal)(AskOrder - list.Where(o => !string.IsNullOrEmpty(o.tid)).ToList().Count) / AskOrder) * 100, 2).ToString() + "%";
        }
    }

    /// <summary>
    /// 询单数
    /// </summary>
    public int AskOrder
    {
        set;
        get;
        //{
        //    DateTime start = DateTime.Parse(SiteTotalDate.Substring(0, 4) + "-" + SiteTotalDate.Substring(4, 2) + "-" + SiteTotalDate.Substring(6));
        //    return new TalkRecodService().GetCustomerList(start, start.AddDays(1), SiteNick).Count;
        //}
    }

    public decimal CPC
    {
        set;
        get;
    }

    /// <summary>
    /// 单均价
    /// </summary>
    public string OneOrderPrice
    {
        get
        {
            if (SiteOrderCount == 0)
                return "0";
            return (SiteOrderPay / SiteOrderCount).ToString(".00");
        }
    }

    private string __OneCustomerPrice;
    /// <summary>
    /// 客单价
    /// </summary>
    public string OneCustomerPrice
    {
        set { __OneCustomerPrice = value; }
        get
        {
            if (!string.IsNullOrEmpty(__OneCustomerPrice)) return __OneCustomerPrice;
            if (SiteBuyCustomTotal == 0)
                return "0";
            return (SiteOrderPay / SiteBuyCustomTotal).ToString(".00");
        }
    }

    private string _CreateAVG;
    /// <summary>
    /// 平均转化率
    /// </summary>
    public string CreateAVG
    {
        set { _CreateAVG = value; }
        get
        {
            if (!string.IsNullOrEmpty(_CreateAVG)) return _CreateAVG;
            if (SiteUVCount == 0)
                return "0";
            return Math.Round((((decimal)SiteBuyCustomTotal / SiteUVCount) * 100), 2).ToString() + "%";
        }
    }


    private string _Refund;
    /// <summary>
    /// 退款率
    /// </summary>
    public string Refund
    {
        set { _Refund = value; }
        get
        {
            if (!string.IsNullOrEmpty(_Refund)) return _Refund;
            if (SiteOrderCount == 0) return "0";
            return Math.Round((((decimal)RefundOrderCount / SiteOrderCount) * 100), 2).ToString() + "%";
        }
    }

    /// <summary>
    /// 收藏量
    /// </summary>
    public int Collection
    {
        set { }
        get
        {
            return new ShopCollectionService().GetShopCollection(CacheCollection.GetNickSessionList().Where(o => o.Nick == SiteNick).ToArray()[0].ShopId, SiteTotalDate);
        }
    }

    /// <summary>
    /// 浏览第一的商品ID
    /// </summary>
    public TaoBaoAPIHelper.GoodsInfo SeeTop
    {
        get
        {
            DateTime start = DateTime.Parse(SiteTotalDate.Substring(0, 4) + "-" + SiteTotalDate.Substring(4, 2) + "-" + SiteTotalDate.Substring(6));
            IList<GoodsInfo> list = new TaoBaoGoodsServive().GetTopGoods(DataHelper.Encrypt(SiteNick), start, start.AddDays(1), 1, 1);
            if (list.Count == 0) return new TaoBaoAPIHelper.GoodsInfo();

            GoodsService goodsDal = new GoodsService();
            return goodsDal.GetGoodsInfo(list[0].num_iid, SiteNick);

            //return goodsName == "" ? "" : goodsName;
        }
    }

    /// <summary>
    /// 销售第一的商品ID
    /// </summary>
    public TaoBaoAPIHelper.GoodsInfo SellTop
    {
        get
        {
            DateTime start = DateTime.Parse(SiteTotalDate.Substring(0, 4) + "-" + SiteTotalDate.Substring(4, 2) + "-" + SiteTotalDate.Substring(6));
            IList<GoodsInfo> list = new TaoBaoGoodsServive().GetTopBuyGoods(SiteNick, start, start.AddDays(1), 1, 1);
            if (list.Count == 0) return new TaoBaoAPIHelper.GoodsInfo();

            GoodsService goodsDal = new GoodsService();
            return goodsDal.GetGoodsInfo(list[0].num_iid, SiteNick);

            //return goodsName == "" ? "" : goodsName;
        }
    }

    private string _BackSee;
    /// <summary>
    /// 浏览回头率
    /// </summary>
    public string BackSee
    {
        set { _BackSee = value; }
        get
        {
            if (!string.IsNullOrEmpty(_BackSee)) return _BackSee;
            if (SiteUVBack == 0)
                return "0";
            return Math.Round((((decimal)SiteUVBack / SiteUVCount) * 100), 2).ToString() + "%";
        }
    }

    private string _SeeDeepAVG;
    /// <summary>
    /// 平均访问深度
    /// </summary>
    public string SeeDeepAVG
    {
        set { _SeeDeepAVG = value; }
        get
        {
            if (!string.IsNullOrEmpty(_SeeDeepAVG)) return _SeeDeepAVG;
            if (SiteUVCount == 0)
                return "0";
            return ((decimal)SitePVCount / SiteUVCount).ToString(".00");
        }
    }

    public int RefundOrderCount { set; get; }

    public decimal RefundMoney { set; get; }

    #endregion

}
