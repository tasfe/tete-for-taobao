using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using TeteTopApi.Entity;
using TeteTopApi.DataContract;

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


        /// <summary>
        /// 替换客户设置的短信内容
        /// </summary>
        /// <param name="giftcontent"></param>
        /// <param name="shopname"></param>
        /// <param name="buynick"></param>
        /// <param name="iscoupon"></param>
        /// <returns></returns>
        public static string GetMsg(string giftcontent, string shopname, string buynick, string iscoupon, string freecard)
        {
            string giftstr = "";
            if (iscoupon == "1")
            {
                giftstr = "优惠券";
            }

            giftcontent = giftcontent.Replace("[freecard]", freecard);
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


        /// <summary>
        /// 替换客户设置的短信内容
        /// </summary>
        /// <param name="giftcontent"></param>
        /// <param name="shopname"></param>
        /// <param name="buynick"></param>
        /// <param name="iscoupon"></param>
        /// <returns></returns>
        public static string GetMsg(ShopInfo shop, Trade trade, string giftcontent)
        {
            string iscoupon = shop.IsCoupon;
            string shopname = shop.MsgShopName;
            string buynick = trade.BuyNick;

            string giftstr = "优惠券";
            //if (iscoupon == "1")
            //{
            //    CouponData data = new CouponData();
            //    Coupon coupon = data.GetCouponInfoById(shop);
            //    giftstr = coupon.Num + "元优惠券";
            //}

            //if (shop.IsAlipay == "1")
            //{
            //    CouponData data = new CouponData();
            //    Alipay alipay = data.GetAlipayInfoById(shop);
            //    giftstr += alipay.Num + "元红包";
            //}

            //if (shop.IsFreeCard == "1")
            //{
            //    giftstr += "包邮卡";
            //}

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

        /// <summary>
        /// 替换客户设置的短信内容
        /// </summary>
        /// <param name="giftcontent"></param>
        /// <param name="shopname"></param>
        /// <param name="buynick"></param>
        /// <param name="iscoupon"></param>
        /// <returns></returns>
        public static string GetMsg(string giftcontent, string shopname, string buynick, string iscoupon, string shiptyp, string shipnumber)
        {
            giftcontent = giftcontent.Replace("[shiptyp]", shiptyp);
            giftcontent = giftcontent.Replace("[shipnumber]", shipnumber);

            return GetMsg(giftcontent,shopname, buynick, iscoupon);
        }


        /// <summary>
        /// 替换客户设置的短信内容
        /// </summary>
        /// <param name="giftcontent"></param>
        /// <param name="shopname"></param>
        /// <param name="buynick"></param>
        /// <param name="iscoupon"></param>
        /// <returns></returns>
        public static string GetMsg(string giftcontent, string shopname, string buynick, string iscoupon, string shiptyp, string shipnumber, ShopInfo shop, Trade trade)
        {
            giftcontent = giftcontent.Replace("[shiptyp]", shiptyp);
            giftcontent = giftcontent.Replace("[shipnumber]", shipnumber);

            return GetMsg(shop, trade, giftcontent);
        }

        public static string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.Default.GetBytes(str);
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }

            return (sb.ToString());
        }

        //public static string Send(string phone, string msg)
        //{
        //    //有客户没有手机号也发送短信
        //    if (phone.Length == 0)
        //    {
        //        return "0";
        //    }

        //    string uid = "terrylv";
        //    string pass = "123456";

        //    msg = HttpUtility.UrlEncode(msg);

        //    string param = "username=" + uid + "&password=" + pass + "&method=sendsms&mobile=" + phone + "&msg=" + msg;
        //    byte[] bs = Encoding.ASCII.GetBytes(param);
        //    HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("http://sms3.eachwe.com/api.php");
        //    req.Method = "POST";
        //    req.ContentType = "application/x-www-form-urlencoded";
        //    req.ContentLength = bs.Length;

        //    using (Stream reqStream = req.GetRequestStream())
        //    {
        //        reqStream.Write(bs, 0, bs.Length);
        //    }

        //    using (HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse())
        //    {
        //        using (StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.GetEncoding("GB2312")))
        //        {
        //            string content = reader.ReadToEnd();

        //            if (content.IndexOf("<error>0</error>") == -1)
        //            {
        //                //发送失败
        //                return content;
        //            }
        //            else
        //            {
        //                //发送成功
        //                Regex reg = new Regex(@"<sid>([^<]*)</sid>", RegexOptions.IgnoreCase);
        //                MatchCollection match = reg.Matches(content);
        //                string number = match[0].Groups[1].ToString();
        //                return number;
        //            }
        //        }
        //    }
        //}



        public static string Send(string phone, string msg)
        {
            //有客户没有手机号也发送短信
            if (phone.Length < 11)
            {
                return "0";
            }

            string uid = "ZXHD-SDK-0107-XNYFLX";
            string pass = utils.MD5("WEGXBEPY").ToLower();

            msg = UrlEncode(msg);

            string param = "regcode=" + uid + "&pwd=" + pass + "&phone=" + phone + "&CONTENT=" + msg + "&extnum=11&level=1&schtime=null&reportflag=0&url=&smstype=0&key=aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] bs = Encoding.ASCII.GetBytes(param);

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("http://sms.pica.com/zqhdServer/sendSMS.jsp" + "?" + param);

            req.Method = "GET";

            using (HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse())
            {
                using (StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.GetEncoding("GB2312")))
                {
                    string content = reader.ReadToEnd();
                    //File.WriteAllText(Server.MapPath("aaa.txt"), content);

                    if (content.IndexOf("<result>0</result>") == -1)
                    {
                        Regex reg = new Regex(@"<result>([^<]*)</result>", RegexOptions.IgnoreCase);
                        MatchCollection match = reg.Matches(content);
                        string number = string.Empty;
                        if (reg.IsMatch(content))
                        {
                            number = match[0].Groups[1].ToString(); // match[0].Groups[1].ToString();
                        }
                        else
                        {
                            number = "888888";
                        }

                        if (number.Length > 50)
                        {
                            number = content.Substring(0, 50);
                        }
                        return number;
                    }
                    else
                    {
                        if (content.Length > 50)
                        {
                            content = content.Substring(0, 50);
                        }

                        return content;
                    }
                }
            }
        }
    }
}
