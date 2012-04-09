using System;
using System.Web;
using System.Runtime.InteropServices;
using System.Web.Security;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using Model;
using CusServiceAchievements.DAL;
using TaoBaoAPIHelper;
using LogHelper;

public class DataHelper
{
    private HttpContext context;

    public DataHelper(HttpContext myContext)
    {
        if (context == null)
        {
            context = myContext;
        }
    }

    public string GetUrl()
    {
        if (null == context.Request.UrlReferrer)
            return "";
        return context.Request.UrlReferrer.ToString();
    }

    public string GetIPAddress()
    {
        return context.Request.ServerVariables["REMOTE_ADDR"];
    }

    public DateTime GetVisitTime()
    {
        return DateTime.Now;
    }

    public string GetUserAgent()
    {
        return GetOSNameByUserAgent(context.Request.UserAgent);
    }

    //获取浏览器的类型及版本号
    public string GetBrower()
    {
        return context.Request.Browser.Browser + context.Request.Browser.Version;
    }

    //获取用户操作系统的语言
    public string GetOSLanguage()
    {
        return context.Request.UserLanguages[0];
    }

    //获取OS：
    private string GetOSNameByUserAgent(string userAgent)
    {
        string osVersion = "未知";
        if (userAgent.Contains("NT 6.1"))
        {
            osVersion = "Windows  7";
        }
        else if (userAgent.Contains("NT 6.0"))
        {
            osVersion = "Windows Vista/Server 2008";
        }
        else if (userAgent.Contains("NT 5.2"))
        {
            osVersion = "Windows Server 2003";
        }
        else if (userAgent.Contains("NT 5.1"))
        {
            osVersion = "Windows XP";
        }
        else if (userAgent.Contains("NT 5"))
        {
            osVersion = "Windows 2000";
        }
        else if (userAgent.Contains("Mac"))
        {
            osVersion = "Mac";
        }
        else if (userAgent.Contains("Unix"))
        {
            osVersion = "UNIX";
        }
        else if (userAgent.Contains("Linux"))
        {
            osVersion = "Linux";
        }
        else if (userAgent.Contains("SunOS"))
        {
            osVersion = "SunOS";
        }

        return osVersion;
    }

    /// <summary>
    /// MD5加密
    /// </summary>
    public static string Encrypt(string value)
    {
        return Encrypt(value, "00000000000000000000000000000000");
    }

    /// <summary>
    /// MD5加密
    /// </summary>
    public static string Encrypt(string value, string defaultValue)
    {
        if (value == null)
        {
            return defaultValue;
        }
        var md5 = FormsAuthentication.HashPasswordForStoringInConfigFile(value, "MD5");
        return md5 != null ? md5.ToLower() : defaultValue;
    }

    ///<summary>
    /// 生成MD5摘要加密，可以对加密结果进行截取
    ///</summary>
    ///<param name="value">源字符串</param>
    ///<param name="start">截取开始位置</param>
    ///<param name="count">截取长度</param>
    public static string EncryptSubstring(string value, int start, int length)
    {
        return Encrypt(value).Substring(start, length);
    }

    /// <summary>
    /// 指定从什么时间开始
    /// </summary>
    /// <param name="start">开始时间</param>
    /// <param name="days">间隔天数</param>
    /// <returns>返回含2个时间的数组，第一个开始，第二个结束</returns>
    public static DateTime[] GetDateTime(DateTime start, int days)
    {
        DateTime end = start.AddDays(days);
        DateTime rstart = new DateTime(start.Year, start.Month, start.Day);
        DateTime rend = new DateTime(end.Year, end.Month, end.Day);
        return new[] { rstart, rend };
    }

    public static string GetAppSetings(string key)
    {
        return ConfigurationSettings.AppSettings[key];
    }

    #region 获取局域网访问者的MAC

    [DllImport("Iphlpapi.dll")]
    private static extern int SendARP(Int32 dest, Int32 host, ref Int64 mac, ref Int32 length);
    [DllImport("Ws2_32.dll")]
    private static extern Int32 inet_addr(string ip);

