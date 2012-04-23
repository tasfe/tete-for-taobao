using System;
using System.Web;

public partial class TestUserSeePage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
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
