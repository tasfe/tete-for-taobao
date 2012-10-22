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
///ShopTotalInfo 的摘要说明
/// </summary>
public class ShopTotalInfo
{
    public string ShopId { set; get; }

    public DateTime AddTime { set; get; }

    public ShopTotalInfo() { }

    public ShopTotalInfo(string shopId, DateTime addtime)
    {
        ShopId = shopId;
        AddTime = addtime;
    }
}
