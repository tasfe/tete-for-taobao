using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;


public class TeteUserTokenService
{
    public IList<TeteUserTokenInfo> GetAllTeteUserToken(string nick)
    {
        string sql = "select * from TeteUserToken where nick=@nick";
        SqlParameter param = new SqlParameter("@nick", nick);
        return TeteUserTokenPropertity(sql, param);
    }
    public int AddTeteUserToken(TeteUserTokenInfo teteusertoken)
    {
        string sql = "insert TeteUserToken values(@nick,@token,@adddate,@mobile,@updatedate,@logintimes)";
        SqlParameter[] param = CreateParameter(teteusertoken);
        return DBHelper.ExecuteNonQuery(sql, param);
    }
    public int ModifyTeteUserToken(TeteUserTokenInfo teteusertoken)
    {
        string sql = "update TeteUserToken set nick=@nick,token=@token,adddate=@adddate,mobile=@mobile,updatedate=@updatedate,logintimes=@logintimes where id=@id";
        SqlParameter[] param = CreateParameter(teteusertoken);
        return DBHelper.ExecuteNonQuery(sql, param);
    }
    public int DeleteTeteUserToken(int teteusertokenId)
    {
        string sql = "delete from TeteUserToken where id=" + teteusertokenId;
        return DBHelper.ExecuteNonQuery(sql);
    }
    public TeteUserTokenInfo GetTeteUserTokenById(int teteusertokenId)
    {
        string sql = "select * from TeteUserToken where id=" + teteusertokenId;
        IList<TeteUserTokenInfo> list = TeteUserTokenPropertity(sql);
        return list.Count == 0 ? null : list[0];
    }
    private SqlParameter[] CreateParameter(TeteUserTokenInfo teteusertoken)
    {
        SqlParameter[] param = new SqlParameter[]
                    {
                      new SqlParameter("@id",teteusertoken.Id),
                      new SqlParameter("@nick",teteusertoken.Nick),
                      new SqlParameter("@token",teteusertoken.Token),
                      new SqlParameter("@adddate",teteusertoken.Adddate),
                      new SqlParameter("@mobile",teteusertoken.Mobile),
                      new SqlParameter("@updatedate",teteusertoken.Updatedate),
                      new SqlParameter("@logintimes",teteusertoken.Logintimes)
                    };
        return param;
    }
    private IList<TeteUserTokenInfo> TeteUserTokenPropertity(string sql,params SqlParameter[] param)
    {
        DataTable dt = DBHelper.ExecuteDataTable(sql);
        IList<TeteUserTokenInfo> list = new List<TeteUserTokenInfo>();
        foreach (DataRow dr in dt.Rows)
        {
            TeteUserTokenInfo teteusertoken = new TeteUserTokenInfo();
            teteusertoken.Id = Convert.ToInt32(dr["id"]);
            teteusertoken.Nick = Convert.ToString(dr["nick"]);
            teteusertoken.Token = Convert.ToString(dr["token"]);
            teteusertoken.Adddate = Convert.ToDateTime(dr["adddate"]);
            teteusertoken.Mobile = dr["mobile"] == DBNull.Value ? "" : Convert.ToString(dr["mobile"]);
            teteusertoken.Updatedate = dr["updatedate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["updatedate"]);
            teteusertoken.Logintimes = Convert.ToInt32(dr["logintimes"]);
            list.Add(teteusertoken);
        }
        return list;
    }
}