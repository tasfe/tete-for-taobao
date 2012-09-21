using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeteTopApi.Entity;

namespace TeteTopApi.TopApi
{
    public class TopApiHaoping
    {
        public TopApiHaoping(string session)
        {
            this.AppKey = "12159997";
            this.Secret = "614e40bfdb96e9063031d1a9e56fbed5";
            this.Session = session;
            this.Url = "http://gw.api.taobao.com/router/rest";
        }

        /// <summary>
        /// 根据订单号码获取该用户的订单信息
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public Trade GetTradeByTid(Trade simpleTrade)
        {
            Api top = new Api(AppKey, Secret, Session, Url);

            IDictionary<string, string> param = new Dictionary<string, string>();
            param.Add("fields", "receiver_mobile, orders.num_iid, created, consign_time, total_fee, promotion_details, type, receiver_name, receiver_state, receiver_city, receiver_district, receiver_address, status, buyer_area");
            param.Add("tid", simpleTrade.Tid);

            string result = top.CommonTopApi("taobao.trade.fullinfo.get", param, Session);
            Console.Write(Session + "\r\n");
            Console.Write(result);
            simpleTrade.Mobile = utils.GetValueByProperty(result, "receiver_mobile");
            simpleTrade.BuyerArea = utils.GetValueByProperty(result, "buyer_area");
            simpleTrade.NumIid = utils.GetValueByPropertyNum(result, "num_iid");
            simpleTrade.Created = utils.GetValueByProperty(result, "created");
            simpleTrade.SendTime = utils.GetValueByProperty(result, "consign_time");
            simpleTrade.OrderPrice = utils.GetValueByProperty(result, "total_fee");
            if (utils.GetValueByProperty(result, "promotion_details").IndexOf("店铺优惠券") != -1)
            {
                simpleTrade.IsUseCoupon = "1";
                simpleTrade.CouponPrice = utils.GetValueByProperty(result, "discount_fee");
            }
            else
            {
                simpleTrade.CouponPrice = "0";
                simpleTrade.IsUseCoupon = "0";
            }

            //特殊判断催单订单不获取物流信息
            if (simpleTrade.Status != "CuiDan")
            {
                simpleTrade = GetOrderShippingInfo(simpleTrade);
            }

            //新增订单类型，判断是否为分销订单
            simpleTrade.OrderType = utils.GetValueByProperty(result, "type");
            //新增订单收货信息，为CRM做准备
            simpleTrade.receiver_name = utils.GetValueByProperty(result, "receiver_name");
            simpleTrade.receiver_state = utils.GetValueByProperty(result, "receiver_state");
            simpleTrade.receiver_city = utils.GetValueByProperty(result, "receiver_city");
            simpleTrade.receiver_district = utils.GetValueByProperty(result, "receiver_district");
            simpleTrade.receiver_address = utils.GetValueByProperty(result, "receiver_address");
            //获取订单状态，为催单服务
            simpleTrade.Status = utils.GetValueByProperty(result, "status");

            return simpleTrade;
        }

        /// <summary>
        /// 根据订单号码获取该订单的物流信息
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public string GetShippingStatusByTid(Trade simpleTrade)
        {
            Api top = new Api(AppKey, Secret, Session, Url);
            Console.Write(simpleTrade.Tid + "\r\n");

            IDictionary<string, string> param = new Dictionary<string, string>();
            param.Add("fields", "delivery_start,delivery_end,status");
            param.Add("tid", simpleTrade.Tid);

            string result = top.CommonTopApi("taobao.logistics.orders.get", param, Session);
            Console.Write(result + "\r\n");
            result = utils.GetValueByProperty(result, "status");

            return result;
        }

        /// <summary>
        /// 免运费
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public string FreeTradePost(Trade simpleTrade)
        {
            Api top = new Api(AppKey, Secret, Session, Url);

            IDictionary<string, string> param = new Dictionary<string, string>();
            param.Add("post_fee", "0");
            param.Add("tid", simpleTrade.Tid);

            string result = top.CommonTopApi("taobao.trade.postage.update", param, Session);
            Console.Write(result + "\r\n");

            return result;
        }

