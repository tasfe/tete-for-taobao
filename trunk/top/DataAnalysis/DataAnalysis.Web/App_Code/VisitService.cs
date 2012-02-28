using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// Summary description for VisitService
/// </summary>
public class VisitService
{
    const string SQL_INSERT = "INSERT TopVisitInfo(VisitID,VisitIP,VisitUrl,VisitTime,VisitUserAgent,VisitBrower,VisitOSLanguage) VALUES(@VisitID,@VisitIP,@VisitUrl,@VisitTime,@VisitUserAgent,@VisitBrower,@VisitOSLanguage)";

    //统计小时流量
    const string SQL_HOUR_TOTAL = "SELECT VisitShopId,Count(*) AS PVCount,DatePart(hh,VisitTime) AS PVHour FROM [TopVisitInfo] GROUP BY CONVERT(VARCHAR(30),VisitTime,5),DatePart(hh,VisitTime),VisitShopId HAVING CONVERT(VARCHAR(30),VisitTime,5)=CONVERT(VARCHAR(30),GetDate(),5) ORDER BY PVHour";// AND VisitShopId IS NOT NULL";

    public void InsertVisitInfo(TopVisitInfo info)
    {
        DBHelper.ExecuteNonQuery(SQL_INSERT,CreateParameter(info));
    }

    public IList<HourPVInfo> GetHourPVTotal()
    {
        DataTable dt = DBHelper.ExecuteDataTable(SQL_HOUR_TOTAL);
        IList<HourPVInfo> list = new List<HourPVInfo>();
        foreach(DataRow dr  in dt.Rows)
        {
            HourPVInfo info =new HourPVInfo();
            info.PVCount = int.Parse( dr["PVCount"].ToString());
            info.Hour = int.Parse(dr["PVHour"].ToString());
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
