using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeteTopApi.DataContract;
using TeteTopApi.Entity;
using TeteTopApi.TopApi;
using System.Text.RegularExpressions;

namespace TeteTopApi.Logic
{
    public class ShopAlert
    {
        private static object padlock5 = new object();

        /// <summary>
        /// 检查有很多未审核评价的卖家并给出提示
        /// </summary>
        public void StartUnChecked()
        {
            //获取账户里面有1周前未审核评价的卖家
            ShopData dbShop = new ShopData();
            List<ShopInfo> list = dbShop.GetShopInfoListUnChecked();

            TradeRateData dbTradeRate = new TradeRateData();
            MessageData dbMessage = new MessageData();
            string typ = "uncheck";

            //循环获取这些卖家的未审核订单
            for (int i = 0; i < list.Count; i++)
            {
                ShopInfo shop = list[i];
                //获取数据库中未审核的数据列表
                string count = dbTradeRate.GetUncheckedTradeRateCount(shop);
                string msg = "好评有礼:亲爱的" + shop.Nick + ",您目前有" + count + "条评价未审核,您可以到服务里面的\"待审核列表\"中处理是否赠送优惠券";
                
                //如果7天内已经发送过类似短信的话则不再提醒
                if (!dbMessage.IsSendMsgNearDays(shop, typ))
                {
                    string msgResult = Message.Send(shop.Mobile, msg);
                    dbMessage.InsertShopAlertMsgLog(shop, msg, msgResult, typ);
                }
            }
        }

        /// <summary>
        /// 检查优惠券过期的卖家并给出提示
        /// </summary>
        public void StartCouponExpired()
        {
            //获取账户里面有1周前未审核评价的卖家
            ShopData dbShop = new ShopData();
            List<ShopInfo> list = dbShop.GetShopInfoListCouponExpired();

            MessageData dbMessage = new MessageData();
            string typ = "couponexpired";

            //循环获取这些卖家的未审核订单
            for (int i = 0; i < list.Count; i++)
            {
                ShopInfo shop = list[i];
                //告之这些卖家他们的优惠券已经过期，需要重新设置
                string msg = "好评有礼:亲爱的" + shop.Nick + ",您设置赠送的优惠券已经到期,您可以到服务里面的\"优惠券\"创建新的优惠券并在\"基本设置\"中重新保存";

                //如果7天内已经发送过类似短信的话则不再提醒
                if (!dbMessage.IsSendMsgNearDays(shop, typ))
                {
                    string msgResult = Message.Send(shop.Mobile, msg);
                    dbMessage.InsertShopAlertMsgLog(shop, msg, msgResult, typ);
                }
            }
        }

        /// <summary>
        /// 检查优惠券已经送完的卖家并给出提示
        /// </summary>
        public void StartCouponOver()
        {
            //获取账户里面有1周前未审核评价的卖家
            ShopData dbShop = new ShopData();
            List<ShopInfo> list = dbShop.GetShopInfoListShippingAlert();

            MessageData dbMessage = new MessageData();
            string typ = "couponover";

            //循环获取这些卖家的未审核订单
            for (int i = 0; i < list.Count; i++)
            {
                ShopInfo shop = list[i];
                //告之这些卖家他们的优惠券已经过期，需要重新设置
                string msg = "好评有礼:亲爱的" + shop.Nick + ",您设置赠送的优惠券已经送完,您可以到服务里面的\"优惠券\"创建新的优惠券并在\"基本设置\"中重新保存";

                //如果7天内已经发送过类似短信的话则不再提醒
                if (!dbMessage.IsSendMsgNearDays(shop, typ))
                {
                    string msgResult = Message.Send(shop.Mobile, msg);
                    dbMessage.InsertShopAlertMsgLog(shop, msg, msgResult, typ);
                }
            }
        }

        /// <summary>
        /// 检查账户即将到期的客户并给出提醒
        /// </summary>
        public void StartShopExpired()
        {
            //获取账户里面有1周前未审核评价的卖家
            ShopData dbShop = new ShopData();
            List<ShopInfo> list = dbShop.GetShopInfoListShippingAlert();

            //循环获取这些卖家的未审核订单
            for (int i = 0; i < list.Count; i++)
            {
                ShopInfo shop = list[i];
                //获取未审核的评价并发送消息

            }
        }

        /// <summary>
        /// 检查账户里余额不足的客户并给出提示
        /// </summary>
        public void StartMsgExpired()
        {
            //获取账户里面有1周前未审核评价的卖家
            ShopData dbShop = new ShopData();
            List<ShopInfo> list = dbShop.GetShopInfoListShippingAlert();

            //循环获取这些卖家的未审核订单
            for (int i = 0; i < list.Count; i++)
            {
                ShopInfo shop = list[i];
                //获取未审核的评价并发送消息

            }
        }

