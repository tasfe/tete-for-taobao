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

public partial class top_blog_linkadd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Common.Cookie cookie = new Common.Cookie();
            string taobaoNick = cookie.getCookie("nick");
            string nickid = string.Empty;

            //过期判断
            if (string.IsNullOrEmpty(taobaoNick))
            {
                Response.Write("<script>window.location.href='http://container.open.taobao.com/container?appkey=12159997'</script>");
                Response.End();
                return;
            }

            Rijndael_ encode = new Rijndael_("tetesoft");
            taobaoNick = encode.Decrypt(taobaoNick);

            string sqlNew = "SELECT sid FROM TopTaobaoShop WHERE nick = '" + taobaoNick + "'";
            DataTable dtNew = utils.ExecuteDataTable(sqlNew);
            if (dtNew.Rows.Count != 0)
            {
                nickid = "http://shop" + dtNew.Rows[0]["sid"].ToString() + ".taobao.com/";
            }
            else
            {
                nickid = "http://www.taobao.com/";
            }

            this.TextBox2.Text = nickid;
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

        string level = string.Empty;
        string num = string.Empty;
        string used = string.Empty;

        string sql = "SELECT * FROM TopTaobaoShop WHERE nick = '" + taobaoNick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            string version = dt.Rows[0]["versionNoBlog"].ToString();
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
        sql = "SELECT COUNT(*) FROM TopBlogLink WHERE nick = '" + taobaoNick + "'";
        dt = utils.ExecuteDataTable(sql);
        used = dt.Rows[0][0].ToString();

        if (int.Parse(used) >= int.Parse(num))
        {
            Response.Write("尊敬的" + level + " " + taobaoNick + "，非常抱歉的告诉您，您的帐号最多只能添加" + num + "个关键字，如需继续添加请<a href='http://pay.taobao.com/mysub/subarticle/upgrade_order_sub_article.htm?market_type=6&article_id=181'  target='_blank'>购买高级会员服务</a>，谢谢！<br> <a href='javascript:history.go(-1)'>返回</a> <a href='qubie.html' target='_blank'>查看版本区别</a>");
            Response.End();
            return;
        }

        string uid = this.TextBox1.Text.Replace("'", "''");
        string pass = this.TextBox2.Text.Replace("'", "''");
        string strHtml = string.Empty;

        if (uid.Length == 0 || pass.Length == 0)
        {
            Response.Write("<script>alert('关键字或者链接不能为空！');history.go(-1);</script>");
            return;
        }

        //帐号录入判断
        sql = "SELECT COUNT(*) FROM TopBlogLink WHERE nick = '" + taobaoNick + "' AND keyword = '" + uid + "'";
        string count = utils.ExecuteString(sql);
        if (count != "0")
        {
            Response.Write("<script>alert('该关键字已经添加过，请重新输入！');history.go(-1);</script>");
            return;
        }

        //插入数据库
        sql = "INSERT INTO TopBlogLink (" +
                        "keyword, " +
                        "link, " +
                        "nick " +
                    " ) VALUES ( " +
                        " '" + uid + "', " +
                        " '" + pass + "', " +
                        " '" + taobaoNick + "' " +
                  ") ";

        utils.ExecuteNonQuery(sql);

        //Response.Write(sql);
        Response.Redirect("linklist.aspx");
    }
}