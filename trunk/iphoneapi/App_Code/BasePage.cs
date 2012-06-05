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
        string msg = "尊敬的用户您好，非常抱歉的告诉您，您尚未订购该功能，如需继续使用请购买该服务:<br><br>58元/月  <a href=' http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-8:1;' target='_blank'>立即购买</a><br><br>158元/季  <a href=' http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-8:3;' target='_blank'>立即购买</a><br><br>298元/半年 <a href=' http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-8:6;' target='_blank'>立即购买</a><br><br>568元/年 <a href=' http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-8:12;' target='_blank'>立即购买</a><br>";
        if (!string.IsNullOrEmpty(Request.QueryString["nick"]) && !string.IsNullOrEmpty(Request.QueryString["nicksession"]))
        {
            HttpCookie cookie = new HttpCookie("nick", HttpUtility.UrlEncode(Request.QueryString["nick"]));
            cookie.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(cookie);
            HttpCookie cookieSe = new HttpCookie("nicksession", Request.QueryString["nicksession"]);
            cookieSe.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(cookieSe);

            TeteShopService tss = new TeteShopService();
            if (tss.GetShopInfo(Request.QueryString["nick"]) == null)
            {
                TeteShopInfo info = new TeteShopInfo();
                info.Nick = Encrypt(Request.QueryString["nick"]);
                info.Session = Request.QueryString["nicksession"];
                info.Short = Request.QueryString["nick"];
                info.Adddate = DateTime.Now;
                info.Appkey = "12132145";
                info.Appsecret = "1fdd2aadd5e2ac2909db2967cbb71e7f";
                tss.InsertShop(info);
            }
        }
        else
        {

            if (Request.Cookies["nick"] == null || Request.Cookies["nicksession"].Value == null)
                Response.Redirect("buy.aspx?msg=" + msg);
           
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
