using System;
using System.Collections;
using System.Web.UI;
using System.Collections.Generic;
using System.Web;
using System.Linq;

public partial class TopGoods : BasePage
{
    TaoBaoGoodsServive taoGoodsService = new TaoBaoGoodsServive();

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            if (!VisitService.CheckTable(DataHelper.Encrypt(HttpUtility.UrlDecode(Request.Cookies["nick"].Value))))
            {
                Response.Redirect("CreateCode.aspx");
                //Response.Write("<script>alert('抱歉,您还没有添加统计代码!');</script>");
                //Response.End();
            }
            else
            {
                DateTime[] darray = DataHelper.GetDateTime(DateTime.Now, 1);
                try
                {
                    darray[0] = DateTime.Parse(Request.QueryString["start"]);
                    darray[1] = DateTime.Parse(Request.QueryString["end"]).AddDays(1);
                }
                catch
                {
                }

                if (!string.IsNullOrEmpty(Request.QueryString["day"]))
                {
                    string date = Request.QueryString["day"];
                    date = date.Substring(0, 4) + "-" + date.Substring(4, 2) + "-" + date.Substring(6);

                    darray[0] = DateTime.Parse(date);
                    darray[1] = DateTime.Parse(date).AddDays(1);
                }

                string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
                string session = Request.Cookies["nicksession"].Value;
                ViewState["count"] = taoGoodsService.GetTopGoodsCount(DataHelper.Encrypt(nick), darray[0], darray[1]);
                Bind(nick, session, darray[0], darray[1], int.Parse(ViewState["count"].ToString()), 9);
            }
        }
    }

    private void Bind(string nick, string session, DateTime start, DateTime end, int totalCount, int recordCount)
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

        IList<GoodsInfo> list = taoGoodsService.GetTopGoods(DataHelper.Encrypt(HttpUtility.UrlDecode(Request.Cookies["nick"].Value)), start, end, page, recordCount);
        string gids = "";
        foreach (GoodsInfo ginfo in list)
        {
            gids += ginfo.num_iid + ",";
        }

        IList<GoodsInfo> seeBuyList = taoGoodsService.GetTopSeeBuyList(HttpUtility.UrlDecode(Request.Cookies["nick"].Value), start, end, gids.Substring(0, gids.Length - 1));
        for (int i = 0; i < list.Count; i++)
        {
            IList<GoodsInfo> seeblist = seeBuyList.Where(o => o.num_iid == list[i].num_iid).ToList();
            if (seeblist.Count > 0)
            {
                list[i].nick = seeblist[0].Count.ToString();
            }
        }

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
                List<GoodsInfo> goodsinfoList = TaoBaoAPI.GetGoodsInfoList(nick, session, pids.Substring(0, pids.Length - 1));

                if (Cache["taobaogoodslist"] == null)
                    Cache.Insert("taobaogoodslist", goodsinfoList, null, DateTime.Now.AddHours(12), System.Web.Caching.Cache.NoSlidingExpiration);
                else
                    cachegoods.AddRange(goodsinfoList);

            }
            for (int i = 0; i < list.Count; i++)
            {
                IList<GoodsInfo> thislist = ((List<GoodsInfo>)Cache["taobaogoodslist"]).Where(o => o.num_iid == list[i].num_iid).ToList();
                if (thislist.Count > 0)
                {
                    list[i].title = thislist[0].title;
                    list[i].price = thislist[0].price;
                    list[i].pic_url = thislist[0].pic_url;
                }
            }
        }

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
        string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
        string session = Request.Cookies["nicksession"].Value;
        ViewState["count"] = taoGoodsService.GetTopGoodsCount(DataHelper.Encrypt(nick), start, endtime.AddDays(1));
        ViewState["page"] = "1";
        Bind(nick, session, start, endtime, int.Parse(ViewState["count"].ToString()), 9);
    }

    protected void Btn_3Days_Click(object sender, EventArgs e)
    {
        string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
        string session = Request.Cookies["nicksession"].Value;
        ViewState["count"] = taoGoodsService.GetTopGoodsCount(DataHelper.Encrypt(nick), DateTime.Now.AddDays(-2), DateTime.Now);
        Bind(nick, session, DateTime.Parse(DateTime.Now.ToShortDateString()).AddDays(-2), DateTime.Now.AddDays(1), int.Parse(ViewState["count"].ToString()), 9);
    }
    protected void Btn_7Days_Click(object sender, EventArgs e)
    {
        string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
        string session = Request.Cookies["nicksession"].Value;
        ViewState["count"] = taoGoodsService.GetTopGoodsCount(DataHelper.Encrypt(nick), DateTime.Now.AddDays(-6), DateTime.Now);
        Bind(nick, session, DateTime.Parse(DateTime.Now.ToShortDateString()).AddDays(-6), DateTime.Now.AddDays(1), int.Parse(ViewState["count"].ToString()), 9);
    }
    protected void Btn_30Days_Click(object sender, EventArgs e)
    {
        string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
        string session = Request.Cookies["nicksession"].Value;
        ViewState["count"] = taoGoodsService.GetTopGoodsCount(DataHelper.Encrypt(nick), DateTime.Now.AddDays(-29), DateTime.Now);
        Bind(nick, session, DateTime.Parse(DateTime.Now.ToShortDateString()).AddDays(-29), DateTime.Now.AddDays(1), int.Parse(ViewState["count"].ToString()), 9);
    }
}
