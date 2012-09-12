using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class detail_test2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        Response.Write(Request.Files.Count.ToString());
        Request.Files[0].SaveAs(Server.MapPath("testestes.gif"));
    }
}