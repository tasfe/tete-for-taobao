using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;

public partial class iphoneapi_msgcheck : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string ispass = utils.NewRequest("ispass", utils.RequestType.QueryString);

        string sql = "SELECT * FROM HuliUserMsg WHERE ispass = 0 ORDER BY adddate DESC";

        if (ispass == "0")
            sql = "SELECT * FROM HuliUserMsg WHERE ispass <> 0 ORDER BY adddate DESC";

        DataTable dt = utils.ExecuteDataTable(sql);

        rptList.DataSource = dt;
        rptList.DataBind();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string ids = utils.NewRequest("ids", utils.RequestType.Form);

        string sql = "UPDATE HuliUserMsg SET ispass = 1 WHERE CHARINDEX(guid, '"+ids+"') > 0";
        utils.ExecuteNonQuery(sql);

        Response.Redirect("msgcheck.aspx");
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        string ids = utils.NewRequest("ids", utils.RequestType.Form);

        string sql = "UPDATE HuliUserMsg SET ispass = 2 WHERE CHARINDEX(guid, '" + ids + "') > 0";
        utils.ExecuteNonQuery(sql);

        Response.Redirect("msgcheck.aspx");
    }
}