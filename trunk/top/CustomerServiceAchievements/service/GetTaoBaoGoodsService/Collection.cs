using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Model;
using CusServiceAchievements.DAL;
using System.Text.RegularExpressions;

namespace GetTaoBaoGoodsService
{
    public class Collection
    {
        public void GetGoodsCollection()
        {
            DateTime now = DateTime.Now;
            string fetchDate = now.ToString("yyyyMMdd");

            IList<TopNickSessionInfo> list = new NickSessionService().GetAllNickSession(Enum.TopTaoBaoService.Temporary);
            GoodsService goodsDal = new GoodsService();

            GoodsCollectionService goodscollecDal = new GoodsCollectionService();
            ShopCollectionService shopcollecDal = new ShopCollectionService();

            List<ShopCollectionInfo> shopcollecList = shopcollecDal.GetShopCollectionList(fetchDate);
            List<GoodsCollectionInfo> goodscollecList = goodscollecDal.GetGoodsCollectionList(fetchDate);

            //Regex regex = null;

            foreach (TopNickSessionInfo info in list)
            {
                string shopUrl = "http://count.tbcdn.cn/counter3?keys=SCCP_2_" + info.ShopId + "&callback=TShop.setShopStat";

                string s = GetWebSiteContent(shopUrl, "get", "", "gbk");
                if (s.Contains(":"))
                {
                    string shopcollec = s.Substring(s.IndexOf(":") + 1, s.IndexOf("}") - s.IndexOf(":") - 1);
                    IList<ShopCollectionInfo> myshop = shopcollecList.Where(o => o.ShopId == info.ShopId).ToList();

                    ShopCollectionInfo shopcinfo = new ShopCollectionInfo();
                    shopcinfo.ShopId = info.ShopId;
                    shopcinfo.ShopDate = fetchDate;
                    shopcinfo.CollectionCount = int.Parse(shopcollec);
                    if (myshop.Count > 0)
                    {
                        shopcollecDal.UpdateCollection(shopcinfo);
                    }
                    else
                    {
                        shopcollecDal.InsertShopCollectionInfo(shopcinfo);
                    }
                }

                //LogHelper.ServiceLog.RecodeLog(info.Nick + shopcollec);

                IList<string> goodsIds = goodsDal.GetGoodsIds(info.Nick);
                foreach (string gid in goodsIds)
                {
                    //string s = GetWebSiteContent("http://item.taobao.com/item.htm?id=" + gid, "get", "", "gbk");
                    //"apiItemViews": "http://count.taobao.com/counter2?keys=ICVT_7_10011714578&inc=ICVT_7_10011714578&callback=page_viewcount&sign=4084248dfb302ce856d227475a79a5b39c653",

                    //regex = new Regex(@"""apiItemViews"": ""([^""]*)"",", RegexOptions.IgnoreCase);
                    string goodsUrl = "http://count.tbcdn.cn/counter3?keys=ICCP_1_" + gid + "&callback=TShop.mods.SKU.Stat.setCollectCount";
                    string gs = GetWebSiteContent(goodsUrl, "get", "", "gbk");
                    if (gs.Contains(":"))
                    {
                        string goodspcollec = gs.Substring(gs.IndexOf(":") + 1, gs.IndexOf("}") - gs.IndexOf(":") - 1);
                        IList<GoodsCollectionInfo> mygoods = goodscollecList.Where(o => o.GoodsId == gid).ToList();

                        GoodsCollectionInfo goodscinfo = new GoodsCollectionInfo();
                        goodscinfo.GoodsId = gid;
                        goodscinfo.CollectionDate = fetchDate;
                        goodscinfo.Collection = int.Parse(goodspcollec);

                        if (mygoods.Count > 0)
                        {
                            goodscollecDal.UpdateCollection(goodscinfo);
                        }
                        else
                        {
                            goodscollecDal.InsertGoodsCollectionInfo(goodscinfo);
                        }
                    }

                    //LogHelper.ServiceLog.RecodeLog(gid + goodspcollec);
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
                //throw ex;
                LogHelper.ServiceLog.RecodeLog(url + ex.Message);
            }

            return strReturn;
        }
    }
}
