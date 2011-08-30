using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using Common;
using System.Data;
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;
using Common;
using System.Web.Security;


public partial class top_blog_blogadd2 : System.Web.UI.Page
{
    public string vtoken = string.Empty;
    public string ads = string.Empty;
    public string cookie1 = string.Empty;
    public string cookie2 = string.Empty;
    public string key = string.Empty;
    public string proxy = string.Empty;
    public string contentNewPic = string.Empty;

    public string blogName = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Common.Cookie cookie = new Common.Cookie();
            this.TextBox1.Text = cookie.getCookie("blogname");
            this.TextBox2.Text = cookie.getCookie("blogpass");
        }

        string path = Server.MapPath("/auto/ipnow.txt");
        if (File.Exists(path))
        {
            proxy = File.ReadAllText(path);
        }

        key = this.tbKey.Text;
        ads = utils.NewRequest("ads", Common.utils.RequestType.Form);

        //判断是否录入新客户
        Common.Cookie cookie1 = new Common.Cookie();
        string taobaoNick = cookie1.getCookie("nick");

        //过期判断
        if (string.IsNullOrEmpty(taobaoNick))
        {
            Response.Write("<script>parent.location.href='http://container.open.taobao.com/container?appkey=12159997'</script>");
            Response.End();
            return;
        }

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        string sql = "SELECT * FROM TopBlogAccountNew WHERE nick = '" + taobaoNick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        //if (dt.Rows.Count != 0)
        //{
        panel1.Visible = false;
        panel2.Visible = true;

        //数据绑定
        rptAccount.DataSource = dt;
        rptAccount.DataBind();
        //}
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");

        //过期判断
        if (string.IsNullOrEmpty(taobaoNick))
        {
            Response.Write("<script>window.location.href='http://container.open.taobao.com/container?appkey=12159997';</script>");
            Response.End();
            return;
        }

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        string level = string.Empty;
        string num = string.Empty;
        string used = string.Empty;

        string sql = "SELECT * FROM TopTaobaoShop WHERE nick = '" + taobaoNick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            string version = dt.Rows[0]["versionNoblog"].ToString();
            switch (version)
            {
                case "1":
                    level = "标准版客户";
                    num = "10";
                    break;
                case "2":
                    level = "专业版客户";
                    num = "30";
                    break;
                case "3":
                    level = "VIP版客户";
                    num = "999999";
                    break;
                default:
                    level = "标准版客户";
                    num = "10";
                    break;
            }
        }

        //获取今日发送的文章数
        sql = "SELECT COUNT(*) FROM TopBlog WHERE nick = '" + taobaoNick + "' AND DATEDIFF(d,GETDATE(),ADDDATE) = 0 AND sendStatus <> 2";
        dt = utils.ExecuteDataTable(sql);
        used = dt.Rows[0][0].ToString();

        if (int.Parse(used) >= int.Parse(num))
        {
            Response.Write("尊敬的" + level + " " + taobaoNick + "，非常抱歉的告诉您，您今天发送的文章已经超出了上限，如需继续发送请<a href='http://pay.taobao.com/mysub/subarticle/upgrade_order_sub_article.htm?market_type=6&article_id=181' target='_blank'>购买高级会员服务</a>，谢谢！<br> <a href='javascript:history.go(-1)'>返回</a>");
            Response.End();
            return;
        }

        string result = string.Empty;

        if (panel1.Visible == false)
        {
            //循环帐号发送
            string account = utils.NewRequest("account", Common.utils.RequestType.Form);
            sql = "SELECT * FROM TopBlogAccountNew WHERE nick = '" + taobaoNick + "' AND CHARINDEX(uid, '" + account + "') > 0";
            dt = utils.ExecuteDataTable(sql);

            //如果帐号为空
            if (dt.Rows.Count == 0)
            {
                Response.Write("<script>alert('请先添加博客帐号！');history.go(-1);</script>");
                Response.End();
                return;
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //对每个发送帐号进行判断，每个帐号每天最多发送5条，防止被封
                sql = "SELECT COUNT(*) FROM TopBlog WHERE nick = '" + taobaoNick + "' AND uid = '" + dt.Rows[i]["uid"].ToString() + "' AND typ = '" + dt.Rows[i]["typ"].ToString() + "' AND DATEDIFF(d,GETDATE(),ADDDATE) = 0 AND sendstatus <> 2";
                string uidCount = utils.ExecuteString(sql);

                if (int.Parse(uidCount) >= 5)
                {
                    if (i == 0)
                    {
                        result = "错误信息：帐号" + dt.Rows[i]["uid"].ToString() + "今天发送的文章数已经超过5篇，为了防止博客被封，今天该帐号无法继续发送博文!<br>";
                    }
                    else
                    {
                        result += "错误信息：帐号" + dt.Rows[i]["uid"].ToString() + "今天发送的文章数已经超过5篇，为了防止博客被封，今天该帐号无法继续发送博文!";
                    }
                    continue;
                }

                string url = SendBlogByAccount(dt.Rows[i]["uid"].ToString(), dt.Rows[i]["pass"].ToString(), dt.Rows[i]["typ"].ToString(), dt.Rows[i]["cookie"].ToString(), taobaoNick);
                if (i == 0)
                {
                    result = "<br><a href='" + url + "' target='_blank'>" + url + "</a>";
                }
                else
                {
                    result += "<br><a href='" + url + "' target='_blank'>" + url + "</a>";
                }
            }
        }
        else
        {
            string uid = this.TextBox1.Text;
            string pass = this.TextBox2.Text;

            string url = SendBlogByAccount(uid, pass, "sina", "", taobaoNick);
            result += "<a href='" + url + "' target='_blank'>" + url + "</a>";
        }

        Response.Write("<script>window.location.href=\"success.aspx?adr=" + HttpUtility.UrlEncode("<br><a href='bloglist.aspx'>点击查看您的文章发送状态</a><br>" + result) + "\";</script>");
    }

    private string SendBlogByAccount(string uid, string pass, string typ, string cookieQQ, string taobaoNick)
    {
        //准备生成
        string strHtml = string.Empty;

        return SendBlog(uid, typ);
    }

    #region hidden
    private string netsLogin(string uid, string pass)
    {
        Common.Cookie cookie = new Common.Cookie();
        string verify = cookie.getCookie("qqverify");

        string strHtml = string.Empty;

        StreamReader sr = null; //用来读取流
        StreamWriter sw = null; //用来写文件
        Encoding code = Encoding.GetEncoding("gb2312"); //定义编码 
        ASCIIEncoding encoding = new ASCIIEncoding();

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

        //html = strHtml;

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

    /// <summary>
    /// 搜狐会员登录
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="pass"></param>
    /// <returns></returns>
    private string sohuLogin(string uid, string pass)
    {
        //准备生成
        string strHtml = string.Empty;
        string url = "http://passport.sohu.com/sso/login.jsp?userid=" + HttpUtility.UrlEncode(uid) + "&password=" + pass + "&appid=9998&persistentcookie=0&s=1293087761161&b=2&w=1440&pwdtype=1&v=26";

        System.Net.ServicePointManager.Expect100Continue = false;

        StreamReader sr = null; //用来读取流
        Encoding code = Encoding.GetEncoding("gb2312"); //定义编码 
        ASCIIEncoding encoding = new ASCIIEncoding();
        //构造web请求，发送请求，获取响应
        HttpWebRequest HttpWebRequest = null;
        HttpWebRequest = (HttpWebRequest)WebRequest.Create(url);


        if (proxy != "")
        {
            string[] proxyArray = proxy.Split(':');
            HttpWebRequest.Proxy = new WebProxy(proxyArray[0], int.Parse(proxyArray[1]));
            HttpWebRequest.Timeout = 40000;
        }

        //发送
        WebResponse HttpWebResponse = null;
        HttpWebResponse = HttpWebRequest.GetResponse();
        //获得流
        sr = new StreamReader(HttpWebResponse.GetResponseStream(), code);
        strHtml = sr.ReadToEnd();

        string aaa = HttpWebResponse.Headers.Get("Set-Cookie").ToString();
        //保存进COOKIE
        string cookieStr = string.Empty;

        cookieStr += getKeyData(@"pprdig=([^;]*);", aaa);
        cookieStr += getKeyData(@"ppinf=([^;]*);", aaa);

        return cookieStr;
    }

    /// <summary>
    /// 百度会员登录
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="pass"></param>
    /// <returns></returns>
    private string baiduLogin(string uid, string pass)
    {
        Common.Cookie cookie = new Common.Cookie();
        string verify = cookie.getCookie("qqverify");

        string strHtml = string.Empty;

        //uid = HttpUtility.UrlEncode(uid);

        System.Net.ServicePointManager.Expect100Continue = false;

        StreamReader sr = null; //用来读取流
        StreamWriter sw = null; //用来写文件
        Encoding code = Encoding.GetEncoding("gb2312"); //定义编码 
        ASCIIEncoding encoding = new ASCIIEncoding();
        //构造POST请求
        string postData = "tpl_ok=&next_target=&tpl=mn&skip_ok=&aid=&need_pay=&need_coin=&pay_method=&u=.%2F&return_method=get&more_param=&return_type=&psp_tt=0&password=" + pass + "&safeflg=0&isphone=tpl&username=" + JsEncode(uid) + "&verifycode=&mem_pass=on";
        byte[] data = code.GetBytes(postData);
        //构造web请求，发送请求，获取响应
        HttpWebRequest HttpWebRequest = null;
        string url = "https://passport.baidu.com/?login";
        HttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        HttpWebRequest.Method = "POST";
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

        //Response.Write(strHtml);
        //Response.End();

        string aaa = HttpWebResponse.Headers.Get("Set-Cookie").ToString();
        //保存进COOKIE
        string cookieStr = string.Empty;

        cookieStr += getKeyData(@"BDUSS=([^;]*);", aaa);
        cookieStr += getKeyData(@"PTOKEN=([^;]*);", aaa);
        cookieStr += getKeyData(@"STOKEN=([^;]*);", aaa);
        cookieStr += getKeyData(@"USERID=([^;]*);", aaa);

        blogName = GetBlogNameBaidu(uid, cookieStr);

        return cookieStr;
    }


    private string GetBlogNameBaidu(string uid, string cookie)
    {
        //准备生成
        string strHtml = string.Empty;
        StreamReader sr = null; //用来读取流
        Encoding code = Encoding.GetEncoding("utf-8"); //定义编码 
        ASCIIEncoding encoding = new ASCIIEncoding();
        //构造web请求，发送请求，获取响应
        HttpWebRequest HttpWebRequest = null;
        HttpWebRequest = (HttpWebRequest)WebRequest.Create("http://hi.baidu.com/sys/checkuser/" + HttpUtility.UrlEncode(uid) + "/space");
        HttpWebRequest.Headers.Set("Cookie", cookie);
        //发送
        WebResponse HttpWebResponse = null;
        HttpWebResponse = HttpWebRequest.GetResponse();
        //获得流
        sr = new StreamReader(HttpWebResponse.GetResponseStream(), code);
        strHtml = sr.ReadToEnd();


        string blogNameBaidu = new Regex(@"url=""/([^/]*)/""\+status;", RegexOptions.IgnoreCase).Match(strHtml).Groups[1].ToString();

        return blogNameBaidu;
    }


    private void AddBlogCookieNets(string cookie)
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


    private void AddBlogCookieBaidu(string uid, string cookie)
    {
        //准备生成
        string strHtml = string.Empty;
        StreamReader sr = null; //用来读取流
        Encoding code = Encoding.GetEncoding("utf-8"); //定义编码 
        ASCIIEncoding encoding = new ASCIIEncoding();
        //构造web请求，发送请求，获取响应
        HttpWebRequest HttpWebRequest = null;
        HttpWebRequest = (HttpWebRequest)WebRequest.Create("http://hi.baidu.com/" + blogName + "/creat/blog/");
        HttpWebRequest.Headers.Set("Cookie", cookie);
        //发送
        WebResponse HttpWebResponse = null;
        HttpWebResponse = HttpWebRequest.GetResponse();
        //获得流
        sr = new StreamReader(HttpWebResponse.GetResponseStream(), code);
        strHtml = sr.ReadToEnd();

        vtoken = new Regex(@"<input type=""hidden"" name=""bdstoken"" value=""([^""]*)"">", RegexOptions.IgnoreCase).Match(strHtml).Groups[1].ToString();
    }


    private void AddBlogCookieSohu(string uid, string cookie)
    {
        //准备生成
        string strHtml = string.Empty;
        StreamReader sr = null; //用来读取流
        Encoding code = Encoding.GetEncoding("utf-8"); //定义编码 
        ASCIIEncoding encoding = new ASCIIEncoding();
        //构造web请求，发送请求，获取响应
        HttpWebRequest HttpWebRequest = null;
        HttpWebRequest = (HttpWebRequest)WebRequest.Create("http://blog.sohu.com/manage/entry.do?m=add&t=shortcut");
        HttpWebRequest.Headers.Set("Cookie", cookie);
        //发送
        WebResponse HttpWebResponse = null;
        HttpWebResponse = HttpWebRequest.GetResponse();
        //获得流
        sr = new StreamReader(HttpWebResponse.GetResponseStream(), code);
        strHtml = sr.ReadToEnd();

        string aaa = HttpWebResponse.Headers.Get("Set-Cookie").ToString();
        cookie2 = getKeyData(@"ppmdig=([^;]*);", aaa);
        cookie2 += getKeyData(@"bloginfo=([^;]*);", aaa);
        cookie2 += getKeyData(@"ppnewsinfo=([^;]*);", aaa);
        cookie2 += getKeyData(@"pvc=([^;]*);", aaa);

        string[] blogNameArray = getKeyData(@"ppnewsinfo=([^;]*);", aaa).Split('|');
        if (blogNameArray.Length > 2)
        {
            blogName = blogNameArray[2];
        }
        //如果获取不到则取页面上的地址
        if (blogName == "")
        {
            blogName = new Regex(@"var _blog_base_url = '([^']*)';", RegexOptions.IgnoreCase).Match(strHtml).Groups[1].ToString();
        }

        vtoken = new Regex(@"<input type=""hidden"" name=""aid"" value=""([^""]*)"">", RegexOptions.IgnoreCase).Match(strHtml).Groups[1].ToString();
    }


    public static string MD5(string str)
    {
        return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
    }
    #endregion

    private string JsEncode(string str)
    {
        string newStr = string.Empty;
        for (int i = 0; i < str.Length; i++)
        {
            newStr += VBEncode(str.Substring(i, 1));
        }
        return newStr;
    }

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

    private void AddBlogCookie()
    {
        //Common.Cookie cookie = new Common.Cookie();
        //string sinaCookie = cookie.getCookie("sinaCookie");
        string trueSinaCookie = cookie1; // sinaCookie.Replace("|", ";");

        //准备生成
        string strHtml = string.Empty;
        StreamReader sr = null; //用来读取流
        Encoding code = Encoding.GetEncoding("utf-8"); //定义编码 
        ASCIIEncoding encoding = new ASCIIEncoding();
        //构造web请求，发送请求，获取响应
        HttpWebRequest HttpWebRequest = null;
        HttpWebRequest = (HttpWebRequest)WebRequest.Create("http://control.blog.sina.com.cn/admin/article/article_add.php?index");
        HttpWebRequest.Headers.Set("Cookie", trueSinaCookie);
        //发送
        WebResponse HttpWebResponse = null;
        HttpWebResponse = HttpWebRequest.GetResponse();
        //获得流
        sr = new StreamReader(HttpWebResponse.GetResponseStream(), code);
        strHtml = sr.ReadToEnd();

        vtoken = new Regex(@"<input type=""hidden"" name=""vtoken"" value=""([^""]*)""/>", RegexOptions.IgnoreCase).Match(strHtml).Groups[1].ToString();
        //获得流
        //string aaa = HttpWebResponse.Headers.Get("Set-Cookie").ToString();
        //string cookieStr = string.Empty;
        //cookieStr += getKeyData(@"SessionID=([^;]*);", aaa);
        //cookieStr += getKeyData(@"SINABLOGNUINFO=([^;]*);", aaa);
        //cookieStr += getKeyData(@"IDC_LOGIN=([^;]*);", aaa);

        //cookie2 = cookieStr;
        ////保存进COOKIE
        //cookie.setCookie("sinaCookieAdd", cookieStr.Replace(";", "|"), 999999);
    }

    private string SendBlog(string uid, string typ)
    {
        string title = this.tbTitle.Text;
        string content = HttpUtility.HtmlDecode(this.FCKeditor1.Text);
        string date = DateTime.Now.ToString("yyyy-MM-dd");
        string time = DateTime.Now.ToString("HH:mm:ss");

        //替换关键字连接
        content = changeKey(content);

        //替换新浪博客图片
        contentNewPic = changePicUrl(content);

        //拼装COOKIE
        string trueSinaCookie = cookie1;// +cookie2;// +cookie.getCookie("sinaCookieAdd").Replace("|", ";");

        RecordData(contentNewPic, uid, typ);

        return "";
    }

    /// <summary>
    /// 替换文章中的新浪本地图片
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    private string changePicUrl(string content)
    {
        Regex reg = new Regex(@"src=""(http\:\/\/s[^""]*)""", RegexOptions.IgnoreCase);
        content = reg.Replace(content, "src=\"http://www.7fshop.com/top/blog/sinaverify.aspx?u=$1\"");

        reg = new Regex(@"src=""(http\:\/\/img[^""]*)""", RegexOptions.IgnoreCase);
        content = reg.Replace(content, "src=\"http://www.7fshop.com/top/blog/sinaverify.aspx?u=$1\"");

        return content;
    }

    private int getGTK(string str)
    {
        int hash = 5381;
        for (int i = 0, len = str.Length; i < len; ++i)
        {
            hash += (hash << 5) + Microsoft.VisualBasic.Strings.Asc(str.Substring(i, 1));
        }
        return hash & 0x7fffffff;
    }

    /// <summary>
    /// 数据库记录
    /// </summary>
    private void RecordData(string content, string uid, string typ)
    {
        //加密后导致过长无法插入数据库
        //content = HttpUtility.HtmlEncode(content);

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
                                "nick, " +
                                "status, " +
                                "uid, " +
                                "content, " +
                                "typ" +
                            " ) VALUES ( " +
                                " '" + this.tbTitle.Text.Replace("'", "''") + "', " +
                                " '" + taobaoNick + "', " +
                                " '1', " +
                                " '" + uid + "', " +
                                " '" + content.Replace("'", "''") + "', " +
                                " '" + typ + "' " +
                          ") ";

        utils.ExecuteNonQuery(sql);


        //记录该帐号发送文章数量
        sql = "UPDATE TopBlogAccountNew SET count = count + 1 WHERE uid = '" + uid + "'";
        utils.ExecuteNonQuery(sql);
    }

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
        //string sqlNew = "SELECT sid FROM TopTaobaoShop WHERE nick = '" + taobaoNick1 + "'";
        //DataTable dtNew = utils.ExecuteDataTable(sqlNew);
        //if (dtNew.Rows.Count != 0)
        //{
        //    nickid = "http://shop" + dtNew.Rows[0]["sid"].ToString() + ".taobao.com/";
        //}
        //else
        //{
        //    nickid = "http://www.taobao.com/";
        //}

        //string[] arr = key.Split(' ');

        //for (int i = 0; i < arr.Length; i++)
        //{
        //    content = content.Replace(arr[i], "<a href='" + nickid + "' target='_blank'>" + arr[i] + "</a>");
        //}

        //替换掉博客文章里面的关键字
        string sql = "SELECT * FROM TopBlogLink WHERE nick = '" + taobaoNick1 + "'";
        DataTable dt = utils.ExecuteDataTable(sql);

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            content = content.Replace(dt.Rows[i]["keyword"].ToString(), "<a href='" + dt.Rows[i]["link"].ToString() + "' target='_blank'>" + dt.Rows[i]["keyword"].ToString() + "</a>");
        }

        //内容底部增加淘宝商品链接
        //'content += BindDataProduct();

        /*if (ads != "")
        {
            //内容头部增加特特广告图片
            string sqlNew = "SELECT sid FROM TopTaobaoShop WHERE nick = '" + taobaoNick1 + "'";
            DataTable dtNew = utils.ExecuteDataTable(sqlNew);
            if (dtNew.Rows.Count != 0)
            {
                nickid = "http://shop" + dtNew.Rows[0]["sid"].ToString() + ".taobao.com/";
            }
            else
            {
                nickid = "http://www.taobao.com/";
            }
            string nickidEncode = "http://www.7fshop.com/click/?s=" + EncodeStr(new string[] { ads, "0", nickid });
            content = "<a href='" + nickid + "' target='_blank'><img src='http://www.7fshop.com/show/html2jpg.aspx?id=" + ads + "' border=0 /></a><br>" + content;
        }*/

        return content;
    }

    private string EncodeStr(string[] parmArray)
    {
        string newStr = string.Empty;
        for (int i = 0; i < parmArray.Length; i++)
        {
            if (i == 0)
            {
                newStr = parmArray[i];
            }
            else
            {
                newStr += "|" + parmArray[i];
            }
        }

        Rijndael_ encode = new Rijndael_("tetesoftstr");
        newStr = encode.Encrypt(newStr);
        newStr = HttpUtility.UrlEncode(newStr);
        return newStr;
    }


    private string BindDataProduct()
    {
        string str = string.Empty;
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        string session = cookie.getCookie("top_sessionblog");

        //COOKIE过期判断
        if (taobaoNick == "")
        {
            //SESSION超期 跳转到登录页
            Response.Write("<script>parent.location.href='http://container.open.taobao.com/container?appkey=12159997'</script>");
            Response.End();
        }

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        //获取用户店铺商品列表
        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", "12159997", "614e40bfdb96e9063031d1a9e56fbed5");
        ItemsOnsaleGetRequest request = new ItemsOnsaleGetRequest();
        request.Fields = "num_iid,title,price,pic_url";
        request.Q = key;
        request.PageSize = 5;
        request.OrderBy = "volume:desc";

        PageList<Item> product = new PageList<Item>();

        try
        {
            product = client.ItemsOnsaleGet(request, session);
        }
        catch (Exception e)
        {
            if (e.Message == "27:Invalid session:Session not exist")
            {
                //SESSION超期 跳转到登录页
                Response.Write("<script>parent.location.href='http://container.open.taobao.com/container?appkey=12159997'</script>");
                Response.End();
            }
            return "";
        }

        for (int i = 0; i < product.Content.Count; i++)
        {
            str += "<a href=\"http://item.taobao.com/item.htm?id=" + product.Content[i].NumIid.ToString() + "\" title=\"" + product.Content[i].Title + "\" target=\"_blank\"><img src=\"" + product.Content[i].PicUrl + "\" border=\"0\" /></a><br />";
            str += "<a href=\"http://item.taobao.com/item.htm?id=" + product.Content[i].NumIid.ToString() + "\" title=\"" + product.Content[i].Title + "\" target=\"_blank\">" + product.Content[i].Title + "</a> 售价：" + product.Content[i].Price + "元<br><br>";
        }

        return str;
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