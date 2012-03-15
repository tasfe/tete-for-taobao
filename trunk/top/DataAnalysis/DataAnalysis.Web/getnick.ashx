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
                info.ServiceId = Enum.TopTaoBaoService.Temporary;
                info.ShopId = TaoBaoAPI.GetShopInfo(nick);
                new NickSessionService().InsertSerssionNew(info);
                CacheCollection.RemoveCacheByKey(CacheCollection.KEY_ALLNICKSESSIONINFO);

                DataHelper.InsertGoodsOrder(DateTime.Parse(now.AddDays(-7).ToShortDateString()), now, session, nick);
                //添加统计数据
                SiteTotalService taoDal = new SiteTotalService();
                for (DateTime i = DateTime.Parse(now.AddDays(-7).ToShortDateString()); i <= now; i = i.AddDays(1))
                {
                    DataHelper.UpdateSiteTotal(nick, i, taoDal);
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
                    DataHelper.InsertGoodsOrder(start, now, session, nick);

                    //添加统计数据
                    SiteTotalService taoDal = new SiteTotalService();
                    for (DateTime i = DateTime.Parse(start.ToShortDateString()); i <= now; i = i.AddDays(1))
                    {
                        DataHelper.UpdateSiteTotal(nick, i, taoDal);
                    }
                }
            }
        }

        //context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");
    }
    
    public bool IsReusable {
        get {
            return false;
        }
    }
}