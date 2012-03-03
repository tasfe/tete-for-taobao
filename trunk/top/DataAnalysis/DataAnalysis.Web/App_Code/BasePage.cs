
/// <summary>
/// Summary description for BasePage
/// </summary>
using System.Web;
public class BasePage : System.Web.UI.Page
{
    protected override void InitializeCulture()
    {
        if (Request.Cookies["nick"] == null && Request.QueryString["istongji"] != "1")
        {
            string nick = Request.Cookies["nick"].Value;

            string msg = "尊敬的用户您好，非常抱歉的告诉您，您尚未订购该功能，如需继续使用请购买该服务:<br><br>9.9元/月  <a href=' http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-8:1;' target='_blank'>立即购买</a><br><br>27元/季  <a href=' http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-8:3;' target='_blank'>立即购买</a><br><br>54元/半年 <a href=' http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-8:6;' target='_blank'>立即购买</a><br><br>108元/年 <a href=' http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-8:12;' target='_blank'>立即购买</a><br>";
            Response.Redirect("buy.aspx?msg=" + msg);

        }
        else
        {
            Session["nick"] = Request.Cookies["nick"].Value;
        }
    }
}
