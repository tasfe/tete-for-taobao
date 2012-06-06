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
using System.Security.Cryptography;

/// <summary>
/// Summary description for BasePage
/// </summary>
public class BasePage : System.Web.UI.Page
{
    protected override void InitializeCulture()
    {
        string msg = "尊敬的用户您好，非常抱歉的告诉您，您尚未订购该功能，如需继续使用请购买该服务:<br><br>9元/月  <a href=' http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-6:1;' target='_blank'>立即购买</a><br>";
        if (!string.IsNullOrEmpty(Request.QueryString["nick"]) && !string.IsNullOrEmpty(Request.QueryString["nicksession"]) && !string.IsNullOrEmpty(Request.QueryString["mobile"]))
        {
            HttpCookie cookie = new HttpCookie("nick", HttpUtility.UrlEncode(Request.QueryString["nick"]));
            cookie.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(cookie);
            HttpCookie cookieSe = new HttpCookie("nicksession", Request.QueryString["nicksession"]);
            cookieSe.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(cookieSe);

            HttpCookie cookieM = new HttpCookie("mobile", Request.QueryString["nicksession"]);
            cookieM.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(cookieM);

            TeteShopService tss = new TeteShopService();
            if (tss.GetShopInfo(Encrypt(Request.QueryString["nick"])) == null)
            {
                TeteShopInfo info = new TeteShopInfo();
                info.Nick = Encrypt(Request.QueryString["nick"]);
                info.Session = Request.QueryString["nicksession"];
                info.Short = Request.QueryString["nick"];
                info.Adddate = DateTime.Now;
                info.Appkey = "12132145";
                info.Appsecret = "1fdd2aadd5e2ac2909db2967cbb71e7f";
                tss.InsertShop(info);
                CacheCollection.RemoveCacheByKey(CacheCollection.KEY_ALLNICKSESSIONINFO);
            }
        }
        else
        {
            if (Request.Cookies["nick"] == null || Request.Cookies["nicksession"] == null || Request.Cookies["mobile"] == null)
                Response.Redirect("http://www.7fshop.com/top/market/buy.aspx?msg=" + msg);

        }
        //else
        //{
        //    Session["nick"] = Request.Cookies["nick"].Value;
        //    Session["session"] = Request.Cookies["nicksession"].Value;
        //}
    }

    public static string Encrypt(string strPwd)
    {
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] data = System.Text.Encoding.Default.GetBytes(strPwd);//将字符编码为一个字节序列 
        byte[] md5data = md5.ComputeHash(data);//计算data字节数组的哈希值 
        md5.Clear();
        string str = "";
        for (int i = 0; i < md5data.Length - 1; i++)
        {
            str += md5data[i].ToString("x").PadLeft(2, '0');
        }
        return str;
    } 
}
