﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeteTopApi.Entity;
using TeteTopApi.Logic;

namespace TeteTopApi
{
    public class ReceiveMessage
    {
        /// <summary>
        /// 构造函数，获取消息正文
        /// </summary>
        /// <param name="msg"></param>
        public ReceiveMessage(string msg)
        {
            this.Msg = msg;
        }

        /// <summary>
        /// 根据消息内容做出相应的逻辑处理
        /// </summary>
        public void ActData()
        {
            string typ = utils.GetMsgType(this.Msg);

            switch (typ)
            { 
                case "notify_trade":
                    ActOrderInfo();
                    break;
            }
        }

        /// <summary>
        /// 处理订单类型的数据
        /// </summary>
        private void ActOrderInfo()
        {
            Trade trade = utils.GetTrade(this.Msg);

            switch (trade.Status)
            {
                case "TradeSellerShip":
                    ActTradeSellerShip(trade);
                    break;
                case "TradeSuccess":
                    ActTradeSuccess(trade);
                    break;
                case "TradeRated":
                    ActTradeSuccess(trade);
                    break;
            }
        }

        /// <summary>
        /// 处理订单发货时的逻辑
        /// </summary>
        private void ActTradeSellerShip(Trade trade)
        {
            //获取好评，赠送优惠券
            Console.Write("[" + trade.Nick + "]-" + trade.BuyNick + "-" + trade.Tid + "-" + trade.Status + "\r\n");
            
            //判断是否有发货时的短信通知
            TradeSellerShip act = new TradeSellerShip(trade);
            act.Start();
        }

        /// <summary>
        /// 处理订单完成时的逻辑
        /// </summary>
        private void ActTradeSuccess(Trade trade)
        {
            //获取好评，赠送优惠券
            Console.Write("[" + trade.Nick + "]-" + trade.BuyNick + "-" + trade.Tid + "-" + trade.Status + "\r\n");
            
            //处理逻辑开始
            TradeSuccess act = new TradeSuccess(trade);
            act.Start();
        }

        /// <summary>
        /// JSON字符串
        /// </summary>
        private string Msg { get; set; }
    }
}
