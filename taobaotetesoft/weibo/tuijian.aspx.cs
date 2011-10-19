using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using Common;
using QWeiboSDK;
using System.Text;
using System.Data;

public partial class weibo_tuijian : System.Web.UI.Page
{
    private string appKey = string.Empty;
    private string appSecret = string.Empty;
    private string tokenKey = string.Empty;
    private string tokenSecret = string.Empty;
    private string uid = string.Empty;
    public string str = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        appKey = "d3225497956249cbb13a7cb7375d62bd";
        appSecret = "6cf7a3274cb676328e77dff3e203061d";

        Common.Cookie cookie = new Common.Cookie();
        tokenKey = cookie.getCookie("tokenKey");
        tokenSecret = cookie.getCookie("tokenSecret");
        uid = cookie.getCookie("uid");

        string sql = string.Empty;

        //每小时最多一键收听一次
        sql = "SELECT COUNT(*) FROM TopMicroBlogNumLog WHERE typ = 'send' AND uid = '" + uid + "' AND  DATEDIFF(s, adddate, GETDATE() ) < 86400";
        string count = utils.ExecuteString(sql);

        if (count == "0")
        {
            string str = "#互听##互听工具#您还在为没有粉丝烦恼吗，向您推荐一款免费迅速的增加您粉丝的软件，让您迅速拥有成千上万的粉丝..http://weibo.tetesoft.com";
            SendMessage(str);

            //记录操作日志
            sql = "INSERT INTO TopMicroBlogNumLog (uid, typ, num) VALUES ('" + uid + "', 'send', 10)";
            utils.ExecuteNonQuery(sql);

            //增加积分
            sql = "UPDATE TopMicroBlogAccount SET score = score + 5 WHERE uid = '" + uid + "'";
            utils.ExecuteNonQuery(sql);

            //输出提示
            str = "收听成功，+5积分！";
        }
        else
        {
            //输出提示
            str = "不好意思，一天内最多微博推荐一次，请您稍后再试~";
        }
    }

    /// <summary>
    /// 发送推广微博
    /// </summary>
    private void SendMessage(string content)
    {
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
        if (request.AsyncRequest("http://open.t.qq.com/api/t/add", "POST", oauthKey, parameters, files, new AsyncRequestCallback(RequestCallback), out nKey))
        {
            //textOutput.Text = "请求中...";
        }
    }

    protected void RequestCallback(int key, string content)
    {
        Encoding utf8 = Encoding.GetEncoding(65001);
        Encoding defaultChars = Encoding.Default;
        byte[] temp = utf8.GetBytes(content);
        byte[] temp1 = Encoding.Convert(utf8, defaultChars, temp);
        string result = defaultChars.GetString(temp1);

        //Response.Write(result);
    }
}