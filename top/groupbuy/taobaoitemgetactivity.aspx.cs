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
               string sql = "select * from Tete_activitylist where Status<>4 and Status<>3 and Status<>2 and  ProductID=" + product.Content[i].NumIid.ToString().Trim();
               DataTable dt = utils.ExecuteDataTable(sql);
               if (dt != null && dt.Rows.Count > 0)
               {
                   tempstr = " <font color=green> 已参加促销活动</font>";
               }

               string str = " <table  width=\"800px;\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"margin:0px; padding:0px\">  <tr><td style=\" width:100px;\"><img width=50px src=" + product.Content[i].PicUrl + "_80x80.jpg></td><td  style=\" width:300px;\"><a   href=\"http://item.taobao.com/item.htm?id=" + product.Content[i].NumIid.ToString() + "\" target=\"_blank\">" + product.Content[i].Title + "</a></td><td  style=\" width:150px;\">" + product.Content[i].Price.ToString() + "</td><td  style=\" width:250px;\">" + tempstr + "</td></tr> </table><hr/>";
                Response.Write(str);


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
            Response.End();
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
            Response.End();
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
            Response.End();
        }
        else if (act == "getactivityitem")
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
            string str = string.Empty;
            string discountType = string.Empty;//促销方式（打折：DISCOUNT，减价：PRICE）
            string discountValue=string.Empty;//促销力度（7折限制）
            string Rcount = "300";//已参团人数(只在对设置团购模板时有用)
            string newPrice2 = string.Empty;//促销价
            string price2 = string.Empty;//商品价格
            string itemtype2 = string.Empty;
            string itemtypevalue2 = string.Empty;
            string yhCount=string.Empty;
            string hdstr = string.Empty;
            string aid = Request.QueryString["aid"].ToString();
            string yuanitemtype = "";
            string yuandiscountType = "";
            string yuandiscountValue = "";
            string yuandecreaseNum = "";
   
            string sql2 = "select * from tete_activity where id="+aid;
            DataTable dt33 = utils.ExecuteDataTable(sql2);
            if (dt33 != null)
            {
                yuanitemtype = dt33.Rows[0]["itemType"].ToString();
                yuandiscountType = dt33.Rows[0]["discountType"].ToString();
                yuandiscountValue = dt33.Rows[0]["discountValue"].ToString();
                yuandecreaseNum = dt33.Rows[0]["decreaseNum"].ToString();
            }
 
            //输出页面HTML
            for (int i = 0; i < product.Content.Count; i++)
            {
               
                //先判断该商品是否已经参团（状态为：进行中和暂停中）
                string sql = "SELECT *  FROM  tete_activitylist where ProductID=" + product.Content[i].NumIid.ToString() + " and (Status=3 or Status=1 or Status=0) and nick='"+taobaoNick+"'";
                DataTable dtNew2 = utils.ExecuteDataTable(sql);
                // 取得促销价格
                if (dtNew2 != null && dtNew2.Rows.Count > 0)
                {
                    Rcount = dtNew2.Rows[0]["Rcount"].ToString();
                    discountValue = dtNew2.Rows[0]["discountValue"].ToString();
                    discountType = dtNew2.Rows[0]["discountType"].ToString();
                    price2 = product.Content[i].Price;
                    hdstr = "<div id=\"del" + product.Content[i].NumIid.ToString() + "\">已参加活动:" + dtNew2.Rows[0]["name"].ToString() + " <br><a href=\"javascript:delItemAction(" + product.Content[i].NumIid.ToString() + ")\">删除此促销活动</a></div> <div style=\"display:none\" id=\"add" + product.Content[i].NumIid.ToString() + "\"><a href=\"javascript:addItemAction(" + product.Content[i].NumIid.ToString() + ")\">参加活动</a></div> ";

                    if (discountType.Trim() == "DISCOUNT")
                    {
                        itemtype2 = "<select onchange=\"changeSelect(" + product.Content[i].NumIid.ToString() + ")\" id=\"discountType" + product.Content[i].NumIid.ToString() + "\" name=\"discountType\" > <option selected=\"selected\" value=\"DISCOUNT\">打折</option><option value=\"PRICE\">减价</option> </select>";

                        itemtypevalue2 = " <div id=\"zheDiv" + product.Content[i].NumIid.ToString() + "\" style=\"display: block;\"><input type=\"text\" onkeyup=\"blurValue(" + product.Content[i].NumIid.ToString() + ")\" id=\"changeZhe" + product.Content[i].NumIid.ToString() + "\" value=\"" + discountValue + "\"  name=\"discountValue\" style=\"width:30px\">折</div><div  style=\"display: none;\" id=\"jianDiv" + product.Content[i].NumIid.ToString() + "\">减<input type=\"text\" onkeyup=\"blurValue(" + product.Content[i].NumIid.ToString() + ")\" id=\"changeJian" + product.Content[i].NumIid.ToString() + "\" name=\"discountValue\" style=\"width:30px\" >元</div>";
                        newPrice2 = (decimal.Parse(price2) * decimal.Parse(discountValue) * 0.1m).ToString();



                        yhCount = " <span id=\"duojian" + product.Content[i].NumIid.ToString() + "\" style=\"display: inline;\">多件<input type=\"hidden\" id=\"decreaseNumHiden" + product.Content[i].NumIid.ToString() + "\" value=\"0\" name=\"decreaseNumHiden\"></span> <span  style=\"display: none;\" id=\"yijian" + product.Content[i].NumIid.ToString() + "\"><select onchange=\"decreaseNumChange(" + product.Content[i].NumIid.ToString() + ")\" id=\"decreaseNumSel" + product.Content[i].NumIid.ToString() + "\" name=\"decreaseNumSel\"> <option selected=\"selected\" value=\"0\">多件</option> <option value=\"1\">一件</option> </select></span>";
                    }
                    else
                    {
                        itemtype2 = "<select onchange=\"changeSelect(" + product.Content[i].NumIid.ToString() + ")\" id=\"discountType" + product.Content[i].NumIid.ToString() + "\" name=\"discountType\" > <option   value=\"DISCOUNT\">打折</option><option  selected=\"selected\"  value=\"PRICE\">减价</option> </select>";

                        if (dtNew2.Rows[0]["decreaseNum"].ToString() == "0")
                        {
                            yhCount = " <span id=\"duojian" + product.Content[i].NumIid.ToString() + "\" style=\"display: none;\">多件<input type=\"hidden\" id=\"decreaseNumHiden" + product.Content[i].NumIid.ToString() + "\" value=\"0\" name=\"decreaseNumHiden\"></span> <span style=\"display: inline;\" id=\"yijian" + product.Content[i].NumIid.ToString() + "\"><select onchange=\"decreaseNumChange(" + product.Content[i].NumIid.ToString() + ")\" id=\"decreaseNumSel" + product.Content[i].NumIid.ToString() + "\" name=\"decreaseNumSel\"> <option selected=\"selected\" value=\"0\">多件</option> <option value=\"1\">一件</option> </select></span>";
                        }
                        else {
                            yhCount = " <span id=\"duojian" + product.Content[i].NumIid.ToString() + "\" style=\"display: none;\">多件<input type=\"hidden\" id=\"decreaseNumHiden" + product.Content[i].NumIid.ToString() + "\" value=\"0\" name=\"decreaseNumHiden\"></span> <span style=\"display: inline;\" id=\"yijian" + product.Content[i].NumIid.ToString() + "\"><select onchange=\"decreaseNumChange(" + product.Content[i].NumIid.ToString() + ")\" id=\"decreaseNumSel" + product.Content[i].NumIid.ToString() + "\" name=\"decreaseNumSel\"> <option value=\"0\">多件</option> <option value=\"1\"  selected=\"selected\">一件</option> </select></span>";
                        }
                        itemtypevalue2 = " <div id=\"zheDiv" + product.Content[i].NumIid.ToString() + "\" style=\"display: none;\" ><input type=\"text\" onkeyup=\"blurValue(" + product.Content[i].NumIid.ToString() + ")\" id=\"changeZhe" + product.Content[i].NumIid.ToString() + "\" value=\"9\" name=\"discountValue\" style=\"width:30px\">折</div><div style=\"display: block;\" id=\"jianDiv" + product.Content[i].NumIid.ToString() + "\">减<input type=\"text\" onkeyup=\"blurValue(" + product.Content[i].NumIid.ToString() + ")\" id=\"changeJian" + product.Content[i].NumIid.ToString() + "\" name=\"discountValue\" style=\"width:30px\" value=\"" + discountValue + "\" >元</div>";
                        newPrice2 = (decimal.Parse(price2) -decimal.Parse(discountValue)).ToString();
                        
                    
                    }
                }
                else {
                    hdstr = " <div id=\"add" + product.Content[i].NumIid.ToString() + "\"><a href=\"javascript:addItemAction(" + product.Content[i].NumIid.ToString() + ")\">参加活动</a></div>";
                    if (yuanitemtype == "same")
                    {
                        //same：每个参加活动的宝贝设置相同促销力度,different：每个参加活动的宝贝设置不同促销力度）
                        //decreaseNum		是否优惠限制（0,1）

                        if (yuandiscountType == "PRICE")
                        {
                            itemtype2 = "<select onchange=\"changeSelect(" + product.Content[i].NumIid.ToString() + ")\" id=\"discountType" + product.Content[i].NumIid.ToString() + "\" name=\"discountType\" > <option  value=\"DISCOUNT\">打折</option><option selected=\"selected\" value=\"PRICE\">减价</option> </select>";

                            itemtypevalue2 = " <div id=\"zheDiv" + product.Content[i].NumIid.ToString() + "\" style=\"display: none;\"><input type=\"text\" onkeyup=\"blurValue(" + product.Content[i].NumIid.ToString() + ")\" id=\"changeZhe" + product.Content[i].NumIid.ToString() + "\" value=\"\" name=\"discountValue\" style=\"width:30px\">折</div><div  style=\"display:block ;\" id=\"jianDiv" + product.Content[i].NumIid.ToString() + "\">减<input type=\"text\" onkeyup=\"blurValue(" + product.Content[i].NumIid.ToString() + ")\" id=\"changeJian" + product.Content[i].NumIid.ToString() + "\" name=\"discountValue\" style=\"width:30px\" value=\"" + yuandiscountValue + "\" >元</div>";
                            if (yuandecreaseNum == "0")
                            {
                                yhCount = " <span id=\"duojian" + product.Content[i].NumIid.ToString() + "\" style=\"display: none;\">多件<input type=\"hidden\" id=\"decreaseNumHiden" + product.Content[i].NumIid.ToString() + "\" value=\"0\" name=\"decreaseNumHiden\"></span> <span  style=\"display: inline;\" id=\"yijian" + product.Content[i].NumIid.ToString() + "\"><select onchange=\"decreaseNumChange(" + product.Content[i].NumIid.ToString() + ")\" id=\"decreaseNumSel" + product.Content[i].NumIid.ToString() + "\" name=\"decreaseNumSel\"> <option selected=\"selected\" value=\"0\">多件</option> <option value=\"1\">一件</option> </select></span>";
                            }
                            else
                            {
                                yhCount = " <span id=\"duojian" + product.Content[i].NumIid.ToString() + "\" style=\"display: none;\">多件<input type=\"hidden\" id=\"decreaseNumHiden" + product.Content[i].NumIid.ToString() + "\" value=\"0\" name=\"decreaseNumHiden\"></span> <span  style=\"display: inline;\" id=\"yijian" + product.Content[i].NumIid.ToString() + "\"><select onchange=\"decreaseNumChange(" + product.Content[i].NumIid.ToString() + ")\" id=\"decreaseNumSel" + product.Content[i].NumIid.ToString() + "\" name=\"decreaseNumSel\"> <option  value=\"0\">多件</option> <option value=\"1\" selected=\"selected\">一件</option> </select></span>";
                            }

                            
                            newPrice2 = (decimal.Parse(product.Content[i].Price) - decimal.Parse(yuandiscountValue)).ToString();
                        }
                        else
                        {
                            itemtype2 = "<select onchange=\"changeSelect(" + product.Content[i].NumIid.ToString() + ")\" id=\"discountType" + product.Content[i].NumIid.ToString() + "\" name=\"discountType\" > <option selected=\"selected\" value=\"DISCOUNT\">打折</option><option value=\"PRICE\">减价</option> </select>";

                            yhCount = " <span id=\"duojian" + product.Content[i].NumIid.ToString() + "\" style=\"display: inline;\">多件<input type=\"hidden\" id=\"decreaseNumHiden" + product.Content[i].NumIid.ToString() + "\" value=\"0\" name=\"decreaseNumHiden\"></span> <span  style=\"display: none;\" id=\"yijian" + product.Content[i].NumIid.ToString() + "\"><select onchange=\"decreaseNumChange(" + product.Content[i].NumIid.ToString() + ")\" id=\"decreaseNumSel" + product.Content[i].NumIid.ToString() + "\" name=\"decreaseNumSel\"> <option selected=\"selected\" value=\"0\">多件</option> <option value=\"1\">一件</option> </select></span>";

                            itemtypevalue2 = " <div id=\"zheDiv" + product.Content[i].NumIid.ToString() + "\" style=\"display: block;\"><input type=\"text\" onkeyup=\"blurValue(" + product.Content[i].NumIid.ToString() + ")\" id=\"changeZhe" + product.Content[i].NumIid.ToString() + "\" value=\""+yuandiscountValue+"\" name=\"discountValue\" style=\"width:30px\">折</div><div  style=\"display: none;\" id=\"jianDiv" + product.Content[i].NumIid.ToString() + "\">减<input type=\"text\" onkeyup=\"blurValue(" + product.Content[i].NumIid.ToString() + ")\" id=\"changeJian" + product.Content[i].NumIid.ToString() + "\" name=\"discountValue\" style=\"width:30px\" value=\"\" >元</div>";
                            newPrice2 = (decimal.Parse(product.Content[i].Price) * decimal.Parse(yuandiscountValue) * 0.1m).ToString();
                     
                        }
                    }
                    else
                    {
 
                        itemtype2 = "<select onchange=\"changeSelect(" + product.Content[i].NumIid.ToString() + ")\" id=\"discountType" + product.Content[i].NumIid.ToString() + "\" name=\"discountType\" > <option selected=\"selected\" value=\"DISCOUNT\">打折</option><option value=\"PRICE\">减价</option> </select>";


                        yhCount = " <span id=\"duojian" + product.Content[i].NumIid.ToString() + "\" style=\"display: inline;\">多件<input type=\"hidden\" id=\"decreaseNumHiden" + product.Content[i].NumIid.ToString() + "\" value=\"0\" name=\"decreaseNumHiden\"></span> <span  style=\"display: none;\" id=\"yijian" + product.Content[i].NumIid.ToString() + "\"><select onchange=\"decreaseNumChange(" + product.Content[i].NumIid.ToString() + ")\" id=\"decreaseNumSel" + product.Content[i].NumIid.ToString() + "\" name=\"decreaseNumSel\"> <option selected=\"selected\" value=\"0\">多件</option> <option value=\"1\">一件</option> </select></span>";

                        itemtypevalue2 = " <div id=\"zheDiv" + product.Content[i].NumIid.ToString() + "\" style=\"display: block;\"><input type=\"text\" onkeyup=\"blurValue(" + product.Content[i].NumIid.ToString() + ")\" id=\"changeZhe" + product.Content[i].NumIid.ToString() + "\" value=\"9\" name=\"discountValue\" style=\"width:30px\">折</div><div  style=\"display: none;\" id=\"jianDiv" + product.Content[i].NumIid.ToString() + "\">减<input type=\"text\" onkeyup=\"blurValue(" + product.Content[i].NumIid.ToString() + ")\" id=\"changeJian" + product.Content[i].NumIid.ToString() + "\" name=\"discountValue\" style=\"width:30px\" value=\"\" >元</div>";


                        newPrice2 = product.Content[i].Price;
                    }
                }
 
                str += "<div id='div" + product.Content[i].NumIid.ToString() + "' width=\"800px\"><table width=\"800px\"><tr width=\"800px\"><td  width=\"15px\"><!--<input type=\"checkbox\" id=\"" + product.Content[i].NumIid.ToString() + "\" class=\"selector\" value=\"" + product.Content[i].NumIid.ToString() + "\" name=\"iids\">--></td><td width=\"100px\"> <a href=\"http://item.taobao.com/item.htm?id=" + product.Content[i].NumIid.ToString() + "\" target=\"_blank\"><img src=\"" + product.Content[i].PicUrl + "_80x80.jpg\" width=\"50px\" height=\"50px\" border=\"0\" /></a></td>";//图片
                str += "<td  width=\"140px\"><a   href=\"http://item.taobao.com/item.htm?id=" + product.Content[i].NumIid.ToString() + "\" target=\"_blank\">" + product.Content[i].Title + "</a> </td>";//名称
                str += "<td  width=\"70px\"> " + product.Content[i].Price + " <input type=\"hidden\" id=\"price" + product.Content[i].NumIid.ToString() + "\" name=\"price\" value=\"" + product.Content[i].Price.ToString() + "\"></td>";//商品原价
                str += "<td  width=\"70px\" id=\"newPrice" + product.Content[i].NumIid.ToString() + "\"> " + newPrice2 + "</td>";//促销价
                str += "<td  width=\"70px\"><div id=\"yhlxDiv" + product.Content[i].NumIid.ToString() + "\"> " + itemtype2 + "</div></td>";//优惠类型
                str += "<td  width=\"70px\"> <div id=\"yhhdDiv" + product.Content[i].NumIid.ToString() + "\"> " + itemtypevalue2 + "</div></td>";//优惠幅度
                str += "<td  width=\"70px\"> <div id=\"yhslDiv" + product.Content[i].NumIid.ToString() + "\">" + yhCount + "</div></td>";//优惠数量
                str += "<td  width=\"70px\"> <input type=\"text\"   id=\"Rcount" + product.Content[i].NumIid.ToString() + "\" name=\"Rcount\" style=\"width:30px\" value=\"" + Rcount + "\" ></td>";//参团人数
                str += "<td  width=\"140px\">" + hdstr + " </td>";//操作
                str += "</tr></table></div>";

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
            Response.End();
        }
    }
}