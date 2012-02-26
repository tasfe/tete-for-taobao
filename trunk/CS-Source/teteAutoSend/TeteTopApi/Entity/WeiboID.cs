using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeteTopApi.Entity
{
    public class WeiboID
    {
        /// <summary>
        /// 微博类型
        /// </summary>
        public string Typ { get; set; }

        /// <summary>
        /// 微博ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 微博授权码-key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 微博授权码-secret
        /// </summary>
        public string Secret { get; set; }
    }
}
