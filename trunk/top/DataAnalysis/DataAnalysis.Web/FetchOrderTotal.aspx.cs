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
                try
                {
                    DateTime now = DateTime.Now;
                    DataHelper.InsertGoodsOrder(DateTime.Parse(now.AddDays(-7).ToShortDateString()), now, session, nick);
                    //获取聊天记录
                    DateTime start = DataHelper.GetTalkContent(nick, session, now);

                    //添加统计数据
                    SiteTotalService taoDal = new SiteTotalService();
                    for (DateTime i = DateTime.Parse(now.AddDays(-7).ToShortDateString()); i <= now; i = i.AddDays(1))
                    {
                        DataHelper.UpdateSiteTotal(nick, session, i, taoDal);
                    }

                    //添加客服绩效统计
                    DataHelper.GetKfjxTotal(nick, start, now);
                }
                catch (Exception ex)
                {
                    LogInfo.WriteLog("出错了", ex.Message);
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
