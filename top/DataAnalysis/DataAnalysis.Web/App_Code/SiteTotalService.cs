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
using System.Collections.Generic;
using System.Data.SqlClient;

/// <summary>
/// Summary description for SiteTotalService
/// </summary>
public class SiteTotalService
{
    const string SQL_SELECT_ORDER_BYDAY = "SELECT SiteTotalDate,SitePVCount,SiteUVCount,SiteOrderCount,SiteOrderPay,SiteUVBack FROM TopSiteTotal WHERE SiteNick=@SiteNick AND SiteTotalDate BETWEEN @start AND @end";

    public IList<TopSiteTotalInfo> GetNickOrderTotal(DateTime start,DateTime end, string nick)
    {
        IList<TopSiteTotalInfo> list = new List<TopSiteTotalInfo>();

        SqlParameter[] param = new[]
        {
            new SqlParameter("@start",start.ToString("yyyyMMdd")),
            new SqlParameter("@end",end.ToString("yyyyMMdd")),
            new SqlParameter("@SiteNick",nick)
        };
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_ORDER_BYDAY, param);

        foreach (DataRow dr in dt.Rows)
        {
            TopSiteTotalInfo info = new TopSiteTotalInfo();
            info.SiteTotalDate = dr["SiteTotalDate"].ToString();
            info.SitePVCount = int.Parse(dr["SitePVCount"].ToString());

            info.SiteUVCount = int.Parse(dr["SiteUVCount"].ToString());
            info.SiteOrderCount = int.Parse(dr["SiteOrderCount"].ToString());
            info.SiteOrderPay = decimal.Parse(dr["SiteOrderPay"].ToString());
            info.SiteUVBack = int.Parse(dr["SiteUVBack"].ToString());
            list.Add(info);
        }

        return list;
    }

    public TopSiteTotalInfo GetOrderTotalInfo(string date, string nick)
    {
        TopSiteTotalInfo info = null;
        SqlParameter[] param = new[]
        {
            new SqlParameter("@start",date),
            new SqlParameter("@SiteNick",nick)
        };
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_ORDER_BYDAY, param);

        foreach (DataRow dr in dt.Rows)
        {
            info = new TopSiteTotalInfo();
            info.SiteTotalDate = dr["SiteTotalDate"].ToString();
            info.SitePVCount = int.Parse(dr["SitePVCount"].ToString());

            info.SiteUVCount = int.Parse(dr["SiteUVCount"].ToString());
            info.SiteOrderCount = int.Parse(dr["SiteOrderCount"].ToString());
            info.SiteOrderPay = decimal.Parse(dr["SiteOrderPay"].ToString());
            info.SiteUVBack = int.Parse(dr["SiteUVBack"].ToString());
        }
        return info;
    }
}
