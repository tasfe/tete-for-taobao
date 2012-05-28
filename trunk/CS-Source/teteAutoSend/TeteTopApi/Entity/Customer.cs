using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeteTopApi.Entity
{
    public class Customer
    {
        /// <summary>
        /// 客户唯一GUID
        /// </summary>
        public string GUID { get; set; }

        /// <summary>
        /// 卖家ID
        /// </summary>
        public string Nick { get; set; }

        /// <summary>
        /// 买家ID
        /// </summary>
        public string BuyNick { get; set; }

        /// <summary>
        /// 买家手机
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 买家真实姓名
        /// </summary>
        public string TrueName { get; set; }

        /// <summary>
        /// 买家地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 交易笔数
        /// </summary>
        public string TradeCount { get; set; }

        /// <summary>
        /// 交易总金额
        /// </summary>
        public string TradeAmount { get; set; }

        /// <summary>
        /// 用户组ID
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// 最近一次交易时间
        /// </summary>
        public string LastOrderDate { get; set; }

        /// <summary>
        /// 会员资料-省
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 会员资料-市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        public string Sheng { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public string Shi { get; set; }

        /// <summary>
        /// 区
        /// </summary>
        public string Qu { get; set; }

        /// <summary>
        /// 平均订单价格
        /// </summary>
        public string AvgPrice { get; set; }

        /// <summary>
        /// 来源ID
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 会员ID
        /// </summary>
        public string BuyerId { get; set; }

        /// <summary>
        /// 会员等级
        /// </summary>
        public string BuyerLevel { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// 会员创建时间
        /// </summary>
        public string Created { get; set; }

        /// <summary>
        /// 上次登录
        /// </summary>
        public string LastLogin { get; set; }

        /// <summary>
        /// 会员生日
        /// </summary>
        public string BirthDay { get; set; }

        /// <summary>
        /// 会员邮件
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 评论次数
        /// </summary>
        public string ReviewCount { get; set; }

        /// <summary>
        /// 优惠券赠送次数
        /// </summary>
        public string GiftCount { get; set; }

        /// <summary>
        /// 优惠券使用次数
        /// </summary>
        public string CouponCount { get; set; }
        
        /// <summary>
        /// 支付宝红包赠送次数
        /// </summary>
        public string AlipayCount { get; set; }

        /// <summary>
        /// 会员等级，0：无会员等级，1：普通会员，2：高级会员，3：VIP会员， 4：至尊VIP会员
        /// </summary>
        public string Grade { get; set; }
    }
}
