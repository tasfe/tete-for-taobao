using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Data;
using Common;
using QWeiboSDK;
using Oauth4Web;
using LeoShi.Soft.OpenSinaAPI;

public partial class top_microblog_weiboindex : System.Web.UI.Page
{
    private string tokenKey = null;
    private string tokenSecret = null;
    private string baseurl = string.Empty;
    private string appKey = string.Empty;
    private string appSecret = string.Empty;
    private string nick = string.Empty;
    public string count1 = string.Empty;
    public string count2 = string.Empty;
    public string count3 = string.Empty;
    public string count4 = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        //参数初始化
        appKey = "d3225497956249cbb13a7cb7375d62bd";
        appSecret = "6cf7a3274cb676328e77dff3e203061d";
        baseurl = "http://www.7fshop.com";

        Common.Cookie cookie1 = new Common.Cookie();
        string taobaoNick = cookie1.getCookie("nick");
        string isMicroBlog = cookie1.getCookie("mircoblog");
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Decrypt(taobaoNick);

        //过期判断
        if (string.IsNullOrEmpty(taobaoNick))
        {
            string msg = "尊敬的淘宝卖家，非常抱歉的告诉您，您还没有购买此服务或者登录信息已失效，如需继续使用请<a href='http://fuwu.taobao.com/serv/detail.htm?service_id=764' target='_blank'>进入该服务</a>，谢谢！";
            Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
            Response.End();
            return;
        }

        //过期判断
        if (isMicroBlog != "1")
        {
            string msg = "尊敬的" + nick + "，非常抱歉的告诉您，您尚未订购该功能，如需继续使用请<a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-9:1;' target='_blank'>购买该服务</a>，谢谢！";
            Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
            Response.End();
            return;
        }


        //判断是否需要取消
        string act = utils.NewRequest("act", utils.RequestType.QueryString);
        if (act == "del")
        {
            string uid = utils.NewRequest("uid", utils.RequestType.QueryString);
            string typ = utils.NewRequest("typ", utils.RequestType.QueryString);

            string newsql = "DELETE FROM TopMicroBlogAccount WHERE nick = '" + nick + "' AND uid = '" + uid + "' AND typ = '" + typ + "'";
            utils.ExecuteNonQuery(newsql);
            Response.Redirect("weiboindex.aspx");
            return;
        }

