using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Common;

public partial class top_blog_bloglist : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        /*
         SELECT TOP 20 * FROM
(
SELECT *,ROW_NUMBER() OVER (ORDER BY id DESC) AS rownumber 
FROM TopBlog WHERE nick = '叶儿随清风'
) AS a
WHERE a.rownumber > 20 
ORDER BY id DESC
         */

        //string sqlNew = "SELECT * FROM TopBlog WHERE nick = '" + taobaoNick + "' AND uid IN (SELECT uid FROM TopBlogAccountNew WHERE nick = '" + taobaoNick + "') ORDER BY id DESC"; 
        string page = utils.NewRequest("page", utils.RequestType.QueryString);
        int pageNow = 1;
        if (page == "")
        {
            pageNow = 1;
        }
        else
        {
            pageNow = int.Parse(page);
        }
        int pageCount = 20;
        int dataCount = (pageNow - 1) * pageCount;
        //string sqlNew = "SELECT TOP " + pageCount.ToString() + " * FROM (SELECT b.id,blogtitle,b.typ,b.uid,b.adddate,莉莉美妆店sendStatus,blogurl,err,isauto,ROW_NUMBER() OVER (ORDER BY b.id DESC) AS rownumber FROM TopBlog b INNER JOIN TopBlogAccountNew n ON n.uid = b.uid AND n.typ = b.typ AND n.nick = b.nick INNER JOIN TopTaobaoShop s ON s.nick = b.nick WHERE b.nick = '" + taobaoNick + "' AND b.sendstatus <> 2) AS a WHERE a.rownumber > " + dataCount.ToString() + " ORDER BY id DESC";

        string sqlNew = "SELECT TOP " + pageCount.ToString() + " * FROM (SELECT b.id,blogtitle,b.typ,b.uid,b.adddate,sendStatus,blogurl,err,isauto,ROW_NUMBER() OVER (ORDER BY b.id DESC) AS rownumber FROM TopBlog b WHERE b.nick = '" + taobaoNick + "' AND b.sendstatus <> 2) AS a WHERE a.rownumber > " + dataCount.ToString() + " ORDER BY id DESC";

        DataTable dtNew = utils.ExecuteDataTable(sqlNew);

        //Response.Write("<!--" + sqlNew + "-->");

        rptArticle.DataSource = dtNew;
        rptArticle.DataBind();

        //分页数据初始化
        sqlNew = "SELECT COUNT(*) FROM TopBlog WHERE nick = '" + taobaoNick + "' AND sendstatus <> 2";
        int totalCount = int.Parse(utils.ExecuteString(sqlNew));

        lbPage.Text = InitPageStr(totalCount, "bloglist.aspx");
    }

    private string InitPageStr(int total, string url)
    {        
        //分页数据初始化
        string str = string.Empty;
        int pageCount = 20;
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

        for (int i = 1; i <= pageSize; i++)
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

    public static string show(string msg, string url, string err, string id, string typ, string isauto)
    {
        if (msg == "1")
        {
            return "<font color='green'><b>发送完毕</b></font> <a href='" + url + "' style='color:black' target='_blank'>查看</a>";
        }
        else if (msg == "2")
        {
            return "<font color='red'><b>发送失败</b><font color='#ccc'>【" + err + "】</font></font>";
        }
        else
        {
            return "<font color='black'>发送中 <script src='listsearch.aspx?id=" + id + "&typ=" + typ + "&isauto=" + isauto + "'></script></font>";
        }
    }

    public static string showtype(string msg)
    {
        if (msg == "1")
        {
            return "自动";
        }
        else
        {
            return "<font color=blue>手动</font>";
        }
    }

    public static string left(string str, int count)
    {
        if (str.Length > count)
        {
            return str.Substring(0, count) + "...";
        }
        else
        {
            return str;
        }
    }

    public static string getblog(string typ)
    {
        if (typ == "baidu")
        {
            return "百度空间";
        }
        else if (typ == "qq")
        {
            return "QQ空间";
        }
        else if (typ == "sohu")
        {
            return "搜狐博客";
        }

        return "";
    }
}