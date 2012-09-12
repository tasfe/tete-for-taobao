using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;

public partial class iphoneapi_api_cate : System.Web.UI.Page
{
    public string nick = string.Empty;
    public string session = string.Empty;
    public string st = string.Empty;

    public string logo1 = string.Empty;
    public string url1 = string.Empty;
    public string logo2 = string.Empty;
    public string url2 = string.Empty;
    public string logo3 = string.Empty;
    public string url3 = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        session = cookie.getCookie("top_session");
        st = cookie.getCookie("short");
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Decrypt(taobaoNick);

        string sql = "SELECT TOP 5 * FROM TeteShopCategory WHERE nick = '" + st + "' AND parentid=0 ORDER BY orderid";

        if (!IsPostBack)
        {
            DataTable dt = utils.ExecuteDataTable(sql);
            rpt1.DataSource = dt;
            rpt1.DataBind();
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string sql = string.Empty;
        string name = string.Empty;
        string orderid = string.Empty;

        string ids = utils.NewRequest("id", utils.RequestType.Form);
        string[] ary = ids.Split(',');
        for (int i = 0; i < ary.Length; i++)
        {
            name = utils.NewRequest("cate_" + ary[i], utils.RequestType.Form);
            orderid = utils.NewRequest("orderid_" + ary[i], utils.RequestType.Form);

            sql = "UPDATE TeteShopCategory SET catename = '" + name + "',orderid='" + orderid + "' WHERE nick = '" + st + "' AND cateid = '" + ary[i] + "'";
            utils.ExecuteNonQuery(sql);
        }

        Response.Redirect("cate.aspx");
    }

    protected void rpt1_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Label lb = (Label)e.Item.FindControl("lb1");
        string sql = "SELECT * FROM TeteShopAds WHERE typ = '" + lb.Text + "' ORDER BY orderid";
        Response.Write(sql);
        DataTable dt = utils.ExecuteDataTable(sql);

        Repeater rpt2 = (Repeater)e.Item.FindControl("rpt2");
        rpt2.DataSource = dt;
        rpt2.DataBind();
    }
}