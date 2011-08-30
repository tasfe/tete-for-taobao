using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Common;

public partial class top_total : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string sqlNew = "SELECT COUNT(*) AS num FROM TopTaobaoShop";
        DataTable dtNew = utils.ExecuteDataTable(sqlNew);
        if (dtNew.Rows.Count != 0)
        {
            Response.Write("当前使用人数-" + dtNew.Rows[0]["num"].ToString());
        }
    }
}
