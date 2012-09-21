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
    public class TradeBuyerPay
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="trade"></param>
        public TradeBuyerPay(Trade trade) 
        {
            this.TradeInfo = trade;
        }

        /// <summary>
        /// 处理订单生成时的相应逻辑
        /// </summary>
        public void Start()
        {
            //获取店铺的基础数据
            ShopData data = new ShopData();
            ShopInfo shop = data.ShopInfoGetByNick(TradeInfo.Nick);

            //目前只做虚拟店铺的逻辑
            if (shop.IsXuni == "1")
            {
                //通过TOP接口查询该订单的详细数据并记录到数据库中
                TopApiHaoping api = new TopApiHaoping(shop.Session);
                //特殊判断催单订单不获取物流信息
                TradeInfo.Status = "CuiDan";
                Trade trade = api.GetTradeByTid(TradeInfo);

                //判断该订单是否存在
                TradeData dbTrade = new TradeData();
                if (!dbTrade.CheckTradeExits(trade))
                {
                    //更新该订单的评价时间
                    dbTrade.InsertTradeInfo(trade);
                }
            }
        }

        /// <summary>
        /// 订单消息
        /// </summary>
        private Trade TradeInfo { get; set; }
    }
}
