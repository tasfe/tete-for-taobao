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

            sql = "SELECT * FROM TeteShopAds WHERE typ = 'index' AND nick = '" + st + "' ORDER BY orderid";
            dt = utils.ExecuteDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                {
                    logo1 = dt.Rows[i]["logo"].ToString();
                    url1 = dt.Rows[i]["url"].ToString();
                }

                if (i == 1)
                {
                    logo2 = dt.Rows[i]["logo"].ToString();
                    url2 = dt.Rows[i]["url"].ToString();
                }

                if (i == 2)
                {
                    logo3 = dt.Rows[i]["logo"].ToString();
                    url3 = dt.Rows[i]["url"].ToString();
                }
            }
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
            for (int j = 1; j < 4; j++)
            {
                name = utils.NewRequest("pic_" + ary[i] + "_" + j, utils.RequestType.Form);
                orderid = utils.NewRequest("url_" + ary[i] + "_" + j, utils.RequestType.Form);

                sql = "UPDATE TeteShopAds SET logo = '" + name + "',url='" + orderid + "' WHERE nick = '" + st + "' AND typ = '" + ary[i] + "' AND orderid = '" + j + "'";
                utils.ExecuteNonQuery(sql);
            }
        }

        Response.Redirect("ads.aspx");
    }

    protected void rpt1_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Label lb = (Label)e.Item.FindControl("lb1");
        string sql = "SELECT * FROM TeteShopAds WHERE typ = '" + lb.Text + "' AND nick = '"+st+"' ORDER BY orderid";
        DataTable dt = utils.ExecuteDataTable(sql);

        Repeater rpt2 = (Repeater)e.Item.FindControl("rpt2");
        rpt2.DataSource = dt;
        rpt2.DataBind();
    }
}