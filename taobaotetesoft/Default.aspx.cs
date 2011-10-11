using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Net.Sockets;
using Common;
using System.Data;

public partial class _Default : System.Web.UI.Page
{
    public string taobaoCookie = string.Empty;
    public string loginTaobaoCookieStr = string.Empty;
    public string loginTelCookieStr = string.Empty;
    public string taobaoUrl = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        area1.Visible = false;
        area2.Visible = true;

        Test();
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        string code = Request.Form["code"];
        string param = paramHidden.Value;

        string postData = "param=" + param + "&style=tb&checkcode=" + code;
        string codeUrl = GetHtmlByUrlpost("http://member1.taobao.com/member/safe/check_phonecodeunusallogin.do?_input_charset=utf-8", postData, "");

        string loginCookieStr = loginTelCookieStr;

        //获取登录COOKIE
        taobaoCookie += getKeyData(@"uc1=([^;]*);", loginCookieStr);
        taobaoCookie += getKeyData(@"tlut=([^;]*);", loginCookieStr);
        taobaoCookie += getKeyData(@"_cc_=([^;]*);", loginCookieStr);
        taobaoCookie += getKeyData(@"t=([^;]*);", loginCookieStr);
        taobaoCookie += getKeyData(@"_nk_=([^;]*);", loginCookieStr);
        taobaoCookie += getKeyData(@"_l_g_=([^;]*);", loginCookieStr);
        taobaoCookie += getKeyData(@"cookie2=([^;]*);", loginCookieStr);
        taobaoCookie += getKeyData(@"_wwmsg=([^;]*);", loginCookieStr);
        taobaoCookie += getKeyData(@"tracknick=([^;]*);", loginCookieStr);
        taobaoCookie += getKeyData(@"lastgetwwmsg=([^;]*);", loginCookieStr);
        taobaoCookie += getKeyData(@"cookie1=([^;]*);", loginCookieStr);
        taobaoCookie += getKeyData(@"cookie17=([^;]*);", loginCookieStr);
        taobaoCookie += getKeyData(@"cna=([^;]*);", loginCookieStr);
        taobaoCookie += getKeyData(@"lzstat_uv=([^;]*);", loginCookieStr);
        taobaoCookie += getKeyData(@"ck1=([^;]*);", loginCookieStr);
        taobaoCookie += getKeyData(@"tg=([^;]*);", loginCookieStr);
        taobaoCookie += getKeyData(@"nt=([^;]*);", loginCookieStr);
        taobaoCookie += getKeyData(@"ssllogin=([^;]*);", loginCookieStr);
        taobaoCookie += getKeyData(@"JSESSIONID=([^;]*);", loginCookieStr);
        taobaoCookie += getKeyData(@"_tb_token_=([^;]*);", loginCookieStr);
        taobaoCookie += getKeyData(@"v=([^;]*);", loginCookieStr);
        taobaoCookie += getKeyData(@"_lang=([^;]*);", loginCookieStr);
        taobaoCookie += getKeyData(@"x=([^;]*);", loginCookieStr);
        taobaoCookie += getKeyData(@"publishItemObj=([^;]*);", loginCookieStr);  
 
        Response.Write("登录成功，您的验证COOKIE是" + taobaoCookie);

        Common.Rijndael_ encode = new Rijndael_("tetetete");
        Common.Cookie cookie = new Common.Cookie();
        string nick = encode.Decrypt(cookie.getCookie("nick"));
        Common.Cookie c = new Common.Cookie();
        c.setCookie("taobaoCookie1", taobaoCookie, DateTime.Now.Minute+DateTime.Now.Second);

        string sql = "UPDATE teteUser SET cookie = '" + taobaoCookie + "' WHERE nick = '" + nick + "'";
        Response.Write(sql);
        utils.ExecuteNonQuery(sql);

