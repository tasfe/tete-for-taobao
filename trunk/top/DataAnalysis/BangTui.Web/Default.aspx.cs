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
using System.Collections.Generic;

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["istongji"]))
            {
                string nick = Request.QueryString["nick"];
                string session = Request.QueryString["session"];
                IList<TaoBaoGoodsInfo> list = TopAPI.GetGoodsInfoListByNick(nick, session);

                GoodsService goodsDal = new GoodsService();

                foreach (TaoBaoGoodsInfo info in list)
                {
                    goodsDal.InsertGoodsInfo(info, nick);
                }

                if (Request.Cookies["nick"] != null)
                {
                    HttpCookie cookie = new HttpCookie("nick", HttpUtility.UrlEncode(nick));
                    HttpCookie cooksession = new HttpCookie("nicksession", session);
                    cookie.Expires = DateTime.Now.AddDays(1);
                    cooksession.Expires = DateTime.Now.AddDays(1);

                    Response.Cookies.Add(cookie);
                    Response.Cookies.Add(cooksession);

                    HttpCookie tongji = new HttpCookie("istongji", Request.QueryString["istongji"]);
                    tongji.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(tongji);
                }
            }
        }
    }
}
