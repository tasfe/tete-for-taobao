﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeteIosTrain;
using System.IO;
using System.Text.RegularExpressions;

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

        Response.Write(result);
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

        string ticketlist = string.Empty;
        MatchCollection matchList = new Regex(@"<td[\s]*class=""blue_bold"">[\s]*([^<]*)<br/>[\s]*([^<]*)<br/>[\s]*([^<]*)<br/>[\s]*([^<]*)</td>[\s]*<td>([^<]*)<br/>[\s]*([^<]*)<br/>[\s]*([^<]*)<br/>[\s]*([^,]*),[\s]*([^<]*)</td>[\s]*<td>([^<]*)<br/>[\s]*([^<]*)<br/>[\s]*</td>[\s]*<td>([^<]*)</td>", RegexOptions.IgnoreCase).Matches(result);

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
        token = Regex.Match(result, @"TOKEN""[\s]*value=""([^""]*)""").Groups[1].ToString();
        outStr += token + "|";

        //ticketlist
        string ticketlist = string.Empty;
        MatchCollection matchList = new Regex(@"<td[\s]*class=""blue_bold"">[\s]*<input[\s]*type=""hidden""[\s]*id=""checkbox_pay""[\s]*name=""[^""]*""[\s]*value=""([^""]*)""[\s]*/>[\s]*([^<]*)<br/>[\s]*([^<]*)<br/>[\s]*([^<]*)<br/>[\s]*([^<]*)</td>[\s]*<td>([^<]*)<br/>[\s]*([^<]*)<br/>[\s]*([^<]*)<br/>[\s]*([^,]*),[\s]*([^<]*)</td>[\s]*<td>([^<]*)<br/>[\s]*([^<]*)<br/>[\s]*</td>[\s]*<td>([^<]*)</td>", RegexOptions.IgnoreCase).Matches(result);

        for (int i = 0; i < matchList.Count; i++)
        {
            if (i == 0)
            {
                ticketid = matchList[i].Groups[1].ToString();
                outStr += matchList[i].Groups[1].ToString() + ";|";
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

        outStr += ticketlist;

        result = send.SendPayRequest(str, token, orderid, ticketid);
        string start = Regex.Match(result, @"var[\s]*beginTime[\s]*=[\s]*""([^""]*)"";").Groups[1].ToString();
        string end = Regex.Match(result, @"var[\s]*loseTime[\s]*=[\s]*""([^""]*)"";").Groups[1].ToString();
        try
        {
            outStr += "|" + ((long.Parse(end) - long.Parse(start)) / 60000).ToString();
        }
        catch { }

        //如果左边是-1则需为排队人数，5为排队
        Response.Write(outStr);
        Response.End();
    }

    private void Log(string txt)
    {
        string content = File.ReadAllText(Server.MapPath("requestlog.txt"));
        File.WriteAllText(Server.MapPath("requestlog.txt"), txt + "---" + DateTime.Now.ToString() + "\r\n" + content);
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
        File.WriteAllText(Server.MapPath("11112.txt"), outStr + "-" + result);

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

 

        File.WriteAllText(Server.MapPath("test111221.txt"), result);

        if (result.IndexOf("该车次在互联网已停止办理业务") == -1)
        {
            //Log(str);
            //第一次支付界面
            string data = new Regex(@"<input[\s]*type=""hidden""[\s]*name=""tranData""[\s]*value=""([^""]*)"">", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
            string msg = new Regex(@"<input[\s]*type=""hidden""[\s]*name=""merSignMsg""[\s]*value=""([^""]*)"">", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
            result = send.SendPayRequestEpay(str, data, msg);
            //result = result.Replace("</body>", "<script type=\"text/javascript\">formsubmit('00011000');</script></body>");
            //result = result.Replace(",", "").Replace("|", "");
            File.WriteAllText(Server.MapPath("test1112222.txt"), result);

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
            //File.WriteAllText(Server.MapPath("test1112222344.txt"), result);
            //result = new Regex(@"<form[\s\S]*?</form>", RegexOptions.IgnoreCase).Match(result).Groups[0].ToString();

            string resStr = string.Empty;

            resStr = @"招商银行,招商银行支付简介,https://epay.12306.cn/pay/webBusiness," + utils.PostData(param) + ",";
            resStr += "|网银支付（银联）,网银支付（银联）,https://epay.12306.cn/pay/webBusiness," + utils.PostData(param1) + ",,window.location.href=document.getElementById('CSPayTab').href;";

            File.WriteAllText(Server.MapPath("test11122223.txt"), result);
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
            File.WriteAllText(Server.MapPath("token0.txt"), result);
            Response.End();
            return;
        }

        //根据动作操作会员联系人   
        if (act == "edit")
        {
            Train send = new Train();
            string result = send.SendEditRequest(str, name, sex, card_type, card_no, passenger_type, mobile);

            File.WriteAllText(Server.MapPath("token1.txt"), result);
            Response.Write("ok");
            Response.End();
            return;
        }

        //根据动作操作会员联系人   
        if (act == "del")
        {
            //File.WriteAllText(Server.MapPath("token.txt"), "");
            Train send = new Train();
            string result = send.SendDelRequest(str, name, sex, card_type, card_no, passenger_type, mobile);
            //File.WriteAllText(Server.MapPath("token1.txt"), "");

            Response.Write("ok");
            File.WriteAllText(Server.MapPath("token2.txt"), result);
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

        File.WriteAllText(Server.MapPath("8888882.txt"), paramStr + "-" + result);

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

        File.WriteAllText(Server.MapPath("8888881.txt"), paramStr + "-" + result + "-" + session);
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
        File.WriteAllText(Server.MapPath("111000.txt"), result);
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