using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class GetNick : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (string.IsNullOrEmpty(Request.QueryString["nick"]))
                return;
            if (string.IsNullOrEmpty(Request.QueryString["session"]))
                return;
            if (string.IsNullOrEmpty(Request.QueryString["istongji"]))
                return;
           ;
            if (!string.IsNullOrEmpty(Request.QueryString["istongji"]))
            {
                string nick = HttpUtility.UrlDecode(Request.QueryString["nick"]);
                if (Request.QueryString["istongji"] == "1")
                {
                    string session = Request.QueryString["session"];
                    HttpCookie cookie = new HttpCookie("nick", HttpUtility.UrlEncode(nick));
                    HttpCookie cooksession = new HttpCookie("nicksession", session);
                    cookie.Expires = DateTime.Now.AddDays(1);
                    cooksession.Expires = DateTime.Now.AddDays(1);

                    Response.Cookies.Add(cookie);
                    Response.Cookies.Add(cooksession);

                    HttpCookie tongji = new HttpCookie("istongji", Request.QueryString["istongji"]);
                    tongji.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(tongji);
                    Response.Redirect("UpdateGoods.aspx");
                }
                else
                {
                    Response.Redirect("buy.aspx?nick=" + Request.QueryString["nick"]);
                }

            }
        }
    }
}