        /// <summary>
        /// 根据订单号码获取该订单的物流信息
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public Customer GetUserInfoByNick(Trade simpleTrade)
        {
            Api top = new Api(AppKey, Secret, Session, Url);
            Customer customer = new Customer();

            IDictionary<string, string> param = new Dictionary<string, string>();
            param.Add("fields", "sex,buyer_credit.level,created,last_visit,birthday,email");
            param.Add("nick", simpleTrade.BuyNick);
            string result = top.CommonTopApi("taobao.user.get", param, Session);

            customer.Sex = utils.GetValueByProperty(result, "sex");
            customer.BuyerLevel = utils.GetValueByPropertyNum(result, "level");
            customer.Created = utils.GetValueByProperty(result, "created");
            customer.LastLogin = utils.GetValueByProperty(result, "last_visit");
            customer.BirthDay = utils.GetValueByProperty(result, "birthday");
            customer.Email = utils.GetValueByProperty(result, "email");
            customer.BuyNick = simpleTrade.BuyNick;
            customer.Nick = simpleTrade.Nick;
            Console.Write(result + "\r\n");

            //CRM相关属性 
            param = new Dictionary<string, string>();
            param.Add("fields", "status,grade,trade_count,trade_amount,group_ids,province,city,avg_price,relation_source,buyer_id");
            param.Add("buyer_nick", simpleTrade.BuyNick);
            param.Add("current_page", "1");
            result = top.CommonTopApi("taobao.crm.members.search", param, Session);
            Console.Write(result + "\r\n");
            
            customer.Status = utils.GetValueByProperty(result, "status");
            customer.Grade = utils.GetValueByPropertyNum(result, "grade");
            customer.TradeCount = utils.GetValueByPropertyNum(result, "trade_count");
            customer.TradeAmount = utils.GetValueByProperty(result, "trade_amount");
            customer.GroupId = utils.GetValueByProperty(result, "group_ids");
            customer.Province = utils.GetValueByPropertyNum(result, "province");
            customer.City = utils.GetValueByProperty(result, "city");
            customer.AvgPrice = utils.GetValueByProperty(result, "avg_price");
            customer.Source = utils.GetValueByPropertyNum(result, "relation_source");
            customer.BuyerId = utils.GetValueByPropertyNum(result, "buyer_id");

            return customer;
        }

        /// <summary>
        /// 根据订单号码获取该订单的物流信息(详细，包含签收人和日期)
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public string GetShippingStatusDetailByTid(Trade simpleTrade)
        {
            Api top = new Api(AppKey, Secret, Session, Url);

            IDictionary<string, string> param = new Dictionary<string, string>();
            param.Add("seller_nick", simpleTrade.Nick);
            param.Add("tid", simpleTrade.Tid);

            string result = top.CommonTopApiXml("taobao.logistics.trace.search", param, Session);

            return result;
        }

        /// <summary>
        /// 获取买家订单相关的优惠券使用信息和金额信息
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public string GetCouponTradeTotalByNick(Trade trade)
        {
            Api top = new Api(AppKey, Secret, Session, Url);

            IDictionary<string, string> param = new Dictionary<string, string>();
            param.Add("fields", "total_fee,promotion_details");
            param.Add("tid", trade.Tid);

            string result = top.CommonTopApiXml("taobao.trade.fullinfo.get", param, Session);

            return result;
        }

        /// <summary>
        /// 根据订单号码获取该订单的物流信息(详细，包含签收人和日期)
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public string GetUserExpiredDate(ShopInfo shop)
        {
            Api top = new Api(AppKey, Secret, Session, Url);

            IDictionary<string, string> param = new Dictionary<string, string>();
            param.Add("article_code", "service-0-22904");
            param.Add("nick", shop.Nick);

            string result = top.CommonTopApi("taobao.vas.subscribe.get", param, Session);
            
            return result;
        }

