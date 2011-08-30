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
using Microsoft.Practices.EnterpriseLibrary.Caching;
using System.IO;

public partial class top_market_addidea_3 : System.Web.UI.Page
{
    public string style = string.Empty;
    public string size = string.Empty;
    public string type = string.Empty;
    public string orderby = string.Empty;
    public string query = string.Empty;
    public string shopcat = string.Empty;
    public string name = string.Empty;
    public string items = string.Empty;

    public string nickid = string.Empty;
    public string nickidEncode = string.Empty;
    public string md5nick = string.Empty;
    public string tabletitle = string.Empty;

    public string width = string.Empty;
    public string height = string.Empty;

    public string id = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            id = utils.NewRequest("id", Common.utils.RequestType.QueryString);

            if (id != "" && !utils.IsInt32(id))
            {
                Response.Write("非法参数1");
                Response.End();
                return;
            }


            style = utils.NewRequest("style", Common.utils.RequestType.Form);
            size = utils.NewRequest("size", Common.utils.RequestType.Form);
            type = utils.NewRequest("type", Common.utils.RequestType.Form);
            orderby = utils.NewRequest("orderby", Common.utils.RequestType.Form);
            query = utils.NewRequest("query", Common.utils.RequestType.Form);
            shopcat = utils.NewRequest("shopcat", Common.utils.RequestType.Form);
            name = utils.NewRequest("name", Common.utils.RequestType.Form);
            items = utils.NewRequest("items", Common.utils.RequestType.Form);

