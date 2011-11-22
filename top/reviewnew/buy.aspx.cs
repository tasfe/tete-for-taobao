using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;

public partial class top_groupbuy_buy : System.Web.UI.Page
{
    public string msg = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        msg = utils.NewRequest("msg", utils.RequestType.QueryString).Replace("''", "'");
    }
}