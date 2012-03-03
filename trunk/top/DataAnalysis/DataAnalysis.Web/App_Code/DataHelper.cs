using System;
using System.Web;
using System.Runtime.InteropServices;
using System.Web.Security;
using System.Configuration;

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

    /// <summary>
    /// 指定从什么时间开始
    /// </summary>
    /// <param name="start">开始时间</param>
    /// <param name="days">间隔天数</param>
    /// <returns>返回含2个时间的数组，第一个开始，第二个结束</returns>
    public static DateTime[] GetDateTime(DateTime start, int days)
    {
        DateTime end = start.AddDays(days);
        DateTime rstart = new DateTime(start.Year, start.Month, start.Day);
        DateTime rend = new DateTime(end.Year, end.Month, end.Day);
        return new[] { rstart, rend };
    }

    public static string GetAppSetings(string key)
    {
        return ConfigurationSettings.AppSettings[key];
    }

    #region 获取局域网访问者的MAC

    [DllImport("Iphlpapi.dll")]
    private static extern int SendARP(Int32 dest, Int32 host, ref Int64 mac, ref Int32 length);
    [DllImport("Ws2_32.dll")]
    private static extern Int32 inet_addr(string ip);

    /// <summary>
    /// 应该可获取局域网访问者的MAC(未测)
    /// </summary>
    /// <returns></returns>
    public string GetClientMAC()
    {
        try
        {
            string userip = GetIPAddress();
            Int32 ldest = inet_addr(userip); //目的地的ip 
            Int32 lhost = inet_addr("223.4.6.115"); //本地服务器的ip 
            Int64 macinfo = new Int64();
            Int32 len = 6;
            int res = SendARP(ldest, 0, ref macinfo, ref len);
            string mac_src = macinfo.ToString("X");
            if (mac_src == "0")
            {
                if (userip == "127.0.0.1")
                    return "Localhost!";
                else
                    return "null";
            }
            while (mac_src.Length < 12)
            {
                mac_src = mac_src.Insert(0, "0");
            }
            string mac_dest = "";
            for (int i = 0; i < 11; i++)
            {
                if (0 == (i % 2))
                {
                    if (i == 10)
                    {
                        mac_dest = mac_dest.Insert(0, mac_src.Substring(i, 2));
                    }
                    else
                    {
                        mac_dest = "-" + mac_dest.Insert(0, mac_src.Substring(i, 2));
                    }
                }
            }
            return mac_dest;
        }
        catch (Exception err)
        {
            LogInfo.Add("获取mac", err.Message);
            return err.Message;
        }
    }

    #endregion
}
