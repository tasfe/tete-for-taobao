using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class top_groupbuy_activitysettemp1 : System.Web.UI.Page
{
    public string html = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        html = "测试";
        Response.Write(Request.Form["selstr"].ToString());
        Response.Write(Request.Form["templetid"].ToString()); 
    }
}