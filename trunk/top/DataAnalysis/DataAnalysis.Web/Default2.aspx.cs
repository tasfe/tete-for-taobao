using System;
using System.Collections;
using System.Web;
using System.Collections.Generic;
using System.Linq;

public partial class Default2 : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            DateTime now = DateTime.Parse(DateTime.Now.ToShortDateString());

            string nickNo = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
            IList<TopSiteTotalInfo> siteTotalList = new SiteTotalService().GetNickOrderTotal(now.AddDays(-10), now.AddDays(-1), nickNo);
            InsertTongbi(siteTotalList);
            Rpt_TotalList.DataSource = siteTotalList;
            Rpt_TotalList.DataBind();

        }
    }

    private void InsertTongbi(IList<TopSiteTotalInfo> siteTotalList)
    {
        TopSiteTotalInfo lastinfo = siteTotalList.OrderByDescending(o => o.SiteTotalDate).ToList()[1];
        TopSiteTotalInfo localinfo = siteTotalList.OrderByDescending(o => o.SiteTotalDate).ToList()[0];

        TopSiteTotalInfo info = new TopSiteTotalInfo();
        info.SiteTotalDate = "同比";
        info.SitePVCount = localinfo.SitePVCount - lastinfo.SitePVCount;
        info.SiteUVCount = localinfo.SiteUVCount - lastinfo.SiteUVCount;
        info.ZhiTongFlow = localinfo.ZhiTongFlow - lastinfo.ZhiTongFlow;
        info.SiteZuanZhan = localinfo.SiteZuanZhan - lastinfo.SiteZuanZhan;
        
        info.FreeFlow = localinfo.FreeFlow - lastinfo.FreeFlow;
        info.AskOrder = localinfo.AskOrder - lastinfo.AskOrder;
        info.LostOrder = Math.Round(decimal.Parse(localinfo.LostOrder.Replace("%", "")) - decimal.Parse(lastinfo.LostOrder.Replace("%", "")), 2) + "%";
        info.SiteOrderCount = localinfo.SiteOrderCount - lastinfo.SiteOrderCount;
        info.SiteOrderPay = localinfo.SiteOrderPay - lastinfo.SiteOrderPay;
        info.SellAvg = (decimal.Parse(localinfo.SellAvg) - decimal.Parse(lastinfo.SellAvg)).ToString();
        info.OneCustomerPrice = (decimal.Parse(localinfo.OneCustomerPrice) - decimal.Parse(lastinfo.OneCustomerPrice)).ToString();

        info.CreateAVG = Math.Round(decimal.Parse(localinfo.CreateAVG.Replace("%", "")) - decimal.Parse(lastinfo.CreateAVG.Replace("%", "")), 2) + "%";
        info.SiteSecondBuy = localinfo.SiteSecondBuy - lastinfo.SiteSecondBuy;
        info.BackSee = Math.Round(decimal.Parse(localinfo.BackSee.Replace("%", "")) - decimal.Parse(lastinfo.BackSee.Replace("%", "")), 2) + "%";
        info.Refund = Math.Round(decimal.Parse(localinfo.Refund.Replace("%", "")) - decimal.Parse(lastinfo.Refund.Replace("%", "")), 2) + "%";

        info.SeeDeepAVG = (decimal.Parse(localinfo.SeeDeepAVG) - decimal.Parse(lastinfo.SeeDeepAVG)).ToString();

        info.Collection = localinfo.Collection - lastinfo.Collection;
        siteTotalList.Add(info);
    }
    protected string GetMonthDay(string date)
    {

        if (date == "同比")
            return date;
        string s = date.Substring(4);

        if (s.Substring(0, 1) == "0")
        {
            if (s.Substring(2, 1) == "0")
                return s.Substring(1, 1) + "/" + s.Substring(3);
            else
                return s.Substring(1, 1) + "/" + s.Substring(2);
        }
        else
        {
            if (s.Substring(2, 1) == "0")
                return s.Substring(0, 2) + "/" + s.Substring(3);
            else
                return s.Substring(0, 2) + "/" + s.Substring(2);
        }
    }

    private void InsertName()
    {
        List<TotalNameInfo> list = new List<TotalNameInfo>();
        list.Add(new TotalNameInfo { TotalName = "PV" });
        list.Add(new TotalNameInfo { TotalName = "UV" });
        list.Add(new TotalNameInfo { TotalName = "直通车流量" });
        list.Add(new TotalNameInfo { TotalName = "推广费用" });
        list.Add(new TotalNameInfo { TotalName = "CPC" });
        list.Add(new TotalNameInfo { TotalName = "询单数" });
        list.Add(new TotalNameInfo { TotalName = "丢单率" });
        list.Add(new TotalNameInfo { TotalName = "订单量" });

        list.Add(new TotalNameInfo { TotalName = "实际销售额" });
        list.Add(new TotalNameInfo { TotalName = "销售均价" });
        list.Add(new TotalNameInfo { TotalName = "客单价" });
        list.Add(new TotalNameInfo { TotalName = "平均转化率" });
        list.Add(new TotalNameInfo { TotalName = "浏览回头客" });

        list.Add(new TotalNameInfo { TotalName = "退款率" });
        list.Add(new TotalNameInfo { TotalName = "平均访问深度" });
        list.Add(new TotalNameInfo { TotalName = "流量排行" });
        list.Add(new TotalNameInfo { TotalName = "销售排行" });
        list.Add(new TotalNameInfo { TotalName = "收藏量" });

        //Rpt_Data.DataSource = list;
        //Rpt_Data.DataBind();
    }

    protected string GetGoodsName(string goodsName)
    {
        if (goodsName.Length > 2)
            return goodsName.Substring(0, 2) + ".";
        return goodsName;
    }
    //protected void Btn_AddCookie_Click(object sender, EventArgs e)
    //{
    //    HttpCookie cookie = new HttpCookie("nick", HttpUtility.UrlEncode("luckyfish8800"));
    //    cookie.Expires = DateTime.Now.AddDays(1);

    //    HttpCookie session = new HttpCookie("nicksession", CacheCollection.GetNickSessionList().Where(o => o.Nick == "luckyfish8800").ToList()[0].Session);
    //    session.Expires = DateTime.Now.AddDays(1);

    //    Response.Cookies.Add(cookie);
    //    Response.Cookies.Add(session);

    //    Response.Write("<script>location.href='Default2.aspx'</script>");
    //}
}

public class TotalNameInfo
{
    public string TotalName { set; get; }
}
