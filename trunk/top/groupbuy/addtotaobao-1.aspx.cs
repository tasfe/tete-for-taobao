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
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;

public partial class top_addtotaobao_1 : System.Web.UI.Page
{
    public string style = string.Empty;
    public string size = string.Empty;
    public string name = string.Empty;
    public string url = string.Empty;
    public string query = string.Empty;
    public string type = string.Empty;
    public string orderby = string.Empty;
    public string shopcat = string.Empty;
    public string id = string.Empty;
    public string items = string.Empty;
    public string itemsStr = string.Empty;
    public string num = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");

        //过期判断
        if (string.IsNullOrEmpty(taobaoNick))
        {
            string msg = "尊敬的淘宝卖家，非常抱歉的告诉您，您还没有购买此服务或者登录信息已失效，如需继续使用请<a href='http://fuwu.taobao.com/serv/detail.htm?service_id=11807' target='_blank'>进入该服务</a>，谢谢！";
            Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
            Response.End();
            return;
        }

        id = utils.NewRequest("id", Common.utils.RequestType.QueryString);
        url = "addtotaobao-2.aspx";

        style = utils.NewRequest("style", Common.utils.RequestType.Form);
        size = utils.NewRequest("size", Common.utils.RequestType.Form);
        name = utils.NewRequest("ideaName", Common.utils.RequestType.Form);

        //判断是否为编辑状态
        id = utils.NewRequest("id", Common.utils.RequestType.QueryString);

        if (id != "" && !utils.IsInt32(id))
        {
            Response.Write("非法参数1");
            Response.End();
            return;
        }

        BindData();
    }

    /// <summary>
    /// 数据绑定
    /// </summary>
    private void BindData()
    {
        Cookie cookie = new Cookie();
        string taobaoNick = cookie.getCookie("nick");
        string session = cookie.getCookie("top_sessiongroupbuy");

        //COOKIE过期判断
        if(taobaoNick == "")
        {
            //SESSION超期 跳转到登录页
            Response.Write("<script>parent.location.href='http://container.open.taobao.com/container?appkey=12287381'</script>");//12287381
            Response.End();
        }

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        string sql = "SELECT * FROM TopTaobaoShopCat WHERE nick = '" + taobaoNick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);

        rptShopCat.DataSource = dt;
        rptShopCat.DataBind();

        Repeater1.DataSource = dt;
        Repeater1.DataBind();

        //获取用户店铺商品列表
        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", "12287381", "d3486dac8198ef01000e7bd4504601a4");
        ItemsOnsaleGetRequest request = new ItemsOnsaleGetRequest();
        request.Fields = "num_iid,title,price,pic_url";
        request.PageSize = 10;

        PageList<Item> product = new PageList<Item>();

        try
        {
            product = client.ItemsOnsaleGet(request, session);
        }
        catch(Exception e)
        {
            if (e.Message == "27:Invalid session:Session not exist")
            { 
                //SESSION超期 跳转到登录页
                Response.Write("<script>parent.location.href='http://container.open.taobao.com/container?appkey=12287381'</script>");
                Response.End();
            }
            return;
        }
        rptItems.DataSource = product.Content;
        rptItems.DataBind();

        //团购数据绑定
        string sqlNew = "SELECT * FROM TopGroupBuy WHERE nick = '" + taobaoNick + "' AND isdelete = 0 ORDER BY id DESC";
        DataTable dtNew = utils.ExecuteDataTable(sqlNew);

        rptAds.DataSource = dtNew;
        rptAds.DataBind();
    }
}
