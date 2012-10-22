using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeteTopApi.Entity;
using System.Data;

namespace TeteTopApi.DataContract
{
    public class TradeRateData
    {
        /// <summary>
        /// 记录评价信息
        /// </summary>
        /// <param name="tradeRate"></param>
        public void InsertTradeInfo(TradeRate tradeRate)
        {
            string sql = "INSERT INTO TCS_TradeRate (" +
                                "orderid, " +
                                "content, " +
                                "reviewdate, " +
                                "buynick, " +
                                "nick, " +
                                "itemid, " +
                                "result " +
                            " ) VALUES ( " +
                                " '" + tradeRate.Tid + "', " +
                                " '" + tradeRate.Content + "', " +
                                " '" + tradeRate.Created + "', " +
                                " '" + tradeRate.BuyNick + "', " +
                                " '" + tradeRate.Nick + "', " +
                                " '" + tradeRate.ItemId + "', " +
                                " '" + tradeRate.Result + "' " +
                            ") ";
            Console.Write(sql + "\r\n");
            utils.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 记录优惠券赠送的结果
        /// </summary>
        /// <param name="tradeRate"></param>
        /// <param name="result"></param>
        public void UpdateTradeRateResult(TradeRate tradeRate, string result)
        {
            string sql = "UPDATE TCS_TradeRate SET sendresult = '" + result + "' WHERE orderid = '" + tradeRate.Tid + "'";
            utils.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 记录淘宝红包赠送的结果
        /// </summary>
        /// <param name="tradeRate"></param>
        /// <param name="result"></param>
        public void UpdateTradeRateResultAlipay(TradeRate tradeRate, string result)
        {
            string sql = "UPDATE TCS_TradeRate SET sendresultalipay = '" + result + "' WHERE orderid = '" + tradeRate.Tid + "'";
            utils.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 记录包邮卡赠送的结果
        /// </summary>
        /// <param name="tradeRate"></param>
        /// <param name="result"></param>
        public void UpdateTradeRateResultFree(TradeRate tradeRate, string result)
        {
            string sql = "UPDATE TCS_TradeRate SET sendresultfree = '" + result + "' WHERE orderid = '" + tradeRate.Tid + "'";
            utils.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 判断该评价信息是否已经记录过
        /// </summary>
        /// <param name="tradeRate"></param>
        /// <returns></returns>
        public bool CheckTradeRateExits(TradeRate tradeRate)
        {
            string sql = "SELECT orderid FROM TCS_TradeRate WITH (NOLOCK) WHERE orderid = '" + tradeRate.Tid + "' AND buynick = '" + tradeRate.BuyNick + "' AND nick = '" + tradeRate.Nick + "'";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取用户账户中的未审核评价数量
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        public string GetUncheckedTradeRateCount(ShopInfo shop)
        {
            string sql = "SELECT COUNT(*) FROM TCS_TradeRateCheck WHERE nick = '" + shop.Nick + "' AND ischeck = 0";
            Console.Write(sql + "\r\n");
            string result = utils.ExecuteString(sql);

            return result;
        }
    }
}
