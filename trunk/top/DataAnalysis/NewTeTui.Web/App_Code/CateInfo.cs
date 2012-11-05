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
/// 商家所售商品类目信息
/// </summary>
public class CateInfo
{
    public String CateId { set; get; }

    public String CateName { set; get; }

    public String ParentId { set; get; }

    public String Nick { set; get; }
}
