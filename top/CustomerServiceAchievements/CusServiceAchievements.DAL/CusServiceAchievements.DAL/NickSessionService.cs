using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using System.Data;
using DBHelp;
using Enum;
using System.Data.SqlClient;

namespace CusServiceAchievements.DAL
{
    public class NickSessionService
    {
        const string SQL_SELECT_NICKNO = "SELECT nick,session,NickState,LastGetOrderTime,ShopId FROM TopNickSession WHERE ServiceID=@ServiceID";

        const string SQL_UPDATE_SHOPID = "UPDATE TopNickSession SET ShopId=@ShopId WHERE nick=@nick";

        const string SQL_INSERT_NEW = "INSERT TopNickSession(nick,session,JoinDate,NickState,LastGetOrderTime,ServiceID,ShopId) VALUES(@nick,@session,@JoinDate,@NickState,@LastGetOrderTime,@ServiceID,@ShopId)";

        const string SQL_SELECT_ALL = "SELECT nick,session,NickState,LastGetOrderTime,ServiceID,ShopId FROM TopNickSession";

        const string SQL_UPDATE = "UPDATE TopNickSession SET NickState=@NickState,session=@session,JoinDate=@JoinDate";

        /// <summary>
        /// 搜索订购指定服务的用户信息
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public IList<TopNickSessionInfo> GetAllNickSession(TopTaoBaoService serviceId)
        {
            SqlParameter param = new SqlParameter("@ServiceID", (int)serviceId);
            IList<TopNickSessionInfo> list = new List<TopNickSessionInfo>();
            using (SqlDataReader dr = ServiceDBHelper.CreateReader(SQL_SELECT_NICKNO, param))
            {
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        TopNickSessionInfo info = new TopNickSessionInfo();
                        info.Nick = dr["nick"].ToString();
                        info.Session = dr["session"].ToString();
                        info.NickState = (bool)dr["NickState"];
                        info.LastGetOrderTime = dr["LastGetOrderTime"] == DBNull.Value ? DateTime.MinValue : DateTime.Parse(dr["LastGetOrderTime"].ToString());
                        info.ShopId = dr["ShopId"].ToString();
                        list.Add(info);
                    }
                }
            }
            return list;
        }

        public int UpdateNickShop(string nick, string shopId)
        {
            SqlParameter[] param = new[]

                {
                    new SqlParameter("@ShopId",shopId),
                    new SqlParameter("@nick",nick)
                };
            return ServiceDBHelper.ExcuteSql(SQL_UPDATE_SHOPID, param);
        }

        public IList<TopNickSessionInfo> GetAllNickSession()
        {
            IList<TopNickSessionInfo> list = new List<TopNickSessionInfo>();
            DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_ALL);
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

            return DBHelper.ExecuteNonQuery(SQL_INSERT_NEW, plist.ToArray());
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
    }
}
