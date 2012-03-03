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

/// <summary>
/// Summary description for NickSessionService
/// </summary>
public class NickSessionService
{
    const string SQL_INSERT = "INSERT TopNickSession(nick,session,JoinDate,NickState) VALUES(@nick,@session,@JoinDate,@NickState)";

    const string SQL_SELECT = "SELECT nick,NickState,LastGetOrderTime FROM TopNickSession WHERE session=@session";

    const string SQL_UPDATE = "UPDATE TopNickSession SET NickState=@NickState,session=@session,JoinDate=@JoinDate";

    private int InsertSerssion(TopNickSessionInfo sessionInfo)
    {
        SqlParameter[] param = Getparameter(sessionInfo);
        return DBHelper.ExecuteNonQuery(SQL_INSERT, param);
    }

    private static SqlParameter[] Getparameter(TopNickSessionInfo sessionInfo)
    {
        SqlParameter[] param = new[]
        {
            new SqlParameter("@nick",sessionInfo.Nick),
            new SqlParameter("@session",sessionInfo.Session),
            new SqlParameter("@JoinDate",sessionInfo.JoinDate),
            new SqlParameter("@NickState",sessionInfo.NickState)
        };
        return param;
    }

    private TopNickSessionInfo GetSessionInfo(string session)
    {
        SqlParameter param = new SqlParameter("@session", session);
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT, param);
        TopNickSessionInfo info = null;
        foreach (DataRow dr in dt.Rows)
        {
            info = new TopNickSessionInfo();
            info.Nick = dr["nick"].ToString();
            info.LastGetOrderTime = DateTime.Parse(dr["LastGetOrderTime"].ToString()); ;
            info.NickState = (bool)dr["NickState"];
        }
        return info;
    }

    private int UpdateSession(TopNickSessionInfo sessionInfo)
    {
        SqlParameter[] param = Getparameter(sessionInfo);

        string sql = SQL_UPDATE;
        if (sessionInfo.LastGetOrderTime != DateTime.MinValue)
            sql += ",LastGetOrderTime='" + sessionInfo.LastGetOrderTime + "'";
        sql += " WEHRE nick=@nick";

        return DBHelper.ExecuteNonQuery(sql, param);
    }

    public int AddSession(TopNickSessionInfo sessionInfo)
    {
        int count = 0;
        TopNickSessionInfo info = GetSessionInfo(sessionInfo.Session);
        if (info == null)
        {
          count =  InsertSerssion(sessionInfo);
        }
        else
        {
            if (DateTime.Now.AddDays(-15) > info.LastGetOrderTime)
            {
                sessionInfo.LastGetOrderTime = DateTime.Now.AddDays(-15);
            }
            count = UpdateSession(sessionInfo);
        }
        return count;
    }
}
