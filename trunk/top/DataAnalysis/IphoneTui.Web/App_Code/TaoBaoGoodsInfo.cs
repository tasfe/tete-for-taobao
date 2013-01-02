using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

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
