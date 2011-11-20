using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeteTopApi.Entity;
using TeteTopApi.DataContract;
using TeteTopApi.TopApi;

namespace TeteTopApi.Logic
{
    public class TradeSellerShip
    {
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

            //通过TOP接口查询该订单的详细数据并记录到数据库中
            TopApiHaoping api = new TopApiHaoping(shop.Session);
            Trade trade = api.GetTradeByTid(TradeInfo);

            //判断该订单是否存在
            TradeData dbTrade = new TradeData();
            if (!dbTrade.CheckTradeExits(trade))
            {
                //更新该订单的评价时间
                dbTrade.InsertTradeInfo(trade);
            }

            //判断该用户是否开启了发货短信
            if (shop.MsgIsFahuo == "1" && int.Parse(shop.MsgCount) > 0)
            {
                //判断同类型的短信该客户今天是否只收到一条
                ShopData db = new ShopData();
                if (!db.IsSendMsgToday(trade, "fahuo"))
                {
                    //发送短信
                    string msg = Message.GetMsg(shop.MsgFahuoContent, shop.MsgShopName, TradeInfo.BuyNick, shop.MsgIsFahuo);
                    string msgResult = Message.Send(trade.Mobile, msg);

                    //记录
                    if (msgResult != "0")
                    {
                        db.InsertShopMsgLog(shop, trade, msg, msgResult, "fahuo");
                    }
                    else
                    {
                        db.InsertShopErrMsgLog(shop, trade, msg, msgResult, "fahuo");
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
