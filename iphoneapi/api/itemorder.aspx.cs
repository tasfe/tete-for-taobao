﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;

public partial class iphoneapi_api_cate : System.Web.UI.Page
{
    public string nick = string.Empty;
    public string session = string.Empty;
    public string st = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        session = cookie.getCookie("top_session");
        st = cookie.getCookie("short");
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Decrypt(taobaoNick);

        string sql = "SELECT * FROM TeteShopCategory WHERE nick = '" + st + "' AND parentid=0 ORDER BY orderid";

        if (!IsPostBack)
        {
            DataTable dt = utils.ExecuteDataTable(sql);
            ddl1.DataSource = dt;
            ddl1.DataTextField = "catename";
            ddl1.DataValueField = "cateid";
            ddl1.DataBind();


            sql = "SELECT * FROM TeteShopItem WHERE nick = '" + st + "' AND isnew = 1 ORDER BY orderid";

            dt = utils.ExecuteDataTable(sql);
            rpt1.DataSource = dt;
            rpt1.DataBind();
        }
    }



    public static string check(string str)
    {
        if (str == "1")
        {
            return "checked";
        }
        else
        {
            return "";
        }
    }



    protected void Button1_Click(object sender, EventArgs e)
    {
        string sql = string.Empty;
        string name = string.Empty;
        string orderid = string.Empty;

        string ids = utils.NewRequest("ids", utils.RequestType.Form);
        string[] ary = ids.Split(',');
        for (int i = 0; i < ary.Length; i++)
        {
            orderid = utils.NewRequest("orderid_" + ary[i], utils.RequestType.Form);

            sql = "UPDATE TeteShopItem SET orderid = '" + orderid + "' WHERE nick = '" + st + "' AND itemid = '" + ary[i] + "'";
            utils.ExecuteNonQuery(sql);
        }

        Response.Redirect("itemorder.aspx");
    }

    protected void ddl1_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = string.Empty;
        string sql = "SELECT * FROM TeteShopCategory WHERE parentid = '" + this.ddl1.SelectedValue + "' ORDER BY orderid";
        DataTable dt = utils.ExecuteDataTable(sql);
        str = "(1 = 2";
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str += " OR CHARINDEX('" + dt.Rows[i]["cateid"].ToString() + "', cateid) > 0";
        }
        str += " OR CHARINDEX('" + this.ddl1.SelectedValue + "', cateid) > 0)";

        sql = "SELECT * FROM TeteShopItem WHERE nick = '" + st + "' AND " + str + " AND isnew = 1 ORDER BY orderid";
        //Response.Write(sql);
        dt = utils.ExecuteDataTable(sql);
        rpt1.DataSource = dt;
        rpt1.DataBind();
    }
}