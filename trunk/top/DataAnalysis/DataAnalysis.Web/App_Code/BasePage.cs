
using System.Web;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 控制合法查看
/// </summary>
public class BasePage : System.Web.UI.Page
{
    protected override void InitializeCulture()
    {
        if (Request.Cookies["istongji"] == null || Request.Cookies["istongji"].Value != "1")
        {
            string msg = "尊敬的用户您好，非常抱歉的告诉您，您尚未订购该功能，如需继续使用请购买该服务:<br><br>58元/月  <a href=' http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-8:1;' target='_blank'>立即购买</a><br><br>158元/季  <a href=' http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-8:3;' target='_blank'>立即购买</a><br><br>298元/半年 <a href=' http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-8:6;' target='_blank'>立即购买</a><br><br>568元/年 <a href=' http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-8:12;' target='_blank'>立即购买</a><br>";
            Response.Redirect("buy.aspx?msg=" + msg);

        }
        else
        {
            if (Request.Cookies["nick"] == null || string.IsNullOrEmpty(Request.Cookies["nick"].Value))
                return;
            else
            {
                if (!CheckNick(HttpUtility.UrlDecode(Request.Cookies["nick"].Value)))
                    return;
            }
        }
        //else
        //{
        //    Session["nick"] = Request.Cookies["nick"].Value;
        //    Session["session"] = Request.Cookies["session"].Value;
        //}
    }

    public static bool CheckNick(string nick)
    {
        IList<TopNickSessionInfo> list = CacheCollection.GetNickSessionList().Where(o => o.Nick == nick).ToList();
        if (list.Count == 0)
            return false;
        if (!list[0].NickState)
            return false;
        return true;
    }
}
