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
    public string nickid = string.Empty;
    public string tplid = string.Empty;
    public string html = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        id = Request.QueryString["itemid"] == null ? Guid.NewGuid().ToString() : Request.QueryString["itemid"].ToString();
        nick = Request.QueryString["nick"] == null ? "0" : Request.QueryString["nick"].ToString();
        nickid = Request.QueryString["nickid"] == null ? "0" : Request.QueryString["nickid"].ToString();
        tplid = Request.QueryString["tplid"] == null ? "0" : Request.QueryString["tplid"].ToString();
        cid = Request.QueryString["cid"] == null ? "0" : Request.QueryString["cid"].ToString();

        //if (cid.IndexOf("10020701") != -1 || Request.UserHostAddress.ToString() == "112.65.166.194" || Request.UserHostAddress.ToString() == "117.81.173.220")
        ////if (cid == "1002200101")
        //{
            html = "document.write(str);";
        //}

            nick = HttpUtility.UrlEncode(nick);
    }
}