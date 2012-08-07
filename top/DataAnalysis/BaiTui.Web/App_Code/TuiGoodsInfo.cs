using System;

/// <summary>
/// 推广的商品信息
/// </summary>
public class TuiGoodsInfo
{
    public Guid TuiId { set; get; }

    public string GoodsPic { set; get; }

    public string GoodsId { set; get; }

    public string GoodsName { set; get; }

    public string Keywords { set; get; }

    public string Nick { set; get; }

    /// <summary>
    /// 百度：1，QQ：2
    /// </summary>
    public int Type { set; get; }
}
