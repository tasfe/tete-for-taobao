<%@ WebHandler Language="C#" Class="getnick" %>

using System;
using System.Web;
using System.Collections.Generic;

public class getnick : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        if(string.IsNullOrEmpty(context.Request.QueryString["nick"]))
            return;
        if(string.IsNullOrEmpty(context.Request.QueryString["session"]))
            return ;
        string nick = context.Request.QueryString["nick"];
        string session = context.Request.QueryString["session"];
        if (context.Session["nick"] == null)
        {
            context.Session["nick"] = nick;
            context.Session["session"] = session;

            DateTime now = DateTime.Now;
            TopNickSessionInfo info = new TopNickSessionInfo();
            info.Nick = nick;
            info.Session = session;
            info.NickState = true;
            info.JoinDate = now;
            info.LastGetOrderTime = now;
            new NickSessionService().AddSession(info);
            TaoBaoAPI.GetGoodsOrderInfoList(now.AddDays(-7), now, session, "TRADE_FINISHED");
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