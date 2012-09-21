using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeteTopApi.Logic;
using System.Threading;

namespace ReviewShippingCheck1
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

            //while (true)
            //{
            //    //如果是晚上10点到早上9点这个时间端内，不发送物流提醒短信
            //    if (DateTime.Now.Hour > 8 && DateTime.Now.Hour < 22)
            //    {
            //        ShippingSuccess act = new ShippingSuccess();
            //        act.Start();

            //        //30分钟检查一次
            //        Thread.Sleep(60000);
            //    }
            //}
        }

        static void CheckNew1()
        {
            while (true)
            {
                try
                {
                    //如果是晚上10点到早上9点这个时间端内，不发送物流提醒短信
                    if (DateTime.Now.Hour > 8 && DateTime.Now.Hour < 22)
                    {
                        ShippingSuccess act = new ShippingSuccess();
                        act.Start("3");

                        //30分钟检查一次
                        Thread.Sleep(60000);
                    }
                }
                catch { }
                //1S检查一次
                Thread.Sleep(10000);
            }
        }

        static void CheckNew2()
        {
            while (true)
            {
                try
                {
                    //如果是晚上10点到早上9点这个时间端内，不发送物流提醒短信
                    if (DateTime.Now.Hour > 8 && DateTime.Now.Hour < 22)
                    {
                        ShippingSuccess act = new ShippingSuccess();
                        act.Start("4");

                        //30分钟检查一次
                        Thread.Sleep(60000);
                    }
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
                    //如果是晚上10点到早上9点这个时间端内，不发送物流提醒短信
                    if (DateTime.Now.Hour > 8 && DateTime.Now.Hour < 22)
                    {
                        ShippingSuccess act = new ShippingSuccess();
                        act.Start("5");

                        //30分钟检查一次
                        Thread.Sleep(60000);
                    }
                }
                catch { }
                //1S检查一次
                Thread.Sleep(5000);
            }
        }
    }
}
