using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeteTopApi.Entity
{
    /// <summary>
    /// 交易信息实体
    /// </summary>
    public class Trade
    {
        /// <summary>
        /// 买家ID
        /// </summary>
        public string BuyNick { get; set; }

        /// <summary>
        /// 买家手机号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>
        public string Payment { get; set; }
        
        /// <summary>
        /// 掌柜ID
        /// </summary>
        public string Nick { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 子订单号
        /// </summary>
        public string Oid { get; set; }

        /// <summary>
        /// 主订单号
        /// </summary>
        public string Tid { get; set; }

        /// <summary>
        /// 买家下单所在区域
        /// </summary>
        public string BuyerArea { get; set; }

        /// <summary>
        /// 订单创建时间
        /// </summary>
        public string Created { get; set; }

        /// <summary>
        /// 订单修改时间
        /// </summary>
        public string Modified { get; set; }

        /// <summary>
        /// 商品id
        /// </summary>
        public string NumIid { get; set; }

        /// <summary>
        /// 多商品id集合
        /// </summary>
        public string[] NumIidList { get; set; }

        /// <summary>
        /// 配送方式(可查物流(system)还是不可查物流(self))
        /// </summary>
        public string ShippingType { get; set; }

        /// <summary>
        /// 发货时间
        /// </summary>
        public string SendTime { get; set; }

        /// <summary>
        /// 是否发送过催确认短信
        /// </summary>
        public string IsTellMsg { get; set; }

        /// <summary>
        /// 订单签收日期
        /// </summary>
        public string DeliveryEnd { get; set; }

        /// <summary>
        /// 物流信息原文，方便重复查看问题
        /// </summary>
        public string DeliveryMsg { get; set; }

        /// <summary>
        /// 物流公司缩写
        /// </summary>
        public string ShippingCompanyShort { get; set; }

        /// <summary>
        /// 物流公司名称
        /// </summary>
        public string ShippingCompanyName { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>
        public string ShippingNumber { get; set; }

        /// <summary>
        /// 是否使用了优惠券
        /// </summary>
        public string IsUseCoupon { get; set; }

        /// <summary>
        /// 优惠券的优惠金额
        /// </summary>
        public string CouponPrice { get; set; }

        /// <summary>
        /// 订单总金额
        /// </summary>
        public string OrderPrice { get; set; }

        /// <summary>
        /// 订单类型 fixed(一口价) auction(拍卖) guarantee_trade(一口价、拍卖) auto_delivery(自动发货) independent_simple_trade(旺店入门版交易) independent_shop_trade(旺店标准版交易) ec(直冲) cod(货到付款) fenxiao(分销) game_equipment(游戏装备) shopex_trade(ShopEX交易) netcn_trade(万网交易) external_trade(统一外部交易)
        /// </summary>
        public string OrderType { get; set; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string receiver_name { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        public string receiver_state { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public string receiver_city { get; set; }

        /// <summary>
        /// 区
        /// </summary>
        public string receiver_district { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string receiver_address { get; set; }
    }
}
