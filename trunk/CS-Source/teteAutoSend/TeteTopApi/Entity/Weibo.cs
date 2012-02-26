using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeteTopApi.Entity
{
    public class Weibo
    {
        /// <summary>
        /// 卖出宝贝发送的内容
        /// </summary>
        public string ContentSell { get; set; }

        /// <summary>
        /// 买家评价发出的内容
        /// </summary>
        public string ContentReview { get; set; }

        /// <summary>
        /// 新商品上架发出的内容
        /// </summary>
        public string ContentUp { get; set; }

        /// <summary>
        /// 橱窗推荐发出的内容
        /// </summary>
        public string ContentRecommend { get; set; }
    }
}
