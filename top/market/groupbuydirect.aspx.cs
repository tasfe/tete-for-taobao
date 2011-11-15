using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;

public partial class top_market_groupbuydirect : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Cookie cookie = new Cookie();
        string taobaoNick = cookie.getCookie("nick");
        string session = cookie.getCookie("top_session");

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        if (session != "" && taobaoNick != "")
        {
            Response.Redirect("http://groupbuy.7fshop.com/top/groupbuy/deletegroupbuy.aspx?session=" + session + "&nick=" + HttpUtility.UrlEncode(taobaoNick));
        }
    }
}