using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace ReviewSendCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime date = DateTime.Now;
            int maxdate = 86000;
            int i = 0;
            int j = 0;
            int k = 0;
            Process process = new Process();
            Process processShipping = new Process();
            Process processOrder = new Process();
            while (true)
            {
                //判断消息接收服务的运行状态
                Process[] oldP = Process.GetProcessesByName("ReviewSend");
                if (oldP.Length > 0) //
                {
                    //24小时后重新启动
                    //Console.Write((DateTime.Now - date).Seconds + "\r\n");
                    if ((DateTime.Now - date).Seconds > maxdate)
                    {
                        //启动程序
                        process.Kill();
                        process.Close();
                        //process = Process.Start("ReviewSend.exe");
                        //重置启动时间
                        date = DateTime.Now;
                    }
                    //Console.Write("程序在运行! \r\n");
                }
                else
                {
                    i++;
                    Console.Write("[" + DateTime.Now.ToString() + "] - [" + i.ToString() + "] ReviewSend程序没有运行!!!!!! \r\n");
                    //启动程序
                    //Process.Start("ReviewSend.exe");
                    process = Process.Start("ReviewSend.exe");
                    //重置启动时间
                    date = DateTime.Now;
                    //24小时后重新启动
                    //Thread.Sleep(86000000);
                }

                //判断订单物流状态查询服务的运行状态
                Process[] oldProShipping = Process.GetProcessesByName("ReviewShippingCheck");
                if (oldProShipping.Length <= 0) //
                {
                    j++;
                    Console.Write("[" + DateTime.Now.ToString() + "] - [" + j.ToString() + "] ReviewShippingCheck程序没有运行!!!!!! \r\n");
                    //启动程序
                    processShipping = Process.Start("ReviewShippingCheck.exe");
                }

                //判断订单物流状态查询服务的运行状态
                Process[] oldProOrder = Process.GetProcessesByName("ReviewOrderAlert");
                if (oldProOrder.Length <= 0) //
                {
                    k++;
                    Console.Write("[" + DateTime.Now.ToString() + "] - [" + k.ToString() + "] ReviewOrderAlert程序没有运行!!!!!! \r\n");
                    //启动程序
                    processOrder = Process.Start("ReviewOrderAlert.exe");
                }

                //1秒刷新一下状态
                Thread.Sleep(1000);
            }
        }
    }
}
