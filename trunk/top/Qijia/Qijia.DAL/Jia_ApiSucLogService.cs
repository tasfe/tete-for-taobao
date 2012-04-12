using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Qijia.Model;
using DBHelp;
using System.Data;
namespace Qijia.DAL
{
    public class Jia_ApiSucLogService
    {
        public IList<Jia_ApiSucLog> GetAllJia_ApiSucLog()
        {
            string sql = "select * from Jia_ApiSucLog";
            return Jia_ApiSucLogPropertity(sql);
        }
        public int AddJia_ApiSucLog(Jia_ApiSucLog jia_apisuclog)
        {
            string sql = "insert Jia_ApiSucLog values(@ApiName,@ActDate,@Data)";
            SqlParameter[] param = CreateParameter(jia_apisuclog);
            return DBHelper.ExecuteNonQuery(sql, param);
        }
        public int ModifyJia_ApiSucLog(Jia_ApiSucLog jia_apisuclog)
        {
            string sql = "update Jia_ApiSucLog set ApiName=@ApiName,ActDate=@ActDate,Data=@Data where Guid=@Guid";
            SqlParameter[] param = CreateParameter(jia_apisuclog);
            return DBHelper.ExecuteNonQuery(sql, param);
        }
        public int DeleteJia_ApiSucLog(int jia_apisuclogId)
        {
            string sql = "delete from Jia_ApiSucLog where Guid=" + jia_apisuclogId;
            return DBHelper.ExecuteNonQuery(sql);
        }
        public Jia_ApiSucLog GetJia_ApiSucLogById(int jia_apisuclogId)
        {
            string sql = "select * from Jia_ApiSucLog where Guid=" + jia_apisuclogId;
            IList<Jia_ApiSucLog> list = Jia_ApiSucLogPropertity(sql);
            return list.Count == 0 ? null : list[0];
        }
        private SqlParameter[] CreateParameter(Jia_ApiSucLog jia_apisuclog)
        {
            SqlParameter[] param = new SqlParameter[]
                    {
                      new SqlParameter("@Guid",jia_apisuclog.Guid),
                      new SqlParameter("@ApiName",jia_apisuclog.ApiName),
                      new SqlParameter("@ActDate",jia_apisuclog.ActDate),
                      new SqlParameter("@Data",jia_apisuclog.Data)
                    };
            return param;
        }
        private IList<Jia_ApiSucLog> Jia_ApiSucLogPropertity(string sql)
        {
            DataTable dt = DBHelper.ExecuteDataTable(sql);
            IList<Jia_ApiSucLog> list = new List<Jia_ApiSucLog>();
            foreach (DataRow dr in dt.Rows)
            {
                Jia_ApiSucLog jia_apisuclog = new Jia_ApiSucLog();
                jia_apisuclog.Guid = Convert.ToString(dr["Guid"]);
                jia_apisuclog.ApiName = Convert.ToString(dr["ApiName"]);
                jia_apisuclog.ActDate = Convert.ToDateTime(dr["ActDate"]);
                jia_apisuclog.Data = Convert.ToString(dr["Data"]);
                list.Add(jia_apisuclog);
            }
            return list;
        }
    }
}
