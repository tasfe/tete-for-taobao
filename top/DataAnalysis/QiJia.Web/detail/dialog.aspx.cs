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
    public string cid = string.Empty;
    public string nick = string.Empty;
    public string nickid = string.Empty;
    public string tplid = string.Empty;
    public string html = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        id = Request.QueryString["id"] == null ? "0" : Request.QueryString["id"].ToString();
        cid = Request.QueryString["cid"] == null ? "0" : Request.QueryString["cid"].ToString();
        nick = Request.QueryString["nick"] == null ? "0" : Request.QueryString["nick"].ToString();
        nickid = Request.QueryString["nickid"] == null ? "0" : Request.QueryString["nickid"].ToString();
        tplid = Request.QueryString["tplid"] == null ? "0" : Request.QueryString["tplid"].ToString();

        if (cid.IndexOf("10020701") != -1)
        {
            html = @"<div style='float:left; width:300px;'><img src=""template1.gif"" /><br /><input type=""button"" style=""background:url(btn_01.jpg) no-repeat; width:116px; height:29px; border:0;margin-left:40px; margin-top:10px; cursor:pointer"" id=""a3b6283d-ac07-4bc4-9d68-c64ab09a4903"" onclick=""selectTemplate(this,'dialog1.aspx')""></div>";
        }

        //if (Request.UserHostAddress.ToString() == "112.65.166.194" || Request.UserHostAddress.ToString() == "117.81.173.220")
        //{
            html += @"<div style='float:left; width:300px;'><img src=""template2.gif"" /><br /><input type=""button"" style=""background:url(btn_01.jpg) no-repeat; width:116px; height:29px; border:0;margin-left:40px; margin-top:10px; cursor:pointer"" id=""a81862b7-2185-4090-a406-a5757fa0ae21"" onclick=""selectTemplate(this,'dialog2.aspx')""></div>";
        //}

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

        nick = HttpUtility.UrlEncode(nick);
    }
}