using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeteTopApi.Entity;
using System.Data;

namespace TeteTopApi.DataContract
{
    public class ShopData
    {
        private static object padlockshop = new object();
        /// <summary>
        /// 根据卖家ID获取好评有礼的相关设置
        /// </summary>
        /// <param name="nick"></param>
        /// <returns></returns>
        public ShopInfo ShopInfoGetByNick(string nick)
        {
            lock (padlockshop)
            {
                string sql = "SELECT * FROM TCS_ShopConfig r WITH (NOLOCK) INNER JOIN TCS_ShopSession s WITH (NOLOCK) ON s.nick = r.nick WHERE r.nick = '" + nick + "'";
                Console.Write(sql + "\r\n");
                DataTable dt = utils.ExecuteDataTable(sql);
                if (dt.Rows.Count != 0)
                {
                    return FormatData(dt);
                }
                else
                {
                    return new ShopInfo();
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="nick"></param>
        /// <returns></returns>
        public List<ShopInfo> ShopInfoListAll()
        {
            lock (padlockshop)
            {
                string sql = "SELECT * FROM TCS_ShopConfig r WITH (NOLOCK) INNER JOIN TCS_ShopSession s WITH (NOLOCK) ON s.nick = r.nick";
                Console.Write(sql + "\r\n");
                DataTable dt = utils.ExecuteDataTable(sql);
                if (dt.Rows.Count != 0)
                {
                    return FormatDataList(dt);
                }
                else
                {
                    return new List<ShopInfo>();
                }
            }
        }


        /// <summary>
        /// 获取账户里面有1周前未审核评价且提交过手机号码的卖家信息
        /// </summary>
        /// <returns></returns>
        public List<ShopInfo> GetShopInfoListUnChecked()
        {
            string sql = "SELECT * FROM TCS_ShopConfig r WITH (NOLOCK) INNER JOIN TCS_ShopSession s WITH (NOLOCK) ON s.nick = r.nick WHERE r.nick IN (SELECT DISTINCT nick FROM TCS_TradeRateCheck WHERE ischeck = 0 AND DATEDIFF(d,reviewdate,GETDATE()) > 7) AND phone IS NOT NULL AND iskefu = 1 AND iscoupon = 1 AND couponid IS NOT NULL AND isdel = 0 AND version = 3";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                return FormatDataList(dt);
            }
            else
            {
                return new List<ShopInfo>();
            }
        }

        /// <summary>
        /// 获取账户里面赠送优惠券到期的客户并提醒他们重新创建优惠券
        /// </summary>
        /// <returns></returns>
        public List<ShopInfo> GetShopInfoListCouponExpired()
        {
            string sql = "SELECT * FROM TCS_ShopConfig r WITH (NOLOCK) INNER JOIN TCS_ShopSession s WITH (NOLOCK) ON s.nick = r.nick WHERE r.couponid IN (SELECT guid FROM TCS_Coupon WHERE enddate < GETDATE()) AND phone IS NOT NULL AND iscoupon = 1 AND couponid IS NOT NULL AND isdel = 0";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                return FormatDataList(dt);
            }
            else
            {
                return new List<ShopInfo>();
            }
        }

        /// <summary>
        /// 获取账户里面赠送优惠券到期的客户并提醒他们重新创建优惠券
        /// </summary>
        /// <returns></returns>
        public List<ShopInfo> GetShopInfoListCouponOver()
        {
            string sql = "SELECT * FROM TCS_ShopConfig r WITH (NOLOCK) INNER JOIN TCS_ShopSession s WITH (NOLOCK) ON s.nick = r.nick WHERE r.couponid IN (SELECT guid FROM TCS_Coupon WHERE count <= used) AND phone IS NOT NULL AND iscoupon = 1 AND couponid IS NOT NULL AND isdel = 0";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                return FormatDataList(dt);
            }
            else
            {
                return new List<ShopInfo>();
            }
        }

