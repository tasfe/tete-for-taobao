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

    protected void Page_Load(object sender, EventArgs e)
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        session = cookie.getCookie("top_session");
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Decrypt(taobaoNick);

        string sql = "SELECT * FROM TeteShopCategory WHERE nick = '" + nick + "' AND parentid=0";

        DataTable dt = utils.ExecuteDataTable(sql);
        rpt1.DataSource = dt;
        rpt1.DataBind();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {

    }
}