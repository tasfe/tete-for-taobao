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
/// 好评记录
/// </summary>
public class GoodPingInfo
{
    public String Nick { set; get; }

    /// <summary>
    /// 好评次数(赠送次数)
    /// </summary>
    public int PingTimes { set; get; }

    public DateTime PingDate { set; get; }

    public String AddIP { set; get; }
}
