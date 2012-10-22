using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeteTopApi.Entity
{
    public class TradeList
    {
        /// <summary>
        /// 主订单号
        /// </summary>
        public string Tid { get; set; }

        /// <summary>
        /// 子订单号
        /// </summary>
        public string Oid { get; set; }

        /// <summary>
        /// 评价结果
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 是否评价完毕
        /// </summary>
        public string IsOK { get; set; }

        /// <summary>
        /// 评价时间
        /// </summary>
        public string ReviewDate { get; set; }

        /// <summary>
        /// 评价内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 卖家名
        /// </summary>
        public string Nick { get; set; }

        /// <summary>
        /// 买家名
        /// </summary>
        public string BuyNick { get; set; }
    }
}