            string act = utils.NewRequest("act", Common.utils.RequestType.Form);
            if (act == "save")
            {
                Cookie cookie = new Cookie();
                string taobaoNick = cookie.getCookie("nick");
                string session = cookie.getCookie("top_session");

                Rijndael_ encode = new Rijndael_("tetesoft");
                taobaoNick = encode.Decrypt(taobaoNick);

                //记录到本地数据库
                string sql = "INSERT INTO TopIdea (" +
                                "name, " +
                                "showtype, " +
                                "nick, " +
                                "size, " +
                                "style, " +
                                "orderby, " +
                                "shopcategoryid, " +
                                "query " +
                            " ) VALUES ( " +
                                " '" + name + "', " +
                                " '" + type + "', " +
                                " '" + taobaoNick + "', " +
                                " '" + size + "', " +
                                " '" + style + "', " +
                                " '" + orderby + "', " +
                                " '" + shopcat + "', " +
                                " '" + query + "' " +
                          ") ";

                //如果为编辑模式
                if (id != "" && id != "0")
                {
                    //更新广告
                    sql = "UPDATE TopIdea SET " +
                                "name = '" + name + "', " +
                                "showtype = '" + type + "', " +
                                "size = '" + size + "', " +
                                "style = '" + style + "', " +
                                "orderby = '" + orderby + "', " +
                                "shopcategoryid = '" + shopcat + "', " +
                                "query = '" + query + "' " +
                            " WHERE id = " + id;
                    utils.ExecuteNonQuery(sql);

                    //如果是自动更新模式，需要更新店铺商品数据
                    sql = "SELECT shopcategoryid,query,nick,showtype FROM TopIdea WHERE nick = '" + taobaoNick + "' AND id = " + id;
                    DataTable dt1 = utils.ExecuteDataTable(sql);
                    if (dt1.Rows[0]["showtype"].ToString() == "0")
                    {
                        //更新下架商品数据
                        shopcat = dt1.Rows[0][0].ToString();
                        query = dt1.Rows[0][1].ToString();
                        taobaoNick = dt1.Rows[0][2].ToString();

                        //获取新商品列表
                        TopXmlRestClient clientaa = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", "12132145", "1fdd2aadd5e2ac2909db2967cbb71e7f");
                        ItemsOnsaleGetRequest request = new ItemsOnsaleGetRequest();
                        request.Fields = "num_iid,title,price,pic_url";
                        request.PageSize = 12;
                        request.OrderBy = "list_time:desc";
                        request.OrderBy = "volume:desc";
                        if (shopcat != "0")
                        {
                            request.SellerCids = shopcat;
                        }
                        request.Q = query;

                        //清理关联商品
                        sql = "DELETE FROM TopIdeaProduct WHERE ideaid = " + id;
                        utils.ExecuteNonQuery(sql);

                        //未登录用户不能获取小二下架或删除的商品-错误过滤，原因未知
                        try
                        {
                            PageList<Item> product = clientaa.ItemsOnsaleGet(request, session);

                            for (int i = 0; i < product.Content.Count; i++)
                            {
                                sql = "INSERT INTO TopIdeaProduct (" +
                                            "itemid, " +
                                            "itemname, " +
                                            "itemprice, " +
                                            "itempicurl, " +
                                            "ideaid " +
                                        " ) VALUES ( " +
                                            " '" + product.Content[i].NumIid + "', " +
                                            " '" + product.Content[i].Title + "', " +
                                            " '" + product.Content[i].Price + "', " +
                                            " '" + product.Content[i].PicUrl + "', " +
                                            " '" + id + "' " +
                                      ") ";
                                utils.ExecuteNonQuery(sql);
                            }
                        }
                        catch { }
                    }

                    //清理关联商品
                    sql = "DELETE FROM TopIdeaProduct WHERE ideaid = " + id;
                    utils.ExecuteNonQuery(sql);

                    //清理广告缓存
                    CacheManager testcaching1 = CacheFactory.GetCacheManager();
                    if (testcaching1.Contains("cache_1_" + id))
                    {
                        testcaching1.Remove("cache_1_" + id);
                    }

                    //更新广告图片
                    string folderPath = Server.MapPath("\\show\\folder\\" + MD5(taobaoNick) + "\\result_" + id + ".jpg");
                    if (File.Exists(folderPath))
                    {
                        File.Delete(folderPath);
                    }
                }

                utils.ExecuteNonQuery(sql);

                if (id != "" && id != "0")
                {
                    //编辑模式，ID不变
                }
                else
                {
                    //插入模式，获取最新ID
                    sql = "SELECT TOP 1 id FROM TopIdea WHERE nick = '" + taobaoNick + "' ORDER BY id DESC";
                    id = utils.ExecuteString(sql);
                }

                //获取符合结果集的相关商品并记录到本地数据库
                TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", "12132145", "1fdd2aadd5e2ac2909db2967cbb71e7f");

                //如果为自动选择模式
                if (type == "0")
                {
                    ItemsOnsaleGetRequest request = new ItemsOnsaleGetRequest();
                    request.Fields = "num_iid,title,price,pic_url";
                    request.PageSize = 12;

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

                    //未登录用户不能获取小二下架或删除的商品-错误过滤，原因未知
                    try
                    {
                        PageList<Item> product = client.ItemsOnsaleGet(request, session);

                        for (int i = 0; i < product.Content.Count; i++)
                        {
                            sql = "INSERT INTO TopIdeaProduct (" +
                                        "itemid, " +
                                        "itemname, " +
                                        "itemprice, " +
                                        "itempicurl, " +
                                        "ideaid " +
                                    " ) VALUES ( " +
                                        " '" + product.Content[i].NumIid + "', " +
                                        " '" + product.Content[i].Title + "', " +
                                        " '" + product.Content[i].Price + "', " +
                                        " '" + product.Content[i].PicUrl + "', " +
                                        " '" + id + "' " +
                                  ") ";
                            utils.ExecuteNonQuery(sql);
                        }
                    }
                    catch { }
                }
                else
                {
                    string[] arr = items.Split(',');
                    List<Item> itemList = new List<Item>();
                    for (int i = 0; i < arr.Length; i++)
                    {
                        ItemGetRequest request = new ItemGetRequest();
                        request.Fields = "num_iid,title,price,pic_url";
                        request.NumIid = long.Parse(arr[i]);

                        Item product = client.ItemGet(request);
                        itemList.Add(product);
                    }

                    for (int i = 0; i < itemList.Count; i++)
                    {
                        sql = "INSERT INTO TopIdeaProduct (" +
                                    "itemid, " +
                                    "itemname, " +
                                    "itemprice, " +
                                    "itempicurl, " +
                                    "ideaid " +
                                " ) VALUES ( " +
                                    " '" + itemList[i].NumIid + "', " +
                                    " '" + itemList[i].Title + "', " +
                                    " '" + itemList[i].Price + "', " +
                                    " '" + itemList[i].PicUrl + "', " +
                                    " '" + id + "' " +
                              ") ";
                        utils.ExecuteNonQuery(sql);
                    }
                }
            }

