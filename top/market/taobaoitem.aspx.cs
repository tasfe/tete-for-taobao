﻿using System;
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

public partial class top_market_taobaoitem : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Cookie cookie = new Cookie();
        string taobaoNick = cookie.getCookie("nick");
        string session = cookie.getCookie("top_session");
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
        string act = utils.NewRequest("act", utils.RequestType.QueryString);
        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", "12132145", "1fdd2aadd5e2ac2909db2967cbb71e7f");

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
                Response.Write("<input type='checkbox' name='items' id='item_" + product.Content[i].NumIid + "' title='" + product.Content[i].Title + "' value='" + product.Content[i].NumIid + "' onclick=\"InitArea(this)\"><label for='item_" + product.Content[i].NumIid + "'>" + product.Content[i].Title + "</label><br>");
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
    }
}
