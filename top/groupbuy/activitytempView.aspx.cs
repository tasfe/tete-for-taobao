using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;
using System.IO;

public partial class top_groupbuy_activitytempView : System.Web.UI.Page
{
    public string html = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["ytid"] != null)
        {
            html = CreateGroupbuyHtml(Request.QueryString["ytid"].ToString());
            div1.Style.Add("display", "none");

        }
        if (Request.QueryString["tid"] != null)
        {
            html = "";
            div1.Style.Add("display", "");
            TextBox1.Text = CreateGroupbuyHtml(Request.QueryString["tid"].ToString());
        }
    }

    /// <summary>
    /// 生成HTML
    /// </summary>
    /// <param name="id">店铺模板ID</param>
    /// <returns></returns>
    private string CreateGroupbuyHtml(string id)
    {
        if (id == "")
        {
            return "";
        }
        string str = string.Empty;
        string sql = "select tete_shoptempletlist.*,templetID,[buttonValue],[scbzvalue],[lpbzvalue],[byvalue],[title],[careteDate],[Sort] from tete_shoptempletlist left join   tete_shoptemplet on tete_shoptempletlist.shoptempletID=tete_shoptemplet.ID  WHERE tete_shoptempletlist.shoptempletID = " + id;

        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt == null || dt.Rows.Count < 1)
        {
            Response.Write("数据为空");
            Response.End();
        }
        string templatehtmlUrl = "tpl/stylenew1.html";//默认模板
        string template2htmlUrl = "tpl/stylenew2-1.html";//第二套模板（一大三小） 小模板  (团购模板第三套和第二套)

        //模板生成需要在好好考虑一下。。。。。。。。。。。。。。。。。。。。。。。。。。。。
        //是多商品团购模板
        if (dt.Rows[0]["templetID"].ToString() == "2")
        {
            //第二套模板（一大三小）
            templatehtmlUrl = "tpl/stylenew2.html";
        }
        //是多商品团购模板
        if (dt.Rows[0]["templetID"].ToString() == "3")
        {
            //第三套模板（一排三列）
            templatehtmlUrl = "tpl/stylenew3.html";
        }


        string html = File.ReadAllText(Server.MapPath(templatehtmlUrl));
        string smailtempStr = string.Empty;//小模板
        string btvalue = dt.Rows[0]["buttonValue"].ToString();
        string scbzvalue = dt.Rows[0]["scbzvalue"].ToString();
        string lpbzvalue = dt.Rows[0]["lpbzvalue"].ToString();
        string byvalue = dt.Rows[0]["byvalue"].ToString();

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string purl = dt.Rows[i]["ProductUrl"].ToString();
            if (i == 0)
            {
                str = html;
                str = tuanhtmlReplace(str, dt.Rows[i]["name"].ToString(), dt.Rows[i]["price"].ToString(), dt.Rows[i]["proprice"].ToString(), dt.Rows[i]["rcount"].ToString(), dt.Rows[i]["producturl"].ToString(), dt.Rows[i]["productimg"].ToString(), id, dt.Rows[0]["templetID"].ToString());

                //如果模板是第三套模板，追加第一个活动HTML
                if (templatehtmlUrl == "tpl/stylenew3.html")
                {
                    html = File.ReadAllText(Server.MapPath(template2htmlUrl));
                    smailtempStr += html;
                    smailtempStr = cxhtmlReplace(smailtempStr, dt.Rows[i]["name"].ToString(), dt.Rows[i]["price"].ToString(), dt.Rows[i]["proprice"].ToString(), dt.Rows[i]["rcount"].ToString(), dt.Rows[i]["producturl"].ToString(), dt.Rows[i]["productimg"].ToString(), id, dt.Rows[0]["templetID"].ToString());
                }
            }
            else
            {
                //是多商品团购模板
                if (dt.Rows[i]["templetID"].ToString() == "2" || dt.Rows[i]["templetID"].ToString() == "3")
                {
                    html = File.ReadAllText(Server.MapPath(template2htmlUrl));
                    smailtempStr += html;
                    smailtempStr = cxhtmlReplace(smailtempStr, dt.Rows[i]["name"].ToString(), dt.Rows[i]["price"].ToString(), dt.Rows[i]["proprice"].ToString(), dt.Rows[i]["rcount"].ToString(), dt.Rows[i]["producturl"].ToString(), dt.Rows[i]["productimg"].ToString(), id, dt.Rows[0]["templetID"].ToString());
                }

                if (dt.Rows[i]["templetID"].ToString() == "1")
                {
                    html = File.ReadAllText(Server.MapPath(templatehtmlUrl));
                    str += html;
                    str = tuanhtmlReplace(str, dt.Rows[i]["name"].ToString(), dt.Rows[i]["price"].ToString(), dt.Rows[i]["proprice"].ToString(), dt.Rows[i]["rcount"].ToString(), dt.Rows[i]["producturl"].ToString(), dt.Rows[i]["productimg"].ToString(), id, dt.Rows[0]["templetID"].ToString());
                }

            }
        }
        str = str.Replace("{productlist}", smailtempStr);//一大三小模板，商品列表替换
        //模板图片替换
        str = tempImgreplace(str, dt.Rows[0]["templetID"].ToString(), btvalue, scbzvalue, lpbzvalue, byvalue);

        return str;
    }


    /// <summary>
    /// 模板图片替换
    /// </summary>
    /// <param name="str">html</param>
    /// <param name="tid">模板ID</param>
    /// <returns></returns>
    public string tempImgreplace(string str, string tid, string btvalue, string scbzvalue, string lpbzvalue, string byvalue)
    {
        Cookie cookie = new Cookie();
        string taobaoNick = cookie.getCookie("nick");
        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);
        string sqlstr = "select * from  tete_shoptempletimg where nick='" + taobaoNick + "' and templetID=" + tid;
        DataTable dt = utils.ExecuteDataTable(sqlstr);
        if (dt == null && dt.Rows.Count < 1)
        {
            //如果为空返回HTML
            return str;
        }
        sqlstr = "select * from  tete_shoptempletimg where nick='" + taobaoNick + "' and url='" + btvalue + ".png'";
        DataTable dtr2 = utils.ExecuteDataTable(sqlstr);
        if (dtr2 != null && dtr2.Rows.Count > 0)
        {
            //参团按钮替换
            str = str.Replace("http://groupbuy.7fshop.com/top/groupbuy/images/temp2ct.png", dtr2.Rows[0]["taobaourl"].ToString());
            str = str.Replace("http://groupbuy.7fshop.com/top/groupbuy/images/temp2ct2.png", dtr2.Rows[0]["taobaourl"].ToString());
        }
        if (tid == "1" || tid == "2")
        {
            //如果是团购模板需要替换团购HTML
            //商城图片替换
            if (scbzvalue == "1")
            {
                sqlstr = "select * from  tete_shoptempletimg where nick='" + taobaoNick + "' and url='mall_protection_icon.png'";
                dtr2 = utils.ExecuteDataTable(sqlstr);
                if (dtr2 != null && dtr2.Rows.Count > 0)
                {
                    str = str.Replace("http://groupbuy.7fshop.com/top/groupbuy/images/mall_protection_icon.png", dtr2.Rows[0]["taobaourl"].ToString());
                }
            }
            else
            {
                str = str.Replace("<img src=\"http://groupbuy.7fshop.com/top/groupbuy/images/mall_protection_icon.png\" />", "");
            }
            //良品图片替换
            if (lpbzvalue == "1")
            {
                sqlstr = "select * from  tete_shoptempletimg where nick='" + taobaoNick + "' and url='lp_protection_icon.png'";
                dtr2 = utils.ExecuteDataTable(sqlstr);
                if (dtr2 != null && dtr2.Rows.Count > 0)
                {
                    str = str.Replace("http://groupbuy.7fshop.com/top/groupbuy/images/lp_protection_icon.png", dtr2.Rows[0]["taobaourl"].ToString());
                }
            }
            else
            {
                str = str.Replace("<img src=\"http://groupbuy.7fshop.com/top/groupbuy/images/lp_protection_icon.png\" />", "");
            }
            //包邮标准替换
            if (byvalue != "")
            {
                string str1 = string.Empty;
                string str2 = string.Empty;
                string str3 = string.Empty;
                string str4 = string.Empty;
                string str5 = string.Empty;
                string str6 = string.Empty;
                string str7 = string.Empty;
                for (int k = 0; k < byvalue.Split(',').Length; k++)
                {
                    #region
                    if (k == 0)
                    {
                        if (byvalue.Split(',')[k].ToString() == "0")
                        {
                            str1 = "0";
                        }
                        if (byvalue.Split(',')[k].ToString() == "1")
                        {
                            str1 = "-33";
                        }
                        if (byvalue.Split(',')[k].ToString() == "2")
                        {
                            str1 = "-66";
                        }
                        if (byvalue.Split(',')[k].ToString() == "3")
                        {
                            str1 = "-99";
                        }
                        if (byvalue.Split(',')[k].ToString() == "4")
                        {
                            str1 = "-132";
                        }
                        if (byvalue.Split(',')[k].ToString() == "5")
                        {
                            str1 = "-165";
                        }
                        if (byvalue.Split(',')[k].ToString() == "6")
                        {
                            str1 = "-198";
                        }

                    }
                    if (k == 1)
                    {
                        if (byvalue.Split(',')[k].ToString() == "0")
                        {
                            str2 = "0";
                        }
                        if (byvalue.Split(',')[k].ToString() == "1")
                        {
                            str2 = "-33";
                        }
                        if (byvalue.Split(',')[k].ToString() == "2")
                        {
                            str2 = "-66";
                        }
                        if (byvalue.Split(',')[k].ToString() == "3")
                        {
                            str2 = "-99";
                        }
                        if (byvalue.Split(',')[k].ToString() == "4")
                        {
                            str2 = "-132";
                        }
                        if (byvalue.Split(',')[k].ToString() == "5")
                        {
                            str2 = "-165";
                        }
                        if (byvalue.Split(',')[k].ToString() == "6")
                        {
                            str2 = "-198";
                        }
                    }
                    if (k == 2)
                    {
                        if (byvalue.Split(',')[k].ToString() == "0")
                        {
                            str3 = "0";
                        }
                        if (byvalue.Split(',')[k].ToString() == "1")
                        {
                            str3 = "-33";
                        }
                        if (byvalue.Split(',')[k].ToString() == "2")
                        {
                            str3 = "-66";
                        }
                        if (byvalue.Split(',')[k].ToString() == "3")
                        {
                            str3 = "-99";
                        }
                        if (byvalue.Split(',')[k].ToString() == "4")
                        {
                            str3 = "-132";
                        }
                        if (byvalue.Split(',')[k].ToString() == "5")
                        {
                            str3 = "-165";
                        }
                        if (byvalue.Split(',')[k].ToString() == "6")
                        {
                            str3 = "-198";
                        }
                    }
                    if (k == 3)
                    {
                        if (byvalue.Split(',')[k].ToString() == "0")
                        {
                            str4 = "0";
                        }
                        if (byvalue.Split(',')[k].ToString() == "1")
                        {
                            str4 = "-33";
                        }
                        if (byvalue.Split(',')[k].ToString() == "2")
                        {
                            str4 = "-66";
                        }
                        if (byvalue.Split(',')[k].ToString() == "3")
                        {
                            str4 = "-99";
                        }
                        if (byvalue.Split(',')[k].ToString() == "4")
                        {
                            str4 = "-132";
                        }
                        if (byvalue.Split(',')[k].ToString() == "5")
                        {
                            str4 = "-165";
                        }
                        if (byvalue.Split(',')[k].ToString() == "6")
                        {
                            str4 = "-198";
                        }
                    }
                    if (k == 4)
                    {
                        if (byvalue.Split(',')[k].ToString() == "0")
                        {
                            str5 = "0";
                        }
                        if (byvalue.Split(',')[k].ToString() == "1")
                        {
                            str5 = "-33";
                        }
                        if (byvalue.Split(',')[k].ToString() == "2")
                        {
                            str5 = "-66";
                        }
                        if (byvalue.Split(',')[k].ToString() == "3")
                        {
                            str5 = "-99";
                        }
                        if (byvalue.Split(',')[k].ToString() == "4")
                        {
                            str5 = "-132";
                        }
                        if (byvalue.Split(',')[k].ToString() == "5")
                        {
                            str5 = "-165";
                        }
                        if (byvalue.Split(',')[k].ToString() == "6")
                        {
                            str5 = "-198";
                        }
                    }
                    if (k == 5)
                    {
                        if (byvalue.Split(',')[k].ToString() == "0")
                        {
                            str6 = "0";
                        }
                        if (byvalue.Split(',')[k].ToString() == "1")
                        {
                            str6 = "-33";
                        }
                        if (byvalue.Split(',')[k].ToString() == "2")
                        {
                            str6 = "-66";
                        }
                        if (byvalue.Split(',')[k].ToString() == "3")
                        {
                            str6 = "-99";
                        }
                        if (byvalue.Split(',')[k].ToString() == "4")
                        {
                            str6 = "-132";
                        }
                        if (byvalue.Split(',')[k].ToString() == "5")
                        {
                            str6 = "-165";
                        }
                        if (byvalue.Split(',')[k].ToString() == "6")
                        {
                            str6 = "-198";
                        }
                    }
                    if (k == 6)
                    {
                        if (byvalue.Split(',')[k].ToString() == "0")
                        {
                            str7 = "0";
                        }
                        if (byvalue.Split(',')[k].ToString() == "1")
                        {
                            str7 = "-33";
                        }
                        if (byvalue.Split(',')[k].ToString() == "2")
                        {
                            str7 = "-66";
                        }
                        if (byvalue.Split(',')[k].ToString() == "3")
                        {
                            str7 = "-99";
                        }
                        if (byvalue.Split(',')[k].ToString() == "4")
                        {
                            str7 = "-132";
                        }
                        if (byvalue.Split(',')[k].ToString() == "5")
                        {
                            str7 = "-165";
                        }
                        if (byvalue.Split(',')[k].ToString() == "6")
                        {
                            str7 = "-198";
                        }
                    }
                    #endregion
                }

                string byhtml = string.Empty;
                #region
                if (byvalue.Split(',').Length == 1)
                {
                    byhtml = "<tr><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str1 + "px; padding: 0pt 0pt 0pt 25px;\"> </td></tr>";
                }
                if (byvalue.Split(',').Length == 2)
                {
                    byhtml = "<tr><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str1 + "px; padding: 0pt 0pt 0pt 25px;\"> </td><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str2 + "px; padding: 0pt 0pt 0pt 25px;\"> </td></tr>";
                }
                if (byvalue.Split(',').Length == 3)
                {
                    byhtml = "<tr><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str1 + "px; padding: 0pt 0pt 0pt 25px;\"> </td><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str2 + "px; padding: 0pt 0pt 0pt 25px;\"> </td></tr><tr><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str3 + "px; padding: 0pt 0pt 0pt 25px;\"> </td></tr>";
                }
                if (byvalue.Split(',').Length == 4)
                {
                    byhtml = "<tr><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str1 + "px; padding: 0pt 0pt 0pt 25px;\"> </td><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str2 + "px; padding: 0pt 0pt 0pt 25px;\"> </td></tr><tr><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str3 + "px; padding: 0pt 0pt 0pt 25px;\"> </td><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str4 + "px; padding: 0pt 0pt 0pt 25px;\"> </td></tr>";
                }
                if (byvalue.Split(',').Length == 5)
                {
                    byhtml = "<tr><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str1 + "px; padding: 0pt 0pt 0pt 25px;\"> </td><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str2 + "px; padding: 0pt 0pt 0pt 25px;\"> </td></tr><tr><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str3 + "px; padding: 0pt 0pt 0pt 25px;\"> </td><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str4 + "px; padding: 0pt 0pt 0pt 25px;\"> </td></tr><tr><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str5 + "px; padding: 0pt 0pt 0pt 25px;\"> </td></tr>";
                }
                if (byvalue.Split(',').Length == 6)
                {
                    byhtml = "<tr><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str1 + "px; padding: 0pt 0pt 0pt 25px;\"> </td><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str2 + "px; padding: 0pt 0pt 0pt 25px;\"> </td></tr><tr><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str3 + "px; padding: 0pt 0pt 0pt 25px;\"> </td><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str4 + "px; padding: 0pt 0pt 0pt 25px;\"> </td></tr><tr><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str5 + "px; padding: 0pt 0pt 0pt 25px;\"> </td><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str6 + "px; padding: 0pt 0pt 0pt 25px;\"> </td></tr>";
                }
                if (byvalue.Split(',').Length == 7)
                {
                    byhtml = "<tr><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str1 + "px; padding: 0pt 0pt 0pt 25px;\"> </td><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str2 + "px; padding: 0pt 0pt 0pt 25px;\"> </td></tr><tr><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str3 + "px; padding: 0pt 0pt 0pt 25px;\"> </td><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str4 + "px; padding: 0pt 0pt 0pt 25px;\"> </td></tr><tr><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str5 + "px; padding: 0pt 0pt 0pt 25px;\"> </td><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str6 + "px; padding: 0pt 0pt 0pt 25px;\"> </td></tr><tr><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str7 + "px; padding: 0pt 0pt 0pt 25px;\"> </td></tr>";
                }

                #endregion
                str = str.Replace("{baoyou}", byhtml);
            }
            else
            {
                str = str.Replace("{baoyou}", "");
            }


        }
        //替换数据库模板url
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            //讲模板本地图片替换成淘宝url
            str = str.Replace("http://groupbuy.7fshop.com/top/groupbuy/images/" + dt.Rows[i]["url"].ToString(), dt.Rows[i]["taobaourl"].ToString());
        }
        return str;
    }

    /// <summary>
    /// 团购模板HTML替换
    /// </summary>
    /// <param name="str">html</param>
    /// <param name="name">商品名</param>
    /// <param name="price">价格</param>
    /// <param name="proprice">促销价格</param>
    /// <param name="rcount">参团人数</param>
    /// <param name="producturl">宝贝地址</param>
    /// <param name="productimg">图片地址</param>
    /// <param name="id"></param>
    /// <param name="tid">模板名</param>
    /// <returns></returns>
    public string tuanhtmlReplace(string str, string name, string price, string proprice, string rcount, string producturl, string productimg, string id, string tid)
    {

        str = str.Replace("{name}", name);
        str = str.Replace("{oldprice}", price);
        str = str.Replace("{zhekou}", Math.Round(decimal.Parse(proprice) / decimal.Parse(price) * 10, 1).ToString());
        str = str.Replace("{leftprice}", proprice.Split('.')[0]);
        str = str.Replace("{rightprice}", proprice.Split('.')[1]);
        str = str.Replace("{newprice}", (decimal.Parse(price) - decimal.Parse(proprice)).ToString());
        str = str.Replace("{buycount}", rcount);
        str = str.Replace("{producturl}", producturl);
        str = str.Replace("{productimg}", productimg);
        str = str.Replace("{id}", id);
        str = str.Replace("'", "''");
        return str;
    }

    /// <summary>
    /// 促销模板HTML替换
    /// </summary>
    /// <param name="smailtempStr">html</param>
    /// <param name="name">商品名</param>
    /// <param name="price">价格</param>
    /// <param name="proprice">促销价格</param>
    /// <param name="rcount">参团人数</param>
    /// <param name="producturl">宝贝地址</param>
    /// <param name="productimg">图片地址</param>
    /// <param name="id"></param>
    /// <param name="tid">模板名</param>
    /// <returns></returns>
    public string cxhtmlReplace(string smailtempStr, string name, string price, string proprice, string rcount, string producturl, string productimg, string id, string tid)
    {
        smailtempStr = smailtempStr.Replace("{name}", name);
        smailtempStr = smailtempStr.Replace("{oldprice}", price);
        smailtempStr = smailtempStr.Replace("{zhekou}", Math.Round(decimal.Parse(proprice) / decimal.Parse(price) * 10, 1).ToString());
        smailtempStr = smailtempStr.Replace("{leftprice}", proprice.Split('.')[0]);
        smailtempStr = smailtempStr.Replace("{rightprice}", proprice.Split('.')[1]);
        smailtempStr = smailtempStr.Replace("{newprice}", (decimal.Parse(price) - decimal.Parse(proprice)).ToString());
        smailtempStr = smailtempStr.Replace("{buycount}", rcount);
        smailtempStr = smailtempStr.Replace("{producturl}", producturl);
        smailtempStr = smailtempStr.Replace("{productimg}", productimg);
        smailtempStr = smailtempStr.Replace("{id}", id);
        smailtempStr = smailtempStr.Replace("'", "''");
        return smailtempStr;
    }
}