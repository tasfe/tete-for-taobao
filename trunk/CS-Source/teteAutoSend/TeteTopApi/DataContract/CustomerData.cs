using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeteTopApi.Entity;
using System.Data;

namespace TeteTopApi.DataContract
{
    public class CustomerData
    {
        /// <summary>
        /// 获取买家生日短信
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        public List<Customer> GetBackCustomer(ShopInfo shop)
        {
            string sql = "SELECT * FROM TCS_Customer WHERE nick = '" + shop.Nick + "' AND DATEDIFF(d, lastorderdate, GETDATE()) > " + shop.MissionBackDay;
            Console.WriteLine(sql);
            DataTable dt = utils.ExecuteDataTable(sql);

            if (dt.Rows.Count != 0)
            {
                return FormatData(dt);
            }
            else
            {
                return new List<Customer>();
            }
        }

        /// <summary>
        /// 获取回访的买家清单
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        public List<Customer> GetBirthdayCustomer(ShopInfo shop)
        {
            string sql = "SELECT * FROM TCS_Customer WHERE nick = '" + shop.Nick + "' AND DATEDIFF(d, birthday, GETDATE()) = 0";
            Console.WriteLine(sql);
            DataTable dt = utils.ExecuteDataTable(sql);

            if (dt.Rows.Count != 0)
            {
                return FormatData(dt);
            }
            else
            {
                return new List<Customer>();
            }
        }

        /// <summary>
        /// 转换为买家实体
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<Customer> FormatData(DataTable dt)
        {
            List<Customer> infoList = new List<Customer>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Customer info = new Customer();

                info.Nick = dt.Rows[i]["Nick"].ToString();
                info.BuyNick = dt.Rows[i]["BuyNick"].ToString();
                info.Mobile = dt.Rows[i]["Mobile"].ToString();

                infoList.Add(info);
            }
            return infoList;
        }

        /// <summary>
        /// 插入CRM会员记录
        /// </summary>
        /// <param name="trade"></param>
        /// <param name="customer"></param>
        public void InsertCustomerData(Trade trade, Customer customer)
        {
            string sql = "INSERT INTO TCS_Customer (" +
                                        "nick, " +
                                        "buynick, " +
                                        "mobile, " +
                                        "sex, " +
                                        "buyerlevel, " +
                                        "created, " +
                                        "lastlogin, " +
                                        "email, " +
                                        "birthday, " +
                                        //好评特有
                                        "reviewcount, " +
                                        "couponcount, " +
                                        "giftcount, " +
                                        "alipaycount, " +
                                        //CRM特有
                                        "status, " +
                                        "grade, " +
                                        "tradecount, " +
                                        "tradeamount, " +
                                        "groupid, " +
                                        "province, " +
                                        "city, " +
                                        "avgprice, " +
                                        "source, " +
                                        "buyerid, " +
                                        "lastorderdate," +

                                        "truename, " +
                                        "address, " +
                                        "sheng, " +
                                        "shi, " +
                                        "qu " +
                                    " ) VALUES ( " +
                                        " '" + trade.Nick + "', " +
                                        " '" + trade.BuyNick + "', " +
                                        " '" + trade.Mobile + "', " +
                                        " '" + customer.Sex + "', " +
                                        " '" + customer.BuyerLevel + "', " +
                                        " '" + customer.Created + "', " +
                                        " '" + customer.LastLogin + "', " +
                                        " '" + customer.Email + "', " +
                                        " '" + customer.BirthDay + "', " +

                                        " '" + customer.ReviewCount + "', " +
                                        " '" + customer.CouponCount + "', " +
                                        " '" + customer.GiftCount + "', " +
                                        " '" + customer.AlipayCount + "', " +

                                        " '" + customer.Status + "', " +
                                        " '" + customer.Grade + "', " +
                                        " '" + customer.TradeCount + "', " +
                                        " '" + customer.TradeAmount + "', " +
                                        " '" + customer.GroupId + "', " +
                                        " '" + customer.Province + "', " +
                                        " '" + customer.City + "', " +
                                        " '" + customer.AvgPrice + "', " +
                                        " '" + customer.Source + "', " +
                                        " '" + customer.BuyerId + "', " +
                                        " GETDATE(), " +

                                        " '" + trade.receiver_name + "', " +
                                        " '" + trade.receiver_address + "', " +
                                        " '" + trade.receiver_state + "', " +
                                        " '" + trade.receiver_city + "', " +
                                        " '" + trade.receiver_district + "'" +
                                    ") ";

            Console.Write(sql + "\r\n");
            utils.ExecuteNonQuery(sql);
        }

