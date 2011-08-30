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

public partial class top_blog_accountaddqq : System.Web.UI.Page
{
    public string proxy = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string path = Server.MapPath("/auto/ipnow.txt");
        if (File.Exists(path))
        {
            proxy = File.ReadAllText(path);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");

        //过期判断
        if (string.IsNullOrEmpty(taobaoNick))
        {
            Response.Write("<script>window.location.href='http://container.open.taobao.com/container?appkey=12159997'</script>");
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
            string version = dt.Rows[0]["versionNoBlog"].ToString();
            switch (version)
            {
                case "1":
                    level = "标准版客户";
                    num = "2";
                    break;
                case "2":
                    level = "专业版客户";
                    num = "10";
                    break;
                case "3":
                    level = "VIP版客户";
                    num = "999999";
                    break;
                default:
                    level = "标准版客户";
                    num = "2";
                    break;
            }
        }

        //获取今日发送的文章数
        sql = "SELECT COUNT(*) FROM TopBlogAccountNew WHERE nick = '" + taobaoNick + "'";
        dt = utils.ExecuteDataTable(sql);
        used = dt.Rows[0][0].ToString();

        if (int.Parse(used) >= int.Parse(num))
        {
            Response.Write("尊敬的" + level + " " + taobaoNick + "，非常抱歉的告诉您，您的最多只能添加" + num + "个博客帐号，如需继续添加请<a href='http://pay.taobao.com/mysub/subarticle/upgrade_order_sub_article.htm?market_type=6&article_id=181' target='_blank'>购买高级会员服务</a>，谢谢！<br> <a href='javascript:history.go(-1)'>返回</a> <a href='qubie.html' target='_blank'>查看版本区别</a>");
            Response.End();
            return;
        }

        string qqCookie = QQLogin();

        string uid = utils.NewRequest("tbUserName", utils.RequestType.Form);
        string pass = utils.NewRequest("tbPassword", utils.RequestType.Form);
        string truepass = utils.NewRequest("truePass", utils.RequestType.Form);

        //插入数据库
        sql = "INSERT INTO TopBlogAccountNew (" +
                        "uid, " +
                        "pass, " +
                        "nick, " +
                        "count, " +
                        "cookie, " +
                        "typ" +
                    " ) VALUES ( " +
                        " '" + uid + "', " +
                        " '" + truepass + "', " +
                        " '" + taobaoNick + "', " +
                        " '0', " +
                        " '" + qqCookie + "', " +
                        " 'qq' " +
                  ") ";

        utils.ExecuteNonQuery(sql);

        //Response.Write(sql);
        string isDialog = utils.NewRequest("isdialog", utils.RequestType.QueryString);
        if (isDialog == "1")
        {
            CloseWindow();
        }
        else
        {
            Response.Redirect("accountlist.aspx");
        }
    }


    private void CloseWindow()
    {
        string str = string.Empty;
        string sql = "SELECT TOP 1 * FROM TopBlogAccountNew ORDER BY id DESC";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str = "<input id=\"account" + dt.Rows[0]["id"].ToString() + "\" type=\"checkbox\" checked=\"checked\" name=\"account\" value=\"" + dt.Rows[0]["uid"].ToString() + "\" /> <label for=\"account" + dt.Rows[0]["id"].ToString() + "\">" + dt.Rows[0]["uid"].ToString() + "</label> - " + dt.Rows[0]["typ"].ToString() + " <br />";
        }

        StringBuilder builder = new StringBuilder();
        builder.Append("<script>");
        builder.Append("var str = '" + str + "';");
        builder.Append("if (navigator.appVersion.indexOf('MSIE') == -1) {");
        builder.Append("window.opener.returnAction(str);");
        builder.Append("window.close();");
        builder.Append("} else {");
        builder.Append("window.returnValue = str;");
        builder.Append("window.close();");
        builder.Append("}");
        builder.Append("</script>");

