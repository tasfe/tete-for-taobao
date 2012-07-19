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
