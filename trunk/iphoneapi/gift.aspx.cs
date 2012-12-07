using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;

public partial class iphoneapi_gift : System.Web.UI.Page
{
    public string id = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        id = utils.NewRequest("id", utils.RequestType.QueryString);
        string act = utils.NewRequest("act", utils.RequestType.QueryString);
        string token = "test";

        if (act == "buy")
        {
            string sql1 = "INSERT INTO MP_TokenOrder (token, orderid, price) VALUES ('" + token + "', '2012121288888888', '88888')";
            utils.ExecuteNonQuery(sql1);
        }

        string sql = "SELECT * FROM MP_Gift WHERE guid = '" + id + "'";

        DataTable dt = utils.ExecuteDataTable(sql);

        rptDetail.DataSource = dt;
        rptDetail.DataBind();
    }
}