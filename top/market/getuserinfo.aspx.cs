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
        string sql = "SELECT TOP 50 * FROM TopTaobaoShop WHERE enddate >= GETDATE() AND session IS NOT NULL ORDER BY enddate DESC";

        DataTable dt = utils.ExecuteDataTable(sql);
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            try
            {
                UserGetRequest request1 = new UserGetRequest();
                request1.Fields = "seller_credit";
                User user = client.UserGet(request1, dt.Rows[i]["session"].ToString());

                TradesBoughtGetRequest request = new TradesBoughtGetRequest();
                request.Fields = "receiver_mobile";

                PageList<Trade> trade = client.TradesBoughtGet(request, dt.Rows[i]["session"].ToString());

                if (trade.Content.Count != 0)
                {
                    if (trade.Content[0].ReceiverMobile.Length != 0)
                    {
                        Response.Write(dt.Rows[i]["nick"].ToString() + "---");
                        Response.Write(user.SellerCredit.Level.ToString() + "---");
                        Response.Write(trade.Content[0].ReceiverMobile + "<br>");
                    }
                }
            }
            catch { }
        }
    }
}