using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System;
using DBHelp;

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
	[VisitUrl] [varchar](500) NULL,
	[VisitTime] [datetime] NULL,
	[VisitUserAgent] [varchar](50) NULL,
	[VisitBrower] [varchar](50) NULL,
	[VisitOSLanguage] [varchar](50) NULL,
    [LastURL] [varchar](500) NULL,
    [GoodsId] [varchar](50) NULL,
    [GoodsClassId] [varchar](50) NULL,
 CONSTRAINT @pk PRIMARY KEY CLUSTERED 
(
	[VisitID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
 CREATE INDEX index_@tableName_VisitTime ON @tableName([VisitTime])
 CREATE INDEX index_@tableName_GoodsId ON @tableName([GoodsId])

 CREATE INDEX index_@tableName_GoodsClassId ON @tableName([GoodsClassId])
";
    // CREATE INDEX index_@tableName_GoodsName ON @tableName([GoodsName])
    // CREATE INDEX index_@tableName_GoodsClassName ON @tableName([GoodsClassName])     
   // [GoodsClassName] [nvarchar](60) NULL,
   // [GoodsName] [nvarchar](100) NULL,

    //指定表插入
    const string SQL_INSERT_TABLE = "INSERT @tableName(VisitID,VisitIP,VisitUrl,VisitTime,VisitUserAgent,VisitBrower,VisitOSLanguage,LastURL,GoodsId,GoodsClassId) VALUES(@VisitID,@VisitIP,@VisitUrl,@VisitTime,@VisitUserAgent,@VisitBrower,@VisitOSLanguage,@LastURL,@GoodsId,@GoodsClassId)";

    //统计小时PV流量(指定表/订购用户)
    const string SQL_HOUR_PVTOTAL_TABLE = "SELECT Count(*) AS PVCount,DatePart(hh,VisitTime) AS PVHour FROM @tableName GROUP BY CONVERT(VARCHAR(30),VisitTime,5),DatePart(hh,VisitTime)  HAVING CONVERT(VARCHAR(30),VisitTime,5)=CONVERT(VARCHAR(30),@date,5)  ";//ORDER BY PVHour

    //统计小时IP流量(指定表/订购用户)
    const string SQL_HOUR_IPTOTAL_TABLE = "SELECT COUNT(distinct VisitIp) AS IPCount,DatePart(hh,VisitTime) AS IPHour FROM @tableName GROUP BY DatePart(hh,VisitTime),CONVERT(VARCHAR(30),VisitTime,5) HAVING CONVERT(VARCHAR(30),VisitTime,5)=CONVERT(VARCHAR(30),@date,5) ";//ORDER BY IPHour";

    //查找流水
    const string SQL_SELECT_ALL_BYDATE = @"SELECT VisitUrl,VisitIP,COUNT(*) AS VCount
                                           FROM (SELECT *  FROM @tableName
                                           WHERE VisitTime BETWEEN @start AND @end AND VisitUrl<>'') a 
                                           GROUP BY VisitUrl,visitIP";

    const string SQL_SELECT_IPTOTAL_TABLE_BYDATE="";

    const string SQL_INDEX_TOTAL = @"SELECT  'pv' as pv,COUNT(*) pvcount FROM  @tableName WHERE VisitTime BETWEEN @start AND @end
UNION 
SELECT 'ip' as ip,sum(ipcount) FROM
(
SELECT COUNT(distinct VisitIP) AS ipcount,VisitIP FROM (select * from @tableName where  VisitTime BETWEEN @start AND @end) a
group by VisitIP 
) b
UNION 
SELECT 'uv' as uv,sum(uvcount) FROM
(
SELECT COUNT(distinct VisitIP) AS uvcount,VisitIP,VisitBrower,VisitUserAgent FROM (select  * from @tableName where  VisitTime BETWEEN @start AND @end) c
group by VisitIP,VisitBrower,VisitUserAgent
) d";

    const string SQL_INDEX_TOP_ONLINECUSTOMER = "SELECT DISTINCT TOP @topNum VisitIP,VisitTime FROM @tableName WHERE  VisitTime BETWEEN @start AND @end ORDER BY VisitTime DESC ";

    const string SQL_SELECT_VISITINFO_BYIP = "SELECT VisitUrl,VisitTime,GoodsId,GoodsClassId FROM @tableName WHERE VisitIP=@VisitIP AND VisitTime BETWEEN @start AND @end";

    const string SQL_SELECT_GETZHITONG_HOURTOTAL = @"SELECT Count(*) AS PVCount,DatePart(hh,VisitTime) AS PVHour FROM                        (
                            select * from @tableName
                            WHERE CHARINDEX( 'ali_refid',VisitUrl)>0 AND CONVERT(VARCHAR(30),VisitTime,5)=CONVERT(VARCHAR(30),@date,5)
                             ) a
                            GROUP BY CONVERT(VARCHAR(30),VisitTime,5),DatePart(hh,VisitTime)";

    const string SQL_SELECT_GETZUANZHAN_HOURTOTAL = @"SELECT Count(*) AS PVCount,DatePart(hh,VisitTime) AS PVHour FROM                        (
                            select * from @tableName
                            WHERE CHARINDEX( 'ali_trackid',VisitUrl)>0 AND CHARINDEX( 'ali_refid',VisitUrl)<=0 AND CONVERT(VARCHAR(30),VisitTime,5)=CONVERT(VARCHAR(30),@date,5)
                             ) a
                            GROUP BY CONVERT(VARCHAR(30),VisitTime,5),DatePart(hh,VisitTime)";

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

    public static bool CheckTable(string nickNo)
    {
        string sql = SQL_SELECT_TABLE_EXISTS.Replace("@tableName", GetRealTable(nickNo));
        int drow = DBHelper.ExecuteScalar(sql);
        return drow == 0 ? false : true;
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

    public IList<HourTotalInfo> GetHourPVTotal(string nickNo, DateTime date)
    {
        string sql = SQL_HOUR_PVTOTAL_TABLE.Replace("@tableName", GetRealTable(nickNo));
        DataTable dt = DBHelper.ExecuteDataTable(sql, new SqlParameter("@date", date));
        IList<HourTotalInfo> list = new List<HourTotalInfo>();
        foreach (DataRow dr in dt.Rows)
        {
            HourTotalInfo info = new HourTotalInfo();
            info.PVCount = int.Parse(dr["PVCount"].ToString());
            info.Hour = int.Parse(dr["PVHour"].ToString());
            list.Add(info);
        }
        IList<HourTotalInfo> rlist = list.OrderBy(o => o.Hour).ToList();

        return rlist;
    }

    public IList<HourTotalInfo> GetHourIPTotal(string nickNo, DateTime date)
    {
        string sql = SQL_HOUR_IPTOTAL_TABLE.Replace("@tableName", GetRealTable(nickNo));
        DataTable dt = DBHelper.ExecuteDataTable(sql, new SqlParameter("@date", date));
        IList<HourTotalInfo> list = new List<HourTotalInfo>();
        foreach (DataRow dr in dt.Rows)
        {
            HourTotalInfo info = new HourTotalInfo();
            info.PVCount = int.Parse(dr["IPCount"].ToString());
            info.Hour = int.Parse(dr["IPHour"].ToString());
            list.Add(info);
        }
        IList<HourTotalInfo> rlist = list.OrderBy(o => o.Hour).ToList();

        return rlist;
    }

    public IList<PageVisitInfoTotal> GetAllVisitPageInfoList(string nickNo, DateTime start, DateTime end)
    {
        string sql = SQL_SELECT_ALL_BYDATE.Replace("@tableName", GetRealTable(nickNo));
        SqlParameter[] param = new[]
            {
                new SqlParameter("@start",start),
                new SqlParameter("@end",end)
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
                info = list.Where(o => o.VisitURL == url).ToList()[0];
                info.VisitCount += count;
                info.IPCount++;
            }
            else
            {
                info = new PageVisitInfoTotal();
                info.VisitURL = dr["VisitUrl"].ToString();
                info.VisitCount = count;
                info.IPCount = 1;

                list.Add(info);
            }
        }
         IList<PageVisitInfoTotal> rlist = list.OrderByDescending(o => o.VisitCount).ToList();

         return rlist;
    }

    public IList<IndexTotalInfo> GetIndexTotalInfoList(string nickNo, DateTime start, DateTime end)
    {
        string sql = SQL_INDEX_TOTAL.Replace("@tableName", GetRealTable(nickNo));
        SqlParameter[] param = new[]
        {
            new SqlParameter("@start",start),
            new SqlParameter("@end",end)
        };
        DataTable dt = DBHelper.ExecuteDataTable(sql, param);
        IList<IndexTotalInfo> list = new List<IndexTotalInfo>();
        foreach (DataRow dr in dt.Rows)
        {
            IndexTotalInfo info = new IndexTotalInfo();
            info.Key = dr["pv"].ToString();
            info.Value = dr["pvcount"] == DBNull.Value ? 0 : int.Parse(dr["pvcount"].ToString());
            list.Add(info);
        }
        IList<IndexTotalInfo> rlist = list.OrderBy(o => o.Key).ToList();
        return rlist;
    }

    public IList<TopVisitInfo> GetIndexOnlineCustomer(string nickNo, int topNum,DateTime start,DateTime end)
    {
        string sql = SQL_INDEX_TOP_ONLINECUSTOMER.Replace("@tableName", GetRealTable(nickNo)).Replace("@topNum",topNum.ToString());
        
        SqlParameter[] param = new[]
        {
            new SqlParameter("@start",start),
            new SqlParameter("@end",end)
        };
        DataTable dt = DBHelper.ExecuteDataTable(sql, param);
        IList<TopVisitInfo> list = new List<TopVisitInfo>();
        foreach (DataRow dr in dt.Rows)
        {
            TopVisitInfo info = new TopVisitInfo();
            info.VisitIP = dr["VisitIP"].ToString();
            info.VisitTime = DateTime.Parse(dr["VisitTime"].ToString());
            list.Add(info);
        }
        IList<TopVisitInfo> rlist = list.OrderByDescending(o => o.VisitTime).ToList();
        return rlist;
    }

    public IList<TopVisitInfo> GetVisitInfoByIp(string nickNo,string ip, DateTime start, DateTime end)
    {
        string sql = SQL_SELECT_VISITINFO_BYIP.Replace("@tableName", GetRealTable(nickNo));
        SqlParameter[] param = new[]
        {
            new SqlParameter("@start",start),
            new SqlParameter("@end",end),
            new SqlParameter("@VisitIP",ip)
        };
        DataTable dt = DBHelper.ExecuteDataTable(sql, param);
        IList<TopVisitInfo> list = new List<TopVisitInfo>();
        foreach (DataRow dr in dt.Rows)
        {
            TopVisitInfo info = new TopVisitInfo();
            info.VisitUrl = dr["VisitUrl"].ToString();
            info.VisitTime = DateTime.Parse(dr["VisitTime"].ToString());
            info.GoodsId = dr["GoodsId"].ToString();
            info.GoodsClassId = dr["GoodsClassId"].ToString();
            list.Add(info);
        }
        IList<TopVisitInfo> rlist = list.OrderBy(o => o.VisitTime).ToList();
        return rlist;
    }

    public IList<HourTotalInfo> GetHourZhiTongOrZuanZhanPVTotal(string nickNo, DateTime date, bool zhi)
    {
        string sql = "";
        if (zhi)
            sql = SQL_SELECT_GETZHITONG_HOURTOTAL.Replace("@tableName", GetRealTable(nickNo));
        else
            sql = SQL_SELECT_GETZUANZHAN_HOURTOTAL.Replace("@tableName", GetRealTable(nickNo));
        DataTable dt = DBHelper.ExecuteDataTable(sql, new SqlParameter("@date", date));
        IList<HourTotalInfo> list = new List<HourTotalInfo>();
        foreach (DataRow dr in dt.Rows)
        {
            HourTotalInfo info = new HourTotalInfo();
            info.PVCount = int.Parse(dr["PVCount"].ToString());
            info.Hour = int.Parse(dr["PVHour"].ToString());
            list.Add(info);
        }
        IList<HourTotalInfo> rlist = list.OrderBy(o => o.Hour).ToList();

        return rlist;
    }

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
            new SqlParameter("@GoodsId",info.GoodsId),
            new SqlParameter("@GoodsClassId",info.GoodsClassId),
            new SqlParameter("@LastURL",info.LastURL)
        };

        return param;
    }
}
