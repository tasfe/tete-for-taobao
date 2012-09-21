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
    //    static void Main(string[] args)
    //    {
    //        while (true)
    //        {
    //            string sql = "SELECT result,id FROM TCS_TaobaoMsgLog WHERE isok = 0 AND typ = 'TradeSellerShip'";
    //            DataTable dt = utils.ExecuteDataTable(sql);

    //            for (int i = 0; i < dt.Rows.Count; i++)
    //            {
    //                try
    //                {
    //                    string result = dt.Rows[i][0].ToString();
    //                    ReceiveMessage msg = new ReceiveMessage(result.ToString());
    //                    msg.ActData();
    //                    sql = "UPDATE TCS_TaobaoMsgLog SET isok = 1 WHERE id = " + dt.Rows[i][1].ToString();
    //                    Console.Write(sql + "\r\n");
    //                    utils.ExecuteNonQuery(sql);

    //                    sql = "INSERT INTO TCS_TaobaoMsgLogBak SELECT * FROM [TCS_TaobaoMsgLog] WHERE id = " + dt.Rows[i][1].ToString();
    //                    Console.Write(sql + "\r\n");
    //                    utils.ExecuteNonQuery(sql);

    //                    sql = "DELETE FROM TCS_TaobaoMsgLog WHERE id = " + dt.Rows[i][1].ToString();
    //                    Console.Write(sql + "\r\n");
    //                    utils.ExecuteNonQuery(sql);
    //                }
    //                catch
    //                {
    //                    sql = "UPDATE TCS_TaobaoMsgLog SET isok = 2 WHERE id = " + dt.Rows[i][1].ToString();
    //                    Console.Write(sql + "\r\n");
    //                    utils.ExecuteNonQuery(sql);
    //                }
    //            }
    //            //1S检查一次
    //            Thread.Sleep(1000);
    //        }
    //    }

        
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
                string sql = "GetTradeSellerShipMsg 0";
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
                string sql = "GetTradeSellerShipMsg 1";
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
                string sql = "GetTradeSellerShipMsg 2";
                ActRateInfoDetail(sql);
                }
                catch { }
                //1S检查一次
                Thread.Sleep(1000);
            }
        }


        static void ActRateInfoDetail(string sql)
        {
            //如果是晚上11点到早上9点这个时间端内，不发送发货短信
            if (DateTime.Now.Hour <= 8 || DateTime.Now.Hour >= 23)
            {
                return;
            }

            DataTable dt = utils.ExecuteDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try
                {
                    string result = dt.Rows[i][0].ToString();
                    ReceiveMessage msg = new ReceiveMessage(result.ToString());
                    msg.ActData();


                    //判断数据里是否该评价的记录，解决评价消息不处理不报错问题
                    //string tid = GetValueByProperty(result, "tid");
                    //sql = "SELECT orderid FROM TCS_TradeRate WHERE orderid ='" + tid + "'";
                    //DataTable dt1 = utils.ExecuteDataTable(sql);
                    //if (dt1.Rows.Count != 0)
                    //{
                    sql = "UPDATE TCS_TaobaoMsgLog SET isok = 1 WHERE id = " + dt.Rows[i][1].ToString();
                    Console.Write(sql + "\r\n");
                    utils.ExecuteNonQuery(sql);

                    sql = "INSERT INTO TCS_TaobaoMsgLogBak SELECT * FROM [TCS_TaobaoMsgLog] WHERE id = " + dt.Rows[i][1].ToString();
                    Console.Write(sql + "\r\n");
                    utils.ExecuteNonQuery(sql);

                    sql = "DELETE FROM TCS_TaobaoMsgLog WHERE id = " + dt.Rows[i][1].ToString();
                    Console.Write(sql + "\r\n");
                    utils.ExecuteNonQuery(sql);
                    //}
                }
                catch
                {
                    sql = "UPDATE TCS_TaobaoMsgLog SET isok = 2 WHERE id = " + dt.Rows[i][1].ToString();
                    utils.ExecuteNonQuery(sql);
                }
            }
        }
    }
}
