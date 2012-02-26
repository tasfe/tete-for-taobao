using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeteTopApi.Entity;
using System.Data;

namespace TeteTopApi.DataContract
{
    public class CouponData
    {
        private static object padlockcoupon = new object();
        /// <summary>
        /// 记录该优惠券的赠送信息
        /// </summary>
        /// <param name="trade"></param>
        /// <param name="shop"></param>
        /// <param name="couponId"></param>
        public void InsertCouponSendRecord(Trade trade, ShopInfo shop, string couponId)
        {
            string sql = "INSERT INTO TCS_CouponSend (" +
                                "nick, " +
                                "guid, " +
                                "buynick, " +
                                "orderid, " +
                                "taobaonumber " +
                            " ) VALUES ( " +
                                " '" + trade.Nick + "', " +
                                " '" + shop.CouponID + "', " +
                                " '" + trade.BuyNick + "', " +
                                " '" + trade.Tid + "', " +
                                " '" + couponId + "' " +
                            ") ";
            Console.Write(sql + "\r\n");
            utils.ExecuteNonQuery(sql);

            //更新优惠券已经赠送数量
            sql = "UPDATE TCS_Coupon SET used = used + 1 WHERE guid = '" + shop.CouponID + "'";
            Console.Write(sql + "\r\n");
            utils.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 获取最近7天内卖家的优惠券赠送数量
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        public string GetCouponSendCountWeekByNick(ShopInfo shop)
        {
            string sql = "SELECT COUNT(*) FROM TCS_CouponSend WHERE nick = '" + shop.Nick + "' AND DATEDIFF(d,senddate,GETDATE()) < 14";
            Console.Write(sql + "\r\n");
            string result = utils.ExecuteString(sql);

            return result;
        }

        /// <summary>
        /// 判定该优惠券是否过期或删除
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        public bool CheckCouponCanUsed(ShopInfo shop)
        {
            string sql = "SELECT guid FROM TCS_Coupon WITH (NOLOCK) WHERE nick= '" + shop.Nick + "' AND guid = '" + shop.CouponID + "' AND (GETDATE() > enddate OR isdel = 1)";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 判断客户是否还能领取赠送的优惠券
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        public bool CheckUserCanGetCoupon(Trade trade, Coupon coupon)
        {
            string sql = "SELECT guid FROM TCS_CouponSend WITH (NOLOCK) WHERE buynick= '" + trade.BuyNick + "' AND guid = '" + coupon.CouponGUID + "'";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count >= int.Parse(coupon.Per))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 判断优惠券是否已经赠送完毕
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        public bool CheckCouponCanSend(Trade trade, Coupon coupon)
        {
            string sql = "SELECT guid FROM TCS_Coupon WITH (NOLOCK) WHERE guid = '" + coupon.CouponGUID + "' AND used >= count";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 判断该订单的优惠券是否赠送过-因为数据库优惠券赠送表没加订单号，所以暂时用每天赠送一个代替
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        public bool CheckUserCouponSend(Trade trade, Coupon coupon)
        {
            string sql = "SELECT guid FROM TCS_CouponSend WITH (NOLOCK) WHERE buynick= '" + trade.BuyNick + "' AND guid = '" + coupon.CouponGUID + "' AND orderid = '" + trade.Tid + "'";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 判断该优惠券是否过期
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        public Coupon GetCouponInfoById(ShopInfo shop)
        {
            lock (padlockcoupon)
            {
                DataTable dt = new DataTable();

                string sql = "SELECT * FROM TCS_Coupon WITH (NOLOCK) WHERE guid = '" + shop.CouponID + "' ";
                Console.Write(sql + "\r\n");
                dt = utils.ExecuteDataTable(sql);
                if (dt.Rows.Count == 0)
                {
                    return new Coupon();
                }
                else
                {
                    return FormatData(dt);
                }
            }
        }

        private Coupon FormatData(DataTable dt)
        {
            Coupon info = new Coupon();

            info.Per = dt.Rows[0]["per"].ToString().Trim();
            info.CouponGUID = dt.Rows[0]["guid"].ToString().Trim();
            info.TaobaoCouponId = dt.Rows[0]["taobaocouponid"].ToString().Trim();

            return info;
        }
    }
}
