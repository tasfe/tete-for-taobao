using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Text;
using System.Data;
using System.Web.Security;

public partial class top_blog_accountaddnets : System.Web.UI.Page
{
    public string html = string.Empty;
    public string cookie1 = string.Empty;
    public string cookie2 = string.Empty;
    public string vtoken = string.Empty;
    public string blogName = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");

        //过期判断
        if (string.IsNullOrEmpty(taobaoNick))
        {
            //Response.Write("<script>window.location.href='http://container.open.taobao.com/container?appkey=12159997'</script>");
            //Response.End();
            //return;
        }

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        string netsCookie = netsLogin();
        cookie1 = netsCookie;

        string uid = utils.NewRequest("tbUserName", utils.RequestType.Form);
        string pass = utils.NewRequest("tbPassword", utils.RequestType.Form);

        //帐号录入判断
        string sql = "SELECT COUNT(*) FROM TopBlogAccountNew WHERE nick = '" + taobaoNick + "' AND uid = '" + uid + "' AND typ = 'nets'";
        string count = utils.ExecuteString(sql);
        if (count != "0")
        {
            Response.Write("<script>alert('该帐号已经添加过，请重新输入！');history.go(-1);</script>");
            return;
        }

        //帐号密码判断
        if (html.IndexOf("errorMsg") != -1)
        {
            Response.Write("<script>alert('帐号密码错误，请重新输入！');history.go(-1);</script>");
            return;
        }

        //是否建立博客判断
        if (!CheckBlogExits(uid, pass))
        {
            Response.Write("<script>alert('您的博客尚未开通，请先登录到网易博客开通您的博客再继续添加！');history.go(-1);</script>");
            return;
        }

        //插入数据库
        sql = "INSERT INTO TopBlogAccountNew (" +
                        "uid, " +
                        "pass, " +
                        "nick, " +
                        "count, " +
                        "typ" +
                    " ) VALUES ( " +
                        " '" + uid + "', " +
                        " '" + pass + "', " +
                        " '" + taobaoNick + "', " +
                        " '0', " +
                        " 'nets' " +
                  ") ";

        utils.ExecuteNonQuery(sql);

