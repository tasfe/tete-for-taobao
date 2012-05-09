using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_detail_initbutton : System.Web.UI.Page
{
    public string cid = string.Empty;
    public string id = string.Empty;
    public string nick = string.Empty;
    public string tplid = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        id = Request.QueryString["itemid"] == null ? "0" : Request.QueryString["id"].ToString();
        nick = Request.QueryString["nick"] == null ? "0" : Request.QueryString["nick"].ToString();
        tplid = Request.QueryString["tplid"] == null ? "0" : Request.QueryString["tplid"].ToString();
    }
}