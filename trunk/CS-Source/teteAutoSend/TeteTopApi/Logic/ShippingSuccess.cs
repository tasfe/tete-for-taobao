using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeteTopApi.DataContract;
using TeteTopApi.Entity;
using TeteTopApi.TopApi;

namespace TeteTopApi.Logic
{
    public class ShippingSuccess
    {
        public void Start()
        {
            //获取目前正在使用的卖家
            ShopData dbShop = new ShopData();
            List<ShopInfo> list = dbShop.GetShopInfoListShippingAlert();

            //循环判定这些卖家的订单是否物流到货
            for (int i = 0; i < list.Count; i++)
            {
                ShopInfo shop = list[i];
                //获取那些状态是发货中且没发过短信的订单给予提示
                TradeData dbTrade = new TradeData();
                List<Trade> listTrade = dbTrade.GetShippingTrade(shop);

                Console.Write(listTrade.Count.ToString() + "\r\n");
                for (int j = 0; j < listTrade.Count; j++)
                {
                    Trade trade = listTrade[j];
                    //获取该订单的物流状态
                    TopApiHaoping api = new TopApiHaoping(shop.Session);
                    string status = api.GetShippingStatusByTid(listTrade[j]);

                    Console.Write(status + "\r\n");
                    //如果该物流信息不存在
                    if (status.IndexOf("不存在") != -1)
                    {
                        //如果该物流公司不支持查询则更新为self
                        dbTrade.UpdateTradeShippingStatusSelf(trade, status);
                        continue;
                    }

                    //根据服务器的物流状态进行判断，如果物流状态是已签收
                    if (status == "ACCEPTED_BY_RECEIVER")
                    {
                        string result = api.GetShippingStatusDetailByTid(trade);
                        Console.Write("【" + result + "】\r\n");
                        //如果订单不是服务器错误
                        if (result.IndexOf("company-not-support") != -1)
                        {
                            //如果该物流公司不支持查询则更新为self
                            dbTrade.UpdateTradeShippingStatusSelf(trade, status);
                            continue;
                        }

                        //再根据订单的详细物流信息判断签收的状态
                        if (result.IndexOf("签收人") != -1 || result.IndexOf("正常签收录入扫描") != -1)
                        {
                            //如果物流已经签收了则更新对应订单状态
                            trade.DeliveryEnd = utils.GetShippingEndTime(result); ;
                            trade.DeliveryMsg = result;
                            dbTrade.UpdateTradeShippingStatusSystem(trade, status);

                            //发送短信
                            //判断同类型的短信该客户今天是否只收到一条
                            ShopData db = new ShopData();
                            if (!db.IsSendMsgOrder(trade, "shipping"))
                            {
                                //判断该用户是否开启了发货短信
                                if (shop.MsgIsShipping == "1" && int.Parse(shop.MsgCount) > 0)
                                {
                                    //发送短信
                                    string msg = Message.GetMsg(shop.MsgShippingContent, shop.MsgShopName, trade.BuyNick, shop.MsgIsFahuo);
                                    string msgResult = Message.Send(trade.Mobile, msg);

                                    //记录
                                    if (msgResult != "0")
                                    {
                                        db.InsertShopMsgLog(shop, trade, msg, msgResult, "shipping");
                                    }
                                    else
                                    {
                                        db.InsertShopErrMsgLog(shop, trade, msg, msgResult, "shipping");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

    }
}
