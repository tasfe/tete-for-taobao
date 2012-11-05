using System;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Data.SqlClient;

/// <summary>
/// Summary description for DBHelper
/// </summary>
public class DBHelper
{
	public DBHelper()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    /// <summary>
    /// 事务执行(重载),利用List传送SQL语句
    /// </summary>
    /// <param name="dtSql">需要执行的SQL集合</param>
    public static void ExcuteTransactionSql(List<string> dtSql)
    {
        Database db = DatabaseFactory.CreateDatabase();
        string str = string.Empty;

        using (DbConnection con = db.CreateConnection())
        {
            con.Open();
            DbTransaction dbtr = con.BeginTransaction();
            try
            {
                foreach (string sql in dtSql)
                {
                    DbCommand dbCommand = db.GetSqlStringCommand(ReplaceSQL(sql));
                    db.ExecuteNonQuery(dbCommand, dbtr);
                }
                dbtr.Commit();
            }
            catch (Exception ex)
            {
                dbtr.Rollback();
                throw ex;
            }
        }
    }

    /// <summary>   
    /// 执行SQL语句返回DataTable
    /// </summary>
    /// <param name="dbstring">SQL语句</param> 
    /// <returns>返回结果的DataTable</returns>
    public static DataTable ExecuteDataTable(string dbstring, params SqlParameter[] sqlparam)
    {

        Database db = DatabaseFactory.CreateDatabase();
        dbstring = ReplaceSQL(dbstring);
        DbCommand dbCommand = db.GetSqlStringCommand(dbstring);
        dbCommand.Parameters.AddRange(sqlparam);
        DataTable dt = new DataTable();


        try
        {
            //HttpContext.Current.Response.Write(dbstring + "<br>");
            dt = db.ExecuteDataSet(dbCommand).Tables[0];
        }
        catch (Exception e)
        {
            db = null;
            LogInfo.Add(dbstring, e.Message);
            //utils.ShowErrorPage("执行SQL语句时时出错：\r\n" + dbstring + "\r\n\r\n下面为具体错误信息：\r\n" + e.Message);
        }

        return dt;
    }


    /// <summary>
    /// 执行无返回的SQL语句
    /// </summary>
    /// <param name="dbstring">SQL语句</param>
    /// <returns>是否成功，0失败，1成功</returns>
    public static int ExecuteNonQuery(string dbstring,params SqlParameter[] sqlparam)
    {
        // dbstring = RewriteQueryInMultiLangMode(dbstring);

        Database db = DatabaseFactory.CreateDatabase();
        dbstring = ReplaceSQL(dbstring);
        DbCommand dbCommand = db.GetSqlStringCommand(dbstring);
        dbCommand.Parameters.AddRange(sqlparam);
        try
        {
            return db.ExecuteNonQuery(dbCommand);
        }
        catch (Exception e)
        {
            //utils.ShowErrorPage("执行SQL语句时时出错：\r\n" + dbstring + "\r\n\r\n下面为具体错误信息：\r\n" + e.Message);
            //return 1;
            LogInfo.Add(dbstring, e.Message);
            return 0;
        }
    }

    /// <summary>
    /// 执行无返回的SQL语句
    /// </summary>
    /// <param name="dbstring">SQL语句</param>
    /// <returns>是否成功，0失败，1成功</returns>
    public static int ExecuteScalar(string dbstring, params SqlParameter[] sqlparam)
    {
        // dbstring = RewriteQueryInMultiLangMode(dbstring);

        Database db = DatabaseFactory.CreateDatabase();
        dbstring = ReplaceSQL(dbstring);
        DbCommand dbCommand = db.GetSqlStringCommand(dbstring);
        dbCommand.Parameters.AddRange(sqlparam);
        try
        {
            return Convert.ToInt32(db.ExecuteScalar(dbCommand));
        }
        catch (Exception e)
        {
            //utils.ShowErrorPage("执行SQL语句时时出错：\r\n" + dbstring + "\r\n\r\n下面为具体错误信息：\r\n" + e.Message);
            //return 1;
            LogInfo.Add(dbstring, e.Message);
            return 0;
        }
    }
    /// <summary>
    /// 替换注入代码
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private static string ReplaceSQL(string str)
    {
        str = Regex.Replace(str, "exec", "", RegexOptions.IgnoreCase);
        str = Regex.Replace(str, "declare", "", RegexOptions.IgnoreCase);
        str = Regex.Replace(str, "@@Fetch_Status", "", RegexOptions.IgnoreCase);

        return str;
    }
    //一个订购用户一张表
    public static string GetRealTable(string nickNo)
    {
        return "TopVisitInfo_" + nickNo;
    }
}
