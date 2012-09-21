using System;
using System.Collections.Generic;
using System.Text;
using TeteTopApi;
using TeteTopApi.Entity;
using TeteTopApi.Logic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using TeteTopApi.DataContract;
using TeteTopApi.Entity;
using TeteTopApi.TopApi;
using System.Text.RegularExpressions;

namespace WeiboSend
{
    class Program
    {
        static void Main(string[] args)
        {
            //LostMessageGet lost = new LostMessageGet();
            //ReflashMsgLine();
            //Console.ReadLine();
            //return;
            string msg = "{\"packet\":{\"code\":202,\"msg\":{\"notify_trade\":{\"topic\":\"trade\",\"payment\":\"152.00\",\"status\":\"TradeRated\",\"type\":\"guarantee_trade\",\"modified\":\"2012-09-14 16:57:53\",\"buyer_nick\":\"xuyj119\",\"nick\":\"特宝贝母婴旗舰店\",\"oid\":178920433144946,\"is_3D\":true,\"user_id\":21775371,\"tid\":178920433144946,\"seller_nick\":\"特宝贝母婴旗舰店\"}}}}";



          //  //lost.Start("2012-07-07 00:00:00", "2012-07-09 10:00:00");
            //ActMissingMsg("2012-09-07 18:50:00", "2012-09-07 19:49:59");
            //LostMessageGet lostGet = new LostMessageGet();
            //lostGet.Start("2012-09-07 18:50:00", "2012-09-07 19:49:59");
           // lostGet.Start("2012-07-21 01:00:01", "2012-07-21 01:59:59");
           // lostGet.Start("2012-07-21 02:00:01", "2012-07-21 02:59:59");
           // lostGet.Start("2012-07-21 03:00:01", "2012-07-21 03:59:59");
           // lostGet.Start("2012-07-21 04:00:01", "2012-07-21 04:59:59");
           // lostGet.Start("2012-07-21 05:00:01", "2012-07-21 05:59:59");
           // lostGet.Start("2012-07-21 06:00:01", "2012-07-21 06:59:59");
           // lostGet.Start("2012-07-21 07:00:01", "2012-07-21 07:59:59");
           //Console.ReadLine();
           //return;
            //ActMissingMsg("2012-08-26 01:00:00", "2012-08-26 23:59:59");
          ////  Console.ReadLine();
          //  ActMissingMsg("2012-07-09 00:00:01", "2012-07-09 10:00:00");
          ////  Console.ReadLine();

            //ActMissingMsg("2012-08-22 11:00:00", "2012-08-22 11:43:00");
            //ActMissingMsg("2012-07-07 20:00:00", "2012-07-07 21:00:00");
            //ActMissingMsg("2012-07-07 21:00:00", "2012-07-07 22:00:00");
            //ActMissingMsg("2012-07-07 22:00:00", "2012-07-07 23:00:00");
            //ActMissingMsg("2012-07-07 23:00:00", "2012-07-08 00:00:00");

            //ActMissingMsg("2012-07-08 00:00:00", "2012-07-08 01:00:00");
            //ActMissingMsg("2012-07-08 01:00:00", "2012-07-08 02:00:00");
            //ActMissingMsg("2012-07-08 02:00:00", "2012-07-08 03:00:00");
            //ActMissingMsg("2012-07-08 03:00:00", "2012-07-08 04:00:00");
            //ActMissingMsg("2012-07-08 04:00:00", "2012-07-08 05:00:00");
            //ActMissingMsg("2012-07-08 05:00:00", "2012-07-08 06:00:00");
            //ActMissingMsg("2012-07-08 06:00:00", "2012-07-08 07:00:00");
            //ActMissingMsg("2012-07-08 07:00:00", "2012-07-08 08:00:00");
            //ActMissingMsg("2012-07-08 08:00:00", "2012-07-08 09:00:00");
            //ActMissingMsg("2012-07-08 09:00:00", "2012-07-08 10:00:00");
            //ActMissingMsg("2012-07-08 10:00:00", "2012-07-08 11:00:00");
            //ActMissingMsg("2012-07-08 11:00:00", "2012-07-08 12:00:00");
            //ActMissingMsg("2012-07-08 12:00:00", "2012-07-08 13:00:00");
            //ActMissingMsg("2012-07-08 13:00:00", "2012-07-08 14:00:00");
            //ActMissingMsg("2012-07-08 14:00:00", "2012-07-08 15:00:00");
            //ActMissingMsg("2012-07-08 15:00:00", "2012-07-08 16:00:00");
            //ActMissingMsg("2012-07-08 16:00:00", "2012-07-08 17:00:00");
            //ActMissingMsg("2012-07-08 17:00:00", "2012-07-08 18:00:00");
            //ActMissingMsg("2012-07-08 18:00:00", "2012-07-08 19:00:00");
            //ActMissingMsg("2012-07-08 19:00:00", "2012-07-08 20:00:00");
            //ActMissingMsg("2012-07-08 20:00:00", "2012-07-08 21:00:00");
            //ActMissingMsg("2012-07-08 21:00:00", "2012-07-08 22:00:00");
            //ActMissingMsg("2012-07-08 22:00:00", "2012-07-08 23:00:00");
            //ActMissingMsg("2012-07-08 23:00:00", "2012-07-09 00:00:00");

            //ActMissingMsg("2012-07-09 00:00:00", "2012-07-09 01:00:00");
            //ActMissingMsg("2012-07-09 01:00:00", "2012-07-09 02:00:00");
            //ActMissingMsg("2012-07-09 02:00:00", "2012-07-09 03:00:00");
            //ActMissingMsg("2012-07-09 03:00:00", "2012-07-09 04:00:00");
            //ActMissingMsg("2012-07-09 04:00:00", "2012-07-09 05:00:00");
            //ActMissingMsg("2012-07-09 05:00:00", "2012-07-09 06:00:00");
            //ActMissingMsg("2012-07-09 06:00:00", "2012-07-09 07:00:00");
            //ActMissingMsg("2012-07-09 07:00:00", "2012-07-09 08:00:00");
            //ActMissingMsg("2012-07-09 08:00:00", "2012-07-09 09:00:00");
            //ActMissingMsg("2012-07-09 09:00:00", "2012-07-09 10:00:00");

            //Console.Read();
            //return;


            //string AppKey = "12159997";
            //string Secret = "614e40bfdb96e9063031d1a9e56fbed5";
            //string Session = "";
            //string Url = "http://gw.api.taobao.com/router/rest";
            //string result = string.Empty;
            //string nick = string.Empty;

            //Api top = new Api(AppKey, Secret, Session, Url);

            //IDictionary<string, string> param = new Dictionary<string, string>();
            //Regex reg = new Regex(@"""user_id"":([0-9]*)", RegexOptions.IgnoreCase);

            //string sql = "SELECT nick,session FROM TCS_ShopSession WHERE version <> -1 AND sid = 0 AND session <> ''";
            ////DataTable dtUser = utils.ExecuteDataTable(sql);
            ////for (int i = 0; i < dtUser.Rows.Count; i++)
            ////{
            ////    param = new Dictionary<string, string>();
            ////    param.Add("fields", "user_id");
            ////    result = top.CommonTopApi("taobao.user.get", param, dtUser.Rows[i][1].ToString());
            ////    //Console.Write(result + "-");
            ////    string taobaoid = reg.Match(result).Groups[1].ToString();
            ////    //Console.Write(taobaoid + "-");
            ////    sql = "UPDATE TCS_ShopSession SET sid = '" + taobaoid + "' WHERE nick = '" + dtUser.Rows[i][0].ToString() + "'";
            ////    Console.Write(sql + "\r\n");
            ////    utils.ExecuteNonQuery(sql);
            ////}
            ////Console.ReadLine();
            ////return;

            //param = new Dictionary<string, string>();
            //param.Add("start", "2012-05-16 08:00:00");
            //param.Add("end", "2012-05-16 09:00:00");

            //result = top.CommonTopApi("taobao.comet.discardinfo.get", param, Session);

            ////Console.Write(result + "-");
            //MatchCollection match = reg.Matches(result);
            //for (int i = 0; i < match.Count; i++)
            //{
            //    string sid = match[i].Groups[1].ToString();
            //    //Console.Write(match[i].Groups[1].ToString() + "\r\n");

            //    sql = "SELECT nick FROM TCS_ShopSession WHERE sid = '" + sid + "'";
            //    DataTable dt = utils.ExecuteDataTable(sql);
            //    if (dt.Rows.Count != 0)
            //    {
            //        nick = dt.Rows[0][0].ToString();
            //        //Console.Write(nick + "\r\n\r\n"); taobao.increment.trades.get
            //        param = new Dictionary<string, string>();
            //        param.Add("start_modified", "2012-05-16 08:00:00");
            //        param.Add("end_modified", "2012-05-16 09:00:00");
            //        param.Add("nick", nick);
            //        result = top.CommonTopApi("taobao.increment.trades.get", param, Session);

            //        Regex reg1 = new Regex(@"\{""buyer_nick""[^\}]*\}", RegexOptions.IgnoreCase);
            //        MatchCollection match1 = reg1.Matches(result);
            //        for (int j = 0; j < match1.Count; j++)
            //        {
            //            Console.Write(match1[j].Groups[0].ToString() + "\r\n");
            //            string resultNew = "\"msg\":{\"notify_trade\":" + match1[j].Groups[0].ToString() + "}";
            //            LogData dbLog = new LogData();
            //            Trade trade = utils.GetTrade(resultNew);
            //            dbLog.InsertMsgLogInfo(trade.Nick, trade.Status, resultNew);
            //        }
            //    }
            //}
                //string sql = "SELECT DISTINCT(nick) FROM TCS_PayLog";
                //DataTable dt = utils.ExecuteDataTable(sql);
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    sql = "SELECT SUM(count) FROM TCS_PayLog WHERE nick = '" + dt.Rows[i]["nick"].ToString() + "'";
                //    string count = utils.ExecuteString(sql);

                //    sql = "SELECT total FROM TCS_ShopConfig WHERE nick = '" + dt.Rows[i]["nick"].ToString() + "'";
                //    string total = utils.ExecuteString(sql);

                //    sql = "SELECT used FROM TCS_ShopConfig WHERE nick = '" + dt.Rows[i]["nick"].ToString() + "'";
                //    string used = utils.ExecuteString(sql);

                //    if (int.Parse(count) != (int.Parse(total) + int.Parse(used)))
                //    {
                //        Console.Write(dt.Rows[i]["nick"].ToString() + "-");
                //        Console.Write(count + "-");
                //        Console.Write(total + "-");
                //        Console.Write(used + "\r\n");
                //    }

                //}

                //  return;
                //ShopAlert act = new ShopAlert();

                //if (1 == 1)
                //{
                //    //店铺等级和过期检查
                //    act.StartDeleteShop();
                //}



            //string sql = "SELECT result,id FROM TCS_TaobaoMsgLog WHERE isok = 0 AND typ = 'TradeSellerShip' AND nick = '老病号之家'";
            //DataTable dt = utils.ExecuteDataTable(sql);

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    try
            //    {
             //   string result = dt.Rows[i][0].ToString();

            //string result = "{\"packet\":{\"code\":202,\"msg\":{\"notify_trade\":{\"topic\":\"trade\",\"payment\":\"152.00\",\"status\":\"TradeRated\",\"type\":\"guarantee_trade\",\"modified\":\"2012-07-18 16:57:53\",\"buyer_nick\":\"evelead\",\"nick\":\"y0511\",\"oid\":161799114889581,\"is_3D\":true,\"user_id\":21775371,\"tid\":161799114889581,\"seller_nick\":\"y0511\"}}}}";

            //string result = "{\"packet\":{\"code\":202,\"msg\":{\"notify_trade\":{\"topic\":\"trade\",\"payment\":\"79.50\",\"status\":\"TradeCreate\",\"type\":\"guarantee_trade\",\"modified\":\"2012-07-19 21:16:14\",\"buyer_nick\":\"linbo528\",\"nick\":\"hh20021014\",\"oid\":162818357570036,\"is_3D\":true,\"user_id\":20733490,\"tid\":162818357570036,\"seller_nick\":\"hh20021014\"}}}}";

            //ReceiveMessage msg = new ReceiveMessage(result.ToString());
            //msg.ActData();
            //        sql = "UPDATE TCS_TaobaoMsgLog SET isok = 1 WHERE id = " + dt.Rows[i][1].ToString();
            //        Console.Write(sql + "\r\n");
            //        utils.ExecuteNonQuery(sql);

            //        sql = "INSERT INTO TCS_TaobaoMsgLogBak SELECT * FROM [TCS_TaobaoMsgLog] WHERE id = " + dt.Rows[i][1].ToString();
            //        Console.Write(sql + "\r\n");
            //        utils.ExecuteNonQuery(sql);

            //        sql = "DELETE FROM TCS_TaobaoMsgLog WHERE id = " + dt.Rows[i][1].ToString();
            //        Console.Write(sql + "\r\n");
            //        utils.ExecuteNonQuery(sql);
            //    }
            //    catch
            //    {
            //        sql = "UPDATE TCS_TaobaoMsgLog SET isok = 2 WHERE id = " + dt.Rows[i][1].ToString();
            //        Console.Write(sql + "\r\n");
            //        utils.ExecuteNonQuery(sql);
            //    }
         //   }

                //return;
            //string sql11 = "SELECT result FROM TCS_TaobaoMsgLogBak WHERE nick = 'hh20021014' AND typ = 'TradeRated'  AND DATEDIFF(D, adddate, GETDATE()) <= 2";
            ////ActRateInfoDetail(sql11);
            ////sql11 = "SELECT result FROM TCS_TaobaoMsgLogBak WHERE id = '8624578' OR id = '8593815'";
            //Console.Write(sql11 + "\r\n");
            //DataTable dt = utils.ExecuteDataTable(sql11);
            //Console.Write(dt.Rows.Count.ToString() + "\r\n");
            //string result = string.Empty;

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    result = dt.Rows[i][0].ToString();
            //    Console.Write(result + "\r\n");
            //    ReceiveMessage msg = new ReceiveMessage(result.ToString());
            //    msg.ActData();
            //    //Console.ReadLine();
            //}
                //Console.ReadLine();
                //return;
                //string sql = "SELECT * FROM TCS_Trade WHERE nick = 'lslive旗舰店'";
                //DataTable dt = utils.ExecuteDataTable(sql);
                //Console.Write(dt.Rows.Count + "....\r\n");
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    Trade trade = new TeteTopApi.Entity.Trade();
                //    trade.BuyNick = dt.Rows[i]["buynick"].ToString();
                //    trade.Nick = dt.Rows[i]["nick"].ToString();
                //    trade.Mobile = dt.Rows[i]["mobile"].ToString();

                //    trade.receiver_name = dt.Rows[i]["receiver_name"].ToString();
                //    trade.receiver_address = dt.Rows[i]["receiver_address"].ToString();
                //    trade.receiver_state = dt.Rows[i]["receiver_state"].ToString();
                //    trade.receiver_city = dt.Rows[i]["receiver_city"].ToString();
                //    trade.receiver_district = dt.Rows[i]["receiver_district"].ToString();

                //    Console.Write(trade.BuyNick + "....\r\n");

                //    GetUserData userData = new GetUserData();
                //    userData.Get(trade);
                //}

                //Api top = new Api("12132145", "1fdd2aadd5e2ac2909db2967cbb71e7f", "", "http://stream.api.taobao.com/stream");
                //string result = top.ConnectServerFree();

                //string sql = "SELECT TOP 1 * FROM TCS_TaobaoMsgLog WHERE nick = 'shujian1672' AND typ = 'TradeCreate'";
                //DataTable dt = utils.ExecuteDataTable(sql);

                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    try
                //    {
                //        string result = dt.Rows[i]["result"].ToString();

                //        string status = utils.GetValueByProperty(result, "status");
                //        string nick = utils.GetValueByProperty(result, "nick");
                //        Item item = utils.GetItem(result);

                //        TeteTopApi.Logic.WeiboSend act = new TeteTopApi.Logic.WeiboSend(item, status, nick, result);
                //        act.Start();
                //    }
                //    catch
                //    {

                //    }
                //}


            //Test test = new Test();
            //test.StartTest();
            //string msg = "{\"packet\":{\"code\":202,\"msg\":{\"notify_trade\":{\"topic\":\"trade\",\"payment\":\"39.00\",\"status\":\"TradeRated\",\"type\":\"guarantee_trade\",\"modified\":\"2012-08-01 16:07:10\",\"buyer_nick\":\"输不起的爱520\",\"nick\":\"骨头帮宠物馆\",\"oid\":195675283708645,\"is_3D\":true,\"user_id\":841815604,\"tid\":195675283708645,\"seller_nick\":\"骨头帮宠物馆\"}}}}";
            Trade trade = utils.GetTrade(msg);
            TradeSuccess suc = new TradeSuccess(trade);
            suc.Start();

            //TestShipping test = new TestShipping();
            //test.StartShipping();

            //WaitSellerConfirm test = new WaitSellerConfirm();
            //test.Start();
            //string sql = "SELECT * FROM TCS_TaobaoMsgLogBak1 WHERE nick = 'norryrock旗舰店'";
            //DataTable dt = utils.ExecuteDataTable(sql);

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    string result = dt.Rows[i]["result"].ToString();
            //    Console.Write(result + "...\r\n");
            //    ReceiveMessage msg = new ReceiveMessage(result.ToString());
            //    string ret = msg.ActDataResult();
            //    if (ret == "1")
            //    {
            //        sql = "UPDATE TCS_TaobaoMsgLogBak1 SET isok = 1 WHERE id = " + dt.Rows[i]["id"].ToString();
            //        Console.Write(sql + "...\r\n");
            //        utils.ExecuteNonQuery(sql);

            //        sql = "INSERT INTO TCS_TaobaoMsgLogBak SELECT * FROM [TCS_TaobaoMsgLogBak1] WHERE id = " + dt.Rows[i]["id"].ToString();
            //        Console.Write(sql + "...\r\n");
            //        utils.ExecuteNonQuery(sql);

            //        sql = "DELETE FROM TCS_TaobaoMsgLog WHERE id = " + dt.Rows[i]["id"].ToString();
            //        Console.Write(sql + "...\r\n");
            //        utils.ExecuteNonQuery(sql);
            //    }
            //}

            Console.ReadLine();
        }


