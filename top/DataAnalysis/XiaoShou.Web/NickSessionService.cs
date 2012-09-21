using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

/// <summary>
/// Summary description for NickSessionService
/// </summary>
public class NickSessionService
{
    const string SQL_INSERT = "INSERT TopNickSession(nick,session,JoinDate,NickState,LastGetOrderTime) VALUES(@nick,@session,@JoinDate,@NickState,@LastGetOrderTime)";

    const string SQL_INSERT_NEW = "INSERT TopNickSession(nick,session,JoinDate,NickState,LastGetOrderTime,ServiceID,ShopId,RefreshToken) VALUES(@nick,@session,@JoinDate,@NickState,@LastGetOrderTime,@ServiceID,@ShopId,@RefreshToken)";

    const string SQL_SELECT = "SELECT nick,NickState,LastGetOrderTime FROM TopNickSession WHERE session=@session";

    const string SQL_UPDATE = "UPDATE TopNickSession SET NickState=@NickState,session=@session,JoinDate=@JoinDate";

    const string SQL_SELECT_NICKNO = "SELECT nick,session,NickState,LastGetOrderTime,ServiceID,ShopId FROM TopNickSession";

    //public int InsertSerssion(TopNickSessionInfo sessionInfo)
    //{
    //    SqlParameter[] param = Getparameter(sessionInfo);
    //    return DBHelper.ExecuteNonQuery(SQL_INSERT, param);
    //}

    /// <summary>
    /// 正式使用的插入用户信息
    /// </summary>
    /// <param name="sessionInfo"></param>
    /// <returns></returns>
    public int InsertSerssionNew(TopNickSessionInfo sessionInfo)
    {
        SqlParameter[] param = Getparameter(sessionInfo);
        List<SqlParameter> plist = new List<SqlParameter>();
        foreach (SqlParameter p in param)
            plist.Add(p);
        plist.Add(new SqlParameter("@ServiceID", (int)sessionInfo.ServiceId));
        plist.Add(new SqlParameter("@ShopId", sessionInfo.ShopId));
        plist.Add(new SqlParameter("@RefreshToken", sessionInfo.RefreshToken));

        return DBHelper.ExecuteNonQuery(SQL_INSERT_NEW, plist.ToArray());
    }

    public IList<TopNickSessionInfo> GetAllNickSession()
    {
        IList<TopNickSessionInfo> list = new List<TopNickSessionInfo>();
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_NICKNO);
        foreach (DataRow dr in dt.Rows)
        {
            TopNickSessionInfo info = new TopNickSessionInfo();
            info.Nick = dr["nick"].ToString();
            info.Session = dr["session"].ToString();
            info.NickState = (bool)dr["NickState"];
            info.LastGetOrderTime = dr["LastGetOrderTime"] == DBNull.Value ? DateTime.MinValue : DateTime.Parse(dr["LastGetOrderTime"].ToString());

            info.ServiceId = (Enum.TopTaoBaoService)int.Parse(dr["ServiceID"].ToString());
           
            info.ShopId = dr["ShopId"].ToString();

            list.Add(info);
        }

        return list;
    }

    private static SqlParameter[] Getparameter(TopNickSessionInfo sessionInfo)
    {
        SqlParameter[] param = new[]
        {
            new SqlParameter("@nick",sessionInfo.Nick),
            new SqlParameter("@session",sessionInfo.Session),
            new SqlParameter("@JoinDate",sessionInfo.JoinDate),
            new SqlParameter("@NickState",sessionInfo.NickState),
            new SqlParameter("@LastGetOrderTime", sessionInfo.LastGetOrderTime)
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

    public int UpdateSession(TopNickSessionInfo sessionInfo)
    {
        SqlParameter[] param = Getparameter(sessionInfo);

        string sql = SQL_UPDATE + ",ShopId='" + sessionInfo.ShopId + "'";
        if (sessionInfo.LastGetOrderTime != DateTime.MinValue)
            sql += ",LastGetOrderTime='" + sessionInfo.LastGetOrderTime + "'";
        sql += " WHERE nick=@nick AND ServiceId=" + (int)sessionInfo.ServiceId;

        return DBHelper.ExecuteNonQuery(sql, param);
    }
}
