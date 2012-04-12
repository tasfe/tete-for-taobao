using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Qijia.Model;
using DBHelp;
using System.Data;
namespace Qijia.DAL
{
    public class Jia_ImgService
    {
        public IList<Jia_Img> GetAllJia_Img()
        {
            string sql = "select * from Jia_Img";
            return Jia_ImgPropertity(sql);
        }
        public int AddJia_Img(Jia_Img jia_img)
        {
            string sql = "insert Jia_Img values(@TplId,@Tag,@JiaImg)";
            SqlParameter[] param = CreateParameter(jia_img);
            return DBHelper.ExecuteNonQuery(sql, param);
        }
        public int ModifyJia_Img(Jia_Img jia_img)
        {
            string sql = "update Jia_Img set TplId=@TplId,Tag=@Tag,JiaImg=@JiaImg where Guid=@Guid";
            SqlParameter[] param = CreateParameter(jia_img);
            return DBHelper.ExecuteNonQuery(sql, param);
        }
        public int DeleteJia_Img(int jia_imgId)
        {
            string sql = "delete from Jia_Img where Guid=" + jia_imgId;
            return DBHelper.ExecuteNonQuery(sql);
        }
        public Jia_Img GetJia_ImgById(int jia_imgId)
        {
            string sql = "select * from Jia_Img where Guid=" + jia_imgId;
            IList<Jia_Img> list = Jia_ImgPropertity(sql);
            return list.Count == 0 ? null : list[0];
        }
        private SqlParameter[] CreateParameter(Jia_Img jia_img)
        {
            SqlParameter[] param = new SqlParameter[]
                    {
                      new SqlParameter("@Guid",jia_img.Guid),
                      new SqlParameter("@TplId",jia_img.TplId),
                      new SqlParameter("@Tag",jia_img.Tag),
                      new SqlParameter("@JiaImg",jia_img.JiaImg)
                    };
            return param;
        }
        private IList<Jia_Img> Jia_ImgPropertity(string sql)
        {
            DataTable dt = DBHelper.ExecuteDataTable(sql);
            IList<Jia_Img> list = new List<Jia_Img>();
            foreach (DataRow dr in dt.Rows)
            {
                Jia_Img jia_img = new Jia_Img();
                jia_img.Guid = Convert.ToString(dr["Guid"]);
                jia_img.TplId = Convert.ToString(dr["TplId"]);
                jia_img.Tag = Convert.ToString(dr["Tag"]);
                jia_img.JiaImg = Convert.ToString(dr["JiaImg"]);
                list.Add(jia_img);
            }
            return list;
        }
    }
}
