using System;
using System.Web;

/// <summary>
/// 获取来访客户端信息
/// </summary>
public class DataHelper
{
    private HttpContext context;

    public DataHelper(HttpContext myContext)
    {
        if (context == null)
        {
            context = myContext;
        }
    }

    public string GetUrl()
    {
        if (null == context.Request.UrlReferrer)
            return "";
        return context.Request.UrlReferrer.ToString();
    }

    public string GetIPAddress()
    {
        return context.Request.ServerVariables["REMOTE_ADDR"];
    }

    public DateTime GetVisitTime()
    {
        return DateTime.Now;
    }

    public string GetUserAgent()
    {
        return GetOSNameByUserAgent(context.Request.UserAgent);
    }

    //获取浏览器的类型及版本号
    public string GetBrower()
    {
        return context.Request.Browser.Browser + context.Request.Browser.Version;
    }

    //获取用户操作系统的语言
    public string GetOSLanguage()
    {
        return context.Request.UserLanguages[0];
    }

    //获取OS：
    private string GetOSNameByUserAgent(string userAgent)
    {
        string osVersion = "未知";
        if (userAgent.Contains("NT 6.1"))
        {
            osVersion = "Windows  7";
        }
        else if (userAgent.Contains("NT 6.0"))
        {
            osVersion = "Windows Vista/Server 2008";
        }
        else if (userAgent.Contains("NT 5.2"))
        {
            osVersion = "Windows Server 2003";
        }
        else if (userAgent.Contains("NT 5.1"))
        {
            osVersion = "Windows XP";
        }
        else if (userAgent.Contains("NT 5"))
        {
            osVersion = "Windows 2000";
        }
        else if (userAgent.Contains("Mac"))
        {
            osVersion = "Mac";
        }
        else if (userAgent.Contains("Unix"))
        {
            osVersion = "UNIX";
        }
        else if (userAgent.Contains("Linux"))
        {
            osVersion = "Linux";
        }
        else if (userAgent.Contains("SunOS"))
        {
            osVersion = "SunOS";
        }

        return osVersion;
    }
}
