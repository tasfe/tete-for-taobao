using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;

public partial class top_blog_dialogProduct : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Cookie cookie = new Cookie();
        string taobaoNick = cookie.getCookie("nick");
        string session = cookie.getCookie("top_sessiongroupbuy");

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);
        //获取用户店铺商品列表
        //TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", "12159997", "614e40bfdb96e9063031d1a9e56fbed5");
        //ItemsOnsaleGetRequest request = new ItemsOnsaleGetRequest();
        //request.Fields = "num_iid,title,price,pic_url";
        //request.PageSize = 10;

        //PageList<Item> product = new PageList<Item>();

        //try
        //{
        //    product = client.ItemsOnsaleGet(request, session);
        //}
        //catch (Exception ex)
        //{
        //    if (ex.Message == "27:Invalid session:Session not exist")
        //    {
        //        //SESSION超期 跳转到登录页
        //        Response.Write("<script>parent.location.href='http://container.open.taobao.com/container?appkey=12159997'</script>");
        //        Response.End();
        //    }
        //    return;
        //}
        //rptItems.DataSource = product.Content;
        //rptItems.DataBind();
    }
}