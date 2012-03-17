using System;
using System.Collections;
using System.Linq;
using System.Web;
using CusServiceAchievements.DAL;
using DBHelp;
using TaoBaoAPIHelper;
using System.Collections.Generic;
using Model;

public partial class CustomerList : System.Web.UI.Page
{

    TalkRecodService trDal = new TalkRecodService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);

            DateTime[] dateArr = DataHelper.GetDateTime(DateTime.Now, 1);

            GoodsOrderService goDal = new GoodsOrderService();
            GoodsOrderList = goDal.GetCustomerList(nick, dateArr[0], dateArr[1]);
            int totalcount = trDal.GetCustomerListCount(dateArr[0], dateArr[1], nick);
            Bind(dateArr[0], dateArr[1], nick, totalcount);
        }
    }

    private void Bind(DateTime start, DateTime end, string nick, int totalCount)
    {
        int recordCount = 20;
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

        IList<CustomerInfo> list = trDal.GetCustomerList(start, end, nick, page, recordCount);

        if (list.Count > 0)
        {
            for (int i = 0; i < list.Count; i++)
            {
                IList<GoodsOrderInfo> thislist = GoodsOrderList.Where(o => o.buyer_nick == list[i].CustomerNick).ToList();
                if (thislist.Count > 0)
                {
                    list[i].tid = thislist[0].tid;
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

        Rpt_CustomerList.DataSource = list;
        Rpt_CustomerList.DataBind();
        TB_Start.Text = start.ToString("yyyy-MM-dd");

    }

    protected List<GoodsOrderInfo> GoodsOrderList
    {
        set { ViewState["GoodsOrderList"] = value; }
        get
        {
            return ViewState["GoodsOrderList"] == null ? new List<GoodsOrderInfo>() : (List<GoodsOrderInfo>)ViewState["GoodsOrderList"];
        }
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
            endtime = start.AddDays(1);
        }
        catch
        {
            TB_Start.Text = start.ToString("yyyy-MM-dd");
        }
        string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
        int totalcount = trDal.GetCustomerListCount(start, endtime, nick);
        ViewState["page"] = "1";
        Bind(start, endtime, nick, totalcount);
    }
}
