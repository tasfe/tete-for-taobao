using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;

public partial class top_review_itemsend : System.Web.UI.Page
{
    public string session = string.Empty;
    public string nick = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string id = utils.NewRequest("id", utils.RequestType.QueryString);
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        session = cookie.getCookie("top_sessiongroupbuy");
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Decrypt(taobaoNick);

        //判断VIP版本，只有VIP才能使用此功能
        string sql = "SELECT * FROM TopTaobaoShop WHERE nick = '" + nick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            string flag = dt.Rows[0]["versionNoBlog"].ToString();
            if (flag == "0")
            {
                Response.Redirect("xufei.aspx");
                Response.End();
                return;
            }
        }

        BindData();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (search.Text.Trim() == "")
        {
            Response.Redirect("itemsend.aspx");
            return;
        }

        string sqlNew = "SELECT * FROM TopItemSend WHERE nick = '" + nick + "' AND sendto = '" + search.Text.Trim().Replace("'", "''") + "'";
        DataTable dt = utils.ExecuteDataTable(sqlNew);

        rptArticle.DataSource = dt;
        rptArticle.DataBind();

        lbPage.Text = "";
    }

    private void BindData()
    {
        string sql = "SELECT * FROM TopItemSend WHERE nick = '" + nick + "' ORDER BY id DESC";
        DataTable dt = utils.ExecuteDataTable(sql);

        rptArticle.DataSource = dt;
        rptArticle.DataBind();
    }
}