using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Net;
using System.Text;
using System.IO;

/// <summary>
///发送到iphone端
/// </summary>
public class PostIphone
{
    /// <summary>
    ///  发送web请求
    /// </summary>
    /// <param name="url"></param>
    /// <param name="requestMethod"></param>
    /// <param name="requestBody"></param>
    /// <param name="encode">网页采用的编码方式</param>
    /// <returns></returns>
    public static string GetWebSiteContent(string url, string requestMethod, string requestBody, string encode, CookieContainer cc)
    {
        string strReturn = "";

        HttpWebRequest wRequestUTF =
            (HttpWebRequest)WebRequest.Create(url +
                              (requestMethod.ToUpper() == "GET" && !string.IsNullOrEmpty(requestBody)
                                   ? "?" + requestBody
                                   : ""));

        wRequestUTF.Credentials = CredentialCache.DefaultCredentials;
        wRequestUTF.Timeout = 10000; //10秒改为5秒超时
        wRequestUTF.Method = requestMethod.ToUpper();
        //wRequestUTF.ContentType = "application/X-www-form-urlencoded;charset=utf-8";
        if (cc != null)
        {
            wRequestUTF.CookieContainer = cc;
        }
        wRequestUTF.ContentType = "application/X-www-form-urlencoded;charset=" + encode;
        wRequestUTF.Headers.Set("Pragma", "no-cache");
        //wRequestUTF.Headers.Set("Referer", " http://www.aidai.com/Frames.html");
        if (wRequestUTF.Method == "POST")
        {
            if (requestBody != null)
            {
                byte[] bs = Encoding.UTF8.GetBytes(requestBody);

                wRequestUTF.ContentLength = bs.Length;

                using (Stream reqStream = wRequestUTF.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                }
            }
        }

        try
        {
            WebResponse wResponseUTF = wRequestUTF.GetResponse();
            Stream streamUTF = wResponseUTF.GetResponseStream();
            //StreamReader sReaderUTF = new StreamReader(streamUTF, Encoding.UTF8);
            StreamReader sReaderUTF = new StreamReader(streamUTF, Encoding.GetEncoding(encode));
            strReturn = sReaderUTF.ReadToEnd();
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(url + ex.Message);
        }

        return strReturn;
    }

    public static Guid InsertAds(string uid, int area, int typ, string imgurl, string linkurl, decimal price, string name)
    {
        string guid = GetWebSiteContent("http://iphone.tetesoft.com/tuiguangapi.aspx?act=add", "post", "uid=" + uid + "&area=" + area + "&typ=" + typ + "&imgurl=" + imgurl + "&linkurl=" + linkurl + "&price=" + price + "&name=" + name, "utf-8", null);

        try
        {
            Guid gid = new Guid(guid);

            return gid;
        }
        catch (Exception ex)
        {
            
        }
        return Guid.Empty;
    }

    public static void DeleteAds(Guid gid)
    {
        GetWebSiteContent("http://iphone.tetesoft.com/tuiguangapi.aspx?act=del", "post", "guid=" + gid, "utf-8", null);
    }
}
