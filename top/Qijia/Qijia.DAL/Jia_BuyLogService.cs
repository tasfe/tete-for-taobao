using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Qijia.Model;
using DBHelp;
using System.Data;
namespace Qijia.DAL
{
    public class Jia_BuyLogService
    {
        public IList<Jia_BuyLog> GetAllJia_BuyLog()
        {
            string sql = "select * from Jia_BuyLog";
            return Jia_BuyLogPropertity(sql);
        }

        public IList<Jia_BuyLog> GetAllJia_BuyLog(string nick)
        {
            string sql = "select * from Jia_BuyLog where Nick=@nick";
            SqlParameter param = new SqlParameter("@nick", nick);

            return Jia_BuyLogPropertity(sql, param);
        }

        public int AddJia_BuyLog(Jia_BuyLog jia_buylog)
        {
            string sql = "insert Jia_BuyLog values(@Nick,@Type,@Price,@BuyDate,@IsOld,@AddDate)";
            SqlParameter[] param = CreateParameter(jia_buylog);
            return DBHelper.ExecuteNonQuery(sql, param);
        }
        public int ModifyJia_BuyLog(Jia_BuyLog jia_buylog)
        {
            string sql = "update Jia_BuyLog set Nick=@Nick,Type=@Type,Price=@Price,BuyDate=@BuyDate,IsOld=@IsOld,AddDate=@AddDate where Guid=@Guid";
            SqlParameter[] param = CreateParameter(jia_buylog);
            return DBHelper.ExecuteNonQuery(sql, param);
        }
        public int DeleteJia_BuyLog(int jia_buylogId)
        {
            string sql = "delete from Jia_BuyLog where Guid=" + jia_buylogId;
            return DBHelper.ExecuteNonQuery(sql);
        }
        public Jia_BuyLog GetJia_BuyLogById(int jia_buylogId)
        {
            string sql = "select * from Jia_BuyLog where Guid=" + jia_buylogId;
            IList<Jia_BuyLog> list = Jia_BuyLogPropertity(sql);
            return list.Count == 0 ? null : list[0];
        }
        private SqlParameter[] CreateParameter(Jia_BuyLog jia_buylog)
        {
            SqlParameter[] param = new SqlParameter[]
                    {
                      new SqlParameter("@Guid",jia_buylog.Guid),
                      new SqlParameter("@Nick",jia_buylog.Nick),
                      new SqlParameter("@Type",jia_buylog.Type),
                      new SqlParameter("@Price",jia_buylog.Price),
                      new SqlParameter("@BuyDate",jia_buylog.BuyDate),
                      new SqlParameter("@IsOld",jia_buylog.IsOld),
                      new SqlParameter("@AddDate",jia_buylog.AddDate)
                    };
            return param;
        }
        private IList<Jia_BuyLog> Jia_BuyLogPropertity(string sql, params SqlParameter[] param)
        {
            DataTable dt = DBHelper.ExecuteDataTable(sql, param);
            IList<Jia_BuyLog> list = new List<Jia_BuyLog>();
            foreach (DataRow dr in dt.Rows)
            {
                Jia_BuyLog jia_buylog = new Jia_BuyLog();
                jia_buylog.Guid = Convert.ToString(dr["Guid"]);
                jia_buylog.Nick = Convert.ToString(dr["Nick"]);
                jia_buylog.Type = Convert.ToString(dr["Type"]);
                jia_buylog.Price = Convert.ToDecimal(dr["Price"]);
                jia_buylog.BuyDate = Convert.ToInt32(dr["BuyDate"]);
                jia_buylog.IsOld = Convert.ToInt32(dr["IsOld"]);
                jia_buylog.AddDate = Convert.ToDateTime(dr["AddDate"]);
                list.Add(jia_buylog);
            }
            return list;
        }
    }
}
