using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace WeiboSendCheckNew
{
    class Program
    {
        static void Main(string[] args)
        {
            Process processShipping = new Process();

            while (true)
            {
                //判断订单物流状态查询服务的运行状态
                Process[] WeiboSend = Process.GetProcessesByName("WeiboSend");
                if (WeiboSend.Length <= 0) //
                {
                    Console.Write("[" + DateTime.Now.ToString() + "] - WeiboSend程序没有运行!!!!!! \r\n");
                    //启动程序
                    processShipping = Process.Start("WeiboSend.exe");
                }

                Thread.Sleep(5000);
            }
        }
    }
}
