using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeteTopApi.Logic;
using System.Threading;
using System.Data;
using TeteTopApi;

namespace ActShippingInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                string sql = "SELECT result,id FROM TCS_TaobaoMsgLog WHERE isok = 0 AND typ = 'TradeSellerShip'";
                DataTable dt = utils.ExecuteDataTable(sql);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        string result = dt.Rows[i][0].ToString();
                        ReceiveMessage msg = new ReceiveMessage(result.ToString());
                        msg.ActData();
                        sql = "UPDATE TCS_TaobaoMsgLog SET isok = 1 WHERE id = " + dt.Rows[i][1].ToString();
                        Console.Write(sql + "\r\n");
                        utils.ExecuteNonQuery(sql);

                        sql = "INSERT INTO TCS_TaobaoMsgLogBak SELECT * FROM [TCS_TaobaoMsgLog] WHERE id = " + dt.Rows[i][1].ToString();
                        Console.Write(sql + "\r\n");
                        utils.ExecuteNonQuery(sql);

                        sql = "DELETE FROM TCS_TaobaoMsgLog WHERE id = " + dt.Rows[i][1].ToString();
                        Console.Write(sql + "\r\n");
                        utils.ExecuteNonQuery(sql);
                    }
                    catch
                    {
                        sql = "UPDATE TCS_TaobaoMsgLog SET isok = 2 WHERE id = " + dt.Rows[i][1].ToString();
                        Console.Write(sql + "\r\n");
                        utils.ExecuteNonQuery(sql);
                    }
                }
                //1S检查一次
                Thread.Sleep(1000);
            }
        }
    }
}
