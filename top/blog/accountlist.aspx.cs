﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Common;


public partial class top_blog_accountlist : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        string act = utils.NewRequest("act", utils.RequestType.QueryString);
        string id = utils.NewRequest("id", utils.RequestType.QueryString);

        //过期判断
        if (string.IsNullOrEmpty(taobaoNick))
        {
            Response.Write("window.location.href='http://container.open.taobao.com/container?appkey=12159997'");
            Response.End();
            return;
        }

        if (id != "" && !utils.IsInt32(id))
        {
            Response.Write("非法参数1");
            Response.End();
            return;
        }

        if (act == "del")
        {
            string sqlnew = "DELETE FROM TopBlogAccountNew WHERE nick = '" + taobaoNick + "' AND id = " + id;
            utils.ExecuteNonQuery(sqlnew);

            Response.Redirect("accountlist.aspx");
            return;
        }

        string sqlNew = "SELECT * FROM TopBlogAccountNew WHERE nick = '" + taobaoNick + "' ORDER BY id DESC";
        DataTable dtNew = utils.ExecuteDataTable(sqlNew);

        rptArticle.DataSource = dtNew;
        rptArticle.DataBind();
    }
}