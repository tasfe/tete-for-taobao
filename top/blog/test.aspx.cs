using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Common;

public partial class top_blog_test : System.Web.UI.Page
{
    public string vtoken = string.Empty;
    public string proxy = string.Empty;
    public string cookie2 = string.Empty;
    public string cookie1 = string.Empty;
    public string contentNewPic = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string path = Server.MapPath("~/auto/ipnow.txt");
        if (File.Exists(path))
        {
            proxy = File.ReadAllText(path);
        }
    }

    /// <summary>
    /// 测试
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {
        //Response.Write(GetRequestBySocket("www.qq.com", "", ""));
        cookie1 = netsLogin(this.TextBox1.Text, this.TextBox2.Text);
        //文章发布页cookie
        AddBlogCookieNet(cookie1);
        //发布文章
        SendBlog(TextBox1.Text, "hexun");

        Response.Write("<script>alert('添加成功！');</script>");
    }


    /// <summary>
    /// 替换关键字
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    private string changeKey(string content)
    {
        Common.Cookie cookie1 = new Common.Cookie();
        string taobaoNick1 = cookie1.getCookie("nick");
        string nickid = string.Empty;

        //COOKIE过期判断
        if (taobaoNick1 == "")
        {
            //SESSION超期 跳转到登录页
            Response.Write("<script>parent.location.href='http://container.open.taobao.com/container?appkey=12159997'</script>");
            Response.End();
        }
        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick1 = encode.Decrypt(taobaoNick1);
 

        //替换掉博客文章里面的关键字
        string sql = "SELECT * FROM TopBlogLink WHERE nick = '" + taobaoNick1 + "'";
        DataTable dt = utils.ExecuteDataTable(sql);

        if (dt != null && dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                content = content.Replace(dt.Rows[i]["keyword"].ToString(), "<a href='" + dt.Rows[i]["link"].ToString() + "' target='_blank'>" + dt.Rows[i]["keyword"].ToString() + "</a>");
            }
        }

 
        return content;
    }

    /// <summary>
    /// 替换图片路径
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    private string changePicUrl(string content)
    {
        Regex reg = new Regex(@"src=""(http\:\/\/static[^""]*)""", RegexOptions.IgnoreCase);
        content = reg.Replace(content, "src=\"http://www.7fshop.com/top/blog/sinaverify.aspx?u=$1\"");

        return content;
    }

    /// <summary>
    /// 发送文章
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="typ"></param>
    /// <returns></returns>
    private string SendBlog(string uid, string typ)
    {

        string title = this.TextBox3.Text;
        string content = HttpUtility.HtmlDecode(this.TextBox4.Text);
        string date = DateTime.Now.ToString("yyyy-MM-dd");
        string time = DateTime.Now.ToString("HH:mm:ss");

        //替换关键字连接
      //  content = changeKey(content);

        //替换新浪博客图片
        contentNewPic = changePicUrl(content);


        string trueSinaCookie = cookie1;// +cookie2;// +cookie.getCookie("sinaCookieAdd").Replace("|", ";");

        //准备生成
        string strHtml = string.Empty;

        StreamReader sr = null; //用来读取流
        StreamWriter sw = null; //用来写文件
        Encoding code = Encoding.GetEncoding("gb2312"); //定义编码 
        ASCIIEncoding encoding = new ASCIIEncoding();

        string postData = "TitleTextbox=" + title + "&postid=0&CategoryList=0&NewCategoryTextbox=&AutoSaveCheckbox=on&ContentSpaw=" + contentNewPic + "&TagTextbox=" + title + "&PostType=%D4%AD%B4%B4&PostClass=0&oldclass=0&SourceTextbox=&SourceUrlTextbox=&BriefTextbox=&TrackbackTextbox=&HideCheckbox=1&AcceptCommentCheckbox=on&StickOutExpiredTimeTextbox=&StickOutOrderNumberTextbox=0&chkSelected=&draftid=0&articleid=0&action=0&VerificationInput=";
 
        byte[] data = code.GetBytes(postData);
        //构造web请求，发送请求，获取响应
        HttpWebRequest HttpWebRequest = null;
        string url = "http://post.blog.hexun.com/16125841/postarticlesubmit.aspx";
        HttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        HttpWebRequest.Method = "POST";
        HttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; InfoPath.2; .NET4.0C; .NET4.0E)";
        HttpWebRequest.ContentType = "application/x-www-form-urlencoded";
        HttpWebRequest.ContentLength = data.Length;
        HttpWebRequest.Referer = "http://post.blog.hexun.com/16125841/postarticle.aspx";
        HttpWebRequest.Headers.Set("Cookie", cookie1);


        if (proxy != "")
        {
            string[] proxyArray = proxy.Split(':');
            HttpWebRequest.Proxy = new WebProxy(proxyArray[0], int.Parse(proxyArray[1]));
            HttpWebRequest.Timeout = 40000;
        }

        Stream newStream = HttpWebRequest.GetRequestStream();
        newStream.Write(data, 0, data.Length);
        newStream.Close();
        //发送
        WebResponse HttpWebResponse = null;
        HttpWebResponse = HttpWebRequest.GetResponse();
        //获得流
        sr = new StreamReader(HttpWebResponse.GetResponseStream(), code);
        strHtml = sr.ReadToEnd();

        //获取文章地址
        string address = new Regex(@"""sfx"":""([^""]*)""", RegexOptions.IgnoreCase).Match(strHtml).Groups[1].ToString();
        address = "http://16125841.blog.hexun.com/61783425_a.html";


        //RecordData(address, uid, "hexun", "");

        return address;

    }

    /// <summary>
    /// 数据库记录
    /// </summary>
    private void RecordData(string url, string uid, string typ, string strHtml)
    {
        strHtml = HttpUtility.HtmlEncode(strHtml);

        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        string session = cookie.getCookie("top_sessionblog");

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        //COOKIE过期判断
        if (taobaoNick == "")
        {
            //SESSION超期 跳转到登录页
            Response.Write("<script>parent.location.href='http://container.open.taobao.com/container?appkey=12159997'</script>");
            Response.End();
        }

        string sql = "INSERT INTO TopBlog (" +
                                "blogtitle, " +
                                "blogurl, " +
                                "nick, " +
                                "status, " +
                                "uid, " +
                                "result, " +
                                "typ" +
                            " ) VALUES ( " +
                                " '" + this.TextBox3.Text.Replace("'", "''") + "', " +
                                " '" + url + "', " +
                                " '" + taobaoNick + "', " +
                                " '1', " +
                                " '" + uid + "', " +
                                " '" + strHtml.Replace("'", "''") + "', " +
                                " '" + typ + "' " +
                          ") ";

        utils.ExecuteNonQuery(sql);


        //记录该帐号发送文章数量
        sql = "UPDATE TopBlogAccountNew SET count = count + 1 WHERE uid = '" + uid + "'";
        utils.ExecuteNonQuery(sql);
    }


    public static string MD5(string str)
    {
        return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
    }

 

    /// <summary>
    /// 模拟登录
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="pass"></param>
    /// <returns></returns>
    private string netsLogin(string uid, string pass)
    {
        #region
        Common.Cookie cookie = new Common.Cookie();
        string verify = cookie.getCookie("qqverify");

        string strHtml = string.Empty;

        StreamReader sr = null; //用来读取流
        StreamWriter sw = null; //用来写文件
        Encoding code = Encoding.Default; //定义编码 

        ASCIIEncoding encoding = new ASCIIEncoding();

        uid = HttpUtility.UrlEncode(uid);
        pass = HttpUtility.UrlEncode(pass);
        //pass = MD5(pass).ToLower();

        //构造POST请求
        string postData = "TextBoxUserName=" + uid + "&encoding=deflate&TextBoxPassword=" + pass + "&LoginStateName=1&LoginStateAuto=1&submitsign=1&hiddenReferrer=http%3a%2f%2fblog.hexun.com&TextGpic=&gourl=http://blog.hexun.com&callback=";


        byte[] data = code.GetBytes(postData);
        //构造web请求，发送请求，获取响应
        HttpWebRequest HttpWebRequest = null;
        string url = "http://reg.hexun.com/login.aspx";
        HttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        HttpWebRequest.Method = "POST";
        HttpWebRequest.AllowWriteStreamBuffering = false;
        HttpWebRequest.AllowAutoRedirect = false;

        HttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; InfoPath.2; .NET4.0C; .NET4.0E)";
        HttpWebRequest.ContentType = "application/x-www-form-urlencoded";
        HttpWebRequest.ContentLength = data.Length;


        if (proxy != "")
        {
            string[] proxyArray = proxy.Split(':');
            HttpWebRequest.Proxy = new WebProxy(proxyArray[0], int.Parse(proxyArray[1]));
            HttpWebRequest.Timeout = 40000;
        }

        Stream newStream = HttpWebRequest.GetRequestStream();
        newStream.Write(data, 0, data.Length);
        newStream.Close();
        //发送
        WebResponse HttpWebResponse = null;
        HttpWebResponse = HttpWebRequest.GetResponse();
        //获得流
        sr = new StreamReader(HttpWebResponse.GetResponseStream(), code);
        strHtml = sr.ReadToEnd();

        //Response.End();
        //html = strHtml;

        string aaa = HttpWebResponse.Headers.Get("Set-Cookie").ToString();
        //保存进COOKIE
        string cookieStr = string.Empty;

        cookieStr += getKeyData(@"userToken=([^;]*);", aaa);
        cookieStr += getKeyData(@"hxck_sq_common=([^;]*);", aaa);
        cookieStr += getKeyData(@"domain=([^;]*);", aaa);

        cookieStr += getKeyData(@"HexunTrack=([^;]*);", aaa);
        cookieStr += getKeyData(@"__utma=([^;]*);", aaa);
        cookieStr += getKeyData(@"__utmz=([^;]*);", aaa);
        cookieStr += getKeyData(@"vjuids=([^;]*);", aaa);
        cookieStr += getKeyData(@"vjlast=([^;]*);", aaa);
        cookieStr += getKeyData(@"ASP.NET_SessionId=([^;]*);", aaa);
 

        return cookieStr;

        #endregion

        #region  Socket
        // int port = 80;
        // string host = "119.75.217.56";
        // IPAddress ip = IPAddress.Parse(host);
        // IPEndPoint ipe = new IPEndPoint(ip, port);//把ip和端口转化为IPEndPoint实例
        // Socket c = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建一个Socket



        // Response.Write("Conneting...\n");
        // c.Connect(ipe);//连接到服务器
        // string sendStr = "GET / HTTP/1.1\r\nAccept: image/jpeg, application/x-ms-application, image/gif, application/xaml+xml, image/pjpeg, application/x-ms-xbap, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/QVOD, application/QVOD, */*\r\nAccept-Language: zh-CN\r\nUser-Agent: Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; InfoPath.2; .NET4.0C; .NET4.0E)\r\nAccept-Encoding: gzip, deflate\r\nHost: www.baidu.com\r\n";

        // byte[] bs = Encoding.Default.GetBytes(sendStr);
        // c.Send(bs, bs.Length, 0);//发送测试信息
        // string recvStr = " ";
        // byte[] recvBytes = new byte[1024];
        // int bytes = 0;


        //// bytes = c.Receive(recvBytes, recvBytes.Length, 0);//从服务器端接受返回信息

        // while (true)
        // {
        //     bytes = c.Receive(recvBytes, recvBytes.Length, 0);
        //     //读取完成后退出循环 
        //     if (bytes <= 0)
        //         break;
        //     //将读取的字节数转换为字符串 
        //     //recvStr += ASCII.GetString(recvBytes, 0, bytes);
        //     recvStr += Encoding.Default.GetString(recvBytes, 0, bytes);
        // }

        // Response.Write("Client Get Message:" + recvStr);//显示服务器返回信息
        // c.Close();

        // return recvStr;
        #endregion

    }

    /// <summary>
    /// 文章发布页cookie
    /// </summary>
    /// <param name="cookie"></param>
    public void AddBlogCookieNet(string cookie)
    {
        //准备生成
        string strHtml = string.Empty;
        StreamReader sr = null; //用来读取流
        Encoding code = Encoding.GetEncoding("utf-8"); //定义编码 

       
        ASCIIEncoding encoding = new ASCIIEncoding();
        //构造web请求，发送请求，获取响应
        HttpWebRequest HttpWebRequest = null;
        HttpWebRequest = (HttpWebRequest)WebRequest.Create("http://post.blog.hexun.com/16125841/postarticle.aspx");
        HttpWebRequest.UserAgent = " Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; InfoPath.2; .NET4.0C; .NET4.0E)";
        HttpWebRequest.ContentType = "application/x-www-form-urlencoded";
        HttpWebRequest.Headers.Set("Cookie", cookie);
        //发送
        WebResponse HttpWebResponse = null;
        HttpWebResponse = HttpWebRequest.GetResponse();
        //获得流
        sr = new StreamReader(HttpWebResponse.GetResponseStream(), code);
        strHtml = sr.ReadToEnd();

        string aaa = HttpWebResponse.Headers.Get("Set-Cookie").ToString();
        //保存进COOKIE
        vtoken = new Regex(@"<input class=""ytag"" type=""hidden"" name=""NETEASE_BLOG_TOKEN_EDITBLOG"" value=""([^""]*)""/>", RegexOptions.IgnoreCase).Match(strHtml).Groups[1].ToString();
         
        //cookie2 = getKeyData(@"NTESBLOGSI=([^;]*);", aaa);

 
    }



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
   

   

}
 