        Response.Write(sql);
        //Response.Redirect("accountlist.aspx");
    }

    //判断博客是否存在
    private bool CheckBlogExits(string uid, string pass)
    {
        string title = "特特博客推广专家_专业淘宝博客营销/博客群发/博客推广/宝贝发博客";
        string content = "特特博客推广专家，让您的产品/店铺的链接出现在千百个热门博客的文章中，大幅提升店铺人气和下单率，完全自动智能发送，减轻了您的推广负担...支持：QQ空间，百度空间，新浪博客，网易博客，搜狐博客，淘江湖...<br><br>此文是<a href='http://www.7fshop.com/' target='_blank'>特特博客推广专家</a>用来测试您提供的账号密码是否正常。。。看到此文说明正常~ ";

        AddBlogCookie(cookie1);

        //准备生成
        string strHtml = string.Empty;

        StreamReader sr = null; //用来读取流
        StreamWriter sw = null; //用来写文件
        Encoding code = Encoding.GetEncoding("gb2312"); //定义编码 
        ASCIIEncoding encoding = new ASCIIEncoding();
        title = HttpUtility.UrlEncode(title);
        content = HttpUtility.UrlEncode(content);
        string postData = "tag=&cls=fks_084066082095083067080084084095085081087065086080084069080&allowview=-100&refurl=&abstract=&bid=&origClassId=&origPublishState=&oldtitle=&todayPublishedCount=0&NETEASE_BLOG_TOKEN_EDITBLOG=" + vtoken + "&title=" + title + "&HEContent=" + content + "&copyPhotos=&msyn=1&p=1";
        byte[] data = code.GetBytes(postData);
        //构造web请求，发送请求，获取响应
        HttpWebRequest HttpWebRequest = null;
        string url = "http://api.blog.163.com/" + blogName + "/editBlogNew.do?p=1&n=1";
        HttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        HttpWebRequest.Method = "POST";
        HttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; InfoPath.2; .NET4.0C; .NET4.0E)";
        HttpWebRequest.ContentType = "application/x-www-form-urlencoded";
        HttpWebRequest.ContentLength = data.Length;
        HttpWebRequest.Referer = "http://api.blog.163.com/crossdomain.html?t=20100205";
        HttpWebRequest.Headers.Set("Cookie", cookie1 + cookie2);

        Stream newStream = HttpWebRequest.GetRequestStream();
        newStream.Write(data, 0, data.Length);
        newStream.Close();
        //发送
        WebResponse HttpWebResponse = null;
        HttpWebResponse = HttpWebRequest.GetResponse();
        //获得流
        sr = new StreamReader(HttpWebResponse.GetResponseStream(), code);
        strHtml = sr.ReadToEnd();

        if (strHtml.IndexOf("r:1") != -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void AddBlogCookie(string cookie)
    {
        //准备生成
        string strHtml = string.Empty;
        StreamReader sr = null; //用来读取流
        Encoding code = Encoding.GetEncoding("utf-8"); //定义编码 
        ASCIIEncoding encoding = new ASCIIEncoding();
        //构造web请求，发送请求，获取响应
        HttpWebRequest HttpWebRequest = null;
        HttpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + blogName + ".blog.163.com/blog/getBlog.do");
        HttpWebRequest.Headers.Set("Cookie", cookie);
        //发送
        WebResponse HttpWebResponse = null;
        HttpWebResponse = HttpWebRequest.GetResponse();
        //获得流
        sr = new StreamReader(HttpWebResponse.GetResponseStream(), code);
        strHtml = sr.ReadToEnd();

        string aaa = HttpWebResponse.Headers.Get("Set-Cookie").ToString();
        //保存进COOKIE
        cookie2 = getKeyData(@"NTESBLOGSI=([^;]*);", aaa);

        vtoken = new Regex(@"<input class=""ytag"" type=""hidden"" name=""NETEASE_BLOG_TOKEN_EDITBLOG"" value=""([^""]*)""/>", RegexOptions.IgnoreCase).Match(strHtml).Groups[1].ToString();
    }

    public static string MD5(string str)
    {
        return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
    }

    private string netsLogin()
    {
        Common.Cookie cookie = new Common.Cookie();
        string verify = cookie.getCookie("qqverify");

        string strHtml = string.Empty;

        StreamReader sr = null; //用来读取流
        StreamWriter sw = null; //用来写文件
        Encoding code = Encoding.GetEncoding("gb2312"); //定义编码 
        ASCIIEncoding encoding = new ASCIIEncoding();

        string uid = utils.NewRequest("tbUserName", utils.RequestType.Form);
        string pass = utils.NewRequest("tbPassword", utils.RequestType.Form);

        uid = HttpUtility.UrlEncode(uid);
        //pass = HttpUtility.UrlEncode(pass);
        pass = MD5(pass).ToLower();

        string postData = "callCount=1\r\n";
        postData += "page=/\r\n";
        postData += "httpSessionId=\r\n";
        postData += "scriptSessionId=4FC528EBDD341FE22046F074D587F373767\r\n";
        postData += "c0-scriptName=UserBean\r\n";
        postData += "c0-methodName=checkUrsAccount\r\n";
        postData += "c0-id=0\r\n";
        postData += "c0-param0=string:" + uid + "\r\n";
        postData += "c0-param1=string:" + pass + "\r\n";
        postData += "c0-param2=boolean:false\r\n";
        postData += "c0-param3=number:0\r\n";
        postData += "c0-param4=boolean:false\r\n";
        postData += "c0-param5=boolean:false\r\n";
        postData += "c0-param6=boolean:false\r\n";
        postData += "batchId=1";

        byte[] data = code.GetBytes(postData);
        //构造web请求，发送请求，获取响应
        HttpWebRequest HttpWebRequest = null;
        string url = "http://blog.163.com/dwr/call/plaincall/UserBean.checkUrsAccount.dwr";
        HttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        HttpWebRequest.Method = "POST";
        HttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; InfoPath.2; .NET4.0C; .NET4.0E)";
        HttpWebRequest.ContentType = "text/plain";
        HttpWebRequest.ContentLength = data.Length;
        HttpWebRequest.Referer = "http://blog.163.com/";

        Stream newStream = HttpWebRequest.GetRequestStream();
        newStream.Write(data, 0, data.Length);
        newStream.Close();
        //发送
        WebResponse HttpWebResponse = null;
        HttpWebResponse = HttpWebRequest.GetResponse();
        //获得流
        sr = new StreamReader(HttpWebResponse.GetResponseStream(), code);
        strHtml = sr.ReadToEnd();

        html = strHtml;

        string aaa = HttpWebResponse.Headers.Get("Set-Cookie").ToString();
        //保存进COOKIE
        string cookieStr = string.Empty;

        cookieStr += getKeyData(@"NTES_SESS=([^;]*);", aaa);
        cookieStr += getKeyData(@"S_INFO=([^;]*);", aaa);
        cookieStr += getKeyData(@"P_INFO=([^;]*);", aaa);
        cookieStr += getKeyData(@"NETEASE_AUTH_USERNAME=([^;]*);", aaa);
        cookieStr += getKeyData(@"USERTRACK=([^;]*);", aaa);
        cookieStr += getKeyData(@"NTESBLOGSI=([^;]*);", aaa);

        blogName = getKeyData(@"NETEASE_AUTH_USERNAME=([^;]*);", aaa).Replace("NETEASE_AUTH_USERNAME=", "").Replace(";", "");

        return cookieStr;
    }

    private string getKeyData(string pat, string str)
    {
        string verify = string.Empty;
        Regex reg = new Regex(pat, RegexOptions.IgnoreCase);
        if (reg.IsMatch(str))
        {
            Match match = reg.Match(str);
            verify = match.Groups[0].ToString();
        }
        return verify;
    }
}