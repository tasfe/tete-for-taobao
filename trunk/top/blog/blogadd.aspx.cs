using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Net;
using System.Text;
using System.IO;
using Common;

public partial class top_blog_blogadd : System.Web.UI.Page
{
    public string radio = string.Empty;
    public string radio1 = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private string GetTuijianBlog()
    {
        string url = "http://blog.sina.com.cn/";
        string strHtml = getUrl(url, "gb2312");
        string radioTuijian = string.Empty;

        Regex reg = new Regex(@"<a href=""(http\:\/\/blog\.sina\.com\.cn/s/[^\.]*\.html\?tj\=1)"" target=""_blank"">([^\<]*?)</a>", RegexOptions.IgnoreCase);
        MatchCollection match = reg.Matches(strHtml);

        for (int i = 0; i < match.Count; i++)
        {
            radioTuijian += ("<input type=radio onclick='selectArticle(this)' title=\"" + match[i].Groups[2].ToString().Replace("<strong>", "").Replace("</strong>", "") + "\" name=title value=\"" + match[i].Groups[1].ToString() + "\"> " + match[i].Groups[2].ToString() + " <a href='" + match[i].Groups[1].ToString() + "' style='color:#3366CC' target='_blank'>查看原文</a><br>");

            if (i > 15)
            {
                break;
            }
        }

        return radioTuijian;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        radio = "";

        string key = tbKey.Text;
        //记录搜索关键字
        RecordSearchKey(key);

        //百度搜索 - http://hi.baidu.com/sys/search?type=1&sort=2&entry=2&word=%D0%AC%D7%D3&region=0
        //搜狐搜索 - http://blogsearch.sogou.com/blog?insite=blog.sohu.com&query=%D0%AC%D7%D3


        string url = "http://uni.sina.com.cn/c.php?k=" + HttpUtility.UrlEncode(key) + "&t=blog&ts=bpost&stype=tag&s=2";
        string strHtml = getUrl(url, "gb2312");
        //取第2页
        url = "http://uni.sina.com.cn/c.php?k=" + HttpUtility.UrlEncode(key) + "&t=blog&ts=bpost&stype=tag&s=2&page=2";

        strHtml += getUrl(url, "gb2312");

        Regex reg = new Regex(@"<span class=""title01""><a target=""_blank"" href=""([^""]*)"">([\s\S]*?)</a></span>", RegexOptions.IgnoreCase);
        MatchCollection match = reg.Matches(strHtml);

        for (int i = 0; i < match.Count; i++)
        {
            radio += ("<input type=radio onclick='selectArticle(this)' title=\"" + match[i].Groups[2].ToString().Replace("<strong>", "").Replace("</strong>", "") + "\" name=title value=\"" + match[i].Groups[1].ToString() + "\"> " + match[i].Groups[2].ToString() + " <a href='" + match[i].Groups[1].ToString() + "' style='color:#3366CC' target='_blank'>查看原文</a><br>");
        }

        //获取搜狐最新博客


        radio += "<br><input type='button' value='下一步' onclick='submitForm()' />";

        //获取今日推荐
        radio1 = GetTuijianBlog();

        //显示
        this.showArea.Visible = true;
    }

    /// <summary>
    /// 记录搜索结果
    /// </summary>
    /// <param name="key"></param>
    private void RecordSearchKey(string key)
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");

        //过期判断
        if (string.IsNullOrEmpty(taobaoNick))
        {
            Response.Write("<script>parent.location.href='http://container.open.taobao.com/container?appkey=12159997'</script>");
            Response.End();
            return;
        }

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        string sql = "SELECT COUNT(*) FROM TopBlogSearchKey WHERE nick = '" + taobaoNick + "' AND searchkey = '" + key.Replace("'", "''") + "'";
        string count = utils.ExecuteString(sql);
        if (count == "0")
        {
            //插入
            sql = "INSERT INTO TopBlogSearchKey (" +
                        "searchkey, " +
                        "nick " +
                    " ) VALUES ( " +
                        " '" + key.Replace("'", "''") + "', " +
                        " '" + taobaoNick + "' " +
                  ") ";

            utils.ExecuteNonQuery(sql);
        }
        else
        {
            //更新
            sql = "UPDATE TopBlogSearchKey SET count = count + 1 WHERE nick = '" + taobaoNick + "' AND searchkey = '" + key.Replace("'", "''") + "'";

            utils.ExecuteNonQuery(sql);
        }
    }

    private string getUrl(string url, string codeStr)
    {
        //准备生成
        string strHtml = string.Empty;
        StreamReader sr = null; //用来读取流
        Encoding code = Encoding.GetEncoding(codeStr); //定义编码 
        ASCIIEncoding encoding = new ASCIIEncoding();
        //构造web请求，发送请求，获取响应
        WebRequest HttpWebRequest = null;
        HttpWebRequest = WebRequest.Create(url);
        //HttpWebRequest.Method = "POST";
        // HttpWebRequest.ContentType = "application/x-www-form-urlencoded";
        //HttpWebRequest.ContentLength = data.Length;
        //Stream newStream = HttpWebRequest.GetRequestStream();
        //newStream.Write(data, 0, data.Length);
        //newStream.Close();
        //发送
        WebResponse HttpWebResponse = null;
        HttpWebResponse = HttpWebRequest.GetResponse();
        //获得流
        sr = new StreamReader(HttpWebResponse.GetResponseStream(), code);
        strHtml = sr.ReadToEnd();

        return strHtml;
    }
}