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
            string sql = "INSERT INTO TopTradeRate (" +
                                "tid, " +
                                "oid, " +
                                "content, " +
                                "created, " +
                                "nick, " +
                                "owner, " +
                                "itemid, " +
                                "result " +
                            " ) VALUES ( " +
                                " '" + tradeRate.Tid + "', " +
                                " '" + tradeRate.Oid + "', " +
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
        /// 判断该评价信息是否已经记录过
        /// </summary>
        /// <param name="tradeRate"></param>
        /// <returns></returns>
        public bool CheckTradeRateExits(TradeRate tradeRate)
        {
            string sql = "SELECT id FROM TopTradeRate WITH (NOLOCK) WHERE tid = '" + tradeRate.Tid + "' AND nick = '" + tradeRate.BuyNick + "' AND owner = '" + tradeRate.Nick + "'";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count == 0)
            {
                return false;
            }

            return true;
        }
    }
}
