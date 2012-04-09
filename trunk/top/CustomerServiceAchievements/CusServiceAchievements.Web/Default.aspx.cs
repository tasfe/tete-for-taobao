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
using Data.Cache;
using CusServiceAchievements.DAL;

public partial class _Default : System.Web.UI.Page 
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
                //获取聊天记录
                DateTime start = DataHelper.GetTalkrContent(nick, session, now);

                //添加统计数据
                SiteTotalService taoDal = new SiteTotalService();
                for (DateTime i = DateTime.Parse(now.AddDays(-7).ToShortDateString()); i <= now; i = i.AddDays(1))
                {
                    DataHelper.UpdateSiteTotal(nick, session, i, taoDal);
                }

                //添加客服绩效统计
                DataHelper.GetKfjxTotal(nick, start, now);

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

    protected void Btn_JoinNick_Click(object sender, EventArgs e)
    {
        HttpCookie cookie = new HttpCookie("nick", HttpUtility.UrlEncode(TB_Nick.Text));
        cookie.Expires = DateTime.Now.AddDays(1);
        Response.Cookies.Add(cookie);
    }
}
