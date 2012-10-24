using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data;
using TeteTopApi;
using TeteTopApi.Entity;
using TeteTopApi.DataContract;

namespace AutoMsgSend
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                string sql = string.Empty;
                sql = "SELECT * FROM TCS_Mission WHERE typ ='act' AND isdel = 0 AND isstop = 0 AND issend = 0 AND GETDATE() > senddate";
                Console.WriteLine(sql);
                DataTable dt = utils.ExecuteDataTable(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //Console.WriteLine(dt.Rows[i]["nick"].ToString() + DateTime.Now.ToString() + dt.Rows[i]["content"].ToString() + dt.Rows[i]["grade"].ToString());
                    //sql = "UPDATE TCS_Mission SET issend = 2 WHERE guid = '" + dt.Rows[i]["guid"].ToString() + "'";
                    //Console.WriteLine(sql);
                    //utils.ExecuteNonQuery(sql);

                    MsgAutoSend m = new MsgAutoSend(dt.Rows[i]["nick"].ToString(), dt.Rows[i]["content"].ToString(), dt.Rows[i]["grade"].ToString(), dt.Rows[i]["guid"].ToString());
                    m.Send();
                }

                Thread.Sleep(2000);
            }
        }
    }


    public class MsgAutoSend
    {
        public string Nick { get; set; }
        public string Msg { get; set; }
        public string Grade { get; set; }
        public string Guid { get; set; }
        public int index = 0;

        public MsgAutoSend(string a, string b, string c, string d)
        {
            this.Nick = a;
            this.Msg = b;
            this.Grade = c;
            this.Guid = d;
        }

        public void Send()
        {
            ThreadPool.SetMaxThreads(20, 100);

            //version1
            Thread newThread1 = new Thread(CheckNew1);
            newThread1.Start();
            Thread.Sleep(2000);
            //Console.Write("START-1\r\n");

            //version2
            Thread newThread2 = new Thread(CheckNew2);
            newThread2.Start();
            Thread.Sleep(2000);
            //Console.Write("START-2\r\n");

            //version3
            Thread newThread3 = new Thread(CheckNew3);
            newThread3.Start();
            Thread.Sleep(2000);
            //Console.Write("START-3\r\n");

            ////version3
            //Thread newThread4 = new Thread(CheckNew4);
            //newThread4.Start();
            //Thread.Sleep(2000);
            ////Console.Write("START-3\r\n");

            ////version3
            //Thread newThread5 = new Thread(CheckNew5);
            //newThread5.Start();
            //Thread.Sleep(2000);
            ////Console.Write("START-3\r\n");

            ////version3
            //Thread newThread6 = new Thread(CheckNew6);
            //newThread6.Start();
            //Thread.Sleep(2000);
            ////Console.Write("START-3\r\n");

            ////version3
            //Thread newThread7 = new Thread(CheckNew7);
            //newThread7.Start();
            //Thread.Sleep(2000);
            ////Console.Write("START-3\r\n");

            ////version3
            //Thread newThread8 = new Thread(CheckNew8);
            //newThread8.Start();
            //Thread.Sleep(2000);
            ////Console.Write("START-3\r\n");
        }



        private void CheckNew1()
        {
            CheckNewIndex("0");
        }
        private void CheckNew2()
        {
            CheckNewIndex("1");
        }
        private void CheckNew3()
        {
            CheckNewIndex("2");
        }
        private void CheckNew4()
        {
            CheckNewIndex("3");
        }
        private void CheckNew5()
        {
            CheckNewIndex("4");
        }
        private void CheckNew6()
        {
            CheckNewIndex("5");
        }
        private void CheckNew7()
        {
            CheckNewIndex("6");
        }
        private void CheckNew8()
        {
            CheckNewIndex("7");
        }

        private void CheckNewIndex(string i)
        {
            string sql = "AutoMsgSend '" + Nick + "','" + Grade + "'," + i;

            try
            {
                ActRateInfoDetail(sql);
                index++;
            }
            catch { }

            if (index == 8)
            {
                //发送完毕
                sql = "UPDATE TCS_Mission SET issend = 1 WHERE guid = '" + this.Guid + "'";
                Console.WriteLine(sql);
                utils.ExecuteNonQuery(sql);
            }
        }

        private void ActRateInfoDetail(string sql)
        {
            try
            {
                DataTable dt = utils.ExecuteDataTable(sql);
                Trade trade = new Trade();
                ShopInfo shop = new ShopInfo();
                ShopData db = new ShopData();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string mobile = dt.Rows[i][0].ToString();
                    trade.Nick = dt.Rows[i][1].ToString();
                    shop.Nick = dt.Rows[i][1].ToString();
                    trade.BuyNick = dt.Rows[i][2].ToString();
                    trade.Mobile = dt.Rows[i][0].ToString();

                    Console.WriteLine(trade.Mobile + "-" + Msg);
                    //string result = Message.Send(trade.Mobile, msg);
                    sql = "SELECT total FROM TCS_ShopConfig WHERE nick = '" + Nick + "'";
                    string total = utils.ExecuteString(sql);

                    if (int.Parse(total) > 0)
                    {
                        if (!db.IsSendMsgToday(trade, "act"))
                        {
                            //先插入数据库 解决多优惠券赠送短信多发问题
                            string msgResult = Message.SendMuti(trade.Mobile, Msg);

                            db.InsertShopMsgLog(shop, trade, Msg, msgResult, "act");
                        }
                    }
                }
            }
            catch { }
        }
    }
}
