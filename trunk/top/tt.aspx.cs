using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Common;

public partial class top_tt : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string sql = "SELECT url,COUNT(url) AS num FROM [TopTongji] WHERE id > 40886 GROUP BY url";
        DataTable dt = utils.ExecuteDataTable(sql);

        rptResult.DataSource = dt;
        rptResult.DataBind();
    }
}