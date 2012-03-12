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
    const string SQL_SELECT_ORDER_BYDAY = "SELECT SiteTotalDate,SitePVCount,SiteUVCount,SiteOrderCount,SiteOrderPay,SiteUVBack,SiteGoodsCount,SitePostFee,SiteSecondBuy,SiteBuyCustomTotal FROM TopSiteTotal WHERE SiteNick=@SiteNick AND SiteTotalDate BETWEEN @start AND @end";

    const string SQL_SELECT_SUM_SITETOTAL = @"SELECT sum(SitePVCount) AS sSitePVCount,sum(SiteUVCount) as sSiteUVCount,sum(SiteOrderCount) as sSiteOrderCount,sum(SiteOrderPay) as sSiteOrderPay,sum(SiteUVBack) as sSiteUVBack,sum(SiteGoodsCount) as sSiteGoodsCount,sum(SitePostFee) as sSitePostFee,sum(SiteSecondBuy) as sSiteSecondBuy,sum(SiteBuyCustomTotal) as sSiteBuyCustomTotal FROM 
  (select * from TopSiteTotal where SiteNick=@nick AND 
  SiteTotalDate BETWEEN @start AND @end) a";

    const string SQL_SELECT_OTOTAL_MONTH = @"select sum(SitePVCount) as sSitePVCount,sum(SiteUVCount) as sSiteUVCount,sum(SiteOrderCount) as sSiteOrderCount,sum(SiteGoodsCount) as sSiteGoodsCount,
            sum(SiteOrderPay) as aSiteOrderPay,sum(SitePostFee) as sSitePostFee,sum(SiteUVBack) as sSiteUVBack,sum(SiteSecondBuy) as sSiteSecondBuy,sum(SiteBuyCustomTotal) as sSiteBuyCustomTotal,
            substring(SiteTotalDate,0,7) as stdate from
            (
            select * from TopSiteTotal 
            where sitenick=@sitenick and substring(SiteTotalDate,0,5)=@year
            ) a group by substring(SiteTotalDate,0,7)";

                const string SQL_SELECT_OTOTAL_YEAR = @"select sum(SitePVCount),sum(SiteUVCount),sum(SiteOrderCount),sum(SiteGoodsCount),
            sum(SiteOrderPay),sum(SitePostFee),sum(SiteUVBack),sum(SiteSecondBuy),sum(SiteBuyCustomTotal),
            substring(SiteTotalDate,0,5) as stdate from
            (
            select * from dbo.TopSiteTotal 
            where sitenick=@sitenick
            ) a group by substring(SiteTotalDate,0,5)";

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
            
            info.SiteSecondBuy = int.Parse(dr["SiteSecondBuy"].ToString());
            info.PostFee =decimal.Parse(dr["SitePostFee"].ToString());
            info.GoodsCount = int.Parse(dr["SiteGoodsCount"].ToString());
            info.SiteBuyCustomTotal = int.Parse(dr["SiteBuyCustomTotal"].ToString());

            list.Add(info);
        }

        return list;
    }

    public TopSiteTotalInfo GetOrderTotalInfo(DateTime start, DateTime end, string nick)
    {
        TopSiteTotalInfo info = new TopSiteTotalInfo();
        SqlParameter[] param = new[]
        {
            new SqlParameter("@start",start.ToString("yyyyMMdd")),
            new SqlParameter("@end",end.ToString("yyyyMMdd")),
            new SqlParameter("@nick",nick)
        };

        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_SUM_SITETOTAL, param);
        foreach (DataRow dr in dt.Rows)
        {
            //info.SiteTotalDate = dr["sSiteTotalDate"].ToString();
            info.SitePVCount = dr["sSitePVCount"] == DBNull.Value ? 0 : int.Parse(dr["sSitePVCount"].ToString());

            info.SiteUVCount = dr["sSiteUVCount"] == DBNull.Value ? 0 : int.Parse(dr["sSiteUVCount"].ToString());
            info.SiteOrderCount = dr["sSiteOrderCount"] == DBNull.Value ? 0 : int.Parse(dr["sSiteOrderCount"].ToString());
            info.SiteOrderPay = dr["sSiteOrderPay"] == DBNull.Value ? 0 : decimal.Parse(dr["sSiteOrderPay"].ToString());
            info.SiteUVBack = dr["sSiteUVBack"] == DBNull.Value ? 0 : int.Parse(dr["sSiteUVBack"].ToString());

            info.SiteSecondBuy = dr["sSiteSecondBuy"] == DBNull.Value ? 0 : int.Parse(dr["sSiteSecondBuy"].ToString());
            info.PostFee = dr["sSitePostFee"] == DBNull.Value ? 0 : decimal.Parse(dr["sSitePostFee"].ToString());
            info.GoodsCount = dr["sSiteGoodsCount"] == DBNull.Value ? 0 : int.Parse(dr["sSiteGoodsCount"].ToString());
            info.SiteBuyCustomTotal = dr["sSiteBuyCustomTotal"] == DBNull.Value ? 0 : int.Parse(dr["sSiteBuyCustomTotal"].ToString());
        }
        return info;
    }

    public List<TopSiteTotalInfo> GetMonthOrderTotalInfoList(string nick, string year)
    {
        TopSiteTotalInfo info = new TopSiteTotalInfo();
        SqlParameter[] param = new[]
        {
            new SqlParameter("@year",year),
            new SqlParameter("@sitenick",nick)
        };
        List<TopSiteTotalInfo> list = new List<TopSiteTotalInfo>();
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_OTOTAL_MONTH, param);
        foreach (DataRow dr in dt.Rows)
        {
            info.SiteTotalDate = dr["stdate"].ToString();
            info.SitePVCount = dr["sSitePVCount"] == DBNull.Value ? 0 : int.Parse(dr["sSitePVCount"].ToString());

            info.SiteUVCount = dr["sSiteUVCount"] == DBNull.Value ? 0 : int.Parse(dr["sSiteUVCount"].ToString());
            info.SiteOrderCount = dr["sSiteOrderCount"] == DBNull.Value ? 0 : int.Parse(dr["sSiteOrderCount"].ToString());
            info.SiteOrderPay = dr["sSiteOrderPay"] == DBNull.Value ? 0 : decimal.Parse(dr["sSiteOrderPay"].ToString());
            info.SiteUVBack = dr["sSiteUVBack"] == DBNull.Value ? 0 : int.Parse(dr["sSiteUVBack"].ToString());

            info.SiteSecondBuy = dr["sSiteSecondBuy"] == DBNull.Value ? 0 : int.Parse(dr["sSiteSecondBuy"].ToString());
            info.PostFee = dr["sSitePostFee"] == DBNull.Value ? 0 : decimal.Parse(dr["sSitePostFee"].ToString());
            info.GoodsCount = dr["sSiteGoodsCount"] == DBNull.Value ? 0 : int.Parse(dr["sSiteGoodsCount"].ToString());
            info.SiteBuyCustomTotal = dr["sSiteBuyCustomTotal"] == DBNull.Value ? 0 : int.Parse(dr["sSiteBuyCustomTotal"].ToString());
            list.Add(info);
        }
        return list;
    }

    public List<TopSiteTotalInfo> GetYearOrderTotalInfoList(string nick)
    {
        TopSiteTotalInfo info = new TopSiteTotalInfo();
        SqlParameter[] param = new[]
        {
            new SqlParameter("@sitenick",nick)
        };
        List<TopSiteTotalInfo> list = new List<TopSiteTotalInfo>();
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_OTOTAL_YEAR, param);
        foreach (DataRow dr in dt.Rows)
        {
            info.SiteTotalDate = dr["stdate"].ToString();
            info.SitePVCount = dr["sSitePVCount"] == DBNull.Value ? 0 : int.Parse(dr["sSitePVCount"].ToString());

            info.SiteUVCount = dr["sSiteUVCount"] == DBNull.Value ? 0 : int.Parse(dr["sSiteUVCount"].ToString());
            info.SiteOrderCount = dr["sSiteOrderCount"] == DBNull.Value ? 0 : int.Parse(dr["sSiteOrderCount"].ToString());
            info.SiteOrderPay = dr["sSiteOrderPay"] == DBNull.Value ? 0 : decimal.Parse(dr["sSiteOrderPay"].ToString());
            info.SiteUVBack = dr["sSiteUVBack"] == DBNull.Value ? 0 : int.Parse(dr["sSiteUVBack"].ToString());

            info.SiteSecondBuy = dr["sSiteSecondBuy"] == DBNull.Value ? 0 : int.Parse(dr["sSiteSecondBuy"].ToString());
            info.PostFee = dr["sSitePostFee"] == DBNull.Value ? 0 : decimal.Parse(dr["sSitePostFee"].ToString());
            info.GoodsCount = dr["sSiteGoodsCount"] == DBNull.Value ? 0 : int.Parse(dr["sSiteGoodsCount"].ToString());
            info.SiteBuyCustomTotal = dr["sSiteBuyCustomTotal"] == DBNull.Value ? 0 : int.Parse(dr["sSiteBuyCustomTotal"].ToString());
            list.Add(info);
        }
        return list;
    }
}
