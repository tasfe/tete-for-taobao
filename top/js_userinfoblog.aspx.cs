using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Common;

public partial class top_js_userinfoblog : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Cookie cookie = new Cookie();
        string taobaoNick = cookie.getCookie("nick");

        //过期判断
        if(string.IsNullOrEmpty(taobaoNick))
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
            string version = dt.Rows[0]["versionNoblog"].ToString();
            switch (version)
            {
                case "1":
                    level = "标准版客户";
                    num = "10";
                    break;
                case "2":
                    level = "专业版客户";
                    num = "30";
                    break;
                case "3":
                    level = "VIP版客户";
                    num = "--";
                    break;
                default:
                    level = "标准版客户";
                    num = "10";
                    break;
            }
        }

        //获取今日发送的文章数
        sql = "SELECT COUNT(*) FROM TopBlog WHERE nick = '" + taobaoNick + "' AND DATEDIFF(d,GETDATE(),ADDDATE) = 0 AND sendStatus <> 2";
        dt = utils.ExecuteDataTable(sql);
        used = dt.Rows[0][0].ToString();

        //版本功能描述
        string show = string.Empty;
        if (num == "--")
        {
            show = "您是" + level + "，您每天可以发送的文章数没有限制，您今天已经发送了" + used + "篇文章~";
        }
        else
        {
            show = "您是" + level + "，您每天最多可以发送" + num + "条文章，您今天已经发送了" + used + "篇文章~";
        }

        Response.Write("document.write('您好，" + taobaoNick + "！(<b>" + level + "</b>) <a href=logout.aspx>退出</a> " + show + " '); ");
    }
}
