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
using System.Net;
using System.IO;
using System.Text;
using System.Collections.Generic;

public partial class container : System.Web.UI.Page
{
    public string top_session = string.Empty;
    public string nick = string.Empty;
    public string top_appkey = string.Empty;
    public string app_secret = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        IDictionary<string, string> param = new Dictionary<string, string>();
        param.Add("client_id", "21093339");
        param.Add("client_secret", "c1c22ba85fb91bd20279213ef7b9ee80");
        param.Add("grant_type", "authorization_code");
        param.Add("code", Request.QueryString["code"]);
        param.Add("redirect_uri", "http://www.fensehenghuo.com/containerNew.aspx");
        string result = "";
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://oauth.taobao.com/token");
        req.Method = "POST";
        req.KeepAlive = true;
        req.Timeout = 300000;
        req.ContentType = "application/x-www-form-urlencoded;charset=gb2312";
        byte[] postData = Encoding.UTF8.GetBytes(PostData(param));
        Stream reqStream = req.GetRequestStream();
        reqStream.Write(postData, 0, postData.Length);
        reqStream.Close();
        try
        {
            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
            Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
            Stream stream = null;
            StreamReader reader = null;
            stream = rsp.GetResponseStream();
            reader = new StreamReader(stream, encoding);
            result = reader.ReadToEnd();
            if (reader != null) reader.Close();
            if (stream != null) stream.Close();
            if (rsp != null) rsp.Close();
        }
        catch (Exception ex)
        {
            //ServiceLog.RecodeLog("session:" + session + "获取淘宝服务响应错误：" + ex.Message);
        }

        Response.Write(result);
        Response.End();
        ////签名验证
        //top_appkey = "21093339";
        //app_secret = "c1c22ba85fb91bd20279213ef7b9ee80";

        ////string top_parameters = utils.NewRequest("top_parameters", utils.RequestType.QueryString).Replace(" ", "+");
        //top_session = utils.NewRequest("access_token", utils.RequestType.QueryString).Replace(" ", "+");
        ////nick = Taobao.Top.Api.Util.TopUtils.DecodeTopParams(top_parameters)["taobao_user_nick"];
        
        ////Response.Write(nick);
        //Response.Write(top_session);
        //Response.End();

        //if (nick == null || nick == "")
        //{
        //    Response.Write("top签名验证不通过，请不要非法注入");
        //    Response.End();
        //    return;
        //}

        ////插入信息
        //InsertSession();
        //Response.Redirect("indextongji.html");
    }

    protected static string PostData(IDictionary<string, string> parameters)
    {
        StringBuilder postData = new StringBuilder();
        bool hasParam = false;
        IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
        while (dem.MoveNext())
        {
            string name = dem.Current.Key;
            string value = dem.Current.Value;
            // 忽略参数名或参数值为空的参数 
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
            {
                if (hasParam)
                {
                    postData.Append("&");
                }
                postData.Append(name);
                postData.Append("=");
                postData.Append(Uri.EscapeDataString(value));
                hasParam = true;
            }
        }
        return postData.ToString();
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
        info.ShopId = "";//先赋空值
        info.ServiceId = Enum.TopTaoBaoService.YingXiaoJueCe;
        //有则不添加
        if (CacheCollection.GetNickSessionList().Where(o => o.Nick == nick && o.ServiceId == Enum.TopTaoBaoService.YingXiaoJueCe).ToList().Count == 0)
        {
            //先添加后删除缓存
            new NickSessionService().InsertSerssionNew(info);
            CacheCollection.RemoveCacheByKey(CacheCollection.KEY_ALLNICKSESSIONINFO);
        }
        else
        {
            //新session赋值
            CacheCollection.GetNickSessionList().Where(o => o.Nick == nick && o.ServiceId == Enum.TopTaoBaoService.YingXiaoJueCe).ToList()[0].Session = top_session;
        }
        //修改缓存后读取店铺信息
        info.ShopId = TaoBaoAPI.GetShopInfo(nick, top_session);
        //更新店铺信息
        new NickSessionService().UpdateSession(info);
        //更新缓存
        CacheCollection.GetNickSessionList().Where(o => o.Nick == nick && o.ServiceId == Enum.TopTaoBaoService.YingXiaoJueCe).ToList()[0].ShopId = info.ShopId;

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
