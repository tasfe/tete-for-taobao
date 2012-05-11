using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBHelp;

public partial class detail_changeitemid : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string itemid = Request.QueryString["itemid"] == null ? "0" : Request.QueryString["itemid"].ToString();
        string tempid = Request.QueryString["tempid"] == null ? "0" : Request.QueryString["tempid"].ToString();

        //更新临时ID
        string sql = "UPDATE Jia_ImgCustomer SET itemid = '" + itemid + "' WHERE itemid = '" + tempid + "'";
        DBHelper.ExecuteNonQuery(sql);

        sql = "UPDATE Jia_Item SET itemid = '" + itemid + "' WHERE itemid = '" + tempid + "'";
        DBHelper.ExecuteNonQuery(sql);

        Response.Write("ok");
        Response.End();
    }
}