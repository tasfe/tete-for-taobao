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
        /// <summary>
        /// 记录该优惠券的赠送信息
        /// </summary>
        /// <param name="trade"></param>
        /// <param name="shop"></param>
        /// <param name="couponId"></param>
        public void InsertCouponSendRecord(Trade trade, ShopInfo shop, string couponId)
        {
            string sql = "INSERT INTO TopCouponSend (" +
                                "nick, " +
                                "couponid, " +
                                "sendto, " +
                                "number, " +
                                "count " +
                            " ) VALUES ( " +
                                " '" + trade.Nick + "', " +
                                " '" + shop.CouponID + "', " +
                                " '" + trade.BuyNick + "', " +
                                " '" + couponId + "', " +
                                " '1' " +
                            ") ";
            Console.Write(sql + "\r\n");
            utils.ExecuteNonQuery(sql);

            //更新优惠券已经赠送数量
            sql = "UPDATE TopCoupon SET used = used + 1 WHERE coupon_id = '" + shop.CouponID + "'";
            Console.Write(sql + "\r\n");
            utils.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 判断该优惠券是否过期
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        public bool CheckCouponCanUsed(ShopInfo shop)
        {
            string sql = "SELECT id FROM TopCoupon WITH (NOLOCK) WHERE nick= '" + shop.Nick + "' AND coupon_id = '" + shop.CouponID + "' AND GETDATE() > end_time";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count == 0)
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
            string sql = "SELECT id FROM TopCouponSend WITH (NOLOCK) WHERE sendto= '" + trade.BuyNick + "' AND couponid = '" + coupon.CouponId + "'";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count >= int.Parse(coupon.Per))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 判断该订单的优惠券是否赠送过
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        public bool CheckUserCouponSend(Trade trade, Coupon coupon)
        {
            string sql = "SELECT id FROM TopCouponSend WITH (NOLOCK) WHERE sendto= '" + trade.BuyNick + "' AND couponid = '" + coupon.CouponId + "'";
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
            string sql = "SELECT * FROM TopCoupon WITH (NOLOCK) WHERE coupon_id = '" + shop.CouponID + "' ";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count == 0)
            {
                return new Coupon();
            }

            return FormatData(dt);
        }

        private Coupon FormatData(DataTable dt)
        {
            Coupon info = new Coupon();

            info.Per = dt.Rows[0]["per"].ToString().Trim();
            info.CouponId = dt.Rows[0]["coupon_id"].ToString().Trim();

            return info;
        }
    }
}
