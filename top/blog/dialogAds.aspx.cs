using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Common;

public partial class top_blog_dialogAds : System.Web.UI.Page
{
    public string nickid = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        //taobaoNick = utils.NewRequest("nick", utils.RequestType.QueryString);
        string session = cookie.getCookie("top_sessionblog");

        ////COOKIE过期判断
        if (taobaoNick == "")
        {
            //SESSION超期 跳转到登录页
            Response.Write("<script>parent.location.href='http://container.open.taobao.com/container?appkey=12159997'</script>");
            Response.End();
        }

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        //数据绑定
        string sqlNew = "SELECT TOP 10 * FROM TopIdea WHERE nick = '" + taobaoNick + "' ORDER BY id DESC";
        DataTable dtNew = utils.ExecuteDataTable(sqlNew);

        rptAds.DataSource = dtNew;
        rptAds.DataBind();

        //内容头部增加特特广告图片
        string sql = "SELECT sid FROM TopTaobaoShop WHERE nick = '" + taobaoNick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            nickid = "http://shop" + dt.Rows[0]["sid"].ToString() + ".taobao.com/";
        }
        else
        {
            nickid = "http://www.taobao.com/";
        }
    }
}