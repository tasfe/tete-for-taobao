using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Common;

public partial class top_market_result : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BindData();
    }

    private void BindData()
    {
        Cookie cookie = new Cookie();
        string taobaoNick = cookie.getCookie("nick");
        string session = cookie.getCookie("top_session");

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        string sql = "SELECT id,name,viewcount,hitcount FROM TopIdea WHERE nick = '" + taobaoNick + "' ORDER BY id DESC";
        DataTable dt = utils.ExecuteDataTable(sql);

        rptResult.DataSource = dt;
        rptResult.DataBind();
    }
}
