<%@ WebHandler Language="C#" Class="CheckShopBody" %>

using System;
using System.Web;
using System.Collections.Generic;

public class CheckShopBody : IHttpHandler {

    public void ProcessRequest(HttpContext context)
    {

        context.Response.ContentType = "text/plain";

        if (context.Request.Cookies["nick"] == null)
        {
            context.Response.Write("nick expired");
            context.Response.End();
        }
        else
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder("{");
            string nickNo = HttpUtility.UrlDecode(context.Request.Cookies["nick"].Value);

            SiteTotalService stsDal = new SiteTotalService();
            TopSiteTotalInfo sitetotalInfo = stsDal.GetOrderTotalInfo(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(-1), nickNo);
            //店铺流量健康
            if ((double)sitetotalInfo.SiteUVCount / sitetotalInfo.SitePVCount > 0.33)
                sb.Append("1,");
            else
                sb.Append("0,");
            //销售客单价健康
            if (((double)sitetotalInfo.SiteOrderPay / sitetotalInfo.SiteBuyCustomTotal) / ((double)sitetotalInfo.SiteOrderPay / sitetotalInfo.GoodsCount) >= 1.5)
                sb.Append("1,");
            else
                sb.Append("0,");

            //店铺转化率
            if ((double)sitetotalInfo.SiteOrderCount / sitetotalInfo.SiteUVCount > 0.01)
                sb.Append("1,");
            else
                sb.Append("0,");

            //店铺浏览回头率
            if ((double)sitetotalInfo.SiteUVBack / sitetotalInfo.SiteUVCount > 0.2)
                sb.Append("1,");
            else
                sb.Append("0,");

            //店铺二次购买率
            if ((double)sitetotalInfo.SiteSecondBuy / sitetotalInfo.SiteBuyCustomTotal >= 0.01)
                sb.Append("1,");
            else
                sb.Append("0,");

            //页面访问深度
            if (sitetotalInfo.SitePVCount / sitetotalInfo.SiteUVCount >= 3)
                sb.Append("1,");
            else
                sb.Append("0,");

            //爆款商品购买率(浏览次数/购买次数)
            TaoBaoGoodsServive taoGoodsService = new TaoBaoGoodsServive();
            //购买
            IList<GoodsInfo> glist = taoGoodsService.GetTopBuyGoods(nickNo, DateTime.Parse(DateTime.Now.AddDays(-1).ToShortDateString()), DateTime.Parse(DateTime.Now.ToShortDateString()), 1, 1);
            //浏览
            IList<GoodsInfo> gllist = taoGoodsService.GetTopGoods(DataHelper.Encrypt(nickNo), DateTime.Parse(DateTime.Now.AddDays(-1).ToShortDateString()), DateTime.Parse(DateTime.Now.ToShortDateString()), 1, 1);
            if (glist.Count > 0 && gllist.Count > 0)
            {
                if ((double)glist[0].Count / gllist[0].Count >= 0.3)
                    sb.Append("1,");
                else sb.Append("0,");

            }
            else
                sb.Append("0,");

            context.Response.Write(sb.ToString().Substring(0, sb.ToString().Length - 1) + "}");

            context.Response.End();
        }

    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}