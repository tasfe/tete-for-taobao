using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Model;
using CusServiceAchievements.DAL;

namespace GetTaoBaoGoodsService
{
    public class Collection
    {

        public void GetGoodsCollection()
        {
            
             IList<TopNickSessionInfo> list = new NickSessionService().GetAllNickSession(Enum.TopTaoBaoService.YingXiaoJueCe);
            GoodsService goodsDal = new GoodsService();

             foreach (TopNickSessionInfo info in list)
             {
                 IList<string> goodsIds = goodsDal.GetGoodsIds(info.Nick);
                 foreach (string gid in goodsIds)
                 {
                     string s = GetWebSiteContent("http://count.tbcdn.cn/counter3?keys=DFX_200_1_14050993029,ICVT_7_" + gid + ",ICCP_1_" + gid + ",SCCP_2_" + info.ShopId + ",ICE_3_feedcount-" + gid + ",ZAN_27_2_" + gid + "&inc=ICVT_7_" + gid + "&sign=733642a8ae2a9e398e96d638798f3281a8bdb&callback=TShop.mods.SKU.CounterCenter.saveCounts", "get", "", "gbk");
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
            string strReturn;

            WebRequest wRequestUTF =
                WebRequest.Create(url +
                                  (requestMethod.ToUpper() == "GET" && !string.IsNullOrEmpty(requestBody)
                                       ? "?" + requestBody
                                       : ""));
            wRequestUTF.Credentials = CredentialCache.DefaultCredentials;
            wRequestUTF.Timeout = 5000; //10秒改为5秒超时
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
                throw ex;
            }

            return strReturn;
        }
    }
}
