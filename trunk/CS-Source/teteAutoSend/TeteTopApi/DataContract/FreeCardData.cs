using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeteTopApi.Entity;
using System.Data;

namespace TeteTopApi.DataContract
{
    public class FreeCardData
    {
        /// <summary>
        /// 更新包邮卡使用次数
        /// </summary>
        /// <param name="guid"></param>
        public void RecordFreeCardLog(string guid, Trade trade)
        {
            string sql = "UPDATE TCS_FreeCard SET usecount = usecount + 1 WHERE guid = '" + guid + "'";
            Console.Write(sql + "\r\n");
            utils.ExecuteNonQuery(sql);

            sql = "INSERT INTO TCS_FreeCardLog(freecardid,nick,buynick,orderid) VALUES ('" + guid + "','" + trade.Nick + "','" + trade.BuyNick + "','" + trade.Tid + "')";
            Console.Write(sql + "\r\n");
            utils.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 获取买家是否用可用的包邮卡，有就返回包邮卡ID，没有返回空
        /// </summary>
        /// <param name="trade"></param>
        /// <returns></returns>
        public string GetUserFreeCard(Trade trade)
        {
            string sql = "SELECT guid FROM TCS_FreeCard WHERE nick = '" + trade.Nick + "' AND buynick = '" + trade.BuyNick + "' AND Enddate > GETDATE() AND usecount < usecountlimit";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
        }

        public bool CheckOrderIsFree(Trade trade)
        {
            string sql = "SELECT COUNT(*) FROM TCS_FreeCardLog WHERE nick = '" + trade.Nick + "' AND buynick = '" + trade.BuyNick + "' AND orderid = '" + trade.Tid + "'";
            Console.Write(sql + "\r\n");
            string count = utils.ExecuteString(sql);

            if (count == "0")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
