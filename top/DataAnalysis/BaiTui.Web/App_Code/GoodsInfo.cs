using System;

/// <summary>
/// 商品信息
/// </summary>
public class GoodsInfo
{

    public String GoodsId { set; get; }

    public String GoodsName { set; get; }

    public decimal GoodsPrice { set; get; }

    public int GoodsCount { set; get; }

    public String GoodsPic { set; get; }

    public DateTime Modified { set; get; }

    /// <summary>
    /// 所属类目ID
    /// </summary>
    public String CateId { set; get; }

    /// <summary>
    /// 淘宝ID
    /// </summary>
    public String TaoBaoCId { set; get; }
}
