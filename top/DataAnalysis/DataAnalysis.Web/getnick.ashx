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
        //正式可不用
        HttpCookie cookietongji = new HttpCookie("istongji", "1");
        cookietongji.Expires = DateTime.Now.AddDays(15);
        context.Response.Cookies.Add(cookietongji);

        context.Response.Cookies.Add(cookie);
        context.Response.Cookies.Add(cooksession);
        if (!string.IsNullOrEmpty(context.Request.QueryString["istongji"]))
        {
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

                InsertGoodsOrder(now.AddDays(-7), now, session, nick);
            }
            else
            {
                if (!list[0].NickState)
                {
                    DateTime start = now.AddDays(-15);
                    if (now.AddDays(-15) < list[0].LastGetOrderTime)
                        start = list[0].LastGetOrderTime;

                    list[0].NickState = true;
                    list[0].JoinDate = now;
                    list[0].LastGetOrderTime = now;
                    new NickSessionService().UpdateSession(list[0]);

                    //二次订购
                    InsertGoodsOrder(start, now, session, nick);
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
 
    public bool IsReusable {
        get {
            return false;
        }
    }
}