        /// <summary>
        /// 检查客户的使用情况并给出总评消息提示
        /// </summary>
        public void StartShopStatusAlert()
        {
            //获取目前在用的而且自动赠送优惠券的
            ShopData dbShop = new ShopData();
            List<ShopInfo> list = dbShop.GetShopInfoNormalUsed();
            
            TradeData dbTrade = new TradeData();
            CouponData dbCoupon = new CouponData();
            MessageData dbMessage = new MessageData();
            string typ = "status";

            //循环获取这些卖家的未审核订单
            for (int i = 0; i < list.Count; i++)
            {
                int couponOrderCount = 0;
                decimal couponOrderPrice = 0;
                ShopInfo shop = list[i];

                //获取卖家赠送的优惠券数量
                string sendcount = dbCoupon.GetCouponSendCountWeekByNick(shop);
                //如果一个都没赠送出去就不发送
                if (sendcount == "0")
                {
                    Console.Write("该卖家没有赠送出任何优惠券，先不消息提示..\r\n");
                    continue;
                }

                TopApiHaoping api = new TopApiHaoping(shop.Session);
                List<Trade> listTrade = dbTrade.GetTradeAllByNick(shop);
                Console.Write("total:[" + listTrade.Count.ToString() + "]\r\n");
                for (int j = 0; j < listTrade.Count; j++)
                {
                    //获取未审核的评价并发送消息
                    string result = api.GetCouponTradeTotalByNick(listTrade[j]);

                    string couponid = new Regex(@"<promotion_detail><discount_fee>[^\<]*</discount_fee><id>([0-9]*)</id><promotion_name>店铺优惠券</promotion_name>", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
                    string price = new Regex(@"<total_fee>([^\<]*)</total_fee>", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();

                    if (couponid != "")
                    {
                        couponOrderCount++;
                        couponOrderPrice += decimal.Parse(price);
                    }
                }

                if (couponOrderCount == 0)
                {
                    Console.Write("该卖家没有优惠券产生2次订购，先不消息提示..\r\n");
                    continue;
                }

                string msg = "好评有礼:" + shop.Nick + ",最近14天共赠送了" + sendcount + "张优惠券," + couponOrderCount.ToString() + "个客户使用优惠券产生了二次购买总额" + couponOrderPrice + "元";

                //如果14天内已经发送过类似短信的话则不再提醒
                if (!dbMessage.IsSendMsgNearDays(shop, typ))
                {
                    string msgResult = Message.Send(shop.Mobile, msg);
                    dbMessage.InsertShopAlertMsgLog(shop, msg, msgResult, typ);
                    Console.Write(msg + "[" + msg.Length.ToString() + "]\r\n");
                }

            }
        }

        /// <summary>
        /// 检查账户里余额不足的客户并给出提示
        /// </summary>
        public void StartDeleteShop()
        {
            //获取全部店铺
            ShopData dbShop = new ShopData();
            List<ShopInfo> list = dbShop.ShopInfoListAll();

            //通过接口获取这些店铺的到期时间
            for (int i = 0; i < list.Count; i++)
            {
                try
                {
                    ShopInfo shop = list[i];

                    //if (shop.Nick != "带你远航2008")
                    //{
                    //    continue;
                    //}

                    TopApiHaoping api = new TopApiHaoping(shop.Session);
                    string result = api.GetUserExpiredDate(shop);
                    Console.Write(result + "\r\n");
                    //判断是否过期
                    if (result.IndexOf("\"article_user_subscribes\":{}") != -1)
                    {
                        //更新为del状态
                        dbShop.DeleteShop(shop);
                    }
                    else
                    {
                        int isok = 0;
                        Regex reg = new Regex(@"""item_code"":""([^""]*)""", RegexOptions.IgnoreCase);
                        //更新店铺的版本号
                        MatchCollection match = reg.Matches(result);
                        for (int j = 0; j < match.Count; j++)
                        {
                            shop.Version = match[j].Groups[1].ToString().Replace("service-0-22904-", "");
                            if (int.Parse(shop.Version) <= 3)
                            {
                                isok = 1;
                                break;
                            }
                        }

                        if (isok == 0)
                        {
                            shop.Version = "0";
                        }

                        dbShop.ActiveShopVersion(shop);
                    }
                }
                catch(Exception ex)
                {
                    Console.Write(ex.Message.ToString() + "\r\n");
                }
            }
        }
    }
}
