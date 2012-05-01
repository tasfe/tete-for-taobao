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
            int p = 0;
            int q = 0;
            int w = 0;
            int e = 0;
            Process process = new Process();
            Process processShipping = new Process();
            Process processGroupbuy = new Process();
            Process oldGroupbuystop = new Process();
            Process oldGroupbuystr = new Process();
            Process oldGroupbuyup = new Process();
            Process oldGroupbuyend = new Process();
            
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

                //判断到期活动团购取消服务的运行状态
                Process[] oldGroupbuy = Process.GetProcessesByName("tetegroupbuyRemove");
                if (oldGroupbuy.Length <= 0) //
                {
                    k++;
                    Console.Write("[" + DateTime.Now.ToString() + "] - [" + k.ToString() + "] tetegroupbuyRemove程序没有运行!!!!!! \r\n");
                    //启动程序
                    processGroupbuy = Process.Start("tetegroupbuyRemove.exe");
                }

                //判断活动服务的运行状态
                Process[] Groupbuystop = Process.GetProcessesByName("teteactivity");
                if (Groupbuystop.Length <= 0) //
                {
                    p++;
                    Console.Write("[" + DateTime.Now.ToString() + "] - [" + p.ToString() + "] teteactivity程序没有运行!!!!!! \r\n");
                    //启动程序
                    oldGroupbuystop = Process.Start("teteactivity.exe");
                }

                //判断活动服务的运行状态
                Process[] Groupbuystr = Process.GetProcessesByName("teteactivitystart");
                if (Groupbuystr.Length <= 0) //
                {
                    w++;
                    Console.Write("[" + DateTime.Now.ToString() + "] - [" + w.ToString() + "] teteactivitystart程序没有运行!!!!!! \r\n");
                    //启动程序
                    oldGroupbuystr = Process.Start("teteactivitystart.exe");
                }

                //判断活动服务的运行状态
                Process[] Groupbuyup = Process.GetProcessesByName("teteactivitystop");
                if (Groupbuyup.Length <= 0) //
                {
                    q++;
                    Console.Write("[" + DateTime.Now.ToString() + "] - [" + q.ToString() + "] teteactivitystop程序没有运行!!!!!! \r\n");
                    //启动程序
                    oldGroupbuyup = Process.Start("teteactivitystop.exe");
                }

                //判断活动服务的运行状态
                Process[] Groupbuyend = Process.GetProcessesByName("teteactivityupdate");
                if (Groupbuyend.Length <= 0) //
                {
                    e++;
                    Console.Write("[" + DateTime.Now.ToString() + "] - [" + e.ToString() + "] teteactivityupdate程序没有运行!!!!!! \r\n");
                    //启动程序
                    oldGroupbuyend = Process.Start("teteactivityupdate.exe");
                }
                

                //1秒刷新一下状态
                Thread.Sleep(1000);
            }
        }
    }
}
