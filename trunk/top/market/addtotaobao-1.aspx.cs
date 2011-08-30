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
        url = "addtotaobao-2.aspx";
        id = utils.NewRequest("id", Common.utils.RequestType.QueryString);

        style = utils.NewRequest("style", Common.utils.RequestType.Form);
        size = utils.NewRequest("size", Common.utils.RequestType.Form);
        name = utils.NewRequest("ideaName", Common.utils.RequestType.Form);

        BindData();

        //判断是否为编辑状态
        id = utils.NewRequest("id", Common.utils.RequestType.QueryString);

        if (id != "" && !utils.IsInt32(id))
        {
            Response.Write("非法参数1");
            Response.End();
            return;
        }

        if (id != "" && id != "0")
        {
            //数据绑定
            string sql = "SELECT * FROM TopIdea WHERE id = " + id;
            DataTable dt = utils.ExecuteDataTable(sql);

            orderby = dt.Rows[0]["orderby"].ToString();
            type = dt.Rows[0]["showtype"].ToString();
            query = dt.Rows[0]["query"].ToString();
            shopcat = dt.Rows[0]["shopcategoryid"].ToString();
            
            //选中商品数据绑定
            if (type == "1")
            {
                sql = "SELECT * FROM TopIdeaProduct WHERE ideaid = " + id;
                dt = utils.ExecuteDataTable(sql);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        items = dt.Rows[i]["itemid"].ToString();
                        itemsStr = dt.Rows[i]["itemname"].ToString();
                    }
                    else
                    {
                        items += "," + dt.Rows[i]["itemid"].ToString();
                        itemsStr += "," + dt.Rows[i]["itemname"].ToString();
                    }
                }

                items = "," + items;
                itemsStr = "," + itemsStr;
            }

            url = "addidea-2.aspx?id=" + id;
        }

        switch (size)
        {
            case "514*160":
                num = "5";
                break;
            case "514*288":
                num = "10";
                break;
            case "312*288":
                num = "6";
                break;
            case "336*280":
                num = "4";
                break;
            case "714*160":
                num = "7";
                break;
            case "114*418":
                num = "3";
                break;
            case "664*160":
                num = "6";
                break;
            case "218*286":
                num = "4";
                break;
            default:
                num = "4";
                break;
        }
    }

    /// <summary>
    /// 数据绑定
    /// </summary>
    private void BindData()
    {
        Cookie cookie = new Cookie();
        string taobaoNick = cookie.getCookie("nick");
        string session = cookie.getCookie("top_session");

        //COOKIE过期判断
        if(taobaoNick == "")
        {
            //SESSION超期 跳转到登录页
            Response.Write("<script>parent.location.href='http://container.open.taobao.com/container?appkey=12132145'</script>");
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
        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", "12132145", "1fdd2aadd5e2ac2909db2967cbb71e7f");
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
                Response.Write("<script>parent.location.href='http://container.open.taobao.com/container?appkey=12132145'</script>");
                Response.End();
            }
            return;
        }
        rptItems.DataSource = product.Content;
        rptItems.DataBind();

        //广告数据绑定
        string sqlNew = "SELECT * FROM TopIdea WHERE nick = '" + taobaoNick + "' ORDER BY id DESC";
        DataTable dtNew = utils.ExecuteDataTable(sqlNew);

        rptAds.DataSource = dtNew;
        rptAds.DataBind();
    }
}
