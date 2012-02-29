using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System;

/// <summary>
/// Summary description for VisitService
/// </summary>
public class VisitService
{

    //查询该表是否存在
    const string SQL_SELECT_TABLE_EXISTS = "SELECT COUNT(*) FROM sysobjects WHERE id=object_id('@tableName') AND OBJECTPROPERTY(id,'IsUserTable')=1";

    //建表
    const string SQL_CREATE_TABLE = @"CREATE TABLE @tableName(
	[VisitID] [uniqueidentifier] NOT NULL,
	[VisitIP] [varchar](50) NULL,
	[VisitUrl] [varchar](250) NULL,
	[VisitTime] [datetime] NULL,
	[VisitUserAgent] [varchar](50) NULL,
	[VisitBrower] [varchar](50) NULL,
	[VisitOSLanguage] [varchar](50) NULL,
	[VisitShopId] [varchar](50) NULL,
 CONSTRAINT @pk PRIMARY KEY CLUSTERED 
(
	[VisitID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
 CREATE INDEX index_@tableName_VisitShopId ON @tableName([VisitShopId])
 CREATE INDEX index_@tableName_VisitTime ON @tableName([VisitTime])";

    //插入
    const string SQL_INSERT = "INSERT TopVisitInfo(VisitID,VisitIP,VisitUrl,VisitTime,VisitUserAgent,VisitBrower,VisitOSLanguage,VisitShopId) VALUES(@VisitID,@VisitIP,@VisitUrl,@VisitTime,@VisitUserAgent,@VisitBrower,@VisitOSLanguage,@VisitShopId)";

    //指定表插入
    const string SQL_INSERT_TABLE = "INSERT @tableName(VisitID,VisitIP,VisitUrl,VisitTime,VisitUserAgent,VisitBrower,VisitOSLanguage,VisitShopId) VALUES(@VisitID,@VisitIP,@VisitUrl,@VisitTime,@VisitUserAgent,@VisitBrower,@VisitOSLanguage,@VisitShopId)";

    //统计小时PV流量
    const string SQL_HOUR_PVTOTAL = "SELECT VisitShopId,Count(*) AS PVCount,DatePart(hh,VisitTime) AS PVHour FROM [TopVisitInfo] GROUP BY CONVERT(VARCHAR(30),VisitTime,5),DatePart(hh,VisitTime),VisitShopId HAVING CONVERT(VARCHAR(30),VisitTime,5)=CONVERT(VARCHAR(30),GetDate(),5)  ";//ORDER BY PVHour";// AND VisitShopId IS NOT NULL";

    //统计小时IP流量
    const string SQL_HOUR_IPTOTAL = "SELECT COUNT(distinct VisitIp) AS IPCount,DatePart(hh,VisitTime) AS IPHour FROM [TopVisitInfo] GROUP BY DatePart(hh,VisitTime),CONVERT(VARCHAR(30),VisitTime,5) HAVING CONVERT(VARCHAR(30),VisitTime,5)=CONVERT(VARCHAR(30),GetDate(),5) ";//ORDER BY IPHour";

    //统计小时PV流量(指定表/订购用户)
    const string SQL_HOUR_PVTOTAL_TABLE = "SELECT VisitShopId,Count(*) AS PVCount,DatePart(hh,VisitTime) AS PVHour FROM @tableName GROUP BY CONVERT(VARCHAR(30),VisitTime,5),DatePart(hh,VisitTime),VisitShopId HAVING CONVERT(VARCHAR(30),VisitTime,5)=CONVERT(VARCHAR(30),GetDate(),5)  ";//ORDER BY PVHour";// AND VisitShopId IS NOT NULL";

    //统计小时IP流量(指定表/订购用户)
    const string SQL_HOUR_IPTOTAL_TABLE = "SELECT COUNT(distinct VisitIp) AS IPCount,DatePart(hh,VisitTime) AS IPHour FROM @tableName GROUP BY DatePart(hh,VisitTime),CONVERT(VARCHAR(30),VisitTime,5) HAVING CONVERT(VARCHAR(30),VisitTime,5)=CONVERT(VARCHAR(30),GetDate(),5) ";//ORDER BY IPHour";

    //查找流水
    const string SQL_SELECT_ALL_BYDATE = "SELECT * FROM(SELECT  VisitUrl,VisitIP, COUNT(*) AS VCount,ROW_NUMBER() OVER(ORDER BY VisitUrl ASC, VisitIP ASC) as RowNum FROM (SELECT *  FROM @tableName WHERE  VisitTime BETWEEN @sdate AND @edate AND VisitUrl<>'') a GROUP BY VisitUrl,visitIP )  b  WHERE RowNum BETWEEN @srecode AND @erecode";

    const string SQL_SELECT_IPTOTAL_TABLE_BYDATE="";

    /// <summary>
    /// 用户订购获取代码时生成一张表
    /// </summary>
    /// <param name="nickNo"></param>
    public void CreateTable(string nickNo)
    {
        string sql = SQL_SELECT_TABLE_EXISTS.Replace("@tableName", GetRealTable(nickNo));
        int drow = DBHelper.ExecuteScalar(sql);
        if (drow == 0)
        {
            sql = SQL_CREATE_TABLE.Replace("@tableName", GetRealTable(nickNo)).Replace("@pk","PK_TopVisitInfo_"+nickNo);

            DBHelper.ExecuteNonQuery(sql);
        }
    }

    //指定表插入一条浏览记录
    public void InsertVisitInfo(TopVisitInfo info, string nickNo)
    {
        string sql = SQL_INSERT_TABLE.Replace("@tableName", GetRealTable(nickNo));
        DBHelper.ExecuteNonQuery(sql, CreateParameter(info));
    }

    //一个订购用户一张表
    private  static string GetRealTable(string nickNo)
    {
        return "TopVisitInfo_" + nickNo;
    }

    public IList<HourTotalInfo> GetHourPVTotal(string nickNo)
    {
        string sql = SQL_HOUR_PVTOTAL_TABLE.Replace("@tableName", GetRealTable(nickNo));
        DataTable dt = DBHelper.ExecuteDataTable(sql);
        IList<HourTotalInfo> list = new List<HourTotalInfo>();
        foreach (DataRow dr in dt.Rows)
        {
            HourTotalInfo info = new HourTotalInfo();
            info.PVCount = int.Parse(dr["PVCount"].ToString());
            info.Hour = int.Parse(dr["PVHour"].ToString());
            list.Add(info);
        }
        list.OrderBy(o => o.Hour);
        return list;
    }

    public IList<HourTotalInfo> GetHourIPTotal(string nickNo)
    {
        string sql = SQL_HOUR_IPTOTAL_TABLE.Replace("@tableName", GetRealTable(nickNo));
        DataTable dt = DBHelper.ExecuteDataTable(sql);
        IList<HourTotalInfo> list = new List<HourTotalInfo>();
        foreach (DataRow dr in dt.Rows)
        {
            HourTotalInfo info = new HourTotalInfo();
            info.PVCount = int.Parse(dr["IPCount"].ToString());
            info.Hour = int.Parse(dr["IPHour"].ToString());
            list.Add(info);
        }
        list.OrderBy(o => o.Hour);
        return list;
    }

    public IList<PageVisitInfoTotal> GetAllVisitPageInfoList(string nickNo, DateTime start, DateTime end, int pcount, int recordCount)
    {
        string sql = SQL_SELECT_ALL_BYDATE.Replace("@tableName", GetRealTable(nickNo));
        int srecode = recordCount * (pcount - 1) + 1;
        int erecode = recordCount * pcount;
        SqlParameter[] param = new[]
            {
                new SqlParameter("@sdate",start),
                new SqlParameter("@edate",end),
                new SqlParameter("@srecode",srecode),
                new SqlParameter("@erecode",erecode)
            };
        DataTable dt = DBHelper.ExecuteDataTable(sql, param);
        IList<PageVisitInfoTotal> list = new List<PageVisitInfoTotal>();
        foreach (DataRow dr in dt.Rows)
        {
            string url = dr["VisitUrl"].ToString();
            int count = int.Parse(dr["VCount"].ToString());

            PageVisitInfoTotal info = null;
            if (list.Where(o => o.VisitURL == url).ToList().Count > 0)
            {
                info = list.Where(o => o.VisitURL == info.VisitURL).ToList()[0];
                info.VisitCount += count;
                info.IPCount += 1;
            }
            else
            {
                info = new PageVisitInfoTotal();
                info.VisitURL = dr["VisitUrl"].ToString();
                info.VisitCount = count;
                info.IPCount = 1;
            }
            list.Add(info);
        }
        list.OrderByDescending(o => o.VisitCount);

        return list;
    }

    #region 所有订购用户使用一张流水表

    //插入一条浏览记录
    [System.Obsolete]
    public void InsertVisitInfo(TopVisitInfo info)
    {
        DBHelper.ExecuteNonQuery(SQL_INSERT, CreateParameter(info));
    }

    [System.Obsolete]
    public IList<HourTotalInfo> GetHourPVTotal()
    {
        DataTable dt = DBHelper.ExecuteDataTable(SQL_HOUR_PVTOTAL);
        IList<HourTotalInfo> list = new List<HourTotalInfo>();
        foreach(DataRow dr  in dt.Rows)
        {
            HourTotalInfo info =new HourTotalInfo();
            info.PVCount = int.Parse( dr["PVCount"].ToString());
            info.Hour = int.Parse(dr["PVHour"].ToString());
            list.Add(info);
        }
        list.OrderBy(o => o.Hour);
        return list;
    }

    [System.Obsolete]
    public IList<HourTotalInfo> GetHourIPTotal()
    {
        DataTable dt = DBHelper.ExecuteDataTable(SQL_HOUR_IPTOTAL);
        IList<HourTotalInfo> list = new List<HourTotalInfo>();
        foreach (DataRow dr in dt.Rows)
        {
            HourTotalInfo info = new HourTotalInfo();
            info.PVCount = int.Parse(dr["IPCount"].ToString());
            info.Hour = int.Parse(dr["IPHour"].ToString());
            list.Add(info);
        }
        list.OrderBy(o => o.Hour);
        return list;
    }

    #endregion

    private static SqlParameter[] CreateParameter(TopVisitInfo info)
    {
        SqlParameter[] param = new[]
        {
            new SqlParameter("@VisitID", info.VisitID),
            new SqlParameter("@VisitIP",info.VisitIP),
            new SqlParameter("@VisitUrl",info.VisitUrl),
            new  SqlParameter("@VisitTime",info.VisitTime),
            new SqlParameter("@VisitUserAgent", info.VisitUserAgent),
            new SqlParameter("@VisitBrower", info.VisitBrower),
            new SqlParameter("@VisitOSLanguage",info.VisitOSLanguage),
            new SqlParameter("@VisitShopId",info.VisitShopId)
        };

        return param;
    }
}