        /// <summary>
        /// 向指定用户赠送优惠券
        /// </summary>
        /// <param name="buyNick"></param>
        /// <param name="couponId"></param>
        /// <returns></returns>
        public string SendCoupon(string buyNick, string couponId, ref string taobaoResult)
        {
            Api top = new Api(AppKey, Secret, Session, Url);

            IDictionary<string, string> param = new Dictionary<string, string>();
            param.Add("coupon_id", couponId);
            param.Add("buyer_nick", buyNick);

            string result = top.CommonTopApi("taobao.promotion.coupon.send", param, Session);
            result = utils.GetValueByPropertyNum(result, "coupon_number");

            taobaoResult = result;

            return result;
        }



        /// <summary>
        /// 获取订单的物流相关信息
        /// </summary>
        /// <param name="buyNick"></param>
        /// <param name="couponId"></param>
        /// <returns></returns>
        public Trade GetOrderShippingInfo(Trade trade)
        {
            Api top = new Api(AppKey, Secret, Session, Url);

            IDictionary<string, string> param = new Dictionary<string, string>();
            param.Add("tid", trade.Tid);
            param.Add("fields", "out_sid, company_name");

            ////临时处理TID为空的情况
            //if (trade.Tid.Length == 0)
            //{
            //    return trade;
            //}   

            string result = top.CommonTopApi("taobao.logistics.orders.detail.get", param, Session);
            Console.Write(result + "\r\n");
            trade.ShippingNumber = utils.GetValueByProperty(result, "out_sid");
            trade.ShippingCompanyName = utils.GetValueByProperty(result, "company_name");

            return trade;
        }

        /// <summary>
        /// 获取丢失的客户通知消息
        /// </summary>
        /// <param name="buyNick"></param>
        /// <param name="couponId"></param>
        /// <returns></returns>
        public string GetLostMessage(string nick, string status, string page, string start, string end)
        {
            Api top = new Api(AppKey, Secret, Session, Url);

            IDictionary<string, string> param = new Dictionary<string, string>();
            param.Add("status", status);
            if (nick != "")
            {
                param.Add("nick", nick);
            }
            param.Add("start_modified", start);
            param.Add("end_modified", end);
            param.Add("page_no", page);
            param.Add("page_size", "200");

            string result = top.CommonTopApi("taobao.increment.trades.get", param, Session);

            return result;
        }

        /// <summary>
        /// 获取用户订单对应的评价记录
        /// </summary>
        /// <param name="buyNick"></param>
        /// <param name="couponId"></param>
        /// <returns></returns>
        public TradeRate GetTradeRate(Trade trade)
        {
            Api top = new Api(AppKey, Secret, Session, Url);

            IDictionary<string, string> param = new Dictionary<string, string>();
            param.Add("fields", "content,created,nick,result");
            param.Add("rate_type", "get");
            param.Add("role", "buyer");
            param.Add("tid", trade.Tid);

            string result = top.CommonTopApi("taobao.traderates.get", param, Session);
            //如果主订单没有评价记录则获取子订单的评价记录
            //如果买家先评则有通知但是获取不到买家评价
            //如果是卖家先评则有通知只能取到卖家的评价，不做记录
            TradeRate tradeRate = new TradeRate();
            tradeRate.Tid = trade.Tid;
            tradeRate.Oid = trade.Oid;
            tradeRate.Content = utils.GetValueByProperty(result, "content");
            tradeRate.Created = utils.GetValueByProperty(result, "created");
            tradeRate.BuyNick = utils.GetValueByProperty(result, "nick");
            tradeRate.Result = utils.GetValueByProperty(result, "result");

            return tradeRate;
        }


        public string AppKey { get; set; }
        public string Secret { get; set; }
        public string Session { get; set; }
        public string Url { get; set; }
    }
}
