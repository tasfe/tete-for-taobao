using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;

namespace TeteIosTrain
{
    public class utils
    {
        public static string CommonPost(string url, IDictionary<string, string> param, string cookieStr)
        {
            try
            {
                CookieContainer cc = new CookieContainer();
                string result = string.Empty;
                #region ---- 完成 HTTP POST 请求----
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                req.KeepAlive = true;
                req.Timeout = 300000;
                req.Accept = "image/gif, image/jpeg, image/pjpeg, image/pjpeg, application/x-shockwave-flash, application/xaml+xml, application/x-ms-xbap, application/x-ms-application, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/QVOD, application/QVOD, application/vnd.ms-xpsdocument, */*";
                req.Referer = "http://dynamic.12306.cn/otsweb/loginAction.do?method=init";
                req.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; QQDownload 702; .NET4.0C; .NET4.0E; .NET CLR 2.0.50727; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";
                req.ContentType = "application/x-www-form-urlencoded";


                string[] ary = cookieStr.Split('|');
                //设置COOKIE
                Cookie ck = new Cookie("JSESSIONID", ary[0], "/otsweb", "dynamic.12306.cn");
                cc.Add(ck);

                ck = new Cookie("BIGipServerotsweb", ary[1], "/", "dynamic.12306.cn");
                cc.Add(ck);
                req.CookieContainer = cc;
                
                byte[] postData = Encoding.UTF8.GetBytes(PostData(param));
                Stream reqStream = req.GetRequestStream();
                reqStream.Write(postData, 0, postData.Length);
                reqStream.Close();
                
                HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
                Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
                Stream stream = null;
                StreamReader reader = null;
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);
                result = reader.ReadToEnd();
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
                #endregion
                return Regex.Replace(result, @"[\x00-\x08\x0b-\x0c\x0e-\x1f]", "");
            }
            catch (Exception e)
            {
                return e.StackTrace;
            }
        }


        public static string CommonGet(string url, string cookieStr)
        {
            try
            {
                CookieContainer cc = new CookieContainer();
                string result = string.Empty;
                #region ---- 完成 HTTP POST 请求----
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                req.KeepAlive = true;
                req.Timeout = 300000;
                req.Accept = "image/gif, image/jpeg, image/pjpeg, image/pjpeg, application/x-shockwave-flash, application/xaml+xml, application/x-ms-xbap, application/x-ms-application, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/QVOD, application/QVOD, application/vnd.ms-xpsdocument, */*";

                if (url.IndexOf("querySingleAction") != -1)
                {
                    req.Referer = "http://dynamic.12306.cn/otsweb/order/querySingleAction.do?method=init";
                }
                else
                {
                    req.Referer = "http://dynamic.12306.cn/otsweb/loginAction.do?method=init";
                }

                req.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; QQDownload 702; .NET4.0C; .NET4.0E; .NET CLR 2.0.50727; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";
                req.ContentType = "application/x-www-form-urlencoded";

                string[] ary = cookieStr.Split('|');
                //设置COOKIE
                Cookie ck = new Cookie("JSESSIONID", ary[0], "/otsweb", "dynamic.12306.cn");
                cc.Add(ck);

                ck = new Cookie("BIGipServerotsweb", ary[1], "/", "dynamic.12306.cn");
                cc.Add(ck);
                req.CookieContainer = cc;

                HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
                Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
                Stream stream = null;
                StreamReader reader = null;
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);
                result = reader.ReadToEnd();

                //获取服务器设置的COOKIE
                if (rsp.Headers.ToString().IndexOf("Set-Cookie") != -1)
                {
                    string cookieSetStr = rsp.Headers.Get("Set-Cookie").ToString();
                    Common.Cookie cookie = new Common.Cookie();
                }

                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
                #endregion
                return Regex.Replace(result, @"[\x00-\x08\x0b-\x0c\x0e-\x1f]", "");
            }
            catch (Exception e)
            {
                return e.StackTrace;
            }
        }

        public static string CommonGetFirst(string url)
        {
            string str = string.Empty;

            try
            {
                string result = string.Empty;
                #region ---- 完成 HTTP POST 请求----
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                req.KeepAlive = true;
                req.Timeout = 300000;
                req.Accept = "image/gif, image/jpeg, image/pjpeg, image/pjpeg, application/x-shockwave-flash, application/xaml+xml, application/x-ms-xbap, application/x-ms-application, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/QVOD, application/QVOD, application/vnd.ms-xpsdocument, */*";
                req.Referer = "http://dynamic.12306.cn/otsweb/loginAction.do?method=init";
                req.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; QQDownload 702; .NET4.0C; .NET4.0E; .NET CLR 2.0.50727; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";
                req.ContentType = "application/x-www-form-urlencoded";

                HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
                Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
                Stream stream = null;
                StreamReader reader = null;
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);
                result = reader.ReadToEnd();

                //获取服务器设置的COOKIE
                if (rsp.Headers.ToString().IndexOf("Set-Cookie") != -1)
                {
                    Common.Cookie cookie = new Common.Cookie();
                    string cookiestr = rsp.Headers.Get("Set-Cookie").ToString();
                    Regex reg = new Regex(@"JSESSIONID\=([^;]*)\;", RegexOptions.IgnoreCase);
                    string verify = string.Empty;
                    if (reg.IsMatch(cookiestr))
                    {
                        Match match = reg.Match(cookiestr);
                        verify = match.Groups[1].ToString();
                        //保存进COOKIE
                        //cookie.setCookie("session", verify, 999999);
                        str = verify;
                    }

                    Regex reg1 = new Regex(@"BIGipServerotsweb\=([^;]*)\;", RegexOptions.IgnoreCase);
                    string verify1 = string.Empty;
                    if (reg1.IsMatch(cookiestr))
                    {
                        Match match = reg1.Match(cookiestr);
                        verify1 = match.Groups[1].ToString();
                        //保存进COOKIE
                        //cookie.setCookie("session1", verify1, 999999);
                        str += "|" + verify1;
                    }
                }

                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
                #endregion
                return str;
            }
            catch (Exception e)
            {
                return e.StackTrace;
            }
        }

        /// <summary> 
        /// 组装普通文本请求参数。 
        /// </summary> 
        /// <param name="parameters">Key-Value形式请求参数字典</param> 
        /// <returns>URL编码后的请求数据</returns> 
        protected static string PostData(IDictionary<string, string> parameters)
        {
            StringBuilder postData = new StringBuilder();
            bool hasParam = false;
            IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                string value = dem.Current.Value;
                // 忽略参数名或参数值为空的参数 
                if (!string.IsNullOrEmpty(name))
                {
                    if (hasParam)
                    {
                        postData.Append("&");
                    }
                    postData.Append(name);
                    postData.Append("=");
                    postData.Append(Uri.EscapeDataString(value));
                    hasParam = true;
                }
            }
            return postData.ToString();
        }
    }
}
