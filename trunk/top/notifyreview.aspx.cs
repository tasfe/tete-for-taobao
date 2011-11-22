using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class top_notifyreview : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string content = File.ReadAllText("a.txt");
        File.WriteAllText("a.txt", content + "\r\n" + Request.Url.ToString());
    }
}