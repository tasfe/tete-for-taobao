using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Common;

public partial class iphoneapi_wddd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string token = "test";
        string sql = "SELECT * FROM MP_TokenOrder WHERE token = '" + token + "'";

        DataTable dt = utils.ExecuteDataTable(sql);

        rptOrder.DataSource = dt;
        rptOrder.DataBind();
    }
}