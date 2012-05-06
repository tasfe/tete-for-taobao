using System;
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
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Rijndael_ encode = new Rijndael_("tetesoft");
            taobaoNick = encode.Decrypt(taobaoNick);

            sql = "SELECT * FROM tete_activity WHERE Nick = '" + taobaoNick + "'";

            DataTable dt = utils.ExecuteDataTable(sql);

            Repeater1.DataSource = dt;
            Repeater1.DataBind();
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string select = select1.Value;
        Response.Redirect("activitysettemp2.aspx?item=" + select);
    }
}