        public Customer InitHaopingData(Customer customer)
        {
            //优惠券赠送次数
            customer.GiftCount = GetGiftCount(customer);

            //优惠券使用次数
            customer.CouponCount = GetCouponUseCount(customer);

            //支付宝红包赠送次数
            customer.AlipayCount = GetAlipayCount(customer);

            //评论次数
            customer.ReviewCount = GetReviewCount(customer);

            return customer;
        }


        /// <summary>
        /// 评论次数
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        private string GetReviewCount(Customer customer)
        {
            string sql = "SELECT COUNT(*) FROM TCS_TradeRate WHERE nick = '" + customer.Nick + "' AND buynick = '" + customer.BuyNick + "'";

            Console.Write(sql + "\r\n");
            return utils.ExecuteString(sql);
        }


        /// <summary>
        /// 支付宝红包赠送次数
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        private string GetAlipayCount(Customer customer)
        {
            string sql = "SELECT COUNT(*) FROM TCS_AlipayDetail WHERE nick = '" + customer.Nick + "' AND buynick = '" + customer.BuyNick + "'";

            Console.Write(sql + "\r\n");
            return utils.ExecuteString(sql);
        }

        /// <summary>
        /// 优惠券赠送次数
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        private string GetGiftCount(Customer customer)
        {
            string sql = "SELECT COUNT(*) FROM TCS_CouponSend WHERE nick = '" + customer.Nick + "' AND buynick = '" + customer.BuyNick + "'";

            Console.Write(sql + "\r\n");
            return utils.ExecuteString(sql);
        }

        /// <summary>
        /// 优惠券使用次数   11
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        private string GetCouponUseCount(Customer customer)
        {
            string sql = "SELECT COUNT(*) FROM TCS_Trade WHERE nick = '" + customer.Nick + "' AND buynick = '" + customer.BuyNick + "' AND iscoupon = 1";

            Console.Write(sql + "\r\n");
            return utils.ExecuteString(sql);
        }

        /// <summary>
        /// 插入CRM会员记录
        /// </summary>
        /// <param name="trade"></param>
        /// <param name="customer"></param>
        public void UpdateCustomerData(Trade trade, Customer customer)
        {
            string sql = "UPDATE TCS_Customer SET " +
                                       "mobile = '" + trade.Mobile + "', " +
                                       "sex = '" + customer.Sex + "', " +
                                       "buyerlevel = '" + customer.BuyerLevel + "', " +
                                       "created = '" + customer.Created + "', " +
                                       "lastlogin = '" + customer.LastLogin + "', " +
                                       "email = '" + customer.Email + "', " +
                                       "birthday = '" + customer.BirthDay + "', " +

                                       "reviewcount = '" + customer.ReviewCount + "', " +
                                       "couponcount = '" + customer.CouponCount + "', " +
                                       "giftcount = '" + customer.GiftCount + "', " +
                                       "alipaycount = '" + customer.AlipayCount + "', " +
                                        //CRM特有
                                        "status = '" + customer.Status + "', " +
                                        "grade = '" + customer.Grade + "', " +
                                        "tradecount = '" + customer.TradeCount + "', " +
                                        "tradeamount = '" + customer.TradeAmount + "', " +
                                        "groupid = '" + customer.GroupId + "', " +
                                        "province = '" + customer.Province + "', " +
                                        "city = '" + customer.City + "', " +
                                        "avgprice = '" + customer.AvgPrice + "', " +
                                        "source = '" + customer.Source + "', " +
                                        "buyerid = '" + customer.BuyerId + "', " +
                                        "lastorderdate = GETDATE(), " +

                                       "truename = '" + trade.receiver_name + "', " +
                                       "address = '" + trade.receiver_address + "', " +
                                       "sheng = '" + trade.receiver_state + "', " +
                                       "shi = '" + trade.receiver_city + "', " +
                                       "qu  = '" + trade.receiver_district + "'" +
                        " WHERE nick = '" + trade.Nick + "' AND buynick = '" + trade.BuyNick + "'";

            Console.Write(sql + "\r\n");
            utils.ExecuteNonQuery(sql);
        }


        /// <summary>
        /// 看看数据库里面是否有此会员数据
        /// </summary>
        /// <param name="trade"></param>
        /// <param name="customer"></param>
        public bool IsHaveThisCustomer(Trade trade)
        {
            string sql = "SELECT COUNT(*) FROM TCS_Customer WHERE nick = '" + trade.Nick + "' AND buynick = '" + trade.BuyNick + "'";
            Console.Write(sql + "\r\n");
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
    }
}
