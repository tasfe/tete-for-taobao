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
/// 用户订购信息
/// </summary>
public class BuyInfo
{
    public Guid FeeId { set; get; }

    public String Nick { set; get; }

    public DateTime BuyTime { set; get; }

    public bool IsExpied { set; get; }
}
