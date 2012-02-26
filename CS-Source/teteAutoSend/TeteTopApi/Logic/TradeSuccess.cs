using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeteTopApi.Entity;
using TeteTopApi.TopApi;
using TeteTopApi.DataContract;

namespace TeteTopApi.Logic
{
    public class TradeSuccess
    {
        private static object padlock3 = new object();
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="trade"></param>
        public TradeSuccess(Trade trade) 
        {
            this.TradeInfo = trade;
        }

        /// <summary>
        /// 处理订单完成时的相应逻辑
        /// </summary>
        public void Start()
        {
            //获取店铺的基础数据
            ShopData data = new ShopData();
            ShopInfo shop = data.ShopInfoGetByNick(TradeInfo.Nick);
            //Console.Write(shop.Session + "!!!!!!!!!!!!\r\n");

            //通过TOP接口查询该订单的详细数据并记录到数据库中
            TopApiHaoping api = new TopApiHaoping(shop.Session);
            Trade trade = api.GetTradeByTid(TradeInfo);

            //记录该订单对应的评价记录
            TradeRate tradeRate = api.GetTradeRate(trade);
            tradeRate.ItemId = trade.NumIid;
            tradeRate.Nick = TradeInfo.Nick;


                //只有双方都评价了才会有插入数据库和赠送的操作
                if (tradeRate.Content != "")
                {
                    //判断该订单是否已经有过评价记录
                    TradeRateData dbTradeRate = new TradeRateData();
                    if (!dbTradeRate.CheckTradeRateExits(tradeRate))
                    {
                        //没有记录过写入数据库
                        dbTradeRate.InsertTradeInfo(tradeRate);
                    }

                    //判断该订单是否存在
                    TradeData dbTrade = new TradeData();
                    if (!dbTrade.CheckTradeExits(trade))
                    {
                        //更新该订单的评价时间
                        dbTrade.InsertTradeInfo(trade);
                    }

                    //更新该订单的评价时间
                    dbTrade.UpdateTradeRateById(trade, tradeRate);
                }
                else
                {
                    //否则中断
                    return;
                }

                //判断是否开启了客服审核，如果开启了则自动记录并中断
                if (shop.IsKefu == "1")
                {
                    //更新该订单的评价为待审核状态
                    TradeData dbTrade = new TradeData();
                    if (!dbTrade.CheckTradeRateCheckExits(trade))
                    {
                        dbTrade.UpdateTradeKefuById(trade, tradeRate);
                    }
                    return;
                }
            

            //获取订单的具体物流状态，如果订单物流状态为空则获取物流信息
            if (trade.ShippingType != "system" && trade.ShippingType != "self")
            {
                //获取该订单的物流状态
                TopApiHaoping apiHaoping = new TopApiHaoping(shop.Session);
                string status = apiHaoping.GetShippingStatusByTid(trade);
                TradeData dbTrade = new TradeData();

                Console.Write(status + "\r\n");
                //如果该物流信息不存在
                if (status.IndexOf("不存在") != -1)
                {
                    //如果该物流公司不支持查询则更新为self
                    dbTrade.UpdateTradeShippingStatusSelf(trade, status);
                }

                //获取该订单的物流相关信息
                trade = api.GetOrderShippingInfo(trade);
                if (!dbTrade.IsTaobaoCompany(trade))
                {
                    //如果不是淘宝合作物流则直接更新
                    dbTrade.UpdateTradeShippingStatusSelf(trade, status);
                }
                else
                {
                    //根据服务器的物流状态进行判断，如果物流状态是已签收
                    if (status == "ACCEPTED_BY_RECEIVER")
                    {
                        string result = api.GetShippingStatusDetailByTid(trade);
                        Console.Write("【" + result + "】\r\n");
                        //如果是虚拟物品
                        if (result.IndexOf("该订单未指定运单号") != -1)
                        {
                            //如果该物流公司不支持查询则更新为self
                            dbTrade.UpdateTradeShippingStatusSelf(trade, status);
                        }

                        //如果订单不是服务器错误
                        if (result.IndexOf("company-not-support") != -1)
                        {
                            //如果该物流公司不支持查询则更新为self
                            dbTrade.UpdateTradeShippingStatusSelf(trade, status);
                        }

                        //再根据订单的详细物流信息判断签收的状态
                        if (result.IndexOf("签收人") != -1 || result.IndexOf("正常签收录入扫描") != -1)
                        {
                            //如果物流已经签收了则更新对应订单状态
                            trade.DeliveryEnd = utils.GetShippingEndTime(result); ;
                            trade.DeliveryMsg = result;

                            //如果物流到货时间还是为空
                            if (trade.DeliveryEnd == "")
                            {
                                LogData dbLog = new LogData();
                                dbLog.InsertErrorLog(trade.Nick, "deliveryDateNullOrder", "", result, "");
                            }

                            dbTrade.UpdateTradeShippingStatusSystem(trade, status);
                        }
                    }
                }
            }



            //发送短信-上LOCK锁定
            lock (padlock3)
            {
                //判断是否符合赠送条件
                if (CheckCouponSend(shop, tradeRate, trade))
                {
                    //如果符合赠送条件调用赠送接口
                    CouponData dbCoupon = new CouponData();
                    Coupon coupon = dbCoupon.GetCouponInfoById(shop);

                    string couponId = api.SendCoupon(trade.BuyNick, coupon.TaobaoCouponId);

                    //判定该优惠券是否过期或删除
                    if (!dbCoupon.CheckCouponCanUsed(shop))
                    {
                        //优惠券过期，自动帮客户延长优惠券期限
                        //考虑到淘宝即将开启短授权，该功能改成消息通知，暂不制作
                        return;
                    }

                    //获取的赠送接口的返回结果
                    if (couponId != "")
                    {
                        //如果成功赠送则记录
                        dbCoupon.InsertCouponSendRecord(trade, shop, couponId);
                    }
                    else
                    {
                        //有可能是客户订购的优惠券服务已经到期记录错误信息并中断
                        return;
                    }

                    //判断该用户是否开启了发货短信
                    if (shop.MsgIsCoupon == "1" && int.Parse(shop.MsgCount) > 0)
                    {
                        ShopData db = new ShopData();
                        if (!db.IsSendMsgToday(trade, "gift"))
                        {
                            //发送短信
                            string msg = Message.GetMsg(shop.MsgCouponContent, shop.MsgShopName, TradeInfo.BuyNick, shop.IsCoupon);
                            string msgResult = Message.Send(trade.Mobile, msg);

                            //记录
                            if (msgResult != "0")
                            {
                                db.InsertShopMsgLog(shop, trade, msg, msgResult, "gift");
                            }
                            else
                            {
                                db.InsertShopErrMsgLog(shop, trade, msg, msgResult, "gift");
                            }
                            shop.MsgCount = (int.Parse(shop.MsgCount) - 1).ToString();
                        }
                    }
                }
            }
        }
        

