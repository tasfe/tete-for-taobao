using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeteTopApi.Entity;
using TeteTopApi.TopApi;
using TeteTopApi.DataContract;
using System.Text.RegularExpressions;

namespace TeteTopApi.Logic
{
    public class TradeSuccess
    {
        public static object padlockRate = new object(); 
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
            if (shop.Version != "2" && shop.Version != "3")
            {
                return;
            }

            //通过TOP接口查询该订单的详细数据并记录到数据库中
            TopApiHaoping api = new TopApiHaoping(shop.Session);
            TradeData tradeData = new TradeData();
            Trade trade = tradeData.GetTradeDetailShippingInfo(TradeInfo);
            trade = api.GetTradeByTid(trade);

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

            //判断如果是分销的订单，则不处理
            if (trade.OrderType.ToLower() == "fenxiao")
            {
                return;
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

            //判断是否开启了客服审核，如果开启了则自动记录并中断
            if (shop.IsKefu == "1")
            {
                TradeRateData dataKefu = new TradeRateData();
                string resultKefu = "手动审核订单！";
                dataKefu.UpdateTradeRateResult(tradeRate, resultKefu);

                //更新该订单的评价为待审核状态
                TradeData dbTrade = new TradeData();
                if (!dbTrade.CheckTradeRateCheckExits(trade))
                {
                    dbTrade.UpdateTradeKefuById(trade, tradeRate);
                }

                try
                {
                    //记录会员信息数据
                    GetUserData getUser = new GetUserData();
                    getUser.Get(trade);
                }
                catch { }

                return;
            }

            Console.WriteLine(trade.ShippingType + "!!");

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
                    trade.ShippingType = "self";
                }
                else
                {
                    //根据服务器的物流状态进行判断，如果物流状态是已签收
                    if (status == "ACCEPTED_BY_RECEIVER" || status == "ACCEPTING" || status == "ACCEPTED")
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
                        if (result.IndexOf("签收人") != -1 || result.IndexOf(" 妥投") != -1 || result.IndexOf(" 签收") != -1 || result.IndexOf("正常签收") != -1 || result.IndexOf(" 已签收") != -1)
                        {
                            //如果物流已经签收了则更新对应订单状态
                            trade.DeliveryEnd = utils.GetShippingEndTime(result); ;
                            trade.DeliveryMsg = result;
                            trade.ShippingType = "system";

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

            //处理优惠券赠送及短信-上LOCK锁定
            lock (TradeSuccess.padlockRate)
            {
                //淘宝优惠券赠送
                bool isSendCoupon = true;
                //判断是否符合赠送条件
                if (CheckCouponSend(shop, tradeRate, trade))
                {
                    //如果符合赠送条件调用赠送接口
                    CouponData dbCoupon = new CouponData();
                    Coupon coupon = dbCoupon.GetCouponInfoById(shop);

                    //判定该优惠券是否过期或删除
                    if (!dbCoupon.CheckCouponCanUsed(shop))
                    {
                        //优惠券过期，自动帮客户延长优惠券期限
                        //考虑到淘宝即将开启短授权，该功能改成消息通知，暂不制作
                        isSendCoupon = false;
                        //return;
                    }
                    else
                    {
                        //解决多线程冲突问题先插入优惠券赠送记录，如果赠送失败再删除记录
                        string couponId = string.Empty;
                        if (dbCoupon.InsertCouponSendRecord(trade, shop, couponId))
                        {
                            string taobaoResult = string.Empty;
                            couponId = api.SendCoupon(trade.BuyNick, coupon.TaobaoCouponId, ref taobaoResult);

                            //获取的赠送接口的返回结果
                            if (couponId != "")
                            {
                                //如果成功赠送则记录
                                dbCoupon.UpdateCouponSendRecord(trade, shop, couponId);
                            }
                            else
                            {
                                //如果没有赠送成功则删除刚才的临时记录
                                dbCoupon.DeleteCouponSendRecord(trade, shop, couponId);
                                Console.WriteLine(couponId);
                                try
                                {
                                    //记录淘宝自身错误
                                    string err = new Regex(@"<reason>([^<]*)</reason>", RegexOptions.IgnoreCase).Match(taobaoResult).Groups[1].ToString();
                                    TradeRateData dataRate = new TradeRateData();
                                    if (err.Length == 0)
                                    {
                                        taobaoResult += "淘宝系统错误，不赠送优惠券，错误代码是【" + taobaoResult + "】！";
                                    }
                                    else
                                    {
                                        taobaoResult += "淘宝系统错误，不赠送优惠券，错误代码是【" + err + "】！";
                                    }
                                    dataRate.UpdateTradeRateResult(tradeRate, taobaoResult);
                                }
                                catch { }
                                //有可能是客户订购的优惠券服务已经到期记录错误信息并中断
                                //return;
                                isSendCoupon = false;
                            }
                        }
                        else
                        {
                            isSendCoupon = false;
                        }
                    }
                }
                else
                {
                    isSendCoupon = false;
                }

                //支付宝现金券赠送
                bool isSendAlipay = true;
                //判断是否符合赠送条件
                if (CheckAlipaySend(shop, tradeRate, trade))
                {
                    //获取一条需要赠送的支付宝红包数据
                    CouponData cou = new CouponData();
                    AlipayDetail detail = cou.GetAlipayDetailInfoById(shop);

                    //如果符合赠送条件调用短信接口直接将红包发到客户手机上
                    string shopName = shop.MsgShopName;
                    if (shop.MsgShopName.Length == 0)
                        shopName = shop.Nick;
                    string msgAlipay = "亲，" + shopName + "赠送您支付宝红包，卡号" + detail.Card + "密码" + detail.Pass + "，您可以到支付宝绑定使用。";
                    Console.Write(msgAlipay + "\r\n");
                    string msgResultAlipay = Message.Send(trade.Mobile, msgAlipay);

                    Console.Write(msgResultAlipay + "\r\n");
                    //更新支付宝红包使用状态
                    cou.InsertAlipaySendRecord(trade, shop, detail);

                    //记录短信发送记录
                    ShopData dbAlipay = new ShopData();
                    if (msgResultAlipay != "0")
                    {
                        dbAlipay.InsertShopMsgLog(shop, trade, msgAlipay, msgResultAlipay, "alipay");
                    }
                    else
                    {
                        isSendAlipay = false;
                        dbAlipay.InsertShopErrMsgLog(shop, trade, msgAlipay, msgResultAlipay, "alipay");
                    }
                    shop.MsgCount = (int.Parse(shop.MsgCount) - 1).ToString();
                }
                else
                {
                    isSendAlipay = false;
                }

                //包邮卡赠送
                bool isSendFreeCard = true;
                try
                {
                    //判断是否符合赠送条件
                    if (CheckFreeCardSend(shop, tradeRate, trade))
                    {
                        //赠送包邮卡
                        FreeCardData freeData = new FreeCardData();
                        freeData.SendFreeCard(shop, trade);

                        //判断该用户是否开启了包邮卡短信
                        if (shop.MsgIsFreecard == "1" && int.Parse(shop.MsgCount) > 0)
                        {
                            ShopData db = new ShopData();
                            FreeCard free = freeData.GetFreeCardById(shop.FreeCardId);
                            //发送短信
                            string msg = Message.GetMsg(shop.MsgFreecardContent, shop.MsgShopName, TradeInfo.BuyNick, shop.IsCoupon, free.Name);

                            //手机号码为空不发
                            if (trade.Mobile.Length == 0)
                            {
                                return;
                            }

                            if (!db.IsSendMsgToday(trade, "freecard"))
                            {
                                string msgResult = Message.Send(trade.Mobile, msg);

                                //记录
                                if (msgResult != "0")
                                {
                                    db.InsertShopMsgLog(shop, trade, msg, msgResult, "freecard");
                                }
                                else
                                {
                                    db.InsertShopErrMsgLog(shop, trade, msg, msgResult, "freecard");
                                }
                                shop.MsgCount = (int.Parse(shop.MsgCount) - 1).ToString();
                            }
                        }
                    }
                    else
                    {
                        isSendFreeCard = false;
                    }
                }
                catch(Exception e) {
                    Console.WriteLine(e.Message + e.StackTrace + e.Source);
                }




                try
                {
                    //记录会员信息数据
                    GetUserData getUser = new GetUserData();
                    getUser.Get(trade);
                }
                catch { }

                //Console.WriteLine(isSendCoupon + "!!");
                //Console.WriteLine(isSendAlipay + "!!");
                //Console.WriteLine(trade.Mobile + "!!");

                //如果优惠券和支付宝现金都没有赠送成功，则直接中断方法不发短信
                //2012.9.17改为只有赠送成功优惠券才发送此短信
                if (!isSendCoupon)
                {
                    return;
                }

                //判断该用户是否开启了发货短信
                if (shop.MsgIsCoupon == "1" && int.Parse(shop.MsgCount) > 0)
                {
                    ShopData db = new ShopData();
                    //发送短信
                    string msg = Message.GetMsg(shop.MsgCouponContent, shop.MsgShopName, TradeInfo.BuyNick, shop.IsCoupon);
                    //string msg = Message.GetMsg(shop, trade, shop.MsgCouponContent);

                    //手机号码为空不发
                    if (trade.Mobile.Length == 0)
                    {
                        return;
                    }

                    if (!db.IsSendMsgToday(trade, "gift"))
                    {
                        ////先插入数据库 解决多优惠券赠送短信多发问题
                        //db.InsertShopMsgLog(shop, trade, msg, "888888", "gift");

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
            TradeRateData data = new TradeRateData();
            string result = string.Empty;
            //是否开启赠送优惠券
            if (shop.IsCoupon == "0")
            {
                result = "卖家没有开启优惠券赠送！";
                data.UpdateTradeRateResult(tradeRate, result);
                return false;
            }

            //不是好评不赠送
            if (tradeRate.Result != "good")
            {
                result = "买家没有给好评，不赠送优惠券！";
                data.UpdateTradeRateResult(tradeRate, result);
                return false;
            }


            //礼品黑名单不赠送
            CouponData couponData = new CouponData();
            if (couponData.IsBlack(trade))
            {
                result = "买家【" + trade.BuyNick + "】在礼品黑名单中，不赠送优惠券！";
                data.UpdateTradeRateResult(tradeRate, result);
                return false;
            }

            Console.WriteLine(shop.IsItem + "--" + shop.ItemList + "-" + trade.NumIid);

            //没有购买指定商品不送
            if (shop.IsItem == "1" && shop.ItemList.Length != 0)
            { 
                //查看订单是否包含指定商品
                if (shop.ItemList.IndexOf(trade.NumIid) == -1)
                {
                    result = "买家订单商品为【" + trade.NumIid + "】，不包含指定商品【" + shop.ItemList + "】，不赠送优惠券！";
                    data.UpdateTradeRateResult(tradeRate, result);
                    return false;
                }
            }

            //没有在规定时间内好评不赠送 (self)
            if (trade.ShippingType == "self" && (DateTime.Parse(tradeRate.Created) - DateTime.Parse(trade.SendTime)).TotalSeconds > int.Parse(shop.MinDateSelf) * 86400)
            {
                result = "没有在规定时间内好评，该物流配送状态不可查，按照发货时间开始计算，发货时间是【" + trade.SendTime + "】，评价时间是【" + tradeRate.Created + "】，周期超过了卖家设定的最短时间【" + shop.MinDateSelf + "】天，不赠送优惠券！";
                data.UpdateTradeRateResult(tradeRate, result);
                return false;
            }
      
            //没有在规定时间内好评不赠送 (system)
            if (trade.ShippingType == "system" && (DateTime.Parse(tradeRate.Created) - DateTime.Parse(trade.DeliveryEnd)).TotalSeconds > int.Parse(shop.MinDateSystem) * 86400)
            {
                result = "没有在规定时间内好评，该物流配送状态可查，按照物流签收时间开始计算，物流签收时间是【" + trade.DeliveryEnd + "】，评价时间是【" + tradeRate.Created + "】，周期超过了卖家设定的最短时间【" + shop.MinDateSystem + "】天，不赠送优惠券！";
                Console.Write(result + "!!!!!!!!!!!!\r\n");
                data.UpdateTradeRateResult(tradeRate, result);
                return false;
            }

            //使用系统默认的好评和过期自动好评不赠送
            if (shop.IsCancelAuto == "1" 
                && ((tradeRate.Content == "好评！" && shop.Cancel1 == "1") 
                    || ((tradeRate.Content == "评价方未及时做出评价,系统默认好评!" || tradeRate.Content == "评价方未及时做出评价,系统默认好评！") && shop.Cancel2 == "1")
                ))
            {
                result = "卖家设置了默认好评不赠送优惠券，此买家给的是系统默认评价，不赠送优惠券！";
                data.UpdateTradeRateResult(tradeRate, result);
                return false;
            }

            //超过了每人的最大领取数量不赠送
            CouponData dbCoupon = new CouponData();
            Coupon coupon = dbCoupon.GetCouponInfoById(shop);
            if (!dbCoupon.CheckUserCanGetCoupon(trade, coupon))
            {
                result = "卖家设置了优惠券【" + coupon.TaobaoCouponId + "】的最大领取数量是【" + coupon.Per + "】，此买家已经领取了【" + coupon.Per + "】张，所以不再赠送优惠券！";
                data.UpdateTradeRateResult(tradeRate, result);
                return false;
            }

            //超出了优惠券最大赠送量不赠送
            if (!dbCoupon.CheckCouponCanSend(trade, coupon))
            {
                result = "系统赠送的优惠券超出了卖家设置的最大赠送数量，所以不再赠送优惠券！";
                data.UpdateTradeRateResult(tradeRate, result);
                return false;
            }

            //如果该订单赠送过则不赠送
            if (!dbCoupon.CheckUserCouponSend(trade, coupon))
            {
                //result = "此订单已经赠送过优惠券，所以不再赠送优惠券！";
                //data.UpdateTradeRateResult(tradeRate, result);
                return false;
            }

            //包含了指定的关键字赠送或者不赠送
            if (shop.IsKeyword == "1")
            {
                //先判定字数是否满足条件
                if (tradeRate.Content.Length < int.Parse(shop.WordCount))
                {
                    result = "卖家开启了评价内容自动审核，此评价的字数是【" + tradeRate.Content.Length.ToString() + "】个字，小于卖家设置的最少评论字数【" + shop.WordCount + "】个字，所以不赠送优惠券！";
                    data.UpdateTradeRateResult(tradeRate, result);
                    return false;
                }

                //判定是否为差评关键字判断
                if (shop.KeywordIsBad == "1")
                {
                    if (shop.BadKeyword != "")
                    {
                        int isInclude = 0;
                        string badword = string.Empty;
                        string[] keyArray = shop.BadKeyword.Split('|');
                        for (int i = 0; i < keyArray.Length; i++)
                        {
                            if (keyArray[i].Trim() != "")
                            {
                                if (tradeRate.Content.IndexOf(keyArray[i].Trim()) != -1)
                                {
                                    badword = keyArray[i].Trim();
                                    isInclude = 1;
                                    break;
                                }
                            }
                        }

                        //判定
                        if (isInclude == 1)
                        {
                            result = "卖家开启了评价内容自动审核，买家评价包含差评关键字【" + badword + "】，所以不赠送优惠券！";
                            data.UpdateTradeRateResult(tradeRate, result);
                            return false;
                        }
                    }
                }
                else
                {
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
                            result = "卖家开启了评价内容自动审核，买家评价没有包含关键字，所以不赠送优惠券！";
                            data.UpdateTradeRateResult(tradeRate, result);
                            return false;
                        }
                    }
                }
            }

            ////订单包含商品判断
            //TradeData tradeData = new TradeData();
            //if (tradeData.IsIncludeProduct(trade, shop))
            //{
            //    result = "卖家开启了特定商品赠送，该订单里面商品不符合条件，所以不赠送优惠券！";
            //    data.UpdateTradeRateResult(tradeRate, result);
            //    return false;
            //}

            return true;
        }

        /// <summary>
        /// 判断是否符合赠送条件
        /// </summary>
        /// <param name="shop"></param>
        /// <param name="trade"></param>
        /// <returns></returns>
        private bool CheckAlipaySend(ShopInfo shop, TradeRate tradeRate, Trade trade)
        {
            TradeRateData data = new TradeRateData();
            string result = string.Empty;
            //是否开启赠送优惠券
            if (shop.IsAlipay == "0")
            {
                result = "卖家没有开启支付宝红包赠送！";
                data.UpdateTradeRateResultAlipay(tradeRate, result);
                return false;
            }

            //判断该用户是否开启了发货短信
            if (int.Parse(shop.MsgCount) <= 0)
            {
                result = "卖家没有短信，无法通过短信告之客户支付宝红包卡号密码！";
                data.UpdateTradeRateResultAlipay(tradeRate, result);
                return false;
            }

            //不是好评不赠送
            if (tradeRate.Result != "good")
            {
                result = "买家没有给好评，不赠送支付宝红包！";
                data.UpdateTradeRateResultAlipay(tradeRate, result);
                return false;
            }

            //礼品黑名单不赠送
            CouponData couponData = new CouponData();
            if (couponData.IsBlack(trade))
            {
                result = "买家【" + trade.BuyNick + "】在礼品黑名单中，不赠送支付宝红包！";
                data.UpdateTradeRateResultAlipay(tradeRate, result);
                return false;
            }

            //没有购买指定商品不送
            if (shop.IsItem == "1" && shop.ItemList.Length != 0)
            {
                //查看订单是否包含指定商品
                if (shop.ItemList.IndexOf(trade.NumIid) == -1)
                {
                    result = "买家订单商品为【" + trade.NumIid + "】，不包含指定商品【" + shop.ItemList + "】，不赠送支付宝红包！";
                    data.UpdateTradeRateResultAlipay(tradeRate, result);
                    return false;
                }
            }

            //没有在规定时间内好评不赠送 (self)
            if (trade.ShippingType == "self" && (DateTime.Parse(tradeRate.Created) - DateTime.Parse(trade.SendTime)).TotalSeconds > int.Parse(shop.MinDateSelf) * 86400)
            {
                result = "没有在规定时间内好评，该物流配送状态不可查，按照发货时间开始计算，发货时间是【" + trade.SendTime + "】，评价时间是【" + tradeRate.Created + "】，周期超过了卖家设定的最短时间【" + shop.MinDateSelf + "】天，不赠送支付宝红包！";
                data.UpdateTradeRateResultAlipay(tradeRate, result);
                return false;
            }

            //没有在规定时间内好评不赠送 (system)
            if (trade.ShippingType == "system" && (DateTime.Parse(tradeRate.Created) - DateTime.Parse(trade.DeliveryEnd)).TotalSeconds > int.Parse(shop.MinDateSystem) * 86400)
            {
                result = "没有在规定时间内好评，该物流配送状态可查，按照物流签收时间开始计算，物流签收时间是【" + trade.DeliveryEnd + "】，评价时间是【" + tradeRate.Created + "】，周期超过了卖家设定的最短时间【" + shop.MinDateSystem + "】天，不赠送支付宝红包！";
                data.UpdateTradeRateResultAlipay(tradeRate, result);
                return false;
            }

            //使用系统默认的好评和过期自动好评不赠送
            if (shop.IsCancelAuto == "1"
        && ((tradeRate.Content == "好评！" && shop.Cancel1 == "1")
            || ((tradeRate.Content == "评价方未及时做出评价,系统默认好评!" || tradeRate.Content == "评价方未及时做出评价,系统默认好评！") && shop.Cancel2 == "1")
        ))
            {
                result = "卖家设置了默认好评不赠送支付宝红包，此买家给的是系统默认评价，不赠送支付宝红包！";
                data.UpdateTradeRateResultAlipay(tradeRate, result);
                return false;
            }


            CouponData dbCoupon = new CouponData();
            Alipay alipay = dbCoupon.GetAlipayInfoById(shop);


            //每订单最多赠送一个
            if (!dbCoupon.CheckOrderAlipayCanSend(trade, alipay))
            {
                result = "每订单最多赠送一个支付宝红包！";
                data.UpdateTradeRateResult(tradeRate, result);
                return false;
            }

            //超过了每人的最大领取数量不赠送
            if (!dbCoupon.CheckUserCanGetAlipay(trade, alipay))
            {
                result = "卖家设置了红包【" + alipay.GUID + "】的最大领取数量是【" + alipay.Per + "】，此买家已经领取了【" + alipay.Per + "】张，所以不再赠送支付宝红包！";
                data.UpdateTradeRateResultAlipay(tradeRate, result);
                return false;
            }

            //超出了优惠券最大赠送量不赠送
            if (!dbCoupon.CheckAlipayCanSend(trade, alipay))
            {
                result = "系统赠送的支付宝红包超出了卖家设置的最大赠送数量，所以不再赠送支付宝红包！";
                data.UpdateTradeRateResultAlipay(tradeRate, result);
                return false;
            }

            //包含了指定的关键字赠送或者不赠送
            if (shop.IsKeyword == "1")
            {
                //先判定字数是否满足条件
                if (tradeRate.Content.Length < int.Parse(shop.WordCount))
                {
                    result = "卖家开启了评价内容自动审核，此评价的字数是【" + tradeRate.Content.Length.ToString() + "】个字，小于卖家设置的最少评论字数【" + shop.WordCount + "】个字，所以不赠送支付宝红包！";
                    data.UpdateTradeRateResultAlipay(tradeRate, result);
                    return false;
                }

                //判定是否为差评关键字判断
                if (shop.KeywordIsBad == "1")
                {
                    if (shop.BadKeyword != "")
                    {
                        int isInclude = 0;
                        string badword = string.Empty;
                        string[] keyArray = shop.BadKeyword.Split('|');
                        for (int i = 0; i < keyArray.Length; i++)
                        {
                            if (keyArray[i].Trim() != "")
                            {
                                if (tradeRate.Content.IndexOf(keyArray[i].Trim()) != -1)
                                {
                                    badword = keyArray[i].Trim();
                                    isInclude = 1;
                                    break;
                                }
                            }
                        }

                        //判定
                        if (isInclude == 1)
                        {
                            result = "卖家开启了评价内容自动审核，买家评价包含差评关键字【" + badword + "】，所以不赠送支付宝红包！";
                            data.UpdateTradeRateResultAlipay(tradeRate, result);
                            return false;
                        }
                    }
                }
                else
                {
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
                            result = "卖家开启了评价内容自动审核，买家评价没有包含关键字，所以不赠送支付宝红包！";
                            data.UpdateTradeRateResultAlipay(tradeRate, result);
                            return false;
                        }
                    }
                }
            }

            //如果该支付宝红包已经过期
            if (dbCoupon.CheckAlipayExpired(trade, alipay))
            {
                result = "该支付宝红包已经过期，所以不再赠送！";
                data.UpdateTradeRateResultAlipay(tradeRate, result);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 判断是否符合赠送条件
        /// </summary>
        /// <param name="shop"></param>
        /// <param name="trade"></param>
        /// <returns></returns>
        private bool CheckFreeCardSend(ShopInfo shop, TradeRate tradeRate, Trade trade)
        {
            TradeRateData data = new TradeRateData();
            string result = string.Empty;
            //是否开启赠送优惠券
            if (shop.IsFreeCard == "0")
            {
                result = "卖家没有开启包邮卡赠送！";
                data.UpdateTradeRateResultFree(tradeRate, result);
                return false;
            }

            ////判断该用户是否开启了发货短信
            //if (int.Parse(shop.MsgCount) <= 0)
            //{
            //    result = "卖家没有短信，无法通过短信告之客户支付宝红包卡号密码！";
            //    data.UpdateTradeRateResultAlipay(tradeRate, result);
            //    return false;
            //}

            //不是好评不赠送
            if (tradeRate.Result != "good")
            {
                result = "买家没有给好评，不赠送包邮卡！";
                data.UpdateTradeRateResultFree(tradeRate, result);
                return false;
            }

            //礼品黑名单不赠送
            CouponData couponData = new CouponData();
            if (couponData.IsBlack(trade))
            {
                result = "买家【" + trade.BuyNick + "】在礼品黑名单中，不赠送包邮卡！";
                data.UpdateTradeRateResultFree(tradeRate, result);
                return false;
            }

            //没有购买指定商品不送
            if (shop.IsItem == "1" && shop.ItemList.Length != 0)
            {
                //查看订单是否包含指定商品
                if (shop.ItemList.IndexOf(trade.NumIid) == -1)
                {
                    result = "买家订单商品为【" + trade.NumIid + "】，不包含指定商品【" + shop.ItemList + "】，不赠送包邮卡！";
                    data.UpdateTradeRateResultFree(tradeRate, result);
                    return false;
                }
            }

            //没有在规定时间内好评不赠送 (self)
            if (trade.ShippingType == "self" && (DateTime.Parse(tradeRate.Created) - DateTime.Parse(trade.SendTime)).TotalSeconds > int.Parse(shop.MinDateSelf) * 86400)
            {
                result = "没有在规定时间内好评，该物流配送状态不可查，按照发货时间开始计算，发货时间是【" + trade.SendTime + "】，评价时间是【" + tradeRate.Created + "】，周期超过了卖家设定的最短时间【" + shop.MinDateSelf + "】天，不赠送支付宝红包！";
                data.UpdateTradeRateResultFree(tradeRate, result);
                return false;
            }

            //没有在规定时间内好评不赠送 (system)
            if (trade.ShippingType == "system" && (DateTime.Parse(tradeRate.Created) - DateTime.Parse(trade.DeliveryEnd)).TotalSeconds > int.Parse(shop.MinDateSystem) * 86400)
            {
                result = "没有在规定时间内好评，该物流配送状态可查，按照物流签收时间开始计算，物流签收时间是【" + trade.DeliveryEnd + "】，评价时间是【" + tradeRate.Created + "】，周期超过了卖家设定的最短时间【" + shop.MinDateSystem + "】天，不赠送支付宝红包！";
                data.UpdateTradeRateResultFree(tradeRate, result);
                return false;
            }

            //使用系统默认的好评和过期自动好评不赠送
            if (shop.IsCancelAuto == "1"
        && ((tradeRate.Content == "好评！" && shop.Cancel1 == "1")
            || ((tradeRate.Content == "评价方未及时做出评价,系统默认好评!" || tradeRate.Content == "评价方未及时做出评价,系统默认好评！") && shop.Cancel2 == "1")
        ))
            {
                result = "卖家设置了默认好评不赠送包邮卡，此买家给的是系统默认评价，不赠送包邮卡！";
                data.UpdateTradeRateResultFree(tradeRate, result);
                return false;
            }

            ////超过了每人的最大领取数量不赠送
            //CouponData dbCoupon = new CouponData();
            //Alipay alipay = dbCoupon.GetAlipayInfoById(shop);
            //if (!dbCoupon.CheckUserCanGetAlipay(trade, alipay))
            //{
            //    result = "卖家设置了红包【" + alipay.GUID + "】的最大领取数量是【" + alipay.Per + "】，此买家已经领取了【" + alipay.Per + "】张，所以不再赠送支付宝红包！";
            //    data.UpdateTradeRateResultAlipay(tradeRate, result);
            //    return false;
            //}

            FreeCardData dbFreeCard = new FreeCardData();
            //超出了包邮卡最大赠送量不赠送
            if (!dbFreeCard.CheckFreeCardCanSend(trade, shop))
            {
                result = "每个买家最多只能领取一张包邮卡，所以不再赠送包邮卡！";
                data.UpdateTradeRateResultFree(tradeRate, result);
                return false;
            }





            //包含了指定的关键字赠送或者不赠送
            if (shop.IsKeyword == "1")
            {
                //先判定字数是否满足条件
                if (tradeRate.Content.Length < int.Parse(shop.WordCount))
                {
                    result = "卖家开启了评价内容自动审核，此评价的字数是【" + tradeRate.Content.Length.ToString() + "】个字，小于卖家设置的最少评论字数【" + shop.WordCount + "】个字，所以不赠送包邮卡！";
                    data.UpdateTradeRateResultFree(tradeRate, result);
                    return false;
                }

                //判定是否为差评关键字判断
                if (shop.KeywordIsBad == "1")
                {
                    if (shop.BadKeyword != "")
                    {
                        int isInclude = 0;
                        string badword = string.Empty;
                        string[] keyArray = shop.BadKeyword.Split('|');
                        for (int i = 0; i < keyArray.Length; i++)
                        {
                            if (keyArray[i].Trim() != "")
                            {
                                if (tradeRate.Content.IndexOf(keyArray[i].Trim()) != -1)
                                {
                                    badword = keyArray[i].Trim();
                                    isInclude = 1;
                                    break;
                                }
                            }
                        }

                        //判定
                        if (isInclude == 1)
                        {
                            result = "卖家开启了评价内容自动审核，买家评价包含差评关键字【" + badword + "】，所以不赠送包邮卡！";
                            data.UpdateTradeRateResultFree(tradeRate, result);
                            return false;
                        }
                    }
                }
                else
                {
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
                            result = "卖家开启了评价内容自动审核，买家评价没有包含关键字，所以不赠送包邮卡！";
                            data.UpdateTradeRateResultFree(tradeRate, result);
                            return false;
                        }
                    }
                }
            }



            //该订单是否满足包邮卡赠送的地区限制
            FreeCard free = dbFreeCard.GetFreeCardById(shop.FreeCardId);
            string[] ary = free.AreaList.Split(',');
            string isok = "";

            if (free.IsFreeAreaList == "1")
            {
                //设置地区免运费
                for (int i = 0; i < ary.Length; i++)
                {
                    if (trade.receiver_state.IndexOf(ary[i]) != -1)
                    {
                        //记录免运费次数
                        return true;
                    }
                    isok += "1";
                }

                if (ary.Length > 0)
                {
                    result = "只有免运费的地区才赠送包邮卡，订单地区为【" + trade.receiver_state + "】，包邮地区为【" + free.AreaList + "】！";
                    data.UpdateTradeRateResultFree(tradeRate, result);
                    return false;
                }
            }
            else
            {
                //设置地区不免运费
                for (int i = 0; i < ary.Length; i++)
                {
                    if (trade.receiver_state.IndexOf(ary[i]) != -1)
                    {
                        result = "不免运费的包邮卡不赠送，订单地区为【" + trade.receiver_state + "】，不包邮地区为【" + ary[i] + "】！";
                        data.UpdateTradeRateResultFree(tradeRate, result);
                        return false;
                    }
                }
            }

            ////如果该支付宝红包已经过期
            //if (dbCoupon.CheckAlipayExpired(trade, alipay))
            //{
            //    result = "该支付宝红包已经过期，所以不再赠送！";
            //    data.UpdateTradeRateResultAlipay(tradeRate, result);
            //    return false;
            //}

            return true;
        }

        /// <summary>
        /// 订单消息
        /// </summary>
        private Trade TradeInfo { get; set; }
    }
}
