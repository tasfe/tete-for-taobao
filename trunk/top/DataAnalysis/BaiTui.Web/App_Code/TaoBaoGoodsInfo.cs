using System;

/// <summary>
/// 淘宝商品返回数据列名
/// </summary>
public class TaoBaoGoodsInfo
{
    public string num_iid { set; get; }

    public string title { set; get; }

    public decimal price { set; get; }

    public int num { set; get; }

    public string pic_url { set; get; }

    public DateTime modified { set; get; }

    public string cid { set; get; }

    public string seller_cids { set; get; }

}
