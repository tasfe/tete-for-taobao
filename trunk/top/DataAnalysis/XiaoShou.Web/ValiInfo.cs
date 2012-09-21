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
///ValiInfo 的摘要说明
/// </summary>
public class ValiInfo
{
    /// <summary>
    /// 店铺ID
    /// </summary>
    public string taobao_user_id { set; get; }

    /// <summary>
    /// 店铺nick
    /// </summary>
    public string taobao_user_nick { set; get; }

    /// <summary>
    /// 更新token
    /// </summary>
    public string refresh_token { set; get; }

    /// <summary>
    /// 授权session
    /// </summary>
    public string access_token { set; get; }
}
