using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;

namespace TeteTopApi
{
    public static class Message
    {
        /// <summary>
        /// 替换客户设置的短信内容
        /// </summary>
        /// <param name="giftcontent"></param>
        /// <param name="shopname"></param>
        /// <param name="buynick"></param>
        /// <param name="iscoupon"></param>
        /// <returns></returns>
        public static string GetMsg(string giftcontent, string shopname, string buynick, string iscoupon)
        {
            string giftstr = "";
            if (iscoupon == "1")
            {
                giftstr = "优惠券";
            }

            giftcontent = giftcontent.Replace("[shopname]", shopname);
            giftcontent = giftcontent.Replace("[buynick]", buynick);
            giftcontent = giftcontent.Replace("[gift]", giftstr);//.Replace("[buynick]", buynick);

            //强行截取
            if (giftcontent.Length > 66)
            {
                giftcontent = giftcontent.Substring(0, 66);
            }

            return giftcontent;
        }


        public static string Send(string phone, string msg)
        {
            string uid = "terrylv";
            string pass = "123456";

            msg = HttpUtility.UrlEncode(msg);

            string param = "username=" + uid + "&password=" + pass + "&method=sendsms&mobile=" + phone + "&msg=" + msg;
            byte[] bs = Encoding.ASCII.GetBytes(param);
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("http://sms3.eachwe.com/api.php");
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = bs.Length;

            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
            }

            using (HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse())
            {
                using (StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.GetEncoding("GB2312")))
                {
                    string content = reader.ReadToEnd();

                    if (content.IndexOf("<error>0</error>") == -1)
                    {
                        //发送失败
                        return "0";
                    }
                    else
                    {
                        //发送成功
                        Regex reg = new Regex(@"<sid>([^<]*)</sid>", RegexOptions.IgnoreCase);
                        MatchCollection match = reg.Matches(content);
                        string number = match[0].Groups[1].ToString();
                        return number;
                    }
                }
            }
        }
    }
}
