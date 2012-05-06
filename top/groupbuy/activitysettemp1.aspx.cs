﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;

public partial class top_groupbuy_activitysettemp1 : System.Web.UI.Page
{
    string taobaoNick = string.Empty;
    string sql = string.Empty;

    public string name = string.Empty;
    public string templetid = string.Empty;
    public string bt = string.Empty;
    public string mall = string.Empty;
    public string liang = string.Empty;
    public string baoy = string.Empty;
    public string strhtml = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {

        name = Request.Form["title"].ToString();
        templetid = Request.Form["templateID"].ToString();
        bt = Request.Form["button1"].ToString();
        mall = Request.Form["showmall"].ToString();
        liang = Request.Form["showliang"].ToString();
        if (Request.Form["flag"] != null && Request.Form["flag"] != "")
        {
            baoy = Request.Form["flag"].ToString();
        }
     

        Cookie cookie = new Cookie();
        string taobaoNick = cookie.getCookie("nick");
        if (!IsPostBack)
        {
            Rijndael_ encode = new Rijndael_("tetesoft");
            taobaoNick = encode.Decrypt(taobaoNick);

            sql = "SELECT * FROM tete_activity WHERE Status=1 and Nick = '" + taobaoNick + "'";

            DataTable dt = utils.ExecuteDataTable(sql);

            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strhtml += "<option value=\"" + dt.Rows[i]["id"].ToString() + "\">" + dt.Rows[i]["name"].ToString() + "</option>";
                }
            }
             
             
        }
    }
 
}