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

        /// <summary>
        /// 搜索订购指定服务的用户信息
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public IList<TopNickSessionInfo> GetAllNickSession(TopTaoBaoService serviceId)
        {
            SqlParameter param = new SqlParameter("@ServiceID", (int)serviceId);
            IList<TopNickSessionInfo> list = new List<TopNickSessionInfo>();
            DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_NICKNO, param);
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

        public int UpdateNickShop(string nick, string shopId)
        {
            SqlParameter[] param = new[]

                {
                    new SqlParameter("@ShopId",shopId),
                    new SqlParameter("@nick",nick)
                };
            return DBHelper.ExecuteNonQuery(SQL_UPDATE_SHOPID, param);
        }
    }
}
