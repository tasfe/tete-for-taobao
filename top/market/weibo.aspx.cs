using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;

public partial class top_market_weibo : System.Web.UI.Page
{
    public string session = string.Empty;
    public string nick = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Common.Cookie cookie1 = new Common.Cookie();
        string taobaoNick = cookie1.getCookie("nick");
        string isMicroBlog = cookie1.getCookie("fensi");
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Decrypt(taobaoNick);

        //过期判断
        if (isMicroBlog != "1")
        {
            string msg = "尊敬的" + nick + "，非常抱歉的告诉您，您尚未订购该功能，如需继续使用请<a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-10:1;' target='_blank'>购买该服务</a>，谢谢！";
            Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
            Response.End();
            return;
        }
    }
}