    /// <summary>
    /// 应该可获取局域网访问者的MAC(未测)
    /// </summary>
    /// <returns></returns>
    public string GetClientMAC()
    {
        try
        {
            string userip = GetIPAddress();
            Int32 ldest = inet_addr(userip); //目的地的ip 
            Int32 lhost = inet_addr("223.4.6.115"); //本地服务器的ip 
            Int64 macinfo = new Int64();
            Int32 len = 6;
            int res = SendARP(ldest, 0, ref macinfo, ref len);
            string mac_src = macinfo.ToString("X");
            if (mac_src == "0")
            {
                if (userip == "127.0.0.1")
                    return "Localhost!";
                else
                    return "null";
            }
            while (mac_src.Length < 12)
            {
                mac_src = mac_src.Insert(0, "0");
            }
            string mac_dest = "";
            for (int i = 0; i < 11; i++)
            {
                if (0 == (i % 2))
                {
                    if (i == 10)
                    {
                        mac_dest = mac_dest.Insert(0, mac_src.Substring(i, 2));
                    }
                    else
                    {
                        mac_dest = "-" + mac_dest.Insert(0, mac_src.Substring(i, 2));
                    }
                }
            }
            return mac_dest;
        }
        catch (Exception err)
        {
            LogInfo.Add("获取mac", err.Message);
            return err.Message;
        }
    }

    #endregion

    public static void GetParam(IList<TijianParamInfo> list)
    {
        list.Add(new TijianParamInfo { ParamName = "客户浏览比率", ParamValue = 0.33M });
        list.Add(new TijianParamInfo { ParamName = "销售关联度", ParamValue = 1.5M });
        list.Add(new TijianParamInfo { ParamName = "浏览转换率", ParamValue = 0.01M });
        list.Add(new TijianParamInfo { ParamName = "浏览回头率", ParamValue = 0.2M });
        list.Add(new TijianParamInfo { ParamName = "二次购买率", ParamValue = 0.01M });
        list.Add(new TijianParamInfo { ParamName = "页面访问深度", ParamValue = 3 });
        list.Add(new TijianParamInfo { ParamName = "爆款商品购买率", ParamValue = 0.3M });
    }

    #region 用户订购后立即获取能获取到的数据

    public static void InsertGoodsOrder(DateTime start, DateTime end, string session, string nick)
    {
        TaoBaoGoodsOrderService tbgo = new TaoBaoGoodsOrderService();

        List<string> allOrderTid = tbgo.GetAllOrderId(start, end, nick);

        //等待卖家发货,即:买家已付款
        InsertGoodsOrderByState(start, end, "WAIT_SELLER_SEND_GOODS", session, nick, tbgo, allOrderTid);
        //等待买家确认收货,即:卖家已发货
        InsertGoodsOrderByState(start, end, "WAIT_BUYER_CONFIRM_GOODS", session, nick, tbgo, allOrderTid);
        //买家已签收,货到付款专用
        InsertGoodsOrderByState(start, end, "TRADE_BUYER_SIGNED", session, nick, tbgo, allOrderTid);
        //交易成功
        InsertGoodsOrderByState(start, end, "TRADE_FINISHED", session, nick, tbgo, allOrderTid);
    }

    private static void InsertGoodsOrderByState(DateTime start, DateTime end, string orderState, string session, string nick, TaoBaoGoodsOrderService tbgoDal, List<string> tids)
    {
        IList<GoodsOrderInfo> goodsOrderList = TaoBaoAPI.GetGoodsOrderInfoList(nick, start, end, session, orderState);
        if (goodsOrderList == null)
        {
            LogInfo.WriteLog("订购时获取订单错误", "参数错误");
        }
        else
        {
            for (int i = 0; i < goodsOrderList.Count; i++)
            {
                //判断是否已经添加
                if (tids.Contains(goodsOrderList[i].tid))
                    continue;

                goodsOrderList[i].UsePromotion = TaoBaoAPI.GetPromotion(nick, session, goodsOrderList[i].tid, "店铺优惠券");
                goodsOrderList[i].PingInfo = TaoBaoAPI.GetPingjia(nick, session, goodsOrderList[i].tid);
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
                //添加进集合
                tids.Add(goodsOrderList[i].tid);
            }
        }
    }