        if (!IsPostBack)
        {
            //数据绑定
            string sql = "SELECT * FROM TopMicroBlogAccount WHERE nick = '" + nick + "'";
            DataTable dt = utils.ExecuteDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["typ"].ToString() == "qq")
                {
                    dt.Rows[i]["pass"] = "http://t.qq.com/" + dt.Rows[i]["uid"].ToString();
                }
                else if (dt.Rows[i]["typ"].ToString() == "sina")
                {
                    dt.Rows[i]["pass"] = "http://t.sina.com.cn/" + dt.Rows[i]["uid"].ToString();
                }
            }

            rptMicroBlog.DataSource = dt;
            rptMicroBlog.DataBind();

            //自动发送数据绑定
            sql = "SELECT * FROM TopMicroBlogAuto WHERE nick = '" + nick + "'";
            dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count == 0)
            {
                text1.Value = "我有新宝贝上架了，分享一下：[宝贝标题]，价格：￥[宝贝价格]元，见[宝贝链接]";
                text2.Value = "我有宝贝售出了，分享一下：[宝贝标题]，价格：￥[宝贝价格]元，见[宝贝链接]";
                text3.Value = "我有宝贝给买家评价了，分享一下：[宝贝标题]，价格：￥[宝贝价格]元，见[宝贝链接]";
                text4.Value = "我有新宝贝橱窗推荐了，分享一下：[宝贝标题]，价格：￥[宝贝价格]元，见[宝贝链接]";

                count1 = "0";
                count2 = "0";
                count3 = "0";
                count4 = "0";
            }
            else
            {
                text1.Value = dt.Rows[0]["content1"].ToString();
                text2.Value = dt.Rows[0]["content2"].ToString();
                text3.Value = dt.Rows[0]["content3"].ToString();
                text4.Value = dt.Rows[0]["content4"].ToString();

                count1 = dt.Rows[0]["num1"].ToString();
                count2 = dt.Rows[0]["num2"].ToString();
                count3 = dt.Rows[0]["num3"].ToString();
                count4 = dt.Rows[0]["num4"].ToString();
            }
        }
    }


    protected void Button6_Click(object sender, EventArgs e)
    {
        //oAuthSina oauth = new oAuthSina();
        //oauth.appKey = "1421367737";
        //oauth.appSecret = "2be4da41eb329b6327b7b2ac56ffbe6e";
        //oauth.RequestTokenGet();
        //string url = oauth.AuthorizationGet();
        //new Common.Cookie().setCookie("oauth_token", oauth.token, 100000);
        //new Common.Cookie().setCookie("oauth_token_secret", oauth.tokenSecret, 100000);
        //Response.Redirect(url + "&oauth_callback=" + baseurl + "/top/microblog/record.aspx?typ=sina");

        HttpGet httpRequest = HttpRequestFactory.CreateHttpRequest(Method.GET) as HttpGet;
        httpRequest.AppKey = "1421367737";
        httpRequest.AppSecret = "2be4da41eb329b6327b7b2ac56ffbe6e";
        httpRequest.GetRequestToken();
        string url = httpRequest.GetAuthorizationUrl();
        Session["oauth_token"] = httpRequest.Token;
        Session["oauth_token_secret"] = httpRequest.TokenSecret;
        Response.Redirect(url + "&oauth_callback=" + baseurl + "/top/market/record.aspx?typ=sina");
    }


    protected void Button4_Click(object sender, EventArgs e)
    {
        //跳转到授权页面
        if (GetRequestToken(appKey, appSecret) != false)
        {
            Session["tokenSecret"] = tokenSecret;
            string url = "http://open.t.qq.com/cgi-bin/authorize?oauth_token=" + tokenKey;
            Response.Redirect(url);
            return;
        }
    }

    protected void Button14_Click(object sender, EventArgs e)
    {
        string text1 = "我有新宝贝上架了，分享一下：[宝贝标题]，价格：￥[宝贝价格]元，见[宝贝链接]";
        string text2 = "我有宝贝售出了，分享一下：[宝贝标题]，价格：￥[宝贝价格]元，见[宝贝链接]";
        string text3 = "我有宝贝给买家评价了，分享一下：[宝贝标题]，价格：￥[宝贝价格]元，见[宝贝链接]";
        string text4 = "我有新宝贝橱窗推荐了，分享一下：[宝贝标题]，价格：￥[宝贝价格]元，见[宝贝链接]";

        string sql = "UPDATE TopMicroBlogAuto SET " +
                        "content1 = '" + text1 + "', " +
                        "content2 = '" + text2 + "', " +
                        "content3 = '" + text3 + "', " +
                        "content4 = '" + text4 + "' " +
                    "WHERE nick = '" + nick + "'";
        utils.ExecuteNonQuery(sql);
        Response.Redirect("weiboindex.aspx");
    }

    protected void Button5_Click(object sender, EventArgs e)
    {
        string sql = "SELECT * FROM TopMicroBlogAccount WHERE nick = '" + nick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (dt.Rows[i]["typ"].ToString() == "qq")
            {
                //发送微博
                List<Parameter> parameters = new List<Parameter>();
                parameters.Add(new Parameter("content", "测试微博自动发送"));

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
                else
                {
                    //textOutput.Text = "请求失败...";
                }
            }
            else if (dt.Rows[i]["typ"].ToString() == "sina")
            {
                //oAuthSina oauth = new oAuthSina();
                //oauth.appKey = "1421367737";
                //oauth.appSecret = "2be4da41eb329b6327b7b2ac56ffbe6e";
                //oauth.token = dt.Rows[i]["tokenKey"].ToString();
                //oauth.tokenSecret = dt.Rows[i]["tokenSecrect"].ToString();

                //string url = "http://api.t.sina.com.cn/statuses/update.xml?";
                //string result = oauth.RequestWithPicture(oAuthSina.Method.POST, url, "status=微博自动发送啊啊啊啊啊");


                //Response.Write(result);
            }
        }
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        //先判断是否关联过博客
        string sql = "SELECT COUNT(*) FROM TopMicroBlogAccount WHERE nick = '" + nick + "'";
        string count = utils.ExecuteString(sql);
        if (count == "0")
        {
            Response.Write("<script>alert('请先关联微博帐号');history.go(-1);</script>");
            Response.End();
            return;
        }

        //先判断是否有记录
        sql = "SELECT COUNT(*) FROM TopMicroBlogAuto WHERE nick = '" + nick + "'";
        count = utils.ExecuteString(sql);
        if (count == "0")
        {
            sql = "INSERT INTO TopMicroBlogAuto (" +
                        "nick, " +
                        "content1, " +
                        "content2, " +
                        "content3, " +
                        "content4 " +
                    " ) VALUES ( " +
                        " '" + nick + "', " +
                        " '" + utils.NewRequest("text1", utils.RequestType.Form) + "', " +
                        " '" + utils.NewRequest("text2", utils.RequestType.Form) + "', " +
                        " '" + utils.NewRequest("text3", utils.RequestType.Form) + "', " +
                        " '" + utils.NewRequest("text4", utils.RequestType.Form) + "' " +
                    ") ";
            utils.ExecuteNonQuery(sql);
        }
        else
        {
            sql = "UPDATE TopMicroBlogAuto SET " +
                        "content1 = '" + utils.NewRequest("text1", utils.RequestType.Form) + "', " +
                        "content2 = '" + utils.NewRequest("text2", utils.RequestType.Form) + "', " +
                        "content3 = '" + utils.NewRequest("text3", utils.RequestType.Form) + "', " +
                        "content4 = '" + utils.NewRequest("text4", utils.RequestType.Form) + "' " +
                    "WHERE nick = '" + nick + "'";
            utils.ExecuteNonQuery(sql);
        }

        Response.Write("<script>alert('保存成功，程序将自动在每天的热门时段帮您自动发送微博！');history.go(-1);</script>");
        Response.End();
        return;
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
        oauthKey.callbackUrl = baseurl + "/top/market/record.aspx?typ=qq";

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