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
    public class TradeCreate
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="trade"></param>
        public TradeCreate(Trade trade) 
        {
            this.TradeInfo = trade;
        }

        /// <summary>
        /// 处理订单生成时的相应逻辑
        /// </summary>
        public string Start()
        {
            ShopData data = new ShopData();
            ShopInfo shop = data.ShopInfoGetByNick(TradeInfo.Nick);

            if (shop.Version != "2" && shop.Version != "3")
            {
                return "1";
            }

            //通过TOP接口查询该订单的详细数据并记录到数据库中
            TopApiHaoping api = new TopApiHaoping(shop.Session);

            //处理包邮卡逻辑
            if (IsFreePost(TradeInfo, shop))
            {
                //通过API免运费
                api.FreeTradePost(TradeInfo);

                //记录免邮订单方便统计销售
            }

            //如果该客户没短信直接推出
            if (int.Parse(shop.MsgCount) <= 0)
            {
                return "1";
            }

            //判断该客户是否开启了催单短信
            if (data.IsCuiByShop(TradeInfo))
            { 
                //获取小时数
                string timecount = data.GetCuiDateByShop(shop);
                //Console.WriteLine(timecount);
                //if (timecount != "0")
                //{
                    //判断下单时间离现在有多久
                    TimeSpan ts1 = new TimeSpan(DateTime.Now.Ticks);
                    TimeSpan ts2 = new TimeSpan(DateTime.Parse(TradeInfo.Modified).Ticks);
                    TimeSpan ts = ts1.Subtract(ts2).Duration();
                    string minutes = ((int)ts.TotalMinutes).ToString();

                    Console.Write(TradeInfo.Tid + "-" + minutes + "....\r\n");
                    if (int.Parse(minutes) > int.Parse(timecount))
                    {
                        if (int.Parse(shop.MsgCount) > 0)
                        {
                            //判断同类型的短信该客户今天是否只收到一条
                            ShopData db = new ShopData();
                            if (!db.IsSendMsgToday(TradeInfo, "cui"))
                            {
                                //特殊判断催单订单不获取物流信息
                                TradeInfo.Status = "CuiDan";
                                Trade trade = api.GetTradeByTid(TradeInfo);
                                //如果是货到交易付款则不发送催单短信并结束改消息
                                if (trade.OrderType.ToLower() == "cod")
                                {
                                    return "1";
                                }

                                Console.WriteLine(trade.Status.ToUpper());
                                //如果该订单已付款则取消发送
                                if (trade.Status.ToUpper() != "WAIT_BUYER_PAY")
                                {
                                    return "1";
                                }

                                //晚上12点到早上10点不发
                                if (DateTime.Now.Hour < 10)
                                {
                                    return "0";
                                }

                                //发送短信
                                string msg = data.GetCuiContentByShop(shop);
                                msg = Message.GetMsg(msg, shop.MsgShopName, "", "");
                                //Console.Write(msg + "...\r\n");
                                string msgResult = Message.Send(trade.Mobile, msg);

                                //记录
                                if (msgResult != "0")
                                {
                                    db.InsertShopMsgLog(shop, trade, msg, msgResult, "cui");
                                }
                                else
                                {
                                    db.InsertShopErrMsgLog(shop, trade, msg, msgResult, "cui");
                                }
                                shop.MsgCount = (int.Parse(shop.MsgCount) - 1).ToString();
                            }
                        }
                    }
                    else
                    {
                        return "0";
                    }
                //}
            }

            return "1";
        }

        /// <summary>
        /// 判断是否给该订单免运费
        /// </summary>
        /// <param name="TradeInfo"></param>
        /// <returns></returns>
        private bool IsFreePost(Trade trade, ShopInfo shop)
        {
            //卖家是否订购了包邮卡服务
            string plus = shop.Plus;
            if (string.IsNullOrEmpty(plus))
            {
                //中断
                return false;
            }
            else
            {
                if (plus.IndexOf("freecard") == -1)
                {
                    return false;
                }
            }
            FreeCardData data = new FreeCardData();

            //如果该订单已经包邮过
            if (data.CheckOrderIsFree(trade))
            {
                return false;
            }

            //该买家是否有能用的包邮卡
            string guid = data.GetUserFreeCard(trade);
            if (guid.Length == 0)
            {
                //中断
                return false;
            }

            //该订单是否满足包邮卡设定的金额
            string freeCardPrice = data.GetFreeCardPrice(guid);
            TopApiHaoping api = new TopApiHaoping(shop.Session);
            trade = api.GetTradeByTid(trade);
            if (decimal.Parse(trade.OrderPrice) < decimal.Parse(freeCardPrice))
            {
                //如果不满足中断
                return false;
            }

            //该订单是否满足包邮卡的地区限制
            FreeCard free = data.GetUserFreeCardById(guid);
            string[] ary = free.AreaList.Split(',');
            if (free.AreaList.Length != 0)
            {
                if (free.IsFreeAreaList == "1")
                {
                    //设置地区免运费
                    for (int i = 0; i < ary.Length; i++)
                    {
                        if (trade.receiver_state.IndexOf(ary[i]) != -1)
                        {
                            //记录免运费次数
                            data.RecordFreeCardLog(guid, trade);
                            return true;
                        }
                    }
                }
                else
                {
                    //设置地区不免运费
                    for (int i = 0; i < ary.Length; i++)
                    {
                        if (trade.receiver_state.IndexOf(ary[i]) != -1)
                        {
                            return false;
                        }
                    }
                }
            }

            //记录免运费次数
            data.RecordFreeCardLog(guid, trade);
            return true;
        }

        /// <summary>
        /// 订单消息
        /// </summary>
        private Trade TradeInfo { get; set; }
    }
}
