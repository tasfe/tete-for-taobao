using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;

public partial class top_market_tuijianadd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string nickto = this.TextBox1.Text.Replace("'", "''");
        if (nickto.Length == 0)
        {
            Response.Write("<script>alert('淘宝昵称不能为空！');history.go(-1);</script>");
            Response.End();
            return;
        }

        string sql = "SELECT COUNT(*) FROM TopTuijian WHERE nickto = '" + nickto + "'";
        string count = utils.ExecuteString(sql);
        if (count != "0")
        {
            Response.Write("<script>alert('该淘宝会员已经被别人推荐过，无法重复推荐！');history.go(-1);</script>");
            Response.End();
            return;
        }

        Cookie cookie = new Cookie();
        string taobaoNick = cookie.getCookie("nick");
        string session = cookie.getCookie("top_session");

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        //插入数据库
        sql = "INSERT INTO TopTuijian (" +
                        "nickfrom, " +
                        "nickto " +
                    " ) VALUES ( " +
                        " '" + taobaoNick + "', " +
                        " '" + nickto + "' " +
                  ") ";

        utils.ExecuteNonQuery(sql);

        Response.Write(@"<div style=""border:solid 1px #CCE2FF; padding:4px; background-color:#E8F2FF; margin:0 3px; color:Red; font-weight:bold"">添加成功，您需要把下面的网址发给您的好友，让您的好友购买该免费服务！他购买成功进入该应用便会成为您的推荐用户！<br><br> http://seller.taobao.com/fuwu/service.htm?service_id=764 <br><br> <a href='tuijianlist.aspx'>返回列表</a></div>");
        Response.End();
    }
}