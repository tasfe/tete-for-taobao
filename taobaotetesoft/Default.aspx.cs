﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Data;
using Common;
using QWeiboSDK;

public partial class _Default : System.Web.UI.Page
{
    private string tokenKey = null;
    private string tokenSecret = null;
    private string baseurl = string.Empty;
    private string appKey = string.Empty;
    private string appSecret = string.Empty;
    public string count = string.Empty;
    public string weiboName = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        appKey = "d3225497956249cbb13a7cb7375d62bd";
        appSecret = "6cf7a3274cb676328e77dff3e203061d";
        baseurl = "http://weibo.tetesoft.com/";

        string sql = "SELECT COUNT(*) FROM TopMicroBlogAccount WHERE typ = 'qq'";
        string truecount = utils.ExecuteString(sql);
        count = (int.Parse(truecount) + 31000).ToString();

        string f = utils.NewRequest("f", utils.RequestType.QueryString);
        if (f == "tete")
        {
            Act();
        }
    }

    private void Act()
    {
        Common.Cookie cookie = new Common.Cookie();
        tokenKey = cookie.getCookie("tokenKey");
        tokenSecret = cookie.getCookie("tokenSecret");
        string uid = cookie.getCookie("uid");

        if (tokenKey != null && tokenSecret != null && uid != null)
        {
            weiboName = uid;



            string score = utils.ExecuteString("SELECT score FROM TopMicroBlogAccount WHERE uid = '" + weiboName + "'");
            if (int.Parse(score) > 20)
            {
                //登录增加20个粉丝
                string sql = "SELECT TOP 20 * FROM TopMicroBlogAccount WHERE typ = 'qq' AND score > 0 AND uid <> '' ORDER BY NEWID()";
                DataTable dt = utils.ExecuteDataTable(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    listen(dt.Rows[i]["uid"].ToString(), weiboName, dt.Rows[i]["tokenKey"].ToString(), dt.Rows[i]["tokenSecrect"].ToString());
                }
            }




            string num = new Random(int.Parse(DateTime.Now.Second.ToString())).Next(0, 100).ToString();

            string str = "#互听##互听工具#【特特互听】我在用的免费互听工具，可以安全迅速的增加听众~~(" + num + ")..http://weibo.tetesoft.com";
            SendMessage(str);
            return;
        }

        //跳转到授权页面
        if (GetRequestToken(appKey, appSecret) != false)
        {
            Session["tokenSecret"] = tokenSecret;
            string url = "http://open.t.qq.com/cgi-bin/authorize?oauth_token=" + tokenKey;
            Response.Redirect(url);
            return;
        }
    }



    protected void Button1_Click(object sender, EventArgs e)
    {
        Act();
    }






    private void listen(string uid, string listento, string tokenKey, string tokenSecret)
    {
        string appKey = "d3225497956249cbb13a7cb7375d62bd";
        string appSecret = "6cf7a3274cb676328e77dff3e203061d";
        //身份验证
        OauthKey oauthKey = new OauthKey();
        oauthKey.customKey = appKey;
        oauthKey.customSecrect = appSecret;
        oauthKey.tokenKey = tokenKey;
        oauthKey.tokenSecrect = tokenSecret;

        //关注对方
        QWeiboRequest request = new QWeiboRequest();
        int nKey = 0;
        List<Parameter> parameters = new List<Parameter>();
        parameters.Add(new Parameter("name", listento));
        if (request.AsyncRequest("http://open.t.qq.com/api/friends/add", "POST", oauthKey, parameters, null, new AsyncRequestCallback(RequestCallbackListen), out nKey))
        {

        }

        //记录日志
        string sql = "INSERT INTO TopMicroBlogListen (uid, listen) VALUES ('" + uid + "', '" + listento + "')";
        utils.ExecuteNonQuery(sql);

        //记录操作日志
        sql = "INSERT INTO TopMicroBlogNumLog (uid, typ, num, bak) VALUES ('" + listento + "', 'deduct', -1, '" + uid + "')";
        utils.ExecuteNonQuery(sql);

        //减少积分
        sql = "UPDATE TopMicroBlogAccount SET score = score - 1 WHERE uid = '" + listento + "'";
        utils.ExecuteNonQuery(sql);
        //Response.Write("【" + uid + "】收听【" + listento + "】成功");
    }


    protected void RequestCallbackListen(int key, string content)
    {
        Encoding utf8 = Encoding.GetEncoding(65001);
        Encoding defaultChars = Encoding.Default;
        byte[] temp = utf8.GetBytes(content);
        byte[] temp1 = Encoding.Convert(utf8, defaultChars, temp);
        string result = defaultChars.GetString(temp1);
    }





    /// <summary>
    /// 发送推广微博
    /// </summary>
    private void SendMessage(string content)
    {
        string appKey = "d3225497956249cbb13a7cb7375d62bd";
        string appSecret = "6cf7a3274cb676328e77dff3e203061d";

        List<Parameter> parameters = new List<Parameter>();
        parameters.Add(new Parameter("content", content));

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
        if (request.AsyncRequest("http://open.t.qq.com/api/t/add", "POST", oauthKey, parameters, files, new AsyncRequestCallback(RequestCallbackSend), out nKey))
        {
            //textOutput.Text = "请求中...";
        }
    }

    protected void RequestCallbackSend(int key, string content)
    {
        Encoding utf8 = Encoding.GetEncoding(65001);
        Encoding defaultChars = Encoding.Default;
        byte[] temp = utf8.GetBytes(content);
        byte[] temp1 = Encoding.Convert(utf8, defaultChars, temp);
        string result = defaultChars.GetString(temp1);


        //更新登录时间和登录次数
        string sql = "UPDATE TopMicroBlogAccount SET lastlogin = GETDATE(), logintimes = logintimes + 1, result = '" + result.Replace("'", "''") + "' WHERE uid = '" + weiboName + "'";
        utils.ExecuteNonQuery(sql);
        //Response.Write(result + "<br><br>");
        Response.Redirect("menu.aspx");
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
        oauthKey.callbackUrl = baseurl + "record.aspx?typ=qq";

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