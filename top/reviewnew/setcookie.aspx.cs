﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;

public partial class top_reviewnew_setcookie : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string nick = utils.NewRequest("nick", utils.RequestType.QueryString);

        string sql = "SELECT * FROM TCS_ShopSession WHERE nick = '" + nick + "'";

        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            Rijndael_ encode = new Rijndael_("tetesoft");
            nick = encode.Encrypt(nick);

            Common.Cookie cookie = new Common.Cookie();
            cookie.setCookie("top_sessiongroupbuy", dt.Rows[0]["session"].ToString(), 999999);
            cookie.setCookie("nick", nick, 999999);
        }
    }
}