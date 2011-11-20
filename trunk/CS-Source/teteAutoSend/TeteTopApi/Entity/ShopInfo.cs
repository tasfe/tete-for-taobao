using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeteTopApi.Entity
{
    /// <summary>
    /// 卖家好评有礼相关信息
    /// </summary>
    public class ShopInfo
    {
        /// <summary>
        /// 卖家ID
        /// </summary>
        public string Nick { get; set; }

        /// <summary>
        /// 卖家授权session
        /// </summary>
        public string Session { get; set; }

        /// <summary>
        /// 卖家版本级别
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 卖家短信剩余数量
        /// </summary>
        public string MsgCount { get; set; }

        /// <summary>
        /// 卖家短信店铺标题
        /// </summary>
        public string MsgShopName { get; set; }

        /// <summary>
        /// 优惠券ID
        /// </summary>
        public string CouponID { get; set; }

        /// <summary>
        /// 最小评价时间（物流到货）
        /// </summary>
        public string MinDateSystem { get; set; }

        /// <summary>
        /// 最小评价时间（其他不可查询类）
        /// </summary>
        public string MinDateSelf { get; set; }

        /// <summary>
        /// 客服审核开关
        /// </summary>
        public string IsKefu { get; set; }

        /// <summary>
        /// 系统自动评价或者短评价不赠送开关
        /// </summary>
        public string IsCancelAuto { get; set; }

        /// <summary>
        /// 发货短信开关
        /// </summary>
        public string MsgIsFahuo { get; set; }

        /// <summary>
        /// 发货短信内容
        /// </summary>
        public string MsgFahuoContent { get; set; }

        /// <summary>
        /// 优惠券赠送短信开关
        /// </summary>
        public string MsgIsCoupon { get; set; }

        /// <summary>
        /// 优惠券赠送短信内容
        /// </summary>
        public string MsgCouponContent { get; set; }

        /// <summary>
        /// 物流到货短信开关
        /// </summary>
        public string MsgIsShipping { get; set; }

        /// <summary>
        /// 物流到货短信内容
        /// </summary>
        public string MsgShippingContent { get; set; }

        /// <summary>
        /// 延期未评短信开关
        /// </summary>
        public string MsgIsReview { get; set; }

        /// <summary>
        /// 延期未评短信内容
        /// </summary>
        public string MsgReviewContent { get; set; }

        /// <summary>
        /// 延期未评提醒时间
        /// </summary>
        public string MsgReviewTime { get; set; }

        /// <summary>
        /// 是否开启好评自动判断
        /// </summary>
        public string IsKeyword { get; set; }

        /// <summary>
        /// 好评判断关键字
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 好评判断评价字数长度
        /// </summary>
        public string WordCount { get; set; }
    }
}
