using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Common;
using System.Data;

public partial class top_groupbuy_LoadAjax : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int mid = Request.QueryString["mid"] == null ? 0 : int.Parse(Request.QueryString["mid"].ToString());
        int d = 10;
        string sql = "select * from  TopMission  WHERE id = " + mid.ToString();
        DataTable dt= utils.ExecuteDataTable(sql);
        if (dt != null)
        {
            int tol = int.Parse(dt.Rows[0]["total"].ToString());//总数
            int su = int.Parse(dt.Rows[0]["success"].ToString()) + int.Parse(dt.Rows[0]["fail"].ToString());//已完成

            int num = 100;
            if (tol != 0)
            {
                num = (su / tol) * 100;
            }

            Response.Write(num.ToString());
            Response.End();
        }
        else {
            Response.Write(d.ToString());
            Response.End();
        }
       
    }
}