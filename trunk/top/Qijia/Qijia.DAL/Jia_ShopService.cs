using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Qijia.Model;
using DBHelp;
using System.Data;
namespace Qijia.DAL
{
    public class Jia_ShopService
    {
        public IList<Jia_Shop> GetAllJia_Shop()
        {
            string sql = "select * from Jia_Shop";
            return Jia_ShopPropertity(sql);
        }
        public int AddJia_Shop(Jia_Shop jia_shop)
        {
            string sql = "insert Jia_Shop values(@ShopId,@IsExpired,@ExpireDate,@AddDate)";
            SqlParameter[] param = CreateParameter(jia_shop);
            return DBHelper.ExecuteNonQuery(sql, param);
        }
        public int ModifyJia_Shop(Jia_Shop jia_shop)
        {
            string sql = "update Jia_Shop set ShopId=@ShopId,IsExpired=@IsExpired,ExpireDate=@ExpireDate,AddDate=@AddDate where Nick=@Nick";
            SqlParameter[] param = CreateParameter(jia_shop);
            return DBHelper.ExecuteNonQuery(sql, param);
        }
        public int DeleteJia_Shop(string nick)
        {
            string sql = "delete from Jia_Shop where Nick=@Nick";
            SqlParameter param = new SqlParameter("@Nick", nick);
            return DBHelper.ExecuteNonQuery(sql, param);
        }
        public Jia_Shop GetJia_ShopByNick(string nick)
        {
            string sql = "select * from Jia_Shop where Nick=@Nick";
            SqlParameter param = new SqlParameter("@Nick", nick);
            IList<Jia_Shop> list = Jia_ShopPropertity(sql, param);
            return list.Count == 0 ? null : list[0];
        }
        private SqlParameter[] CreateParameter(Jia_Shop jia_shop)
        {
            SqlParameter[] param = new SqlParameter[]
                    {
                      new SqlParameter("@Nick",jia_shop.Nick),
                      new SqlParameter("@ShopId",jia_shop.ShopId),
                      new SqlParameter("@IsExpired",jia_shop.IsExpired),
                      new SqlParameter("@ExpireDate",jia_shop.ExpireDate),
                      new SqlParameter("@AddDate",jia_shop.AddDate)
                    };
            return param;
        }
        private IList<Jia_Shop> Jia_ShopPropertity(string sql, params SqlParameter[] param)
        {
            DataTable dt = DBHelper.ExecuteDataTable(sql, param);
            IList<Jia_Shop> list = new List<Jia_Shop>();
            foreach (DataRow dr in dt.Rows)
            {
                Jia_Shop jia_shop = new Jia_Shop();
                jia_shop.Nick = Convert.ToString(dr["Nick"]);
                jia_shop.ShopId = Convert.ToString(dr["ShopId"]);
                jia_shop.IsExpired = Convert.ToInt32(dr["IsExpired"]);
                jia_shop.ExpireDate = Convert.ToDateTime(dr["ExpireDate"]);
                jia_shop.AddDate = Convert.ToDateTime(dr["AddDate"]);
                list.Add(jia_shop);
            }
            return list;
        }
    }
}
