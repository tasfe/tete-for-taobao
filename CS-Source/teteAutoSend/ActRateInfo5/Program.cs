using System;
using System.Collections.Generic;
using System.Text;
using TeteTopApi.Logic;
using System.Threading;
using System.Data;
using TeteTopApi;
using System.Text.RegularExpressions;

namespace ActRateInfo5
{
    class Program
    {
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
                string sql = "GetTradeRateMsg 15";
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
                string sql = "GetTradeRateMsg 16";
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
                string sql = "GetTradeRateMsg 17";
                ActRateInfoDetail(sql);
                }
                catch { }
                //1S检查一次
                Thread.Sleep(1000);
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
