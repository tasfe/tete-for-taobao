using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class top_crm_missionadd : System.Web.UI.Page
{
    public string now = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        now = DateTime.Now.AddHours(1).ToString();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {

    }
}