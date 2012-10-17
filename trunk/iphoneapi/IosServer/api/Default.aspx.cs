using System;
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
    
    }

    /// <summary>
    /// 订单支付银行清单
    /// </summary>
    private void OrderPayPost()
    {
    
    }

    //添加联系人
    private void PersonActPost(string act)
    {
        //根据动作操作会员联系人
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

        ////string result = @"var passengerJson = [{""first_letter"":""LEONFEIFEI0"",""isUserSelf"":"""",""mobile_no"":""18651613908"",""old_passenger_id_no"":"""",""old_passenger_id_type_code"":"""",""old_passenger_name"":"""",""passenger_flag"":""0"",""passenger_id_no"":""320105198207291617"",""passenger_id_type_code"":""1"",""passenger_id_type_name"":"""",""passenger_name"":""王里"",""passenger_type"":""1"",""passenger_type_name"":"""",""recordCount"":""3""},{""first_letter"":""WANGLI"",""isUserSelf"":"""",""mobile_no"":""18651613908"",""old_passenger_id_no"":"""",""old_passenger_id_type_code"":"""",""old_passenger_name"":"""",""passenger_flag"":""0"",""passenger_id_no"":""320105198207291617"",""passenger_id_type_code"":""1"",""passenger_id_type_name"":"""",""passenger_name"":""wangli"",""passenger_type"":""1"",""passenger_type_name"":"""",""recordCount"":""3""},{""first_letter"":""MYJ"",""isUserSelf"":"""",""mobile_no"":"""",""old_passenger_id_no"":"""",""old_passenger_id_type_code"":"""",""old_passenger_name"":"""",""passenger_flag"":""0"",""passenger_id_no"":""320103197801030527"",""passenger_id_type_code"":""1"",""passenger_id_type_name"":"""",""passenger_name"":""马永洁"",""passenger_type"":""1"",""passenger_type_name"":"""",""recordCount"":""3""}];
        //        var ticketTypeReserveFlags = [{""epayFlag"":true,""ticket_type_code"":""1""},{""epayFlag"":true,""ticket_type_code"":""2""},{""epayFlag"":false,""ticket_type_code"":""3""},{""epayFlag"":false,""ticket_type_code"":""4""}];
        //        var limitBuySeatTicketDTO  = {""seat_type_codes"":[{""end_station_name"":"""",""end_time"":"""",""id"":""M"",""start_station_name"":"""",""start_time"":"""",""value"":""一等座""},{""end_station_name"":"""",""end_time"":"""",""id"":""O"",""start_station_name"":"""",""start_time"":"""",""value"":""二等座""},{""end_station_name"":"""",""end_time"":"""",""id"":""9"",""start_station_name"":"""",""start_time"":"""",""value"":""商务座""}],""ticket_seat_codeMap"":{""3"":[{""end_station_name"":"""",""end_time"":"""",""id"":""O"",""start_station_name"":"""",""start_time"":"""",""value"":""二等座""}],""2"":[{""end_station_name"":"""",""end_time"":"""",""id"":""9"",""start_station_name"":"""",""start_time"":"""",""value"":""商务座""},{""end_station_name"":"""",""end_time"":"""",""id"":""M"",""start_station_name"":"""",""start_time"":"""",""value"":""一等座""},{""end_station_name"":"""",""end_time"":"""",""id"":""O"",""start_station_name"":"""",""start_time"":"""",""value"":""二等座""}],""1"":[{""end_station_name"":"""",""end_time"":"""",""id"":""9"",""start_station_name"":"""",""start_time"":"""",""value"":""商务座""},{""end_station_name"":"""",""end_time"":"""",""id"":""M"",""start_station_name"":"""",""start_time"":"""",""value"":""一等座""},{""end_station_name"":"""",""end_time"":"""",""id"":""O"",""start_station_name"":"""",""start_time"":"""",""value"":""二等座""}],""4"":[{""end_station_name"":"""",""end_time"":"""",""id"":""9"",""start_station_name"":"""",""start_time"":"""",""value"":""商务座""},{""end_station_name"":"""",""end_time"":"""",""id"":""M"",""start_station_name"":"""",""start_time"":"""",""value"":""一等座""},{""end_station_name"":"""",""end_time"":"""",""id"":""O"",""start_station_name"":"""",""start_time"":"""",""value"":""二等座""}]},""ticket_type_codes"":[{""end_station_name"":"""",""end_time"":"""",""id"":""1"",""start_station_name"":"""",""start_time"":"""",""value"":""成人票""},{""end_station_name"":"""",""end_time"":"""",""id"":""2"",""start_station_name"":"""",""start_time"":"""",""value"":""儿童票""},{""end_station_name"":"""",""end_time"":"""",""id"":""3"",""start_station_name"":"""",""start_time"":"""",""value"":""学生票""},{""end_station_name"":"""",""end_time"":"""",""id"":""4"",""start_station_name"":"""",""start_time"":"""",""value"":""残军票""}]};";

        string userList = string.Empty;
        string ticketList = string.Empty;
        string priceList = string.Empty;

        try
        {
            //返回车票价格阶梯
            userList = Regex.Match(result, @"passengerJson[\s]*=[\s]*([^;]*);").Groups[1].ToString();
            userList = "{\"data\":" + userList + "}";

            //返回联系人列表
            ticketList = Regex.Match(result, @"limitBuySeatTicketDTO[\s]*=[\s]*([^;]*);").Groups[1].ToString();

            //车票价格和剩余数量
            priceList = "一等座(230.00元)7张票,二等座(135.00元)3张票,特等座(260.00元)10张票";
        }
        catch { }

        //返回验证码
        outStr = "1234(南京-上海)|2012-10-15 16:31-20:58(04:27)|" + ticketList + "|" + userList + "|" + priceList;

        File.WriteAllText(Server.MapPath("1112.txt"), outStr + "-" + result);

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

        Response.Write(result);
        Response.End();
    }

    /// <summary>
    /// 订票操作
    /// </summary>
    private void SubmitOrderPost()
    {
        string session = Common.utils.NewRequest("session", Common.utils.RequestType.Form);
        string date = Common.utils.NewRequest("date", Common.utils.RequestType.Form);
        string startcity = Common.utils.NewRequest("startcity", Common.utils.RequestType.Form);
        string endcity = Common.utils.NewRequest("endcity", Common.utils.RequestType.Form);
        string no = Common.utils.NewRequest("no", Common.utils.RequestType.Form);
        string data = Common.utils.NewRequest("data", Common.utils.RequestType.Form);
                      
        //Train send = new Train();
        //string result = send.SendOrderSubmitRequest();

        Response.Write("ok");
        Response.End();
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

        //result = "0,<span id='id_580000200202' class='base_txtdiv' onmouseover=javascript:onStopHover('580000200202#SNH#NJH') onmouseout='onStopOut()'>2002</span>,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;上海南&nbsp;&nbsp;&nbsp;&nbsp;<br>&nbsp;&nbsp;&nbsp;&nbsp;04:16,<img src='/otsweb/images/tips/last.gif'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;南京&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br>&nbsp;&nbsp;&nbsp;&nbsp;08:32,04:16,--,--,--,--,--,5,<font color='#008800'>有</font>,--,<font color='#008800'>有</font>,<font color='#008800'>有</font>,--,<input type='button' class='yuding_u' onmousemove=this.className='yuding_u_over' onmousedown=this.className='yuding_u_down' onmouseout=this.className='yuding_u' onclick=javascript:getSelected('2002#04:16#04:16#580000200202#SNH#NJH#08:32#上海南#南京#1*****31904*****00051*****01273*****0059#CFFED964FB3F995E4F5EDD74BC59777FABAA62E950EE22AF23A6244D') value='预订'></input>\n1,<span id='id_630000K5280B' class='base_txtdiv' onmouseover=javascript:onStopHover('630000K5280B#SNH#NJH') onmouseout='onStopOut()'>K528</span>,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;上海南&nbsp;&nbsp;&nbsp;&nbsp;<br>&nbsp;&nbsp;&nbsp;&nbsp;04:29,<img src='/otsweb/images/tips/last.gif'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;南京&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br>&nbsp;&nbsp;&nbsp;&nbsp;08:10,03:41,--,--,--,--,--,1,10,--,<font color='darkgray'>无</font>,<font color='#008800'>有</font>,--,<input type='button' class='yuding_u' onmousemove=this.className='yuding_u_over' onmousedown=this.className='yuding_u_down' onmouseout=this.className='yuding_u' onclick=javascript:getSelected('K528#03:41#04:29#630000K5280B#SNH#NJH#08:10#上海南#南京#1*****34224*****00011*****00003*****0010#923D194A6C3DA8AEEFC224D9D7D6E9C444B680F5B34074E4ABF4792B') value='预订'></input>\n";

        Regex reg = new Regex(@">([^\<]*)</span>,[\s\S]*?&nbsp;([^\&\;']+)&nbsp;[\s\S]*?([0-9]{2}\:[0-9]{2}),[\s\S]*?&nbsp;([^\&\;']+)&nbsp;[\s\S]*?([0-9]{2}\:[0-9]{2}),([^,]*),([^,]*),([^,]*),([^,]*),([^,]*),([^,]*),([^,]*),([^,]*),([^,]*),([^,]*),([^,]*),", RegexOptions.IgnoreCase);
        MatchCollection match = reg.Matches(result);

        Regex regBottom = new Regex(@"onclick\=javascript\:getSelected\('([^']*)'\)", RegexOptions.IgnoreCase);
        MatchCollection matchBottom = regBottom.Matches(result);

        for (int i = 0; i < matchBottom.Count; i++)
        {
            if (i != 0)
                outStr += "|";

            for (int j = 1; j < 17; j++)
            {
                outStr += match[i].Groups[j].ToString().Replace("<font color='#008800'>", "").Replace("</font>", "").Replace("<font color='darkgray'>", "") + ",";
            }

            outStr += matchBottom[i].Groups[1].ToString();
        }

        File.WriteAllText(Server.MapPath("111.txt"), matchBottom.Count + "-" + outStr + "-" + result);

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
            Train t = new Train();
            string cookieStr = t.GetVerifyImgOrder(session);
            Response.End();
        }
        catch
        {
            Response.Write("err");
            Response.End();
        }
    }
}