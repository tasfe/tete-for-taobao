using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Qijia.Model;
using DBHelp;
using System.Data;
namespace Qijia.DAL
{
    public class Jia_ApiFailLogService
    {
        public IList<Jia_ApiFailLog> GetAllJia_ApiFailLog()
        {
            string sql = "select * from Jia_ApiFailLog";
            return Jia_ApiFailLogPropertity(sql);
        }
        public int AddJia_ApiFailLog(Jia_ApiFailLog jia_apifaillog)
        {
            string sql = "insert Jia_ApiFailLog values(@ApiName,@ActDate,@Data,@ErrInfo)";
            SqlParameter[] param = CreateParameter(jia_apifaillog);
            return DBHelper.ExecuteNonQuery(sql, param);
        }
        public int ModifyJia_ApiFailLog(Jia_ApiFailLog jia_apifaillog)
        {
            string sql = "update Jia_ApiFailLog set ApiName=@ApiName,ActDate=@ActDate,Data=@Data,ErrInfo=@ErrInfo where Guid=@Guid";
            SqlParameter[] param = CreateParameter(jia_apifaillog);
            return DBHelper.ExecuteNonQuery(sql, param);
        }
        public int DeleteJia_ApiFailLog(int jia_apifaillogId)
        {
            string sql = "delete from Jia_ApiFailLog where Guid=" + jia_apifaillogId;
            return DBHelper.ExecuteNonQuery(sql);
        }
        public Jia_ApiFailLog GetJia_ApiFailLogById(int jia_apifaillogId)
        {
            string sql = "select * from Jia_ApiFailLog where Guid=" + jia_apifaillogId;
            IList<Jia_ApiFailLog> list = Jia_ApiFailLogPropertity(sql);
            return list.Count == 0 ? null : list[0];
        }
        private SqlParameter[] CreateParameter(Jia_ApiFailLog jia_apifaillog)
        {
            SqlParameter[] param = new SqlParameter[]
                    {
                      new SqlParameter("@Guid",jia_apifaillog.Guid),
                      new SqlParameter("@ApiName",jia_apifaillog.ApiName),
                      new SqlParameter("@ActDate",jia_apifaillog.ActDate),
                      new SqlParameter("@Data",jia_apifaillog.Data),
                      new SqlParameter("@ErrInfo",jia_apifaillog.ErrInfo)
                    };
            return param;
        }
        private IList<Jia_ApiFailLog> Jia_ApiFailLogPropertity(string sql)
        {
            DataTable dt = DBHelper.ExecuteDataTable(sql);
            IList<Jia_ApiFailLog> list = new List<Jia_ApiFailLog>();
            foreach (DataRow dr in dt.Rows)
            {
                Jia_ApiFailLog jia_apifaillog = new Jia_ApiFailLog();
                jia_apifaillog.Guid = Convert.ToString(dr["Guid"]);
                jia_apifaillog.ApiName = Convert.ToString(dr["ApiName"]);
                jia_apifaillog.ActDate = Convert.ToDateTime(dr["ActDate"]);
                jia_apifaillog.Data = Convert.ToString(dr["Data"]);
                jia_apifaillog.ErrInfo = Convert.ToString(dr["ErrInfo"]);
                list.Add(jia_apifaillog);
            }
            return list;
        }
    }
}
