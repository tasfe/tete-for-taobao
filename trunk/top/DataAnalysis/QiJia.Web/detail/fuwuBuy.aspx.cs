using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBHelp;

public partial class detail_fuwuBuy : System.Web.UI.Page
{
    public string id = string.Empty;
    public string nick = string.Empty;
    public string nickid = string.Empty;
    public string day = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        id = Request.QueryString["id"] == null ? "0" : Request.QueryString["id"].ToString();
        nick = Request.QueryString["nick"] == null ? "" : Request.QueryString["nick"].ToString();
        nickid = Request.QueryString["nickid"] == null ? "" : Request.QueryString["nickid"].ToString();
        //默认订购天数
        day = "15";

        //如果已经订购而且正常使用则直接跳转到日志页面
        string sql = "SELECT COUNT(*) FROM Jia_Shop WHERE nick = '" + nick + "' AND isexpired = 0";
        string count = DBHelper.ExecuteDataTable(sql).Rows[0][0].ToString();
        if (count != "0")
        {
            Response.Redirect("fuwuLog.aspx?nick=" + HttpUtility.UrlEncode(nick));
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        //如果没有该客户记录则增加记录
        string sql = "SELECT COUNT(*) FROM Jia_Shop WHERE nick = '" + nick + "'";
        Response.Write(sql);
        string count = DBHelper.ExecuteDataTable(sql).Rows[0][0].ToString();
        if (count == "0")
        {
            sql = "INSERT INTO Jia_Shop (nick, shopid, isexpired, adddate, expiredate) VALUES ('" + nick + "','" + nickid + "','0','" + DateTime.Now.ToString() + "','" + DateTime.Now.AddDays(int.Parse(day)) + "')";
            DBHelper.ExecuteNonQuery(sql);

            //写入订购日志
            sql = "INSERT INTO Jia_BuyLog (nick, type, price, buydate, isold, adddate) VALUES ('" + nick + "','detail','0','" + day + "',0,GETDATE())";
            DBHelper.ExecuteNonQuery(sql);
        }
        else
        {
            sql = "UPDATE Jia_Shop SET isexpired = 0, expiredate = '" + DateTime.Now.AddDays(int.Parse(day)) + "' WHERE nick = '" + nick + "'";
            DBHelper.ExecuteNonQuery(sql);

            //写入订购日志
            sql = "INSERT INTO Jia_BuyLog (nick, type, price, buydate, isold, adddate) VALUES ('" + nick + "','detail','0','" + day + "',1,GETDATE())";
            DBHelper.ExecuteNonQuery(sql);
        }

        Response.Redirect("fuwuLog.aspx?nick=" + HttpUtility.UrlEncode(nick) + "&nickid=" + nickid + "&id=" + id);
    }
}