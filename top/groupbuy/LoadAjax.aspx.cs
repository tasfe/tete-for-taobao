using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class top_groupbuy_LoadAjax : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int d = new Random().Next(500);
        File.Delete("E://" + d.ToString() + ".txt");
        File.Create("E://"+d.ToString()+".txt");
        Response.Write(d.ToString());
        Response.End();
    }
}