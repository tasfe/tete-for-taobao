using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Web;
using System.IO;
using System.Threading;

namespace TeteIosTrain
{
    public class Train
    {
        public Train()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback += (se, cert, chain, sslerror) => { return true; };
        }

        /// <summary>
        /// 登录验证请求
        /// </summary>
        /// <returns></returns>
        public string SendLoginRequest(string uid, string pass, string verify, string session)
        {
            string url = string.Empty;
            string result = string.Empty;
            string result1 = string.Empty;
            string flag = string.Empty;
            string loginRand = string.Empty;

            IDictionary<string, string> param = new Dictionary<string, string>();

            //获取验证字符
            url = "https://dynamic.12306.cn/otsweb/loginAction.do?method=loginAysnSuggest";
            result1 = utils.CommonGet(url, session);
            //解析
            Regex reg = new Regex(@"{""loginRand"":""([0-9]*)""", RegexOptions.IgnoreCase);
            if (reg.IsMatch(result1))
            {
                loginRand = reg.Match(result1).Groups[1].ToString();
            }
            else
            {
                loginRand = "000";
            }

            bool isok = false;

            param.Add("loginRand", loginRand);
            param.Add("refundLogin", "N");
            param.Add("refundFlag", "Y");
            param.Add("loginUser.user_name", uid);
            param.Add("nameErrorFocus", "");
            param.Add("user.password", pass);
            param.Add("passwordErrorFocus", "");
            param.Add("randCode", verify);
            param.Add("randErrorFocus", "");

            //while (!isok)
            //{
                //发送登录请求
                url = "https://dynamic.12306.cn/otsweb/loginAction.do?method=login";
                result = utils.CommonPost(url, param, session);

                //解析
                //reg = new Regex(@"style=""color: red;"">([^\<]*)</span>", RegexOptions.IgnoreCase);
                //if (reg.IsMatch(result))
                //{
                //    flag = reg.Match(result).Groups[1].ToString();
                //}
                //else
                //{
                //    flag = result1 + "!!!" + result + "other";
                //}

                //if (result.IndexOf("当前访问用户过多") == -1)
                //{
                //    isok = true;
                //}

                //Thread.Sleep(1000);
            //}

            return result;
        }

        /// <summary>
        /// 获取验证码和session
        /// </summary>
        public string GetVerifyImg()
        {
            string result = string.Empty;
            string resultNew = string.Empty;
            string url = string.Empty;
            CookieContainer cc = new CookieContainer();
            IDictionary<string, string> param = new Dictionary<string, string>();

            //获取服务器sessionid
            url = "https://dynamic.12306.cn/otsweb/loginAction.do?method=init";
            result = utils.CommonGetFirst(url);

            string[] ary = result.Split('|');
            //设置COOKIE
            Cookie ck = new Cookie("JSESSIONID", ary[0], "/otsweb", "dynamic.12306.cn");
            resultNew = "JSESSIONID=" + ary[0] + ";";

            cc.Add(ck);
            if (ary.Length > 1)
            {
                ck = new Cookie("BIGipServerotsweb", ary[1], "/", "dynamic.12306.cn");
                cc.Add(ck);
                resultNew += "BIGipServerotsweb=" + ary[1];
            }

            //请求验证码图片
            url = "https://dynamic.12306.cn/otsweb/passCodeAction.do?rand=sjrand";
            //构造web请求，发送请求，获取响应
            HttpWebRequest HttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebRequest.KeepAlive = true;
            HttpWebRequest.Timeout = 300000;
            HttpWebRequest.Accept = "image/gif, image/jpeg, image/pjpeg, image/pjpeg, application/x-shockwave-flash, application/xaml+xml, application/x-ms-xbap, application/x-ms-application, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/QVOD, application/QVOD, application/vnd.ms-xpsdocument, */*";
            HttpWebRequest.Referer = "https://dynamic.12306.cn/otsweb/loginAction.do?method=init";
            HttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; QQDownload 702; .NET4.0C; .NET4.0E; .NET CLR 2.0.50727; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";
            HttpWebRequest.ContentType = "application/x-www-form-urlencoded";
            HttpWebRequest.CookieContainer = cc;

            WebResponse HttpWebResponse = HttpWebRequest.GetResponse();
            byte[] arrayByte;

            Stream stream = HttpWebResponse.GetResponseStream();


            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ContentType = "image/gif";

            //读取长度
            int l = 1;
            arrayByte = new byte[1024];

            while (l != 0)
            {
                int i = stream.Read(arrayByte, 0, 1024);
                l = i;
                HttpContext.Current.Response.BinaryWrite(arrayByte);
            }

            stream.Close();
            HttpWebResponse.Close();

            return result;
        }

        public string SendSearchRequest(string date, string startcity, string endcity, string no, string rtyp, string ttype, string student, string timearea, string session)
        {
            string url = string.Empty;
            string result = string.Empty;
            string result1 = string.Empty;
            string flag = string.Empty;
            string loginRand = string.Empty;

            IDictionary<string, string> param = new Dictionary<string, string>();

            //查询请求
            url = "https://dynamic.12306.cn/otsweb/order/querySingleAction.do?method=queryLeftTicket&orderRequest.train_date=" + date + "&orderRequest.from_station_telecode=" + startcity + "&orderRequest.to_station_telecode=" + endcity + "&orderRequest.train_no=" + no + "&trainPassType=" + rtyp + "&trainClass=" + ttype + "&includeStudent=" + student + "&seatTypeAndNum=&orderRequest.start_time_str=" + timearea;
            
            param.Add("method", "queryLeftTicket");
            param.Add("orderRequest.train_date", date);
            param.Add("orderRequest.from_station_telecode", startcity);
            param.Add("orderRequest.to_station_telecode", endcity);
            param.Add("orderRequest.train_no", no);
            param.Add("trainPassType", rtyp);
            param.Add("trainClass", ttype);
            param.Add("includeStudent", student);
            param.Add("seatTypeAndNum", "");
            param.Add("orderRequest.start_time_str", timearea);

            //url = "https://dynamic.12306.cn/otsweb/order/querySingleAction.do";



            result = utils.CommonGet(url, session);

            return result;
        }

        /// <summary>
        /// 列车详情查询
        /// </summary>
        /// <param name="date"></param>
        /// <param name="startcity"></param>
        /// <param name="endcity"></param>
        /// <param name="no"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public string SendSearchDetailRequest(string date, string startcity, string endcity, string no, string session)
        {
            string url = string.Empty;
            string result = string.Empty;
            string result1 = string.Empty;
            string flag = string.Empty;
            string loginRand = string.Empty;

            IDictionary<string, string> param = new Dictionary<string, string>();

            //查询请求
            url = "https://dynamic.12306.cn/otsweb/order/querySingleAction.do?method=queryaTrainStopTimeByTrainNo&depart_date=" + date + "&from_station_telecode=" + startcity + "&to_station_telecode=" + endcity + "&train_no=" + no + "";

            result = utils.CommonGet(url, session);

            return result;
        }
    }
}
