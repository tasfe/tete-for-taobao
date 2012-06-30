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
/// 访问IP信息
/// </summary>
[Serializable]
public class ClickIPInfo
{
    public Guid UserAdsId { set; get; }

    public String VisitIP { set; get; }

    public String VisitDate { set; get; }

    public Guid ClickId { set; get; }

    public override bool Equals(object obj)
    {
        ClickIPInfo info = obj as ClickIPInfo;
        if (info == null)
            return false;
        if (info.VisitIP == this.VisitIP)
            return true;
        return base.Equals(obj);
    }
}
