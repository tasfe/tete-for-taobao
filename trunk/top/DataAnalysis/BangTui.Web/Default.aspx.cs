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
                CacheCollection.RemoveCacheByKey(CacheCollection.KEY_ALLBUYNINFO);
                string nick = Request.QueryString["nick"];
                string session = Request.QueryString["session"];
                if (Request.Cookies["nick"] == null)
                {
                    IList<TaoBaoGoodsClassInfo> classList = TopAPI.GetGoodsClassInfoList(nick, session);

                    if (classList != null)
                    {
                        TaoBaoGoodsClassService tbgcDal = new TaoBaoGoodsClassService();

                        foreach (TaoBaoGoodsClassInfo cinfo in classList)
                        {
                            tbgcDal.InsertGoodsClass(cinfo, nick);
                        }
                    }

                    GoodsService goodsDal = new GoodsService();
                    IList<TaoBaoGoodsInfo> list = TopAPI.GetGoodsInfoListByNick(nick, session);

                    foreach (TaoBaoGoodsInfo info in list)
                    {
                        goodsDal.InsertGoodsInfo(info, nick);
                    }
                }

                HttpCookie cookie = new HttpCookie("nick", HttpUtility.UrlEncode(nick));
                HttpCookie cooksession = new HttpCookie("nicksession", session);
                cookie.Expires = DateTime.Now.AddDays(1);
                cooksession.Expires = DateTime.Now.AddDays(1);

                Response.Cookies.Add(cookie);
                LogInfo.Add("添加了cookie", nick);

                Session["snick"] = nick;
                Session["ssession"] = session;

                Response.Cookies.Add(cooksession);

                HttpCookie tongji = new HttpCookie("istongji", Request.QueryString["istongji"]);
                tongji.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Add(tongji);

                Response.Redirect("http://www.7fshop.com/top/index.html");
            }
        }
    }
}
