using System;
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
    public string nickcode = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Cookie cookie = new Cookie();
        string taobaoNick = cookie.getCookie("nick");
        nickcode = cookie.getCookie("nick");
        string session = cookie.getCookie("top_session");

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        string sql = "SELECT * FROM TCS_Tuijian WHERE nickfrom = '" + taobaoNick + "' ORDER BY adddate DESC";
        DataTable dt = utils.ExecuteDataTable(sql);

        rptIdeaList.DataSource = dt;
        rptIdeaList.DataBind();

        sql = "SELECT COUNT(*) FROM TCS_Tuijian WHERE nickfrom = '" + taobaoNick + "'";
        tuijian = utils.ExecuteString(sql);
    }
}