        Response.Redirect("menu.aspx");
    }

    private void Test()
    {
        string token = GetTaobaoToken();
        string username = JsEncode(Request.Form["uid"]);
        string password = JsEncode(Request.Form["pass"]);

        string postData = "TPL_username=" + username + "&TPL_password=" + password + "&_tb_token_=" + token + "&action=Authenticator&event_submit_do_login=anything&TPL_redirect_url=&from=tb&fc=2&style=default&css_style=&tid=&support=000001&CtrlVersion=1%2C0%2C0%2C7&loginType=3&minititle=&minipara=&pstrong=3&longLogin=-1&llnick=&sign=&need_sign=&isIgnore=&popid=&callback=&guf=&not_duplite_str=&need_user_id=&poy=&gvfdcname=10&from_encoding=";
        string cookie = loginTaobaoCookieStr;

        int port = 80;
        //IPAddress[] ip = Dns.GetHostAddresses("login.taobao.com");
        //string host = ip[0].Address;
        IPAddress ip = Dns.GetHostAddresses("login.taobao.com")[0];
        IPEndPoint ipe = new IPEndPoint(ip, port);//把ip和端口转化为IPEndPoint实例
        Socket c = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建一个Socket

        c.Connect(ipe);//连接到服务器

        c.Send(Encoding.Default.GetBytes("POST http://login.taobao.com/member/login.jhtml HTTP/1.1\r\n"));
        c.Send(Encoding.Default.GetBytes("Host: login.taobao.com\r\n"));
        c.Send(Encoding.Default.GetBytes("User-Agent: Mozilla/5.0 (Windows; U; Windows NT 5.2; zh-CN; rv:1.9.2) Gecko/20100115 Firefox/3.6 (.NET CLR 3.5.30729)\r\n"));
        c.Send(Encoding.Default.GetBytes("Accept: text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8\r\n"));
        //c.Send(Encoding.Default.GetBytes("Accept-Language: zh-cn,zh;q=0.5\r\n"));
        //c.Send(Encoding.Default.GetBytes("Accept-Encoding: gzip,deflate\r\n"));
        //c.Send(Encoding.Default.GetBytes("Accept-Charset: GB2312,utf-8;q=0.7,*;q=0.7\r\n"));
        c.Send(Encoding.Default.GetBytes("Keep-Alive: 115\r\n"));
        c.Send(Encoding.Default.GetBytes("Connection: keep-alive\r\n"));
        c.Send(Encoding.Default.GetBytes("Cookie: " + cookie + "\r\n"));
        c.Send(Encoding.Default.GetBytes("Content-Type: application/x-www-form-urlencoded\r\n"));
        c.Send(Encoding.Default.GetBytes("Content-Length: " + postData.Length.ToString() + "\r\n"));
        c.Send(Encoding.Default.GetBytes("\r\n"));
        c.Send(Encoding.Default.GetBytes(postData + "\r\n"));

        string loginCookieStr = string.Empty;

        byte[] buffer = new byte[10240];
        int byteCount = c.Receive(buffer, buffer.Length, 0);
        loginCookieStr = Encoding.Default.GetString(buffer, 0, byteCount);
        while (byteCount > 0)
        {
            byteCount = c.Receive(buffer, buffer.Length, 0);
            loginCookieStr += Encoding.Default.GetString(buffer, 0, byteCount);
        }

        //Response.Write(loginCookieStr);

        //判断是否需要输入验证码
        //if (loginCookieStr.IndexOf("Location:") == -1)
        //{ 
        //    //获取验证码
        //    area1.Visible = false;
        //    area2.Visible = false;
        //    area3.Visible = true;

        //    return;
        //}

        //Response.Write("<textarea>" + loginCookieStr + "</textarea>");


        taobaoUrl = new Regex(@"Location: ([\S]*)", RegexOptions.IgnoreCase).Match(loginCookieStr).Groups[1].ToString();
        //获取跳转1
        string text = GetHtmlByUrlget(taobaoUrl, "");
        string redirectUrl1 = new Regex(@"window\.location = ""([^""]*)"";", RegexOptions.IgnoreCase).Match(text).Groups[1].ToString();

        //获取跳转2
        text = GetHtmlByUrlget(redirectUrl1, "");
        string redirectUrl2 = new Regex(@"TB\.SecurityPop\.attach\(""J_Dredge"", ""([^""]*)""\);", RegexOptions.IgnoreCase).Match(text).Groups[1].ToString();

        //获取手机验证码地址
        text = GetHtmlByUrlget(redirectUrl2, "");
        string param = new Regex(@"<input name=""param""  type=""hidden"" value=""([^""]*)"">", RegexOptions.IgnoreCase).Match(text).Groups[1].ToString();
        string telUrl = "http://member1.taobao.com/member/safe/sendcheckcodeunusuallogin.do?param=" + param;
        paramHidden.Value = param;

        //发送手机验证码
        text = GetHtmlByUrlget(telUrl, "");

        //记录COOKIE
        Common.Rijndael_ encode = new Rijndael_("tetetete");
        Common.Cookie cookieObj = new Common.Cookie();
        cookieObj.setCookie("nick", encode.Encrypt(Request.Form["uid"]), 999999);

        //保存用户数据
        string sql = "SELECT COUNT(*) FROM teteUser WHERE nick = '" + Request.Form["uid"].Replace("'", "''") + "'";
        string count = utils.ExecuteString(sql);
        if (count == "0")
        {
            sql = "INSERT INTO teteUser (nick, pass) VALUES ('" + Request.Form["uid"].Replace("'", "''") + "', '" + Request.Form["pass"].Replace("'", "''") + "')";
        }
        else
        {
            sql = "UPDATE teteUser SET loginDate = GETDATE(),loginTimes = loginTimes + 1 WHERE nick = '" + Request.Form["uid"].Replace("'", "''") + "'";
        }
        utils.ExecuteNonQuery(sql);


        //输入验证码
        //string code = "793768";
        

        ////获取跳转地址
        //if (text.IndexOf("script_redirect.htm") == -1)
        //{
        //    //如果开启短信验证
        //    string param = new Regex(@"<input name=""param""  type=""hidden"" value=""([^""]*)"">", RegexOptions.IgnoreCase).Match(loginCookieStr).Groups[1].ToString();
        //    string telUrl = "http://member1.taobao.com/member/safe/sendcheckcodeunusuallogin.do?param=" + param;

        //    MessageBox.Show(telUrl);
        //}
    }

    private string GetTaobaoToken()
    {
        string url = "http://login.taobao.com/member/login.jhtml";
        string text = GetHtmlByUrlget(url, "");

        string token = new Regex(@"<input name='_tb_token_' type='hidden' value='([^']*)'>", RegexOptions.IgnoreCase).Match(text).Groups[1].ToString();

        return token;
    }


    #region URL相关方法
    /// <summary>
    /// 正则获取COOKIE中的参数并重新拼装
    /// </summary>
    /// <param name="pat"></param>
    /// <param name="str"></param>
    /// <returns></returns>
    private string getKeyData(string pat, string str)
    {
        string verify = string.Empty;
        Regex reg = new Regex(pat);
        if (reg.IsMatch(str))
        {
            Match match = reg.Match(str);
            verify = match.Groups[0].ToString();
        }
        return verify;
    }

    /// <summary>
    /// 模拟JS的URL加密
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private string JsEncode(string str)
    {
        string newStr = string.Empty;
        for (int i = 0; i < str.Length; i++)
        {
            newStr += VBEncode(str.Substring(i, 1));
        }
        return newStr;
    }

    /// <summary>
    /// 计算字符ASCII码
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private string VBEncode(string str)
    {
        //如果不是中文则直接使用urlencode
        if (Microsoft.VisualBasic.Strings.Asc(str) > -1000)
        {
            //Response.Write(Microsoft.VisualBasic.Strings.Asc(str) + "-" + str + "<br>");
            return HttpUtility.UrlEncode(str);
        }

        string newStr = Microsoft.VisualBasic.Conversion.Hex(Microsoft.VisualBasic.Strings.Asc(str));
        newStr = newStr.Replace("FFFF", "");
        if (newStr.Length == 4)
        {
            newStr = "%" + newStr.Substring(0, 2) + "%" + newStr.Substring(2, 2);
        }
        else
        {
            newStr = "%" + newStr;
        }
        return newStr;
    }

    /// <summary>
    /// 根据网页地址获取返回HEADER
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    private string GetCookieByUrlGet(string url)
    {
        //发送HTTPS请求设置
        Encoding code = Encoding.Default; //定义编码 
        ASCIIEncoding encoding = new ASCIIEncoding();
        //构造web请求，发送请求，获取响应
        HttpWebRequest HttpWebRequest = null;
        if (url == "")
        {
            return "";
        }
        HttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        HttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; InfoPath.2; .NET4.0C; .NET4.0E)";
        HttpWebRequest.ContentType = "application/x-www-form-urlencoded";
        HttpWebRequest.ServicePoint.Expect100Continue = false;

        //发送
        WebResponse HttpWebResponse = null;
        HttpWebResponse = HttpWebRequest.GetResponse();
        string aaa = HttpWebResponse.Headers.Get("Set-Cookie").ToString();

        return aaa;
    }

    /// <summary>
    /// 根据网页地址获取返回HEADER
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    private string GetCookieByUrl(string url, string postData)
    {
        //发送HTTPS请求设置
        Encoding code = Encoding.Default; //定义编码 
        ASCIIEncoding encoding = new ASCIIEncoding();
        byte[] data = code.GetBytes(postData);
        //构造web请求，发送请求，获取响应
        HttpWebRequest HttpWebRequest = null;
        if (url == "")
        {
            return "";
        }
        HttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        HttpWebRequest.Method = "POST";
        HttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; InfoPath.2; .NET4.0C; .NET4.0E)";
        HttpWebRequest.ContentType = "application/x-www-form-urlencoded";
        HttpWebRequest.ContentLength = data.Length;
        HttpWebRequest.ServicePoint.Expect100Continue = false;

        Stream newStream = HttpWebRequest.GetRequestStream();
        newStream.Write(data, 0, data.Length);
        newStream.Close();
        //发送
        WebResponse HttpWebResponse = null;
        HttpWebResponse = HttpWebRequest.GetResponse();
        string aaa = HttpWebResponse.Headers.Get("Set-Cookie").ToString();

        return aaa;
    }

    /// <summary>
    /// 根据网页地址获取返回HTML
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    private string GetHtmlByUrlget(string url, string cookie)
    {
        //发送HTTPS请求设置
        System.Net.ServicePointManager.Expect100Continue = false;

        //准备生成
        string strHtml = string.Empty;
        StreamReader sr = null; //用来读取流
        Encoding code = Encoding.Default; //定义编码 
        ASCIIEncoding encoding = new ASCIIEncoding();
        //构造web请求，发送请求，获取响应
        HttpWebRequest HttpWebRequest = null;
        if (url == "")
        {
            return "";
        }

        HttpWebRequest = (HttpWebRequest)WebRequest.Create("http://login.taobao.com/member/login.jhtml");
        HttpWebRequest.Headers.Set("Cookie", cookie);
        //发送
        WebResponse HttpWebResponse = null;
        HttpWebResponse = HttpWebRequest.GetResponse();
        //获得流
        sr = new StreamReader(HttpWebResponse.GetResponseStream(), code);
        strHtml = sr.ReadToEnd();

        string loginCookieStr = HttpWebResponse.Headers.Get("Set-Cookie").ToString();

        loginTaobaoCookieStr += getKeyData(@"cookie2=([^;]*);", loginCookieStr);
        loginTaobaoCookieStr += getKeyData(@"_tb_token_=([^;]*);", loginCookieStr);
        loginTaobaoCookieStr += getKeyData(@"t=([^;]*);", loginCookieStr);
        loginTaobaoCookieStr += getKeyData(@"uc1=([^;]*);", loginCookieStr);

        return strHtml;
    }

    /// <summary>
    /// 根据网页地址获取返回HTML
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    private string GetHtmlByUrlpost(string url, string postData, string cookie)
    {
        //发送HTTPS请求设置
        System.Net.ServicePointManager.Expect100Continue = false;

        //准备生成
        string strHtml = string.Empty;

        StreamReader sr = null; //用来读取流
        Encoding code = Encoding.Default; //定义编码 
        ASCIIEncoding encoding = new ASCIIEncoding();
        //构造POST请求
        byte[] data = code.GetBytes(postData);
        //构造web请求，发送请求，获取响应
        HttpWebRequest HttpWebRequest = null;
        if (url == "")
        {
            return "";
        }
        HttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        HttpWebRequest.Method = "POST";
        HttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; InfoPath.2; .NET4.0C; .NET4.0E)";
        HttpWebRequest.ContentType = "application/x-www-form-urlencoded";
        HttpWebRequest.ContentLength = data.Length;
        HttpWebRequest.Headers.Set("Cookie", cookie);

        Stream newStream = HttpWebRequest.GetRequestStream();
        newStream.Write(data, 0, data.Length);
        newStream.Close();
        //发送
        WebResponse HttpWebResponse = null;
        HttpWebResponse = HttpWebRequest.GetResponse();
        //获得流
        sr = new StreamReader(HttpWebResponse.GetResponseStream(), code);
        strHtml = sr.ReadToEnd();

    
        loginTelCookieStr = HttpWebResponse.Headers.Get("Set-Cookie").ToString();
        Response.Write(loginTelCookieStr + "ddddddddd");
        return strHtml;
    }
    #endregion
}