using System;
using System.Collections;
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
using Common;

public partial class container : System.Web.UI.Page
{
    public string top_session = string.Empty;
    public string nick = string.Empty;
    public string top_appkey = string.Empty;
    public string app_secret = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        //签名验证
        top_appkey = "12133190";
        app_secret = "3442d45c5dcc79decbe8ff4db855a800";

        string top_parameters = utils.NewRequest("top_parameters", utils.RequestType.QueryString).Replace(" ", "+");
        top_session = utils.NewRequest("top_session", utils.RequestType.QueryString).Replace(" ", "+");

        string top_sign = utils.NewRequest("top_sign", utils.RequestType.QueryString).Replace(" ", "+"); //字符串中的+在获取后会被替换成空格，要再替换回来

        string sign = utils.NewRequest("sign", utils.RequestType.QueryString).Replace(" ", "+");

        string leaseId = utils.NewRequest("leaseId", utils.RequestType.QueryString).Replace(" ", "+"); //可以从 QueryString 来获取,也可以固定 

        string timestamp = utils.NewRequest("timestamp", utils.RequestType.QueryString).Replace(" ", "+"); //可以从 QueryString 来获取 

        string agreementsign = utils.NewRequest("agreementsign", utils.RequestType.QueryString).Replace(" ", "+");


        if (!Taobao.Top.Api.Util.TopUtils.VerifyTopResponse(top_parameters, top_session, top_sign, top_appkey, app_secret))
        {
            Response.Write("top签名验证不通过，请不要非法注入");
            Response.End();
            return;
        }

        nick = Taobao.Top.Api.Util.TopUtils.DecodeTopParams(top_parameters)["visitor_nick"];
        if (nick == null || nick == "")
        {
            Response.Write("top签名验证不通过，请不要非法注入");
            Response.End();
            return;
        }

        //插入信息
        InsertSession();
        Response.Redirect("indextongji.html");
    }

    /// <summary>
    /// 插入session
    /// </summary>
    private void InsertSession()
    {
        TopNickSessionInfo info = new TopNickSessionInfo();
        info.Nick = nick;
        info.Session = top_session;
        info.NickState = true;
        DateTime now = DateTime.Now;
        info.JoinDate = now;
        info.LastGetOrderTime = now;
        info.ShopId = TaoBaoAPIHelper.TaoBaoAPI.GetShopInfo(nick);
        info.ServiceId = Enum.TopTaoBaoService.YingXiaoJueCe;
        new NickSessionService().InsertSerssionNew(info);
        CacheCollection.RemoveCacheByKey(CacheCollection.KEY_ALLNICKSESSIONINFO);

        HttpCookie cookie = new HttpCookie("nick", HttpUtility.UrlEncode(nick));
        HttpCookie cooksession = new HttpCookie("nicksession", top_session);
        HttpCookie cookietongji = new HttpCookie("istongji", "1");

        cookie.Expires = DateTime.Now.AddDays(1);
        cooksession.Expires = DateTime.Now.AddDays(1);
        cookietongji.Expires = DateTime.Now.AddDays(1);

        Response.Cookies.Add(cookie);
        Response.Cookies.Add(cooksession);
        Response.Cookies.Add(cookietongji);
    }
}
