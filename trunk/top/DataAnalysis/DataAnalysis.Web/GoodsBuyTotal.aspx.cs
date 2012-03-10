using System;
using System.Collections;
using System.Web.UI;
using System.Collections.Generic;
using System.Web;
using System.Linq;

public partial class GoodsBuyTotal : BasePage
{
    TaoBaoGoodsServive taoGoodsService = new TaoBaoGoodsServive();

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            DateTime[] darray = DataHelper.GetDateTime(DateTime.Now, 1);
            try
            { 
                darray[0] = DateTime.Parse(Request.QueryString["start"]);
                darray[1] = DateTime.Parse(Request.QueryString["end"]);
            }
            catch
            {
            }
            ViewState["count"] = taoGoodsService.GetTopGoodsBuyCount(HttpUtility.UrlDecode(Request.Cookies["nick"].Value), darray[0], darray[1]);
            Bind(darray[0], darray[1], int.Parse(ViewState["count"].ToString()), 20);
        }
    }

    private void Bind(DateTime start, DateTime end, int totalCount, int recordCount)
    {
        int TotalPage = totalCount % recordCount != 0 ? (totalCount / recordCount) + 1 : totalCount / recordCount; //总页数

        int page = 1;
        try
        {
            page = int.Parse(Request.QueryString["Page"]);
            if (ViewState["page"] != null)
            {
                page = int.Parse(ViewState["page"].ToString());
                ViewState["page"] = null;
            }

        }
        catch { }

        IList<GoodsInfo> list = taoGoodsService.GetTopBuyGoods(HttpUtility.UrlDecode(Request.Cookies["nick"].Value), start, end, page, recordCount);

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
                }
            }
        }

        //for (int i = 0; i < list.Count; i++)
        //{
        //    GoodsInfo rinfo = TaoBaoAPI.GetGoodsInfo(list[i].num_iid);
        //    list[i].title = rinfo.title;
        //    list[i].price = rinfo.price;
        //}

        lblCurrentPage.Text = "共" + totalCount.ToString() + "条记录 当前页：" + page + "/" + (TotalPage == 0 ? 1 : TotalPage);

        lnkFrist.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=1&" + "start=" + start.ToShortDateString() + "&end=" + end.ToShortDateString();
        if (page > 1)
            lnkPrev.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + Convert.ToString(page - 1) + "&" + "start=" + start.ToShortDateString() + "&end=" + end.ToShortDateString();

        if (page != TotalPage && TotalPage != 0)
            lnkNext.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + Convert.ToString(page + 1) + "&" + "start=" + start.ToShortDateString() + "&end=" + end.ToShortDateString();
        lnkEnd.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + (TotalPage == 0 ? 1 : TotalPage) + "&" + "start=" + start.ToShortDateString() + "&end=" + end.ToShortDateString();

        Rpt_PageVisit.DataSource = list;
        Rpt_PageVisit.DataBind();
        TB_Start.Text = start.ToString("yyyy-MM-dd");
        TB_End.Text = end.ToString("yyyy-MM-dd");
    }

    protected void Btn_Select_Click(object sender, EventArgs e)
    {
        DateTime now = DateTime.Now;
        DateTime end = now.AddDays(1);
        DateTime start = new DateTime(now.Year, now.Month, now.Day);
        DateTime endtime = new DateTime(end.Year, end.Month, end.Day);
        try
        {
            start = DateTime.Parse(TB_Start.Text);
            endtime = DateTime.Parse(TB_End.Text);
            ViewState["start"] = start;
            ViewState["end"] = end;
        }
        catch
        {
            TB_Start.Text = start.ToString("yyyy-MM-dd");
            TB_End.Text = endtime.ToString("yyyy-MM-dd");
        }
        ViewState["count"] = taoGoodsService.GetTopGoodsBuyCount(HttpUtility.UrlDecode(Request.Cookies["nick"].Value), start, endtime);
        ViewState["page"] = "1";
        Bind(start, endtime, int.Parse(ViewState["count"].ToString()), 20);
    }
}
