using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;

public partial class top_blog_search : System.Web.UI.Page
{
    public string radio = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string key = tbKey.Text;
        string url = "http://uni.sina.com.cn/c.php?k=" + key + "&t=blog&ts=bpost&stype=title";

        string strHtml = getUrl(url, "gb2312");

        Regex reg = new Regex(@"<span class=""title01""><a target=""_blank"" href=""([^""]*)"">([\s\S]*?)</a></span>", RegexOptions.IgnoreCase);
        MatchCollection match = reg.Matches(strHtml);

        for (int i = 0; i < match.Count; i++)
        {
            if (i == 0)
            {
                radio += ("<input type=radio checked name=title value=\"" + match[i].Groups[1].ToString() + "\"> " + match[i].Groups[2].ToString() + "<br>");
            }
            else
            {
                radio += ("<input type=radio name=title value=\"" + match[i].Groups[1].ToString() + "\"> " + match[i].Groups[2].ToString() + "<br>");
            }
        }
    }

    protected void btnSearch1_Click(object sender, EventArgs e)
    {
        string url = Request.Form["title"];
        string strHtml = getUrl(url, "utf-8");
        //<div id="sina_keyword_ad_area2" class="articalContent">  <div class="shareUp">

        Regex reg = new Regex(@"<div id=""sina_keyword_ad_area2"" class=""articalContent"">([\s\S]*?)</div>[\s]*<!-- 正文结束 -->", RegexOptions.IgnoreCase);
        MatchCollection match = reg.Matches(strHtml);

        //替换博客里面的图片
        string str = match[0].Groups[1].ToString();
        Regex regImg = new Regex(@"<img src=""([^""]*)"" real_src =""([^""]*)""", RegexOptions.IgnoreCase);
        str = regImg.Replace(str, @"<img src=""$2""");

        //替换掉博客内容里面的连接
        Regex regLink = new Regex(@"<a[^>]*>([\s\S]*?)</a>", RegexOptions.IgnoreCase);
        str = regLink.Replace(str, "$1");


        //Response.Write(url + "<br>");
        //Response.Write(str);
    }

    /// <summary>
    /// 发送博文
    /// </summary>
    /// <param name="str"></param>
    /// <param name="str_2"></param>
    private void AddBlogCookie()
    {
        Common.Cookie cookie = new Common.Cookie();
        string sinaCookie = cookie.getCookie("sinaCookie");
        string trueSinaCookie = sinaCookie.Replace("|", ";");

        //准备生成
        string strHtml = string.Empty;
        StreamReader sr = null; //用来读取流
        Encoding code = Encoding.GetEncoding("utf-8"); //定义编码 
        ASCIIEncoding encoding = new ASCIIEncoding();
        //构造web请求，发送请求，获取响应
        HttpWebRequest HttpWebRequest = null;
        HttpWebRequest = (HttpWebRequest)WebRequest.Create("http://control.blog.sina.com.cn/admin/article/article_add.php?index");
        HttpWebRequest.Headers.Set("Cookie", trueSinaCookie);
        //HttpWebRequest.Method = "POST";
        // HttpWebRequest.ContentType = "application/x-www-form-urlencoded";
        //HttpWebRequest.ContentLength = data.Length;
        //Stream newStream = HttpWebRequest.GetRequestStream();
        //newStream.Write(data, 0, data.Length);
        //newStream.Close();
        //发送
        WebResponse HttpWebResponse = null;
        HttpWebResponse = HttpWebRequest.GetResponse();
        //获得流
        string aaa = HttpWebResponse.Headers.Get("Set-Cookie").ToString();
        string cookieStr = string.Empty;
        cookieStr += getKeyData(@"SessionID=([^;]*);", aaa);
        cookieStr += getKeyData(@"SINABLOGNUINFO=([^;]*);", aaa);
        cookieStr += getKeyData(@"expires=([^;]*);", aaa);
        cookieStr += getKeyData(@"IDC_LOGIN=([^;]*);", aaa);
        cookieStr += getKeyData(@"SINABLOGNUINFO=([^;]*);", aaa);

        //保存进COOKIE
        cookie.setCookie("sinaCookieAdd", cookieStr, 999999);
    }

    protected void btnSearch2_Click(object sender, EventArgs e)
    {
        //请求页面获取相关参数
        //AddBlogCookie();

        //拼装COOKIE
        Common.Cookie cookie = new Common.Cookie();
        string sinaCookie = cookie.getCookie("sinaCookie");
        string trueSinaCookie = sinaCookie.Replace("|", ";");// +cookie.getCookie("sinaCookieAdd").Replace("|", ";") + ";";

        //准备生成
        string strHtml = string.Empty;

        StreamReader sr = null; //用来读取流
        StreamWriter sw = null; //用来写文件
        Encoding code = Encoding.GetEncoding("utf-8"); //定义编码 
        ASCIIEncoding encoding = new ASCIIEncoding();
        //构造POST请求
        string postData = "ptype=&teams=&worldcuptags=&album=&album_cite=&blog_id=&is_album=0&stag=&sno=&book_worksid=&channel_id=&url=&channel=&newsid=&fromuid=&wid=&articletj=&vtoken=fd7d511d4da2fbb8732db6ee8d1ef86a&is_media=0&is_stock=0&is_tpl=0&assoc_article=&assoc_style=1&assoc_article_data=&article_BGM=&xRankStatus=&commentGlobalSwitch=&commenthideGlobalSwitch=&articleStatus_preview=1&source=&topic_id=0&topic_channel=0&topic_more=&utf8=1&date_pub=2010-11-18&blog_title=来测试一个&time=16%3A22%3A31&blog_body=fdsfdsafdsfdasfdsafdsafd&blog_class=00&tag=&x_cms_flag=0&sina_sort_id=117&join_circle=1";
        byte[] data = code.GetBytes(postData);
        //构造web请求，发送请求，获取响应
        HttpWebRequest HttpWebRequest = null;
        string url = "http://control.blog.sina.com.cn/admin/article/article_post.php ";
        HttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        HttpWebRequest.Method = "POST";
        HttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; InfoPath.2; .NET4.0C; .NET4.0E)";
        HttpWebRequest.ContentType = "application/x-www-form-urlencoded";
        HttpWebRequest.ContentLength = data.Length;
        HttpWebRequest.Referer = "http://control.blog.sina.com.cn/admin/article/article_add.php";
        //HttpWebRequest.Headers.Set("Cookie", "EditorToolType=base; UOR=www.baidu.com,blog,; SINAGLOBAL=116.225.243.88.135441289973113118; ULV=1290068456224:6:6:6:116.225.243.88.67501290065988660:1290067272974; vjuids=1abca039b.12c5865e028.0.9fd135f2d15328; vjlast=1289973260.1290066021.13; ALLYESID4=00101117135324204575428; FocusMediaIpCgiCookie=%u4E0A%u6D77%7C%7C%u4E0A%u6D77; FocusMediaRotatorInputCookie=12; FocusMediaRotatorCookie=14; Apache=116.225.243.88.67501290065988660; _s_upa=8; PHPSESSID=928ab7e0a9809f461df8907c6b16d207; SUE=es%3D791918fdc6ef7b2feb43f8106fdbb22c%26ev%3Dv0%26es2%3Da006e7bd85d7ca9e820b661097199b3e; SUP=cv%3D1%26bt%3D1290068513%26et%3D1290154913%26lt%3D1%26uid%3D1665537805%26user%3Dgolddonkey%2540126.com%26ag%3D4%26name%3Dgolddonkey%2540126.com%26nick%3Dgolddonkey%26sex%3D%26ps%3D0%26email%3D%26dob%3D%26ln%3Dgolddonkey%2540126.com; ucMemList_1665537805=; SessionID=a4a0b37f3dfd01d55a692d575157f1f7; SINABLOGNUINFO=1665537805.6346170d.tetesoft; SSCSum=3; SinaRot//=9; tblogt=0; BILS=c; CoupletMediahttp://blog.sina.com.cn/=0; rpb_1_1=1290068472146; iCast_Rotator_1_2=1290066012958; ad680=1290121200568;");
        //HttpWebRequest.Headers.Set("Cookie", " SUE=es%3D791918fdc6ef7b2feb43f8106fdbb22c%26ev%3Dv0%26es2%3Da006e7bd85d7ca9e820b661097199b3e; SUP=cv%3D1%26bt%3D1290068513%26et%3D1290154913%26lt%3D1%26uid%3D1665537805%26user%3Dgolddonkey%2540126.com%26ag%3D4%26name%3Dgolddonkey%2540126.com%26nick%3Dgolddonkey%26sex%3D%26ps%3D0%26email%3D%26dob%3D%26ln%3Dgolddonkey%2540126.com; ");
        //HttpWebRequest.Headers.Set("Cookie", "SUE=es%3D13f20d779254ea584d3779e08e09258b%26ev%3Dv0%26es2%3D949cd07b74e1e7b7d546532a5cad7f4b;SUP=cv%3D1%26bt%3D1290147276%26et%3D1290233676%26lt%3D1%26uid%3D1665537805%26user%3Dgolddonkey%2540126.com%26ag%3D4%26name%3Dgolddonkey%2540126.com%26nick%3Dgolddonkey%26sex%3D%26ps%3D0%26email%3D%26dob%3D%26ln%3Dgolddonkey%2540126.com; ");
        HttpWebRequest.Headers.Set("Cookie", trueSinaCookie);

        Stream newStream = HttpWebRequest.GetRequestStream();
        newStream.Write(data, 0, data.Length);
        newStream.Close();
        //发送
        WebResponse HttpWebResponse = null;
        HttpWebResponse = HttpWebRequest.GetResponse();
        //获得流
        sr = new StreamReader(HttpWebResponse.GetResponseStream(), code);
        strHtml = sr.ReadToEnd();

        Response.Write(trueSinaCookie + "<br>");
        Response.Write(strHtml);
    }

    protected void btnSearch3_Click(object sender, EventArgs e)
    {
        string uid = "golddonkey@126.com";
        string pass = "t57685768";

        //准备生成
        string strHtml = string.Empty;

        StreamReader sr = null; //用来读取流
        StreamWriter sw = null; //用来写文件
        Encoding code = Encoding.GetEncoding("utf-8"); //定义编码 
        ASCIIEncoding encoding = new ASCIIEncoding();
        //构造POST请求
        string postData = "service=sso&client=ssologin.js%28v1.3.9%29&entry=blog&encoding=utf-8&gateway=1&savestate=0&from=&useticket=0&username=" + uid + "&password="+pass+"&callback=parent.sinaSSOController.loginCallBack&returntype=IFRAME&setdomain=1";
        byte[] data = code.GetBytes(postData);
        //构造web请求，发送请求，获取响应
        HttpWebRequest HttpWebRequest = null;
        string url = "http://login.sina.com.cn/sso/login.php?client=ssologin.js(v1.3.9)";
        HttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        HttpWebRequest.Method = "POST";
        HttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; InfoPath.2; .NET4.0C; .NET4.0E)";
        HttpWebRequest.ContentType = "application/x-www-form-urlencoded";
        HttpWebRequest.ContentLength = data.Length;

        Stream newStream = HttpWebRequest.GetRequestStream();
        newStream.Write(data, 0, data.Length);
        newStream.Close();
        //发送
        WebResponse HttpWebResponse = null;
        HttpWebResponse = HttpWebRequest.GetResponse();
        string aaa = HttpWebResponse.Headers.Get("Set-Cookie").ToString();
        //获得流
        sr = new StreamReader(HttpWebResponse.GetResponseStream(), code);
        strHtml = sr.ReadToEnd();

        string cookieStr = string.Empty;
        //cookieStr += getKeyData(@"tgc=([^;]*);", aaa);
        cookieStr += getKeyData(@"SUE=([^;]*);", aaa);
        cookieStr += getKeyData(@"SUP=([^;]*);", aaa);
       // cookieStr += getKeyData(@"domain=([^;]*);", aaa);

        //保存进COOKIE
        Common.Cookie cookie = new Common.Cookie();
        cookie.setCookie("sinaCookie", cookieStr.Replace(";", "|"), 999999);

        Response.Write(cookieStr);
    }

    private string getUrl(string url, string codeStr)
    {
        //准备生成
        string strHtml = string.Empty;
        StreamReader sr = null; //用来读取流
        Encoding code = Encoding.GetEncoding(codeStr); //定义编码 
        ASCIIEncoding encoding = new ASCIIEncoding();
        //构造web请求，发送请求，获取响应
        WebRequest HttpWebRequest = null;
        HttpWebRequest = WebRequest.Create(url);
        //HttpWebRequest.Method = "POST";
        // HttpWebRequest.ContentType = "application/x-www-form-urlencoded";
        //HttpWebRequest.ContentLength = data.Length;
        //Stream newStream = HttpWebRequest.GetRequestStream();
        //newStream.Write(data, 0, data.Length);
        //newStream.Close();
        //发送
        WebResponse HttpWebResponse = null;
        HttpWebResponse = HttpWebRequest.GetResponse();
        //获得流
        sr = new StreamReader(HttpWebResponse.GetResponseStream(), code);
        strHtml = sr.ReadToEnd();

        return strHtml;
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