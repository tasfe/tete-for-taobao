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


    protected void Button3_Click(object sender, EventArgs e)
    {
        string sql = "SELECT * FROM TCS_ShopSession WHERE nick = '" + this.TextBox1.Text + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            Response.Write("该客户已经进过服务");
        }
        else
        {
            Response.Write("该客户没有进过服务！！");
        }
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        if (TextBox6.Text != "xiaoman")
        {
            return;
        }

        string sql = "INSERT INTO [TeteCrmSaas].[dbo].[TCS_PayLog]([typ],[adddate],[nextdate],[enddate],[nick],[mouth],[count])VALUES('" + TextBox5.Text + "',GETDATE(),GETDATE(),GETDATE(),'" + TextBox3.Text + "',12," + TextBox4.Text + ")";
        utils.ExecuteNonQuery(sql);

        sql = "UPDATE TCS_ShopConfig SET total = total + " + TextBox4.Text + " WHERE nick = '" + TextBox3.Text + "'";
        utils.ExecuteNonQuery(sql);

        sql = "SELECT total FROM TCS_ShopConfig WHERE nick = '" + TextBox3.Text + "'";
        string count = utils.ExecuteString(sql);

        Response.Write("该客户短信剩余条数为【" + count + "】条");
    }
}