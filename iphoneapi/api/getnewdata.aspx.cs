﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Common;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;
using Taobao.Top.Api;

public partial class api_getnewdata : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string uid = this.TextBox1.Text;
        string taobaonick = this.TextBox2.Text;
        string sql = "SELECT * FROM TeteShop WHERE nick = '" + uid + "'";

        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        { 
            //同步分类数据
            TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", dt.Rows[0]["appkey"].ToString(), dt.Rows[0]["appsecret"].ToString());
            SellercatsListGetRequest request1 = new SellercatsListGetRequest();
            request1.Fields = "cid,parent_cid,name,is_parent,";
            request1.Nick = taobaonick;
            PageList<SellerCat> cat = client.SellercatsListGet(request1);

            //清除之前的老分类
            sql = "DELETE FROM TeteShopCategory WHERE nick = '" + uid + "'";
            utils.ExecuteNonQuery(sql);

            for (int i = 0; i < cat.Content.Count; i++)
            {
                //过滤其他软件增加的分类
                if (cat.Content[i].Name.IndexOf("统计") != -1 || cat.Content[i].Name.IndexOf("多买多优惠") != -1)
                {
                    continue;
                }

                sql = "INSERT INTO TeteShopCategory (" +
                                "cateid, " +
                                "catename, " +
                                "parentid, " +
                                "nick " +
                            " ) VALUES ( " +
                                " '" + cat.Content[i].Cid + "', " +
                                " '" + cat.Content[i].Name + "', " +
                                " '" + cat.Content[i].ParentCid + "', " +
                                " '" + uid + "' " +
                          ") ";
                Response.Write(sql + "<br>");
                utils.ExecuteNonQuery(sql);
            }


            //清除之前的老商品数据
            sql = "DELETE FROM TeteShopItem WHERE nick = '" + uid + "'";
            utils.ExecuteNonQuery(sql);

            //同步商品数据
            for (int j = 1; j <= 500; j++)
            {
                ItemsOnsaleGetRequest request = new ItemsOnsaleGetRequest();
                request.Fields = "num_iid,title,price,pic_url,seller_cids";
                request.PageSize = 200;
                request.PageNo = j;

                PageList<Item> product = client.ItemsOnsaleGet(request, dt.Rows[0]["session"].ToString());
                for (int i = 0; i < product.Content.Count; i++)
                {
                    sql = "INSERT INTO TeteShopItem (" +
                                "cateid, " +
                                "itemid, " +
                                "itemname, " +
                                "picurl, " +
                                "linkurl, " +
                                "price, " +
                                "nick " +
                            " ) VALUES ( " +
                                " '" + product.Content[i].SellerCids + "', " +
                                " '" + product.Content[i].NumIid + "', " +
                                " '" + product.Content[i].Title + "', " +
                                " '" + product.Content[i].PicUrl + "', " +
                                " 'http://a.m.taobao.com/i" + product.Content[i].NumIid + ".htm', " +
                                " '" + product.Content[i].Price + "', " +
                                " '" + uid + "' " +
                          ") ";
                    Response.Write(sql + "<br>");
                    utils.ExecuteNonQuery(sql);

                    //更新分类数量
                    sql = "UPDATE TeteShopCategory SET catecount = catecount + 1 WHERE nick = '" + uid + "' AND CHARINDEX(cateid, '" + product.Content[i].SellerCids + "') > 0";
                    utils.ExecuteNonQuery(sql);
                }
                if (product.Content.Count < 200)
                {
                    break;
                }
            }
        }
    }
}