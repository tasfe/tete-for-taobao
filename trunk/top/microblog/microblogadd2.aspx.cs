using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using Common;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Text;
using System.Data;
using QWeiboSDK;
using Oauth4Web;
using LeoShi.Soft.OpenSinaAPI;

public partial class top_microblog_microblogadd2 : System.Web.UI.Page
{
    public string topic = string.Empty;
    private string tokenKey = null;
    private string tokenSecret = null;
    private string baseurl = string.Empty;
    private string appKey = string.Empty;
    private string appSecret = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        //参数初始化
        appKey = "d3225497956249cbb13a7cb7375d62bd";
        appSecret = "6cf7a3274cb676328e77dff3e203061d";
        baseurl = "http://www.7fshop.com";

        topic = utils.NewRequest("topic", utils.RequestType.Form);
        topic = HttpUtility.UrlDecode(topic);

        SendMicroBlog();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Common.Cookie cookie = new Common.Cookie();
        string cookieStr = cookie.getCookie("tCookie");
        ////登录QQ微博
        //if (string.IsNullOrEmpty(cookieStr))
        //{
        //    QQLogin();
        //}
        //发送微博
        SendMicroBlog();
    }

    private void SendMicroBlog()
    {
        //拼装发送内容
        string sendContent = string.Empty;
        sendContent = "#" + topic + "#";
        sendContent += this.tbContent.Text;

        Common.Cookie cookie1 = new Common.Cookie();
        string taobaoNick1 = cookie1.getCookie("nick");
        string nickid = string.Empty;

        //COOKIE过期判断
        if (taobaoNick1 == "")
        {
            //SESSION超期 跳转到登录页
            Response.Write("<script>parent.location.href='http://container.open.taobao.com/container?appkey=12159997'</script>");
            Response.End();
        }
        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick1 = encode.Decrypt(taobaoNick1);
        string sqlNew = "SELECT sid FROM TopTaobaoShop WHERE nick = '" + taobaoNick1 + "'";
        DataTable dtNew = utils.ExecuteDataTable(sqlNew);
        if (dtNew.Rows.Count != 0)
        {
            nickid = "http://shop" + dtNew.Rows[0]["sid"].ToString() + ".taobao.com/";
        }
        else
        {
            nickid = "http://www.taobao.com/";
        }
        sendContent += nickid;

        //自动往空间
        SendMsg(sendContent, taobaoNick1, "");

        Response.Write("<script>alert('发送成功');window.location.href='microblogadd.aspx';</script>");
        Response.End();
    }


    //向空间发送说说
    private void SendMsg(string content, string nick, string mainurl)
    {
        string appKey = "d3225497956249cbb13a7cb7375d62bd";
        string appSecret = "6cf7a3274cb676328e77dff3e203061d";
        string sql = "SELECT * FROM TopMicroBlogAccount WHERE nick = '" + nick + "'";

        DataTable dt = utils.ExecuteDataTable(sql);
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            //发送微博
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("content", content));

            //身份验证
            OauthKey oauthKey = new OauthKey();
            oauthKey.customKey = appKey;
            oauthKey.customSecrect = appSecret;
            oauthKey.tokenKey = dt.Rows[i]["tokenKey"].ToString();
            oauthKey.tokenSecrect = dt.Rows[i]["tokenSecrect"].ToString();

            //图片信息
            List<Parameter> files = new List<Parameter>();

            QWeiboRequest request = new QWeiboRequest();
            int nKey = 0;
            if (request.AsyncRequest("http://open.t.qq.com/api/t/add", "POST", oauthKey, parameters, files, new AsyncRequestCallback(RequestCallback), out nKey))
            {
                //textOutput.Text = "请求中...";
            }

            sql = "INSERT INTO TopMicroBlogSendLog (result, nick, uid, typ, content, auto) VALUES ('','" + nick + "','" + dt.Rows[i]["uid"].ToString() + "','qq','" + content + "','99')";
            utils.ExecuteNonQuery(sql);
        }
    }

    private void QQLogin()
    {
        Common.Cookie cookie = new Common.Cookie();
        string verify = cookie.getCookie("qqverify");

        string url = "http://ptlogin2.qq.com/login?u=" + Request.Form["tbUserName"] + "&p=" + Request.Form["tbPassword"].ToUpper() + "&verifycode=" + Request.Form["tbVerify"] + "&aid=46000101&u1=http%3A%2F%2Ft.qq.com&ptredirect=1&h=1&from_ui=1&dumy=&fp=loginerroralert";

        //准备生成
        string strHtml = string.Empty;

        StreamReader sr = null; //用来读取流
        StreamWriter sw = null; //用来写文件
        Encoding code = Encoding.GetEncoding("utf-8"); //定义编码 
        ASCIIEncoding encoding = new ASCIIEncoding();
        //构造web请求，发送请求，获取响应
        WebRequest HttpWebRequest = null;
        HttpWebRequest = WebRequest.Create(url);
        HttpWebRequest.Headers.Set("Cookie", "verifysession=" + verify + ";");
        //发送
        WebResponse HttpWebResponse = null;
        HttpWebResponse = HttpWebRequest.GetResponse();
        //获得流
        sr = new StreamReader(HttpWebResponse.GetResponseStream(), code);
        strHtml = sr.ReadToEnd();

        string aaa = HttpWebResponse.Headers.Get("Set-Cookie").ToString();
        //保存进COOKIE
        string cookieStr = string.Empty;

        cookieStr += getKeyData(@"pt2gguin=([^;]*);", aaa);
        cookieStr += getKeyData(@"uin=([^;]*);", aaa);
        cookieStr += getKeyData(@"skey=([^;]*);", aaa);
        cookieStr += getKeyData(@"ptcz=([^;]*);", aaa);

        //保存登录状态
        Rijndael_ encode = new Rijndael_("tetesoft");
        cookie.setCookie("tmpQQ", encode.Encrypt(cookieStr), 999999);

        cookieStr += "verifysession=" + verify + ";";

        //请求说说首页获取COOKIE
        url = "http://t.qq.com";
        HttpWebRequest = WebRequest.Create(url);
        HttpWebRequest.Headers.Set("Cookie", cookieStr);
        //获得流
        sr = new StreamReader(HttpWebResponse.GetResponseStream(), code);
        strHtml = sr.ReadToEnd();
        //发送
        HttpWebResponse = null;
        HttpWebResponse = HttpWebRequest.GetResponse();

        string direct = HttpWebResponse.ResponseUri.ToString();

        url = direct;
        HttpWebRequest = WebRequest.Create(url);
        HttpWebRequest.Headers.Set("Cookie", cookieStr);
        //获得流
        sr = new StreamReader(HttpWebResponse.GetResponseStream(), code);
        strHtml = sr.ReadToEnd();
        //发送
        HttpWebResponse = null;
        HttpWebResponse = HttpWebRequest.GetResponse();
        string ccc = HttpWebResponse.Headers.ToString();

        cookie.setCookie("tCookie", cookieStr.Replace(";", "|"), 999999);
        cookie.setCookie("directUrl", direct, 999999);
        return;
    }

    private string getKeyData(string pat, string str)
    {
        string verify = string.Empty;
        Regex reg = new Regex(pat);
        if (reg.IsMatch(str))
        {
            Match match = reg.Match(str);
            verify = match.Groups[0].ToString();
        }
        return verify;
    }












    protected void RequestCallback(int key, string content)
    {
        Encoding utf8 = Encoding.GetEncoding(65001);
        Encoding defaultChars = Encoding.Default;
        byte[] temp = utf8.GetBytes(content);
        byte[] temp1 = Encoding.Convert(utf8, defaultChars, temp);
        string result = defaultChars.GetString(temp1);

        Response.Write(result);
    }

    private bool GetRequestToken(string customKey, string customSecret)
    {
        string url = "https://open.t.qq.com/cgi-bin/request_token";
        List<Parameter> parameters = new List<Parameter>();
        OauthKey oauthKey = new OauthKey();
        oauthKey.customKey = customKey;
        oauthKey.customSecrect = customSecret;
        oauthKey.callbackUrl = baseurl + "/top/microblog/record.aspx?typ=qq";

        QWeiboRequest request = new QWeiboRequest();
        return ParseToken(request.SyncRequest(url, "GET", oauthKey, parameters, null));
    }

    private bool GetAccessToken(string customKey, string customSecret, string requestToken, string requestTokenSecrect, string verify)
    {
        string url = "https://open.t.qq.com/cgi-bin/access_token";
        List<Parameter> parameters = new List<Parameter>();
        OauthKey oauthKey = new OauthKey();
        oauthKey.customKey = customKey;
        oauthKey.customSecrect = customSecret;
        oauthKey.tokenKey = requestToken;
        oauthKey.tokenSecrect = requestTokenSecrect;
        oauthKey.verify = verify;

        QWeiboRequest request = new QWeiboRequest();
        return ParseToken(request.SyncRequest(url, "GET", oauthKey, parameters, null));
    }

    private bool ParseToken(string response)
    {
        if (string.IsNullOrEmpty(response))
        {
            return false;
        }

        string[] tokenArray = response.Split('&');

        if (tokenArray.Length < 2)
        {
            return false;
        }

        string strTokenKey = tokenArray[0];
        string strTokenSecrect = tokenArray[1];

        string[] token1 = strTokenKey.Split('=');
        if (token1.Length < 2)
        {
            return false;
        }
        tokenKey = token1[1];

        string[] token2 = strTokenSecrect.Split('=');
        if (token2.Length < 2)
        {
            return false;
        }
        tokenSecret = token2[1];

        return true;
    }
}