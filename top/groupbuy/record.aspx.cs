using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using QWeiboSDK;
using System.Text;
using Common;
using System.Text.RegularExpressions;
using LeoShi.Soft.OpenSinaAPI;

public partial class top_groupbuy_record : System.Web.UI.Page
{
    private string tokenKey = null;
    private string tokenSecret = null;
    private string appKey = string.Empty;
    private string appSecret = string.Empty;
    private string baseurl = string.Empty;
    private string weiboName = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        //参数初始化
        baseurl = "http://groupbuy.7fshop.com";

        Common.Cookie cookie1 = new Common.Cookie();
        string taobaoNick = cookie1.getCookie("nick");

        //过期判断
        if (string.IsNullOrEmpty(taobaoNick))
        {
            Response.Write("<script>parent.location.href='http://container.open.taobao.com/container?appkey=12159997'</script>");
            Response.End();
            return;
        }

        Rijndael_ encode = new Rijndael_("tetesoft");
        string nick = encode.Decrypt(taobaoNick);

        string typ = utils.NewRequest("typ", utils.RequestType.QueryString);
        string verify = string.Empty;

        if (typ == "qq")
        {
            appKey = "d3225497956249cbb13a7cb7375d62bd";
            appSecret = "6cf7a3274cb676328e77dff3e203061d";

            tokenKey = utils.NewRequest("oauth_token", utils.RequestType.QueryString);
            tokenSecret = Session["tokenSecret"].ToString();
            verify = utils.NewRequest("oauth_verifier", utils.RequestType.QueryString);
            //获取账户信息
            GetAccessToken(appKey, appSecret, tokenKey, tokenSecret, verify);

            //发送微博
            List<Parameter> parameters = new List<Parameter>();

            //身份验证
            OauthKey oauthKey = new OauthKey();
            oauthKey.customKey = appKey;
            oauthKey.customSecrect = appSecret;
            oauthKey.tokenKey = tokenKey;
            oauthKey.tokenSecrect = tokenSecret;

            //图片信息
            List<Parameter> files = new List<Parameter>();

            QWeiboRequest request = new QWeiboRequest();
            int nKey = 0;
            if (request.AsyncRequest("http://open.t.qq.com/api/user/info", "POST", oauthKey, parameters, files, new AsyncRequestCallback(RequestCallback), out nKey))
            {
                //textOutput.Text = "请求中...";
            }

            string sql = "SELECT COUNT(*) FROM TopMicroBlogAccount WHERE nick = '" + nick + "' AND uid='" + weiboName + "' AND typ = 'qq'";
            string count = utils.ExecuteString(sql);
            if (count == "0")
            {
                //写入数据库
                sql = "INSERT INTO TopMicroBlogAccount (nick, uid, typ, tokenKey, tokenSecrect) VALUES ('" + nick + "', '" + weiboName + "', 'qq', '" + tokenKey + "', '" + tokenSecret + "')";
                utils.ExecuteNonQuery(sql);
            }
        }
        else if (typ == "sina")
        {
            tokenKey = Session["oauth_token"].ToString();
            tokenSecret = Session["oauth_token_secret"].ToString();
            verify = utils.NewRequest("oauth_verifier", utils.RequestType.QueryString);

            HttpGet httpRequest = HttpRequestFactory.CreateHttpRequest(Method.GET) as HttpGet;
            httpRequest.AppKey = "1421367737";
            httpRequest.AppSecret = "2be4da41eb329b6327b7b2ac56ffbe6e";
            httpRequest.Token = tokenKey;
            httpRequest.TokenSecret = tokenSecret;
            httpRequest.Verifier = verify;
            httpRequest.GetAccessToken();
            tokenKey = httpRequest.Token;
            tokenSecret = httpRequest.TokenSecret;
            weiboName = httpRequest.UserId;

            string sql = "SELECT COUNT(*) FROM TopMicroBlogAccount WHERE nick = '" + nick + "' AND uid='" + weiboName + "' AND typ = 'sina'";
            string count = utils.ExecuteString(sql);
            if (count == "0")
            {
                sql = "INSERT INTO TopMicroBlogAccount (nick, uid, typ, tokenKey, tokenSecrect) VALUES ('" + nick + "', '" + weiboName + "', 'sina', '" + tokenKey + "', '" + tokenSecret + "')";
                utils.ExecuteNonQuery(sql);
            }
        }
        //Response.Write(sql);
        //跳转
        Response.Redirect("weiboindex.aspx");
    }


    protected void RequestCallback(int key, string content)
    {
        Encoding utf8 = Encoding.GetEncoding(65001);
        Encoding defaultChars = Encoding.Default;
        byte[] temp = utf8.GetBytes(content);
        byte[] temp1 = Encoding.Convert(utf8, defaultChars, temp);
        string result = defaultChars.GetString(temp1);

        //获取微博名称
        weiboName = Regex.Match(result, @"""name"":""([^""]*)""").Groups[1].ToString();
        //Response.Write(result);
    }

    private bool GetRequestToken(string customKey, string customSecret)
    {
        string url = "https://open.t.qq.com/cgi-bin/request_token";
        List<Parameter> parameters = new List<Parameter>();
        OauthKey oauthKey = new OauthKey();
        oauthKey.customKey = customKey;
        oauthKey.customSecrect = customSecret;
        oauthKey.callbackUrl = baseurl + "/top/groupbuy/record.aspx";

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