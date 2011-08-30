using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;

public partial class top_blog_listsearch : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string id = utils.NewRequest("id", utils.RequestType.QueryString);
        string typ = utils.NewRequest("typ", utils.RequestType.QueryString);
        string isauto = utils.NewRequest("isauto", utils.RequestType.QueryString);

        if (id == "")
        {
            id = "0";
        }

        //注入判断
        if (utils.IsInt32(id))
        {
            string sql = "SELECT COUNT(*) FROM TopBlog b INNER JOIN TopBlogAccountNew n ON n.uid = b.uid AND n.typ = b.typ AND n.nick = b.nick INNER JOIN TopTaobaoShop s ON s.nick = b.nick WHERE sendStatus = 0 AND isauto = " + isauto + " AND b.typ = '" + typ + "' AND b.id < " + id;

            string num = utils.ExecuteString(sql);

            Response.Write("document.write('(队列中<b>" + num + "</b>位)');");
        }
    }
}