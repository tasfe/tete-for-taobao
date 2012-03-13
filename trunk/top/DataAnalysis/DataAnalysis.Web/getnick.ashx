<%@ WebHandler Language="C#" Class="getnick" %>

using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;

public class getnick : IHttpHandler {

    public void ProcessRequest(HttpContext context)
    {
        if (string.IsNullOrEmpty(context.Request.QueryString["nick"]))
            return;
        if (string.IsNullOrEmpty(context.Request.QueryString["session"]))
            return;
        string nick = HttpUtility.UrlDecode(context.Request.QueryString["nick"]);
        string session = context.Request.QueryString["session"];
        HttpCookie cookie = new HttpCookie("nick", HttpUtility.UrlEncode(nick));
        HttpCookie cooksession = new HttpCookie("nicksession", session);
        cookie.Expires = DateTime.Now.AddDays(15);
        cooksession.Expires = DateTime.Now.AddDays(15);
        //cookie.Domain = ".test.7fshop.com";


        //nick == "上宫庄健客专卖店"免费测试
        if (CacheCollection.GetNickSessionList().Where(o => o.Nick == nick || nick == "上宫庄健客专卖店").ToList().Count > 0)
        {
            //正式可不用
            HttpCookie cookietongji = new HttpCookie("istongji", "1");
            cookietongji.Expires = DateTime.Now.AddDays(1);
            context.Response.Cookies.Add(cookietongji);
        }
        context.Response.Cookies.Add(cookie);
        context.Response.Cookies.Add(cooksession);
        if (!string.IsNullOrEmpty(context.Request.QueryString["istongji"]) || nick == "上宫庄健客专卖店")
        {
            HttpCookie cookietongji = new HttpCookie("istongji", "1");
            cookietongji.Expires = DateTime.Now.AddDays(1);
            context.Response.Cookies.Add(cookietongji);
            IList<TopNickSessionInfo> list = CacheCollection.GetNickSessionList().Where(o => o.Nick == nick).ToList();
            DateTime now = DateTime.Now;
            if (list.Count == 0)
            {
                TopNickSessionInfo info = new TopNickSessionInfo();
                info.Nick = nick;
                info.Session = session;
                info.NickState = true;
                info.JoinDate = now;
                info.LastGetOrderTime = now;
                new NickSessionService().InsertSerssion(info);
                CacheCollection.RemoveCacheByKey(CacheCollection.KEY_ALLNICKSESSIONINFO);

                InsertGoodsOrder(DateTime.Parse(now.AddDays(-7).ToShortDateString()), now, session, nick);
                //添加统计数据
                SiteTotalService taoDal = new SiteTotalService();
                for (DateTime i = DateTime.Parse(now.AddDays(-7).ToShortDateString()); i <= now; i = i.AddDays(1))
                {
                    UpdateSiteTotal(nick, i, taoDal);
                }
                
            }
            else
            {
                if (!list[0].NickState)
                {
                    DateTime start = DateTime.Parse(now.AddDays(-15).ToShortDateString());
                    if (now.AddDays(-15) < list[0].LastGetOrderTime)
                        start = list[0].LastGetOrderTime;

                    list[0].NickState = true;
                    list[0].JoinDate = now;
                    list[0].LastGetOrderTime = now;
                    new NickSessionService().UpdateSession(list[0]);

                    //二次订购
                    InsertGoodsOrder(start, now, session, nick);

                    //添加统计数据
                    SiteTotalService taoDal = new SiteTotalService();
                    for (DateTime i = DateTime.Parse(start.ToShortDateString()); i <= now; i = i.AddDays(1))
                    {
                        UpdateSiteTotal(nick, i, taoDal);
                    }
                }
            }
        }

        //context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");
    }

    private void InsertGoodsOrder(DateTime start, DateTime end, string session, string nick)
    {
        TaoBaoGoodsOrderService tbgo = new TaoBaoGoodsOrderService();
        //等待卖家发货,即:买家已付款
        InsertGoodsOrderByState(start, end, "WAIT_SELLER_SEND_GOODS", session, nick, tbgo);
        //等待买家确认收货,即:卖家已发货
        InsertGoodsOrderByState(start, end, "WAIT_BUYER_CONFIRM_GOODS", session, nick, tbgo);
        //买家已签收,货到付款专用
        InsertGoodsOrderByState(start, end, "TRADE_BUYER_SIGNED", session, nick, tbgo);
        //交易成功
        InsertGoodsOrderByState(start, end, "TRADE_FINISHED", session, nick, tbgo);
    }

