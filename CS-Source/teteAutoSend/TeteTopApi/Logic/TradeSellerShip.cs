using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeteTopApi.Entity;
using TeteTopApi.DataContract;
using TeteTopApi.TopApi;
using System.Text.RegularExpressions;

namespace TeteTopApi.Logic
{
    public class TradeSellerShip
    {
        private static object padlock2 = new object();
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="trade"></param>
        public TradeSellerShip(Trade trade) 
        {
            this.TradeInfo = trade;
        }

        /// <summary>
        /// 处理订单发货时的相应逻辑
        /// </summary>
        public void Start()
        {
            //获取店铺的基础数据
            ShopData data = new ShopData();
            ShopInfo shop = data.ShopInfoGetByNick(TradeInfo.Nick);

            if (shop.Version != "2" && shop.Version != "3")
            {
                return;
            }

            //通过TOP接口查询该订单的详细数据并记录到数据库中
            TopApiHaoping api = new TopApiHaoping(shop.Session);
            Trade trade = api.GetTradeByTid(TradeInfo);

            trade = api.GetOrderShippingInfo(trade);

            //发送短信-上LOCK锁定
            //判断该评价是否存在
            TradeData dbTrade = new TradeData();
            if (!dbTrade.CheckTradeExits(trade))
            {
                //更新该订单的评价时间
                dbTrade.InsertTradeInfo(trade);
            }

            try
            {
                //更新订单的优惠券使用情况
                TopApiHaoping apiCoupon = new TopApiHaoping(shop.Session);
                string result = apiCoupon.GetCouponTradeTotalByNick(trade);

                MatchCollection match = new Regex(@"<promotion_details list=""true""><promotion_detail><discount_fee>([^\<]*)</discount_fee><id>[0-9]*</id><promotion_desc>[^\<]*</promotion_desc><promotion_id>shopbonus-[0-9]*_[0-9]*-([0-9]*)</promotion_id><promotion_name>店铺优惠券</promotion_name></promotion_detail>", RegexOptions.IgnoreCase).Matches(result);

                if (match.Count != 0)
                {
                    string price = match[0].Groups[1].ToString();
                    string couponid = match[0].Groups[2].ToString();

                    if (couponid.Length != 0)
                    {
                        TradeData dataTradeCoupon = new TradeData();
                        dataTradeCoupon.UpdateTradeCouponInfo(trade, price, couponid);
                    }
                }
            }
            catch { }



            lock (padlock2)
            {
                //判断如果是分销的订单，则不处理
                if (trade.OrderType.ToLower() == "fenxiao")
                { 
                    return;
                }

                //判断该用户是否开启了发货短信
                if (shop.MsgIsFahuo == "1" && int.Parse(shop.MsgCount) > 0)
                {
                    //判断同类型的短信该客户今天是否只收到一条
                    ShopData db = new ShopData();
                    if (!db.IsSendMsgToday(trade, "fahuo"))
                    {
                        //发送短信
                        string msg = Message.GetMsg(shop.MsgFahuoContent, shop.MsgShopName, TradeInfo.BuyNick, shop.IsCoupon, TradeInfo.ShippingCompanyName, TradeInfo.ShippingNumber, shop, trade);
                        string msgResult = Message.SendGuodu(trade.Mobile, msg);

                        //记录
                        if (msgResult != "0")
                        {
                            db.InsertShopMsgLog(shop, trade, msg, msgResult, "fahuo");
                        }
                        else
                        {
                            db.InsertShopErrMsgLog(shop, trade, msg, msgResult, "fahuo");
                        }
                        shop.MsgCount = (int.Parse(shop.MsgCount) - 1).ToString();
                    }
                }
            }
        }

        /// <summary>
        /// 订单消息
        /// </summary>
        private Trade TradeInfo { get; set; }
    }
}
