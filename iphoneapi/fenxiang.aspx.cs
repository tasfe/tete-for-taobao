﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;

public partial class iphoneapi_fenxiang : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string token = "test";
        string sql = "INSERT INTO MP_TokenScoreLog (token, score, remark) VALUES ('" + token + "', '10', '分享拿积分10分')";
        utils.ExecuteNonQuery(sql);

        sql = "UPDATE MP_Token SET score = score + 10 WHERE token = '" + token + "'";
        utils.ExecuteNonQuery(sql);
    }
}