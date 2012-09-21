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
        private static object padlocktrade = new object();
        /// <summary>
        /// 记录卖家的已发货订单信息
        /// </summary>
        /// <param name="trade"></param>
        public void InsertTradeInfo(Trade trade)
        {
            string sql = "INSERT INTO TCS_Trade (" +
                                "nick, " +
                                "orderid, " +
                                "status, " +
                                "adddate, " +
                                "senddate, " +
                                "buynick, " +
                                "totalprice, " +
                                "iscoupon, " +
                                "couponprice, " +
                                "shippingshort, " +
                                "shippingnumber, " +
                                "ordertype, " +
                                "receiver_name, " +
                                "receiver_state, " +
                                "receiver_city, " +
                                "receiver_district, " +
                                "receiver_address, " +
                                "orderArea, " +
                                "mobile " +
                            " ) VALUES ( " +
                                " '" + trade.Nick + "', " +
                                " '" + trade.Tid + "', " +
                                " '" + trade.Status + "', " +
                                " '" + trade.Created + "', " +
                                " '" + trade.SendTime + "', " +
                                " '" + trade.BuyNick + "', " +
                                " '" + trade.OrderPrice + "', " +
                                " '" + trade.IsUseCoupon + "', " +
                                " '" + trade.CouponPrice + "', " +
                                " '" + trade.ShippingCompanyShort + "', " +
                                " '" + trade.ShippingNumber + "', " +
                                " '" + trade.OrderType + "', " +
                                " '" + trade.receiver_name + "', " +
                                " '" + trade.receiver_state + "', " +
                                " '" + trade.receiver_city + "', " +
                                " '" + trade.receiver_district + "', " +
                                " '" + trade.receiver_address + "', " +
                                " '" + trade.BuyerArea + "', " +
                                " '" + trade.Mobile + "' " +
                            ") ";
            Console.Write(sql + "\r\n");
            utils.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 更新该订单的评价为待审核状态
        /// </summary>
        /// <param name="trade"></param>
        public void UpdateTradeKefuById(Trade trade, TradeRate tradeRate)
        {
            string sql = "INSERT INTO TCS_TradeRateCheck (" +
                                "nick, " +
                                "orderid, " +
                                "adddate, " +
                                "reviewdate, " +
                                "buynick, " +
                                "result, " +
                                "content " +
                            " ) VALUES ( " +
                                " '" + trade.Nick + "', " +
                                " '" + trade.Tid + "', " +
                                " '" + trade.Created + "', " +
                                " '" + tradeRate.Created + "', " +
                                " '" + trade.BuyNick + "', " +
                                " '" + tradeRate.Result + "', " +
                                " '" + tradeRate.Content + "' " +
                            ") ";
            Console.Write(sql + "\r\n");
            utils.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 判断该客服评价审核信息是否已经记录过
        /// </summary>
        /// <param name="tradeRate"></param>
        /// <returns></returns>
        public bool CheckTradeRateCheckExits(Trade trade)
        {
            lock (padlocktrade)
            {
                bool boolResult = true;
                string sql = "SELECT orderid FROM TCS_TradeRateCheck WITH (NOLOCK) WHERE orderid = '" + trade.Tid + "' AND nick = '" + trade.Nick + "'";
                Console.Write(sql + "\r\n");
                DataTable dt = utils.ExecuteDataTable(sql);
                if (dt.Rows.Count == 0)
                {
                    boolResult = false;
                }

                return boolResult;
            }
        }

        /// <summary>
        /// 更新订单的促销相关信息
        /// </summary>
        /// <param name="trade"></param>
        /// <param name="typ"></param>
        /// <param name="status"></param>
        public void UpdateTradeCouponInfo(Trade trade, string price, string couponid)
        {
            string sql = "SELECT COUNT(*) FROM TCS_CouponSend WHERE taobaonumber = '" + couponid + "'";
            string count = utils.ExecuteString(sql);
            if (count == "0")
            {
                sql = "UPDATE TCS_Trade SET iscoupon = 2,couponprice='" + price + "',couponnumber ='" + couponid + "' WHERE orderid = '" + trade.Tid + "'";
            }
            else
            {
                sql = "UPDATE TCS_Trade SET iscoupon = 1,couponprice='" + price + "',couponnumber ='" + couponid + "' WHERE orderid = '" + trade.Tid + "'";
            }

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
            string sql = "UPDATE TCS_Trade SET typ = 'self', shippingstatus = '" + status + "',shippingshort = '" + trade.ShippingCompanyName + "',shippingnumber= '" + trade.ShippingNumber + "' WHERE orderid = '" + trade.Tid + "'";
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
            string sql = "UPDATE TCS_Trade SET typ = 'system', shippingstatus = '" + status + "', shippingdate = '" + trade.DeliveryEnd + "',shippingshort = '" + trade.ShippingCompanyName + "',shippingnumber= '" + trade.ShippingNumber + "' WHERE orderid = '" + trade.Tid + "'";
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
            string sql = "UPDATE TCS_Trade WITH (ROWLOCK) SET reviewdate='" + tradeRate.Created + "',status='TRADE_FINISHED' WHERE orderid = '" + trade.Tid + "'";
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
            lock (padlocktrade)
            {
                bool boolResult = true;
                string sql = "SELECT orderid FROM TCS_Trade WITH (NOLOCK) WHERE orderid = '" + trade.Tid + "' AND nick = '" + trade.Nick + "'";
                Console.Write(sql + "\r\n");
                DataTable dt = utils.ExecuteDataTable(sql);
                if (dt.Rows.Count == 0)
                {
                    boolResult = false;
                }

                return boolResult;
            }
        }

        /// <summary>
        /// 获取卖家已发货且未确认的订单
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        public List<Trade> GetShippingTrade(ShopInfo shop)
        {
            //string sql = "SELECT * FROM TCS_Trade WHERE nick = '" + shop.Nick + "' AND orderstatus <> 'WAIT_SELLER_SEND_GOODS' AND orderstatus <> 'WAIT_BUYER_PAY' AND orderstatus <> 'TradeRated' ";
            string sql = "SELECT * FROM TCS_Trade WHERE nick = '" + shop.Nick + "' AND (typ IS NULL OR (typ = 'system' AND shippingdate IS NULL AND shippingstatus <> 'ACCEPTED_BY_RECEIVER')) AND reviewdate IS NULL AND mobile <> ''";
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
            string sql = "SELECT * FROM TCS_Trade WHERE (nick = '" + shop.Nick + "' AND typ = 'system' AND reviewdate IS NULL AND DATEDIFF(d, shippingdate, GETDATE()) = " + shop.MinDateSystem + ") OR (nick = '" + shop.Nick + "' AND typ = 'self' AND reviewdate IS NULL AND DATEDIFF(d, senddate, GETDATE()) = " + shop.MinDateSelf + ")";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);

            return FormatDataList(dt);
        }

        /// <summary>
        /// 获取指定卖家的长期未确认的订单
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        public List<Trade> GetUnconfirmTradeXuni(ShopInfo shop)
        {
            string sql = "SELECT t.nick,t.buynick,t.orderid,t.ordertype,c.mobile FROM TCS_Trade t INNER JOIN TCS_Customer c ON c.buynick = t.buynick AND c.mobile <> '' WHERE t.nick = '" + shop.Nick + "' AND reviewdate IS NULL AND DATEDIFF(MINUTE, adddate, GETDATE()) > " + shop.XuniDate + " AND DATEDIFF(MINUTE, adddate, GETDATE()) < " + (int.Parse(shop.XuniDate) + 30).ToString() + " AND t.status <> 'TRADE_FINISHED'";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);

            return FormatDataList(dt);
        }

        /// <summary>
        /// 获取指定卖家的长期未确认的订单
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        public List<Trade> GetUnconfirmTradeTest(ShopInfo shop)
        {
            string sql = "SELECT * FROM TCS_Trade WHERE (nick = '" + shop.Nick + "' AND typ = 'system' AND reviewdate IS NULL AND DATEDIFF(d, shippingdate, GETDATE()) >= " + shop.MinDateSystem + " AND DATEDIFF(d, shippingdate, GETDATE()) <= " + (int.Parse(shop.MinDateSystem) + 4).ToString() + ") OR (nick = '" + shop.Nick + "' AND typ = 'self' AND reviewdate IS NULL AND DATEDIFF(d, senddate, GETDATE()) >= " + shop.MinDateSelf + " AND DATEDIFF(d, senddate, GETDATE()) <= " + (int.Parse(shop.MinDateSelf) + 4).ToString() + ")";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);

            return FormatDataList(dt);
        }

        /// <summary>
        /// 获取指定卖家的全部订单
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        public List<Trade> GetTradeAllByNick(ShopInfo shop)
        {
            string sql = "SELECT * FROM TCS_Trade WHERE nick = '" + shop.Nick + "'";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);

            return FormatDataList(dt);
        }


        /// <summary>
        /// 判断今天该短信是否发过
        /// </summary>
        /// <param name="trade"></param>
        /// <param name="typ"></param>
        /// <returns></returns>
        public bool IsTaobaoCompany(Trade trade)
        {
            string sql = "SELECT id FROM TCS_TaobaoShippingCompany WHERE name = '" + trade.ShippingCompanyName + "'";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 订单是否包含指定商品判断
        /// </summary>
        /// <param name="trade"></param>
        /// <param name="shop"></param>
        /// <returns></returns>
        public bool IsIncludeProduct(Trade trade, ShopInfo shop)
        {
            string[] tradeProduct = trade.NumIidList;
            string includeProduct = shop.IncludeProductList;

            //如果没设置商品则不生效
            if (shop.IncludeProductList.Length == 0)
            {
                return false;
            }

            //如果是包含商品就送
            if (shop.IncludeProductFlag == "0")
            {
                for (int i = 0; i < tradeProduct.Length; i++)
                {
                    if (includeProduct.IndexOf("," + tradeProduct[i]) != -1)
                    {
                        return true;
                    }
                }
            }

            //如果是不包含商品就送
            if (shop.IncludeProductFlag == "1")
            {
                for (int i = 0; i < tradeProduct.Length; i++)
                {
                    if (includeProduct.IndexOf("," + tradeProduct[i]) != -1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public Trade GetTradeDetailShippingInfo(Trade trade)
        {
            string sql = "SELECT * FROM TCS_Trade WHERE orderid = '" + trade.Tid + "'";
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                trade.SendTime = dt.Rows[0]["senddate"].ToString();
                trade.DeliveryEnd = dt.Rows[0]["shippingdate"].ToString();
                trade.ShippingType = dt.Rows[0]["typ"].ToString();
            }
            return trade;
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

                info.Mobile = dt.Rows[i]["mobile"].ToString();
                info.BuyNick = dt.Rows[i]["buynick"].ToString();
                info.Tid = dt.Rows[i]["orderid"].ToString();
                info.Nick = dt.Rows[i]["nick"].ToString();
                info.OrderType = dt.Rows[i]["ordertype"].ToString();

                infoList.Add(info);
            }
            return infoList;
        }
    }
}
