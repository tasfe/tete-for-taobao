using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_detail_create : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Write("<script>window.parent.document.getElementById('showArea').style.display = 'none';</script>");
        Response.End();
    }
}