﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

/// <summary>
/// Summary description for NickSessionService
/// </summary>
public class NickSessionService
{
    const string SQL_INSERT = "INSERT TopNickSession(nick,session,JoinDate,NickState,LastGetOrderTime) VALUES(@nick,@session,@JoinDate,@NickState,@LastGetOrderTime)";

    const string SQL_SELECT = "SELECT nick,NickState,LastGetOrderTime FROM TopNickSession WHERE session=@session";

    const string SQL_UPDATE = "UPDATE TopNickSession SET NickState=@NickState,session=@session,JoinDate=@JoinDate";

    const string SQL_SELECT_NICKNO = "SELECT nick,session,NickState,LastGetOrderTime FROM TopNickSession";

    public int InsertSerssion(TopNickSessionInfo sessionInfo)
    {
        SqlParameter[] param = Getparameter(sessionInfo);
        return DBHelper.ExecuteNonQuery(SQL_INSERT, param);
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

        string sql = SQL_UPDATE;
        if (sessionInfo.LastGetOrderTime != DateTime.MinValue)
            sql += ",LastGetOrderTime='" + sessionInfo.LastGetOrderTime + "'";
        sql += " WHERE nick=@nick";

        return DBHelper.ExecuteNonQuery(sql, param);
    }
}
