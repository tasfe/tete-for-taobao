using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeteTopApi.Entity
{
    public class Alipay
    {
        /// <summary>
        /// 每人最大领取数量
        /// </summary>
        public string Per { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public string Num { get; set; }

        /// <summary>
        /// 红包最大赠送数量
        /// </summary>
        public string Count { get; set; }

        /// <summary>
        /// 红包结束日期
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// 支付宝红包ID
        /// </summary>
        public string GUID { get; set; }
    }
}
