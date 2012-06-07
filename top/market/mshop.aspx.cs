using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;

public partial class top_market_mshop : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.Cookies["nick"] == null)
            {
                Response.Write("请登录");
                Response.End();
                return;
            }
            //解密NICK
            Rijndael_ encode = new Rijndael_("tetesoft");
            string nick = encode.Decrypt(Request.Cookies["nick"].Value);

            if (Request.Cookies["mobile"] != null && Request.Cookies["mobile"].Value == "1")
            {
                Response.Redirect("http://iphone.7fshop.com/CreateAPK.aspx?nick=" + HttpUtility.UrlEncode(nick) + "&nicksession=" + Request.Cookies["top_session"].Value + "&mobile=1");
            }
        }
    }

    protected void CheckNull(object sender, ImageClickEventArgs e)
    {
        Page.RegisterStartupScript("ss", "<script>open('http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-6:1;','_blank');</script>");

        //Response.Redirect("http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-6:1;", false);
    }
}