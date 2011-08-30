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
using System.IO;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using Common;

public partial class topTest_market_qqverify : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string q = utils.NewRequest("q", utils.RequestType.QueryString);
        string url = "http://captcha.qq.com/getimage?aid=46000101&r=0.03370694697363402&vc_type=5dedbca8871cd5308d6ad6d641e3539c386932b3dfc8c5a1&uin=" + q;
        string strHtml = string.Empty;

        StreamReader sr = null; //用来读取流
        StreamWriter sw = null; //用来写文件
        Encoding code = Encoding.GetEncoding("utf-8"); //定义编码 
        ASCIIEncoding encoding = new ASCIIEncoding();
        //构造web请求，发送请求，获取响应
        WebRequest HttpWebRequest = null;
        HttpWebRequest = WebRequest.Create(url);

        WebResponse HttpWebResponse = null;
        HttpWebResponse = HttpWebRequest.GetResponse();
        //获得流
        //sr = new StreamReader(HttpWebResponse.GetResponseStream(), code);
        //strHtml = sr.ReadToEnd();
        byte[] arrayByte;

        Stream stream = HttpWebResponse.GetResponseStream();

        //获取服务器设置的COOKIE
        if (HttpWebResponse.Headers.ToString().IndexOf("Set-Cookie") != -1)
        {
            string cookiestr = HttpWebResponse.Headers.Get("Set-Cookie").ToString();
            Regex reg = new Regex(@"verifysession\=([^;]*)\;");
            string verify = string.Empty;
            if (reg.IsMatch(cookiestr))
            {
                Match match = reg.Match(cookiestr);
                verify = match.Groups[1].ToString();
                //保存进COOKIE
                Common.Cookie cookie = new Common.Cookie();
                cookie.setCookie("qqverify", verify, 999999);
            }
        }

        //保存图片
        int imgLong = (int)HttpWebResponse.ContentLength;
        arrayByte = new byte[imgLong];
        int l = 0;
        while (l < imgLong)
        {
            int i = stream.Read(arrayByte, 0, imgLong);
            l += i;
        }
        stream.Close();
        HttpWebResponse.Close();

        Response.ClearContent();
        Response.ContentType = "image/gif";
        Response.BinaryWrite(arrayByte);
    }
}
