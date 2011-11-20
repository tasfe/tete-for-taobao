using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeteTopApi.Entity
{
    /// <summary>
    /// 优惠券相关信息
    /// </summary>
    public class Coupon
    {
        /// <summary>
        /// 每人最大领取数量
        /// </summary>
        public string Per { get; set; }

        /// <summary>
        /// 优惠券ID
        /// </summary>
        public string CouponId { get; set; }
    }
}
