using System;
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

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string SiteTotalDate = "20120401";
            DateTime start = DateTime.Parse(SiteTotalDate.Substring(0, 4) + "-" + SiteTotalDate.Substring(4, 2) + "-" + SiteTotalDate.Substring(6));
            string s = DateTime.Now.ToString("yyyyMMdd");
        }
    }
    protected void Btn_JoinNick_Click(object sender, EventArgs e)
    {
        HttpCookie cookie = new HttpCookie("nick", HttpUtility.UrlEncode(TB_Nick.Text));
        cookie.Expires = DateTime.Now.AddDays(1);
        Response.Cookies.Add(cookie);
    }
}
