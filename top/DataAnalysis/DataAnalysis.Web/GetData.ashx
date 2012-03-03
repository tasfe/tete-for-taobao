<%@ WebHandler Language="C#" Class="GetData" %>

using System;
using System.Web;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

public class GetData : IHttpHandler {

    public void ProcessRequest(HttpContext context)
    {
        //插入信息
        TopVisitInfo info = CreateVisitInfo(context);
        if (string.IsNullOrEmpty(info.VisitUrl)) return;
        OperatUrl(info.VisitUrl, context, info);
        InsertInfo(info);
        
        //context.Response.Redirect("http://groupbuy.7fshop.com/top/groupbuy/groupbuy_imag.aspx?id=" + context.Request.QueryString["id"].ToString() + "&typ=" + context.Request.QueryString["typ"].ToString() + "&isok=1");

        context.Response.ContentType = "image/jpeg";
        context.Response.Clear();
        context.Response.BufferOutput = true;


        //if (SetCookie(context, info.VisitIP))
        //{

        //}
        //else
        //{

        //}

        System.Drawing.Image img = System.Drawing.Image.FromFile(context.Server.MapPath("~/Images/nickimgs/" + info.VisitShopId + ".jpg"));
        img.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
        context.Response.End();
        ////context.Response.ContentType = "text/plain";
        ////context.Response.Write("Hello World");
    }

    private static bool SetCookie(HttpContext context,string ip)
    {
        string serverIp = "";
        if (context.Cache[ip] != null)
        {
            serverIp = context.Cache[ip].ToString();
        }
        
        if (context.Request.Cookies[ip] == null || context.Request.Cookies[ip].Value != serverIp)
        {
            string  cvalue  = Guid.NewGuid().ToString();

            DateTime now = DateTime.Now.AddDays(1);
            DateTime useDate = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);

            //服务端插入缓存
            context.Cache.Insert(ip, cvalue, null, useDate, System.Web.Caching.Cache.NoSlidingExpiration);
                      
            //插入cookie
            HttpCookie cookie = new HttpCookie(ip, cvalue);
            cookie.Expires = useDate;
            //cookie.Domain = ".test.7fshop.com";
            context.Response.Cookies.Add(cookie);
            
            return false;
        }
        
        return true;
    }

    private static void InsertInfo(TopVisitInfo info)
    {
        VisitService visitDal = new VisitService();

        //测试用
        string nickNo = DataHelper.Encrypt(info.VisitShopId);
        visitDal.CreateTable(nickNo);

        visitDal.InsertVisitInfo(info, nickNo);
    }

    private static TopVisitInfo CreateVisitInfo(HttpContext context)
    {
        DataHelper dataHelper = new DataHelper(context);
        
        TopVisitInfo info = new TopVisitInfo();
        info.VisitID = Guid.NewGuid();
        info.VisitIP = dataHelper.GetIPAddress();
        info.VisitTime = dataHelper.GetVisitTime();
        info.VisitUrl =dataHelper.GetUrl();
        info.VisitUserAgent = dataHelper.GetUserAgent();
        info.VisitBrower = dataHelper.GetBrower();
        info.VisitOSLanguage = dataHelper.GetOSLanguage();
        info.VisitShopId = DataHelper.Encrypt(HttpUtility.UrlDecode(context.Request.QueryString["nick"]));  // "234543534"
        
        info.GoodsId = "";
        info.GoodsClassId = "";
        
        return info;
    }

    private static void OperatUrl(string url, HttpContext context, TopVisitInfo info)
    {
        if (url.Contains("?id=") || url.Contains("&id="))
        {
            InitGoodsId(url, context, info);
        }
        if (url.Contains("?scid=") && url.Contains("?scid="))
        {
            InitGoodsClassId(url, context, info);
        }

    }

    private static void InitGoodsId(string url, HttpContext context, TopVisitInfo vinfo)
    {
        Regex regex = new Regex("(\\?id=\\d+)|(&id=\\d+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        string idstr = regex.Match(url).Value;
        string pid = idstr.Replace("?id=", "").Replace("&id=", "");
        vinfo.GoodsId = pid;
    }

    private static void InitGoodsClassId(string url, HttpContext context, TopVisitInfo vinfo)
    {
        Regex regex = new Regex("(\\?scid=\\d+)|(&id=\\d+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        string idstr = regex.Match(url).Value;
        string cid = idstr.Replace("?scid=", "").Replace("&scid=", "");
        vinfo.GoodsClassId = cid;
    }
    
    public bool IsReusable {
        get {
            return false;
        }
    }

}