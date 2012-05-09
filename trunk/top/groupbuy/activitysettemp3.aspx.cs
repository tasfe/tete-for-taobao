using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.IO;
using System.Data;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Collections;
using System.Web.Security;
using System.Data;
using System.Threading;
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Util;

public partial class top_groupbuy_activitysettemp1 : System.Web.UI.Page
{
    public string name = string.Empty;
    public string templetid = string.Empty;
    public string bt = string.Empty;
    public string mall = string.Empty;
    public string liang = string.Empty;
    public string baoy = string.Empty;
    public string hdID = string.Empty;
    public string productid = string.Empty;
    public string price = string.Empty;
    public string zhekou = string.Empty;
    public string rcount = string.Empty;
    public string sort = string.Empty;
    public string sql = string.Empty;
    public string pid = string.Empty;
    public string pimg = string.Empty;
    public string purl = string.Empty;
    public string pname = string.Empty;
    string appkey = "12287381";
    string secret = "d3486dac8198ef01000e7bd4504601a4";
 

    protected void Page_Load(object sender, EventArgs e)
    {
        name = Request.Form["name"].ToString();
        templetid = Request.Form["templetid"].ToString();//模板ID
        bt = Request.Form["bt"].ToString();
        mall = Request.Form["mall"].ToString();
        liang = Request.Form["liang"].ToString();
        hdID = Request.Form["hdID"].ToString();//活动ID
        baoy = Request.Form["baoy"].ToString();

 

        productid = Request.Form["productid"].ToString();
        price = Request.Form["price"].ToString();
        zhekou = Request.Form["zhekou"].ToString();
        rcount = Request.Form["rcount"].ToString();
        sort = Request.Form["sort"].ToString();
        pimg = Request.Form["pimg"].ToString(); 
        pname=  Request.Form["pname"].ToString();
        Cookie cookie = new Cookie();
         string taobaoNick = cookie.getCookie("nick");
         string session = cookie.getCookie("top_sessiongroupbuy");
        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);
        //判断模板图片是否已经上传（查询本地数据库tete_shoptempletimg）,生成HTML时图片需要替换图片地址
        sql = "select * from  tete_shoptempletimg where nick='"+taobaoNick+"'";
        DataTable dts3 = utils.ExecuteDataTable(sql);

