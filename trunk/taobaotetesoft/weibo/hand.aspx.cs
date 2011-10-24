using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using Common;
using QWeiboSDK;
using System.Text;
using System.Data;

public partial class weibo_hand : System.Web.UI.Page
{
    private string appKey = string.Empty;
    private string appSecret = string.Empty;
    private string tokenKey = string.Empty;
    private string tokenSecret = string.Empty;
    private string uid = string.Empty;
    public string str = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string sql = string.Empty;
        appKey = "d3225497956249cbb13a7cb7375d62bd";
        appSecret = "6cf7a3274cb676328e77dff3e203061d";

        Common.Cookie cookie = new Common.Cookie();
        tokenKey = cookie.getCookie("tokenKey");
        tokenSecret = cookie.getCookie("tokenSecret");
        uid = cookie.getCookie("uid");

        string act = utils.NewRequest("act", utils.RequestType.QueryString);
        string listento = utils.NewRequest("listento", utils.RequestType.QueryString);
        if (act == "listen")
        {
            //每小时最多一键收听一次
            sql = "SELECT COUNT(*) FROM TopMicroBlogNumLog WHERE typ = 'add' AND uid = '" + uid + "' AND  DATEDIFF(s, adddate, GETDATE() ) < 3600";
            string count = utils.ExecuteString(sql);
            if (int.Parse(count) < 10)
            {
                listen(listento);

                //记录操作日志
                sql = "INSERT INTO TopMicroBlogNumLog (uid, typ, num, bak) VALUES ('" + uid + "', 'add', 1, '" + listento + "')";
                utils.ExecuteNonQuery(sql);

                //减少积分
                sql = "UPDATE TopMicroBlogAccount SET score = score + 1 WHERE uid = '" + uid + "'";
                utils.ExecuteNonQuery(sql);

                //如果这个人积分为0则给他发送私信提醒他回来赚积分
                sql = "SELECT score FROM TopMicroBlogAccount WHERE uid = '" + uid + "'";

                Response.Redirect("hand.aspx");
                return;
            }
            else
            {
                Response.Write("<script>alert('每小时最多收听10个别人的微博，请您过一会再来：）');history.go(-1);</script>");
                Response.End();
                return;
            }
        }
           
        //每小时最多一键收听一次
        sql = "SELECT TOP 16 uid FROM TopMicroBlogAccount WHERE typ = 'qq' AND score > 0 AND uid <> '' AND uid NOT IN (SELECT listen FROM TopMicroBlogListen WHERE uid = '" + uid + "') ORDER BY NEWID()";

        DataTable dt = utils.ExecuteDataTable(sql);
        rptArticle.DataSource = dt;
        rptArticle.DataBind();


        lbPage.Text = InitPageStr(200, "hand.aspx");
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

    private string InitPageStr(int total, string url)
    {
        //分页数据初始化
        string str = string.Empty;
        int pageCount = 18;
        int pageSize = 0;
        int pageNow = 1;
        string page = utils.NewRequest("page", utils.RequestType.QueryString);
        if (page == "")
        {
            pageNow = 1;
        }
        else
        {
            pageNow = int.Parse(page);
        }

        //取总分页数
        if (total % pageCount == 0)
        {
            pageSize = total / pageCount;
        }
        else
        {
            pageSize = total / pageCount + 1;
        }

        //如果总页面大于20，则最大页面差不超过20
        int start = 1;
        int end = 20;

        if (pageSize < end)
        {
            end = pageSize;
        }
        else
        {
            if (pageNow > 15)
            {
                start = pageNow - 10;

                if (pageNow < (total - 10))
                {
                    end = pageNow + 10;
                }
                else
                {
                    end = total;
                }
            }
        }

        for (int i = start; i <= end; i++)
        {
            if (i.ToString() == pageNow.ToString())
            {
                str += i.ToString() + " ";
            }
            else
            {
                str += "<a href='" + url + "?page=" + i.ToString() + "'>[" + i.ToString() + "]</a> ";
            }
        }

        return str;
    }
}