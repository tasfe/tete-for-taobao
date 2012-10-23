using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class top_reviewnew_getreturn : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        File.WriteAllText(Server.MapPath("111url.txt"), Request.Url.ToString());
    }
}