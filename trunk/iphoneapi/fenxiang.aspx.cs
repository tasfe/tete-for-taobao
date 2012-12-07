using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;

public partial class iphoneapi_fenxiang : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string token = "test";
        string sql = "UPDATE MP_Token SET score = score + 1 WHERE token = '" + token + "'";
        utils.ExecuteNonQuery(sql);
    }
}