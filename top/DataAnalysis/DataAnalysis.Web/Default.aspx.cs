using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;

public partial class _Default : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.Cookies["nick"] == null)
                System.Threading.Thread.Sleep(1000 * 6);
            string nickNo = DataHelper.Encrypt(HttpUtility.UrlDecode(Request.Cookies["nick"].Value));
            VisitService vistitDal = new VisitService();
            DateTime[] darray = DataHelper.GetDateTime(DateTime.Now, 1);
            Rpt_IpPV.DataSource = vistitDal.GetIndexTotalInfoList(nickNo, darray[0], darray[1]).OrderByDescending(o => o.Value).ToList();
            Rpt_IpPV.DataBind();

            Rpt_OnlineCustomer.DataSource = vistitDal.GetIndexOnlineCustomer(nickNo, 3, darray[0], darray[1]);
            Rpt_OnlineCustomer.DataBind();

            TaoBaoGoodsServive taoGoodsService = new TaoBaoGoodsServive();

            IList<GoodsInfo> list = taoGoodsService.GetTopBuyGoods(HttpUtility.UrlDecode(Request.Cookies["nick"].Value), darray[0], darray[1], 1, 3);

            if (list.Count > 0)
            {
                string pids = "";
                List<GoodsInfo> cachegoods = new List<GoodsInfo>();
                if (Cache["taobaogoodslist"] != null)
                    cachegoods = (List<GoodsInfo>)Cache["taobaogoodslist"];
                foreach (GoodsInfo info in list)
                {
                    if (!cachegoods.Contains(info))
                        pids += info.num_iid + ",";
                }

                if (pids != "")
                {
                    List<GoodsInfo> goodsinfoList = TaoBaoAPI.GetGoodsInfoList(pids.Substring(0, pids.Length - 1));

                    if (Cache["taobaogoodslist"] == null)
                        Cache.Insert("taobaogoodslist", goodsinfoList, null, DateTime.Now.AddHours(12), System.Web.Caching.Cache.NoSlidingExpiration);
                    else
                        cachegoods.AddRange(goodsinfoList);

                }
                for (int i = 0; i < list.Count; i++)
                {
                    IList<GoodsInfo> thislist = cachegoods.Where(o => o.num_iid == list[i].num_iid).ToList();
                    if (thislist.Count > 0)
                    {
                        list[i].title = thislist[0].title;
                        list[i].price = thislist[0].price;
                        list[i].pic_url = thislist[0].pic_url;
                    }
                }
            }

            Rpt_GoodsSellTop.DataSource = list;
            Rpt_GoodsSellTop.DataBind();

            SiteTotalService siteTotalDal = new SiteTotalService();
            BindRpt_OrderTotal(siteTotalDal);
            TopSiteTotalInfo lastweek = siteTotalDal.GetOrderTotalInfo(DateTime.Now.AddDays(-7), DateTime.Now, nickNo);
            TopSiteTotalInfo llastweek = siteTotalDal.GetOrderTotalInfo(DateTime.Now.AddDays(-14), DateTime.Now.AddDays(-7), nickNo);
            ViewState["lastweek"] = lastweek;
            ViewState["llastweek"] = llastweek;
        }
    }

    private void BindRpt_OrderTotal(SiteTotalService siteTotalDal)
    {
        string nickNo = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
        List<TopSiteTotalInfo> list = new List<TopSiteTotalInfo>();
        TopSiteTotalInfo today = siteTotalDal.GetOrderTotalInfo(DateTime.Now, DateTime.Now.AddDays(1), nickNo);
        today.SiteNick = "今天";

        TopSiteTotalInfo yesterday = siteTotalDal.GetOrderTotalInfo(DateTime.Now.AddDays(-1), DateTime.Now, nickNo);
        yesterday.SiteNick = "昨天";

        list.Add(today);
        list.Add(yesterday);

        Rpt_OrderTotal.DataSource = list;
        Rpt_OrderTotal.DataBind();
    }

    protected void Btn_AddCId_Click(object sender, EventArgs e)
    {
        if (TaoBaoAPI.AddCID(HttpUtility.UrlDecode(Request.Cookies["nick"].Value), Request.Cookies["nicksession"].Value))
        {
            Page.RegisterStartupScript("恭喜", "<script>alert('添加成功!');</script>");
        }
        else
        {
            Page.RegisterStartupScript("抱歉", "<script>alert('添加失败!');</script>");
        }
    }

    protected string GetCusOne(object siteOrderPay, object siteBuyCustomTotal)
    {
        if (siteBuyCustomTotal.ToString() == "0") return "0";
        return ((double)siteOrderPay / (double)siteBuyCustomTotal).ToString(".00");
    }

    protected string GetSellOne(object siteOrderPay, object goodsCount)
    {
        if (goodsCount.ToString() == "0") return "0";
        return ((double)siteOrderPay / (double)goodsCount).ToString(".00");
    }

    protected string GetEval(object siteOrderPay, object siteBuyCustomTotal, object goodsCount)
    {
        if (siteBuyCustomTotal.ToString() == "0" || goodsCount.ToString() == "0") return "0";
        return (((double)siteOrderPay / (double)siteBuyCustomTotal) / ((double)siteOrderPay / (double)goodsCount)).ToString(".00");
    }

    #region 7天走势

    protected string SevenSitePay()
    {
        TopSiteTotalInfo linfo = (TopSiteTotalInfo)ViewState["lastweek"];
        TopSiteTotalInfo llinfo = (TopSiteTotalInfo)ViewState["llastweek"];

        if (llinfo.SiteOrderPay == 0)
        {
            if (linfo.SiteOrderPay == 0)
                return "0";
            return "<font color='green'>↑" + linfo.SiteOrderPay * 100 + "%</font>";
        }

        double pay = (double)(linfo.SiteOrderPay / llinfo.SiteOrderPay);
        if (pay > 1)
            return "<font color='green'>↑" + (pay - 1).ToString(".00") + "%</font>";

        return "<font color='red'>↓" + (1 - pay).ToString(".00") + "%</font>";
    }

    protected string SevenSiteOrderCount()
    {
        TopSiteTotalInfo linfo = (TopSiteTotalInfo)ViewState["lastweek"];
        TopSiteTotalInfo llinfo = (TopSiteTotalInfo)ViewState["llastweek"];

        if (llinfo.SiteOrderCount == 0)
        {
            if (linfo.SiteOrderCount == 0)
                return "0";
            return "<font color='green'>↑" + linfo.SiteOrderCount * 100 + "%</font>";
        }

        double pay = linfo.SiteOrderCount / llinfo.SiteOrderCount;
        if (pay > 1)
            return "<font color='green'>↑" + (pay - 1).ToString(".00") + "%</font>";

        return "<font color='red'>↓" + (1 - pay).ToString(".00") + "%</font>";
    }

    protected string SevenSiteBackOrder()
    {
        TopSiteTotalInfo linfo = (TopSiteTotalInfo)ViewState["lastweek"];
        TopSiteTotalInfo llinfo = (TopSiteTotalInfo)ViewState["llastweek"];

        if (llinfo.SiteSecondBuy == 0)
        {
            if (linfo.SiteSecondBuy == 0)
                return "0";
            return "<font color='green'>↑" + linfo.SiteSecondBuy * 100 + "%</font>";
        }

        double pay = linfo.SiteSecondBuy / llinfo.SiteSecondBuy;
        if (pay > 1)
            return "<font color='green'>↑" + (pay - 1).ToString(".00") + "%</font>";

        return "<font color='red'>↓" + (1 - pay).ToString(".00") + "%</font>";
    }

    protected string SevenSiteOnePay()
    {
        TopSiteTotalInfo linfo = (TopSiteTotalInfo)ViewState["lastweek"];
        TopSiteTotalInfo llinfo = (TopSiteTotalInfo)ViewState["llastweek"];

        if (llinfo.SiteBuyCustomTotal == 0 || linfo.SiteBuyCustomTotal == 0)
        {
            return "0";
        }

        if (linfo.SiteBuyCustomTotal != 0 && llinfo.SiteBuyCustomTotal == 0)
        {
            return "<font color='green'>↑" + (linfo.SiteOrderPay / linfo.SiteBuyCustomTotal) * 100 + "%</font>";
        }

        double pay = (double)((linfo.SiteOrderPay / linfo.SiteBuyCustomTotal) / (llinfo.SiteOrderPay / llinfo.SiteBuyCustomTotal));
        if (pay > 1)
            return "<font color='green'>↑" + (pay - 1).ToString(".00") + "%</font>";

        return "<font color='red'>↓" + (1 - pay).ToString(".00") + "%</font>";
    }

    protected string SevenSiteSellOnePay()
    {
        TopSiteTotalInfo linfo = (TopSiteTotalInfo)ViewState["lastweek"];
        TopSiteTotalInfo llinfo = (TopSiteTotalInfo)ViewState["llastweek"];

        if (llinfo.GoodsCount == 0 || linfo.GoodsCount == 0)
        {
            return "0";
        }

        if (linfo.GoodsCount != 0 && llinfo.GoodsCount == 0)
        {
            return "<font color='green'>↑" + (linfo.SiteOrderPay / linfo.GoodsCount) * 100 + "%</font>";
        }

        double pay = (double)((linfo.SiteOrderPay / linfo.GoodsCount) / (llinfo.SiteOrderPay / llinfo.GoodsCount));
        if (pay > 1)
            return "<font color='green'>↑" + (pay - 1).ToString(".00") + "%</font>";

        return "<font color='red'>↓" + (1 - pay).ToString(".00") + "%</font>";
    }

    protected string SevenSiteSellWith()
    {
        TopSiteTotalInfo linfo = (TopSiteTotalInfo)ViewState["lastweek"];
        TopSiteTotalInfo llinfo = (TopSiteTotalInfo)ViewState["llastweek"];

        if (llinfo.GoodsCount == 0 || linfo.GoodsCount == 0 || llinfo.SiteBuyCustomTotal == 0 || linfo.SiteBuyCustomTotal == 0)
        {
            return "0";
        }

        if (linfo.GoodsCount != 0 && linfo.SiteBuyCustomTotal != 0 && llinfo.GoodsCount == 0 && llinfo.SiteBuyCustomTotal == 0)
        {
            return "<font color='green'>↑" + (linfo.SiteOrderPay / linfo.GoodsCount) * 100 + "%</font>";
        }

        double pay = (double)(((linfo.SiteOrderPay / linfo.SiteBuyCustomTotal) / (llinfo.SiteOrderPay / llinfo.SiteBuyCustomTotal)) / ((linfo.SiteOrderPay / linfo.GoodsCount) / (llinfo.SiteOrderPay / llinfo.GoodsCount)));
        if (pay > 1)
            return "<font color='green'>↑" + (pay - 1).ToString(".00") + "%</font>";

        return "<font color='red'>↓" + (1 - pay).ToString(".00") + "%</font>";
    }

    #endregion

}
