using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeteTopApi.Logic;
using System.Threading;
using System.Data;
using TeteTopApi;

namespace ActRateInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            //version1
            Thread newThread1 = new Thread(CheckNew1);
            newThread1.Start();

            //version2
            Thread newThread2 = new Thread(CheckNew2);
            newThread2.Start();

            //version3
            Thread newThread3 = new Thread(CheckNew3);
            newThread3.Start();
        }

        static void CheckNew1()
        {
            while (true)
            {
                string sql = "SELECT result,l.id FROM TCS_TaobaoMsgLog l INNER JOIN TCS_ShopSession s ON s.nick = l.nick WHERE isok = 0 AND l.typ = 'TradeRated' AND version = 1";
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
                        sql = "UPDATE TCS_TaobaoMsgLog SET isok = 2 WHERE id = " + dt.Rows[0][1].ToString();
                        utils.ExecuteNonQuery(sql);
                    }
                }
                //1S检查一次
                Thread.Sleep(1000);
            }
        }

        static void CheckNew2()
        {
            while (true)
            {
                string sql = "SELECT result,l.id FROM TCS_TaobaoMsgLog l INNER JOIN TCS_ShopSession s ON s.nick = l.nick WHERE isok = 0 AND l.typ = 'TradeRated' AND version = 2";
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
                        sql = "UPDATE TCS_TaobaoMsgLog SET isok = 2 WHERE id = " + dt.Rows[0][1].ToString();
                        utils.ExecuteNonQuery(sql);
                    }
                }
                //1S检查一次
                Thread.Sleep(1000);
            }
        }

        static void CheckNew3()
        {
            while (true)
            {
                string sql = "SELECT result,l.id FROM TCS_TaobaoMsgLog l INNER JOIN TCS_ShopSession s ON s.nick = l.nick WHERE isok = 0 AND l.typ = 'TradeRated' AND version = 3";
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
                        sql = "UPDATE TCS_TaobaoMsgLog SET isok = 2 WHERE id = " + dt.Rows[0][1].ToString();
                        utils.ExecuteNonQuery(sql);
                    }
                }
                //1S检查一次
                Thread.Sleep(1000);
            }
        }
    }
}
