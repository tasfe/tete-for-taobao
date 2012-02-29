using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;

public partial class PageVisitTotal : System.Web.UI.Page
{

    readonly VisitService visitDal = new VisitService();
    PagedDataSource pds = new PagedDataSource();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DateTime now = DateTime.Now;
            DateTime end = now.AddDays(1);
            DateTime start = new DateTime(now.Year, now.Month, now.Day);
            DateTime endtime = new DateTime(end.Year, end.Month, end.Day);

            Bind(start, endtime);
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
        }
        catch { }

        IList<PageVisitInfoTotal> list = visitDal.GetAllVisitPageInfoList("246bcca56c050c665b67708d33127e46", start, end, page, 20);
        TotalCount = list.Count;
        pds.DataSource = list;
        pds.AllowPaging = true;
        pds.PageSize = 20;
        int CurPage;
        if (Request.QueryString["Page"] != null)
            CurPage = Convert.ToInt32(Request.QueryString["Page"]);
        else
            CurPage = 1;

        if (TotalCount == 0)
            TotalPage = 1;
        else
        {
            if (TotalCount % pds.PageSize == 0)
                TotalPage = TotalCount / pds.PageSize;
            else
                TotalPage = TotalCount / pds.PageSize + 1;
        }

        pds.CurrentPageIndex = CurPage - 1;
        lblCurrentPage.Text = "共" + TotalCount.ToString() + "条记录 当前页：" + CurPage.ToString() + "/" + TotalPage;

        lnkFrist.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=1";
        if (!pds.IsFirstPage)
            lnkPrev.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + Convert.ToString(CurPage - 1);

        if (!pds.IsLastPage)
            lnkNext.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + Convert.ToString(CurPage + 1);
        lnkEnd.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + TotalPage;

        Rpt_PageVisit.DataSource = pds;
        Rpt_PageVisit.DataBind();
    }

    protected void Btn_Select_Click(object sender, EventArgs e)
    {
        DateTime now = DateTime.Now;
        DateTime end = now.AddDays(1);
        DateTime start = new DateTime(now.Year, now.Month, now.Day);
        DateTime endtime = new DateTime(end.Year, end.Month, end.Day);
        try
        {
            start = DateTime.Parse(TB_Start.Text + ":0:0");
            endtime = DateTime.Parse(TB_End.Text + ":0:0");
        }
        catch
        {
            TB_Start.Text = start.ToString("yyyy-MM-dd HH");
            TB_End.Text = endtime.ToString("yyyy-MM-dd HH");
        }
        Bind(start, endtime);
    }
}
