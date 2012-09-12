using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBHelp;

public partial class detail_fuwuLog : System.Web.UI.Page
{
    public string id = string.Empty;
    public string nick = string.Empty;
    public string nickid = string.Empty;
    public string nickurl = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        id = Request.QueryString["id"] == null ? "0" : Request.QueryString["id"].ToString();
        nick = Request.QueryString["nick"] == null ? "" : Request.QueryString["nick"].ToString();
        nickid = Request.QueryString["nickid"] == null ? "" : Request.QueryString["nickid"].ToString();

        nickurl = HttpUtility.UrlEncode(nick);

        string sql = "SELECT * FROM Jia_Shop WHERE nick = '" + nick + "'";
        DataTable dt = DBHelper.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            this.lbEndDate.Text = dt.Rows[0]["ExpireDate"].ToString();
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (id == "0")
        {
            this.Label1.Text = "您可以在发布或者编辑商品时使用我们的生成宝贝描述模板功能！";
        }
        else
        {
            Response.Redirect("dialog.aspx?nick=" + HttpUtility.UrlEncode(nick) + "&nickid=" + nickid + "&id=" + id);
        }
    }
}