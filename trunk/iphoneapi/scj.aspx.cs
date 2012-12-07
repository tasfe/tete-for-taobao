using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Common;

public partial class iphoneapi_scj : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string token = "test";
        string sql = "SELECT * FROM MP_CityShopCoupon WHERE guid IN (SELECT DISTINCT couponid FROM MP_TokenCollect WHERE token = '" + token + "')";

        DataTable dt = utils.ExecuteDataTable(sql);

        rptCoupon.DataSource = dt;
        rptCoupon.DataBind();
    }
}