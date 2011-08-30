using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;

public partial class top_market_redirect : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");

        //过期判断
        if (string.IsNullOrEmpty(taobaoNick))
        {
            Response.Write("window.location.href='http://container.open.taobao.com/container?appkey=12132145'");
            Response.End();
            return;
        }

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        string t = utils.NewRequest("t", utils.RequestType.QueryString);

        string sql = "INSERT INTO TopRedirect (nick, t) VALUES ('" + taobaoNick + "', '" + t + "')";

        utils.ExecuteNonQuery(sql);

        //判断该用户是否为收费用户，如果是的话则直接跳转到服务页面
        sql = "SELECT ISNULL(enddateblog,GETDATE()) FROM TopTaobaoShop WHERE nick = '" + taobaoNick + "'";

        string enddate = utils.ExecuteString(sql);

        if (DateTime.Now < DateTime.Parse(enddate))
        {
            //跳转到订购页面
            Response.Redirect("http://container.open.taobao.com/container?appkey=12159997");
        }
        else
        {
            Response.Redirect("http://seller.taobao.com/fuwu/service.htm?service_id=4545");
        }
    }
}