        private static void ReflashMsgLine()
        {
            //根据昨日总消息处理量分割消息队列
            string sql = string.Empty;

            sql = "SELECT SUM(COUNT) FROM (SELECT DISTINCT nick,count FROM TCS_MsgCountLog WHERE DATEDIFF(D, adddate, GETDATE()) = 0) AS a";

            int total = int.Parse(utils.ExecuteString(sql));

            int per = total / 15;
            int id = 0;
            int temp = 0;

            Console.WriteLine(per);

            //清理排序表
            sql = "DELETE FROM TCS_MsgOrder";
            utils.ExecuteNonQuery(sql);

            //遍历昨天所有调用的用户
            sql = "SELECT DISTINCT nick,count FROM TCS_MsgCountLog WHERE DATEDIFF(D, adddate, GETDATE()) = 0";
            DataTable dt = utils.ExecuteDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                temp += int.Parse(dt.Rows[i]["count"].ToString());
                sql = "INSERT INTO TCS_MsgOrder (id, nick) VALUES ('" + id + "', '" + dt.Rows[i]["nick"].ToString() + "')";
                utils.ExecuteNonQuery(sql);

                if (temp > per)
                {
                    Console.WriteLine(temp);
                    temp = 0;
                    id++;
                }
            }
        }


        static void ActMissingMsg(string start, string end)
        {
            string AppKey = "12159997";
            string Secret = "614e40bfdb96e9063031d1a9e56fbed5";
            string Session = "";
            string Url = "http://gw.api.taobao.com/router/rest";
            string result = string.Empty;
            string nick = string.Empty;

            Api top = new Api(AppKey, Secret, Session, Url);

            IDictionary<string, string> param = new Dictionary<string, string>();
            Regex reg = new Regex(@"""user_id"":([0-9]*)", RegexOptions.IgnoreCase);

            string sql = "SELECT nick,session FROM TCS_ShopSession WHERE version <> -1 AND sid = 0 AND session <> '' AND nick = '施仙丽旗舰店'";

            //param = new Dictionary<string, string>();
            //param.Add("start", start);
            //param.Add("end", end);

            //result = top.CommonTopApi("taobao.comet.discardinfo.get", param, Session);

            Console.Write(start + "-" + end + "-");
            //Console.Read();
            //MatchCollection match = reg.Matches(result);
            //for (int i = 0; i < match.Count; i++)
            //{
            //    string sid = match[i].Groups[1].ToString();
            //    //Console.Write(match[i].Groups[1].ToString() + "\r\n");

            //    if (sid != "72104903")
            //        continue;

            //sql = "SELECT nick FROM TCS_ShopSession WHERE nick = 'rong50'";
            sql = "SELECT nick FROM TCS_ShopSession WHERE version > 1 AND nick = '施仙丽旗舰店'";
                DataTable dt = utils.ExecuteDataTable(sql);
                for (int i = 0; i < dt.Rows.Count;i++ )
                {
                    try
                    {
                        nick = dt.Rows[i][0].ToString();
                        Console.Write(nick + "taobao.increment.trades.get \r\n\r\n"); //taobao.increment.trades.get
                        param = new Dictionary<string, string>();
                        param.Add("start_modified", start);
                        //Console.Write(start + "taobao.increment.trades.get \r\n\r\n");
                        param.Add("end_modified", end);
                        //Console.Write(end + "taobao.increment.trades.get \r\n\r\n"); 
                        param.Add("nick", nick);
                        param.Add("page_no", "1");
                        param.Add("page_size", "200");
                        result = top.CommonTopApi("taobao.increment.trades.get", param, Session);
                        Console.Write(result + "taobao.increment.trades.get \r\n\r\n"); 

                        Regex reg1 = new Regex(@"\{""buyer_nick""[^\}]*\}", RegexOptions.IgnoreCase);
                        MatchCollection match1 = reg1.Matches(result);
                        for (int j = 0; j < match1.Count; j++)
                        {
                            Console.Write(match1[j].Groups[0].ToString() + "\r\n");
                            string resultNew = "\"msg\":{\"notify_trade\":" + match1[j].Groups[0].ToString() + "}";
                            LogData dbLog = new LogData();
                            Trade trade = utils.GetTrade(resultNew);
                            dbLog.InsertMsgLogInfo(trade.Nick, trade.Status, resultNew);
                        }

                        string totalpage = new Regex(@"""total_results"":([0-9]*)", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
                        if (int.Parse(totalpage) > 200)
                        {
                            int page = 0;

                            if (int.Parse(totalpage) % 200 == 0)
                            {
                                page = int.Parse(totalpage) / 200;
                            }
                            else
                            {
                                page = int.Parse(totalpage) / 200 + 1;
                            }

                            //有多页数据，继续获取
                            for (int j = 2; j <= page; j++)
                            {
                                param = new Dictionary<string, string>();
                                param.Add("start_modified", start);
                                param.Add("end_modified", end);
                                param.Add("nick", nick);
                                param.Add("page_no", page.ToString());
                                param.Add("page_size", "200");
                                result = top.CommonTopApi("taobao.increment.trades.get", param, Session);

                                Console.Write(result + "\r\n");
                                reg1 = new Regex(@"\{""buyer_nick""[^\}]*\}", RegexOptions.IgnoreCase);
                                match1 = reg1.Matches(result);
                                for (int k = 0; k < match1.Count; k++)
                                {
                                    Console.Write(match1[k].Groups[0].ToString() + "\r\n");
                                    string resultNew = "\"msg\":{\"notify_trade\":" + match1[k].Groups[0].ToString() + "}";
                                    LogData dbLog = new LogData();
                                    Trade trade = utils.GetTrade(resultNew);
                                    dbLog.InsertMsgLogInfo(trade.Nick, trade.Status, resultNew);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.Write(e.Message);
                        //Console.ReadLine();
                    }
                }
           // }
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
                    //}
                }
                catch
                {
                    sql = "UPDATE TCS_TaobaoMsgLog SET isok = 2 WHERE id = " + dt.Rows[i][1].ToString();
                    Console.Write(sql + "\r\n");
                    utils.ExecuteNonQuery(sql);
                }
            }
        }
    }

    public class TestShipping
    {
        /// <summary>
        /// 获取正常使用且有手机提醒的店铺而且最近7天内没有同类型提醒消息的
        /// </summary>
        /// <returns></returns>
        public List<ShopInfo> GetShopInfoNormalUsed()
        {
            string sql = "SELECT * FROM TCS_ShopConfig r WITH (NOLOCK) INNER JOIN TCS_ShopSession s WITH (NOLOCK) ON s.nick = r.nick WHERE isdel = 0";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                return FormatDataList(dt);
            }
            else
            {
                return new List<ShopInfo>();
            }
        }

        public void StartShopStatusAlertAddress()
        {
            //获取目前在用的而且自动赠送优惠券的
            List<ShopInfo> list = GetShopInfoNormalUsed();
            TradeData dbTrade = new TradeData();

            //循环获取这些卖家的未审核订单
            for (int i = 0; i < list.Count; i++)
            {
                ShopInfo shop = list[i];

                TopApiHaoping api = new TopApiHaoping(shop.Session);
                List<Trade> listTrade = dbTrade.GetTradeAllByNick(shop);
                Console.Write("total:[" + listTrade.Count.ToString() + "]\r\n");
                //循环获取这些卖家的未审核订单
                for (int j = 0; j < listTrade.Count; j++)
                {
                    TopApiHaoping apiCoupon = new TopApiHaoping(shop.Session);
                    Trade trade = api.GetTradeByTid(listTrade[j]);

                    string sql = "UPDATE TCS_Trade SET receiver_name = '" + trade.receiver_name + "', " +
                                "receiver_state = '" + trade.receiver_state + "', " +
                                "receiver_city = '" + trade.receiver_city + "', " +
                                "receiver_district = '" + trade.receiver_district + "', " +
                                "receiver_address = '" + trade.receiver_address + "' WHERE orderid = '" + listTrade[j].Tid + "'";
                    Console.Write(sql + "\r\n");
                    utils.ExecuteNonQuery(sql);
                }
            }
        }

        public void StartShopStatusAlert()
        {
            //获取目前在用的而且自动赠送优惠券的
            List<ShopInfo> list = GetShopInfoNormalUsed();

            TradeData dbTrade = new TradeData();
            CouponData dbCoupon = new CouponData();
            MessageData dbMessage = new MessageData();
            string typ = "status";

            //循环获取这些卖家的未审核订单
            for (int i = 0; i < list.Count; i++)
            {
                ShopInfo shop = list[i];

                TopApiHaoping api = new TopApiHaoping(shop.Session);
                List<Trade> listTrade = dbTrade.GetTradeAllByNick(shop);
                Console.Write("total:[" + listTrade.Count.ToString() + "]\r\n");
                for (int j = 0; j < listTrade.Count; j++)
                {
                    //更新订单的优惠券使用情况
                    TopApiHaoping apiCoupon = new TopApiHaoping(shop.Session);
                    string result = apiCoupon.GetCouponTradeTotalByNick(listTrade[j]);
                    Console.Write(result + "\r\n\r\n\r\n");

                    MatchCollection match = new Regex(@"<promotion_details list=""true""><promotion_detail><discount_fee>([^\<]*)</discount_fee><id>[0-9]*</id><promotion_desc>[^\<]*</promotion_desc><promotion_id>shopbonus-[0-9]*_[0-9]*-([0-9]*)</promotion_id><promotion_name>店铺优惠券</promotion_name></promotion_detail>", RegexOptions.IgnoreCase).Matches(result);

                    if (match.Count != 0)
                    {
                        string price = match[0].Groups[1].ToString();
                        string couponid = match[0].Groups[2].ToString();

                        if (couponid.Length != 0)
                        {
                            TradeData dataTradeCoupon = new TradeData();
                            dataTradeCoupon.UpdateTradeCouponInfo(listTrade[j], price, couponid);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 获取全部正常使用中的店铺
        /// </summary>
        /// <returns></returns>
        public List<ShopInfo> GetShopInfoNormalUsedAllTest()
        {
            string sql = "SELECT * FROM TCS_ShopConfig r WITH (NOLOCK) INNER JOIN TCS_ShopSession s WITH (NOLOCK) ON s.nick = r.nick WHERE isdel = 0 AND r.nick = '喜洋洋婚礼用品'";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                return FormatDataList(dt);
            }
            else
            {
                return new List<ShopInfo>();
            }
        }

        /// <summary>
        /// 获取卖家已发货且未确认的订单
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        public List<Trade> GetShippingTrade(ShopInfo shop)
        {
//            string sql = "SELECT * FROM TCS_Trade WHERE nick = '" + shop.Nick + "' AND DATEDIFF(d,shippingdate)";
//            sql = @"SELECT * FROM(
//                    SELECT * FROM TCS_Trade  WHERE nick = '回力风林专卖店' 
//                    ) AS a WHERE status = 'WAIT_BUYER_CONFIRM_GOODS'
//                    AND reviewdate IS NULL";
            string sql = "SELECT * FROM TCS_Trade WHERE nick = '" + shop.Nick + "' AND (typ IS NULL OR (typ = 'system' AND shippingdate IS NULL AND shippingstatus <> 'ACCEPTED_BY_RECEIVER')) AND reviewdate IS NULL AND mobile <> ''";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);

            return FormatDataListAA(dt);
        }

        /// <summary>
        /// 生成新格式的店铺基本设置数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<Trade> FormatDataListAA(DataTable dt)
        {
            List<Trade> infoList = new List<Trade>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Trade info = new Trade();

                info.Mobile = dt.Rows[i]["mobile"].ToString();
                info.BuyNick = dt.Rows[i]["buynick"].ToString();
                info.Tid = dt.Rows[i]["orderid"].ToString();
                info.Nick = dt.Rows[i]["nick"].ToString();
                info.OrderType = dt.Rows[i]["ordertype"].ToString();

                infoList.Add(info);
            }
            return infoList;
        }

        public void StartShipping()
        {
            //获取目前正在使用的卖家(需要改成全部卖家，否则物流状态无法获取)
            ShopData dbShop = new ShopData();
            List<ShopInfo> list = GetShopInfoNormalUsedAllTest();


            //循环判定这些卖家的订单是否物流到货
            for (int i = 0; i < list.Count; i++)
            {
                ShopInfo shop = list[i];
                //获取那些状态是发货中且没发过短信的订单给予提示
                TradeData dbTrade = new TradeData();
                List<Trade> listTrade = GetShippingTrade(shop);

                Console.Write(listTrade.Count.ToString() + "\r\n");
                for (int j = 0; j < listTrade.Count; j++)
                {
                    Trade trade = listTrade[j];
                    //获取该订单的物流状态
                    TopApiHaoping api = new TopApiHaoping(shop.Session);
                    string status = api.GetShippingStatusByTid(listTrade[j]);

                    Console.Write(status + "\r\n");
                    //如果该物流信息不存在
                    if (status.IndexOf("不存在") != -1)
                    {
                        //如果该物流公司不支持查询则更新为self
                        dbTrade.UpdateTradeShippingStatusSelf(trade, status);
                        continue;
                    }

                    trade = api.GetOrderShippingInfo(trade);

                    if (!dbTrade.IsTaobaoCompany(trade))
                    {
                        //如果不是淘宝合作物流则直接更新
                        dbTrade.UpdateTradeShippingStatusSelf(trade, status);
                    }
                    else
                    {
                        //根据服务器的物流状态进行判断，如果物流状态是已签收
                        if (status == "ACCEPTED_BY_RECEIVER" || status == "ACCEPTING" || status == "ACCEPTED")
                        {
                            string result = api.GetShippingStatusDetailByTid(trade);
                            Console.Write("【" + result + "】\r\n");
                            //如果订单不是服务器错误
                            if (result.IndexOf("company-not-support") != -1)
                            {
                                //如果该物流公司不支持查询则更新为self
                                dbTrade.UpdateTradeShippingStatusSelf(trade, status);
                                continue;
                            }

                            //再根据订单的详细物流信息判断签收的状态
                            if (result.IndexOf("签收人") != -1 || result.IndexOf(" 签收") != -1 || result.IndexOf(" 已签收") != -1 || result.IndexOf(" 妥投") != -1 || result.IndexOf("正常签收") != -1)
                            {
                                //如果物流已经签收了则更新对应订单状态
                                trade.DeliveryEnd = utils.GetShippingEndTime(result); ;
                                trade.DeliveryMsg = result;

                                //如果物流到货时间还是为空
                                if (trade.DeliveryEnd == "")
                                {
                                    LogData dbLog = new LogData();
                                    dbLog.InsertErrorLog(trade.Nick, "deliveryDateNull", "", result, "");
                                    continue;
                                }

                                //更新物流到货时间
                                dbTrade.UpdateTradeShippingStatusSystem(trade, status);

                                //发送短信-上LOCK锁定
                          
                                    //判断同类型的短信该客户今天是否只收到一条
                                    ShopData db = new ShopData();
                                    if (!db.IsSendMsgOrder(trade, "shipping"))
                                    {
                                        //判断该用户是否开启了发货短信
                                        if (shop.MsgIsShipping == "1" && int.Parse(shop.MsgCount) > 0)
                                        {
                                            //发送短信
                                            string msg = Message.GetMsg(shop.MsgShippingContent, shop.MsgShopName, trade.BuyNick, shop.IsCoupon);
                                            string msgResult = Message.Send(trade.Mobile, msg);
                                            Console.WriteLine(msg);

                                            //记录
                                            if (msgResult != "0")
                                            {
                                                db.InsertShopMsgLog(shop, trade, msg, msgResult, "shipping");
                                            }
                                            else
                                            {
                                                db.InsertShopErrMsgLog(shop, trade, msg, msgResult, "shipping");
                                            }

                                            shop.MsgCount = (int.Parse(shop.MsgCount) - 1).ToString();
                                        }
                                    }
                                
                            }
                        }
                    }

                    Console.ReadLine();
                }
            }
        }



        public List<ShopInfo> ShopInfoListAll()
        {
            string sql = "SELECT * FROM TCS_ShopConfig r WITH (NOLOCK) INNER JOIN TCS_ShopSession s WITH (NOLOCK) ON s.nick = r.nick WHERE r.nick = '跃动运动专营店'";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                return FormatDataList(dt);
            }
            else
            {
                return new List<ShopInfo>();
            }
        }


        public void StartDeleteShop()
        {
            //获取全部店铺
            ShopData dbShop = new ShopData();
            List<ShopInfo> list = ShopInfoListAll();

            //通过接口获取这些店铺的到期时间
            for (int i = 0; i < list.Count; i++)
            {
                try
                {
                    ShopInfo shop = list[i];
                    Console.Write(shop.Nick + "\r\n");
                    TopApiHaoping api = new TopApiHaoping(shop.Session);
                    string result = api.GetUserExpiredDate(shop);
                    Console.Write(result + "\r\n");
                    //判断是否过期
                    if (result.IndexOf("\"article_user_subscribes\":{}") != -1)
                    {
                        //更新为del状态
                        dbShop.DeleteShop(shop);
                    }
                    else
                    {
                        int isok = 0;
                        Regex reg = new Regex(@"""item_code"":""([^""]*)""", RegexOptions.IgnoreCase);
                        //更新店铺的版本号
                        MatchCollection match = reg.Matches(result);
                        for (int j = 0; j < match.Count; j++)
                        {
                            shop.Version = match[j].Groups[1].ToString().Replace("service-0-22904-", "");

                            if (shop.Version == "9")
                            {
                                shop.Version = "3";
                            }

                            if (int.Parse(shop.Version) <= 3)
                            {
                                isok = 1;
                                break;
                            }
                        }

                        if (isok == 0)
                        {
                            shop.Version = "0";
                        }

                        dbShop.ActiveShopVersion(shop);
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message.ToString() + "\r\n");
                }
            }
        }
        /// <summary>
        /// 获取全部正常使用中的店铺
        /// </summary>
        /// <returns></returns>
        public List<ShopInfo> GetShopInfoNormalUsedAll()
        {
            string sql = "SELECT * FROM TCS_ShopConfig r WITH (NOLOCK) INNER JOIN TCS_ShopSession s WITH (NOLOCK) ON s.nick = r.nick WHERE isdel = 0 AND r.nick = 'ishow生活'";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                return FormatDataList(dt);
            }
            else
            {
                return new List<ShopInfo>();
            }
        }


        /// <summary>
        /// 生成新格式的店铺基本设置数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<ShopInfo> FormatDataList(DataTable dt)
        {
            List<ShopInfo> infoList = new List<ShopInfo>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ShopInfo info = new ShopInfo();

                info.CouponID = dt.Rows[i]["couponid"].ToString().Trim();
                info.IsKefu = dt.Rows[i]["iskefu"].ToString();
                info.MinDateSelf = dt.Rows[i]["maxdate"].ToString();
                info.MinDateSystem = dt.Rows[i]["mindate"].ToString();
                info.MsgCount = dt.Rows[i]["total"].ToString();
                info.MsgCouponContent = dt.Rows[i]["giftcontent"].ToString();
                info.MsgFahuoContent = dt.Rows[i]["fahuocontent"].ToString();
                info.MsgIsCoupon = dt.Rows[i]["giftflag"].ToString();
                info.MsgIsFahuo = dt.Rows[i]["fahuoflag"].ToString();
                info.MsgIsReview = dt.Rows[i]["reviewflag"].ToString();
                info.MsgReviewTime = dt.Rows[i]["reviewtime"].ToString();
                info.MsgIsShipping = dt.Rows[i]["shippingflag"].ToString();
                info.MsgReviewContent = dt.Rows[i]["reviewcontent"].ToString();
                info.MsgShippingContent = dt.Rows[i]["shippingcontent"].ToString();
                info.MsgShopName = dt.Rows[i]["shopname"].ToString();
                info.Nick = dt.Rows[i]["nick"].ToString();
                info.Session = dt.Rows[i]["session"].ToString();
                info.Version = dt.Rows[i]["version"].ToString();
                info.IsCancelAuto = dt.Rows[i]["IsCancelAuto"].ToString();
                info.IsKeyword = dt.Rows[0]["IsKeyword"].ToString();
                info.Keyword = dt.Rows[i]["Keyword"].ToString();
                info.WordCount = dt.Rows[i]["WordCount"].ToString();
                info.IsCoupon = dt.Rows[i]["IsCoupon"].ToString();
                info.Mobile = dt.Rows[i]["phone"].ToString();

                infoList.Add(info);
            }
            return infoList;
        }

        public void Start()
        {
            //获取目前正在使用的卖家(需要改成全部卖家，否则物流状态无法获取)
            ShopData dbShop = new ShopData();
            List<ShopInfo> list = GetShopInfoNormalUsedAll();

            //循环判定这些卖家的订单是否物流到货
            for (int i = 0; i < list.Count; i++)
            {
                ShopInfo shop = list[i];
                TopApiHaoping api11 = new TopApiHaoping(shop.Session);

                Trade trade111 = new TeteTopApi.Entity.Trade();
                trade111.Nick = shop.Nick;
                trade111.Tid = "122725979323660";
                string result11 = api11.GetShippingStatusDetailByTid(trade111);
                Console.Write(result11 + "..\r\n");
                return;

                //获取那些状态是发货中且没发过短信的订单给予提示
                TradeData dbTrade = new TradeData();
                List<Trade> listTrade = dbTrade.GetShippingTrade(shop);

                Console.Write(listTrade.Count.ToString() + "\r\n");
                for (int j = 0; j < listTrade.Count; j++)
                {
                    Trade trade = listTrade[j];
                    //获取该订单的物流状态
                    TopApiHaoping api = new TopApiHaoping(shop.Session);
                    string status = api.GetShippingStatusByTid(listTrade[j]);

                    Console.Write(status + "\r\n");
                    //如果该物流信息不存在
                    if (status.IndexOf("不存在") != -1)
                    {
                        //如果该物流公司不支持查询则更新为self
                        dbTrade.UpdateTradeShippingStatusSelf(trade, status);
                        continue;
                    }

                    trade = api.GetOrderShippingInfo(trade);

                    if (!dbTrade.IsTaobaoCompany(trade))
                    {
                        //如果不是淘宝合作物流则直接更新
                        dbTrade.UpdateTradeShippingStatusSelf(trade, status);
                    }
                    else
                    {
                        //根据服务器的物流状态进行判断，如果物流状态是已签收
                        if (status == "ACCEPTED_BY_RECEIVER")
                        {
                            string result = api.GetShippingStatusDetailByTid(trade);
                            Console.Write("【" + result + "】\r\n");
                            //如果订单不是服务器错误
                            if (result.IndexOf("company-not-support") != -1)
                            {
                                //如果该物流公司不支持查询则更新为self
                                dbTrade.UpdateTradeShippingStatusSelf(trade, status);
                                continue;
                            }

                            //再根据订单的详细物流信息判断签收的状态
                            if (result.IndexOf("签收人") != -1 || result.IndexOf("正常签收录入扫描") != -1)
                            {
                                //如果物流已经签收了则更新对应订单状态
                                trade.DeliveryEnd = utils.GetShippingEndTime(result); ;
                                trade.DeliveryMsg = result;

                                //如果物流到货时间还是为空
                                if (trade.DeliveryEnd == "")
                                {
                                    LogData dbLog = new LogData();
                                    dbLog.InsertErrorLog(trade.Nick, "deliveryDateNull", "", result, "");
                                    continue;
                                }

                                //更新物流到货时间
                                dbTrade.UpdateTradeShippingStatusSystem(trade, status);

                                //发送短信-上LOCK锁定
                      
                                    //判断同类型的短信该客户今天是否只收到一条
                                    ShopData db = new ShopData();
                                    if (!db.IsSendMsgOrder(trade, "shipping"))
                                    {
                                        //判断该用户是否开启了发货短信
                                        if (shop.MsgIsShipping == "1" && int.Parse(shop.MsgCount) > 0)
                                        {
                                            //发送短信
                                            string msg = Message.GetMsg(shop.MsgShippingContent, shop.MsgShopName, trade.BuyNick, shop.IsCoupon);
                                            string msgResult = Message.Send(trade.Mobile, msg);

                                            //记录
                                            if (msgResult != "0")
                                            {
                                                db.InsertShopMsgLog(shop, trade, msg, msgResult, "shipping");
                                            }
                                            else
                                            {
                                                db.InsertShopErrMsgLog(shop, trade, msg, msgResult, "shipping");
                                            }

                                            shop.MsgCount = (int.Parse(shop.MsgCount) - 1).ToString();
                                        }
                                    }
                                
                            }
                        }
                    }

                    //return;
                }
            }
        }
    }

    public class Test
    {
        /// <summary>
        /// 获取当前正在使用物流到货短信通知并有短信可发的卖家信息(增加延迟发货短信的卖家，否则无法获取物流状态)
        /// </summary>
        /// <param name="nick"></param>
        /// <returns></returns>
        public List<ShopInfo> GetShopInfoListShippingAlert()
        {
            string sql = "SELECT * FROM TCS_ShopConfig r WITH (NOLOCK) INNER JOIN TCS_ShopSession s WITH (NOLOCK) ON s.nick = r.nick WHERE isdel = 0 AND r.nick = 'saber318888'";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                return FormatDataList(dt);
            }
            else
            {
                return new List<ShopInfo>();
            }
        }


        /// <summary>
        /// 生成新格式的店铺基本设置数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<ShopInfo> FormatDataList(DataTable dt)
        {
            List<ShopInfo> infoList = new List<ShopInfo>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ShopInfo info = new ShopInfo();

                info.CouponID = dt.Rows[i]["couponid"].ToString().Trim();
                info.IsKefu = dt.Rows[i]["iskefu"].ToString();
                info.MinDateSelf = dt.Rows[i]["maxdate"].ToString();
                info.MinDateSystem = dt.Rows[i]["mindate"].ToString();
                info.MsgCount = dt.Rows[i]["total"].ToString();
                info.MsgCouponContent = dt.Rows[i]["giftcontent"].ToString();
                info.MsgFahuoContent = dt.Rows[i]["fahuocontent"].ToString();
                info.MsgIsCoupon = dt.Rows[i]["giftflag"].ToString();
                info.MsgIsFahuo = dt.Rows[i]["fahuoflag"].ToString();
                info.MsgIsReview = dt.Rows[i]["reviewflag"].ToString();
                info.MsgReviewTime = dt.Rows[i]["reviewtime"].ToString();
                info.MsgIsShipping = dt.Rows[i]["shippingflag"].ToString();
                info.MsgReviewContent = dt.Rows[i]["reviewcontent"].ToString();
                info.MsgShippingContent = dt.Rows[i]["shippingcontent"].ToString();
                info.MsgShopName = dt.Rows[i]["shopname"].ToString();
                info.Nick = dt.Rows[i]["nick"].ToString();
                info.Session = dt.Rows[i]["session"].ToString();
                info.Version = dt.Rows[i]["version"].ToString();
                info.IsCancelAuto = dt.Rows[i]["IsCancelAuto"].ToString();
                info.Keyword = dt.Rows[i]["Keyword"].ToString();
                info.WordCount = dt.Rows[i]["WordCount"].ToString();
                info.IsCoupon = dt.Rows[i]["IsCoupon"].ToString();
                info.Mobile = dt.Rows[i]["phone"].ToString();

                infoList.Add(info);
            }
            return infoList;
        }

        public void StartTest()
        {
            //获取目前正在使用的卖家(需要改成全部卖家，否则物流状态无法获取)
            List<ShopInfo> list = GetShopInfoListShippingAlert();

            //循环判定这些卖家的订单是否物流到货
            for (int i = 0; i < list.Count; i++)
            {
                ShopInfo shop = list[i];
                //获取那些状态是发货中且没发过短信的订单给予提示
                TradeData dbTrade = new TradeData();
                List<Trade> listTrade = dbTrade.GetShippingTrade(shop);

                Console.Write(listTrade.Count.ToString() + "\r\n");
                for (int j = 0; j < listTrade.Count; j++)
                {
                    Trade trade = listTrade[j];

                    Console.Write(trade.BuyNick + "\r\n");

                    //获取该订单的物流状态
                    TopApiHaoping api = new TopApiHaoping(shop.Session);
                    string status = api.GetShippingStatusByTid(listTrade[j]);

                    Console.Write(status + "\r\n");
                    //如果该物流信息不存在
                    if (status.IndexOf("不存在") != -1)
                    {
                        //如果该物流公司不支持查询则更新为self
                        dbTrade.UpdateTradeShippingStatusSelf(trade, status);
                        continue;
                    }

                    trade = api.GetOrderShippingInfo(trade);

                    if (!dbTrade.IsTaobaoCompany(trade))
                    {
                        //如果不是淘宝合作物流则直接更新
                        dbTrade.UpdateTradeShippingStatusSelf(trade, status);
                    }
                    else
                    {
                        //根据服务器的物流状态进行判断，如果物流状态是已签收
                        if (status == "ACCEPTED_BY_RECEIVER")
                        {
                            string result = api.GetShippingStatusDetailByTid(trade);
                            Console.Write("【" + result + "】\r\n");
                            //如果订单不是服务器错误
                            if (result.IndexOf("company-not-support") != -1)
                            {
                                //如果该物流公司不支持查询则更新为self
                                dbTrade.UpdateTradeShippingStatusSelf(trade, status);
                                continue;
                            }

                            //再根据订单的详细物流信息判断签收的状态
                            if (result.IndexOf("签收人") != -1 || result.IndexOf(" 签收") != -1 || result.IndexOf("正常签收") != -1)
                            {
                                //如果物流已经签收了则更新对应订单状态
                                trade.DeliveryEnd = utils.GetShippingEndTime(result); ;
                                trade.DeliveryMsg = result;

                                //如果物流到货时间还是为空
                                if (trade.DeliveryEnd == "")
                                {
                                    LogData dbLog = new LogData();
                                    dbLog.InsertErrorLog(trade.Nick, "deliveryDateNull", "", result, "");
                                    continue;
                                }

                                //更新物流到货时间
                                dbTrade.UpdateTradeShippingStatusSystem(trade, status);

                                //发送短信-上LOCK锁定
                                //lock (padlock1)
                                //{
                                //判断同类型的短信该客户今天是否只收到一条
                                ShopData db = new ShopData();
                                if (!db.IsSendMsgOrder(trade, "shipping"))
                                {
                                    //判断该用户是否开启了发货短信
                                    if (shop.MsgIsShipping == "1" && int.Parse(shop.MsgCount) > 0)
                                    {
                                        //发送短信
                                        string msg = Message.GetMsg(shop.MsgShippingContent, shop.MsgShopName, trade.BuyNick, shop.IsCoupon);
                                        string msgResult = Message.Send(trade.Mobile, msg);

                                        //记录
                                        if (msgResult != "0")
                                        {
                                            db.InsertShopMsgLog(shop, trade, msg, msgResult, "shipping");
                                        }
                                        else
                                        {
                                            db.InsertShopErrMsgLog(shop, trade, msg, msgResult, "shipping");
                                        }

                                        shop.MsgCount = (int.Parse(shop.MsgCount) - 1).ToString();
                                    }
                                }
                                //}
                            }
                        }
                    }

                    Console.ReadLine();
                }
            }
        }

        /// <summary>
        /// 执行SQL语句返回DataTable
        /// </summary>
        /// <param name="dbstring">SQL语句</param>
        /// <returns>返回结果的DataTable</returns>
        public static DataTable ExecuteDataTable(string dbstring)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(dbstring);
            DataTable dt = null;

            try
            {
                dt = db.ExecuteDataSet(dbCommand).Tables[0];
            }
            catch (Exception e)
            {
                db = null;
            }

            return dt;
        }
    }


    public class WaitSellerConfirm
    {
        private static object padlock4 = new object();

        public List<ShopInfo> GetShopInfoListShippingAlert()
        {
            string sql = "SELECT * FROM TCS_ShopConfig r WITH (NOLOCK) INNER JOIN TCS_ShopSession s WITH (NOLOCK) ON s.nick = r.nick WHERE isdel = 0 AND r.nick = '迅泽点卡专营店'";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                return FormatDataList(dt);
            }
            else
            {
                return new List<ShopInfo>();
            }
        }

        private List<ShopInfo> FormatDataList(DataTable dt)
        {
            List<ShopInfo> infoList = new List<ShopInfo>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ShopInfo info = new ShopInfo();

                info.CouponID = dt.Rows[i]["couponid"].ToString().Trim();
                info.IsKefu = dt.Rows[i]["iskefu"].ToString();
                info.MinDateSelf = dt.Rows[i]["maxdate"].ToString();
                info.MinDateSystem = dt.Rows[i]["mindate"].ToString();
                info.MsgCount = dt.Rows[i]["total"].ToString();
                info.MsgCouponContent = dt.Rows[i]["giftcontent"].ToString();
                info.MsgFahuoContent = dt.Rows[i]["fahuocontent"].ToString();
                info.MsgIsCoupon = dt.Rows[i]["giftflag"].ToString();
                info.MsgIsFahuo = dt.Rows[i]["fahuoflag"].ToString();
                info.MsgIsReview = dt.Rows[i]["reviewflag"].ToString();
                info.MsgReviewTime = dt.Rows[i]["reviewtime"].ToString();
                info.MsgIsShipping = dt.Rows[i]["shippingflag"].ToString();
                info.MsgReviewContent = dt.Rows[i]["reviewcontent"].ToString();
                info.MsgShippingContent = dt.Rows[i]["shippingcontent"].ToString();
                info.MsgShopName = dt.Rows[i]["shopname"].ToString();
                info.Nick = dt.Rows[i]["nick"].ToString();
                info.Session = dt.Rows[i]["session"].ToString();
                info.Version = dt.Rows[i]["version"].ToString();
                info.IsCancelAuto = dt.Rows[i]["IsCancelAuto"].ToString();
                info.Keyword = dt.Rows[i]["Keyword"].ToString();
                info.WordCount = dt.Rows[i]["WordCount"].ToString();
                info.IsCoupon = dt.Rows[i]["IsCoupon"].ToString();
                info.Mobile = dt.Rows[i]["phone"].ToString();

                info.IsXuni = dt.Rows[i]["IsXuni"].ToString();
                info.XuniDate = dt.Rows[i]["XuniDate"].ToString();

                infoList.Add(info);
            }
            return infoList;
        }

        public void Start()
        {
            //获取目前正在使用的卖家
            ShopData dbShop = new ShopData();
            List<ShopInfo> list = GetShopInfoListShippingAlert();

            //循环判定卖家是否到提醒发货时间了
            Console.Write(list.Count.ToString() + "\r\n");
            for (int i = 0; i < list.Count; i++)
            {
                ShopInfo shop = list[i];
                Console.Write(shop.Nick + "-" + shop.MsgReviewTime + "-" + DateTime.Now.Hour.ToString() + "-" + shop.IsXuni.ToString() + "\r\n");
                if (DateTime.Now.Hour.ToString() == list[i].MsgReviewTime || shop.IsXuni == "1")
                {
                    //获取那些延迟不确认且没发过短信的订单给予提示
                    TradeData dbTrade = new TradeData();
                    List<Trade> listTrade = new List<Trade>();

                    if (shop.IsXuni == "1")
                    {
                        //如果是虚拟商品
                        listTrade = dbTrade.GetUnconfirmTradeXuni(shop);
                    }
                    else
                    {
                        listTrade = dbTrade.GetUnconfirmTrade(shop);
                    }

                    Console.Write(listTrade.Count.ToString() + "\r\n");
                    for (int j = 0; j < listTrade.Count; j++)
                    {
                        Trade trade = listTrade[j];

                        //判断如果是分销的订单，则不处理
                        if (trade.OrderType.ToLower() == "fenxiao")
                        {
                            return;
                        }

                        //发送短信-上LOCK锁定
                        lock (padlock4)
                        {
                            //判断同类型的短信该客户今天是否只收到一条
                            ShopData db = new ShopData();
                            if (!db.IsSendMsgOrder(trade, "review") && !db.IsSendMsgToday(trade, "review"))
                            {
                                //判断该用户是否开启了发货短信
                                if (shop.MsgIsReview == "1" && int.Parse(shop.MsgCount) > 0)
                                {
                                    //发送短信
                                    //string msg = Message.GetMsg(shop.MsgReviewContent, shop.MsgShopName, trade.BuyNick, shop.IsCoupon);
                                    string msg = Message.GetMsg(shop, trade, shop.MsgReviewContent);
                                    Console.WriteLine(trade.Mobile);
                                    Console.WriteLine(msg);
                                    string msgResult = Message.Send(trade.Mobile, msg);

                                    Console.WriteLine(msgResult);

                                    //记录
                                    if (msgResult != "0")
                                    {
                                        db.InsertShopMsgLog(shop, trade, msg, msgResult, "review");
                                    }
                                    else
                                    {
                                        db.InsertShopErrMsgLog(shop, trade, msg, msgResult, "review");
                                    }
                                    shop.MsgCount = (int.Parse(shop.MsgCount) - 1).ToString();
                                }
                            }
                        }

                        Console.Read();
                    }
                }
            }
        }
    }
}
