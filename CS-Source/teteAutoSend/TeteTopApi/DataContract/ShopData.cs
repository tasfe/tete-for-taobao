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
        /// <summary>
        /// 根据卖家ID获取好评有礼的相关设置
        /// </summary>
        /// <param name="nick"></param>
        /// <returns></returns>
        public ShopInfo ShopInfoGetByNick(string nick)
        {
            string sql = "SELECT * FROM TopAutoReview r WITH (NOLOCK) INNER JOIN TopTaobaoShop s WITH (NOLOCK) ON s.nick = r.nick WHERE r.nick = '" + nick + "'";
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


        /// <summary>
        /// 获取当前正在使用延迟发货短信通知并有短信可发的卖家信息
        /// </summary>
        /// <param name="nick"></param>
        /// <returns></returns>
        public List<ShopInfo> GetShopInfoListAlert()
        {
            string sql = "SELECT * FROM TopAutoReview r WITH (NOLOCK) INNER JOIN TopTaobaoShop s WITH (NOLOCK) ON s.nick = r.nick AND r.isdel = 0 AND r.reviewflag = 1 AND r.total > 0";
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
        /// 获取当前正在使用物流到货短信通知并有短信可发的卖家信息
        /// </summary>
        /// <param name="nick"></param>
        /// <returns></returns>
        public List<ShopInfo> GetShopInfoListShippingAlert()
        {
            string sql = "SELECT * FROM TopAutoReview r WITH (NOLOCK) INNER JOIN TopTaobaoShop s WITH (NOLOCK) ON s.nick = r.nick AND r.isdel = 0 AND r.shippingflag = 1 AND r.total > 0";
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
            info.Session = dt.Rows[0]["sessionblog"].ToString();
            info.Version = dt.Rows[0]["versionnoblog"].ToString();
            info.IsCancelAuto = dt.Rows[0]["IsCancelAuto"].ToString();
            info.Keyword = dt.Rows[0]["Keyword"].ToString();
            info.WordCount = dt.Rows[0]["WordCount"].ToString();

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
                info.Session = dt.Rows[i]["sessionblog"].ToString();
                info.Version = dt.Rows[i]["versionnoblog"].ToString();
                info.IsCancelAuto = dt.Rows[0]["IsCancelAuto"].ToString();
                info.Keyword = dt.Rows[0]["Keyword"].ToString();
                info.WordCount = dt.Rows[0]["WordCount"].ToString();

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
            string sql = "INSERT INTO TopMsg (" +
                                "nick, " +
                                "sendto, " +
                                "phone, " +
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

            //更新状态
            sql = "UPDATE TopOrder WITH (ROWLOCK) SET isgiftmsg = 1 WHERE orderid = '" + trade.Tid + "'";
            Console.Write(sql + "\r\n");
            utils.ExecuteNonQuery(sql);

            //更新短信数量
            sql = "UPDATE TopAutoReview WITH (ROWLOCK) SET used = used + 1,total = total-1 WHERE nick = '" + shop.Nick + "'";
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
            string sql = "SELECT id FROM TopMsg WHERE sendto = '" + trade.BuyNick + "' AND typ = '" + typ + "' AND DATEDIFF(d, addtime, GETDATE()) = 0";
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
            string sql = "SELECT id FROM TopMsg WHERE sendto = '" + trade.BuyNick + "' AND typ = '" + typ + "' AND orderid = '" + trade.Tid + "'";
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
            string sql = "INSERT INTO TopMsgBak (" +
                                "nick, " +
                                "sendto, " +
                                "phone, " +
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
        }    
    }
}
