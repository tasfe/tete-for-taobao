using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using Common;
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;
using System.Collections.Generic;
using System.Data;

public partial class top_groupbuy_taobaoitemgetactivity : System.Web.UI.Page
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
               string tempstr = "未参加促销活动";

               string str = " <table  width=\"800px;\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"margin:0px; padding:0px\">  <tr><td style=\" width:100px;\"><img src=" + product.Content[i].PicUrl + "_80x80.jpg></td><td  style=\" width:300px;\"><a   href=\"http://item.taobao.com/item.htm?id=" + product.Content[i].NumIid.ToString() + "\" target=\"_blank\">" + product.Content[i].Title + "</a></td><td  style=\" width:150px;\">" + product.Content[i].Price.ToString() + "</td><td  style=\" width:250px;\">" + tempstr + "</td></tr> </table><hr/>";
                Response.Write(str);
               //Response.Write("<input type='checkbox' name='items' id='item_" + product.Content[i].NumIid + "' title='" + product.Content[i].Title.Replace("%", "") + "' value='" + product.Content[i].NumIid + "' onclick=\"SetInitArea(this)\"><label onMouseOver=\"javascript:ddrivetip('<img src=" + product.Content[i].PicUrl + "_80x80.jpg>','#ffffff',100)\" onMouseOut=\"hideddrivetip()\" for='item_" + product.Content[i].NumIid + "'>" + product.Content[i].Title.Replace("%", "") + "</label><br>");

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

            for (int i = 0; i < itemList.Count; i++)
            {
                str += "<div id='div" + itemList[i].NumIid.ToString() + "' width=\"800px\"><table width=\"800px\"><tr width=\"800px\"><td width=\"90px\"><a href=\"http://item.taobao.com/item.htm?id=" + itemList[i].NumIid.ToString() + "\" target=\"_blank\"><img src=\"" + itemList[i].PicUrl + "_80x80.jpg\" border=\"0\" /></a></td>";
                str += "<td  width=\"140px\"><a   href=\"http://item.taobao.com/item.htm?id=" + itemList[i].NumIid.ToString() + "\" target=\"_blank\">" + itemList[i].Title + "</a> </td><td   width=\"200px\"><input type=\"text\" size=\"27\" id=\"groupbuylistname" + itemList[i].NumIid.ToString() + "\"   name=\"groupbuylistname\"  maxlength=\"30\"  value=\"\" /></td><td  width=\"100px\"> " + itemList[i].Price + "元 <input   type=\"hidden\" id=\"productid" + itemList[i].NumIid.ToString() + "\" name=\"productid\"  value=\"" + itemList[i].NumIid.ToString() + "\"><input type=\"hidden\" id=\"price" + itemList[i].NumIid.ToString() + "\" name=\"price\" value=\"" + itemList[i].Price.ToString() + "\"></td><td  width=\"100px\">  <input type=\"text\" id=\"zhekou" + itemList[i].NumIid.ToString() + "\" size=\"10\" name=\"zhekou\" onblur=\"setzekou(this)\" /> 元 <br /> 打<input type=\"text\" size=\"8\" id=\"zhekou1" + itemList[i].NumIid.ToString() + "\" name=\"zhekou1\"   onblur=\"setprice(this)\"/> 折 <br />限购：<select id=\"xiangou" + itemList[i].NumIid.ToString() + "\" name=\"xiangou\"> <OPTION selected  value=0>否</OPTION> <OPTION  value=1>是</OPTION> </select>  <span id=\"errmsg" + i + "\" style=\"color:Red\"></span> </td><td  width=\"90px\">  <input type=\"text\"  size=\"8\"  name=\"rcount\" value=\"300\" /> </td><td  width=\"80px\"><a onclick=\"deleteDIV('del1" + itemList[i].NumIid.ToString() + "')\"  style=\"cursor:hand;\">删除</a></td></tr></table><input id=\"del1" + itemList[i].NumIid.ToString() + "\" name=\"del\" value='' type=\"hidden\" ></div>";
            }
 
            Response.Write(str);
        }
        else if (act == "getactivityitem")
        {
            //获取用户店铺商品列表
            //ItemsOnsaleGetRequest request = new ItemsOnsaleGetRequest();
            //request.Fields = "num_iid,title,price,pic_url";
            //request.PageSize = pageSizeNow;
            //request.PageNo = int.Parse(page);
            //request.Q = q;
            //if (catid != "0")
            //{
            //    request.SellerCids = catid;
            //}

            //PageList<Item> product = client.ItemsOnsaleGet(request, session);

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
            string str = string.Empty;
            //输出页面HTML
            for (int i = 0; i < product.Content.Count; i++)
            {


                str += "<div id='div" + product.Content[i].NumIid.ToString() + "' width=\"800px\"><table width=\"800px\"><tr width=\"800px\"><td width=\"90px\"><a href=\"http://item.taobao.com/item.htm?id=" + product.Content[i].NumIid.ToString() + "\" target=\"_blank\"><img src=\"" + product.Content[i].PicUrl + "_80x80.jpg\" border=\"0\" /></a></td>";
                str += "<td  width=\"140px\"><a   href=\"http://item.taobao.com/item.htm?id=" + product.Content[i].NumIid.ToString() + "\" target=\"_blank\">" + product.Content[i].Title + "</a> </td><td   width=\"200px\"><input type=\"text\" size=\"27\" id=\"groupbuylistname" + product.Content[i].NumIid.ToString() + "\"   name=\"groupbuylistname\"  maxlength=\"30\"  value=\"\" /></td><td  width=\"100px\"> " + product.Content[i].Price + "元 <input   type=\"hidden\" id=\"productid" + product.Content[i].NumIid.ToString() + "\" name=\"productid\"  value=\"" + product.Content[i].NumIid.ToString() + "\"><input type=\"hidden\" id=\"price" + product.Content[i].NumIid.ToString() + "\" name=\"price\" value=\"" + product.Content[i].Price.ToString() + "\"></td><td  width=\"100px\">  <input type=\"text\" id=\"zhekou" + product.Content[i].NumIid.ToString() + "\" size=\"10\" name=\"zhekou\" onblur=\"setzekou(this)\" /> 元 <br /> 打<input type=\"text\" size=\"8\" id=\"zhekou1" + product.Content[i].NumIid.ToString() + "\" name=\"zhekou1\"   onblur=\"setprice(this)\"/> 折 <br />限购：<select id=\"xiangou" + product.Content[i].NumIid.ToString() + "\" name=\"xiangou\"> <OPTION selected  value=0>否</OPTION> <OPTION  value=1>是</OPTION> </select>  <span id=\"errmsg" + i + "\" style=\"color:Red\"></span> </td><td  width=\"90px\">  <input type=\"text\"  size=\"8\"  name=\"rcount\" value=\"300\" /> </td><td  width=\"80px\"><a onclick=\"deleteDIV('del1" + product.Content[i].NumIid.ToString() + "')\"  style=\"cursor:hand;\">删除</a></td></tr></table><input id=\"del1" + product.Content[i].NumIid.ToString() + "\" name=\"del\" value='' type=\"hidden\" ></div>";

            }
            Response.Write(str);
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
    }
}