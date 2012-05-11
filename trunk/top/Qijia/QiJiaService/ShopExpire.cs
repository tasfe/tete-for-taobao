using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Qijia.Model;

namespace QiJiaService
{
    public class ShopExpire
    {
        Qijia.DAL.Jia_ShopService shopDal = new Qijia.DAL.Jia_ShopService();

        Qijia.PCI.PasswordParam pp = new Qijia.PCI.PasswordParam();

        readonly string qijiaURL = System.Configuration.ConfigurationManager.AppSettings["url"];

        public void CheckExpire()
        {
            IList<Jia_Shop> list = shopDal.GetAllJia_Shop();

            for (int i = 0; i < list.Count; i++)
            {
                int temptype = 0;
                if (list[i].ExpireDate >= DateTime.Now)
                {
                    temptype = 1;
                    list[i].IsExpired = 0;
                }
                else
                {
                    list[i].IsExpired = 1;
                    //更新店铺过期
                    shopDal.ModifyJia_Shop(list[i]);
                }

                string data = "uid=" + list[i].ShopId + "&tempType=" + temptype;
                string sign = pp.Encrypt3DES(data);
                data = pp.Encrypt3DES(data + "&sign=" + sign);
                data = data.Replace("+", "[jia]");
                string s = GetWebSiteContent(qijiaURL, "post", "data=" + data, "utf-8");
                //失败
                if (!s.Contains("success"))
                {
                    LogHelper.ServiceLog.RecodeLog("更新店铺" + list[i].ShopId + "模版" + temptype + "失败");
                }
            }
        }

        /// <summary>
        ///  发送web请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="requestMethod"></param>
        /// <param name="requestBody"></param>
        /// <param name="encode">网页采用的编码方式</param>
        /// <returns></returns>
        private static string GetWebSiteContent(string url, string requestMethod, string requestBody, string encode)
        {
            string strReturn = "";

            WebRequest wRequestUTF =
                WebRequest.Create(url +
                                  (requestMethod.ToUpper() == "GET" && !string.IsNullOrEmpty(requestBody)
                                       ? "?" + requestBody
                                       : ""));
            wRequestUTF.Credentials = CredentialCache.DefaultCredentials;
            wRequestUTF.Timeout = 10000 * 60; //10秒改为5秒超时
            wRequestUTF.Method = requestMethod.ToUpper();
            //wRequestUTF.ContentType = "application/X-www-form-urlencoded;charset=utf-8";

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
                //throw ex;
                LogHelper.ServiceLog.RecodeLog(url + ex.Message);
            }

            return strReturn;
        }
    }
}
