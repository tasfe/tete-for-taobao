using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using TeteTopApi;

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
            int j1 = 0;
            int j2 = 0;
            int j3 = 0;
            int k = 0;
            Process process = new Process();
            Process processShipping = new Process();
            Process processOrder = new Process();
            Process process1 = new Process();
            Process process2 = new Process();
            Process process3 = new Process();
            Process process4 = new Process();
            Process process5 = new Process();
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
                Process[] oldProShipping = Process.GetProcessesByName("AutoMsgSend");
                if (oldProShipping.Length <= 0) //
                {
                    j++;
                    Console.Write("[" + DateTime.Now.ToString() + "] - [" + j.ToString() + "] AutoMsgSend程序没有运行!!!!!! \r\n");
                    //启动程序
                    processShipping = Process.Start("AutoMsgSend.exe");
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


                ////判断订单物流状态查询服务的运行状态
                //Process[] ActShippingInfo = Process.GetProcessesByName("ActShippingInfo");
                //if (ActShippingInfo.Length <= 0) //
                //{
                //    j2++;
                //    Console.Write("[" + DateTime.Now.ToString() + "] - [" + j2.ToString() + "] ActShippingInfo程序没有运行!!!!!! \r\n");
                //    //启动程序
                //    process2 = Process.Start("ActShippingInfo.exe");
                //}


                ////判断订单物流状态查询服务的运行状态
                //Process[] ActUnpayTrade = Process.GetProcessesByName("ActUnpayTrade");
                //if (ActUnpayTrade.Length <= 0) //
                //{
                //    j3++;
                //    Console.Write("[" + DateTime.Now.ToString() + "] - [" + j3.ToString() + "] ActUnpayTrade程序没有运行!!!!!! \r\n");
                //    //启动程序
                //    process5 = Process.Start("ActUnpayTrade.exe");
                //}


                ////判断订单物流状态查询服务的运行状态
                //Process[] ReviewShippingCheckNormal = Process.GetProcessesByName("ReviewShippingCheckNormal");
                //if (ReviewShippingCheckNormal.Length <= 0) //
                //{
                //    j3++;
                //    Console.Write("[" + DateTime.Now.ToString() + "] - [" + j3.ToString() + "] ReviewShippingCheckNormal程序没有运行!!!!!! \r\n");
                //    //启动程序
                //    process5 = Process.Start("ReviewShippingCheckNormal.exe");
                //}


                //刷新session和整理队伍
                if (DateTime.Now.Hour.ToString() == "0" && DateTime.Now.Minute.ToString() == "0" && DateTime.Now.Second.ToString() == "1")
                {
                    Process[] ReviewLostGet = Process.GetProcessesByName("TeteGetUserOrder");
                    if (ReviewLostGet.Length <= 0) //
                    {
                        //启动程序
                        process4 = Process.Start("TeteGetUserOrder.exe");
                    }
                }


                //整点启动丢失数据找回服务
                if (DateTime.Now.Minute.ToString() == "11" && DateTime.Now.Second.ToString() == "11")
                {
                    Process[] ReviewLostGet = Process.GetProcessesByName("ReviewLostGet");
                    if (ReviewLostGet.Length <= 0) //
                    {
                        //启动程序
                        process4 = Process.Start("ReviewLostGet.exe");
                    }
                }

                //判断订单物流状态查询服务的运行状态
                if (DateTime.Now.Hour.ToString() == "14" && DateTime.Now.Minute.ToString() == "34" && DateTime.Now.Second.ToString() == "50")
                {
                    Process[] ReviewShopAlert = Process.GetProcessesByName("ReviewShopAlert");
                    if (ReviewShopAlert.Length <= 0) //
                    {
                        //启动程序
                        process4 = Process.Start("ReviewShopAlert.exe");
                    }
                }

                try
                {
                    string sql = "SELECT TOP 1 adddate FROM TCS_TaobaoMsgLog ORDER BY id DESC";
                    string lastDate = utils.ExecuteString(sql);

                    Console.Write("[" + DateTime.Now.ToString() + "] - [" + lastDate.ToString() + "] \r\n");
                    if((DateTime.Now - DateTime.Parse(lastDate)).Seconds > 60)
                    {
                        process = Process.Start("ReviewSend.exe");
                    }
                }
                catch { }
                //1秒刷新一下状态
                Thread.Sleep(1000);
            }
        }
    }
}
