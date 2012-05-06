﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.IO;
using System.Data;

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
 
        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);
 
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
        
             TextBox1.Text  = CreateGroupbuyHtml(shoptempid);
         }
         else
         {
             TextBox1.Text = "模板创建失败！";
         }
    }

    /// <summary>
    /// 
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
        string sql = "select tete_shoptempletlist.*,templetID,[buttonValue],[scbzvalue],[lpbzvalue],[byvalue],[title],[careteDate],[Sort] from tete_shoptempletlist left join   tete_shoptemplet on tete_shoptempletlist.shoptempletID=tete_shoptemplet.ID  WHERE tete_shoptemplet.templetID = '" + id + "'";
       
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt == null)
        {
            return "";
        }
        string templatehtmlUrl = "tpl/style1.html";//默认模板
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
                templatehtmlUrl = "tpl/style3.html";
            }

 
        string html = File.ReadAllText(Server.MapPath(templatehtmlUrl));
        string smailtempStr = string.Empty;//小模板
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string purl =  dt.Rows[i]["ProductUrl"].ToString();
            if (i == 0)
            {
                str = html;
                str = str.Replace("{name}", dt.Rows[i]["name"].ToString());
                str = str.Replace("{oldprice}", dt.Rows[i]["price"].ToString());
                str = str.Replace("{zhekou}", Math.Round((decimal.Parse(dt.Rows[i]["price"].ToString()) - decimal.Parse(dt.Rows[i]["proprice"].ToString())) / decimal.Parse(dt.Rows[i]["price"].ToString()) * 10, 1).ToString());
                str = str.Replace("{leftprice}", (decimal.Parse(dt.Rows[i]["price"].ToString()) - decimal.Parse(dt.Rows[i]["proprice"].ToString())).ToString().Split('.')[0]);
                str = str.Replace("{rightprice}", (decimal.Parse(dt.Rows[i]["price"].ToString()) - decimal.Parse(dt.Rows[i]["proprice"].ToString())).ToString().Split('.')[1]);
                str = str.Replace("{newprice}", (decimal.Parse(dt.Rows[i]["price"].ToString()) - decimal.Parse(dt.Rows[i]["proprice"].ToString())).ToString());
                str = str.Replace("{buycount}", dt.Rows[i]["rcount"].ToString());
                str = str.Replace("{producturl}", dt.Rows[i]["producturl"].ToString());
                str = str.Replace("{productimg}", dt.Rows[i]["productimg"].ToString());
                str = str.Replace("{id}", id);
                str = str.Replace("'", "''");
                //如果模板是第三套模板，追加第一个活动HTML
                if (templatehtmlUrl == "tpl/style3.html")
                {
                    html = File.ReadAllText(Server.MapPath(template2htmlUrl));
                    smailtempStr += html;
                    smailtempStr = smailtempStr.Replace("{name}", dt.Rows[i]["name"].ToString());
                    smailtempStr = smailtempStr.Replace("{oldprice}", dt.Rows[i]["productprice"].ToString());
                    smailtempStr = smailtempStr.Replace("{zhekou}", Math.Round((decimal.Parse(dt.Rows[i]["productprice"].ToString()) - decimal.Parse(dt.Rows[i]["zhekou"].ToString())) / decimal.Parse(dt.Rows[i]["productprice"].ToString()) * 10, 1).ToString());
                    smailtempStr = smailtempStr.Replace("{leftprice}", (decimal.Parse(dt.Rows[i]["productprice"].ToString()) - decimal.Parse(dt.Rows[i]["zhekou"].ToString())).ToString().Split('.')[0]);
                    smailtempStr = smailtempStr.Replace("{rightprice}", (decimal.Parse(dt.Rows[i]["productprice"].ToString()) - decimal.Parse(dt.Rows[i]["zhekou"].ToString())).ToString().Split('.')[1]);
                    smailtempStr = smailtempStr.Replace("{newprice}", dt.Rows[i]["zhekou"].ToString());
                    smailtempStr = smailtempStr.Replace("{buycount}", dt.Rows[i]["buycount"].ToString());
                    smailtempStr = smailtempStr.Replace("{producturl}", dt.Rows[i]["producturl"].ToString());
                    smailtempStr = smailtempStr.Replace("{productimg}", dt.Rows[i]["productimg"].ToString());
                    smailtempStr = smailtempStr.Replace("{id}", id);
                    smailtempStr = smailtempStr.Replace("'", "''");
                }
            }
            else
            {
                //是多商品团购模板
                if (dt.Rows[i]["templetID"].ToString() == "2" || dt.Rows[i]["templetID"].ToString() == "3")
                {
                    html = File.ReadAllText(Server.MapPath(template2htmlUrl));
                    smailtempStr += html;
                    smailtempStr = smailtempStr.Replace("{name}", dt.Rows[i]["name"].ToString());
                    smailtempStr = smailtempStr.Replace("{oldprice}", dt.Rows[i]["productprice"].ToString());
                    smailtempStr = smailtempStr.Replace("{zhekou}", Math.Round((decimal.Parse(dt.Rows[i]["productprice"].ToString()) - decimal.Parse(dt.Rows[i]["zhekou"].ToString())) / decimal.Parse(dt.Rows[i]["productprice"].ToString()) * 10, 1).ToString());
                    smailtempStr = smailtempStr.Replace("{leftprice}", (decimal.Parse(dt.Rows[i]["productprice"].ToString()) - decimal.Parse(dt.Rows[i]["zhekou"].ToString())).ToString().Split('.')[0]);
                    smailtempStr = smailtempStr.Replace("{rightprice}", (decimal.Parse(dt.Rows[i]["productprice"].ToString()) - decimal.Parse(dt.Rows[i]["zhekou"].ToString())).ToString().Split('.')[1]);
                    smailtempStr = smailtempStr.Replace("{newprice}", dt.Rows[i]["zhekou"].ToString());
                    smailtempStr = smailtempStr.Replace("{buycount}", dt.Rows[i]["buycount"].ToString());
                    smailtempStr = smailtempStr.Replace("{producturl}", dt.Rows[i]["producturl"].ToString());
                    smailtempStr = smailtempStr.Replace("{productimg}", dt.Rows[i]["productimg"].ToString());
                    smailtempStr = smailtempStr.Replace("{id}", id);
                    smailtempStr = smailtempStr.Replace("'", "''");
                }

            }
        }
        str = str.Replace("{productlist}", smailtempStr);//一大三小模板，商品列表替换
        return str;
    }
}