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

public partial class top_blog_linkaddnew : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");

        //过期判断
        if (string.IsNullOrEmpty(taobaoNick))
        {
            Response.Write("<script>window.location.href='http://container.open.taobao.com/container?appkey=12159997'</script>");
            Response.End();
            return;
        }

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        string uid = this.TextBox1.Text.Replace("'", "''");
        string strHtml = string.Empty;

        //帐号录入判断
        string sql = "SELECT COUNT(*) FROM TopBlogSearchKey WHERE nick = '" + taobaoNick + "' AND searchkey = '" + uid + "'";
        string count = utils.ExecuteString(sql);
        if (count != "0")
        {
            Response.Write("<script>alert('该关键字已经添加过，请重新输入！');history.go(-1);</script>");
            return;
        }

        //插入数据库
        sql = "INSERT INTO TopBlogSearchKey (" +
                        "searchkey, " +
                        "nick " +
                    " ) VALUES ( " +
                        " '" + uid + "', " +
                        " '" + taobaoNick + "' " +
                  ") ";

        utils.ExecuteNonQuery(sql);

        //Response.Write(sql);
        Response.Redirect("linklistnew.aspx");
    }
}