using System;
using System.Collections;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;

public partial class Commerce : BasePage
{
    SiteTotalService stDal = new SiteTotalService();
    PagedDataSource pds = new PagedDataSource();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DateTime[] dates = DataHelper.GetDateTime(DateTime.Now.AddDays(-3), 3);

            Bind(dates[0], dates[1]);
        }
    }

    private void Bind(DateTime start,DateTime end)
    {
        int TotalCount = 0;//总记录数
        int TotalPage = 1; //总页数

        string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
        IList<TopSiteTotalInfo> list = stDal.GetRealTotal(nick, start, end);
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
        string startstr = start.ToString("yyyy-MM-dd");
        string endstr = end.ToString("yyyy-MM-dd");

        pds.CurrentPageIndex = page - 1;
        lblCurrentPage.Text = "共" + TotalCount.ToString() + "条记录 当前页：" + page + "/" + TotalPage;

        lnkFrist.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=1" + "&start=" + startstr + "&end=" + endstr;
        if (!pds.IsFirstPage)
            lnkPrev.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + (page - 1) + "&start=" + startstr + "&end=" + endstr;

        if (!pds.IsLastPage)
            lnkNext.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + (page + 1) + "&start=" + startstr + "&end=" + endstr;
        lnkEnd.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + TotalPage + "&start=" + startstr + "&end=" + endstr;

        Rpt_RealTotal.DataSource = pds;
        Rpt_RealTotal.DataBind();

        TB_Start.Text = start.ToString("yyyy-MM-dd");
        TB_End.Text = end.ToString("yyyy-MM-dd");
    }

    protected void Btn_Select_Click(object sender, EventArgs e)
    {
        Bind(DateTime.Parse(TB_Start.Text), DateTime.Parse(TB_End.Text));
    }

    protected string GetDate(string date)
    {
        return date.Substring(0, 4) + "-" + date.Substring(4, 2) + "-" + date.Substring(6);
    }
}
