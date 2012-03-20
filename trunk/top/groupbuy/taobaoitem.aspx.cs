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

public partial class top_blog_taobaoitem : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Cookie cookie = new Cookie();
        string taobaoNick = cookie.getCookie("nick");
        string session = cookie.getCookie("top_sessiongroupbuy");
        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        //获取参数
        string q = utils.NewRequest("query", utils.RequestType.QueryString);
        string page = utils.NewRequest("p", utils.RequestType.QueryString);
        if (page == "")
        {
            page = "1";
        }
        string catid = utils.NewRequest("catid", utils.RequestType.QueryString);
        string isradio = utils.NewRequest("isradio", utils.RequestType.QueryString);
        string act = utils.NewRequest("act", utils.RequestType.QueryString);
        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", "12287381", "d3486dac8198ef01000e7bd4504601a4");

        int pageSizeNow = 15;

        if (act == "get")
        {
            //获取用户店铺商品列表
            ItemsOnsaleGetRequest request = new ItemsOnsaleGetRequest();
            request.Fields = "num_iid,title,price,pic_url";
            request.PageSize = pageSizeNow;
            request.PageNo = int.Parse(page);
            request.Q = q;
            if (catid != "0")
            {
                request.SellerCids = catid;
            }

            PageList<Item> product = client.ItemsOnsaleGet(request, session);

            //输出页面HTML
            for (int i = 0; i < product.Content.Count; i++)
            {
                if (isradio == "1")
                {
                    Response.Write("<input type='checkbox' name='items' id='item_" + product.Content[i].NumIid + "' title='" + product.Content[i].Title.Replace("%", "") + "' value='" + product.Content[i].NumIid + "' onclick=\"SetInitArea(this)\"><label onMouseOver=\"javascript:ddrivetip('<img src=" + product.Content[i].PicUrl + "_80x80.jpg>','#ffffff',100)\" onMouseOut=\"hideddrivetip()\" for='item_" + product.Content[i].NumIid + "'>" + product.Content[i].Title.Replace("%", "") + "</label><br>");
                }
                else
                {
                    Response.Write("<input type='checkbox' name='items' id='item_" + product.Content[i].NumIid + "' title='" + product.Content[i].Title.Replace("%", "") + "' value='" + product.Content[i].NumIid + "' onclick=\"SetInitArea(this)\"><label onMouseOver=\"javascript:ddrivetip('<img src=" + product.Content[i].PicUrl + "_80x80.jpg>','#ffffff',100)\" onMouseOut=\"hideddrivetip()\" for='item_" + product.Content[i].NumIid + "'>" + product.Content[i].Title.Replace("%", "") + "</label><br>");
                }
            }
            Response.Write("<br>");
            long totalPage = (product.TotalResults % pageSizeNow == 0) ? (product.TotalResults / pageSizeNow) : (product.TotalResults / pageSizeNow + 1);
            //输出分页HTML
            for (int i = 1; i <= totalPage; i++)
            {
                if (page == i.ToString())
                {
                    Response.Write(i.ToString() + " ");
                }
                else
                {
                    Response.Write("<a href=\"javascript:spreadStat(" + i.ToString() + ")\">[" + i.ToString() + "]</a> ");
                }
            }
        }
        else if (act == "getCat")
        {
            //记录店铺分类信息
            SellercatsListGetRequest request1 = new SellercatsListGetRequest();
            request1.Fields = "cid,parent_cid,name,is_parent";
            request1.Nick = taobaoNick;
            PageList<SellerCat> cat = client.SellercatsListGet(request1);

            //清除老分类
            string sql = "DELETE FROM TopTaobaoShopCat WHERE nick = '" + taobaoNick + "'";
            utils.ExecuteNonQuery(sql);

            //插入新分类
            for (int i = 0; i < cat.Content.Count; i++)
            {
                sql = "INSERT INTO TopTaobaoShopCat (" +
                                "cid, " +
                                "parent_cid, " +
                                "name, " +
                                "pic_url, " +
                                "sort_order, " +
                                "created, " +
                                "nick, " +
                                "modified " +
                            " ) VALUES ( " +
                                " '" + cat.Content[i].Cid + "', " +
                                " '" + cat.Content[i].ParentCid + "', " +
                                " '" + cat.Content[i].Name + "', " +
                                " '" + cat.Content[i].PicUrl + "', " +
                                " '" + cat.Content[i].SortOrder + "', " +
                                " '" + cat.Content[i].Created + "', " +
                                " '" + taobaoNick + "', " +
                                " '" + cat.Content[i].Modified + "' " +
                          ") ";
                utils.ExecuteNonQuery(sql);
            }

            //输出页面HTML
            Response.Write("<select id=\"shopcat\" name=\"shopcat\"><option value=\"0\"></option>");
            for (int i = 0; i < cat.Content.Count; i++)
            {
                Response.Write("<option value='" + cat.Content[i].Cid + "'>" + cat.Content[i].Name + "</option>");
            }
            Response.Write("</select>");
        }
        else if (act == "getResultStr")
        {
            string items = utils.NewRequest("ids", utils.RequestType.QueryString);
            string[] arr = items.Split(',');
            List<Item> itemList = new List<Item>();

            //标志
            int flag = 1;
            if (arr.Length == 1)
            {
                flag = 0;
            }

            for (int i = flag; i < arr.Length; i++)
            {
                ItemGetRequest request = new ItemGetRequest();
                request.Fields = "num_iid,title,price,pic_url";
                request.NumIid = long.Parse(arr[i]);

                Item product = client.ItemGet(request);
                itemList.Add(product);
            }

            string str = string.Empty;
            //根据不同样式显示不同的字符串
            string style = utils.NewRequest("style", utils.RequestType.QueryString);

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


            if (isradio == "1")
            {
                //团购需要的商品数据
                for (int i = 0; i < itemList.Count; i++)
                {
                    str += "<div id=\"divPro" + itemList[i].NumIid.ToString() + "\"><A href=\"http://item.taobao.com/item.htm?id=" + itemList[i].NumIid.ToString() + "\" target=\"_blank\"><IMG src=\"" + itemList[i].PicUrl + "_160x160.jpg\" border=0 /></A><br>" + itemList[i].Title + "<br>" + itemList[i].Price + "<input type=\"hidden\" id=\"productid" + itemList[i].NumIid.ToString() + "\" name=\"productid" + itemList[i].NumIid.ToString() + "\" value=\"" + itemList[i].NumIid.ToString() + "\"><input type=\"hidden\" id=\"price" + itemList[i].NumIid.ToString() + "\" name=\"price" + itemList[i].NumIid.ToString() + "\" value=\"" + itemList[i].Price.ToString() + "\"></div>";
                }
            }
            else
            {
                if (style == "1")
                {
                    str += "<table background=\"http://www.7fshop.com/top/show1/4.gif\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" height=\"30\" style=\"border-right: #999999 1px solid; border-top: #999999 1px solid;border-left: #999999 1px solid; border-bottom: #999999 1px solid\" width=\"580\"><tr><td>    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"><tr><td align=\"left\"><table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" height=\"30\"><tr><td width=\"10\"></td><td background=\"http://www.7fshop.com/top/show1/1.gif\" width=\"24\"></td><td background=\"http://www.7fshop.com/top/show1/2.gif\"><font color=\"white\" style=\"font-size: 13px\"><strong>掌柜推荐商品</strong></font></td><td><img src=\"http://www.7fshop.com/top/show1/3.gif\" /></td></tr></table></td><td align=\"right\"></td></tr></table></td></tr></table>  <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-right: #999999 1px solid;border-top: #999999 1px solid; overflow: hidden; border-left: #999999 1px solid;border-bottom: #999999 1px solid\" width=\"578\"><tr><td valign=\"top\"><TABLE cellSpacing=0 cellPadding=0 width=578 border=0><TBODY><TR><TD align=\"middle\"><TABLE cellSpacing=8 cellPadding=0 align=center border=0><TBODY><TR>";
                    for (int i = 0; i < itemList.Count; i++)
                    {
                        str += "<TD vAlign=top align=\"middle\" width=175 bgColor=white><TABLE cellSpacing=0 cellPadding=0 align=center border=0><TBODY><TR><TD vAlign=top align=\"middle\" width=175 bgColor=white><TABLE cellSpacing=0 cellPadding=0 align=center border=0><TBODY><TR><TD align=\"middle\"><DIV style=\"BORDER-RIGHT: #cccccc 1px solid; BORDER-TOP: #cccccc 1px solid; MARGIN-TOP: 4px; BORDER-LEFT: #cccccc 1px solid; WIDTH: 160px; BORDER-BOTTOM: #cccccc 1px solid; HEIGHT: 160px\"><DIV style=\"OVERFLOW: hidden; WIDTH: 160px; HEIGHT: 160px\"><A href=\"http://item.taobao.com/item.htm?id=" + itemList[i].NumIid.ToString() + "\" target=\"_blank\"><IMG src=\"" + itemList[i].PicUrl + "_160x160.jpg\" border=0 /></A></DIV></DIV></TD></TR><TR><TD align=\"middle\"><DIV style=\"PADDING-RIGHT: 4px; PADDING-LEFT: 4px; FONT-SIZE: 12px; PADDING-BOTTOM: 4px; PADDING-TOP: 4px\"><A style=\"FONT-SIZE: 12px; COLOR: #3f3f3f; TEXT-DECORATION: none\" href=\"http://item.taobao.com/item.htm?id=" + itemList[i].NumIid.ToString() + "\" target=\"_blank\">" + itemList[i].Title + "</A><BR /><FONT style=\"COLOR: #fe596a\"><B>￥&nbsp;" + itemList[i].Price + "元</B></FONT> </DIV><A href=\"http://item.taobao.com/item.htm?id=" + itemList[i].NumIid.ToString() + "\" target=\"_blank\"><IMG src=\"http://www.7fshop.com/top/show1/buy1.gif\" border=0 /></A> <DIV></DIV></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE></TD>";
                        if ((i + 1) % 3 == 0)
                        {
                            str += "</tr><tr>";
                        }
                    }

                    str += "</TR></TBODY></TABLE></td></tr><tr><td align=\"right\" height=\"24\" style=\"border-bottom: #999999 1px solid\" valign=\"center\"><a href=\"" + nickid + "\" style=\"text-decoration: none\" target=\"_blank\"><font style=\"font-size: 13px; color: #ff6600\"><strong>更多详情请见 " + nickid + "</strong>&nbsp;</font></a></td></tr></table></td></tr></table>";
                }
                else
                {
                    for (int i = 0; i < itemList.Count; i++)
                    {
                        str += "<a href=\"http://item.taobao.com/item.htm?id=" + itemList[i].NumIid.ToString() + "\" target=\"_blank\"><img src=\"" + itemList[i].PicUrl + "\" border=\"0\" /></a><br />";
                        str += "<a href=\"http://item.taobao.com/item.htm?id=" + itemList[i].NumIid.ToString() + "\" target=\"_blank\">" + itemList[i].Title + "</a> 售价：" + itemList[i].Price + "元<br><br>";
                    }
                }
            }
            Response.Write(str);
        }
    }
}
