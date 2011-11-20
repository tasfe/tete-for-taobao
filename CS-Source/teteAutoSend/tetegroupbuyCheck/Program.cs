using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace tetegroupbuyCheck
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
                //判断同步宝贝详细服务的运行状态
                Process[] oldP = Process.GetProcessesByName("teteWriteItem");
                
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
                    Console.Write("[" + DateTime.Now.ToString() + "] - [" + i.ToString() + "] teteWriteItem程序没有运行!!!!!! \r\n");
                    //启动程序
                    //Process.Start("ReviewSend.exe");
                    process = Process.Start("teteWriteItem.exe");
                    //重置启动时间
                    date = DateTime.Now;
                    //24小时后重新启动
                    //Thread.Sleep(86000000);
                }

                //判断团购服务的运行状态
                Process[] oldProShipping = Process.GetProcessesByName("tetegroupbuy");
                if (oldProShipping.Length <= 0) //
                {
                    j++;
                    Console.Write("[" + DateTime.Now.ToString() + "] - [" + j.ToString() + "] tetegroupbuy程序没有运行!!!!!! \r\n");
                    //启动程序
                    processShipping = Process.Start("tetegroupbuy.exe");
                }

                

                //1秒刷新一下状态
                Thread.Sleep(1000);
            }
        }
    }
}
