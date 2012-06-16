using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class taobaotetesoft_caiji_initads : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string str = "document.write('<img src=http://gg.7fshop.com/show.aspx?id='+alimama_pid+' height='+alimama_height+' width=\"+alimama_width+\">');";

        Response.Write(str);
    }
}