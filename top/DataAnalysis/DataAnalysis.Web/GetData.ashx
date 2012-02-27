<%@ WebHandler Language="C#" Class="GetData" %>

using System;
using System.Web;

public class GetData : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "image/gif";
        context.Response.Clear();
        context.Response.BufferOutput = true;
        
        //插入信息
        TopVisitInfo info = CreateVisitInfo(context);
        InsertInfo(info);

        System.Drawing.Image img = System.Drawing.Image.FromFile(context.Server.MapPath("~/Images/2.gif"));
        img.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Gif);
        context.Response.End();
        //context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");
    }

    private void InsertInfo(TopVisitInfo info)
    {
        VisitService visitDal = new VisitService();
        visitDal.InsertVisitInfo(info);
    }

    private TopVisitInfo CreateVisitInfo(HttpContext context)
    {
        DataHelper dataHelper = new DataHelper(context);
        TopVisitInfo info = new TopVisitInfo();
        info.VisitID = Guid.NewGuid();
        info.VisitIP = dataHelper.GetIPAddress();
        info.VisitTime = dataHelper.GetVisitTime();
        info.VisitUrl = dataHelper.GetUrl();
        info.VisitUserAgent = dataHelper.GetUserAgent();
        info.VisitBrower = dataHelper.GetBrower();
        info.VisitOSLanguage = dataHelper.GetOSLanguage();
        return info;
    }
    
    public bool IsReusable {
        get {
            return false;
        }
    }

}