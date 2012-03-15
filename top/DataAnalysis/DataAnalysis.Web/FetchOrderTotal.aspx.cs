using System;
using System.Linq;
using System.Web;

public partial class FetchOrderTotal : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            if (Request.Cookies["nick"] == null || string.IsNullOrEmpty(Request.Cookies["nick"].Value))
            {
                Response.Write("false");
                Response.End();
                return;
            }
            string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
            string session = Request.Cookies["nicksession"].Value;

            if (CacheCollection.GetNickSessionList().Where(o => o.Nick == nick).ToList().Count > 0)
            {

                DateTime now = DateTime.Now;
                DataHelper.InsertGoodsOrder(DateTime.Parse(now.AddDays(-7).ToShortDateString()), now, session, nick);
                //添加统计数据
                SiteTotalService taoDal = new SiteTotalService();
                for (DateTime i = DateTime.Parse(now.AddDays(-7).ToShortDateString()); i <= now; i = i.AddDays(1))
                {
                    DataHelper.UpdateSiteTotal(nick, i, taoDal);
                }

                Response.Write("true"); 
                Response.End();
            }
            else
            {
                Response.Write("false");
                Response.End();
            }
        }
    }
}
