using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using Common;
using QWeiboSDK;
using System.Text;
using System.Data;

public partial class weibo_teteauto1 : System.Web.UI.Page
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

        string sql = string.Empty;

        //每小时最多一键收听一次
        sql = "SELECT uid FROM TopMicroBlogAccount WHERE typ = 'qq' AND score > 0 AND uid <> '' AND id > 672";
        DataTable dt = utils.ExecuteDataTable(sql);
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            autoListen(dt.Rows[i]["uid"].ToString());
            Response.Write("<br>****************************************************<br>");
        }
    }

    /// <summary>
    /// 自动收听某客户
    /// </summary>
    /// <param name="p"></param>
    /// <param name="p_2"></param>
    /// <param name="p_3"></param>
    private void autoListen(string uid)
    {
        string sql = string.Empty;

        //每小时最多一键收听一次
        sql = "SELECT TOP 20 * FROM TopMicroBlogAccount WHERE typ = 'qq' AND score > 0 AND uid <> '' AND id < 672 ORDER BY NEWID()";
        DataTable dt = utils.ExecuteDataTable(sql);
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            listen(dt.Rows[i]["uid"].ToString(), uid, dt.Rows[i]["tokenKey"].ToString(), dt.Rows[i]["tokenSecret"].ToString());
        }
    }

    private void listen(string uid, string listento, string tokenKey, string tokenSecret)
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
        parameters.Add(new Parameter("name", uid));
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
        Response.Write("【" + uid + "】收听【" + listento + "】成功");
    }


    protected void RequestCallback(int key, string content)
    {
        Encoding utf8 = Encoding.GetEncoding(65001);
        Encoding defaultChars = Encoding.Default;
        byte[] temp = utf8.GetBytes(content);
        byte[] temp1 = Encoding.Convert(utf8, defaultChars, temp);
        string result = defaultChars.GetString(temp1);

        Response.Write(result);
        Response.Write("<br>");
    }
}