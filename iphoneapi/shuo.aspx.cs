using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;

public partial class iphoneapi_shuoshuo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string sql = "SELECT * FROM MP_Gbook ORDER BY adddate DESC";
        DataTable dt = utils.ExecuteDataTable(sql);

        rptShuo.DataSource = dt;
        rptShuo.DataBind();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string content = utils.NewRequest("content", utils.RequestType.Form);
        string sql = "INSERT INTO MP_Gbook (token,content) VALUES ('', '" + content.Replace("'","''") + "')";

        utils.ExecuteNonQuery(sql);

        Response.Write("<script>alert('发表成功！');window.location.href='shuo.aspx';</script>");
        Response.End();
    }
}