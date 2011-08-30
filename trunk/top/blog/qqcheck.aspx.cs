using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using System.Text;
using System.Net;
using Common;
using System.Text.RegularExpressions;

public partial class top_blog_qqcheck : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.AddHeader("P3P", "CP=CURa ADMa DEVa PSAo PSDo OUR BUS UNI PUR INT DEM STA PRE COM NAV OTC NOI DSP COR");
        string uid = utils.NewRequest("uid", utils.RequestType.QueryString);
        string url = "http://ptlogin2.qq.com/check?uin=" + uid + "&appid=15000101";
        string html = GetHtmlByUrlget(url, "");

        Response.Write(html);
        Response.End();
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

        //获取服务器设置的COOKIE
        if (HttpWebResponse.Headers.ToString().IndexOf("Set-Cookie") != -1)
        {
            string cookiestr = HttpWebResponse.Headers.Get("Set-Cookie").ToString();
            Regex reg = new Regex(@"ptvfsession\=([^;]*)\;");
            string verify = string.Empty;
            if (reg.IsMatch(cookiestr))
            {
                Match match = reg.Match(cookiestr);
                verify = match.Groups[1].ToString();
                //保存进COOKIE
                Common.Cookie cookie1 = new Common.Cookie();
                cookie1.setCookie("qqverify", verify, 999999);
            }
        }

        return strHtml;
    }
}
