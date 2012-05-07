using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_detail_jsinfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string nick = GetRequest("nick");
        string itemid = GetRequest("itemid");


    }

    private string GetRequest(string str)
    {
        string val = Request.QueryString[str] == null ? "" : Request.QueryString[str].ToString();

        return val;
    }
}