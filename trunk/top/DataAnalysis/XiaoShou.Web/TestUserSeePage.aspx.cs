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
using System.Collections.Generic;
using Model;

public partial class TestUserSeePage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //IList<OrderExpressInfo> list = TaoBaoAPI.GetOrderLogisticompanies("luckyfish8800", "6101312587cbdace711a1e26d5877064329a4dd05d9c96326907498",DateTime.Now.AddDays(-7),DateTime.Now.AddDays(-1));

         
        }
    }
    protected void Btn_AddCookie_Click(object sender, EventArgs e)
    {
        string nick = HttpUtility.UrlEncode(Tb_UserNick.Text);
        string session = Tb_UserSession.Text;
        HttpCookie cookie = new HttpCookie("nick", nick);
        HttpCookie cooksession = new HttpCookie("nicksession", session);
        cookie.Expires = DateTime.Now.AddDays(1);
        cooksession.Expires = DateTime.Now.AddDays(1);


        HttpCookie istongji = new HttpCookie("istongji", "1");
        istongji.Expires = DateTime.Now.AddDays(1);
        Response.Cookies.Add(istongji);

        Response.Cookies.Add(cookie);
        Response.Cookies.Add(cooksession);
        Response.Redirect("indextongji.html");
    }
}
