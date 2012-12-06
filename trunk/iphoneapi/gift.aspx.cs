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
    protected void Page_Load(object sender, EventArgs e)
    {
        string id = utils.NewRequest("id", utils.RequestType.QueryString);
        string sql = "SELECT * FROM MP_Gift WHERE guid = '" + id + "'";

        DataTable dt = utils.ExecuteDataTable(sql);


    }
}