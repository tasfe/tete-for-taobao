using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Net;
using System.Text;
using System.IO;

public partial class top_microblog_microblogadd : System.Web.UI.Page
{
    public string radio = string.Empty;
    public string radio2 = string.Empty;
    public string radio3 = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        InitData();
    }

    /// <summary>
    /// 加载今日焦点话题
    /// </summary>
    private void InitData()
    {
        string str = getUrl("http://t.qq.com/p/topic", "utf-8");
        Regex regex = new Regex(@"<li><a href=""http://t.qq.com/k/([^""]*)"">[\s\S]*?</a></li>", RegexOptions.IgnoreCase);
        MatchCollection match = regex.Matches(str);

        radio3 = "<table>";
        for (int i = 0; i < match.Count; i++)
        {
            radio3 += ("<td> <input type=radio onclick='selectTopic(this)' title=\"" + match[i].Groups[1].ToString().Replace("<strong>", "").Replace("</strong>", "") + "\" name=title value=\"" + match[i].Groups[1].ToString() + "\"> " + HttpUtility.UrlDecode(HttpUtility.UrlDecode(match[i].Groups[1].ToString())) + " <a href='http://t.qq.com/k/" + match[i].Groups[1].ToString() + "' style='color:#3366CC' target='_blank'>查看话题</a> </td>");

            if ((i + 1) % 4 == 0)
            {
                radio3 += "</tr><tr>";
            }
        }
        radio3 += "</table>";

        //加载热门话题
        regex = new Regex(@"<div class=""dot""></div><a href=""([^""]*)"" title=""[^""]*"">([\s\S]*?)<em>\([0-9]*\)</em></a>", RegexOptions.IgnoreCase);
        match = regex.Matches(str);

        radio2 = "<table>";
        for (int i = 0; i < match.Count; i++)
        {
            radio2 += ("<td> <input type=radio onclick='selectTopic(this)' title=\"" + match[i].Groups[2].ToString().Replace("<strong>", "").Replace("</strong>", "") + "\" name=title value=\"" + match[i].Groups[2].ToString() + "\"> " + HttpUtility.UrlDecode(HttpUtility.UrlDecode(match[i].Groups[2].ToString())) + " <a href='http://t.qq.com/k/" + match[i].Groups[2].ToString() + "' style='color:#3366CC' target='_blank'>查看话题</a> </td>");

            if ((i + 1) % 4 == 0)
            {
                radio2 += "</tr><tr>";
            }
        }
        radio2 += "</table>";
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {

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
        //发送
        WebResponse HttpWebResponse = null;
        HttpWebResponse = HttpWebRequest.GetResponse();
        //获得流
        sr = new StreamReader(HttpWebResponse.GetResponseStream(), code);
        strHtml = sr.ReadToEnd();

        return strHtml;
    }
}