            //获取NICK的淘宝ID
            Cookie cookieNew = new Cookie();
            string taobaoNickNew = cookieNew.getCookie("nick");

            Rijndael_ encodeaa = new Rijndael_("tetesoft");
            taobaoNickNew = encodeaa.Decrypt(taobaoNickNew);

            md5nick = MD5(taobaoNickNew);

            string sqlNew = "SELECT sid FROM TopTaobaoShop WHERE nick = '" + taobaoNickNew + "'";
            DataTable dtNew = utils.ExecuteDataTable(sqlNew);
            if (dtNew.Rows.Count != 0)
            {
                nickid = "http://shop" + dtNew.Rows[0]["sid"].ToString() + ".taobao.com/";
            }
            else
            {
                nickid = "http://www.taobao.com/";
            }
            nickidEncode = "http://www.7fshop.com/click/?s=" + EncodeStr(new string[] { id, "0", nickid });

            string sql112 = "SELECT * FROM TopIdea WHERE id = " + id;
            DataTable newdt1 = utils.ExecuteDataTable(sql112);
            if (newdt1.Rows.Count != 0)
            {
                tabletitle = newdt1.Rows[0]["name"].ToString();
                size = newdt1.Rows[0]["size"].ToString();
            }

            string num = string.Empty;
            string row = string.Empty;
            switch (size)
            {
                case "514*160":
                    num = "5";
                    row = "5";
                    break;
                case "514*288":
                    num = "10";
                    row = "5";
                    break;
                case "664*160":
                    num = "6";
                    row = "6";
                    break;
                case "312*288":
                    num = "6";
                    row = "3";
                    break;
                case "336*280":
                    num = "4";
                    row = "2";
                    break;
                case "714*160":
                    num = "7";
                    row = "7";
                    break;
                case "114*418":
                    num = "3";
                    row = "1";
                    break;
                case "218*286":
                    num = "4";
                    row = "2";
                    break;
                case "743*308":
                    num = "4";
                    row = "4";
                    break;
                default:
                    num = "4";
                    row = "4";
                    break;
            }

            //绑定商品列表
            string sql11 = "SELECT TOP " + num + " *,'' AS html FROM TopIdeaProduct WHERE ideaid = " + id;
            DataTable newdt = utils.ExecuteDataTable(sql11);

            //加换行符
            for (int i = 0; i < newdt.Rows.Count; i++)
            {
                if ((i + 1) % int.Parse(row) == 0)
                {
                    newdt.Rows[i]["html"] = "</tr><tr>";
                }
            }

            if (size != "743*308")
            {
                panel1.Visible = true;
                panel2.Visible = false;
                rptProduct.DataSource = newdt;
                rptProduct.DataBind();
            }
            else
            {
                panel1.Visible = false;
                panel2.Visible = true;
                rptProduct2.DataSource = newdt;
                rptProduct2.DataBind();
            }

            string[] arrNew = size.Split('*');
            width = arrNew[0];
            height = arrNew[1];
        }
        catch { }
    }

    private string EncodeStr(string[] parmArray)
    {
        string newStr = string.Empty;
        for (int i = 0; i < parmArray.Length; i++)
        {
            if (i == 0)
            {
                newStr = parmArray[i];
            }
            else
            {
                newStr += "|" + parmArray[i];
            }
        }

        Rijndael_ encode = new Rijndael_("tetesoftstr");
        newStr = encode.Encrypt(newStr);
        newStr = HttpUtility.UrlEncode(newStr);
        return newStr;
    }

    /// <summary> 
    /// MD5 加密函数 
    /// </summary> 
    /// <param name="str"></param> 
    /// <param name="code"></param> 
    /// <returns></returns> 
    public static string MD5(string str)
    {
        return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").Substring(8, 16);
    }
}
