using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeteTopApi.DataContract;
using TeteTopApi.Entity;

namespace TeteTopApi.Logic
{
    public class WaitSellerConfirm
    {
        private static object padlock4 = new object();

        public void Start()
        {
            //获取目前正在使用的卖家
            ShopData dbShop = new ShopData();
            List<ShopInfo> list = dbShop.GetShopInfoListAlert();

            //循环判定卖家是否到提醒发货时间了
            Console.Write(list.Count.ToString() + "\r\n");
            for (int i = 0; i < list.Count; i++)
            {
                ShopInfo shop = list[i];
                Console.Write(shop.Nick + "-" + shop.MsgReviewTime + "-" + DateTime.Now.Hour.ToString() + "\r\n");
                if (DateTime.Now.Hour.ToString() == list[i].MsgReviewTime || shop.IsXuni == "1")
                { 
                    //获取那些延迟不确认且没发过短信的订单给予提示
                    TradeData dbTrade = new TradeData();
                    List<Trade> listTrade = new List<Trade>();

                    if (shop.IsXuni == "1")
                    {
                        //如果是虚拟商品
                        listTrade = dbTrade.GetUnconfirmTradeXuni(shop);
                    }
                    else
                    {
                        listTrade = dbTrade.GetUnconfirmTrade(shop);
                    }

                    Console.Write(listTrade.Count.ToString() + "\r\n");
                    for (int j = 0; j < listTrade.Count; j++)
                    {
                        Trade trade = listTrade[j];

                        //判断如果是分销的订单，则不处理
                        if (trade.OrderType.ToLower() == "fenxiao")
                        {
                            return;
                        }

                        //发送短信-上LOCK锁定
                        lock (padlock4)
                        {
                            //判断同类型的短信该客户今天是否只收到一条
                            ShopData db = new ShopData();
                            if (!db.IsSendMsgOrder(trade, "review") && !db.IsSendMsgToday(trade, "review"))
                            {
                                //判断该用户是否开启了发货短信
                                if (shop.MsgIsReview == "1" && int.Parse(shop.MsgCount) > 0)
                                {
                                    //发送短信
                                    //string msg = Message.GetMsg(shop.MsgReviewContent, shop.MsgShopName, trade.BuyNick, shop.IsCoupon);
                                    string msg = Message.GetMsg(shop, trade, shop.MsgReviewContent);
                                    string msgResult = Message.Send(trade.Mobile, msg);

                                    //记录
                                    if (msgResult != "0")
                                    {
                                        db.InsertShopMsgLog(shop, trade, msg, msgResult, "review");
                                    }
                                    else
                                    {
                                        db.InsertShopErrMsgLog(shop, trade, msg, msgResult, "review");
                                    }
                                    shop.MsgCount = (int.Parse(shop.MsgCount) - 1).ToString();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
