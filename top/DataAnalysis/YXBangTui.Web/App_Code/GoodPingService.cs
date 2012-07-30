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
using System.Data.SqlClient;
using System.Collections.Generic;

/// <summary>
/// 好评存取类
/// </summary>
public class GoodPingService
{

    const string SQL_INSERT = "INSERT BangT_HaoPing(Nick,PingTimes,PingDate,AddIP) VALUES(@Nick,@PingTimes,@PingDate,@AddIP)";

    const string SQL_SELECT = "SELECT Nick,PingTimes,PingDate,AddIP FROM BangT_HaoPing ORDER BY PingDate DESC";

    const string SQL_SELECT_BY_NICK = "SELECT Nick,PingTimes,PingDate,AddIP FROM BangT_HaoPing WHERE Nick=@Nick";

    const string SQL_UPDATE = "UPDATE BangT_HaoPing SET PingTimes=@PingTimes,AddIP=@AddIP,PingDate=@PingDate WHERE Nick=@Nick";

    public int InsertGoodPing(GoodPingInfo info)
    {
        return DBHelper.ExecuteNonQuery(SQL_INSERT, CreateParameter(info));
    }

    public IList<GoodPingInfo> GetGoodPing()
    {
        IList<GoodPingInfo> list = new List<GoodPingInfo>();
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT);

        foreach (DataRow dr in dt.Rows)
        {
            GoodPingInfo info = new GoodPingInfo();
            info.Nick = dr["Nick"].ToString();
            info.PingTimes = int.Parse(dr["PingTimes"].ToString());
            info.PingDate = DateTime.Parse(dr["PingDate"].ToString());
            info.AddIP = dr["AddIP"].ToString();

            list.Add(info);
        }

        return list;
    }

    public IList<GoodPingInfo> GetGoodPingByNick(string nick)
    {
        IList<GoodPingInfo> list = new List<GoodPingInfo>();
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_BY_NICK, new SqlParameter("@Nick", nick));

        foreach (DataRow dr in dt.Rows)
        {
            GoodPingInfo info = new GoodPingInfo();
            info.Nick = dr["Nick"].ToString();
            info.PingTimes = int.Parse(dr["PingTimes"].ToString());
            info.PingDate = DateTime.Parse(dr["PingDate"].ToString());
            info.AddIP = dr["AddIP"].ToString();

            list.Add(info);
        }

        return list;
    }

    public int UpdatePingInfo(GoodPingInfo info)
    {
        return DBHelper.ExecuteNonQuery(SQL_UPDATE, CreateParameter(info));
    }

    private static SqlParameter[] CreateParameter(GoodPingInfo info)
    {
        SqlParameter[] param = new[]
        {
            new SqlParameter("@Nick",info.Nick),
            new SqlParameter("@PingTimes",info.PingTimes),
            new SqlParameter("@PingDate",info.PingDate),
            new SqlParameter("@AddIP",info.AddIP)
        };

        return param;
    }
}
