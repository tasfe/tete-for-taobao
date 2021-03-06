﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Common;

public partial class top_market_tuijianlist : System.Web.UI.Page
{
    public string tuijian = string.Empty;
    public string need = string.Empty;
    public string id = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Cookie cookie = new Cookie();
        string taobaoNick = cookie.getCookie("nick");
        string session = cookie.getCookie("top_session");

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        string sql = "SELECT * FROM TopTuijian WHERE nickfrom = '" + taobaoNick + "' ORDER BY id ASC";
        DataTable dt = utils.ExecuteDataTable(sql);

        rptIdeaList.DataSource = dt;
        rptIdeaList.DataBind();

        sql = "SELECT COUNT(*) FROM TopTuijian WHERE nickfrom = '" + taobaoNick + "' AND isok = 1";
        tuijian = utils.ExecuteString(sql);

        if (int.Parse(tuijian) % 10 != 0)
        {
            need = (10 - int.Parse(tuijian) % 10).ToString();
        }
        else
        {
            need = "10";
        }

        sql = "SELECT sid FROM TopTaobaoShop WHERE nick = '" + taobaoNick + "'";
        id = utils.ExecuteString(sql);
    }
}