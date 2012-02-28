using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// Summary description for VisitService
/// </summary>
public class VisitService
{
    const string SQL_INSERT = "INSERT TopVisitInfo(VisitID,VisitIP,VisitUrl,VisitTime,VisitUserAgent,VisitBrower,VisitOSLanguage) VALUES(@VisitID,@VisitIP,@VisitUrl,@VisitTime,@VisitUserAgent,@VisitBrower,@VisitOSLanguage)";

    //统计小时PV流量
    const string SQL_HOUR_PVTOTAL = "SELECT VisitShopId,Count(*) AS PVCount,DatePart(hh,VisitTime) AS PVHour FROM [TopVisitInfo] GROUP BY CONVERT(VARCHAR(30),VisitTime,5),DatePart(hh,VisitTime),VisitShopId HAVING CONVERT(VARCHAR(30),VisitTime,5)=CONVERT(VARCHAR(30),GetDate(),5) ORDER BY PVHour";// AND VisitShopId IS NOT NULL";

    //统计小时IP流量
    const string SQL_HOUR_IPTOTAL = "SELECT COUNT(distinct VisitIp) AS IPCount,DatePart(hh,VisitTime) AS IPHour FROM[TopVisitInfo]GROUP BY DatePart(hh,VisitTime),CONVERT(VARCHAR(30),VisitTime,5) HAVING CONVERT(VARCHAR(30),VisitTime,5)=CONVERT(VARCHAR(30),GetDate(),5) ORDER BY IPHour";

    public void InsertVisitInfo(TopVisitInfo info)
    {
        DBHelper.ExecuteNonQuery(SQL_INSERT,CreateParameter(info));
    }

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

        return list;
    }

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

        return list;
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
            new SqlParameter("@VisitOSLanguage",info.VisitOSLanguage)
        };

        return param;
    }
}
