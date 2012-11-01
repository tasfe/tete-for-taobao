using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class visitor_getvisit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string id = Request.QueryString["id"];
            if (string.IsNullOrEmpty(id))
            {
                Response.Write("请传入id");
                Response.End();
                return;
            }
            int s;
            if (!int.TryParse(id, out s))
            {
                Response.Write("id不合法");
                Response.End();
                return;
            }

            string refer = Request.QueryString["lasturl"];

            InsertInfo(CreateVisitInfo(refer));
        }
    }

    private TopVisitInfo CreateVisitInfo(string refer)
    {
        TopVisitInfo info = new TopVisitInfo();
        info.VisitID = Guid.NewGuid();
        info.VisitIP = GetIPAddress();
        info.VisitTime = GetVisitTime();
        info.VisitUrl = GetUrl();
        info.VisitUserAgent = GetUserAgent();
        info.VisitBrower = GetBrower();
        info.VisitOSLanguage = GetOSLanguage();
        info.VisitShopId = HttpUtility.UrlDecode(Request.QueryString["id"]);  // "234543534"
        info.LastURL = refer;

        info.GoodsId = "";
        info.GoodsClassId = "";

        return info;
    }

    public string GetUrl()
    {
        if (null == Request.UrlReferrer)
            return "";
        return Request.UrlReferrer.ToString();
    }

    public string GetIPAddress()
    {
        return Request.ServerVariables["REMOTE_ADDR"];
    }

    public DateTime GetVisitTime()
    {
        return DateTime.Now;
    }

    public string GetUserAgent()
    {
        return GetOSNameByUserAgent(Request.UserAgent);
    }

    //获取浏览器的类型及版本号
    public string GetBrower()
    {
        return Request.Browser.Browser + Request.Browser.Version;
    }

    //获取用户操作系统的语言
    public string GetOSLanguage()
    {
        return Request.UserLanguages[0];
    }

    //获取OS：
    private string GetOSNameByUserAgent(string userAgent)
    {
        string osVersion = "未知";
        if (userAgent.Contains("NT 6.1"))
        {
            osVersion = "Windows  7";
        }
        else if (userAgent.Contains("NT 6.0"))
        {
            osVersion = "Windows Vista/Server 2008";
        }
        else if (userAgent.Contains("NT 5.2"))
        {
            osVersion = "Windows Server 2003";
        }
        else if (userAgent.Contains("NT 5.1"))
        {
            osVersion = "Windows XP";
        }
        else if (userAgent.Contains("NT 5"))
        {
            osVersion = "Windows 2000";
        }
        else if (userAgent.Contains("Mac"))
        {
            osVersion = "Mac";
        }
        else if (userAgent.Contains("Unix"))
        {
            osVersion = "UNIX";
        }
        else if (userAgent.Contains("Linux"))
        {
            osVersion = "Linux";
        }
        else if (userAgent.Contains("SunOS"))
        {
            osVersion = "SunOS";
        }

        return osVersion;
    }

    private static void InsertInfo(TopVisitInfo info)
    {
        VisitService visitDal = new VisitService();

        //测试用
        string nickNo = BasePage.Encrypt(info.VisitShopId);
        visitDal.CreateTable(nickNo);

        visitDal.InsertVisitInfo(info, nickNo);
    }
}
