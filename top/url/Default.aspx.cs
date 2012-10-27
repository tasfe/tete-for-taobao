using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Common;

public partial class top_url_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string guid = Request.QueryString.ToString().Replace("'", "''");

        string sql = "SELECT * FROM TCS_Url WHERE guid = '" + guid + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            sql = "UPDATE TCS_Url SET click = click + 1 WHERE guid = '" + guid + "'";
            utils.ExecuteNonQuery(sql);

            Response.Redirect(dt.Rows[0]["url"].ToString());
        }
    }
}