    public static void UpdateSiteTotal(string nick, string session, DateTime now, SiteTotalService taoDal)
    {
        List<RefundInfo> refundList = (List<RefundInfo>)TaoBaoAPI.GetRefundInfoList(now.AddDays(-2), now, nick, session, "SUCCESS");
        //所有有订单的用户
        TopSiteTotalInfo stinfo = taoDal.GetOrderTotalPay(DateTime.Parse(now.ToShortDateString()), DateTime.Parse(now.AddDays(1).ToShortDateString()), nick);
        //所有表名(添加过统计代码到网页的)
        //List<string> tableList = taoDal.GetTableName();
        //string tablename = GetTableName(nick);
        List<string> mytablelist = new List<string>();//tableList.Where(o => o == tablename).ToList();

        TopSiteTotalInfo addup = new TopSiteTotalInfo();
        addup.SiteNick = nick;
        addup.SiteTotalDate = now.ToString("yyyyMMdd");
        //给询单数赋值
        addup.AskOrder = new TalkRecodService().GetCustomerList(DateTime.Parse(now.ToShortDateString()), DateTime.Parse(now.AddDays(1).ToShortDateString()), nick).Count;

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
            //TopSiteTotalInfo puvinfo = taoDal.GetPvUvTotal(DateTime.Parse(now.ToShortDateString()), DateTime.Parse(now.AddDays(1).ToShortDateString()), tablename);
            addup.SiteUVCount = 0;//puvinfo.SiteUVCount;
            addup.SitePVCount = 0;// puvinfo.SitePVCount;
            addup.ZhiTongFlow = 0;// taoDal.GetZhiTongTotal(now, now.AddDays(1), tablename);
            addup.SiteZuanZhan = 0;// taoDal.GetZuanZhanTotal(now, now.AddDays(1), tablename);

            addup.SiteUVBack = 0;// taoDal.GetBackTotal(DateTime.Parse(now.ToShortDateString()), DateTime.Parse(now.AddDays(1).ToShortDateString()), tablename);
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

        //退款情况
        List<RefundInfo> trefundList = refundList.Where(o => o.modified.ToShortDateString() == now.ToShortDateString()).ToList();
        if (trefundList.Count > 0)
        {
            int rorderCount = 0;
            decimal rpay = 0;
            foreach (RefundInfo refund in trefundList)
            {
                if (!tidList.Contains(refund.tid)) continue;
                rorderCount++;
                rpay += (refund.total_fee - refund.payment);
            }
            addup.RefundOrderCount = rorderCount;
            addup.RefundMoney = rpay;
        }

        taoDal.AddOrUp(addup);

    }

    public static DateTime GetTalkrContent(string nick, string session, DateTime now)
    {
        TalkRecodService trDal = new TalkRecodService();
        SubUserService userDal = new SubUserService();

        DateTime start = DateTime.Parse(now.AddDays(-7).ToShortDateString());
        if (TalkRecodService.CheckTable(DataHelper.Encrypt(nick)))
        {
            DateTime max = trDal.GetMaxTime(nick);
            if (start < max)
                start = max;
        }
        else
            trDal.CreateTable(DataHelper.Encrypt(nick));

        List<string> childNicks = new List<string>();
        IList<SubUserInfo> userList = TaoBaoAPIHelper.TaoBaoAPI.GetChildNick(nick, session);
        List<SubUserInfo> hasuserList = userDal.GetAllChildNick(nick);

        foreach (SubUserInfo uinfo in userList)
        {
            childNicks.Add(uinfo.nick);
            if (hasuserList.Where(o => o.nick == uinfo.nick).ToList().Count == 0)
                userDal.InsertSubUserInfo(uinfo);
        }

        List<TalkContent> allcontent = trDal.GetAllContent(now.AddHours(-16), now, nick);

        foreach (string fromNick in childNicks)
        {
            List<TalkObj> objList = TaoBaoAPIHelper.TaoBaoAPI.GetTalkObjList(fromNick.Replace("cntaobao", ""), session, start, now);
            foreach (TalkObj obj in objList)
            {
                List<TalkContent> contents = TaoBaoAPIHelper.TaoBaoAPI.GetTalkContentNow(session, fromNick.Replace("cntaobao", ""), obj.uid.Replace("cntaobao", ""), start, now);

                for (int i = 0; i < contents.Count; i++)
                {
                    contents[i].FromNick = fromNick.Replace("cntaobao", "");
                    contents[i].ToNick = obj.uid.Replace("cntaobao", "");

                    if (allcontent.Contains(contents[i]))
                    {
                        continue;
                    }
                    trDal.InsertContent(contents[i], nick);
                }
            }
        }

        //返回获取聊天记录的开始时间
        return start;
    }

