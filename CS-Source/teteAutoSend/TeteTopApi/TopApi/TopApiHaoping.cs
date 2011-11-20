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
            param.Add("fields", "receiver_mobile, orders.num_iid, created, consign_time");
            param.Add("tid", simpleTrade.Tid);

            string result = top.CommonTopApi("taobao.trade.fullinfo.get", param, Session);
            Console.Write(result);
            simpleTrade.Mobile = utils.GetValueByProperty(result, "receiver_mobile");
            simpleTrade.NumIid = utils.GetValueByPropertyNum(result, "num_iid");
            simpleTrade.Created = utils.GetValueByProperty(result, "created");
            simpleTrade.SendTime = utils.GetValueByProperty(result, "consign_time");

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

            IDictionary<string, string> param = new Dictionary<string, string>();
            param.Add("fields", "delivery_start,delivery_end,status");
            param.Add("tid", simpleTrade.Tid);

            string result = top.CommonTopApi("taobao.logistics.orders.get", param, Session);
            result = utils.GetValueByProperty(result, "status");

            return result;
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
        /// 向指定用户赠送优惠券
        /// </summary>
        /// <param name="buyNick"></param>
        /// <param name="couponId"></param>
        /// <returns></returns>
        public string SendCoupon(string buyNick, string couponId)
        {
            Api top = new Api(AppKey, Secret, Session, Url);

            IDictionary<string, string> param = new Dictionary<string, string>();
            param.Add("coupon_id", couponId);
            param.Add("buyer_nick", buyNick);

            string result = top.CommonTopApi("taobao.promotion.coupon.send", param, Session);
            result = utils.GetValueByPropertyNum(result, "coupon_number");

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
