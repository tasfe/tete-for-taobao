using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Common;

public partial class iphoneapi_wdjf : System.Web.UI.Page
{
    public string score = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string token = "test";
        string sql = "SELECT * FROM MP_Token  WHERE token = '" + token + "'";

        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            score = dt.Rows[0]["score"].ToString();
        }
    }
}