    private void InsertGoodsOrderByState(DateTime start, DateTime end, string orderState, string session, string nick, TaoBaoGoodsOrderService tbgoDal)
    {
        IList<GoodsOrderInfo> goodsOrderList = TaoBaoAPI.GetGoodsOrderInfoList(start, end, session, orderState);
        if (goodsOrderList == null)
        {
            LogInfo.WriteLog("订购时获取订单错误", "参数错误");
            System.Threading.Thread.Sleep(10 * 60);
        }
        else
        {
            for (int i = 0; i < goodsOrderList.Count; i++)
            {
                goodsOrderList[i].UsePromotion = TaoBaoAPI.GetPromotion(session, goodsOrderList[i].tid, "店铺优惠券");
                goodsOrderList[i].PingInfo = TaoBaoAPI.GetPingjia(session, goodsOrderList[i].tid);
                if (goodsOrderList[i].PingInfo == null)
                {
                    goodsOrderList[i].PingInfo = new PingJiaInfo
                    {
                        content = "",
                        created = DateTime.Parse("1990-1-1"),
                        result = ""
                    };
                }
                tbgoDal.InsertTaoBaoGoodsOrder(goodsOrderList[i]);
                tbgoDal.InsertChildOrderInfo(goodsOrderList[i].orders, goodsOrderList[i].tid);
            }
        }
    }

    public void UpdateSiteTotal(string nick, DateTime now, SiteTotalService taoDal)
    {

        //所有有订单的用户
        TopSiteTotalInfo stinfo = taoDal.GetOrderTotalPay(DateTime.Parse(now.ToShortDateString()), DateTime.Parse(now.AddDays(1).ToShortDateString()), nick);
        //所有表名(添加过统计代码到网页的)
        List<string> tableList = taoDal.GetTableName();
        string tablename = GetTableName(nick);
        List<string> mytablelist = tableList.Where(o => o == tablename).ToList();

        TopSiteTotalInfo addup = new TopSiteTotalInfo();
        addup.SiteNick = nick;
        addup.SiteTotalDate = now.ToString("yyyyMMdd");
        //有添加统计代码
        if (mytablelist.Count > 0)
        {
            //有订单
            if (stinfo != null)
            {
                addup.SiteOrderCount = stinfo.SiteOrderCount;
                addup.SiteOrderPay = stinfo.SiteOrderPay;
                addup.PostFee = stinfo.PostFee;
                addup.SiteBuyCustomTotal = stinfo.SiteBuyCustomTotal;
                addup.SiteSecondBuy = taoDal.GetSecondBuyTotal(DateTime.Parse(now.ToShortDateString()), DateTime.Parse(now.AddDays(1).ToShortDateString()), nick);
            }
            TopSiteTotalInfo puvinfo = taoDal.GetPvUvTotal(DateTime.Parse(now.ToShortDateString()), DateTime.Parse(now.AddDays(1).ToShortDateString()), tablename);
            addup.SiteUVCount = puvinfo.SiteUVCount;
            addup.SitePVCount = puvinfo.SitePVCount;

            addup.SiteUVBack = taoDal.GetBackTotal(DateTime.Parse(now.ToShortDateString()), DateTime.Parse(now.AddDays(1).ToShortDateString()), tablename);
        }
        else
        {
            //有订单
            if (stinfo != null)
            {
                addup.SiteOrderCount = stinfo.SiteOrderCount;
                addup.SiteOrderPay = stinfo.SiteOrderPay;
                addup.PostFee = stinfo.PostFee;
                addup.SiteBuyCustomTotal = stinfo.SiteBuyCustomTotal;
                addup.SiteSecondBuy = taoDal.GetSecondBuyTotal(DateTime.Parse(now.ToShortDateString()), DateTime.Parse(now.AddDays(1).ToShortDateString()), nick);
            }
        }

        IList<string> tidList = taoDal.GetOrderIds(DateTime.Parse(now.ToShortDateString()), DateTime.Parse(now.AddDays(1).ToShortDateString()), nick);
        if (tidList.Count > 0)
            addup.GoodsCount = taoDal.GetGoodsCount(tidList);

        taoDal.AddOrUp(addup);

    }

    private string GetTableName(string nick)
    {
        return "TopVisitInfo_" + DataHelper.Encrypt(nick);
    }
    
    public bool IsReusable {
        get {
            return false;
        }
    }
}