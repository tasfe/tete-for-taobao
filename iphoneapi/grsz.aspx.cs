using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Common;

public partial class iphoneapi_grsz : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = utils.NewRequest("act", utils.RequestType.Form);
        string token = "test";
        string sql = "SELECT * FROM MP_Token  WHERE token = '" + token + "'";

        if (act == "save")
        {
            string truename = utils.NewRequest("truename", utils.RequestType.Form).Replace("'", "''");
            string address = utils.NewRequest("address", utils.RequestType.Form).Replace("'", "''");
            string birthday = utils.NewRequest("birthday", utils.RequestType.Form).Replace("'", "''");
            string email = utils.NewRequest("email", utils.RequestType.Form).Replace("'", "''");
            string weibo = utils.NewRequest("weibo", utils.RequestType.Form).Replace("'", "''");
            string mobile = utils.NewRequest("mobile", utils.RequestType.Form).Replace("'", "''");

            sql = "UPDATE MP_Token SET truename = '" + truename + "',address = '" + address + "',birthday = '" + birthday + "',email = '" + email + "',weibo = '" + weibo + "',mobile = '" + mobile + "' WHERE token = '" + token + "'";
            utils.ExecuteNonQuery(sql);
            Response.Write("<script>alert('保存成功！');window.location.href='grsz.aspx';</script>");
            Response.End();
            return;
        }

        DataTable dt = utils.ExecuteDataTable(sql);
        rptDetail.DataSource = dt;
        rptDetail.DataBind();
    }
}