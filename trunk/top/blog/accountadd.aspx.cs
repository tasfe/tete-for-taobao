using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using Common;
using System.Data;
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;
using Common;

public partial class top_blog_accountadd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");

        //过期判断
        if (string.IsNullOrEmpty(taobaoNick))
        {
            Response.Write("window.location.href='http://container.open.taobao.com/container?appkey=12159997'");
            Response.End();
            return;
        }

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);


        string level = string.Empty;
        string num = string.Empty;
        string used = string.Empty;

        string sql = "SELECT * FROM TopTaobaoShop WHERE nick = '" + taobaoNick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            string version = dt.Rows[0]["versionNo"].ToString();
            switch (version)
            {
                case "1":
                    level = "标准版客户";
                    num = "2";
                    break;
                case "2":
                    level = "专业版客户";
                    num = "10";
                    break;
                case "3":
                    level = "VIP版客户";
                    num = "999999";
                    break;
                default:
                    level = "标准版客户";
                    num = "2";
                    break;
            }
        }

        //获取今日发送的文章数
        sql = "SELECT COUNT(*) FROM TopBlogAccountNew WHERE nick = '" + taobaoNick + "'";
        dt = utils.ExecuteDataTable(sql);
        used = dt.Rows[0][0].ToString();

        if (int.Parse(used) >= int.Parse(num))
        {
            Response.Write("尊敬的" + level + " " + taobaoNick + "，非常抱歉的告诉您，您的最多只能添加" + num + "个博客帐号，如需继续添加请<a href='http://pay.taobao.com/mysub/subdeal/orderSubDeal.htm?servId=22002761' target='_blank'>购买高级会员服务</a>，谢谢！<br> <a href='javascript:history.go(-1)'>返回</a> <a href='qubie.html' target='_blank'>查看版本区别</a>");
            Response.End();
            return;
        }


        string uid = this.TextBox1.Text.Replace("'", "''");
        string pass = this.TextBox2.Text.Replace("'", "''");

        //cookie保存帐号密码

        //准备生成
        string strHtml = string.Empty;

        StreamReader sr = null; //用来读取流
        StreamWriter sw = null; //用来写文件
        Encoding code = Encoding.GetEncoding("utf-8"); //定义编码 
        ASCIIEncoding encoding = new ASCIIEncoding();
        //构造POST请求
        string postData = "service=sso&client=ssologin.js%28v1.3.9%29&entry=blog&encoding=utf-8&gateway=1&savestate=0&from=&useticket=0&username=" + uid + "&password=" + pass + "";
        byte[] data = code.GetBytes(postData);
        //构造web请求，发送请求，获取响应
        HttpWebRequest HttpWebRequest = null;
        string url = "http://login.sina.com.cn/sso/login.php?client=ssologin.js(v1.3.9)";
        HttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        HttpWebRequest.Method = "POST";
        HttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; InfoPath.2; .NET4.0C; .NET4.0E)";
        HttpWebRequest.ContentType = "application/x-www-form-urlencoded";
        HttpWebRequest.ContentLength = data.Length;

        Stream newStream = HttpWebRequest.GetRequestStream();
        newStream.Write(data, 0, data.Length);
        newStream.Close();
        //发送
        WebResponse HttpWebResponse = null;
        HttpWebResponse = HttpWebRequest.GetResponse();
        //获得流
        sr = new StreamReader(HttpWebResponse.GetResponseStream(), code);
        strHtml = sr.ReadToEnd();

        //帐号录入判断
        sql = "SELECT COUNT(*) FROM TopBlogAccountNew WHERE nick = '" + taobaoNick + "' AND uid = '" + uid + "'";
        string count = utils.ExecuteString(sql);
        if (count != "0")
        {
            Response.Write("<script>alert('该帐号已经添加过，请重新输入！');history.go(-1);</script>");
            return;
        }

        //帐号密码判断
        if (strHtml.IndexOf("location.replace(\"http://blog.sina.com.cn\");") == -1)
        {
            Response.Write("<script>alert('帐号密码错误，请重新输入！');history.go(-1);</script>");
            return;
        }

        //是否建立博客判断
        if (!CheckBlogExits(uid, pass))
        {
            Response.Write("<script>alert('您的博客尚未开通，请先登录到新浪博客开通您的博客再继续添加！');history.go(-1);</script>");
            return;
        }

        //插入数据库
        sql = "INSERT INTO TopBlogAccountNew (" +
                        "uid, " +
                        "pass, " +
                        "nick, " +
                        "count, " +
                        "typ" +
                    " ) VALUES ( " +
                        " '" + uid + "', " +
                        " '" + pass + "', " +
                        " '" + taobaoNick + "', " +
                        " '0', " +
                        " 'sina' " +
                  ") ";

        utils.ExecuteNonQuery(sql);

        //Response.Write(sql);
        string isDialog = utils.NewRequest("isdialog", utils.RequestType.QueryString);
        if (isDialog == "1")
        {
            CloseWindow();
        }
        else
        {
            Response.Redirect("accountlist.aspx");
        }
    }

    //判断博客是否存在
    private bool CheckBlogExits(string uid, string pass)
    {
        string title = "特特博客推广专家_专业淘宝博客营销/博客群发/博客推广";
        string content = "特特博客推广专家，让您的产品/店铺的链接出现在千百个热门博客的文章中，大幅提升店铺人气和下单率，完全自动智能发送，减轻了您的推广负担...支持：QQ空间，百度空间，新浪博客，网易博客，搜狐博客，淘江湖...<br><br>此文是<a href='http://www.7fshop.com/' target='_blank'>特特博客推广专家</a>用来测试您提供的账号密码是否正常。。。看到此文说明正常~ ";

        SinaBlog sina = new Common.SinaBlog(uid, pass);
        string strHtml = sina.Send(title, content);

        string address = new Regex(@"""data"":""([^""]*)""", RegexOptions.IgnoreCase).Match(strHtml).Groups[1].ToString();
        address = "http://blog.sina.com.cn/s/blog_" + address + ".html";

        if (address != "http://blog.sina.com.cn/s/blog_.html")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void CloseWindow()
    {
        string str = string.Empty;
        string sql = "SELECT TOP 1 * FROM TopBlogAccountNew ORDER BY id DESC";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str = "<input id=\"account" + dt.Rows[0]["id"].ToString() + "\" type=\"checkbox\" checked=\"checked\" name=\"account\" value=\"" + dt.Rows[0]["uid"].ToString() + "\" /> <label for=\"account" + dt.Rows[0]["id"].ToString() + "\">" + dt.Rows[0]["uid"].ToString() + "</label> - " + dt.Rows[0]["typ"].ToString() + " <br />";
        }

        StringBuilder builder = new StringBuilder();
        builder.Append("<script>");
        builder.Append("var str = '"+str+"';");
        builder.Append("if (navigator.appVersion.indexOf('MSIE') == -1) {");
        builder.Append("window.opener.returnAction(str);");
        builder.Append("window.close();");
        builder.Append("} else {");
        builder.Append("window.returnValue = str;");
        builder.Append("window.close();");
        builder.Append("}");
        builder.Append("</script>");

        Response.Write(builder.ToString());
        Response.End();
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
}