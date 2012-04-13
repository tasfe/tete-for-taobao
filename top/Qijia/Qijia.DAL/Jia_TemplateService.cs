using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Qijia.Model;
using DBHelp;
using System.Data;

namespace Qijia.DAL
{
    public class Jia_TemplateService
    {
        public IList<Jia_Template> GetAllJia_Template()
        {
            string sql = "select * from Jia_Template";
            return Jia_TemplatePropertity(sql);
        }
        public int AddJia_Template(Jia_Template jia_template)
        {
            string sql = "insert Jia_Template(TplId,Tplname,OrderIid,Count,TplImg,TplShort,TplHtml,CateId,UglyTplHtml) values(@TplId,@Tplname,@OrderIid,@Count,@TplImg,@TplShort,@TplHtml,@CateId,@UglyTplHtml)";
            SqlParameter[] param = CreateParameter(jia_template);
            return DBHelper.ExecuteNonQuery(sql, param);
        }
        public int ModifyJia_Template(Jia_Template jia_template)
        {
            string sql = "update Jia_Template set Tplname=@Tplname,OrderIid=@OrderIid,Count=@Count,TplImg=@TplImg,TplShort=@TplShort,TplHtml=@TplHtml,CateId=@CateId,UglyTplHtml=@UglyTplHtml where TplId=@TplId";
            SqlParameter[] param = CreateParameter(jia_template);
            return DBHelper.ExecuteNonQuery(sql, param);
        }
        public int DeleteJia_Template(string jia_templateId)
        {
            string sql = "delete from Jia_Template where TplId=@TplId";
            SqlParameter param = new SqlParameter("@TplId", jia_templateId);
            return DBHelper.ExecuteNonQuery(sql, param);
        }
        public Jia_Template GetJia_TemplateById(string jia_templateId)
        {
            string sql = "select * from Jia_Template where TplId=@TplId";
            SqlParameter param = new SqlParameter("@TplId", jia_templateId);
            IList<Jia_Template> list = Jia_TemplatePropertity(sql, param);
            return list.Count == 0 ? null : list[0];
        }
        private SqlParameter[] CreateParameter(Jia_Template jia_template)
        {
            SqlParameter[] param = new SqlParameter[]
                    {
                      new SqlParameter("@TplId",jia_template.TplId),
                      new SqlParameter("@Tplname",jia_template.Tplname),
                      new SqlParameter("@OrderIid",jia_template.OrderIid),
                      new SqlParameter("@Count",jia_template.Count),
                      new SqlParameter("@TplImg",jia_template.TplImg),
                      new SqlParameter("@TplShort",jia_template.TplShort),
                      new SqlParameter("@TplHtml",jia_template.TplHtml),
                      new SqlParameter("@CateId",jia_template.CateId),
                      new SqlParameter("@UglyTplHtml",jia_template.UglyTplHtml)
                    };
            return param;
        }
        private IList<Jia_Template> Jia_TemplatePropertity(string sql, params SqlParameter[] param)
        {
            DataTable dt = DBHelper.ExecuteDataTable(sql, param);
            IList<Jia_Template> list = new List<Jia_Template>();
            foreach (DataRow dr in dt.Rows)
            {
                Jia_Template jia_template = new Jia_Template();
                jia_template.TplId = Convert.ToString(dr["TplId"]);
                jia_template.Tplname = Convert.ToString(dr["Tplname"]);
                jia_template.OrderIid = Convert.ToInt32(dr["OrderIid"]);
                jia_template.Count = Convert.ToInt32(dr["Count"]);
                jia_template.TplImg = Convert.ToString(dr["TplImg"]);
                jia_template.TplShort = Convert.ToString(dr["TplShort"]);
                jia_template.TplHtml = Convert.ToString(dr["TplHtml"]);
                jia_template.CateId = Convert.ToString(dr["CateId"]);
                jia_template.UglyTplHtml = Convert.ToString(dr["UglyTplHtml"]);
                list.Add(jia_template);
            }
            return list;
        }
    }
}
