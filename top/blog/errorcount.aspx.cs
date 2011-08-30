using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Common;

public partial class top_blog_errorcount : System.Web.UI.Page
{
    public string proxy = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string path = Server.MapPath("/auto/ipnow.txt");
        if (File.Exists(path))
        {
            proxy = File.ReadAllText(path);
        }

        utils.ExecuteNonQuery("UPDATE TopProxyList SET errcount = errcount + 1 WHERE proxy = '" + proxy + "'");
    }
}