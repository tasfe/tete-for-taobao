using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBHelp;

public partial class detail_fuwuLoglist : System.Web.UI.Page
{
    public string nick = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        nick = Request.QueryString["nick"] == null ? "" : Request.QueryString["nick"].ToString();
        string sql = "SELECT * FROM Jia_BuyLog WHERE nick = '" + nick + "' ORDER BY adddate DESC";
        DataTable dt = DBHelper.ExecuteDataTable(sql);

        this.rpt.DataSource = dt;
        rpt.DataBind();
    }
}