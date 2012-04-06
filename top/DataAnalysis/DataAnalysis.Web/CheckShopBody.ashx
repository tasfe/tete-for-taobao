<%@ WebHandler Language="C#" Class="CheckShopBody" %>

using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;

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

            //获取用户自定义体检参数
            IList<Model.TijianParamInfo> tijianList = new TijianParamValueService().GetParamInfo(nickNo);
            if (tijianList.Count == 0)
            {
                DataHelper.GetParam(tijianList);
            }
            
            //店铺流量健康
            if (sitetotalInfo.SitePVCount == 0)
                sb.Append("0|0,");
            else
            {
                if ((decimal)sitetotalInfo.SiteUVCount / sitetotalInfo.SitePVCount > tijianList.First(o => o.ParamName == "客户浏览比率").ParamValue)
                    sb.Append("1|" + Math.Round(((decimal)sitetotalInfo.SiteUVCount / sitetotalInfo.SitePVCount), 2) + ",");
                else
                    sb.Append("0|" + Math.Round(((decimal)sitetotalInfo.SiteUVCount / sitetotalInfo.SitePVCount),2) + ",");
            }
            //销售客单价健康
            if (sitetotalInfo.SiteBuyCustomTotal == 0 || sitetotalInfo.GoodsCount == 0)
                sb.Append("0,");
            else
            {
                if (((decimal)sitetotalInfo.SiteOrderPay / sitetotalInfo.SiteBuyCustomTotal) / ((decimal)sitetotalInfo.SiteOrderPay / sitetotalInfo.GoodsCount) >= tijianList.First(o => o.ParamName == "销售关联度").ParamValue)
                    sb.Append("1|" + (((decimal)sitetotalInfo.SiteOrderPay / sitetotalInfo.SiteBuyCustomTotal) / ((decimal)sitetotalInfo.SiteOrderPay / sitetotalInfo.GoodsCount)) + ",");
                else
                    sb.Append("0|" + ((decimal)sitetotalInfo.SiteOrderPay / sitetotalInfo.SiteBuyCustomTotal) / ((decimal)sitetotalInfo.SiteOrderPay / sitetotalInfo.GoodsCount) + ",");
            }

            //店铺转化率
            if (sitetotalInfo.SiteUVCount > 0)
                sb.Append("0|0,");
            else
            {
                if ((decimal)sitetotalInfo.SiteOrderCount / sitetotalInfo.SiteUVCount > tijianList.First(o => o.ParamName == "浏览转换率").ParamValue)
                    sb.Append("1|" + ((decimal)sitetotalInfo.SiteOrderCount / sitetotalInfo.SiteUVCount) + ",");
                else
                    sb.Append("0|" + ((decimal)sitetotalInfo.SiteOrderCount / sitetotalInfo.SiteUVCount) + ",");
            }

            //店铺浏览回头率
            if (sitetotalInfo.SiteUVCount == 0)
                sb.Append("0|0,");
            else
            {
                if ((decimal)sitetotalInfo.SiteUVBack / sitetotalInfo.SiteUVCount > tijianList.First(o => o.ParamName == "浏览回头率").ParamValue)
                    sb.Append("1" + ((decimal)sitetotalInfo.SiteUVBack / sitetotalInfo.SiteUVCount) + ",");
                else
                    sb.Append("0|" + ((decimal)sitetotalInfo.SiteUVBack / sitetotalInfo.SiteUVCount) + ",");
            }
            
            //店铺二次购买率
            if (sitetotalInfo.SiteBuyCustomTotal == 0)
                sb.Append("0|0,");
            else
            {
                if ((decimal)sitetotalInfo.SiteSecondBuy / sitetotalInfo.SiteBuyCustomTotal >= tijianList.First(o => o.ParamName == "二次购买率").ParamValue)
                    sb.Append("1|" + ((decimal)sitetotalInfo.SiteSecondBuy / sitetotalInfo.SiteBuyCustomTotal) + ",");
                else
                    sb.Append("0|" + ((decimal)sitetotalInfo.SiteSecondBuy / sitetotalInfo.SiteBuyCustomTotal) + ",");
            }

            //页面访问深度
            if (sitetotalInfo.SiteUVCount == 0)
                sb.Append("0|0,");
            else
            {
                if (sitetotalInfo.SitePVCount / sitetotalInfo.SiteUVCount >= tijianList.First(o => o.ParamName == "页面访问深度").ParamValue)
                    sb.Append("1|" + (sitetotalInfo.SitePVCount / sitetotalInfo.SiteUVCount) + ",");
                else
                    sb.Append("0|" + (sitetotalInfo.SitePVCount / sitetotalInfo.SiteUVCount) + ",");
            }

            //爆款商品购买率(浏览次数/购买次数)
            TaoBaoGoodsServive taoGoodsService = new TaoBaoGoodsServive();
            //购买
            IList<GoodsInfo> glist = taoGoodsService.GetTopBuyGoods(nickNo, DateTime.Parse(DateTime.Now.AddDays(-1).ToShortDateString()), DateTime.Parse(DateTime.Now.ToShortDateString()), 1, 1);
            //浏览
            IList<GoodsInfo> gllist = taoGoodsService.GetTopGoods(DataHelper.Encrypt(nickNo), DateTime.Parse(DateTime.Now.AddDays(-1).ToShortDateString()), DateTime.Parse(DateTime.Now.ToShortDateString()), 1, 1);
            if (glist.Count > 0 && gllist.Count > 0)
            {
                if ((decimal)glist[0].Count / gllist[0].Count >= tijianList.First(o => o.ParamName == "爆款商品购买率").ParamValue)
                    sb.Append("1|" + ((decimal)glist[0].Count / gllist[0].Count) + ",");
                else sb.Append("0|" + ((decimal)glist[0].Count / gllist[0].Count) + ",");

            }
            else
                sb.Append("0|0,");

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