        Response.Write(builder.ToString());
        Response.End();
    }

    private string QQLogin()
    {
        Common.Cookie cookie = new Common.Cookie();
        string verify = cookie.getCookie("qqverify");

        string url = "http://ptlogin2.qq.com/login?u=" + Request.Form["tbUserName"] + "&p=" + Request.Form["tbPassword"].ToUpper() + "&verifycode=" + Request.Form["tbVerify"] + "&aid=46000101&u1=http%3A%2F%2Ft.qq.com&ptredirect=1&h=1&from_ui=1&dumy=&fp=loginerroralert";
        url = "http://ptlogin2.qq.com/login?u=" + Request.Form["tbUserName"] + "&p=" + Request.Form["tbPassword"].ToUpper() + "&verifycode=" + Request.Form["tbVerify"] + "&low_login_enable=1&low_login_hour=720&=on&aid=15000101&u1=http%3A%2F%2Fimgcache.qq.com%2Fqzone%2Fv5%2Floginsucc.html%3Fpara%3Dizone&ptredirect=1&h=1&from_ui=1&dumy=&fp=loginerroralert";

        //准备生成
        string strHtml = string.Empty;

        StreamReader sr = null; //用来读取流
        StreamWriter sw = null; //用来写文件
        Encoding code = Encoding.GetEncoding("utf-8"); //定义编码 
        ASCIIEncoding encoding = new ASCIIEncoding();
        //构造web请求，发送请求，获取响应
        WebRequest HttpWebRequest = null;
        HttpWebRequest = WebRequest.Create(url);
        HttpWebRequest.Headers.Set("Cookie", "verifysession=" + verify + ";");

        //if (proxy != "")
        //{
        //    string[] proxyArray = proxy.Split(':');
        //    HttpWebRequest.Proxy = new WebProxy(proxyArray[0], int.Parse(proxyArray[1]));
        //    HttpWebRequest.Timeout = 40000;
        //}

        //发送
        WebResponse HttpWebResponse = null;
        HttpWebResponse = HttpWebRequest.GetResponse();
        //获得流
        sr = new StreamReader(HttpWebResponse.GetResponseStream(), code);
        strHtml = sr.ReadToEnd();

        //如果验证错误
        /*if (strHtml.IndexOf("您输入的验证码有误") != -1)
        {
            Response.Write("<script>alert('您输入的验证码或帐号密码有误，请重新输入！');history.go(-1);</script>");
            Response.End();
            return "";
        }*/


        //Response.Write("<script>alert('QQ空间自动发布程序升级中，请稍后使用！');history.go(-1);</script>");
        //Response.End();
        //return "";
        string aaa = string.Empty;

        try
        {
            aaa = HttpWebResponse.Headers.Get("Set-Cookie").ToString();
        }
        catch
        {
            Response.Write(verify + "<br>");
            Response.Write(strHtml + "<br>");
            Response.Write(HttpWebResponse.Headers.ToString());
            Response.End();
            return "";
        }
        //保存进COOKIE
        string cookieStr = string.Empty;

        cookieStr += getKeyData(@"pt2gguin=([^;]*); ", aaa);
        cookieStr += getKeyData(@"ptcz=([^;]*); ", aaa);
        cookieStr += getKeyData(@"uin=([^;]*); ", aaa);
        cookieStr += getKeyData(@"skey=([^;]*); ", aaa);

        //保存登录状态
        Rijndael_ encode = new Rijndael_("tetesoft");
        cookie.setCookie("tmpQQ", encode.Encrypt(cookieStr), 999999);

        //cookieStr += "verifysession=" + verify + ";";
        cookieStr += "ptisp=ctc;";

        //请求说说首页获取COOKIE
        //url = "http://t.qq.com";
        //HttpWebRequest = WebRequest.Create(url);
        //HttpWebRequest.Headers.Set("Cookie", cookieStr);
        ////获得流
        //sr = new StreamReader(HttpWebResponse.GetResponseStream(), code);
        //strHtml = sr.ReadToEnd();
        ////发送
        //HttpWebResponse = null;
        //HttpWebResponse = HttpWebRequest.GetResponse();

        //string direct = HttpWebResponse.ResponseUri.ToString();

        //url = direct;
        //HttpWebRequest = WebRequest.Create(url);
        //HttpWebRequest.Headers.Set("Cookie", cookieStr);
        ////获得流
        //sr = new StreamReader(HttpWebResponse.GetResponseStream(), code);
        //strHtml = sr.ReadToEnd();
        ////发送
        //HttpWebResponse = null;
        //HttpWebResponse = HttpWebRequest.GetResponse();
        //string ccc = HttpWebResponse.Headers.ToString();

        //cookie.setCookie("tCookie", cookieStr.Replace(";", "|"), 999999);
        //cookie.setCookie("directUrl", direct, 999999);
        return cookieStr;
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

    /// <summary>
    /// 根据网页地址获取返回HTML
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    private string GetHtmlByUrlget(string url, string cookie)
    {
        //准备生成
        string strHtml = string.Empty;
        StreamReader sr = null; //用来读取流
        Encoding code = Encoding.GetEncoding("utf-8"); //定义编码 
        ASCIIEncoding encoding = new ASCIIEncoding();
        //构造web请求，发送请求，获取响应
        HttpWebRequest HttpWebRequest = null;
        HttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        HttpWebRequest.Headers.Set("Cookie", cookie);
        //发送
        WebResponse HttpWebResponse = null;
        HttpWebResponse = HttpWebRequest.GetResponse();
        //获得流
        sr = new StreamReader(HttpWebResponse.GetResponseStream(), code);
        strHtml = sr.ReadToEnd();

        return strHtml;
    }
}