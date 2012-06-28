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
/// 淘宝店铺信息
/// </summary>
public class TaoBaoShopInfo
{
    public String Name { set; get; }

    public String Description { set; get; }

    public String Nick { set; get; }

    public String ShopLogo { set; get; }

    public String ShopId { set; get; }

    public String CateId { set; get; }
}
