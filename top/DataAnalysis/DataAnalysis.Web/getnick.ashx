﻿<%@ WebHandler Language="C#" Class="getnick" %>

using System;
using System.Web;
using System.Collections.Generic;

public class getnick : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        if(string.IsNullOrEmpty(context.Request.QueryString["nick"]))
            return;
        if(string.IsNullOrEmpty(context.Request.QueryString["session"]))
            return ;
        string nick = HttpUtility.UrlDecode(context.Request.QueryString["nick"]);
        string session = context.Request.QueryString["session"];
        if (context.Session == null || context.Session["nick"] == null)
        {
            HttpCookie cookie = new HttpCookie("nick", nick);
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
            if (string.IsNullOrEmpty(context.Request.QueryString["istongji"]))
            {
                DateTime now = DateTime.Now;
                TopNickSessionInfo info = new TopNickSessionInfo();
                info.Nick = nick;
                info.Session = session;
                info.NickState = true;
                info.JoinDate = now;
                info.LastGetOrderTime = now;
                new NickSessionService().AddSession(info);

                InsertGoodsOrder(TaoBaoAPI.GetGoodsOrderInfoList(now.AddDays(-7), now, session, "TRADE_FINISHED"), session, nick);
            }
        }
        //context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");
    }

    public void InsertGoodsOrder(IList<GoodsOrderInfo> goodsOrderList,string session,string nick)
    {
        if (goodsOrderList == null)
        {
            LogInfo.WriteLog("订购时获取订单错误","参数错误");
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
                        nick = "",
                        content = "",
                        created = DateTime.Parse("1990-1-1"),
                        result = ""
                    };
                }
                new TaoBaoGoodsOrderService().InsertTaoBaoGoodsOrder(goodsOrderList[i]);
                new TaoBaoGoodsOrderService().InsertChildOrderInfo(goodsOrderList[i].orders, goodsOrderList[i].tid);
            }
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}