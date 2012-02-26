using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeteTopApi.TopApi;
using System.Text.RegularExpressions;
using TeteTopApi.Entity;
using TeteTopApi.DataContract;
using System.Data;

namespace TeteTopApi.Logic
{
    public class LostMessageGet
    {
        public void Start()
        {
            string status = string.Empty;
            string nick = string.Empty;
            string start = "2011-12-03 00:00:00";
            string end = "2011-12-03 23:59:59";
            string insertDate = string.Empty;

            //nick = "artka官方旗舰店";
            //ShopData dbShop = new ShopData();
            //List<ShopInfo> shopList = dbShop.GetShopInfoNormalUsedAll();
            string sql = "SELECT * FROM TCS_Shopconfig WHERE sessionold IS NULL AND isdel = 0 AND LEN(couponid) > 0 ORDER BY starttime DESC";
            DataTable dt = utils.ExecuteDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                nick = dt.Rows[i]["nick"].ToString();
                insertDate = dt.Rows[i]["starttime"].ToString();
                //按照时间进行遍历
                DateTime startDate = DateTime.Parse(insertDate);
                while (!(startDate.Day == DateTime.Now.Day))
                {
                    if (startDate.Day.ToString().Length == 1)
                    {
                        start = startDate.Year + "-" + startDate.Month + "-0" + startDate.Day + " 00:00:00";
                        end = startDate.Year + "-" + startDate.Month + "-0" + startDate.Day + " 23:59:59";

                        status = "TradeSellerShip";
                        GetMessage(status, nick, start, end);
                        status = "TradeRated";
                        GetMessage(status, nick, start, end);
                        Console.Write(start + "-" + end + "\r\n");
                    }
                    startDate = startDate.AddDays(1);
                }

                //start = "2011-12-04 00:00:00";
                //end = "2011-12-04 18:21:00";
                //status = "TradeSellerShip";
                //GetMessage(status, nick, start, end);
                //status = "TradeRated";
                //GetMessage(status, nick, start, end);
            }
        }

        private void GetMessage(string status, string nick, string start, string end)
        {
            try
            {
                TopApiHaoping api = new TopApiHaoping("");

                string result = api.GetLostMessage(nick, status, "1", start, end);
                Console.Write(result + "\r\n");
                MatchCollection match = new Regex(@"{""buyer[^\}]*""}", RegexOptions.IgnoreCase).Matches(result);
                for (int i = 0; i < match.Count; i++)
                {
                    Console.Write("\"msg\":{\"notify_trade\":" + match[i].Groups[0].ToString() + "}" + "\r\n");
                    ReceiveMessage msg = new ReceiveMessage("\"msg\":{\"notify_trade\":" + match[i].Groups[0].ToString() + "}");
                    msg.ActData();
                }

                string totalpage = new Regex(@"""total_results"":([0-9]*)", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
                //Console.Write(totalpage + "\r\n");
                if (int.Parse(totalpage) > 40)
                {
                    int page = 0;

                    if (int.Parse(totalpage) % 40 == 0)
                    {
                        page = int.Parse(totalpage) / 40;
                    }
                    else
                    {
                        page = int.Parse(totalpage) / 40 + 1;
                    }

                    //有多页数据，继续获取
                    for (int j = 2; j <= page; j++)
                    {
                        result = api.GetLostMessage(nick, status, j.ToString(), start, end);
                        Console.Write(result + "\r\n");
                        match = new Regex(@"{""buyer[^\}]*""}", RegexOptions.IgnoreCase).Matches(result);
                        for (int i = 0; i < match.Count; i++)
                        {
                            Console.Write("\"msg\":{\"notify_trade\":" + match[i].Groups[0].ToString() + "}" + "\r\n");
                            ReceiveMessage msg = new ReceiveMessage("\"msg\":{\"notify_trade\":" + match[i].Groups[0].ToString() + "}");
                            msg.ActData();
                        }
                    }
                }
            }
            catch { }
        }
    }
}