        /// <summary>
        /// 自动给该客户的优惠券增加使用期限
        /// </summary>
        /// <param name="?"></param>
        /// <param name="?"></param>
        /// <param name="?"></param>
        private void AddCouponDateInfo()
        {
            
        }

        /// <summary>
        /// 判断是否符合赠送条件
        /// </summary>
        /// <param name="shop"></param>
        /// <param name="trade"></param>
        /// <returns></returns>
        private bool CheckCouponSend(ShopInfo shop, TradeRate tradeRate, Trade trade)
        {
            //是否开启赠送优惠券
            if (shop.IsCoupon == "0")
            {
                return false;
            }

            //不是好评不赠送
            if (tradeRate.Result != "good")
            {
                return false;
            }

            //没有在规定时间内好评不赠送 (self)
            if (trade.ShippingType == "self" && (DateTime.Parse(tradeRate.Created) - DateTime.Parse(trade.SendTime)).Seconds > int.Parse(shop.MinDateSelf) * 86400)
            {
                return false;
            }

            //没有在规定时间内好评不赠送 (system)
            if (trade.ShippingType == "system" && (DateTime.Parse(tradeRate.Created) - DateTime.Parse(trade.DeliveryEnd)).Seconds > int.Parse(shop.MinDateSystem) * 86400)
            {
                return false;
            }

            //使用系统默认的好评和过期自动好评不赠送
            if (shop.IsCancelAuto == "1" && (tradeRate.Content == "好评！" || tradeRate.Content == "评价方未及时做出评价,系统默认好评!" || tradeRate.Content == "评价方未及时做出评价,系统默认好评！"))
            {
                return false;
            }

            //超过了每人的最大领取数量不赠送
            CouponData dbCoupon = new CouponData();
            Coupon coupon = dbCoupon.GetCouponInfoById(shop);
            if (!dbCoupon.CheckUserCanGetCoupon(trade, coupon))
            {
                return false;
            }

            //超出了优惠券最大赠送量不赠送
            if (!dbCoupon.CheckCouponCanSend(trade, coupon))
            {
                return false;
            }

            //如果该订单赠送过则不赠送
            if (!dbCoupon.CheckUserCouponSend(trade, coupon))
            {
                return false;
            }

            //包含了指定的关键字赠送或者不赠送
            if (shop.IsKeyword == "1")
            {
                //先判定字数是否满足条件
                if (tradeRate.Content.Length < int.Parse(shop.WordCount))
                {
                    return false;
                }

                //再判定是否包含关键字
                if (shop.Keyword != "")
                {
                    int isInclude = 0;
                    string[] keyArray = shop.Keyword.Split('|');
                    for (int i = 0; i < keyArray.Length; i++)
                    {
                        if (keyArray[i].Trim() != "")
                        {
                            if (tradeRate.Content.IndexOf(keyArray[i].Trim()) != -1)
                            {
                                isInclude = 1;
                                break;
                            }
                        }
                    }

                    //判定
                    if (isInclude == 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 订单消息
        /// </summary>
        private Trade TradeInfo { get; set; }
    }
}
