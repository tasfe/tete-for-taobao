using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace teteactivitystart
{
    /// <summary>
    /// 数据据操作本地类
    /// </summary>
    public class DBSql
    {
        private SqlConnection mcnt = null;
        private SqlCommand mcmd = null;
        private SqlTransaction mtrans = null;

        private static DBSql instance = null;

        public static DBSql getInstance()
        {
            if (instance == null)
            {
                instance = new DBSql();
            }
            return instance;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public DBSql()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="strConnStr">连接字符串</param>
        public DBSql(string strConnStr)
        {
            string ConnectionString = "";

            ConnectionString = strConnStr;

            mcnt = new SqlConnection();
            mcnt.ConnectionString = ConnectionString;

            mcmd = new SqlCommand();

        }

        /// <summary>
        /// 打开数据库
        /// </summary>
        public void Open()
        {
            //try
            //{
            //    if (mcnt.State == ConnectionState.Closed)
            //    {
            //        mcnt.Open();
            //        mcmd.Connection = mcnt;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw (ex);
            //}
        }

        /// <summary>
        /// 关闭数据库
        /// </summary>
        public void Close()
        {
            //try
            //{
            //    if (mcnt.State == ConnectionState.Open)
            //    {
            //        mcnt.Close();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw (ex);
            //}
        }

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        public int ExecSql(string sql)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(sql);
            DataTable dt = null;

            try
            {
                dt = db.ExecuteDataSet(dbCommand).Tables[0];
                return 1;
            }
            catch (Exception e)
            {
                db = null;
                return 0;
            }

            //try
            //{
            //    if (mcnt.State == ConnectionState.Closed)
            //    {
            //        mcnt.Open();
            //        mcmd.Connection = mcnt;
            //    }
            //    mcmd.CommandText = sql;
            //    return mcmd.ExecuteNonQuery();
            //}
            //catch (Exception ex)
            //{
            //    //MessageBox.Show(sql + "--" + ex.ToString());
            //    return 0;
            //}
        }

        /// <summary>
        /// 取得查询数据
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public DataTable GetTable(string sql)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(sql);
            DataTable dt = null;

            try
            {
                dt = db.ExecuteDataSet(dbCommand).Tables[0];
                return dt;
            }
            catch (Exception e)
            {
                db = null;
                return new DataTable();
            }

            //try
            //{
            //    DataTable dt = new DataTable();
            //    SqlDataAdapter da = new SqlDataAdapter();

            //    if (mcnt.State == ConnectionState.Closed)
            //    {
            //        mcnt.Open();
            //        mcmd.Connection = mcnt;
            //    }
            //    da.SelectCommand = new SqlCommand(sql, mcnt);
            //    if (mtrans != null)
            //    {
            //        da.SelectCommand.Transaction = mtrans;
            //    }
            //    da.Fill(dt);
            //    return dt;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(sql + "--" + ex.ToString());
            //    return new DataTable();
            //}
        }

        /// <summary>
        /// 开始一个事务
        /// </summary>
        /// <returns></returns>
        public void BeginTran()
        {
            try
            {
                if (mcnt.State == ConnectionState.Closed)
                {
                    mcnt.Open();
                }
                mtrans = mcnt.BeginTransaction();
                mcmd.Transaction = mtrans;
            }
            catch (Exception ex)
            {
                mtrans = null;
                throw (ex);
            }
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        /// <returns></returns>
        public void Commit()
        {
            try
            {
                if (mtrans != null)
                {
                    mtrans.Commit();
                    mtrans = null;
                }
            }
            catch
            {
            }

        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollBack()
        {
            try
            {
                if (mtrans != null)
                {
                    mtrans.Rollback();
                    mtrans = null;
                }
            }
            catch
            {
            }

        }
    }
}
