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
        /// 支付宝红包ID
        /// </summary>
        public string AlipayID { get; set; }

        /// <summary>
        /// 最小评价时间（物流到货）
        /// </summary>
        public string MinDateSystem { get; set; }

        /// <summary>
        /// 最小评价时间（其他不可查询类）
        /// </summary>
        public string MinDateSelf { get; set; }

        /// <summary>
        /// 是否为虚拟商品
        /// </summary>
        public string IsXuni { get; set; }

        /// <summary>
        /// 虚拟商品最迟评价天数
        /// </summary>
        public string XuniDate { get; set; }

        /// <summary>
        /// 客服审核开关
        /// </summary>
        public string IsKefu { get; set; }

        /// <summary>
        /// 优惠券自动赠送开关
        /// </summary>
        public string IsCoupon { get; set; }

        /// <summary>
        /// 支付宝红包赠送开关
        /// </summary>
        public string IsAlipay { get; set; }

        /// <summary>
        /// 系统自动评价或者短评价不赠送开关
        /// </summary>
        public string IsCancelAuto { get; set; }

        /// <summary>
        /// 好评！
        /// </summary>
        public string Cancel1 { get; set; }

        /// <summary>
        /// 评价方未及时做出评价,系统默认好评！
        /// </summary>
        public string Cancel2 { get; set; }

        /// <summary>
        /// 是否开启包邮卡赠送
        /// </summary>
        public string IsFreeCard { get; set; }

        /// <summary>
        /// 包邮卡id
        /// </summary>
        public string FreeCardId { get; set; }

        /// <summary>
        /// 是否开启指定商品赠送
        /// </summary>
        public string IsItem { get; set; }

        /// <summary>
        /// 指定商品清单，多个numiid用逗号分开
        /// </summary>
        public string ItemList { get; set; }

        /// <summary>
        /// 是否开启订单商品判断
        /// </summary>
        public string IsIncludeProduct { get; set; }

        /// <summary>
        /// 订单商品判断类型，0包含商品就送，1不包含商品就送
        /// </summary>
        public string IncludeProductFlag { get; set; }

        /// <summary>
        /// 包含商品清单，多个商品ID用英文逗号分开
        /// </summary>
        public string IncludeProductList { get; set; }

        /// <summary>
        /// 发货短信开关
        /// </summary>
        public string MsgIsFahuo { get; set; }

        /// <summary>
        /// 发货短信内容
        /// </summary>
        public string MsgFahuoContent { get; set; }

        /// <summary>
        /// 包邮卡短信开关
        /// </summary>
        public string MsgIsFreecard { get; set; }

        /// <summary>
        /// 包邮卡短信内容
        /// </summary>
        public string MsgFreecardContent { get; set; }

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
        /// 是否为判断差评
        /// </summary>
        public string KeywordIsBad { get; set; }

        /// <summary>
        /// 好评判断关键字
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 差评判断关键字
        /// </summary>
        public string BadKeyword { get; set; }

        /// <summary>
        /// 好评判断评价字数长度
        /// </summary>
        public string WordCount { get; set; }

        /// <summary>
        /// 卖家手机号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 是否开启催单有礼
        /// </summary>
        public string IsCui { get; set; }

        /// <summary>
        /// 催单有礼赠送免邮卡ID
        /// </summary>
        public string CuiFreeCard { get; set; }

        /// <summary>
        /// 催单有礼赠送优惠券ID
        /// </summary>
        public string CuiCouponId { get; set; }

        /// <summary>
        /// 催单有礼赠送支付宝红包ID
        /// </summary>
        public string CuiAlipayId { get; set; }

        /// <summary>
        /// 卖家订购信息
        /// </summary>
        public string Plus { get; set; }

        /// <summary>
        /// 卖家CRM短信内容
        /// </summary>
        public string MissionContent { get; set; }

        /// <summary>
        /// 卖家CRM买家回访天数
        /// </summary>
        public string MissionBackDay { get; set; }
    }
}