        if (dts3 != null && dts3.Rows.Count > 0)
        {
            //如果没有上传，断模板分类是否创建,
            sql = "select * from  tete_shoptempletimg where nick='" + taobaoNick + "' and templetID=" + templetid;
            dts3 = utils.ExecuteDataTable(sql);
            //如果没有上传图片
            if (dts3 == null || dts3.Rows.Count < 1)
            {
                //获取分类ID，上传图片，返回图片地址，创建本地店铺模板图片地址
                IDictionary<string, string> param = new Dictionary<string, string>();
                //创建活动
                param = new Dictionary<string, string>();
                param.Add("picture_category_name", "特特团购模板图片勿删");

                string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.picture.category.get", session, param);
                if (result.IndexOf("error_response") != -1)
                {
                    string err = new Regex(@"<sub_msg>([^<]*)</sub_msg>", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
                    if (err == "")
                    {
                        Response.Write("<b>模板创建失败，错误原因：</b><br><font color='red'>您的session已经失效，需要重新授权</font><br><a href='http://container.api.taobao.com/container?appkey=12287381&scope=promotion' target='_parent'>重新授权</a>");
                        Response.End();
                    }

                    Response.Write("<b>模板创建失败，错误原因：</b><br><font color='red'>" + err + "</font><br><a href='activityadd.aspx'>重新添加</a>");
                    Response.End();
                    return;
                }
                string categoryid = new Regex(@"<picture_category_id>([^<]*)</picture_category_id>", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
                sql = "select * from  tete_templetimg where templetID=" + templetid;
                dts3 = utils.ExecuteDataTable(sql);
                if (dts3 != null && dts3.Rows.Count > 0)
                {
                    //如上传图片，返回图片地址，创建本地店铺模板图片地址
                    for (int j = 0; j < dts3.Rows.Count; j++)
                    {
                        //上传图片
                        string newurl = TaobaoUpload(dts3.Rows[j]["url"].ToString(), "temp" + templetid.ToString() + "" + j.ToString(), long.Parse(categoryid));
                        //创建本地店铺模板图片地址
                        sql = "insert into tete_shoptempletimg ([templetID],[url] ,[taobaourl] ,[nick]) VALUES (" + templetid + ",'" + dts3.Rows[j]["url"].ToString() + "','" + newurl + "','" + taobaoNick + "')";
                        utils.ExecuteNonQuery(sql);
                    }
                }
            }

        }
        else {
            //添加分类，获取分类ID，上传图片，返回图片地址，创建本地店铺模板图片地址
            IDictionary<string, string> param = new Dictionary<string, string>();
            //创建特特图片分类
            param = new Dictionary<string, string>();
            param.Add("picture_category_name", "特特团购模板图片勿删");

            string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.picture.category.add", session, param);
            if (result.IndexOf("error_response") != -1)
            {
                string err = new Regex(@"<sub_msg>([^<]*)</sub_msg>", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
                if (err == "")
                {
                    Response.Write("<b>模板创建失败，错误原因：</b><br><font color='red'>您的session已经失效，需要重新授权</font><br><a href='http://container.api.taobao.com/container?appkey=12287381&scope=promotion' target='_parent'>重新授权</a>");
                    Response.End();
                }

                Response.Write("<b>模板创建失败，错误原因：</b><br><font color='red'>" + err + "</font><br><a href='activityadd.aspx'>重新添加</a>");
                Response.End();
                return;
            }
            string categoryid = new Regex(@"<picture_category_id>([^<]*)</picture_category_id>", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
            sql = "select * from  tete_templetimg where templetID=0 or  templetID=" + templetid;
            dts3 = utils.ExecuteDataTable(sql);
            if (dts3 != null && dts3.Rows.Count > 0)
            {
                //如上传图片，返回图片地址，创建本地店铺模板图片地址
                for (int j = 0; j < dts3.Rows.Count; j++)
                {
                    //上传图片
                    string newurl = TaobaoUpload(dts3.Rows[j]["url"].ToString(), "temp" + templetid.ToString() + "" + j.ToString(), long.Parse(categoryid));
                    //创建本地店铺模板图片地址
                    sql = "insert into tete_shoptempletimg ([templetID],[url] ,[taobaourl] ,[nick]) VALUES (" + templetid + ",'" + dts3.Rows[j]["url"].ToString() + "','" + newurl + "','" + taobaoNick + "')";
                    utils.ExecuteNonQuery(sql);
                }
            }
        }
 
        //添加店铺模板
        sql = "INSERT INTO  [tete_shoptemplet] ([templetID] ,[buttonValue] ,[scbzvalue] ,[lpbzvalue] ,[byvalue] ,[nick] ,[title] ,careteDate)   VALUES   ("+templetid+",'"+bt+"','"+mall+"','"+liang+"','"+baoy+"' ,'"+taobaoNick+"' ,'"+name+"' ,'"+ DateTime.Now.ToString()+"')";
 
         utils.ExecuteNonQuery(sql);
         DataTable dt2 = utils.ExecuteDataTable("select top 1 * from tete_shoptemplet where nick='" + taobaoNick + "' order by id desc");
         string shoptempid = "";
         if (dt2 != null && dt2.Rows.Count > 0)
         {
             shoptempid = dt2.Rows[0]["id"].ToString();
         }
 
         if (shoptempid != "")
         {
 
             //添加店铺模板列表
             for (int i = 0; i < price.Split(',').Length; i++)
             {
                 sql = "INSERT INTO  [tete_shoptempletlist] ([shoptempletID],[name],[price],[proprice],[Sort],[nick],[rcount],[ProductImg],[ProductUrl],[ProductID])  VALUES (" + shoptempid + ",'" + pname.Split(',')[i].ToString() + "','" + price.Split(',')[i].ToString() + "','" + zhekou.Split(',')[i].ToString() + "'," + sort.Split(',')[i].ToString() + ",'" + taobaoNick + "'," + rcount.Split(',')[i].ToString() + ",'" + pimg.Split(',')[i].ToString() + "','http:///item.taobao.com/item.htm?id=" + productid.Split(',')[i].ToString() + "','" + productid.Split(',')[i].ToString() + "')";
                  
                 utils.ExecuteNonQuery(sql);
   
             }
            //替换图片
             TextBox1.Text  = CreateGroupbuyHtml(shoptempid);

         }
         else
         {
             TextBox1.Text = "模板创建失败！";
         }
    }

    /// <summary>
    /// 图片上传
    /// </summary>
    /// <param name="picurl"></param>
    /// <param name="picname"></param>
    /// <param name="CategoryId"></param>
    /// <returns></returns>
    public string TaobaoUpload(string picurl, string picname, long CategoryId)
    {
        Cookie cookie = new Cookie(); 
        string session = cookie.getCookie("top_sessiongroupbuy");
        TopXmlRestClient clientaa = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", appkey, secret);
        PictureUploadRequest request = new PictureUploadRequest();

        string filepath = Server.MapPath("images/" + picurl);

        request.Img = new FileItem(filepath, File.ReadAllBytes(filepath));
        request.ImageInputTitle = picurl;
        request.PictureCategoryId = CategoryId;
        request.Title = picname;

        clientaa.PictureUpload(request, session);


        PictureGetRequest request1 = new PictureGetRequest();
        request1.Title = picname;
        string path = string.Empty;
        path = clientaa.PictureGet(request1, session).Content[0].PicturePath;

        return path;
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
        if (dt == null||dt.Rows.Count<1)
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
            string purl =  dt.Rows[i]["ProductUrl"].ToString();
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
        if (tid == "1" || tid == "2")
        {
            //如果是团购模板需要替换团购HTML
            
            sqlstr = "select * from  tete_shoptempletimg where nick='" + taobaoNick + "' and url='"+btvalue+".png'";
            DataTable dtr2 = utils.ExecuteDataTable(sqlstr);
            if (dtr2 != null && dtr2.Rows.Count > 0)
            {
                //参团按钮替换
                str = str.Replace("http://groupbuy.7fshop.com/top/groupbuy/images/temp2ct.png", dtr2.Rows[0]["taobaourl"].ToString());
            }
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
    public string tuanhtmlReplace(string str, string name, string price, string proprice, string rcount, string producturl, string productimg,string id,string tid)
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
    #region  TOP API POST 请求

    /// <summary> 
    /// 给TOP请求签名 API v2.0 
    /// </summary> 
    /// <param name="parameters">所有字符型的TOP请求参数</param> 
    /// <param name="secret">签名密钥</param> 
    /// <returns>签名</returns> 
    protected static string CreateSign(IDictionary<string, string> parameters, string secret)
    {
        parameters.Remove("sign");
        IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters);
        IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();
        StringBuilder query = new StringBuilder(secret);
        while (dem.MoveNext())
        {
            string key = dem.Current.Key;
            string value = dem.Current.Value;
            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
            {
                query.Append(key).Append(value);
            }
        }
        query.Append(secret);
        MD5 md5 = MD5.Create();
        byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(query.ToString()));
        StringBuilder result = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
        {
            string hex = bytes[i].ToString("X");
            if (hex.Length == 1)
            {
                result.Append("0");
            }
            result.Append(hex);
        }
        return result.ToString();
    }
    /// <summary> 
    /// 组装普通文本请求参数。 
    /// </summary> 
    /// <param name="parameters">Key-Value形式请求参数字典</param> 
    /// <returns>URL编码后的请求数据</returns> 
    protected static string PostData(IDictionary<string, string> parameters)
    {
        StringBuilder postData = new StringBuilder();
        bool hasParam = false;
        IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
        while (dem.MoveNext())
        {
            string name = dem.Current.Key;
            string value = dem.Current.Value;
            // 忽略参数名或参数值为空的参数 
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
            {
                if (hasParam)
                {
                    postData.Append("&");
                }
                postData.Append(name);
                postData.Append("=");
                postData.Append(Uri.EscapeDataString(value));
                hasParam = true;
            }
        }
        return postData.ToString();
    }
    /// <summary> 
    /// TOP API POST 请求 
    /// </summary> 
    /// <param name="url">请求容器URL</param> 
    /// <param name="appkey">AppKey</param> 
    /// <param name="appSecret">AppSecret</param> 
    /// <param name="method">API接口方法名</param> 
    /// <param name="session">调用私有的sessionkey</param> 
    /// <param name="param">请求参数</param> 
    /// <returns>返回字符串</returns> 
    public static string Post(string url, string appkey, string appSecret, string method, string session,
    IDictionary<string, string> param)
    {
        #region -----API系统参数----
        param.Add("app_key", appkey);
        param.Add("method", method);
        param.Add("session", session);
        param.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        param.Add("format", "xml");
        param.Add("v", "2.0");
        param.Add("sign_method", "md5");
        param.Add("sign", CreateSign(param, appSecret));
        #endregion
        string result = string.Empty;
        #region ---- 完成 HTTP POST 请求----
        System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
        req.Method = "POST";
        req.KeepAlive = true;
        req.Timeout = 300000;
        req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
        byte[] postData = Encoding.UTF8.GetBytes(PostData(param));
        Stream reqStream = req.GetRequestStream();
        reqStream.Write(postData, 0, postData.Length);
        reqStream.Close();
        System.Net.HttpWebResponse rsp = (System.Net.HttpWebResponse)req.GetResponse();
        Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
        Stream stream = null;
        StreamReader reader = null;
        stream = rsp.GetResponseStream();
        reader = new StreamReader(stream, encoding);
        result = reader.ReadToEnd();
        if (reader != null) reader.Close();
        if (stream != null) stream.Close();
        if (rsp != null) rsp.Close();
        #endregion
        return Regex.Replace(result, @"[\x00-\x08\x0b-\x0c\x0e-\x1f]", "");
    }

    #endregion
}