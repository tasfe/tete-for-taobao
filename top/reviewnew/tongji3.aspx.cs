﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;

public partial class top_review_tongji : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string url = Request.UrlReferrer == null ? "" : Request.UrlReferrer.ToString();
        string ip = Request.UserHostAddress;

        string sql = "SELECT COUNT(*) FROM TopTongji WHERE DATEDIFF(d, date, GETDATE()) = 0 AND ip = '" + ip + "' AND sellerid = '7888'";
        string count = utils.ExecuteString(sql);
        if (count == "0")
        {
            sql = "INSERT INTO TopTongji (ip, url, sellerid) VALUES ('" + ip + "','" + url + "','7888')";
        }
        else
        {
            sql = "UPDATE TopTongji SET count = count + 1 WHERE DATEDIFF(d, date, GETDATE()) = 0 AND ip = '" + ip + "' AND sellerid = '7888'";
        }
        utils.ExecuteNonQuery(sql);
        Response.Redirect("http://www.kkye.com/helper/helper_get_ads.php?adv_id=10004");
    }
}