    public static void GetKfjxTotal(string nick, DateTime start, DateTime now)
    {
        TalkRecodService trDal = new TalkRecodService();
        GoodsService goodsDal = new GoodsService();
        GoodsOrderService goDal = new GoodsOrderService();
        TopKefuTotalService kfDal = new TopKefuTotalService();
        //所有商品
        List<TaoBaoAPIHelper.GoodsInfo> goodsNickList = goodsDal.GetAllGoods(nick);

        for (DateTime h = start; h < now; h = h.AddDays(1))
        {

            //所有订单
            List<TaoBaoAPIHelper.GoodsOrderInfo> orderList = goDal.GetGoodsOrderList(nick, h, h.AddDays(1));

            //得到回复次数和接待人数
            List<CustomerInfo> cuslist = trDal.GetReceiveList(nick, h, h.AddDays(1));

            //得到接待人和购买者信息
            IList<CustomerInfo> cusBuyList = trDal.GetCustomerList(h, h.AddDays(1), nick);

            //得到客服未回复的客户数量
            List<TopKefuTotalInfo> untalkList = trDal.GetUnTalkCustomerList(nick, h, h.AddDays(1));

            for (int i = 0; i < cusBuyList.Count; i++)
            {
                IList<TaoBaoAPIHelper.GoodsOrderInfo> thislist = orderList.Where(o => o.buyer_nick == cusBuyList[i].CustomerNick).ToList();
                if (thislist.Count > 0)
                {
                    cusBuyList[i].tid = thislist[0].tid;
                }
            }
            //得到成功下单
            List<CustomerInfo> orderCusList = cusBuyList.Where(o => !string.IsNullOrEmpty(o.tid)).ToList();
            foreach (CustomerInfo fromInfo in cuslist)
            {
                TopKefuTotalInfo kefutotalInfo = new TopKefuTotalInfo();
                kefutotalInfo.CustomerCount = int.Parse(fromInfo.CustomerNick);
                kefutotalInfo.ReceiveCount = fromInfo.TalkCount;
                kefutotalInfo.Nick = fromInfo.FromNick;
                kefutotalInfo.NickDate = h.ToString("yyyyMMdd");
                //赋值未回复数量
                if (untalkList.Where(o => o.Nick == fromInfo.FromNick).ToList().Count > 0)
                    kefutotalInfo.UnTalkCustomerCount = untalkList.Where(o => o.Nick == fromInfo.FromNick).ToList()[0].UnTalkCustomerCount;

                List<CustomerInfo> mylist = orderCusList.Where(o => o.FromNick == fromInfo.FromNick).ToList();
                if (mylist.Count > 0)
                {
                    List<string> tids = new List<string>();
                    foreach (CustomerInfo inf in mylist)
                        tids.Add(inf.tid);

                    List<TaoBaoAPIHelper.GoodsOrderInfo> myolist = orderList.Where(o => tids.Contains(o.tid)).ToList();

                    kefutotalInfo.OrderCount = mylist.Count;

                    kefutotalInfo.PostFee = myolist.Sum(o => o.post_fee);
                    kefutotalInfo.Payment = myolist.Sum(o => o.payment);

                    List<TaoBaoAPIHelper.GoodsInfo> goodsList = goDal.GetGoodsCount(tids);
                    kefutotalInfo.GoodsCount = goodsList.Sum(o => o.Count);
                    decimal goodsPriceTotal = 0;
                    foreach (TaoBaoAPIHelper.GoodsInfo ginfo in goodsList)
                    {
                        List<TaoBaoAPIHelper.GoodsInfo> mygoodsList = goodsNickList.Where(o => o.num_iid == ginfo.num_iid).ToList();
                        if (mygoodsList.Count > 0)
                        {
                            goodsPriceTotal += mygoodsList[0].price * ginfo.Count;
                        }
                        else
                        {
                            TaoBaoAPIHelper.GoodsInfo newgoods = TaoBaoAPIHelper.TaoBaoAPI.GetGoodsInfoService(ginfo.num_iid);
                            if (newgoods != null)
                            {
                                goodsPriceTotal += newgoods.price * ginfo.Count;
                                //找到后添加到集合
                                goodsNickList.Add(newgoods);
                            }
                        }
                    }
                    kefutotalInfo.GoodsPay = goodsPriceTotal;
                }
                //添加或者更新
                kfDal.AddOrUp(kefutotalInfo);
            }
        }
    }

    #endregion
}