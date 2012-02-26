using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeteTopApi.Entity;
using System.Data;

namespace TeteTopApi.DataContract
{
    public class LogData
    {
        /// <summary>
        /// 记录该优惠券的赠送信息
        /// </summary>
        /// <param name="trade"></param>
        /// <param name="shop"></param>
        /// <param name="couponId"></param>
        public void InsertMsgLogInfo(string nick, string typ, string result)
        {
            string sql = "INSERT INTO TCS_TaobaoMsgLog (" +
                                "nick, " +
                                "typ, " +
                                "result " +
                            " ) VALUES ( " +
                                " '" + nick + "', " +
                                " '" + typ + "', " +
                                " '" + result + "' " +
                            ") ";
            Console.Write(sql + "\r\n");
            utils.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 插入错误日志
        /// </summary>
        /// <param name="nick"></param>
        /// <param name="typ"></param>
        /// <param name="sqlDebug"></param>
        /// <param name="message"></param>
        public void InsertErrorLog(string nick, string typ, string sqlDebug, string message, string error)
        {
            string sql = "INSERT INTO TCS_ErrorLog (" +
                                "nick, " +
                                "typ, " +
                                "sql, " +
                                "error, " +
                                "message " +
                            " ) VALUES ( " +
                                " '" + nick + "', " +
                                " '" + typ + "', " +
                                " '" + sqlDebug + "', " +
                                " '" + error + "', " +
                                " '" + message + "' " +
                            ") ";
            Console.Write(sql + "\r\n");
            utils.ExecuteNonQuery(sql);
        }
    }
}
