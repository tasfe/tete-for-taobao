using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeteTopApi.Logic;
using System.Threading;
using System.Data;
using TeteTopApi;

namespace ActUnpayTrade2
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    while (true)
        //    {
        //        string sql = "SELECT result,id FROM TCS_TaobaoMsgLog WHERE isok = 0 AND typ = 'TradeCreate'";// AND nick = '美杜莎之心'";
        //        DataTable dt = utils.ExecuteDataTable(sql);

        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            try
        //            {
        //                string result = dt.Rows[i][0].ToString();
        //                Console.Write(result + "...\r\n");
        //                ReceiveMessage msg = new ReceiveMessage(result.ToString());
        //                string ret = msg.ActDataResult();
        //                if (ret == "1")
        //                {
        //                    sql = "UPDATE TCS_TaobaoMsgLog SET isok = 1 WHERE id = " + dt.Rows[i][1].ToString();
        //                    Console.Write(sql + "...\r\n");
        //                    utils.ExecuteNonQuery(sql);

        //                    sql = "INSERT INTO TCS_TaobaoMsgLogBak SELECT * FROM [TCS_TaobaoMsgLog] WHERE id = " + dt.Rows[i][1].ToString();
        //                    Console.Write(sql + "...\r\n");
        //                    utils.ExecuteNonQuery(sql);

        //                    sql = "DELETE FROM TCS_TaobaoMsgLog WHERE id = " + dt.Rows[i][1].ToString();
        //                    Console.Write(sql + "...\r\n");
        //                    utils.ExecuteNonQuery(sql);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                sql = "UPDATE TCS_TaobaoMsgLog SET isok = 2 WHERE id = " + dt.Rows[i][1].ToString();
        //                utils.ExecuteNonQuery(sql);
        //                Console.Write(ex.Message.ToString() + "...\r\n");
        //                Console.Write(ex.Source.ToString() + "...\r\n");
        //                Console.Write(ex.StackTrace.ToString() + "...\r\n");
        //            }
        //        }
        //        //1S检查一次
        //        Thread.Sleep(5000);
        //    }
        //}


        static void Main(string[] args)
        {
            ThreadPool.SetMaxThreads(10, 100);

            //version1
            Thread newThread1 = new Thread(CheckNew1);
            newThread1.Start();
            Thread.Sleep(4000);
            Console.Write("START-1\r\n");

            //version2
            Thread newThread2 = new Thread(CheckNew2);
            newThread2.Start();
            Thread.Sleep(4000);
            Console.Write("START-2\r\n");

            //version3
            Thread newThread3 = new Thread(CheckNew3);
            newThread3.Start();
            Thread.Sleep(4000);
            Console.Write("START-3\r\n");
        }

        static void CheckNew1()
        {
            while (true)
            {
                try
                {
                    string sql = "GetTradeCreateMsg 6";
                    ActRateInfoDetail(sql);
                }
                catch { }
                //1S检查一次
                Thread.Sleep(1000);
            }
        }

        static void CheckNew2()
        {
            while (true)
            {
                try
                {
                    string sql = "GetTradeCreateMsg 7";
                    ActRateInfoDetail(sql);
                }
                catch { }
                //1S检查一次
                Thread.Sleep(1000);
            }
        }

        static void CheckNew3()
        {
            while (true)
            {
                try
                {
                    string sql = "GetTradeCreateMsg 8";
                    ActRateInfoDetail(sql);
                }
                catch { }
                //1S检查一次
                Thread.Sleep(1000);
            }
        }


        static void ActRateInfoDetail(string sql)
        {
            DataTable dt = utils.ExecuteDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try
                {
                    string result = dt.Rows[i][0].ToString();
                    Console.Write(result + "...\r\n");
                    ReceiveMessage msg = new ReceiveMessage(result.ToString());
                    string ret = msg.ActDataResult();
                    if (ret == "1")
                    {
                        sql = "UPDATE TCS_TaobaoMsgLog SET isok = 1 WHERE id = " + dt.Rows[i][1].ToString();
                        Console.Write(sql + "...\r\n");
                        utils.ExecuteNonQuery(sql);

                        sql = "INSERT INTO TCS_TaobaoMsgLogBak SELECT * FROM [TCS_TaobaoMsgLog] WHERE id = " + dt.Rows[i][1].ToString();
                        Console.Write(sql + "...\r\n");
                        utils.ExecuteNonQuery(sql);

                        sql = "DELETE FROM TCS_TaobaoMsgLog WHERE id = " + dt.Rows[i][1].ToString();
                        Console.Write(sql + "...\r\n");
                        utils.ExecuteNonQuery(sql);
                    }
                }
                catch (Exception ex)
                {
                    sql = "UPDATE TCS_TaobaoMsgLog SET isok = 2 WHERE id = " + dt.Rows[i][1].ToString();
                    utils.ExecuteNonQuery(sql);
                    Console.Write(ex.Message.ToString() + "...\r\n");
                    Console.Write(ex.Source.ToString() + "...\r\n");
                    Console.Write(ex.StackTrace.ToString() + "...\r\n");
                }
            }
        }
    }
}
