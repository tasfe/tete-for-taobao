using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeteIosTrain;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.Text;

public partial class api_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Log(Request.Url.ToString());

        string act = Common.utils.NewRequest("act", Common.utils.RequestType.QueryString);
        if (act == "verify")
        {
            OutPutVerify();
        }

        if (act == "token")
        {
            GetToken();
        }

        if (act == "buy")
        {
            BuyInfo();
        }

        if (act == "about")
        {
            GetAbout();
        }

        if (act == "encodetoken")
        {
            GetEncodeToken();
        }

        if (act == "review")
        {
            InitReview();
        }

        if (act == "verifyorder")
        {
            OutPutVerifyOrder();
        }

        if (act == "login")
        {
            LoginPost();
        }

        if (act == "waitingorder")
        {
            GetWaitingOrder();
        }

        if (act == "myorder")
        {
            GetMyOrder();
        }

        if (act == "search")
        {
            SearchPost();
        }

        if (act == "detail")
        {
            SearchDetailPost();
        }

        //车票预下单
        if (act == "order")
        {
            TicketOrderPost();
        }

        //车票预下单
        if (act == "preorder")
        {
            PreSubmitOrderPost();
        }

        //车票正式下单
        if (act == "submit")
        {
            SubmitOrderPost();
        }

        //排队获取订单号
        if (act == "getorder")
        {
            GetOrderPost();
        }

        //排队获取订单号
        if (act == "getip")
        {
            GetIP();
        }

        //订单支付
        if (act == "pay")
        {
            OrderPayPost();
        }

        //取消订单
        if (act == "cancel")
        {
            OrderCancelPost();
        }

        //订单退票
        if (act == "returnticket")
        {
            ReturnTicketPost();
        }

        //订单下单状态查询
        if (act == "statussearch")
        {
            StatusSearchPost();
        }

        //添加联系人
        if (act == "personadd")
        {
            PersonActPost("add");
        }

        //联系人查询
        if (act == "personsearch")
        {
            PersonActPost("search");
        }

        //编辑联系人
        if (act == "personedit")
        {
            PersonActPost("edit");
        }

        //删除联系人
        if (act == "persondel")
        {
            PersonActPost("del");
        }
    }

    private void GetAbout()
    {
        Response.Write("特特软件 版本所有");
        Response.End();
    }

    private void GetEncodeToken()
    {
        //string keyword = "tetesoft%&^*%&^*";
        string token = Common.utils.NewRequest("token", Common.utils.RequestType.Form);
        string keyword1 = "tetesoft%&^*%&^*";
        //string code = AESencode.DecryptString(token, "tetesoft%&^*%&^*");

        Response.Write("0");
    }


    /// <summary>
    /// 购买接口
    /// </summary>
    private void BuyInfo()
    {
        string token = Common.utils.NewRequest("token", Common.utils.RequestType.Form);
        string data = Common.utils.NewRequest("data", Common.utils.RequestType.Form);
        //File.WriteAllText(Server.MapPath(DateTime.Now.Ticks.ToString() + ".txt"), data);
        //data = @"{""receipt-data"":""ewoJInNpZ25hdHVyZSIgPSAiQXNQOVNQRThhdUFTV2lwMkhvL1lYaHZua1VTMmRXMUxmdHlTcmdyTzh6TmZ6a3QyNFZTNHVqQ2VvOHpsS2s0MCtUOTR6TXpBaE4xTVNOUnA4ZlFzYloxSDhzSG0yaXNsMXZlNkZIVUhVL3RCdGNNbE8zbHBOVzlGYjNZT3oyRXFESnVLYkhlZzI5cEM1c3VId053Mi9ObDc4dVp3K21HNkNKWUF2dmVSdjY0K0FBQURWekNDQTFNd2dnSTdvQU1DQVFJQ0NHVVVrVTNaV0FTMU1BMEdDU3FHU0liM0RRRUJCUVVBTUg4eEN6QUpCZ05WQkFZVEFsVlRNUk13RVFZRFZRUUtEQXBCY0hCc1pTQkpibU11TVNZd0pBWURWUVFMREIxQmNIQnNaU0JEWlhKMGFXWnBZMkYwYVc5dUlFRjFkR2h2Y21sMGVURXpNREVHQTFVRUF3d3FRWEJ3YkdVZ2FWUjFibVZ6SUZOMGIzSmxJRU5sY25ScFptbGpZWFJwYjI0Z1FYVjBhRzl5YVhSNU1CNFhEVEE1TURZeE5USXlNRFUxTmxvWERURTBNRFl4TkRJeU1EVTFObG93WkRFak1DRUdBMVVFQXd3YVVIVnlZMmhoYzJWU1pXTmxhWEIwUTJWeWRHbG1hV05oZEdVeEd6QVpCZ05WQkFzTUVrRndjR3hsSUdsVWRXNWxjeUJUZEc5eVpURVRNQkVHQTFVRUNnd0tRWEJ3YkdVZ1NXNWpMakVMTUFrR0ExVUVCaE1DVlZNd2daOHdEUVlKS29aSWh2Y05BUUVCQlFBRGdZMEFNSUdKQW9HQkFNclJqRjJjdDRJclNkaVRDaGFJMGc4cHd2L2NtSHM4cC9Sd1YvcnQvOTFYS1ZoTmw0WElCaW1LalFRTmZnSHNEczZ5anUrK0RyS0pFN3VLc3BoTWRkS1lmRkU1ckdYc0FkQkVqQndSSXhleFRldngzSExFRkdBdDFtb0t4NTA5ZGh4dGlJZERnSnYyWWFWczQ5QjB1SnZOZHk2U01xTk5MSHNETHpEUzlvWkhBZ01CQUFHamNqQndNQXdHQTFVZEV3RUIvd1FDTUFBd0h3WURWUjBqQkJnd0ZvQVVOaDNvNHAyQzBnRVl0VEpyRHRkREM1RllRem93RGdZRFZSMFBBUUgvQkFRREFnZUFNQjBHQTFVZERnUVdCQlNwZzRQeUdVakZQaEpYQ0JUTXphTittVjhrOVRBUUJnb3Foa2lHOTJOa0JnVUJCQUlGQURBTkJna3Foa2lHOXcwQkFRVUZBQU9DQVFFQUVhU2JQanRtTjRDL0lCM1FFcEszMlJ4YWNDRFhkVlhBZVZSZVM1RmFaeGMrdDg4cFFQOTNCaUF4dmRXLzNlVFNNR1k1RmJlQVlMM2V0cVA1Z204d3JGb2pYMGlreVZSU3RRKy9BUTBLRWp0cUIwN2tMczlRVWU4Y3pSOFVHZmRNMUV1bVYvVWd2RGQ0TndOWXhMUU1nNFdUUWZna1FRVnk4R1had1ZIZ2JFL1VDNlk3MDUzcEdYQms1MU5QTTN3b3hoZDNnU1JMdlhqK2xvSHNTdGNURXFlOXBCRHBtRzUrc2s0dHcrR0szR01lRU41LytlMVFUOW5wL0tsMW5qK2FCdzdDMHhzeTBiRm5hQWQxY1NTNnhkb3J5L0NVdk02Z3RLc21uT09kcVRlc2JwMGJzOHNuNldxczBDOWRnY3hSSHVPTVoydG04bnBMVW03YXJnT1N6UT09IjsKCSJwdXJjaGFzZS1pbmZvIiA9ICJld29KSW05eWFXZHBibUZzTFhCMWNtTm9ZWE5sTFdSaGRHVXRjSE4wSWlBOUlDSXlNREV5TFRBM0xUSTJJREExT2pVd09qQTRJRUZ0WlhKcFkyRXZURzl6WDBGdVoyVnNaWE1pT3dvSkluVnVhWEYxWlMxcFpHVnVkR2xtYVdWeUlpQTlJQ0kxTURjM09URTFPVFJrWVRrME5qWTBaREUzTmpreE5HUmlNVEU0TVdZMFlUUm1OV0k1TTJVeklqc0tDU0p2Y21sbmFXNWhiQzEwY21GdWMyRmpkR2x2YmkxcFpDSWdQU0FpTVRBd01EQXdNREExTXpRNE56UXhPQ0k3Q2draVluWnljeUlnUFNBaU1TNHdJanNLQ1NKMGNtRnVjMkZqZEdsdmJpMXBaQ0lnUFNBaU1UQXdNREF3TURBMU16UTROelF4T0NJN0Nna2ljWFZoYm5ScGRIa2lJRDBnSWpFaU93b0pJbTl5YVdkcGJtRnNMWEIxY21Ob1lYTmxMV1JoZEdVdGJYTWlJRDBnSWpFek5ETXpNRGN3TURnek5ERWlPd29KSW5CeWIyUjFZM1F0YVdRaUlEMGdJbU52YlM1amIyTnZMbk50YzE4eE1DSTdDZ2tpYVhSbGJTMXBaQ0lnUFNBaU5UUTJOVGN6TXpBeElqc0tDU0ppYVdRaUlEMGdJbU52YlM1amIyTnZMblJwYldsdVoxTk5VeUk3Q2draWNIVnlZMmhoYzJVdFpHRjBaUzF0Y3lJZ1BTQWlNVE0wTXpNd056QXdPRE0wTVNJN0Nna2ljSFZ5WTJoaGMyVXRaR0YwWlNJZ1BTQWlNakF4TWkwd055MHlOaUF4TWpvMU1Eb3dPQ0JGZEdNdlIwMVVJanNLQ1NKd2RYSmphR0Z6WlMxa1lYUmxMWEJ6ZENJZ1BTQWlNakF4TWkwd055MHlOaUF3TlRvMU1Eb3dPQ0JCYldWeWFXTmhMMHh2YzE5QmJtZGxiR1Z6SWpzS0NTSnZjbWxuYVc1aGJDMXdkWEpqYUdGelpTMWtZWFJsSWlBOUlDSXlNREV5TFRBM0xUSTJJREV5T2pVd09qQTRJRVYwWXk5SFRWUWlPd3A5IjsKCSJlbnZpcm9ubWVudCIgPSAiU2FuZGJveCI7CgkicG9kIiA9ICIxMDAiOwoJInNpZ25pbmctc3RhdHVzIiA9ICIwIjsKfQ==""}";

        string sql = string.Empty;
        string str = string.Empty;
        string msgCount = string.Empty;
        string url = "https://sandbox.itunes.apple.com/verifyReceipt";
        url = "https://buy.itunes.apple.com/verifyReceipt";
        string result = SendPostData(url, data);
        string orderid = Regex.Match(result, @"""original_transaction_id"":""([^""]*)""").Groups[1].ToString();
        string typ = Regex.Match(result, @"""product_id"":""([^""]*)""").Groups[1].ToString();
        string status = Regex.Match(result, @"""status"":([0-9]*)").Groups[1].ToString();

        if (status == "0")
        {
            if (typ == "com.coco.sms_10")
            {
                msgCount = "20";
            }
            if (typ == "com.coco.sms_25")
            {
                msgCount = "50";
            }
            if (typ == "com.coco.sms_80")
            {
                msgCount = "160";
            }
            if (typ == "com.coco.sms_200")
            {
                msgCount = "400";
            }


            sql = "SELECT COUNT(*) FROM HuliBuyLog WHERE orderid = '" + orderid + "'";
            string count = Common.utils.ExecuteString(sql);
            if (count == "0")
            {
                sql = "INSERT INTO HuliBuyLog (token, adddate, typ, orderid, count) VALUES ('" + token + "',GETDATE(),'" + typ + "','" + orderid + "','" + msgCount + "')";
                Common.utils.ExecuteNonQuery(sql);

                //加短信
                sql = "UPDATE [TeteUserToken] SET total = total + " + msgCount + " WHERE token = '" + token + "' AND nick = 'huli'";
                Common.utils.ExecuteNonQuery(sql);

                str = "{\"result\":\"" + msgCount + "\",\"orderid\":\"" + orderid + "\"}";
                Response.Write(str);
            }
            else
            {
                str = "{\"result\":\"0\"}";
                Response.Write(str);
            }
        }
        else
        {
            str = "{\"result\":\"-1\"}";
            Response.Write(str);
        }
    }



    public static string SendPostData(string url, string data)
    {
        string result = string.Empty;
        #region ---- 完成 HTTP POST 请求----
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        req.Method = "POST";
        req.KeepAlive = true;
        req.Timeout = 300000;
        req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
        byte[] postData = Encoding.UTF8.GetBytes(data);
        Stream reqStream = req.GetRequestStream();
        reqStream.Write(postData, 0, postData.Length);
        reqStream.Close();
        HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
        Encoding encoding = Encoding.UTF8;
        Stream stream = null;
        StreamReader reader = null;
        stream = rsp.GetResponseStream();
        reader = new StreamReader(stream, encoding);
        result = reader.ReadToEnd();
        if (reader != null) reader.Close();
        if (stream != null) stream.Close();
        if (rsp != null) rsp.Close();
        #endregion
        return result;
    }

    private void InitReview()
    {
        string usertoken = Common.utils.NewRequest("usertoken", Common.utils.RequestType.Form);
        string content = Common.utils.NewRequest("content", Common.utils.RequestType.Form);

        Response.Write("提交成功");
        Response.End();
    }

    private void GetToken()
    {
        string usertoken = Common.utils.NewRequest("token", Common.utils.RequestType.QueryString);
        string alerttoken = Common.utils.NewRequest("alerttoken", Common.utils.RequestType.QueryString);
        string typ = Common.utils.NewRequest("typ", Common.utils.RequestType.QueryString);
        string sql = string.Empty;

        if (typ != "")
        {
            typ = "tdr";
        }
        
        
        sql = "INSERT INTO APS_Token (token) VALUES ()";
        

        //File.WriteAllText(Server.MapPath("token.txt"), usertoken + "|" + alerttoken + "|" + Request.Url.ToString());

        Response.Write("http://free.7fshop.com");
        Response.End();
    }

    private void ReturnTicketPost()
    {
        string session = Common.utils.NewRequest("session", Common.utils.RequestType.Form);
        string token = Common.utils.NewRequest("token", Common.utils.RequestType.Form);
        string ticketid = Common.utils.NewRequest("ticketid", Common.utils.RequestType.Form);
        string result = string.Empty;

        string str1 = new Regex(@"JSESSIONID=([^;]*);", RegexOptions.IgnoreCase).Match(session).Groups[1].ToString();
        string str2 = new Regex(@"BIGipServerotsweb=([^;]*);", RegexOptions.IgnoreCase).Match(session).Groups[1].ToString();
        string str = str1 + "|" + str2;

        Train send = new Train();
        result = send.ReturnTicket(str, token, ticketid);


        //File.WriteAllText(Server.MapPath("11112336565.txt"), result);

        Response.Write("ok");
        Response.End();
    }

    private void GetMyOrder()
    {
        string session = Common.utils.NewRequest("session", Common.utils.RequestType.Form);
        string start = Common.utils.NewRequest("start", Common.utils.RequestType.Form);
        string end = Common.utils.NewRequest("end", Common.utils.RequestType.Form);
        string token = Common.utils.NewRequest("token", Common.utils.RequestType.Form);
        string outStr = string.Empty;

        string str1 = new Regex(@"JSESSIONID=([^;]*);", RegexOptions.IgnoreCase).Match(session).Groups[1].ToString();
        string str2 = new Regex(@"BIGipServerotsweb=([^;]*);", RegexOptions.IgnoreCase).Match(session).Groups[1].ToString();
        string str = str1 + "|" + str2;
        string ticketid = string.Empty;

        Train send = new Train();
        string result = send.GetMyOrder(str, token, start, end);

        File.WriteAllText(Server.MapPath("1111233.txt"), result);

        string ticketlist = string.Empty;
        result = Regex.Replace(result, @"[\s]", "");
        MatchCollection matchList = new Regex(@"<tdclass=""blue_bold"">([^<]*)<br/>([^<]*)<br/>([^<]*)<br/>([^<]*)</td><td>([^<]*)<br/>([^<]*)<br/>([^<]*)<br/>([^<]*),([^<]*)</td><td>([^<]*)<br/>([^<]*)<br/><!--[^-]*--></td><td>([^<]*)</td><td><buttontype=""button""onclick=""javascript:refundTicket[(]this,'([^']*)'[)]""", RegexOptions.IgnoreCase).Matches(result);

        //Response.Write(matchList.Count.ToString() + " | ");
        //Response.Write(result);
        for (int i = 0; i < matchList.Count; i++)
        {
            if (i == 0)
            {
                for (int j = 1; j <= 13; j++)
                {
                    if (j == 1)
                    {
                        ticketlist = matchList[i].Groups[j].ToString().Trim();
                    }
                    else
                    {
                        ticketlist += "*" + matchList[i].Groups[j].ToString().Trim();
                    }
                }
            }
            else
            {
                ticketlist += ",";
                for (int j = 1; j <= 13; j++)
                {
                    if (j == 1)
                    {
                        ticketlist += matchList[i].Groups[j].ToString().Trim();
                    }
                    else
                    {
                        ticketlist += "*" + matchList[i].Groups[j].ToString().Trim();
                    }
                }
            }
        }

        outStr += ticketlist;

        File.WriteAllText(Server.MapPath("1111233.txt"), outStr + "-" + result);

        Response.Write(outStr);
        Response.End();
    }

    private void GetIP()
    {
        string ip = Request.UserHostAddress.ToString();

        Response.Write(ip);
        Response.End();
    }

    private void GetWaitingOrder()
    {
        string session = Common.utils.NewRequest("session", Common.utils.RequestType.Form);
        string outStr = string.Empty;
        string token = string.Empty;

        string str1 = new Regex(@"JSESSIONID=([^;]*);", RegexOptions.IgnoreCase).Match(session).Groups[1].ToString();
        string str2 = new Regex(@"BIGipServerotsweb=([^;]*);", RegexOptions.IgnoreCase).Match(session).Groups[1].ToString();
        string str = str1 + "|" + str2;
        string ticketid = string.Empty;

        Train send = new Train();
        string result = send.GetWaitingOrder(str);

        //订单号
        string orderid = new Regex(@"epayOrder[(]'([^']*)'", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
        outStr = orderid + "|";

        //token

        //ticketlist
        string ticketlist = string.Empty;
        MatchCollection matchList = new Regex(@"<td[\s]*class=""blue_bold"">[\s]*<input[\s]*type=""hidden""[\s]*id=""checkbox_pay""[\s]*name=""[^""]*""[\s]*value=""([^""]*)""[\s]*/>[\s]*([^<]*)<br/>[\s]*([^<]*)<br/>[\s]*([^<]*)<br/>[\s]*([^<]*)</td>[\s]*<td>([^<]*)<br/>[\s]*([^<]*)<br/>[\s]*([^<]*)<br/>[\s]*([^,]*),[\s]*([^<]*)</td>[\s]*<td>([^<]*)<br/>[\s]*([^<]*)<br/>[\s]*</td>[\s]*<td>([^<]*)</td>", RegexOptions.IgnoreCase).Matches(result);

        for (int i = 0; i < matchList.Count; i++)
        {
            if (i == 0)
            {
                ticketid = matchList[i].Groups[1].ToString();
                for (int j = 2; j <= 13; j++)
                {
                    if (j == 2)
                    {
                        ticketlist = matchList[i].Groups[j].ToString().Trim();
                    }
                    else
                    {
                        ticketlist += "*" + matchList[i].Groups[j].ToString().Trim();
                    }
                }
            }
            else
            {
                ticketlist += ",";
                for (int j = 2; j <= 13; j++)
                {
                    if (j == 2)
                    {
                        ticketlist += matchList[i].Groups[j].ToString().Trim();
                    }
                    else
                    {
                        ticketlist += "*" + matchList[i].Groups[j].ToString().Trim();
                    }
                }
            }
        }

        result = send.SendPayRequest(str, token, orderid, ticketid);

        token = Regex.Match(result, @"TOKEN""[\s]*value=""([^""]*)""").Groups[1].ToString();
        outStr += token + "|";
        outStr += ticketid + ";|";
        outStr += ticketlist;



        string start = Regex.Match(result, @"var[\s]*beginTime[\s]*=[\s]*""([^""]*)"";").Groups[1].ToString();
        string end = Regex.Match(result, @"var[\s]*loseTime[\s]*=[\s]*""([^""]*)"";").Groups[1].ToString();
        try
        {
            outStr += "|" + ((long.Parse(end) - long.Parse(start)) / 60000).ToString();
        }
        catch { }

        if (result.IndexOf("停止办理业务") != -1)
        {
            outStr += "|该车次在互联网已停止办理业务！";
        }


        File.WriteAllText(Server.MapPath("1111233aaa.txt"), outStr + "-" + result);

        //如果左边是-1则需为排队人数，5为排队
        Response.Write(outStr);
        Response.End();
    }

    private void Log(string txt)
    {
        //string content = File.ReadAllText(Server.MapPath("requestlog.txt"));
        ////File.WriteAllText(Server.MapPath("requestlog.txt"), txt + "---" + DateTime.Now.ToString() + "\r\n" + content);
    }

    private void GetOrderPost()
    {
        string session = Common.utils.NewRequest("session", Common.utils.RequestType.Form);
        string outStr = string.Empty;
        string orderid = string.Empty;

        string str1 = new Regex(@"JSESSIONID=([^;]*);", RegexOptions.IgnoreCase).Match(session).Groups[1].ToString();
        string str2 = new Regex(@"BIGipServerotsweb=([^;]*);", RegexOptions.IgnoreCase).Match(session).Groups[1].ToString();
        string str = str1 + "|" + str2;

        Train send = new Train();
        string result = send.GetOrderNumberRequest(str);
        string waitTime = new Regex(@"""waitTime"":([^,]*),", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();

        outStr = waitTime + "|";
        if (waitTime == "-1")
        {
            orderid = new Regex(@"""orderId"":""([^""]*)""", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
            outStr += orderid;
        }
        //File.WriteAllText(Server.MapPath("11112.txt"), outStr + "-" + result);

        //如果左边是-1则需为排队人数，5为排队
        Response.Write(outStr);
        Response.End();
    }

    /// <summary>
    /// 订单下单成功状态查询
    /// </summary>
    private void StatusSearchPost()
    {

    }

    /// <summary>
    /// 订单取消
    /// </summary>
    private void OrderCancelPost()
    {
        string session = Common.utils.NewRequest("session", Common.utils.RequestType.Form);
        string token = Common.utils.NewRequest("token", Common.utils.RequestType.Form);
        string orderid = Common.utils.NewRequest("orderid", Common.utils.RequestType.Form);
        string ticketid = Common.utils.NewRequest("ticketid", Common.utils.RequestType.Form);

        string str1 = new Regex(@"JSESSIONID=([^;]*);", RegexOptions.IgnoreCase).Match(session).Groups[1].ToString();
        string str2 = new Regex(@"BIGipServerotsweb=([^;]*);", RegexOptions.IgnoreCase).Match(session).Groups[1].ToString();
        string str = str1 + "|" + str2;

        Train send = new Train();
        string result = send.SendCancelRequest(str, token, orderid, ticketid);

        Log(str);
        File.WriteAllText(Server.MapPath("test2222.txt"), result);

        if (result.IndexOf("取消订单成功") != -1)
        {
            Response.Write("ok");
        }

        Response.End();
    }

    /// <summary>
    /// 订单支付银行清单
    /// </summary>
    private void OrderPayPost()
    {
        string session = Common.utils.NewRequest("session", Common.utils.RequestType.Form);
        string token = Common.utils.NewRequest("token", Common.utils.RequestType.Form);
        string orderid = Common.utils.NewRequest("orderid", Common.utils.RequestType.Form);
        string ticketid = Common.utils.NewRequest("ticketid", Common.utils.RequestType.Form);

        string str1 = new Regex(@"JSESSIONID=([^;]*);", RegexOptions.IgnoreCase).Match(session).Groups[1].ToString();
        string str2 = new Regex(@"BIGipServerotsweb=([^;]*);", RegexOptions.IgnoreCase).Match(session).Groups[1].ToString();
        string str = str1 + "|" + str2;

        string result1 = string.Empty;

        Train send = new Train();
        string result = send.SendPayRequest(str, token, orderid, ticketid);

 

        //File.WriteAllText(Server.MapPath("test111221.txt"), result);

        if (result.IndexOf("该车次在互联网已停止办理业务") == -1)
        {
            //Log(str);
            //第一次支付界面
            string data = new Regex(@"<input[\s]*type=""hidden""[\s]*name=""tranData""[\s]*value=""([^""]*)"">", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
            string msg = new Regex(@"<input[\s]*type=""hidden""[\s]*name=""merSignMsg""[\s]*value=""([^""]*)"">", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
            result = send.SendPayRequestEpay(str, data, msg);
            //result = result.Replace("</body>", "<script type=\"text/javascript\">formsubmit('00011000');</script></body>");
            //result = result.Replace(",", "").Replace("|", "");
            //File.WriteAllText(Server.MapPath("test1112222.txt"), result);

            //第二次支付界面
            data = new Regex(@"<input[\s]*type=""hidden""[\s]*value=""([^""]*)""[\s]*name=""tranData"" />", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
            msg = new Regex(@"<input[\s]*type=""hidden""[\s]*value=""([^""]*)""[\s]*name=""merSignMsg"" />", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
            orderid = new Regex(@"<input[\s]*type=""hidden""[\s]*value=""([^""]*)""[\s]*name=""orderTimeoutDate"" />", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
            //result = send.SendPayRequestEpayStep(data, msg, orderid, str, "00011000");
            //result1 = send.SendPayRequestEpayStep(data, msg, orderid, str, "03080000");

            //招商银行支付     
            IDictionary<string, string> param = new Dictionary<string, string>();
            param.Add("tranData", data);
            param.Add("transType", "01");
            param.Add("channelId", "1");
            param.Add("appId", "0001");
            param.Add("merSignMsg", msg);
            param.Add("merCustomIp", "{ip}");
            param.Add("orderTimeoutDate", orderid);
            param.Add("bankId", "03080000");


            //网银直接支付     
            IDictionary<string, string> param1 = new Dictionary<string, string>();
            param1.Add("tranData", data);
            param1.Add("transType", "01");
            param1.Add("channelId", "1");
            param1.Add("appId", "0001");
            param1.Add("merSignMsg", msg);
            param1.Add("merCustomIp", "{ip}");
            param1.Add("orderTimeoutDate", orderid);
            param1.Add("bankId", "00011000"); 
            //result = result.Replace("value=\"01\"", "value=\"05\"");   
            //result = result.Replace(",", "").Replace("|", "");
            //result1 = result1.Replace(",", "").Replace("|", "");

            //第三次支付界面
            string time = new Regex(@"<input[\s]*type=""hidden""[\s]*name=""orderTime""[\s]*value=""([^""]*)"">", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
            string backUrl = new Regex(@"<input[\s]*type=""hidden""[\s]*name=""backEndUrl""[\s]*value=""([^""]*)"">", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
            string merId = new Regex(@"<input[\s]*type=""hidden""[\s]*name=""merId""[\s]*value=""([^""]*)"">", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
            string frontUrl = new Regex(@"<input[\s]*type=""hidden""[\s]*name=""frontEndUrl""[\s]*value=""([^""]*)"">", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
            string signature = new Regex(@"<input[\s]*type=""hidden""[\s]*name=""signature""[\s]*value=""([^""]*)"">", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
            string amount = new Regex(@"<input[\s]*type=""hidden""[\s]*name=""orderAmount""[\s]*value=""([^""]*)"">", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
            string mer = new Regex(@"<input[\s]*type=""hidden""[\s]*name=""merReserved""[\s]*value=""([^""]*)"">", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
            string payorderid = new Regex(@"<input[\s]*type=""hidden""[\s]*name=""orderNumber""[\s]*value=""([^""]*)"">", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
            //result = send.SendPayRequestEpayStepNew(time, backUrl, merId, frontUrl, signature, amount, mer, payorderid, str);
            ////File.WriteAllText(Server.MapPath("test1112222344.txt"), result);
            //result = new Regex(@"<form[\s\S]*?</form>", RegexOptions.IgnoreCase).Match(result).Groups[0].ToString();

            string resStr = string.Empty;

            resStr = @"招商银行*招商银行支付简介*https://epay.12306.cn/pay/webBusiness*" + utils.PostData(param) + "*";
            resStr += "|网银支付（银联）*网银支付（银联）*https://epay.12306.cn/pay/webBusiness*" + utils.PostData(param1) + "*********setTimeout('direct()',10); function direct(){if(document.getElementById('CSPayTab')){window.location.href=document.getElementById('CSPayTab').getAttribute('href');}}";
            resStr += "|建行支付（银联）*建行支付（银联）*https://epay.12306.cn/pay/webBusiness*" + utils.PostData(param1) + "*********setTimeout('direct()',10); function direct(){if(document.getElementById('CSPayTab')){window.location.href=document.getElementById('CSPayTab').getAttribute('href');}}*****setTimeout('direct()',10); function direct(){document.getElementsByName('csBank')[3].checked = true;document.getElementById('csForm').submit();}";

            //File.WriteAllText(Server.MapPath("test11122223.txt"), result);
            Response.Write(resStr);
            Response.End();
        }
        else
        {
            Response.Write(@"该车次在互联网已停止办理业务,该车次在互联网已停止办理业务,该车次在互联网已停止办理业务");
            Response.End();
        }
    }

    /// <summary>
    /// 获取表单html
    /// </summary>
    /// <returns></returns>
    private string getFormStr(string html, string bankid)
    {
        string str = string.Empty;

        str = Regex.Match(html, @"<form[^>]*>([\s\S]*?)</form>").Groups[0].ToString();
        str += @"<SCRIPT type=text/javascript>
		document.getElementsByName(""bankId"")[0].value='" + bankid + @"';
		document.getElementsByName(""myform"")[0].submit();}</SCRIPT>";

        return str;
    }

    //添加联系人
    private void PersonActPost(string act)
    {                            
        string session = Common.utils.NewRequest("session", Common.utils.RequestType.Form);
        
        string name = Common.utils.NewRequest("name", Common.utils.RequestType.Form);
        string sex = Common.utils.NewRequest("sex", Common.utils.RequestType.Form);  
        string card_type = Common.utils.NewRequest("card_type", Common.utils.RequestType.Form);
        string card_no = Common.utils.NewRequest("card_no", Common.utils.RequestType.Form);
        string passenger_type = Common.utils.NewRequest("passenger_type", Common.utils.RequestType.Form);
        string mobile = Common.utils.NewRequest("mobile", Common.utils.RequestType.Form);
                  
        string str1 = new Regex(@"JSESSIONID=([^;]*);", RegexOptions.IgnoreCase).Match(session).Groups[1].ToString();
        string str2 = new Regex(@"BIGipServerotsweb=([^;]*);", RegexOptions.IgnoreCase).Match(session).Groups[1].ToString();
        string str = str1 + "|" + str2;

        //根据动作操作会员联系人   
        if (act == "add")
        { 
            Train send = new Train();
            string result = send.SendAddRequest(str, name, sex, card_type, card_no, passenger_type, mobile);

            Response.Write("ok");
            //File.WriteAllText(Server.MapPath("token0.txt"), result);
            Response.End();
            return;
        }

        //根据动作操作会员联系人   
        if (act == "edit")
        {
            Train send = new Train();
            string result = send.SendEditRequest(str, name, sex, card_type, card_no, passenger_type, mobile);

            //File.WriteAllText(Server.MapPath("token1.txt"), result);
            Response.Write("ok");
            Response.End();
            return;
        }

        //根据动作操作会员联系人   
        if (act == "del")
        {
            ////File.WriteAllText(Server.MapPath("token.txt"), "");
            Train send = new Train();
            string result = send.SendDelRequest(str, name, sex, card_type, card_no, passenger_type, mobile);
            ////File.WriteAllText(Server.MapPath("token1.txt"), "");

            Response.Write("ok");
            //File.WriteAllText(Server.MapPath("token2.txt"), result);
            Response.End();
            return;
        }

        //根据动作操作会员联系人   
        if (act == "search")
        {
            Train send = new Train();
            string result = send.SendSearchRequest(str);

            result = result.Replace("null", "\"null\"");

            Response.Write(result);
            Response.End();
            return;
        }
    }

    private void TicketOrderPost()
    {
        string session = Common.utils.NewRequest("session", Common.utils.RequestType.Form);
        //车次关键字
        string key = Common.utils.NewRequest("key", Common.utils.RequestType.Form);
        //上页查询信息
        string date = HttpUtility.UrlEncode(Common.utils.NewRequest("date", Common.utils.RequestType.Form));
        string startcity = HttpUtility.UrlEncode(Common.utils.NewRequest("startcity", Common.utils.RequestType.Form));
        string endcity = HttpUtility.UrlEncode(Common.utils.NewRequest("endcity", Common.utils.RequestType.Form));
        string no = HttpUtility.UrlEncode(Common.utils.NewRequest("no", Common.utils.RequestType.Form));
        string rtyp = HttpUtility.UrlEncode(Common.utils.NewRequest("rtyp", Common.utils.RequestType.Form));
        string ttype = HttpUtility.UrlEncode(Common.utils.NewRequest("ttype", Common.utils.RequestType.Form));
        string student = HttpUtility.UrlEncode(Common.utils.NewRequest("student", Common.utils.RequestType.Form));
        string timearea = HttpUtility.UrlEncode(Common.utils.NewRequest("timearea", Common.utils.RequestType.Form));

        string outStr = string.Empty;

        string str1 = new Regex(@"JSESSIONID=([^;]*);", RegexOptions.IgnoreCase).Match(session).Groups[1].ToString();
        string str2 = new Regex(@"BIGipServerotsweb=([^;]*);", RegexOptions.IgnoreCase).Match(session).Groups[1].ToString();
        string str = str1 + "|" + str2;

        Train send = new Train();
        string result = send.SendOrderRequest(str, key, date, startcity, endcity, no, rtyp, ttype, student, timearea);

        string userList = string.Empty;
        string ticketList = string.Empty;
        string priceList = string.Empty;
        string token = string.Empty;
        string ticketStr = string.Empty;
        string train_no = string.Empty;

        string[] ary = key.Split('#');

        //<input type="hidden" name="org.apache.struts.taglib.html.TOKEN" value="ee3f5476cc070983f99484be47ddd53d">
        //<input type="hidden" name="leftTicketStr" id="left_ticket" value="O013500000M0230000009043000012" />

        if (result.IndexOf("目前您还有未处理的订单") != -1)
        {
            Response.Write("目前您还有未处理的订单，请您到【未完成订单】进行处理");
            Response.End();
            return;
        }

        try
        {
            //返回车票价格阶梯
            userList = Regex.Match(result, @"passengerJson[\s]*=[\s]*([^;]*);").Groups[1].ToString();
            userList = "{\"data\":" + userList + "}";

            //返回联系人列表
            ticketList = Regex.Match(result, @"limitBuySeatTicketDTO[\s]*=[\s]*([^;]*);").Groups[1].ToString();

            //车票价格和剩余数量
            Match match = Regex.Match(result, @"class=""bluetext"">[^<]*</td>[\s]*</tr>[\s]*<tr>[\s]*<td>([^<]*)</td>[\s]*<td>([^<]*)</td>[\s]*<td>([^<]*)</td>");
            priceList = match.Groups[1].ToString() + "," + match.Groups[2].ToString() + "," + match.Groups[3].ToString();

            //token
            token = Regex.Match(result, @"TOKEN""[\s]*value=""([^""]*)""").Groups[1].ToString();

            //ticketstr
            ticketStr = Regex.Match(result, @"left_ticket""[\s]*value=""([^""]*)""").Groups[1].ToString();

            train_no = Regex.Match(result, @"train_no""[\s]*value=""([^""]*)""").Groups[1].ToString();
        }
        catch { }

        //返回验证码
        outStr = ary[0] + "(" + ary[7] + "-" + ary[8] + ")|" + date + " " + ary[2] + "-" + ary[6] + "(" + ary[1] + ")|" + ticketList + "|" + userList + "|" + priceList + "|" + token + "|" + ticketStr + "|" + train_no;

        File.WriteAllText(Server.MapPath("1112.txt"), outStr + "-" + result);

        Log(outStr);

        Response.Write(outStr);
        Response.End();
    }

    /// <summary>
    /// 查询火车详情
    /// </summary>
    private void SearchDetailPost()
    {
        string session = Common.utils.NewRequest("session", Common.utils.RequestType.Form);
        string date = Common.utils.NewRequest("date", Common.utils.RequestType.Form);
        string startcity = Common.utils.NewRequest("startcity", Common.utils.RequestType.Form);
        string endcity = Common.utils.NewRequest("endcity", Common.utils.RequestType.Form);
        string no = Common.utils.NewRequest("no", Common.utils.RequestType.Form);

        string str1 = new Regex(@"JSESSIONID=([^;]*);", RegexOptions.IgnoreCase).Match(session).Groups[1].ToString();
        string str2 = new Regex(@"BIGipServerotsweb=([^;]*);", RegexOptions.IgnoreCase).Match(session).Groups[1].ToString();
        string str = str1 + "|" + str2;

        Train send = new Train();
        string result = send.SendSearchDetailRequest(date, startcity, endcity, no, str1 + "|" + str2);

        Log(session);
        Response.Write(result);
        Response.End();
    }

    /// <summary>
    /// 订票操作
    /// </summary>
    private void SubmitOrderPost()
    {
        string session = Common.utils.NewRequest("session", Common.utils.RequestType.Form);
        string orderid = Common.utils.NewRequest("orderid", Common.utils.RequestType.Form);
        string date = Common.utils.NewRequest("date", Common.utils.RequestType.Form);
        string randCode = Common.utils.NewRequest("randCode", Common.utils.RequestType.Form);
        string token = Common.utils.NewRequest("token", Common.utils.RequestType.Form);
        string ticket = Common.utils.NewRequest("ticket", Common.utils.RequestType.Form);
        string train_no = Common.utils.NewRequest("train_no", Common.utils.RequestType.Form);
        //车次关键字
        string key = Common.utils.NewRequest("key", Common.utils.RequestType.Form);

        string str1 = new Regex(@"JSESSIONID=([^;]*);", RegexOptions.IgnoreCase).Match(session).Groups[1].ToString();
        string str2 = new Regex(@"BIGipServerotsweb=([^;]*);", RegexOptions.IgnoreCase).Match(session).Groups[1].ToString();
        string str = str1 + "|" + str2;

        List<User> userList = InitUserStr();
        string paramStr = string.Empty;

        Train send = new Train();
        string result = send.SendOrderSubmitRequest(str, randCode, orderid, userList, key, date, token, ticket, ref paramStr, train_no);

        //File.WriteAllText(Server.MapPath("8888882.txt"), paramStr + "-" + result);

        Log(session);
        Response.Write(result);     
        Response.End();
    }                                 

    /// <summary>
    /// 订票操作
    /// </summary>
    private void PreSubmitOrderPost()
    {
        string session = Common.utils.NewRequest("session", Common.utils.RequestType.Form);
        string date = Common.utils.NewRequest("date", Common.utils.RequestType.Form);
        string randCode = Common.utils.NewRequest("randCode", Common.utils.RequestType.Form);
        string token = Common.utils.NewRequest("token", Common.utils.RequestType.Form);
        string ticket = Common.utils.NewRequest("ticket", Common.utils.RequestType.Form);
        string train_no = Common.utils.NewRequest("train_no", Common.utils.RequestType.Form);
        //车次关键字
        string key = Common.utils.NewRequest("key", Common.utils.RequestType.Form);

        string str1 = new Regex(@"JSESSIONID=([^;]*);", RegexOptions.IgnoreCase).Match(session).Groups[1].ToString();
        string str2 = new Regex(@"BIGipServerotsweb=([^;]*);", RegexOptions.IgnoreCase).Match(session).Groups[1].ToString();
        string str = str1 + "|" + str2;

        List<User> userList = InitUserStr();
        string paramStr = string.Empty;

        Train send = new Train();
        string result = send.PreSendOrderSubmitRequest(str, randCode, userList, key, date, token, ticket, ref paramStr, train_no);

        //File.WriteAllText(Server.MapPath("8888881.txt"), paramStr + "-" + result + "-" + session);
        Log(paramStr + "-" + result + "-" + session);

        Response.Write(result);
        Response.End();
    }

    private List<User> InitUserStr()
    {
        List<User> userList = new List<User>();
        string list = Common.utils.NewRequest("passengerTickets", Common.utils.RequestType.Form);
        string list1 = Common.utils.NewRequest("oldPassengers", Common.utils.RequestType.Form);

        string[] ary = list.Split(',');
        string[] ary1 = list1.Split(',');
        for (int i = 0; i < ary.Length; i++)
        {
            User u = new User();

            u.Str1 = ary[i].Replace("|", ",");
            u.Str2 = ary1[i].Replace("|", ",");
            u.Str3 = Common.utils.NewRequest("passenger_" + (i + 1).ToString() + "_seat", Common.utils.RequestType.Form);
            u.Str4 = Common.utils.NewRequest("passenger_" + (i + 1).ToString() + "_seat_detail", Common.utils.RequestType.Form);
            u.Str5 = Common.utils.NewRequest("passenger_" + (i + 1).ToString() + "_ticket", Common.utils.RequestType.Form);
            u.Str6 = Common.utils.NewRequest("passenger_" + (i + 1).ToString() + "_name", Common.utils.RequestType.Form);
            u.Str7 = Common.utils.NewRequest("passenger_" + (i + 1).ToString() + "_cardtype", Common.utils.RequestType.Form);
            u.Str8 = Common.utils.NewRequest("passenger_" + (i + 1).ToString() + "_cardno", Common.utils.RequestType.Form);
            u.Str9 = Common.utils.NewRequest("passenger_" + (i + 1).ToString() + "_mobileno", Common.utils.RequestType.Form);

            userList.Add(u);
        }

        return userList;
    }

    /// <summary>
    /// 车次搜索
    /// </summary>
    private void SearchPost()
    {
        string session = Common.utils.NewRequest("session", Common.utils.RequestType.Form);
        string date = HttpUtility.UrlEncode(Common.utils.NewRequest("date", Common.utils.RequestType.Form));
        string startcity = HttpUtility.UrlEncode(Common.utils.NewRequest("startcity", Common.utils.RequestType.Form));
        string endcity = HttpUtility.UrlEncode(Common.utils.NewRequest("endcity", Common.utils.RequestType.Form));
        string no = HttpUtility.UrlEncode(Common.utils.NewRequest("no", Common.utils.RequestType.Form));
        string rtyp = HttpUtility.UrlEncode(Common.utils.NewRequest("rtyp", Common.utils.RequestType.Form));
        string ttype = HttpUtility.UrlEncode(Common.utils.NewRequest("ttype", Common.utils.RequestType.Form));
        string student = HttpUtility.UrlEncode(Common.utils.NewRequest("student", Common.utils.RequestType.Form));
        string timearea = HttpUtility.UrlEncode(Common.utils.NewRequest("timearea", Common.utils.RequestType.Form));

        string outStr = string.Empty;

        string str1 = new Regex(@"JSESSIONID=([^;]*);", RegexOptions.IgnoreCase).Match(session).Groups[1].ToString();
        string str2 = new Regex(@"BIGipServerotsweb=([^;]*);", RegexOptions.IgnoreCase).Match(session).Groups[1].ToString();
        string str = str1 + "|" + str2;

        Train send = new Train();
        string result = send.SendSearchRequest(date, startcity, endcity, no, rtyp, ttype, student, timearea, str1 + "|" + str2);

        Regex reg = new Regex(@">([^\<]*)</span>,[\s\S]*?&nbsp;([^\&\;']+)&nbsp;[\s\S]*?([0-9]{2}\:[0-9]{2}),[\s\S]*?&nbsp;([^\&\;']+)&nbsp;[\s\S]*?([0-9]{2}\:[0-9]{2}),([^,]*),([^,]*),([^,]*),([^,]*),([^,]*),([^,]*),([^,]*),([^,]*),([^,]*),([^,]*),([^,]*),[\s\S]*?value='预订'", RegexOptions.IgnoreCase);
        MatchCollection match = reg.Matches(result);

        Regex regBottom = new Regex(@"onclick\=javascript\:getSelected\('([^']*)'\)", RegexOptions.IgnoreCase);
        MatchCollection matchBottom = regBottom.Matches(result);
        int index = 0;
        for (int i = 0; i < match.Count; i++)
        {
            if (i != 0)
                outStr += "|";

            for (int j = 1; j < 17; j++)
            {
                outStr += match[i].Groups[j].ToString().Replace("<font color='#008800'>", "").Replace("</font>", "").Replace("<font color='darkgray'>", "") + ",";
            }

            //如果有票才加
            if (match[i].Groups[0].ToString().IndexOf("getSelected") != -1)
            {
                outStr += matchBottom[index].Groups[1].ToString();
                index++;
            }
        }

        File.WriteAllText(Server.MapPath("111.txt"), matchBottom.Count + "-" + outStr + "-" + result);

        Log(session);
        Response.Write(outStr);
        Response.End();
    }

    /// <summary>
    /// 登录请求
    /// </summary>
    private void LoginPost()
    {
        string uid = Common.utils.NewRequest("uid", Common.utils.RequestType.Form);
        string pass = Common.utils.NewRequest("pass", Common.utils.RequestType.Form);
        string verify = Common.utils.NewRequest("verify", Common.utils.RequestType.Form);
        string session = Common.utils.NewRequest("session", Common.utils.RequestType.Form);

        //JSESSIONID=2D196E71DE9C9DDF0BE726984B66C03C;
        string str1 = new Regex(@"JSESSIONID=([^;]*);", RegexOptions.IgnoreCase).Match(session).Groups[1].ToString();
        string str2 = new Regex(@"BIGipServerotsweb=([^;]*);", RegexOptions.IgnoreCase).Match(session).Groups[1].ToString();

        Train send = new Train();
        string result = send.SendLoginRequest(uid, pass, verify, str1 + "|" + str2);

        if (result.IndexOf("访问用户过多") != -1)
        {
            Response.Write("busy");
        }
        else if (result.IndexOf("请输入正确的验证码") != -1)
        {
            Response.Write("请输入正确的验证码");
        }
        else if (result.IndexOf("邮箱不存在") != -1)
        {
            Response.Write("邮箱不存在");
        }
        else if (result.IndexOf("密码输入错误") != -1)
        {
            Response.Write("密码输入错误");
        }
        else if (result.IndexOf(@"class=""text_14"" id=""activeEamil""") != -1)
        {
            Response.Write("请您先到12306官方网站上激活帐号再登录");
        }
        else
        {
            if (result.IndexOf("我的12306") == -1)
            {
                Response.Write("密码输入错误超过4次或者验证码失效，请重试！");
            }
            else
            {
                Response.Write("ok");
            }
        }
        Log(session);
        //File.WriteAllText(Server.MapPath("111000.txt"), result);
        Response.End();
    }

    /// <summary>
    /// 输出验证码
    /// </summary>
    private void OutPutVerify()
    {
        try
        {
            Train t = new Train();
            string cookieStr = t.GetVerifyImg();

            Common.Cookie cookie = new Common.Cookie();

            string[] ary = cookieStr.Split('|');

            cookie.setCookie("JSESSIONID", ary[0], 999999);

            if (ary.Length > 1)
            {
                cookie.setCookie("BIGipServerotsweb", ary[1], 999999);
            }
            Log(cookieStr);
            Response.End();
        }
        catch
        {
            Response.Write("err");
            Response.End();
        }
    }


    /// <summary>
    /// 输出验证码
    /// </summary>
    private void OutPutVerifyOrder()
    {
        try
        {
            string session = Common.utils.NewRequest("session", Common.utils.RequestType.Form);

            string str1 = new Regex(@"JSESSIONID=([^;]*);", RegexOptions.IgnoreCase).Match(session).Groups[1].ToString();
            string str2 = new Regex(@"BIGipServerotsweb=([^;]*);", RegexOptions.IgnoreCase).Match(session).Groups[1].ToString();

            Train t = new Train();
            string cookieStr = t.GetVerifyImgOrder(str1 + "|" + str2);
            Log(session);
            Response.End();
        }
        catch
        {
            Response.Write("err");
            Response.End();
        }
    }
}