using System;
using System.Collections.Generic;

/// <summary>
/// Summary description for GoodsOrderInfo
/// </summary>
public class GoodsOrderInfo
{
    /// <summary>
    ///  商品总价
    /// </summary>
    public decimal total_fee { set; get; } //decimal

    /// <summary>
    /// 收货省份
    /// </summary>
    public string receiver_state { set; get; }

    /// <summary>
    /// 收货城市
    /// </summary>
    public string receiver_city { set; get; }

    /// <summary>
    /// 交易佣金
    /// </summary>
    public string commission_fee { set; get; } //decimal

    /// <summary>
    /// 实际金额
    /// </summary>
    public decimal payment { set; get; }//decimal

    /// <summary>
    /// 货到付款服务费
    /// </summary>
    public string cod_fee { set; get; }//decimal

    /// <summary>
    /// 交易结束时间
    /// </summary>
    public DateTime end_time { set; get; } //DateTime

    /// <summary>
    /// 付款时间
    /// </summary>
    public DateTime pay_time { set; get; } //DateTime

    /// <summary>
    /// 交易创建时间
    /// </summary>
    public DateTime created { set; get; } //DateTime

    /// <summary>
    /// 商品数量
    /// </summary>
    // public int  num {set;get;}

    /// <summary>
    /// 商品编号
    /// </summary>
    //public string num_iid { set; get; }

    /// <summary>
    /// 自订单状态
    /// </summary>
    // public string status { set; get; }

    /// <summary>
    /// 邮费
    /// </summary>
    public decimal post_fee { set; get; } //decimal

    /// <summary>
    /// 交易编号 (父订单的交易编号)
    /// </summary>
    public string tid { set; get; }

    /// <summary>
    /// 卖家nickNo
    /// </summary>
    public string seller_nick { set; get; }

    //public string orders { set; get; }

    public List<ChildOrderInfo> orders { set; get; }

    /// <summary>
    /// 是否使用店内优惠
    /// </summary>
    public bool UsePromotion { set; get; }

    public PingJiaInfo PingInfo { set; get; }
}

/// <summary>
/// 子订单信息
/// </summary>
public class ChildOrderInfo
{
    public ChildOrderInfo(){}

    public string status { set; get; }

    public int num { set; get; }

    public string num_iid { set; get; }
}

/// <summary>
/// 评价信息(针对子订单)
/// </summary>
public class PingJiaInfo
{
    /// <summary>
    /// 评价时间
    /// </summary>
    public DateTime created { set; get; }

    /// <summary>
    /// 评价人
    /// </summary>
    public string nick { set; get; }

    /// <summary>
    ///  评价内容
    /// </summary>
    public string content { set; get; }

    /// <summary>
    ///  好中差评good(好评),neutral(中评),bad(差评)
    /// </summary>
    public string result { set; get; }
}