using System;
using System.Collections.Generic;
using System.Text;
using TeteTopApi.Logic;
using System.Threading;
using System.Data;
using TeteTopApi;
using System.Text.RegularExpressions;

namespace ActRateInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadPool.SetMaxThreads(20, 100);

            //version1
            Thread newThread1 = new Thread(CheckNew1);
            newThread1.Start();
            Thread.Sleep(2000);
            Console.Write("START-1\r\n");

            //version2
            Thread newThread2 = new Thread(CheckNew2);
            newThread2.Start();
            Thread.Sleep(2000);
            Console.Write("START-2\r\n");

            //version3
            Thread newThread3 = new Thread(CheckNew3);
            newThread3.Start();
            Thread.Sleep(2000);
            Console.Write("START-3\r\n");

            ////version4
            //Thread newThread4 = new Thread(CheckNew4);
            //newThread4.Start();
            //Thread.Sleep(2000);
            //Console.Write("START-4\r\n");

            ////version5
            //Thread newThread5 = new Thread(CheckNew5);
            //newThread5.Start();
            //Thread.Sleep(2000);
            //Console.Write("START-5\r\n");

            ////version5
            //Thread newThread6 = new Thread(CheckNew6);
            //newThread6.Start();
            //Thread.Sleep(2000);
            //Console.Write("START-5\r\n");

            ////version5
            //Thread newThread7 = new Thread(CheckNew7);
            //newThread7.Start();
            //Thread.Sleep(2000);
            //Console.Write("START-5\r\n");

            ////version5
            //Thread newThread8 = new Thread(CheckNew8);
            //newThread8.Start();
            //Thread.Sleep(2000);
            //Console.Write("START-5\r\n");

            ////version5
            //Thread newThread9 = new Thread(CheckNew9);
            //newThread9.Start();
            //Thread.Sleep(2000);
            //Console.Write("START-5\r\n");

            ////version5
            //Thread newThread10 = new Thread(CheckNew10);
            //newThread10.Start();
            //Thread.Sleep(2000);
            //Console.Write("START-5\r\n");

            ////version5
            //Thread newThread11 = new Thread(CheckNew11);
            //newThread11.Start();
            //Thread.Sleep(2000);
            //Console.Write("START-5\r\n");

            ////version5
            //Thread newThread12 = new Thread(CheckNew12);
            //newThread12.Start();
            //Thread.Sleep(2000);
            //Console.Write("START-5\r\n");

            ////version5
            //Thread newThread13 = new Thread(CheckNew13);
            //newThread13.Start();
            //Thread.Sleep(2000);
            //Console.Write("START-5\r\n");

            ////version5
            //Thread newThread14 = new Thread(CheckNew14);
            //newThread14.Start();
            //Thread.Sleep(2000);
            //Console.Write("START-5\r\n");

            ////version5
            //Thread newThread15 = new Thread(CheckNew15);
            //newThread15.Start();
            //Thread.Sleep(2000);
            //Console.Write("START-5\r\n");

            ////version5
            //Thread newThread16 = new Thread(CheckNew16);
            //newThread16.Start();
            //Thread.Sleep(2000);
            //Console.Write("START-5\r\n");

            ////version5
            //Thread newThread17 = new Thread(CheckNew17);
            //newThread17.Start();
            //Thread.Sleep(2000);
            //Console.Write("START-5\r\n");

            ////version5
            //Thread newThread18 = new Thread(CheckNew18);
            //newThread18.Start();
            //Thread.Sleep(2000);
            //Console.Write("START-5\r\n");
        }

        static void CheckNew1()
        {
            while (true)
            {
                try
                {
                string sql = "GetTradeRateMsg 0";
                ActRateInfoDetail(sql);
                }
                catch { }
                //1S检查一次
                Thread.Sleep(5000);
            }
        }

        static void CheckNew2()
        {
            while (true)
            {
                try
                {
                string sql = "GetTradeRateMsg 1";
                ActRateInfoDetail(sql);
                }
                catch { }
                //1S检查一次
                Thread.Sleep(5000);
            }
        }

        static void CheckNew3()
        {
            while (true)
            {
                try
                {
                string sql = "GetTradeRateMsg 2";
                ActRateInfoDetail(sql);
                }
                catch { }
                //1S检查一次
                Thread.Sleep(5000);
            }
        }

        static void CheckNew4()
        {
            while (true)
            {
                string sql = "GetTradeRateMsg 3";
                ActRateInfoDetail(sql);
                //1S检查一次
                Thread.Sleep(5000);
            }
        }

        static void CheckNew5()
        {
            while (true)
            {
                string sql = "GetTradeRateMsg 4";
                ActRateInfoDetail(sql);
                //1S检查一次
                Thread.Sleep(5000);
            }
        }

        static void CheckNew6()
        {
            while (true)
            {
                string sql = "GetTradeRateMsg 5";
                ActRateInfoDetail(sql);
                //1S检查一次
                Thread.Sleep(5000);
            }
        }

        static void CheckNew7()
        {
            while (true)
            {
                string sql = "GetTradeRateMsg 6";
                ActRateInfoDetail(sql);
                //1S检查一次
                Thread.Sleep(5000);
            }
        }

        static void CheckNew8()
        {
            while (true)
            {
                string sql = "GetTradeRateMsg 7";
                ActRateInfoDetail(sql);
                //1S检查一次
                Thread.Sleep(5000);
            }
        }

        static void CheckNew9()
        {
            while (true)
            {
                string sql = "GetTradeRateMsg 8";
                ActRateInfoDetail(sql);
                //1S检查一次
                Thread.Sleep(5000);
            }
        }
        static void CheckNew10()
        {
            while (true)
            {
                string sql = "GetTradeRateMsg 9";
                ActRateInfoDetail(sql);
                //1S检查一次
                Thread.Sleep(5000);
            }
        }
        static void CheckNew11()
        {
            while (true)
            {
                string sql = "GetTradeRateMsg 10";
                ActRateInfoDetail(sql);
                //1S检查一次
                Thread.Sleep(5000);
            }
        }
        static void CheckNew12()
        {
            while (true)
            {
                string sql = "GetTradeRateMsg 11";
                ActRateInfoDetail(sql);
                //1S检查一次
                Thread.Sleep(5000);
            }
        }
        static void CheckNew13()
        {
            while (true)
            {
                string sql = "GetTradeRateMsg 12";
                ActRateInfoDetail(sql);
                //1S检查一次
                Thread.Sleep(5000);
            }
        }
        static void CheckNew14()
        {
            while (true)
            {
                string sql = "GetTradeRateMsg 13";
                ActRateInfoDetail(sql);
                //1S检查一次
                Thread.Sleep(5000);
            }
        }
        static void CheckNew15()
        {
            while (true)
            {
                string sql = "GetTradeRateMsg 14";
                ActRateInfoDetail(sql);
                //1S检查一次
                Thread.Sleep(5000);
            }
        }
        static void CheckNew16()
        {
            while (true)
            {
                string sql = "GetTradeRateMsg 15";
                ActRateInfoDetail(sql);
                //1S检查一次
                Thread.Sleep(5000);
            }
        }
        static void CheckNew17()
        {
            while (true)
            {
                string sql = "GetTradeRateMsg 16";
                ActRateInfoDetail(sql);
                //1S检查一次
                Thread.Sleep(5000);
            }
        }
        static void CheckNew18()
        {
            while (true)
            {
                string sql = "GetTradeRateMsg 17";
                ActRateInfoDetail(sql);
                //1S检查一次
                Thread.Sleep(5000);
            }
        }



        /// <summary>
        /// 根据属性名获取JSON中的属性值（字符）
        /// </summary>
        /// <param name="str"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static string GetValueByProperty(string str, string prop)
        {
            Regex reg = new Regex(@"""" + prop + @""":""([^""]*)""", RegexOptions.IgnoreCase);
            if (reg.IsMatch(str))
            {
                return reg.Match(str).Groups[1].ToString();
            }
            else
            {
                return "";
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

                        sql = "[RecordMsgCountTradeRate] '" + dt.Rows[i][2].ToString() + "'";
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
