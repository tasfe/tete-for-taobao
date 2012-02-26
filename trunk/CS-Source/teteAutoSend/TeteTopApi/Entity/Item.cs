using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeteTopApi.Entity
{
    public class Item
    {
        /// <summary>
        /// 宝贝ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 宝贝名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 宝贝价格
        /// </summary>
        public string Price { get; set; }

        /// <summary>
        /// 宝贝链接
        /// </summary>
        public string ItemUrl { get; set; }

        /// <summary>
        /// 宝贝图片
        /// </summary>
        public string ImageUrl { get; set; }
    }
}
