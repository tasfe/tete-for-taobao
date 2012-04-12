using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Qijia.Model;
using DBHelp;
using System.Data;
namespace Qijia.DAL
{
    public class Jia_ItemService
    {
        public IList<Jia_Item> GetAllJia_Item()
        {
            string sql = "select * from Jia_Item";
            return Jia_ItemPropertity(sql);
        }
        public int AddJia_Item(Jia_Item jia_item)
        {
            string sql = "insert Jia_Item values(@ItemId,@TplId,@UpdateDate,@PropertyText)";
            SqlParameter[] param = CreateParameter(jia_item);
            return DBHelper.ExecuteNonQuery(sql, param);
        }
        public int ModifyJia_Item(Jia_Item jia_item)
        {
            string sql = "update Jia_Item set ItemId=@ItemId,TplId=@TplId,UpdateDate=@UpdateDate,PropertyText=@PropertyText where Nick=@Nick";
            SqlParameter[] param = CreateParameter(jia_item);
            return DBHelper.ExecuteNonQuery(sql, param);
        }
        public int DeleteJia_Item(int jia_itemId)
        {
            string sql = "delete from Jia_Item where Nick=" + jia_itemId;
            return DBHelper.ExecuteNonQuery(sql);
        }
        public Jia_Item GetJia_ItemById(int jia_itemId)
        {
            string sql = "select * from Jia_Item where Nick=" + jia_itemId;
            IList<Jia_Item> list = Jia_ItemPropertity(sql);
            return list.Count == 0 ? null : list[0];
        }
        private SqlParameter[] CreateParameter(Jia_Item jia_item)
        {
            SqlParameter[] param = new SqlParameter[]
                    {
                      new SqlParameter("@Nick",jia_item.Nick),
                      new SqlParameter("@ItemId",jia_item.ItemId),
                      new SqlParameter("@TplId",jia_item.TplId),
                      new SqlParameter("@UpdateDate",jia_item.UpdateDate),
                      new SqlParameter("@PropertyText",jia_item.PropertyText)
                    };
            return param;
        }
        private IList<Jia_Item> Jia_ItemPropertity(string sql)
        {
            DataTable dt = DBHelper.ExecuteDataTable(sql);
            IList<Jia_Item> list = new List<Jia_Item>();
            foreach (DataRow dr in dt.Rows)
            {
                Jia_Item jia_item = new Jia_Item();
                jia_item.Nick = Convert.ToString(dr["Nick"]);
                jia_item.ItemId = Convert.ToString(dr["ItemId"]);
                jia_item.TplId = Convert.ToString(dr["TplId"]);
                jia_item.UpdateDate = Convert.ToDateTime(dr["UpdateDate"]);
                jia_item.PropertyText = Convert.ToString(dr["PropertyText"]);
                list.Add(jia_item);
            }
            return list;
        }
    }
}
