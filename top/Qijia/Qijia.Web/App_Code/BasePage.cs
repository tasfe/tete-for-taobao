
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System;

/// <summary>
/// 控制合法查看
/// </summary>
public class BasePage : System.Web.UI.Page
{
    protected override void InitializeCulture()
    {
        string msg = "尊敬的用户您好，非常抱歉的告诉您，您尚未订购该功能，如需继续使用请购买该服务:<br><br>58元/月  <a href=' http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-8:1;' target='_blank'>立即购买</a><br><br>158元/季  <a href=' http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-8:3;' target='_blank'>立即购买</a><br><br>298元/半年 <a href=' http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-8:6;' target='_blank'>立即购买</a><br><br>568元/年 <a href=' http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-8:12;' target='_blank'>立即购买</a><br>";
        if (Request.Cookies["istongji"] == null || Request.Cookies["istongji"].Value != "1")
        { 
            //Response.Redirect("buy.aspx?msg=" + msg); 
        }
        else
        {
            if (Request.Cookies["nick"] == null || string.IsNullOrEmpty(Request.Cookies["nick"].Value))
                Response.Redirect("http://fuwu.taobao.com/serv/my_service.htm");
            else
            {
                //if (!CheckNick(HttpUtility.UrlDecode(Request.Cookies["nick"].Value)))
                //    Response.Redirect("buy.aspx?msg=" + msg);
            }
        }
    }

    private static string GetTableName(string nick)
    {
        return "TopVisitInfo_" + Encrypt(nick);
    }

    /// <summary>
    /// MD5加密
    /// </summary>
    public static string Encrypt(string value)
    {
        return Encrypt(value, "00000000000000000000000000000000");
    }

    /// <summary>
    /// MD5加密
    /// </summary>
    public static string Encrypt(string value, string defaultValue)
    {
        if (value == null)
        {
            return defaultValue;
        }
        var md5 = FormsAuthentication.HashPasswordForStoringInConfigFile(value, "MD5");
        return md5 != null ? md5.ToLower() : defaultValue;
    }

    ///<summary>
    /// 生成MD5摘要加密，可以对加密结果进行截取
    ///</summary>
    ///<param name="value">源字符串</param>
    ///<param name="start">截取开始位置</param>
    ///<param name="count">截取长度</param>
    public static string EncryptSubstring(string value, int start, int length)
    {
        return Encrypt(value).Substring(start, length);
    }

    public static DateTime[] GetDateTime(DateTime start, int days)
    {
        DateTime end = start.AddDays(days);
        DateTime rstart = new DateTime(start.Year, start.Month, start.Day);
        DateTime rend = new DateTime(end.Year, end.Month, end.Day);
        return new[] { rstart, rend };
    }
}
