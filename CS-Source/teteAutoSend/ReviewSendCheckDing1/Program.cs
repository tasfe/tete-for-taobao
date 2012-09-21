using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace ReviewSendCheckDing1
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
                //Process[] ActRateInfo = Process.GetProcessesByName("ActRateInfo");
                //if (ActRateInfo.Length <= 0) //
                //{
                //    j1++;
                //    Console.Write("[" + DateTime.Now.ToString() + "] - [" + j1.ToString() + "] ActRateInfo程序没有运行!!!!!! \r\n");
                //    //启动程序
                //    process1 = Process.Start("ActRateInfo.exe");
                //}

                ////判断订单物流状态查询服务的运行状态
                //Process[] ActRateInfo1 = Process.GetProcessesByName("ActRateInfo1");
                //if (ActRateInfo1.Length <= 0) //
                //{
                //    j1++;
                //    Console.Write("[" + DateTime.Now.ToString() + "] - [" + j1.ToString() + "] ActRateInfo1程序没有运行!!!!!! \r\n");
                //    //启动程序
                //    process1 = Process.Start("ActRateInfo1.exe");
                //}

                ////判断订单物流状态查询服务的运行状态
                //Process[] ActRateInfo2 = Process.GetProcessesByName("ActRateInfo2");
                //if (ActRateInfo2.Length <= 0) //
                //{
                //    j1++;
                //    Console.Write("[" + DateTime.Now.ToString() + "] - [" + j1.ToString() + "] ActRateInfo2程序没有运行!!!!!! \r\n");
                //    //启动程序
                //    process1 = Process.Start("ActRateInfo2.exe");
                //}

                //判断订单物流状态查询服务的运行状态
                Process[] ActRateInfo3 = Process.GetProcessesByName("ActRateInfo3");
                if (ActRateInfo3.Length <= 0) //
                {
                    j1++;
                    Console.Write("[" + DateTime.Now.ToString() + "] - [" + j1.ToString() + "] ActRateInfo3程序没有运行!!!!!! \r\n");
                    //启动程序
                    process1 = Process.Start("ActRateInfo3.exe");
                }

                //判断订单物流状态查询服务的运行状态
                Process[] ActRateInfo4 = Process.GetProcessesByName("ActRateInfo4");
                if (ActRateInfo4.Length <= 0) //
                {
                    j1++;
                    Console.Write("[" + DateTime.Now.ToString() + "] - [" + j1.ToString() + "] ActRateInfo4程序没有运行!!!!!! \r\n");
                    //启动程序
                    process1 = Process.Start("ActRateInfo4.exe");
                }

                //判断订单物流状态查询服务的运行状态
                Process[] ActRateInfo5 = Process.GetProcessesByName("ActRateInfo5");
                if (ActRateInfo5.Length <= 0) //
                {
                    j1++;
                    Console.Write("[" + DateTime.Now.ToString() + "] - [" + j1.ToString() + "] ActRateInfo5程序没有运行!!!!!! \r\n");
                    //启动程序
                    process1 = Process.Start("ActRateInfo5.exe");
                }

                //判断订单物流状态查询服务的运行状态
                Process[] ActTradeBuyerPay = Process.GetProcessesByName("ActTradeBuyerPay");
                if (ActTradeBuyerPay.Length <= 0) //
                {
                    j1++;
                    Console.Write("[" + DateTime.Now.ToString() + "] - [" + j1.ToString() + "] ActTradeBuyerPay程序没有运行!!!!!! \r\n");
                    //启动程序
                    process1 = Process.Start("ActTradeBuyerPay.exe");
                }

                //1秒刷新一下状态
                Thread.Sleep(5000);
            }
        }
    }
}
