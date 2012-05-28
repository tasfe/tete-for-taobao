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
        public void Start(string start, string end)
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

            string sql = "SELECT nick,session FROM TCS_ShopSession WHERE version <> -1 AND sid = 0 AND session <> ''";

            param = new Dictionary<string, string>();
            param.Add("start", start);
            param.Add("end", end);

            result = top.CommonTopApi("taobao.comet.discardinfo.get", param, Session);

            Console.Write(result + "-");
            MatchCollection match = reg.Matches(result);
            for (int i = 0; i < match.Count; i++)
            {
                string sid = match[i].Groups[1].ToString();
                //Console.Write(match[i].Groups[1].ToString() + "\r\n");

                sql = "SELECT nick FROM TCS_ShopSession WHERE sid = '" + sid + "'";
                DataTable dt = utils.ExecuteDataTable(sql);
                if (dt.Rows.Count != 0)
                {
                    nick = dt.Rows[0][0].ToString();
                    //Console.Write(nick + "\r\n\r\n"); taobao.increment.trades.get
                    param = new Dictionary<string, string>();
                    param.Add("start_modified", start);
                    param.Add("end_modified", end);
                    param.Add("nick", nick);
                    param.Add("page_no", "1");
                    param.Add("page_size", "200");
                    result = top.CommonTopApi("taobao.increment.trades.get", param, Session);

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
            }
        }
    }
}
