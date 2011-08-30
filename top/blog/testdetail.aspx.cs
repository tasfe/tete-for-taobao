using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;

public partial class top_blog_testdetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //string id = utils.NewRequest("id", utils.RequestType.QueryString);
        //string sql = "SELECT content FROM TopBlog WHERE id = " + id;

        //DataTable dt = utils.ExecuteDataTable(sql);
        //if (dt.Rows.Count != 0)
        //{
        //    Response.Write(HttpUtility.HtmlDecode(dt.Rows[0]["content"].ToString()));
        //    Response.Write("*********************************************<br>");
        //    Response.Write(HttpUtility.HtmlDecode(dt.Rows[0]["content"].ToString()).Length);
        //}
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string newstr = HttpUtility.HtmlDecode(TextBox1.Text);
        string str = newstr.Substring(0, 18000); //GetContentSummary(this.TextBox1.Text, 18000, false);

        Response.Write(str.Length);
    }



    /// <summary>   
    /// 按字节长度截取字符串(支持截取带HTML代码样式的字符串)   
    /// </summary>   
    /// <param name="content">将要截取的字符串参数</param>   
    /// <param name="length">截取的字节长度</param>   
    /// <param name="StripHTML">截取的结果是否为html代码</param>   
    /// <returns>截取的字符串</returns>   
    public static string GetContentSummary(string content, int length, bool StripHTML)
    {
        if (string.IsNullOrEmpty(content) || length == 0)
            return "";
        if (StripHTML)
        {
            System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex("<[^>]*>");
            content = re.Replace(content, "");
            content = content.Replace("　", "").Replace(" ", "").Replace(" ", "");
            if (content.Length <= length)
                return content;
            else
                return content.Substring(0, length);
        }
        else
        {
            if (content.Length <= length)
                return content;

            int pos = 0, npos = 0, size = 0;
            bool firststop = false, notr = false, noli = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            while (true)
            {
                if (pos >= content.Length)
                    break;
                string cur = content.Substring(pos, 1);
                if (cur == "<")
                {
                    string next = content.Substring(pos + 1, 3).ToLower();
                    if (next.IndexOf("p") == 0 && next.IndexOf("pre") != 0)
                    {
                        npos = content.IndexOf(">", pos) + 1;
                    }
                    else if (next.IndexOf("/p") == 0 && next.IndexOf("/pr") != 0)
                    {
                        npos = content.IndexOf(">", pos) + 1;
                        if (size < length)
                            sb.Append("<br />");
                    }
                    else if (next.IndexOf("br") == 0)
                    {
                        npos = content.IndexOf(">", pos) + 1;
                        if (size < length)
                            sb.Append("<br />");
                    }
                    else if (next.IndexOf("img") == 0)
                    {
                        npos = content.IndexOf(">", pos) + 1;
                        if (size < length)
                        {
                            sb.Append(content.Substring(pos, npos - pos));
                            size += npos - pos + 1;
                        }
                    }
                    else if (next.IndexOf("li") == 0 || next.IndexOf("/li") == 0)
                    {
                        npos = content.IndexOf(">", pos) + 1;
                        if (size < length)
                        {
                            sb.Append(content.Substring(pos, npos - pos));
                        }
                        else
                        {
                            if (!noli && next.IndexOf("/li") == 0)
                            {
                                sb.Append(content.Substring(pos, npos - pos));
                                noli = true;
                            }
                        }
                    }
                    else if (next.IndexOf("tr") == 0 || next.IndexOf("/tr") == 0)
                    {
                        npos = content.IndexOf(">", pos) + 1;
                        if (size < length)
                        {
                            sb.Append(content.Substring(pos, npos - pos));
                        }
                        else
                        {
                            if (!notr && next.IndexOf("/tr") == 0)
                            {
                                sb.Append(content.Substring(pos, npos - pos));
                                notr = true;
                            }
                        }
                    }
                    else if (next.IndexOf("td") == 0 || next.IndexOf("/td") == 0)
                    {
                        npos = content.IndexOf(">", pos) + 1;
                        if (size < length)
                        {
                            sb.Append(content.Substring(pos, npos - pos));
                        }
                        else
                        {
                            if (!notr)
                            {
                                sb.Append(content.Substring(pos, npos - pos));
                            }
                        }
                    }
                    else
                    {
                        npos = content.IndexOf(">", pos) + 1;
                        sb.Append(content.Substring(pos, npos - pos));
                    }
                    if (npos <= pos)
                        npos = pos + 1;
                    pos = npos;
                }
                else
                {
                    if (size < length)
                    {
                        sb.Append(cur);
                        size++;
                    }
                    else
                    {
                        if (!firststop)
                        {
                            sb.Append("...");
                            firststop = true;
                        }
                    }
                    pos++;
                }

            }
            return HttpUtility.HtmlDecode(sb.ToString());
        }
    }  
}