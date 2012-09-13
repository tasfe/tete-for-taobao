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

        string appkey = "12159997";
        string secret = "614e40bfdb96e9063031d1a9e56fbed5";

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
        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", appkey, secret);

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
                    Response.Write("<input type='radio' name='items' id='item_" + product.Content[i].NumIid + "' title='" + product.Content[i].Title.Replace("%", "") + "' value='" + product.Content[i].NumIid + "' onclick=\"InitArea(this)\"><label onMouseOver=\"javascript:ddrivetip('<img src=" + product.Content[i].PicUrl + "_80x80.jpg>','#ffffff',100)\" onMouseOut=\"hideddrivetip()\" for='item_" + product.Content[i].NumIid + "'>" + product.Content[i].Title.Replace("%", "") + "</label><br>");
                }
                else
                {
                    Response.Write("<input type='checkbox' name='items' id='item_" + product.Content[i].NumIid + "' title='" + product.Content[i].Title.Replace("%", "") + "' value='" + product.Content[i].NumIid + "' onclick=\"InitArea(this)\"><label onMouseOver=\"javascript:ddrivetip('<img src=" + product.Content[i].PicUrl + "_80x80.jpg>','#ffffff',100)\" onMouseOut=\"hideddrivetip()\" for='item_" + product.Content[i].NumIid + "'>" + product.Content[i].Title.Replace("%", "") + "</label><br>");
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

            //string sqlNew = "SELECT sid FROM TopTaobaoShop WHERE nick = '" + taobaoNick + "'";
            //DataTable dtNew = utils.ExecuteDataTable(sqlNew);
            //string nickid = string.Empty;
            //if (dtNew.Rows.Count != 0)
            //{
            //    nickid = "http://shop" + dtNew.Rows[0]["sid"].ToString() + ".taobao.com/";
            //}
            //else
            //{
            //    nickid = "http://www.taobao.com/";
            //}


            if (isradio == "1")
            {
                //团购需要的商品数据
                for (int i = 0; i < itemList.Count; i++)
                {
                    str = "<div id=\"item_" + itemList[i].NumIid.ToString() + "\" style=\"float:left;width:46px;border:solid 1px #ccc;padding:2px;margin:2px;\"><A href=\"http://item.taobao.com/item.htm?id=" + itemList[i].NumIid.ToString() + "\" target=\"_blank\"><IMG src=\"" + itemList[i].PicUrl + "_40x40.jpg\" border=0 title=\"" + itemList[i].Title + "\" /></A><br>" + itemList[i].Price + "<input type=\"hidden\" id=\"productid\" name=\"productid\" value=\"" + itemList[i].NumIid.ToString() + "\"><input type=\"hidden\" id=\"price\" name=\"price\" value=\"" + itemList[i].Price.ToString() + "\"><br><a href=\"javascript:delitem(" + itemList[i].NumIid.ToString() + ")\">删除</a></div>";
                }
            }
            
            Response.Write(str);
        }
    }
}
