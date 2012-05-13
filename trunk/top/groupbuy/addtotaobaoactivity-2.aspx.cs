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
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class top_groupbuy_addtotaobaoactivity_2 : System.Web.UI.Page
{
    public string style = string.Empty;
    public string size = string.Empty;
    public string type = string.Empty;
    public string orderby = string.Empty;
    public string query = string.Empty;
    public string shopcat = string.Empty;
    public string name = string.Empty;
    public string items = string.Empty;
    public string id = string.Empty;
    public string url = string.Empty;
    public string ads = string.Empty;//活动ID
    public string myadstemp = string.Empty;//活动ID

    protected void Page_Load(object sender, EventArgs e)
    {
        id = utils.NewRequest("id", Common.utils.RequestType.QueryString);
        url = "addtotaobaoactivity-3.aspx?id=" + id;

        style = utils.NewRequest("style", Common.utils.RequestType.Form);
        size = utils.NewRequest("size", Common.utils.RequestType.Form);
        type = utils.NewRequest("type", Common.utils.RequestType.Form);
        orderby = utils.NewRequest("orderby", Common.utils.RequestType.Form);
        query = utils.NewRequest("query", Common.utils.RequestType.Form);
        shopcat = utils.NewRequest("shopcat", Common.utils.RequestType.Form);
        name = utils.NewRequest("name", Common.utils.RequestType.Form);
        items = utils.NewRequest("itemsStr", Common.utils.RequestType.Form);
        ads = utils.NewRequest("myads", Common.utils.RequestType.Form);
        myadstemp = utils.NewRequest("myadstemp", Common.utils.RequestType.Form);

        //过滤items中的0
        if (items.Length > 2)
        {
            items = items.Substring(1, items.Length - 1);
        }

        //BindData(items, ads);

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
            url = "addtotaobaoactivity-3.aspx?id=" + id;
        }
    }

    private void BindData(string items, string adsid)
    {
        string html = string.Empty;
        string total = string.Empty;
        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", "12287381", "d3486dac8198ef01000e7bd4504601a4");//d3486dac8198ef01000e7bd4504601a4

        if (type != "1")
        {
            ItemsOnsaleGetRequest request = new ItemsOnsaleGetRequest();
            request.Fields = "num_iid,title,price,pic_url";
            request.PageSize = 1;

            if (orderby == "new")
            {
                request.OrderBy = "list_time:desc";
            }
            else if (orderby == "sale")
            {
                request.OrderBy = "volume:desc";
            }

            if (shopcat != "0")
            {
                request.SellerCids = shopcat;
            }

            if (query != "0")
            {
                request.Q = query;
            }

            Cookie cookie = new Cookie();
            string taobaoNick = cookie.getCookie("nick");
            string session = cookie.getCookie("top_sessiongroupbuy");
            PageList<Item> product = client.ItemsOnsaleGet(request, session);


            ItemGetRequest request1 = new ItemGetRequest();
            request1.Fields = "num_iid,title,price,pic_url,desc";
            request1.NumIid = product.Content[0].NumIid;
            Item product1 = client.ItemGet(request1);
            html = product1.Desc;
        }
        else
        {
            string itemId = items.Split(',')[0];

            ItemGetRequest request = new ItemGetRequest();
            request.Fields = "num_iid,title,price,pic_url,desc";
            request.NumIid = long.Parse(itemId);

            Item product = client.ItemGet(request);
            //获取商品详细描述
            html = product.Desc;
        }

        //获取广告代码
        string adsHtml = "团购的HTML代码";// getAdsContent(ads);

        if (!Regex.IsMatch(html, @"<img alt=""tetesoft-area-start"" width=""0"" height=""0"">([\s\S]*)<img alt=""tetesoft-area-end"" width=""0"" height=""0"">"))
        {
            html += @"<img alt=""tetesoft-area-start"" width=""0"" height=""0"">" + adsHtml + @"<img alt=""tetesoft-area-end"" width=""0"" height=""0"">";
        }
        else
        {
            html = Regex.Replace(html, @"<img alt=""tetesoft-area-start"" width=""0"" height=""0"">([\s\S]*)<img alt=""tetesoft-area-end"" width=""0"" height=""0"">", @"<img alt=""tetesoft-area-start"" width=""0"" height=""0"">" + adsHtml + @"<img alt=""tetesoft-area-end"" width=""0"" height=""0"">");
        }

        lbView.InnerHtml = html;

        //Response.Write("搜索结果共计" + total + "个商品...<br>");
    }
}