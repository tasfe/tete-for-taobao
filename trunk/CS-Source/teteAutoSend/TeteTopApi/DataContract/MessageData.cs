using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeteTopApi.Entity;
using System.Data;

namespace TeteTopApi.DataContract
{
    public class MessageData
    {
        /// <summary>
        /// 记录卖家提醒短信
        /// </summary>
        /// <param name="shop"></param>
        /// <param name="trade"></param>
        /// <param name="msg"></param>
        /// <param name="result"></param>
        /// <param name="typ"></param>
        public void InsertShopAlertMsgLog(ShopInfo shop, string msg, string result, string typ)
        {
            //记录短信发送记录
            string sql = "INSERT INTO TCS_MsgSendShop (" +
                                "nick, " +
                                "buynick, " +
                                "mobile, " +
                                "[content], " +
                                "guoduid, " +
                                "num, " +
                                "typ " +
                            " ) VALUES ( " +
                                " '" + shop.Nick + "', " +
                                " '', " +
                                " '" + shop.Mobile + "', " +
                                " '" + msg.Replace("'", "''") + "', " +
                                " '" + result + "', " +
                                " '1', " +
                                " '" + typ + "' " +
                            ") ";
            Console.Write(sql + "\r\n");
            utils.ExecuteNonQuery(sql);
        }

        public bool IsSendMsgNearDays(Customer customer, string typ, string day)
        {
            string sql = "SELECT cguid FROM TCS_MsgSend WHERE nick = '" + customer.Nick + "' AND buynick = '" + customer.BuyNick + "' AND typ = '" + typ + "' AND DATEDIFF(d, adddate, GETDATE()) < " + day;
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 判断今天该短信是否发过
        /// </summary>
        /// <param name="trade"></param>
        /// <param name="typ"></param>
        /// <returns></returns>
        public bool IsSendMsgNearDays(ShopInfo shop, string typ)
        {
            string sql = "SELECT cguid FROM TCS_MsgSendShop WHERE nick = '" + shop.Nick + "' AND typ = '" + typ + "' AND DATEDIFF(d, adddate, GETDATE()) < 7";
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
