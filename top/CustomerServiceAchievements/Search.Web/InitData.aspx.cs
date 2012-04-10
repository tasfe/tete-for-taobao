using System;
using System.Linq;
using System.Web;
using Data.Cache;

public partial class InitData : System.Web.UI.Page
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
            //"610062491512599df1afc0664ee3a7041eb4f8d0b200134204200856";//测试

            if (CacheCollection.GetNickSessionList().Where(o => o.Nick == nick).ToList().Count > 0)
            {

                DataHelper.GetAllGoods(nick, session);

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