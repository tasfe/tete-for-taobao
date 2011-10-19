using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using Common;
using QWeiboSDK;
using System.Text;
using System.Data;

public partial class weibo_listen : System.Web.UI.Page
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

        //Response.Write(tokenKey + "-");
        //Response.Write(tokenSecret);

        string sql = string.Empty;
        string uids = string.Empty;

        uids = "以下为您具体收听的微博清单：<br>";

        //每小时最多一键收听一次
        sql = "SELECT COUNT(*) FROM TopMicroBlogNumLog WHERE typ = 'onekey' AND uid = '" + uid + "' AND  DATEDIFF(s, adddate, GETDATE() ) < 3600";
        string count = utils.ExecuteString(sql);

        if (count == "0")
        {
            //一键收听，取得自己没有收听过的20个随机账户
            sql = "SELECT TOP 20 uid FROM TopMicroBlogAccount WHERE typ = 'qq' AND score > 0 AND uid <> '' AND uid NOT IN (SELECT listen FROM TopMicroBlogListen WHERE uid = '" + uid + "') ORDER BY NEWID()";
            DataTable dt = utils.ExecuteDataTable(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                listen(dt.Rows[i]["uid"].ToString());

                uids += "<a href='http://t.qq.com/" + dt.Rows[i]["uid"].ToString() + "' target='_blank'></a><br>";
            }

            //记录操作日志
            sql = "INSERT INTO TopMicroBlogNumLog (uid, typ, num) VALUES ('" + uid + "', 'onekey', 20)";
            utils.ExecuteNonQuery(sql);

            //增加积分
            sql = "UPDATE TopMicroBlogAccount SET score = score + 20 WHERE uid = '" + uid + "'";
            utils.ExecuteNonQuery(sql);

            //输出提示
            str = uids + "<br>收听成功，+20积分！";
        }
        else
        {
            //输出提示
            str = "不好意思，一个小时内最多一键收听一次，请您稍后再试~";
        }
    }

    private void listen(string listento)
    {
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
        if (request.AsyncRequest("http://open.t.qq.com/api/friends/add", "POST", oauthKey, parameters, null, new AsyncRequestCallback(RequestCallback), out nKey))
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