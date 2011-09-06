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

public partial class top_addtotaobao_2 : System.Web.UI.Page
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
    public string ads = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        url = "addtotaobao-3.aspx";
        id = utils.NewRequest("id", Common.utils.RequestType.QueryString);

        style = utils.NewRequest("style", Common.utils.RequestType.Form);
        size = utils.NewRequest("size", Common.utils.RequestType.Form);
        type = utils.NewRequest("type", Common.utils.RequestType.Form);
        orderby = utils.NewRequest("orderby", Common.utils.RequestType.Form);
        query = utils.NewRequest("query", Common.utils.RequestType.Form);
        shopcat = utils.NewRequest("shopcat", Common.utils.RequestType.Form);
        name = utils.NewRequest("name", Common.utils.RequestType.Form);
        items = utils.NewRequest("itemsStr", Common.utils.RequestType.Form);
        ads = utils.NewRequest("myads", Common.utils.RequestType.Form);

        //过滤items中的0
        if (items.Length > 2) 
        {
            items = items.Substring(1, items.Length - 1);
        }

        BindData(items, ads);

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
            url = "addtotaobao-3.aspx?id=" + id;
        }
    }

    private void BindData(string items, string adsid)
    {
        string html = string.Empty;
        string total = string.Empty;
        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", "12132145", "1fdd2aadd5e2ac2909db2967cbb71e7f");

        if (type == "0")
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
            string session = cookie.getCookie("top_session");
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
        string adsHtml = getAdsContent(ads);

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

    private string getAdsContent(string id)
    {
        Cookie cookie = new Cookie();
        string taobaoNick = cookie.getCookie("nick");
        string session = cookie.getCookie("top_session");

        //COOKIE过期判断
        if (taobaoNick == "")
        {
            //SESSION超期 跳转到登录页
            Response.Write("<script>parent.location.href='http://container.open.taobao.com/container?appkey=12132145'</script>");
            Response.End();
        }

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        string sql = "SELECT TOP 8 * FROM TopIdeaProduct WHERE ideaid = " + id + "";// ORDER BY NEWID()";
        DataTable dt = utils.ExecuteDataTable(sql);

        string str = string.Empty;

        string sqlNew = "SELECT sid FROM TopTaobaoShop WHERE nick = '" + taobaoNick + "'";
        DataTable dtNew = utils.ExecuteDataTable(sqlNew);
        string nickid = string.Empty;
        if (dtNew.Rows.Count != 0)
        {
            nickid = "http://shop" + dtNew.Rows[0]["sid"].ToString() + ".taobao.com/";
        }
        else
        {
            nickid = "http://www.taobao.com/";
        }

        str += "<table background=\"http://www.7fshop.com/top/show1/4.gif\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" height=\"30\" style=\"border-right: #999999 1px solid; border-top: #999999 1px solid;border-left: #999999 1px solid; border-bottom: #999999 1px solid\" width=\"734\"><tr><td>    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"><tr><td align=\"left\"><table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" height=\"30\"><tr><td width=\"10\"></td><td background=\"http://www.7fshop.com/top/show1/1.gif\" width=\"24\"></td><td background=\"http://www.7fshop.com/top/show1/2.gif\"><font color=\"white\" style=\"font-size: 13px\"><strong>掌柜推荐商品</strong></font></td><td><img src=\"http://www.7fshop.com/top/show1/3.gif\" /></td></tr></table></td><td align=\"right\"></td></tr></table></td></tr></table>  <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-right: #999999 1px solid;border-top: #999999 1px solid; overflow: hidden; border-left: #999999 1px solid;border-bottom: #999999 1px solid\" width=\"730\"><tr><td valign=\"top\"><TABLE cellSpacing=0 cellPadding=0 width=730 border=0><TBODY><TR><TD align=\"middle\"><TABLE cellSpacing=8 cellPadding=0 align=center border=0><TBODY><TR>";
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str += "<TD vAlign=top align=\"middle\" width=175 bgColor=white><TABLE cellSpacing=0 cellPadding=0 align=center border=0><TBODY><TR><TD vAlign=top align=\"middle\" width=175 bgColor=white><TABLE cellSpacing=0 cellPadding=0 align=center border=0><TBODY><TR><TD align=\"middle\"><DIV style=\"BORDER-RIGHT: #cccccc 1px solid; BORDER-TOP: #cccccc 1px solid; MARGIN-TOP: 4px; BORDER-LEFT: #cccccc 1px solid; WIDTH: 160px; BORDER-BOTTOM: #cccccc 1px solid; HEIGHT: 160px\"><DIV style=\"OVERFLOW: hidden; WIDTH: 160px; HEIGHT: 160px\"><A href=\"http://item.taobao.com/item.htm?id=" + dt.Rows[i]["itemid"].ToString() + "\" target=\"_blank\"><IMG src=\"" + dt.Rows[i]["itempicurl"].ToString() + "_160x160.jpg\" border=0 /></A></DIV></DIV></TD></TR><TR><TD align=\"middle\"><DIV style=\"PADDING-RIGHT: 4px; PADDING-LEFT: 4px; FONT-SIZE: 12px; PADDING-BOTTOM: 4px; PADDING-TOP: 4px\"><A style=\"FONT-SIZE: 12px; COLOR: #3f3f3f; TEXT-DECORATION: none\" href=\"http://item.taobao.com/item.htm?id=" + dt.Rows[i]["itemid"].ToString() + "\" target=\"_blank\">" + dt.Rows[i]["itemname"].ToString() + "</A><BR /><FONT style=\"COLOR: #fe596a\"><B>￥&nbsp;" + dt.Rows[i]["itemprice"].ToString() + "元</B></FONT> </DIV><A href=\"http://item.taobao.com/item.htm?id=" + dt.Rows[i]["itemid"].ToString() + "\" target=\"_blank\"><IMG src=\"http://www.7fshop.com/top/show1/buy1.gif\" border=0 /></A> <DIV></DIV></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE></TD>";
            if ((i + 1) % 4 == 0)
            {
                str += "</tr><tr>";
            }
        }

        str += "</TR></TBODY></TABLE></td></tr><tr><td align=\"right\" height=\"24\" style=\"border-bottom: #999999 1px solid\" valign=\"center\"><a href=\"" + nickid + "\" style=\"text-decoration: none\" target=\"_blank\"><font style=\"font-size: 13px; color: #ff6600\"><strong>更多详情请见 " + nickid + "</strong>&nbsp;</font></a></td></tr></table></td></tr></table>";

        return str;
    }
}
