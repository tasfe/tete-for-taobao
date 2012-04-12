using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Qijia.Model;
using DBHelp;
using System.Data;
namespace Qijia.DAL
{
    public class Jia_CategoryService
    {
        public IList<Jia_Category> GetAllJia_Category()
        {
            string sql = "select * from Jia_Category";
            return Jia_CategoryPropertity(sql);
        }
        public int AddJia_Category(Jia_Category jia_category)
        {
            string sql = "insert Jia_Category values(@CateName,@HeadId,@Tree,@Level)";
            SqlParameter[] param = CreateParameter(jia_category);
            return DBHelper.ExecuteNonQuery(sql, param);
        }
        public int ModifyJia_Category(Jia_Category jia_category)
        {
            string sql = "update Jia_Category set CateName=@CateName,HeadId=@HeadId,Tree=@Tree,Level=@Level where CateId=@CateId";
            SqlParameter[] param = CreateParameter(jia_category);
            return DBHelper.ExecuteNonQuery(sql, param);
        }
        public int DeleteJia_Category(int jia_categoryId)
        {
            string sql = "delete from Jia_Category where CateId=" + jia_categoryId;
            return DBHelper.ExecuteNonQuery(sql);
        }
        public Jia_Category GetJia_CategoryById(int jia_categoryId)
        {
            string sql = "select * from Jia_Category where CateId=" + jia_categoryId;
            IList<Jia_Category> list = Jia_CategoryPropertity(sql);
            return list.Count == 0 ? null : list[0];
        }
        private SqlParameter[] CreateParameter(Jia_Category jia_category)
        {
            SqlParameter[] param = new SqlParameter[]
                    {
                      new SqlParameter("@CateId",jia_category.CateId),
                      new SqlParameter("@CateName",jia_category.CateName),
                      new SqlParameter("@HeadId",jia_category.HeadId),
                      new SqlParameter("@Tree",jia_category.Tree),
                      new SqlParameter("@Level",jia_category.Level)
                    };
            return param;
        }
        private IList<Jia_Category> Jia_CategoryPropertity(string sql)
        {
            DataTable dt = DBHelper.ExecuteDataTable(sql);
            IList<Jia_Category> list = new List<Jia_Category>();
            foreach (DataRow dr in dt.Rows)
            {
                Jia_Category jia_category = new Jia_Category();
                jia_category.CateId = Convert.ToString(dr["CateId"]);
                jia_category.CateName = Convert.ToString(dr["CateName"]);
                jia_category.HeadId = Convert.ToString(dr["HeadId"]);
                jia_category.Tree = Convert.ToString(dr["Tree"]);
                jia_category.Level = Convert.ToInt32(dr["Level"]);
                list.Add(jia_category);
            }
            return list;
        }
    }
}
