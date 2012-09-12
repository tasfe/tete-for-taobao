using System;
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
    public string nick = string.Empty;
    public string session = string.Empty;
    public string st = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        session = cookie.getCookie("top_session");
        st = cookie.getCookie("short");
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Decrypt(taobaoNick);

        Act(st, nick);
        Response.Write("数据更新完毕！");
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string uid = this.TextBox1.Text;
        string taobaonick = this.TextBox2.Text;
        Act(uid, taobaonick);
        Response.Write("数据更新完毕！");
    }

    private void Act(string uid, string taobaonick)
    {
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
            //sql = "DELETE FROM TeteShopCategory WHERE nick = '" + uid + "'";
            //utils.ExecuteNonQuery(sql);

            for (int i = 0; i < cat.Content.Count; i++)
            {
                //过滤其他软件增加的分类
                if (cat.Content[i].Name.IndexOf("统计") != -1 || cat.Content[i].Name.IndexOf("多买多优惠") != -1)
                {
                    continue;
                }

                //如果已经存在则不处理
                sql = "SELECT COUNT(*) FROM TeteShopCategory WHERE cateid='" + cat.Content[i].Cid + "'";
                string count2 = utils.ExecuteString(sql);
                if (count2 == "0")
                {
                    sql = "INSERT INTO TeteShopCategory (" +
                                    "cateid, " +
                                    "catename, " +
                                    "oldname, " +
                                    "parentid, " +
                                    "nick " +
                                " ) VALUES ( " +
                                    " '" + cat.Content[i].Cid + "', " +
                                    " '" + cat.Content[i].Name + "', " +
                                    " '" + cat.Content[i].Name + "', " +
                                    " '" + cat.Content[i].ParentCid + "', " +
                                    " '" + uid + "' " +
                              ") ";
                    //Response.Write(sql + "<br>");
                    utils.ExecuteNonQuery(sql);
                }

                if (cat.Content[i].ParentCid == 0)
                {
                    sql = "SELECT COUNT(*) FROM TeteShopAds WHERE nick = '" + uid + "' AND typ = '" + cat.Content[i].Cid + "'";
                    string count1 = utils.ExecuteString(sql);
                    if (count1 == "0")
                    {
                        sql = "INSERT INTO TeteShopAds (" +
                                    "typ, " +
                                    "url, " +
                                    "orderid, " +
                                    "nick " +
                                " ) VALUES ( " +
                                    " '" + cat.Content[i].Cid + "', " +
                                    " 'http://langbow.tmall.com', " +
                                    " '1', " +
                                    " '" + uid + "' " +
                              ") ";
                        utils.ExecuteNonQuery(sql);

                        sql = "INSERT INTO TeteShopAds (" +
                                    "typ, " +
                                    "url, " +
                                    "orderid, " +
                                    "nick " +
                                " ) VALUES ( " +
                                    " '" + cat.Content[i].Cid + "', " +
                                    " 'http://langbow.tmall.com', " +
                                    " '2', " +
                                    " '" + uid + "' " +
                              ") ";
                        utils.ExecuteNonQuery(sql);

                        sql = "INSERT INTO TeteShopAds (" +
                                    "typ, " +
                                    "url, " +
                                    "orderid, " +
                                    "nick " +
                                " ) VALUES ( " +
                                    " '" + cat.Content[i].Cid + "', " +
                                    " 'http://langbow.tmall.com', " +
                                    " '3', " +
                                    " '" + uid + "' " +
                              ") ";
                        utils.ExecuteNonQuery(sql);
                    }
                }
            }


            //清除之前的老商品数据
            //sql = "DELETE FROM TeteShopItem WHERE nick = '" + uid + "'";
            //utils.ExecuteNonQuery(sql);

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

                    sql = "SELECT COUNT(*) FROM TeteShopItem WHERE nick = '" + uid + "' AND itemid = '" + product.Content[i].NumIid + "'";
                    string count3 = utils.ExecuteString(sql);
                    if (count3 == "0")
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
                        //Response.Write(sql + "<br>");
                        utils.ExecuteNonQuery(sql);

                        //更新分类数量
                        sql = "UPDATE TeteShopCategory SET catecount = catecount + 1 WHERE nick = '" + uid + "' AND CHARINDEX(cateid, '" + product.Content[i].SellerCids + "') > 0";
                        //Response.Write(sql + "<br>");
                        utils.ExecuteNonQuery(sql);
                    }

                }
                if (product.Content.Count < 200)
                {
                    break;
                }
            }


            //更新大类商品数量
            sql = "SELECT * FROM TeteShopCategory WHERE nick = '" + uid + "' AND parentid <> 0";
            DataTable dtCate = utils.ExecuteDataTable(sql);
            for (int k = 0; k < dtCate.Rows.Count; k++)
            {
                //Response.Write(sql + "<br>");
                sql = "UPDATE TeteShopCategory SET catecount = catecount + " + dtCate.Rows[k]["catecount"].ToString() + " WHERE nick = '" + uid + "' AND cateid = " + dtCate.Rows[k]["parentid"].ToString();
                utils.ExecuteNonQuery(sql);
            }
        }
    }
}