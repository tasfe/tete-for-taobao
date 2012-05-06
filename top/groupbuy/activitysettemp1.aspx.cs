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
        Cookie cookie = new Cookie();
        string taobaoNick = cookie.getCookie("nick");
        if (!IsPostBack)
        {
            Rijndael_ encode = new Rijndael_("tetesoft");
            taobaoNick = encode.Decrypt(taobaoNick);

            sql = "SELECT * FROM tete_activity WHERE Status=1 and Nick = '" + taobaoNick + "'";

            DataTable dt = utils.ExecuteDataTable(sql);
 

            DropDownList1.DataSource = dt;
            DropDownList1.DataTextField = "name";
            DropDownList1.DataValueField = "ID";
            DropDownList1.DataBind();

            DropDownList1.Items.Insert(0, new ListItem("请选择", "0"));
            DropDownList1.Items.Add(new ListItem("手动选择", ""));
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string select = DropDownList1.SelectedValue;
        Response.Redirect("activitysettemp2.aspx?item=" + select);
    }
}