        /// <summary>
        /// 获取正常使用且有手机提醒的店铺
        /// </summary>
        /// <returns></returns>
        public List<ShopInfo> GetShopInfoNormalUsed()
        {
            string sql = "SELECT * FROM TCS_ShopConfig r WITH (NOLOCK) INNER JOIN TCS_ShopSession s WITH (NOLOCK) ON s.nick = r.nick WHERE phone IS NOT NULL AND iscoupon = 1 AND couponid IS NOT NULL AND isdel = 0";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                return FormatDataList(dt);
            }
            else
            {
                return new List<ShopInfo>();
            }
        }


        /// <summary>
        /// 获取全部正常使用中的店铺
        /// </summary>
        /// <returns></returns>
        public List<ShopInfo> GetShopInfoNormalUsedAll()
        {
            string sql = "SELECT * FROM TCS_ShopConfig r WITH (NOLOCK) INNER JOIN TCS_ShopSession s WITH (NOLOCK) ON s.nick = r.nick WHERE isdel = 0";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                return FormatDataList(dt);
            }
            else
            {
                return new List<ShopInfo>();
            }
        }


        /// <summary>
        /// 获取当前正在使用延迟发货短信通知并有短信可发的卖家信息
        /// </summary>
        /// <param name="nick"></param>
        /// <returns></returns>
        public List<ShopInfo> GetShopInfoListAlert()
        {
            string sql = "SELECT * FROM TCS_ShopConfig r WITH (NOLOCK) INNER JOIN TCS_ShopSession s WITH (NOLOCK) ON s.nick = r.nick AND r.isdel = 0 AND r.reviewflag = 1 AND r.total > 0";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                return FormatDataList(dt);
            }
            else
            {
                return new List<ShopInfo>();
            }
        }


        /// <summary>
        /// 获取当前正在使用物流到货短信通知并有短信可发的卖家信息(增加延迟发货短信的卖家，否则无法获取物流状态)
        /// </summary>
        /// <param name="nick"></param>
        /// <returns></returns>
        public List<ShopInfo> GetShopInfoListShippingAlert()
        {
            string sql = "SELECT * FROM TCS_ShopConfig r WITH (NOLOCK) INNER JOIN TCS_ShopSession s WITH (NOLOCK) ON s.nick = r.nick AND r.isdel = 0 AND (r.shippingflag = 1 OR r.reviewflag = 1) AND r.total > 0";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                return FormatDataList(dt);
            }
            else
            {
                return new List<ShopInfo>();
            }
        }

        /// <summary>
        /// 生成新格式的店铺基本设置数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private ShopInfo FormatData(DataTable dt)
        {
            ShopInfo info = new ShopInfo();

            info.CouponID = dt.Rows[0]["couponid"].ToString().Trim();
            info.IsKefu = dt.Rows[0]["iskefu"].ToString();
            info.MinDateSelf = dt.Rows[0]["maxdate"].ToString();
            info.MinDateSystem = dt.Rows[0]["mindate"].ToString();
            info.MsgCount = dt.Rows[0]["total"].ToString();
            info.MsgCouponContent = dt.Rows[0]["giftcontent"].ToString();
            info.MsgFahuoContent = dt.Rows[0]["fahuocontent"].ToString();
            info.MsgIsCoupon = dt.Rows[0]["giftflag"].ToString();
            info.MsgIsFahuo = dt.Rows[0]["fahuoflag"].ToString();
            info.MsgIsReview = dt.Rows[0]["reviewflag"].ToString();
            info.MsgIsShipping = dt.Rows[0]["shippingflag"].ToString();
            info.MsgReviewContent = dt.Rows[0]["reviewcontent"].ToString();
            info.MsgShippingContent = dt.Rows[0]["shippingcontent"].ToString();
            info.MsgShopName = dt.Rows[0]["shopname"].ToString();
            info.Nick = dt.Rows[0]["nick"].ToString();
            info.Session = dt.Rows[0]["session"].ToString();
            info.Version = dt.Rows[0]["version"].ToString();
            info.IsCancelAuto = dt.Rows[0]["IsCancelAuto"].ToString();
            info.Keyword = dt.Rows[0]["Keyword"].ToString();
            info.WordCount = dt.Rows[0]["WordCount"].ToString();
            info.IsCoupon = dt.Rows[0]["IsCoupon"].ToString();
            info.Mobile = dt.Rows[0]["phone"].ToString();

            Console.Write(dt.Rows[0]["session"].ToString() + "@@@@@@@@@@@@@\r\n");
            Console.Write(info.Session + "############\r\n");

            return info;
        }

        /// <summary>
        /// 生成新格式的店铺基本设置数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<ShopInfo> FormatDataList(DataTable dt)
        {
            List<ShopInfo> infoList = new List<ShopInfo>();

            for(int i = 0;i<dt.Rows.Count;i++)
            {
                ShopInfo info = new ShopInfo();

                info.CouponID = dt.Rows[i]["couponid"].ToString().Trim();
                info.IsKefu = dt.Rows[i]["iskefu"].ToString();
                info.MinDateSelf = dt.Rows[i]["maxdate"].ToString();
                info.MinDateSystem = dt.Rows[i]["mindate"].ToString();
                info.MsgCount = dt.Rows[i]["total"].ToString();
                info.MsgCouponContent = dt.Rows[i]["giftcontent"].ToString();
                info.MsgFahuoContent = dt.Rows[i]["fahuocontent"].ToString();
                info.MsgIsCoupon = dt.Rows[i]["giftflag"].ToString();
                info.MsgIsFahuo = dt.Rows[i]["fahuoflag"].ToString();
                info.MsgIsReview = dt.Rows[i]["reviewflag"].ToString();
                info.MsgReviewTime = dt.Rows[i]["reviewtime"].ToString();
                info.MsgIsShipping = dt.Rows[i]["shippingflag"].ToString();
                info.MsgReviewContent = dt.Rows[i]["reviewcontent"].ToString();
                info.MsgShippingContent = dt.Rows[i]["shippingcontent"].ToString();
                info.MsgShopName = dt.Rows[i]["shopname"].ToString();
                info.Nick = dt.Rows[i]["nick"].ToString();
                info.Session = dt.Rows[i]["session"].ToString();
                info.Version = dt.Rows[i]["version"].ToString();
                info.IsCancelAuto = dt.Rows[i]["IsCancelAuto"].ToString();
                info.Keyword = dt.Rows[i]["Keyword"].ToString();
                info.WordCount = dt.Rows[i]["WordCount"].ToString();
                info.IsCoupon = dt.Rows[i]["IsCoupon"].ToString();
                info.Mobile = dt.Rows[i]["phone"].ToString();

                infoList.Add(info);
            }
            return infoList;
        }

        /// <summary>
        /// 记录已经成功发送的短信
        /// </summary>
        /// <param name="shop"></param>
        /// <param name="trade"></param>
        /// <param name="msg"></param>
        /// <param name="result"></param>
        /// <param name="typ"></param>
        public void InsertShopMsgLog(ShopInfo shop, Trade trade, string msg, string result, string typ)
        {
            //记录短信发送记录
            string sql = "INSERT INTO TCS_MsgSend (" +
                                "nick, " +
                                "buynick, " +
                                "mobile, " +
                                "[content], " +
                                "yiweiid, " +
                                "orderid, " +
                                "num, " +
                                "typ " +
                            " ) VALUES ( " +
                                " '" + shop.Nick + "', " +
                                " '" + trade.BuyNick + "', " +
                                " '" + trade.Mobile + "', " +
                                " '" + msg.Replace("'", "''") + "', " +
                                " '" + result + "', " +
                                " '" + trade.Tid + "', " +
                                " '1', " +
                                " '" + typ + "' " +
                            ") ";
            Console.Write(sql + "\r\n");
            utils.ExecuteNonQuery(sql);

            ////更新状态
            //sql = "UPDATE TopOrder WITH (ROWLOCK) SET isgiftmsg = 1 WHERE orderid = '" + trade.Tid + "'";
            //Console.Write(sql + "\r\n");
            //utils.ExecuteNonQuery(sql);

            //更新短信数量
            sql = "UPDATE TCS_ShopConfig WITH (ROWLOCK) SET used = used + 1,total = total-1 WHERE nick = '" + shop.Nick + "'";
            Console.Write(sql + "\r\n");
            utils.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 判断今天该短信是否发过
        /// </summary>
        /// <param name="trade"></param>
        /// <param name="typ"></param>
        /// <returns></returns>
        public bool IsSendMsgToday(Trade trade, string typ)
        {
            string sql = "SELECT cguid FROM TCS_MsgSend WHERE buynick = '" + trade.BuyNick + "' AND nick = '" + trade.Nick + "' AND typ = '" + typ + "' AND DATEDIFF(d, adddate, GETDATE()) = 0";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 判断该短信是否发过按订单算
        /// </summary>
        /// <param name="trade"></param>
        /// <param name="typ"></param>
        /// <returns></returns>
        public bool IsSendMsgOrder(Trade trade, string typ)
        {
            string sql = "SELECT cguid FROM TCS_MsgSend WHERE buynick = '" + trade.BuyNick + "' AND nick = '" + trade.Nick + "' AND typ = '" + typ + "' AND orderid = '" + trade.Tid + "'";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 记录因为网络原因导致发送失败的短信
        /// </summary>
        /// <param name="shop"></param>
        /// <param name="trade"></param>
        /// <param name="msg"></param>
        /// <param name="result"></param>
        /// <param name="typ"></param>
        public void InsertShopErrMsgLog(ShopInfo shop, Trade trade, string msg, string result, string typ)
        {
            //记录短信发送记录
            //string sql = "INSERT INTO TopMsgBak (" +
            //                    "nick, " +
            //                    "sendto, " +
            //                    "phone, " +
            //                    "[content], " +
            //                    "yiweiid, " +
            //                    "orderid, " +
            //                    "num, " +
            //                    "typ " +
            //                " ) VALUES ( " +
            //                    " '" + shop.Nick + "', " +
            //                    " '" + trade.BuyNick + "', " +
            //                    " '" + trade.Mobile + "', " +
            //                    " '" + msg.Replace("'", "''") + "', " +
            //                    " '" + result + "', " +
            //                    " '" + trade.Tid + "', " +
            //                    " '1', " +
            //                    " '" + typ + "' " +
            //                ") ";
            //Console.Write(sql + "\r\n");
            //utils.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 设置该店铺的状态为不可用
        /// </summary>
        /// <param name="shop"></param>
        public void DeleteShop(ShopInfo shop)
        {
            string sql = "UPDATE TCS_ShopConfig SET isdel = 1 WHERE nick = '" + shop.Nick + "'";
            Console.Write(sql + "\r\n");
            utils.ExecuteNonQuery(sql);

            sql = "UPDATE TCS_ShopSession SET version = -1 WHERE nick = '" + shop.Nick + "'";
            Console.Write(sql + "\r\n");
            utils.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 设置该店铺的状态为可用并更新版本号
        /// </summary>
        /// <param name="shop"></param>
        public void ActiveShopVersion(ShopInfo shop)
        {
            string sql = "UPDATE TCS_ShopConfig SET isdel = 0 WHERE nick = '" + shop.Nick + "'";
            Console.Write(sql + "\r\n");
            utils.ExecuteNonQuery(sql);

            sql = "UPDATE TCS_ShopSession SET version = " + shop.Version + " WHERE nick = '" + shop.Nick + "'";
            Console.Write(sql + "\r\n");
            utils.ExecuteNonQuery(sql);
        }
    }
}
