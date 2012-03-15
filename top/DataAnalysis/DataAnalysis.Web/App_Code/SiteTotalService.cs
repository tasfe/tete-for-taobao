using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

/// <summary>
/// Summary description for SiteTotalService
/// </summary>
public class SiteTotalService
{
    const string SQL_SELECT_ORDER_BYDAY = "SELECT SiteTotalDate,SitePVCount,SiteUVCount,SiteOrderCount,SiteOrderPay,SiteUVBack,SiteGoodsCount,SitePostFee,SiteSecondBuy,SiteBuyCustomTotal,SiteZhiTongTotal FROM TopSiteTotal WHERE SiteNick=@SiteNick AND SiteTotalDate BETWEEN @start AND @end";

    const string SQL_SELECT_SUM_SITETOTAL = @"SELECT sum(SitePVCount) AS sSitePVCount,sum(SiteUVCount) as sSiteUVCount,sum(SiteOrderCount) as sSiteOrderCount,sum(SiteOrderPay) as sSiteOrderPay,sum(SiteUVBack) as sSiteUVBack,sum(SiteGoodsCount) as sSiteGoodsCount,sum(SitePostFee) as sSitePostFee,sum(SiteSecondBuy) as sSiteSecondBuy,sum(SiteBuyCustomTotal) as sSiteBuyCustomTotal FROM 
  (select * from TopSiteTotal where SiteNick=@nick AND 
  SiteTotalDate BETWEEN @start AND @end) a";

    const string SQL_SELECT_OTOTAL_MONTH = @"select sum(SitePVCount) as sSitePVCount,sum(SiteUVCount) as sSiteUVCount,sum(SiteOrderCount) as sSiteOrderCount,sum(SiteGoodsCount) as sSiteGoodsCount,
            sum(SiteOrderPay) as sSiteOrderPay,sum(SitePostFee) as sSitePostFee,sum(SiteUVBack) as sSiteUVBack,sum(SiteSecondBuy) as sSiteSecondBuy,sum(SiteBuyCustomTotal) as sSiteBuyCustomTotal,
            substring(SiteTotalDate,0,7) as stdate from
            (
            select * from TopSiteTotal 
            where sitenick=@sitenick and substring(SiteTotalDate,0,5)=@year
            ) a group by substring(SiteTotalDate,0,7)";

