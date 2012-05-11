using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBHelp;

public partial class Web_detail_dialog : System.Web.UI.Page
{
    public string id = string.Empty;
    public string nick = string.Empty;
    public string nickid = string.Empty;
    public string tplid = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        id = Request.QueryString["id"] == null ? "0" : Request.QueryString["id"].ToString();
        nick = Request.QueryString["nick"] == null ? "0" : Request.QueryString["nick"].ToString();
        nickid = Request.QueryString["nickid"] == null ? "0" : Request.QueryString["nickid"].ToString();
        tplid = Request.QueryString["tplid"] == null ? "0" : Request.QueryString["tplid"].ToString();

        //判断该客户是否订购
        string sql = "SELECT * FROM Jia_Shop WHERE nick = '" + nick + "'";
        DataTable dt = DBHelper.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            if (dt.Rows[0]["isExpired"].ToString() == "1")
            {
                Response.Redirect("fuwuBuy.aspx?nick=" + HttpUtility.UrlEncode(nick) + "&nickid=" + nickid + "&id=" + id);
            }
        }
        else
        {
            Response.Redirect("fuwuBuy.aspx?nick=" + HttpUtility.UrlEncode(nick) + "&nickid=" + nickid + "&id=" + id); 
        }
    }
}