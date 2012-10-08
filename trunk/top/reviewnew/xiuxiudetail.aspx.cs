using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;
using System.Net;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Data;

public partial class top_reviewnew_xiuxiudetail : System.Web.UI.Page
{
    public string session = string.Empty;
    public string nick = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        session = cookie.getCookie("top_sessiongroupbuy");
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Decrypt(taobaoNick);

        //过期判断
        if (string.IsNullOrEmpty(taobaoNick))
        {
            string msg = "尊敬的淘宝卖家，非常抱歉的告诉您，您还没有购买此服务或者登录信息已失效，如需继续使用请<a href='http://fuwu.taobao.com/serv/detail.htm?service_id=4545' target='_blank'>进入该服务</a>，谢谢！";
            Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
            Response.End();
            return;
        }

        if (!IsPostBack)
        {
            BindData();
        }
    }

    /// <summary>
    /// 绑定数据
    /// </summary>
    private void BindData()
    {
        string sql = "SELECT * FROM TCS_ShopConfig WHERE nick = '" + nick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            tbTime.Text = dt.Rows[0]["xiuxiutime"].ToString();
            tbTitle.Text = dt.Rows[0]["xiuxiuname"].ToString();
            ddlShow.SelectedValue = dt.Rows[0]["xiuxiuisshow"].ToString();
            this.TextBox1.Text = dt.Rows[0]["xiuxiuads"].ToString();
        }

        if (tbTitle.Text.Length == 0)
        {
            tbTitle.Text = "好评有礼";
        }
    }


    protected void Button1_Click(object sender, EventArgs e)
    {
        string sql = "UPDATE TCS_ShopConfig SET xiuxiutime = '" + tbTime.Text + "',xiuxiuname = '" + tbTitle.Text + "',xiuxiuisshow = '" + ddlShow.SelectedValue + "',xiuxiuads = '" + this.TextBox1.Text + "' WHERE nick = '" + nick + "'";
        utils.ExecuteNonQuery(sql);

        Response.Redirect("xiuxiudetail.aspx");
    }
}