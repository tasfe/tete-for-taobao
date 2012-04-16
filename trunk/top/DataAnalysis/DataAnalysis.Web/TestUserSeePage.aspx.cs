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

public partial class TestUserSeePage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            IList<TaoBaoAPIHelper.SubUserInfo> list = TaoBaoAPIHelper.TaoBaoAPI.GetChildNick("冥狱冰炎", "6101501de17072e582aec5b3daaa520274fd02c9edebef716906993");

            TaoBaoAPIHelper.TaoBaoAPI.GetTalkObjList("冥狱冰炎:冥狱紫炎", "6101501de17072e582aec5b3daaa520274fd02c9edebef716906993", DateTime.Now.AddDays(-6), DateTime.Now);
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
