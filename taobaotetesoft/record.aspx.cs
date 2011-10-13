using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using Common;
using QWeiboSDK;
using System.Text;
using System.Text.RegularExpressions;

public partial class record : System.Web.UI.Page
{
    private string tokenKey = null;
    private string tokenSecret = null;
    private string weiboName = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        Common.Cookie cookie1 = new Common.Cookie();
        string nick = cookie1.getCookie("nickweibo");

        string appKey = "d3225497956249cbb13a7cb7375d62bd";
        string appSecret = "6cf7a3274cb676328e77dff3e203061d";
        tokenKey = utils.NewRequest("oauth_token", utils.RequestType.QueryString);
        tokenSecret = Session["tokenSecret"].ToString();

        string verify = utils.NewRequest("oauth_verifier", utils.RequestType.QueryString);
        //获取账户信息
        GetAccessToken(appKey, appSecret, tokenKey, tokenSecret, verify);

        Common.Cookie cookie = new Common.Cookie();
        cookie.setCookie("tokenKey", tokenKey, 999999);
        cookie.setCookie("tokenSecret", tokenSecret, 999999);

        List<Parameter> parameters = new List<Parameter>();
        List<Parameter> files = new List<Parameter>();
        //身份验证
        OauthKey oauthKey = new OauthKey();
        oauthKey.customKey = appKey;
        oauthKey.customSecrect = appSecret;
        oauthKey.tokenKey = tokenKey;
        oauthKey.tokenSecrect = tokenSecret;

        QWeiboRequest request = new QWeiboRequest();
        int nKey = 0;
        if (request.AsyncRequest("http://open.t.qq.com/api/statuses/broadcast_timeline", "POST", oauthKey, parameters, files, new AsyncRequestCallback(RequestCallback), out nKey))
        {
            //textOutput.Text = "请求中...";
        }

        //insert sql
        string sql = "SELECT COUNT(*) FROM TopMicroBlogAccount WHERE typ = 'qq' AND uid = '" + weiboName + "'";
        string count = utils.ExecuteString(sql);
        if (count == "0")
        {
            //写入数据库
            sql = "INSERT INTO TopMicroBlogAccount (nick, uid, typ, tokenKey, tokenSecrect) VALUES ('" + nick + "', '" + weiboName + "', 'qq', '" + tokenKey + "', '" + tokenSecret + "')";
            utils.ExecuteNonQuery(sql);

            //赠送积分

            //记录操作日志
            sql = "INSERT INTO TopMicroBlogNumLog (uid, typ, num) VALUES ('" + weiboName + "', 'reg', 100)";
            utils.ExecuteNonQuery(sql);

            //增加积分
            sql = "UPDATE TopMicroBlogAccount SET score = score + 100 WHERE uid = '" + weiboName + "'";
            utils.ExecuteNonQuery(sql);
        }

        cookie.setCookie("uid", weiboName, 999999);

        Response.Redirect("menu.aspx");
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
        //Response.Write(weiboName);
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