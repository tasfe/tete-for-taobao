using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;

public partial class top_market_tuijianredirect : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string id = utils.NewRequest("id", utils.RequestType.QueryString);

        Cookie cookie = new Cookie();
        cookie.setCookie("tuijianid", id, 999999);

        Response.Redirect("http://seller.taobao.com/fuwu/service.htm?service_id=4545&from=haoyoutuijian");
    }
}