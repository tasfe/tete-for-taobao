using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;

public partial class iphoneapi_zhsz : System.Web.UI.Page
{
    public string check1 = string.Empty;
    public string check2 = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string isqq = utils.NewRequest("isqq", utils.RequestType.QueryString);
        string issina = utils.NewRequest("issina", utils.RequestType.QueryString);
        string token = "test";
        string sql = string.Empty;

        if (isqq == "1")
        {
            sql = "UPDATE MP_Token SET isqq = 1 WHERE token = '" + token + "'";
            utils.ExecuteNonQuery(sql);
        }

        if (issina == "1")
        {
            sql = "UPDATE MP_Token SET issina = 1 WHERE token = '" + token + "'";
            utils.ExecuteNonQuery(sql);
        }

        sql = "SELECT * FROM MP_token WHERE token = '" + token + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            if (dt.Rows[0]["isqq"].ToString() == "1")
            {
                check1 = "checked";
            }
            if (dt.Rows[0]["issina"].ToString() == "1")
            {
                check2 = "checked";
            }
        }
    }
}