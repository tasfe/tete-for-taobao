﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;
using System.Text;

public partial class top_reviewnew_freesearch : System.Web.UI.Page
{
    public string buynick = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string nick = utils.NewRequest("nick", utils.RequestType.QueryString);
        buynick = utils.NewRequest("buynick", utils.RequestType.QueryString);
        //StringBuilder builder = new StringBuilder();

        string sql = "SELECT a.name,a.areaisfree,a.arealist,f.startdate,f.carddate,f.usecount,f.usecountlimit,f.price FROM TCS_FreeCard f INNER JOIN TCS_FreeCardAction a ON a.guid = f.cardid WHERE f.nick = '" + nick + "' AND f.buynick = '" + buynick + "'";

        DataTable dt = utils.ExecuteDataTable(sql);

        rptArticle.DataSource = dt;
        rptArticle.DataBind();

        if (dt.Rows.Count == 0)
        {
            Panel1.Visible = false;
        }

        //builder.Append(buynick + "，您目前拥有店铺【" + nick + "】的包邮卡" + dt.Rows.Count + "张<br>");
        //for (int i = 0; i < dt.Rows.Count; i++)
        //{
        //    builder.Append("<br>");
        //}
    }


    public static string show(string isfree, string arealist)
    {
        if (arealist.Length == 0)
        {
            return "【国内全部包邮】";
        }

        if (isfree == "1")
        {
            return "【只有以下地区包邮】<br>（" + arealist + "）";
        }
        else
        {
            return "【以下地区不包邮】<br>（" + arealist + "）";
        }
    }
}