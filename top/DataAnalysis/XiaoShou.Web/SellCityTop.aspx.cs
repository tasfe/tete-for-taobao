using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

public partial class SellCityTop : BasePage
{
    readonly TaoBaoGoodsOrderService orderDal = new TaoBaoGoodsOrderService();
    PagedDataSource pds = new PagedDataSource();

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
            Bind(darray[0], darray[1]);
        }
    }

    private void Bind(DateTime start, DateTime end)
    {
        int TotalCount = 0;//总记录数
        int TotalPage = 1; //总页数

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

        IList<GoodsOrderInfo> list = orderDal.GetSellCityTop(start, end, HttpUtility.UrlDecode(Request.Cookies["nick"].Value));
        TotalCount = list.Count;
        pds.DataSource = list;
        pds.AllowPaging = true;
        pds.PageSize = 20;
        if (TotalCount == 0)
            TotalPage = 1;
        else
        {
            if (TotalCount % pds.PageSize == 0)
                TotalPage = TotalCount / pds.PageSize;
            else
                TotalPage = TotalCount / pds.PageSize + 1;
        }

        pds.CurrentPageIndex = page - 1;
        lblCurrentPage.Text = "共" + TotalCount.ToString() + "条记录 当前页：" + page + "/" + TotalPage;

        lnkFrist.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=1&" + "start=" + start.ToShortDateString() + "&end=" + end.ToShortDateString();
        if (!pds.IsFirstPage)
            lnkPrev.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + Convert.ToString(page - 1) + "&" + "start=" + start.ToShortDateString() + "&end=" + end.ToShortDateString();

        if (!pds.IsLastPage)
            lnkNext.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + Convert.ToString(page + 1) + "&" + "start=" + start.ToShortDateString() + "&end=" + end.ToShortDateString();
        lnkEnd.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + TotalPage + "&" + "start=" + start.ToShortDateString() + "&end=" + end.ToShortDateString();

        Rpt_PageVisit.DataSource = pds;
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
        }
        catch
        {
            TB_Start.Text = start.ToString("yyyy-MM-dd");
            TB_End.Text = endtime.ToString("yyyy-MM-dd");
        }
        ViewState["page"] = "1";
        Bind(start, endtime);
    }

    protected void Btn_3Days_Click(object sender, EventArgs e)
    {
        Bind(DateTime.Now.AddDays(-2), DateTime.Now);
    }
    protected void Btn_7Days_Click(object sender, EventArgs e)
    {
        Bind(DateTime.Now.AddDays(-6), DateTime.Now);
    }
    protected void Btn_30Days_Click(object sender, EventArgs e)
    {
        Bind(DateTime.Now.AddDays(-29), DateTime.Now);
    }
}
