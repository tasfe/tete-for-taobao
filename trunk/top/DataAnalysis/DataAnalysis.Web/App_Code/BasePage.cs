
/// <summary>
/// Summary description for BasePage
/// </summary>
using System.Web;
public class BasePage : System.Web.UI.Page
{
    protected override void InitializeCulture()
    {
        if (Request.QueryString["istongji"] != "1")
        {
            string nick = Request.Cookies["nick"].Value;

            string msg = "尊敬的" + nick + "，非常抱歉的告诉您，您尚未订购该功能，如需继续使用请购买该服务:<br><br>19元/月 【赠送短信50条】 <a href=' http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-4:1;' target='_blank'>立即购买</a><br><br>54元/季 【赠送短信150条】 <a href=' http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-4:3;' target='_blank'>立即购买</a><br><br>99元/半年 【赠送短信300条】<a href=' http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-4:6;' target='_blank'>立即购买</a><br><br>188元/年 【赠送短信600条】<a href=' http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-4:12;' target='_blank'>立即购买</a><br>";
            Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));

        }
        else
        {
            Session["nick"] = Request.Cookies["nick"].Value;
        }
    }
}
