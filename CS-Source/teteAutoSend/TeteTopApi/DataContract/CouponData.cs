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
        public bool InsertCouponSendRecord(Trade trade, ShopInfo shop, string couponId)
        {
            //判断该订单是否赠送过
            string sql = "SendCouponInfo '" + trade.Nick + "','" + trade.BuyNick + "','" + shop.CouponID + "','" + trade.Tid + "'";
            string count = utils.ExecuteString(sql);

            //如果该订单送过则中断
            if (count != "0")
            {
                return false;
            }
            else
            {
                return true;
            }

            //sql = "INSERT INTO TCS_CouponSend (" +
            //                    "nick, " +
            //                    "guid, " +
            //                    "buynick, " +
            //                    "orderid, " +
            //                    "taobaonumber " +
            //                " ) VALUES ( " +
            //                    " '" + trade.Nick + "', " +
            //                    " '" + shop.CouponID + "', " +
            //                    " '" + trade.BuyNick + "', " +
            //                    " '" + trade.Tid + "', " +
            //                    " '" + couponId + "' " +
            //                ") ";
            //Console.Write(sql + "\r\n");
            //utils.ExecuteNonQuery(sql);

            //return true;
        }

        /// <summary>
        /// 是否为黑名单
        /// </summary>
        /// <param name="trade"></param>
        /// <returns></returns>
        public bool IsBlack(Trade trade)
        {
            string sql = "SELECT COUNT(*) FROM TCS_BlackListGift WHERE nick = '" + trade.Nick + "' AND buynick = '" + trade.BuyNick + "'";
            string count = utils.ExecuteString(sql);
            if (count == "0")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 记录该优惠券的赠送信息
        /// </summary>
        /// <param name="trade"></param>
        /// <param name="shop"></param>
        /// <param name="couponId"></param>
        public void UpdateCouponSendRecord(Trade trade, ShopInfo shop, string couponId)
        {
            //更新优惠券淘宝ID
            string sql = "UPDATE TCS_CouponSend SET taobaonumber = '" + couponId + "' WHERE orderid = '" + trade.Tid + "'";
            Console.Write(sql + "\r\n");
            utils.ExecuteNonQuery(sql);

            //更新优惠券已经赠送数量
            sql = "UPDATE TCS_Coupon SET used = used + 1 WHERE guid = '" + shop.CouponID + "'";
            Console.Write(sql + "\r\n");
            utils.ExecuteNonQuery(sql);
            return;
        }


        /// <summary>
        /// 记录该优惠券的赠送信息
        /// </summary>
        /// <param name="trade"></param>
        /// <param name="shop"></param>
        /// <param name="couponId"></param>
        public void DeleteCouponSendRecord(Trade trade, ShopInfo shop, string couponId)
        {
            string sql = "DELETE FROM TCS_CouponSend WHERE orderid = '" + trade.Tid + "'";
            Console.Write(sql + "\r\n");
            utils.ExecuteNonQuery(sql);
            return;
        }

        /// <summary>
        /// 记录该优惠券的赠送信息
        /// </summary>
        /// <param name="trade"></param>
        /// <param name="shop"></param>
        /// <param name="couponId"></param>
        public void InsertAlipaySendRecord(Trade trade, ShopInfo shop, AlipayDetail detail)
        {
            string sql = "UPDATE TCS_Alipay SET used = used + 1 WHERE guid = '" + detail.GUID + "'";
            Console.Write(sql + "\r\n");
            utils.ExecuteNonQuery(sql);

            //更新优惠券已经赠送数量
            sql = "UPDATE TCS_AlipayDetail SET issend = 1,buynick = '" + trade.BuyNick + "',senddate = GETDATE(), orderid = '" + trade.Tid + "' WHERE guid = '" + detail.GUID + "' AND card = '" + detail.Card + "'";
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
            string sql = "SELECT guid FROM TCS_CouponSend WITH (NOLOCK) WHERE buynick= '" + trade.BuyNick + "' AND guid = '" + coupon.CouponGUID + "' AND taobaonumber NOT IN (SELECT couponnumber FROM TCS_Trade WHERE nick = '" + trade.Nick + "' AND iscoupon = 1)";
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
        /// 判断支付宝红包是否已经赠送完毕
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        public bool CheckAlipayCanSend(Trade trade, Alipay alipay)
        {
            string sql = "SELECT guid FROM TCS_Alipay WITH (NOLOCK) WHERE guid = '" + alipay.GUID + "' AND used >= count";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                return false;
            }

            return true;
        }



        /// <summary>
        /// 每订单最多赠送一个
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        public bool CheckOrderAlipayCanSend(Trade trade, Alipay alipay)
        {
            string sql = "SELECT guid FROM [TCS_AlipayDetail] WITH (NOLOCK) WHERE buynick= '" + trade.BuyNick + "' AND guid = '" + alipay.GUID + "' AND orderid = '" + trade.Tid + "'";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count > 0)
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
        /// 判断该订单的优惠券是否赠送过-因为数据库优惠券赠送表没加订单号，所以暂时用每天赠送一个代替
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        public bool CheckAlipayExpired(Trade trade, Alipay alipay)
        {
            string sql = "SELECT guid FROM TCS_Alipay WITH (NOLOCK) WHERE guid = '" + alipay.GUID + "' AND DATEDIFF(d, GETDATE(), enddate) > 0";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count > 0)
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
        public bool CheckUserCanGetAlipay(Trade trade, Alipay alipay)
        {
            string sql = "SELECT guid FROM TCS_AlipayDetail WITH (NOLOCK) WHERE buynick= '" + trade.BuyNick + "' AND guid = '" + alipay.GUID + "'";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);

            //获取优惠券最大赠送数量
            if (dt.Rows.Count >= int.Parse(alipay.Per))
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

        /// <summary>
        /// 判断该优惠券是否过期
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        public Alipay GetAlipayInfoById(ShopInfo shop)
        {
            DataTable dt = new DataTable();

            string sql = "SELECT * FROM TCS_Alipay WITH (NOLOCK) WHERE guid = '" + shop.AlipayID + "' ";
            Console.Write(sql + "\r\n");
            dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count == 0)
            {
                return new Alipay();
            }
            else
            {
                return FormatDataAlipay(dt);
            }
        }

        /// <summary>
        /// 判断该优惠券是否过期
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        public AlipayDetail GetAlipayDetailInfoById(ShopInfo shop)
        {
            DataTable dt = new DataTable();

            string sql = "SELECT TOP 1 * FROM TCS_AlipayDetail WITH (NOLOCK) WHERE guid = '" + shop.AlipayID + "' AND issend = 0";
            Console.Write(sql + "\r\n");
            dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count == 0)
            {
                return new AlipayDetail();
            }
            else
            {
                return FormatDataAlipayDetail(dt);
            }
        }

        private AlipayDetail FormatDataAlipayDetail(DataTable dt)
        {
            AlipayDetail info = new AlipayDetail();

            info.GUID = dt.Rows[0]["GUID"].ToString().Trim();
            info.Card = dt.Rows[0]["Card"].ToString().Trim();
            info.Pass = dt.Rows[0]["Pass"].ToString().Trim();

            return info;
        }

        private Alipay FormatDataAlipay(DataTable dt)
        {
            Alipay info = new Alipay();

            info.GUID = dt.Rows[0]["GUID"].ToString().Trim();
            info.Num = dt.Rows[0]["Num"].ToString().Trim();
            info.Per = dt.Rows[0]["per"].ToString().Trim();
            info.Count = dt.Rows[0]["count"].ToString().Trim();
            info.EndDate = dt.Rows[0]["enddate"].ToString().Trim();

            return info;
        }

        private Coupon FormatData(DataTable dt)
        {
            Coupon info = new Coupon();

            info.Per = dt.Rows[0]["per"].ToString().Trim();
            info.Num = dt.Rows[0]["num"].ToString().Trim();
            info.CouponGUID = dt.Rows[0]["guid"].ToString().Trim();
            info.TaobaoCouponId = dt.Rows[0]["taobaocouponid"].ToString().Trim();

            return info;
        }
    }
}
