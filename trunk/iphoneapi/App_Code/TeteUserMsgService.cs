using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
public class TeteUserMsgService
{
    public IList<TeteUserMsgInfo> GetAllTeteUserMsg()
    {
        string sql = "select * from TeteUserMsg";
        return TeteUserMsgPropertity(sql);
    }
    public int AddTeteUserMsg(TeteUserMsgInfo teteusermsg)
    {
        string sql = "insert TeteUserMsg values(@title,@html,@adddate,@nick,@isread,@token)";
        SqlParameter[] param = CreateParameter(teteusermsg);
        return DBHelper.ExecuteNonQuery(sql, param);
    }
    public int ModifyTeteUserMsg(TeteUserMsgInfo teteusermsg)
    {
        string sql = "update TeteUserMsg set title=@title,html=@html,adddate=@adddate,nick=@nick,isread=@isread,token=@token where id=@id";
        SqlParameter[] param = CreateParameter(teteusermsg);
        return DBHelper.ExecuteNonQuery(sql, param);
    }
    public int DeleteTeteUserMsg(int teteusermsgId)
    {
        string sql = "delete from TeteUserMsg where id=" + teteusermsgId;
        return DBHelper.ExecuteNonQuery(sql);
    }
    public TeteUserMsgInfo GetTeteUserMsgById(int teteusermsgId)
    {
        string sql = "select * from TeteUserMsg where id=" + teteusermsgId;
        IList<TeteUserMsgInfo> list = TeteUserMsgPropertity(sql);
        return list.Count == 0 ? null : list[0];
    }
    private SqlParameter[] CreateParameter(TeteUserMsgInfo teteusermsg)
    {
        SqlParameter[] param = new SqlParameter[]
                    {
                      new SqlParameter("@id",teteusermsg.Id),
                      new SqlParameter("@title",teteusermsg.Title),
                      new SqlParameter("@html",teteusermsg.Html),
                      new SqlParameter("@adddate",teteusermsg.Adddate),
                      new SqlParameter("@nick",teteusermsg.Nick),
                      new SqlParameter("@isread",teteusermsg.Isread),
                      new SqlParameter("@token",teteusermsg.Token)
                    };
        return param;
    }
    private IList<TeteUserMsgInfo> TeteUserMsgPropertity(string sql)
    {
        DataTable dt = DBHelper.ExecuteDataTable(sql);
        IList<TeteUserMsgInfo> list = new List<TeteUserMsgInfo>();
        foreach (DataRow dr in dt.Rows)
        {
            TeteUserMsgInfo teteusermsg = new TeteUserMsgInfo();
            teteusermsg.Id = Convert.ToInt32(dr["id"]);
            teteusermsg.Title = Convert.ToString(dr["title"]);
            teteusermsg.Html = Convert.ToString(dr["html"]);
            teteusermsg.Adddate = Convert.ToDateTime(dr["adddate"]);
            teteusermsg.Nick = Convert.ToString(dr["nick"]);
            teteusermsg.Isread = Convert.ToInt32(dr["isread"]);
            teteusermsg.Token = Convert.ToString(dr["token"]);
            list.Add(teteusermsg);
        }
        return list;
    }
}