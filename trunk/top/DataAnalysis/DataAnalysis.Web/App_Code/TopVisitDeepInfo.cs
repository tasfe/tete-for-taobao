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
/// 访问深度
/// </summary>
public class TopVisitDeepInfo
{
	public TopVisitDeepInfo()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    /// <summary>
    /// 主键ID
    /// </summary>
    public Guid TopVisitDeepId { set; get; }

    /// <summary>
    /// 访问日期(yyyyMMdd)
    /// </summary>
    public string TopVisitDate { set; get; }

    /// <summary>
    /// 店铺ID
    /// </summary>
    public string TopVisitShopId { set; get; }

    /// <summary>
    /// 访问页面数
    /// </summary>
    public int TopVisitPV { set; get; }

    /// <summary>
    /// 访问IP
    /// </summary>
    public string TopVisitIP { set; get; }

    /// <summary>
    /// 访问者客户端cookie
    /// </summary>
    public Guid TopVisitRandomNumber { set; get; } 
}