    const string SQL_SELECT_OTOTAL_YEAR = @"select sum(SitePVCount) as sSitePVCount,sum(SiteUVCount) as sSiteUVCount,sum(SiteOrderCount) as sSiteOrderCount,sum(SiteGoodsCount) as sSiteGoodsCount,
            sum(SiteOrderPay) as sSiteOrderPay,sum(SitePostFee) as sSitePostFee,sum(SiteUVBack) as sSiteUVBack,sum(SiteSecondBuy) as sSiteSecondBuy,sum(SiteBuyCustomTotal) as sSiteBuyCustomTotal,
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
            info.ZhiTongFlow = int.Parse(dr["SiteZhiTongTotal"].ToString());

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
        SqlParameter[] param = new[]
        {
            new SqlParameter("@year",year),
            new SqlParameter("@sitenick",nick)
        };
        List<TopSiteTotalInfo> list = new List<TopSiteTotalInfo>();
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_OTOTAL_MONTH, param);
        foreach (DataRow dr in dt.Rows)
        {
            TopSiteTotalInfo info = new TopSiteTotalInfo();
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
        SqlParameter[] param = new[]
        {
            new SqlParameter("@sitenick",nick)
        };
        List<TopSiteTotalInfo> list = new List<TopSiteTotalInfo>();
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_OTOTAL_YEAR, param);
        foreach (DataRow dr in dt.Rows)
        {
            TopSiteTotalInfo info = new TopSiteTotalInfo();
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

    #region 用户订购时添加统计数据

    const string SQL_SELECT_ORDERPAYTOTAL = @"select sum(paytotal) mypaytotal,seller_nick,
                COUNT(*) custotal,SUM(ototal) myototal, SUM(postfee) mypostfee from
                (
                SELECT SUM(payment) as paytotal,seller_nick,buy_nick,COUNT(*) as ototal,sum(post_fee) AS postfee from  
                (
                select * from TopTaoBaoGoodsOrderInfo 
                where created between @start and @end and seller_nick=@seller_nick
                ) a  group by seller_nick,buy_nick
                ) b group by seller_nick";

    const string SQL_SELECT_PVUVTOTAL = @"SELECT COUNT(*) ucount,SUM(VCOUNT) vtotal FROM
(
select COUNT(*) vcount,VisitBrower,VisitIP,VisitUserAgent from 
(select * from @tableName where
VisitTime between @start and @end)
A
group by VisitBrower,VisitIP,VisitUserAgent 
) B";

    const string SQL_SELECT_BACKTOTAL = @"select COUNT(*) as backtotal from (
        select distinct VisitIP,VisitBrower,VisitUserAgent from @tableName where 
        VisitTime between @start and @end and VisitIP in
        (
        select VisitIP from @tableName
        where VisitTime<@start
        )
            ) a";

    const string SQL_SELECT_CREATETABLE = "select name from sysobjects where xtype='U' and CHARINDEX('TopVisitInfo',name)>0";

    const string SQL_SELECT_GOODSCOUNT = "SELECT SUM(num) FROM TopTaoBaoOrderGoodsList WHERE status in('WAIT_SELLER_SEND_GOODS','TRADE_FINISHED','WAIT_BUYER_CONFIRM_GOODS','TRADE_BUYER_SIGNED') AND tid in(@tids)";

    const string SQL_SELECT_SECONDBUY = @"select COUNT(*) as backtotal from (
        select distinct buy_nick from TopTaoBaoGoodsOrderInfo where 
        created between @start and @end and seller_nick=@seller_nick and buy_nick in
        (
        select buy_nick from TopTaoBaoGoodsOrderInfo
        where created<@start and seller_nick=@seller_nick
        )
            ) a";

    const string SQL_SELECT = "SELECT SiteTotalDate FROM TopSiteTotal WHERE SiteNick=@SiteNick AND SiteTotalDate=@SiteTotalDate";

    const string SQL_INSERT = "INSERT TopSiteTotal(SiteNick,SiteTotalDate,SitePVCount,SiteUVCount,SiteOrderCount,SiteOrderPay,SiteUVBack,SiteGoodsCount,SitePostFee,SiteSecondBuy,SiteBuyCustomTotal) VALUES(@SiteNick,@SiteTotalDate,@SitePVCount,@SiteUVCount,@SiteOrderCount,@SiteOrderPay,@SiteUVBack,@SiteGoodsCount,@SitePostFee,@SiteSecondBuy,@SiteBuyCustomTotal)";

    const string SQL_UPDATE = "UPDATE TopSiteTotal SET SitePVCount=@SitePVCount,SiteUVCount=@SiteUVCount,SiteOrderCount=@SiteOrderCount,SiteOrderPay=@SiteOrderPay,SiteUVBack=@SiteUVBack,SiteGoodsCount=@SiteGoodsCount,SitePostFee=@SitePostFee,SiteSecondBuy=@SiteSecondBuy,SiteBuyCustomTotal=@SiteBuyCustomTotal WHERE SiteNick=@SiteNick AND SiteTotalDate=@SiteTotalDate";

    const string SQL_SELECT_GETORDERID = "SELECT tid FROM TopTaoBaoGoodsOrderInfo WHERE seller_nick=@seller_nick AND created BETWEEN @start AND @end";

    public TopSiteTotalInfo GetOrderTotalPay(DateTime start, DateTime end,string nick)
    {
        SqlParameter[] param = new[]
           {
               new SqlParameter("@start",start),
               new SqlParameter("@end",end),
               new SqlParameter("@seller_nick",nick)
           };
        TopSiteTotalInfo info = null;

        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_ORDERPAYTOTAL, param);
        foreach (DataRow dr in dt.Rows)
        {
            info = new TopSiteTotalInfo();
            info.SiteOrderCount = int.Parse(dr["myototal"].ToString());
            info.SiteOrderPay = decimal.Parse(dr["mypaytotal"].ToString());
            info.SiteNick = dr["seller_nick"].ToString();
            info.PostFee = decimal.Parse(dr["mypostfee"].ToString());
            info.SiteBuyCustomTotal = int.Parse(dr["custotal"].ToString());
        }
        return info;
    }

    public TopSiteTotalInfo GetPvUvTotal(DateTime start, DateTime end, string tableName)
    {
        string sql = SQL_SELECT_PVUVTOTAL.Replace("@tableName", tableName);

        SqlParameter[] param = new[]
           {
               new SqlParameter("@start",start),
               new SqlParameter("@end",end)
           };
        TopSiteTotalInfo info = null;
        DataTable dt = DBHelper.ExecuteDataTable(sql, param);
        foreach (DataRow dr in dt.Rows)
        {

            info = new TopSiteTotalInfo();
            info.SiteUVCount = int.Parse(dr["ucount"].ToString());
            info.SitePVCount = dr["vtotal"] == DBNull.Value ? 0 : int.Parse(dr["vtotal"].ToString());
        }

        return info;
    }

    public int GetBackTotal(DateTime start, DateTime end, string tableName)
    {
        string sql = SQL_SELECT_BACKTOTAL.Replace("@tableName", tableName);
        SqlParameter[] param = new[]
           {
               new SqlParameter("@start",start),
               new SqlParameter("@end",end)
           };
        return DBHelper.ExecuteScalar(sql, param);
    }

    public List<string> GetTableName()
    {
        List<string> list = new List<string>();
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_CREATETABLE);
        foreach (DataRow dr in dt.Rows)
        {
            list.Add(dr[0].ToString());
        }

        return list;
    }

    public int GetGoodsCount(IList<string> tidlist)
    {
        StringBuilder sb = new StringBuilder();
        foreach (string tid in tidlist)
        {
            sb.Append("'" + tid + "',");
        }

        string sql = SQL_SELECT_GOODSCOUNT.Replace("@tids", sb.ToString().Substring(0, sb.Length - 1));
        return DBHelper.ExecuteScalar(sql);
    }

    public int GetSecondBuyTotal(DateTime start, DateTime end, string nick)
    {
        SqlParameter[] param = new[]
           {
               new SqlParameter("@start",start),
               new SqlParameter("@end",end),
               new SqlParameter("@seller_nick",nick)
           };
        return DBHelper.ExecuteScalar(SQL_SELECT_SECONDBUY, param);
    }

    public List<string> GetOrderIds(DateTime start, DateTime end, string nick)
    {
        SqlParameter[] param = new[]
           {
               new SqlParameter("@start",start),
               new SqlParameter("@end",end),
               new SqlParameter("@seller_nick",nick)
           };
        List<string> list = new List<string>();
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_GETORDERID, param);
        foreach (DataRow dr in dt.Rows)
        {
            list.Add(dr[0].ToString());
        }

        return list;
    }

    /// <summary>
    /// 插入一条
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    private static int AddSiteTotalInfo(TopSiteTotalInfo info)
    {
        return DBHelper.ExecuteNonQuery(SQL_INSERT, GetParameter(info));
    }

    /// <summary>
    /// 更新一条
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    private static int UpdateSiteTotalInfo(TopSiteTotalInfo info)
    {
        return DBHelper.ExecuteNonQuery(SQL_UPDATE, GetParameter(info));
    }

    public void AddOrUp(TopSiteTotalInfo info)
    {
        if (CheckHadSiteTotalInfo(info.SiteNick, info.SiteTotalDate))
            UpdateSiteTotalInfo(info);
        else
            AddSiteTotalInfo(info);
    }

    /// <summary>
    /// 判断是否插入过
    /// </summary>
    /// <param name="nickNo"></param>
    /// <param name="localdate"></param>
    /// <returns></returns>
    private static bool CheckHadSiteTotalInfo(string nickNo, string localdate)
    {

        SqlParameter[] param = new[]
            {
                new SqlParameter("@SiteNick",nickNo),
                new SqlParameter("@SiteTotalDate",localdate)
            };

        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT, param);
        foreach (DataRow dr in dt.Rows)
        {
            return true;
        }

        return false;
    }

    private static SqlParameter[] GetParameter(TopSiteTotalInfo info)
    {
        return new[]
              {
                    new SqlParameter("@SiteNick",info.SiteNick),
                    new SqlParameter("@SiteOrderCount",info.SiteOrderCount),
                    new SqlParameter("@SiteOrderPay",info.SiteOrderPay),
                    new SqlParameter("@SitePVCount",info.SitePVCount),
                    new SqlParameter("@SiteTotalDate",info.SiteTotalDate),
                    new SqlParameter("@SiteUVBack",info.SiteUVBack),
                    new SqlParameter("@SiteUVCount",info.SiteUVCount),
                    new SqlParameter("@SiteGoodsCount",info.GoodsCount),
                    new SqlParameter("@SitePostFee",info.PostFee),
                    new SqlParameter("@SiteSecondBuy", info.SiteSecondBuy),
                    new SqlParameter("@SiteBuyCustomTotal",info.SiteBuyCustomTotal)
              };
    }

    #endregion 
}
