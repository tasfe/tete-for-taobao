using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Common;

public partial class top_reviewnew_search : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (TextBox2.Text != "xiaoman")
        {
            return;
        }

        string sql = "SELECT * FROM TCS_ShopConfig WHERE nick = '" + this.TextBox1.Text + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            Response.Write("该客户的手机号是" + dt.Rows[0]["phone"].ToString() + "，QQ号码是" + dt.Rows[0]["qq"].ToString());
        }
    }
}