using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeteTopApi.Entity;
using System.Data;

namespace TeteTopApi.DataContract
{
    public class TradeData
    {
        /// <summary>
        /// 记录卖家的已发货订单信息
        /// </summary>
        /// <param name="trade"></param>
        public void InsertTradeInfo(Trade trade)
        {
            string sql = "INSERT INTO TopOrder (" +
                                "nick, " +
                                "orderid, " +
                                "orderstatus, " +
                                "addtime, " +
                                "paytime, " +
                                "buynick, " +
                                "receiver_mobile " +
                            " ) VALUES ( " +
                                " '" + trade.Nick + "', " +
                                " '" + trade.Tid + "', " +
                                " '" + trade.Status + "', " +
                                " '" + trade.Created + "', " +
                                " '" + trade.SendTime + "', " +
                                " '" + trade.BuyNick + "', " +
                                " '" + trade.Mobile + "' " +
                            ") ";
            Console.Write(sql + "\r\n");
            utils.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 更新该订单的评价为待审核状态
        /// </summary>
        /// <param name="trade"></param>
        public void UpdateTradeKefuById(Trade trade)
        {
            string sql = "UPDATE TopOrder WITH (ROWLOCK) SET issend = 2 WHERE orderid = '" + trade.Tid + "'";
            Console.Write(sql + "\r\n");
            utils.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 更新订单的物流状态self
        /// </summary>
        /// <param name="trade"></param>
        /// <param name="typ"></param>
        /// <param name="status"></param>
        public void UpdateTradeShippingStatusSelf(Trade trade, string status)
        {
            string sql = "UPDATE TopOrder SET typ = 'self', shippingstatus = '" + status + "' WHERE orderid = '" + trade.Tid + "'";
            Console.Write(sql + "\r\n");
            utils.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 更新订单的物流状态system
        /// </summary>
        /// <param name="trade"></param>
        /// <param name="typ"></param>
        /// <param name="status"></param>
        public void UpdateTradeShippingStatusSystem(Trade trade, string status)
        {
            string sql = "UPDATE TopOrder SET typ = 'system', shippingstatus = '" + status + "', delivery_end = '" + trade.DeliveryEnd + "', deliverymsg = '" + trade.DeliveryMsg + "', isdeliverymsg = 1  WHERE orderid = '" + trade.Tid + "'";
            Console.Write(sql + "\r\n");
            utils.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 更新该订单的评价时间和评价内容
        /// </summary>
        /// <param name="trade"></param>
        /// <param name="tradeRate"></param>
        public void UpdateTradeRateById(Trade trade, TradeRate tradeRate)
        {
            string sql = "UPDATE TopOrder WITH (ROWLOCK) SET isok = 1, reviewtime='" + tradeRate.Created + "',result='" + tradeRate.Result + "',content='" + tradeRate.Content + "' WHERE orderid = '" + trade.Tid + "'";
            Console.Write(sql + "\r\n");
            utils.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 判断该评价信息是否已经记录过
        /// </summary>
        /// <param name="tradeRate"></param>
        /// <returns></returns>
        public bool CheckTradeExits(Trade trade)
        {
            string sql = "SELECT orderid FROM TopOrder WITH (NOLOCK) WHERE orderid = '" + trade.Tid + "' AND nick = '" + trade.Nick + "'";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取卖家已发货且未确认的订单
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        public List<Trade> GetShippingTrade(ShopInfo shop)
        {
            string sql = "SELECT * FROM TopOrder WHERE nick = '" + shop.Nick + "' AND isok = 0 AND orderstatus <> 'WAIT_SELLER_SEND_GOODS' AND orderstatus <> 'WAIT_BUYER_PAY' AND orderstatus <> 'TradeRated' AND isdeliverymsg = 0";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);

            return FormatDataList(dt);
        }

        /// <summary>
        /// 获取指定卖家的长期未确认的订单
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        public List<Trade> GetUnconfirmTrade(ShopInfo shop)
        {
            string sql = "SELECT * FROM TopOrder WHERE (nick = '" + shop.Nick + "' AND typ = 'system' AND delivery_start IS NOT NULL AND (orderstatus = 'WAIT_BUYER_CONFIRM_GOODS' OR orderstatus = 'TradeSellerShip') AND DATEDIFF(d, delivery_start, GETDATE()) = " + shop.MinDateSystem + ") OR  (nick = '" + shop.Nick + "' AND (typ = 'self' OR delivery_start IS NULL) AND (orderstatus = 'WAIT_BUYER_CONFIRM_GOODS' OR orderstatus = 'TradeSellerShip') AND DATEDIFF(d, addtime, GETDATE()) = " + shop.MinDateSelf + ") AND istellmsg = 0";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);

            return FormatDataList(dt);
        }


        /// <summary>
        /// 生成新格式的店铺基本设置数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<Trade> FormatDataList(DataTable dt)
        {
            List<Trade> infoList = new List<Trade>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Trade info = new Trade();

                info.Mobile = dt.Rows[i]["receiver_mobile"].ToString();
                info.BuyNick = dt.Rows[i]["buynick"].ToString();
                info.Tid = dt.Rows[i]["orderid"].ToString();
                info.Nick = dt.Rows[i]["nick"].ToString();

                infoList.Add(info);
            }
            return infoList;
        }
    }
}
