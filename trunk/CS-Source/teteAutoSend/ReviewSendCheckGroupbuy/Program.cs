using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace ReviewSendCheckGroupbuy
{
    class Program
    {
        static void Main(string[] args)
        {
            int j1 = 0;
            Process process1 = new Process();

            while (true)
            {
                ////判断订单物流状态查询服务的运行状态
                //Process[] ReviewOrderAlert = Process.GetProcessesByName("ReviewOrderAlert");
                //if (ReviewOrderAlert.Length <= 0) //
                //{
                //    j1++;
                //    Console.Write("[" + DateTime.Now.ToString() + "] - [" + j1.ToString() + "] ReviewOrderAlert程序没有运行!!!!!! \r\n");
                //    //启动程序
                //    process1 = Process.Start("ReviewOrderAlert.exe");
                //}

                //判断订单物流状态查询服务的运行状态
                Process[] ReviewShippingCheck = Process.GetProcessesByName("ReviewShippingCheck");
                if (ReviewShippingCheck.Length <= 0) //
                {
                    j1++;
                    Console.Write("[" + DateTime.Now.ToString() + "] - [" + j1.ToString() + "] ReviewShippingCheck程序没有运行!!!!!! \r\n");
                    //启动程序
                    process1 = Process.Start("ReviewShippingCheck.exe");
                }

                //判断订单物流状态查询服务的运行状态
                Process[] ReviewShippingCheck1 = Process.GetProcessesByName("ReviewShippingCheck1");
                if (ReviewShippingCheck1.Length <= 0) //
                {
                    j1++;
                    Console.Write("[" + DateTime.Now.ToString() + "] - [" + j1.ToString() + "] ReviewShippingCheck1程序没有运行!!!!!! \r\n");
                    //启动程序
                    process1 = Process.Start("ReviewShippingCheck1.exe");
                }

                if (DateTime.Now.Hour.ToString() == "10" && DateTime.Now.Minute.ToString() == "34" && DateTime.Now.Second.ToString() == "50")
                {
                    //判断订单物流状态查询服务的运行状态
                    Process[] ReviewShippingCheckNormal = Process.GetProcessesByName("ReviewShippingCheckNormal");
                    if (ReviewShippingCheckNormal.Length <= 0) //
                    {
                        j1++;
                        Console.Write("[" + DateTime.Now.ToString() + "] - [" + j1.ToString() + "] ReviewShippingCheckNormal程序没有运行!!!!!! \r\n");
                        //启动程序
                        process1 = Process.Start("ReviewShippingCheckNormal.exe");
                    }
                }

                //判断订单物流状态查询服务的运行状态
                Process[] ActShippingInfo = Process.GetProcessesByName("ActShippingInfo");
                if (ActShippingInfo.Length <= 0) //
                {
                    j1++;
                    Console.Write("[" + DateTime.Now.ToString() + "] - [" + j1.ToString() + "] ActShippingInfo程序没有运行!!!!!! \r\n");
                    //启动程序
                    process1 = Process.Start("ActShippingInfo.exe");
                }


                //判断订单物流状态查询服务的运行状态
                Process[] ActShippingInfo1 = Process.GetProcessesByName("ActShippingInfo1");
                if (ActShippingInfo1.Length <= 0) //
                {
                    j1++;
                    Console.Write("[" + DateTime.Now.ToString() + "] - [" + j1.ToString() + "] ActShippingInfo1程序没有运行!!!!!! \r\n");
                    //启动程序
                    process1 = Process.Start("ActShippingInfo1.exe");
                }

                //判断订单物流状态查询服务的运行状态
                Process[] ActShippingInfo2 = Process.GetProcessesByName("ActShippingInfo2");
                if (ActShippingInfo2.Length <= 0) //
                {
                    j1++;
                    Console.Write("[" + DateTime.Now.ToString() + "] - [" + j1.ToString() + "] ActShippingInfo2程序没有运行!!!!!! \r\n");
                    //启动程序
                    process1 = Process.Start("ActShippingInfo2.exe");
                }



                //判断订单物流状态查询服务的运行状态
                Process[] ActUnpayTrade = Process.GetProcessesByName("ActUnpayTrade");
                if (ActUnpayTrade.Length <= 0) //
                {
                    j1++;
                    Console.Write("[" + DateTime.Now.ToString() + "] - [" + j1.ToString() + "] ActUnpayTrade程序没有运行!!!!!! \r\n");
                    //启动程序
                    process1 = Process.Start("ActUnpayTrade.exe");
                }

                //1秒刷新一下状态
                Thread.Sleep(5000);
            }
        }
    }
}
