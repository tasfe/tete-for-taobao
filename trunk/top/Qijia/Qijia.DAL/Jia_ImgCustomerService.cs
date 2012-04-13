using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Qijia.Model;
using DBHelp;
using System.Data;
namespace Qijia.DAL
{
    public class Jia_ImgCustomerService
    {
        public IList<Jia_ImgCustomer> GetAllJia_ImgCustomer(string itemId)
        {
            string sql = "select * from Jia_ImgCustomer where ItemId=@ItemId";
            SqlParameter param = new SqlParameter("@ItemId", itemId);
            return Jia_ImgCustomerPropertity(sql, param);
        }
        public int AddJia_ImgCustomer(Jia_ImgCustomer jia_imgcustomer)
        {
            string sql = "insert Jia_ImgCustomer values(@ItemId,@Tag,@JiaImg)";
            SqlParameter[] param = CreateParameter(jia_imgcustomer);
            return DBHelper.ExecuteNonQuery(sql, param);
        }
        public int ModifyJia_ImgCustomer(Jia_ImgCustomer jia_imgcustomer)
        {
            string sql = "update Jia_ImgCustomer set ItemId=@ItemId,Tag=@Tag,JiaImg=@JiaImg where Guid=@Guid";
            SqlParameter[] param = CreateParameter(jia_imgcustomer);
            return DBHelper.ExecuteNonQuery(sql, param);
        }
        public int DeleteJia_ImgCustomer(string jia_imgcustomerId)
        {
            string sql = "delete from Jia_ImgCustomer where Guid=@jia_imgcustomerId";
            SqlParameter param = new SqlParameter("@jia_imgcustomerId", jia_imgcustomerId);
            return DBHelper.ExecuteNonQuery(sql, param);
        }
        public Jia_ImgCustomer GetJia_ImgCustomerById(int jia_imgcustomerId)
        {
            string sql = "select * from Jia_ImgCustomer where Guid=@jia_imgcustomerId";
            SqlParameter param = new SqlParameter("@jia_imgcustomerId", jia_imgcustomerId);
            IList<Jia_ImgCustomer> list = Jia_ImgCustomerPropertity(sql, param);
            return list.Count == 0 ? null : list[0];
        }
        private SqlParameter[] CreateParameter(Jia_ImgCustomer jia_imgcustomer)
        {
            SqlParameter[] param = new SqlParameter[]
                    {
                      new SqlParameter("@Guid",jia_imgcustomer.Guid),
                      new SqlParameter("@ItemId",jia_imgcustomer.ItemId),
                      new SqlParameter("@Tag",jia_imgcustomer.Tag),
                      new SqlParameter("@JiaImg",jia_imgcustomer.JiaImg)
                    };
            return param;
        }
        private IList<Jia_ImgCustomer> Jia_ImgCustomerPropertity(string sql,params SqlParameter[] param)
        {
            DataTable dt = DBHelper.ExecuteDataTable(sql, param);
            IList<Jia_ImgCustomer> list = new List<Jia_ImgCustomer>();
            foreach (DataRow dr in dt.Rows)
            {
                Jia_ImgCustomer jia_imgcustomer = new Jia_ImgCustomer();
                jia_imgcustomer.Guid = Convert.ToString(dr["Guid"]);
                jia_imgcustomer.ItemId = Convert.ToString(dr["ItemId"]);
                jia_imgcustomer.Tag = Convert.ToString(dr["Tag"]);
                jia_imgcustomer.JiaImg = Convert.ToString(dr["JiaImg"]);
                list.Add(jia_imgcustomer);
            }
            return list;
        }
    }
}
