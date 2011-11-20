using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeteTopApi.Entity
{
    /// <summary>
    /// 交易评价的数据实体
    /// </summary>
    public class TradeRate
    {
        /// <summary>
        /// 订单主ID
        /// </summary>
        public string Tid { get; set; }

        /// <summary>
        /// 子订单ID
        /// </summary>
        public string Oid { get; set; }

        /// <summary>
        /// 评价内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 评价时间
        /// </summary>
        public string Created { get; set; }

        /// <summary>
        /// 卖家ID
        /// </summary>
        public string Nick { get; set; }

        /// <summary>
        /// 买家ID
        /// </summary>
        public string BuyNick { get; set; }

        /// <summary>
        /// 评论对应宝贝ID
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// 评论结果(好，中，差)
        /// </summary>
        public string Result { get; set; }
    }
}
