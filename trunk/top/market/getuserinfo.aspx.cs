using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Common;
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;

public partial class top_market_getuserinfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", "12132145", "1fdd2aadd5e2ac2909db2967cbb71e7f");
        string sql = "SELECT TOP 20 * FROM TopTaobaoShop WHERE enddate >= GETDATE() AND session IS NOT NULL ORDER BY enddate DESC";

        DataTable dt = utils.ExecuteDataTable(sql);
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            TradesBoughtGetRequest request = new TradesBoughtGetRequest();
            request.Fields = "receiver_mobile";

            PageList<Trade> trade = client.TradesBoughtGet(request, dt.Rows[i]["session"].ToString());

            Response.Write(dt.Rows[i]["session"].ToString() + "---");
            Response.Write(trade.Content[0].ReceiverMobile + "<br>");
        }
    }
}