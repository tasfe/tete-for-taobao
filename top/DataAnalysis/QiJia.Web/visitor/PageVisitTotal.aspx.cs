using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web;

public partial class PageVisitTotal : BasePage
{

    readonly VisitService visitDal = new VisitService();
    PagedDataSource pds = new PagedDataSource();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (!VisitService.CheckTable(Encrypt(HttpUtility.UrlDecode(Request.Cookies["nick"].Value))))
            {
                //Response.Redirect("CreateCode.aspx");
                //Response.Write("<script>alert('抱歉,您还没有添加统计代码!');</script>");
                //Response.End();
                Response.Write("暂时没有人访问!");
                Response.End();
            }
            else
            {
                DateTime[] darray = GetDateTime(DateTime.Now, 1);
                try
                {
                    string start = HttpUtility.UrlDecode(Request.QueryString["start"]);
                    string end = HttpUtility.UrlDecode(Request.QueryString["end"]);
                    darray[0] = DateTime.Parse(start.Substring(0, start.LastIndexOf('-')) + " " + start.Substring(start.LastIndexOf('-') + 1, 2) + ":0:0");
                    darray[1] = DateTime.Parse(end.Substring(0, end.LastIndexOf('-')) + " " + end.Substring(end.LastIndexOf('-') + 1, 2) + ":0:0");
                }
                catch
                {
                }
                Bind(darray[0], darray[1]);
            }
        }
    }

    protected string GetLastMonth
    {
        get { return DateTime.Now.AddMonths(-1).ToShortDateString(); }
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

        IList<PageVisitInfoTotal> list = visitDal.GetAllVisitPageInfoList(Encrypt(HttpUtility.UrlDecode(Request.Cookies["nick"].Value)), start, end);
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

        string startstr = HttpUtility.UrlEncode(start.ToString("yyyy-MM-dd-HH"));
        string endstr = HttpUtility.UrlEncode(end.ToString("yyyy-MM-dd-HH"));

        pds.CurrentPageIndex = page - 1;
        lblCurrentPage.Text = "共" + TotalCount.ToString() + "条记录 当前页：" + page + "/" + TotalPage;

        lnkFrist.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=1" + "&start=" + startstr + "&end=" + endstr;
        if (!pds.IsFirstPage)
            lnkPrev.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + (page - 1) + "&start=" + startstr + "&end=" + endstr;

        if (!pds.IsLastPage)
            lnkNext.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + (page + 1) + "&start=" + startstr + "&end=" + endstr;
        lnkEnd.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + TotalPage + "&start=" + startstr + "&end=" + endstr;

        Rpt_PageVisit.DataSource = pds;
        Rpt_PageVisit.DataBind();
        TB_Start.Text = start.ToString("yyyy-MM-dd HH");
        TB_End.Text = end.ToString("yyyy-MM-dd HH");
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
        ViewState["page"] = "1";
        Bind(start, endtime);
    }

    protected string GetSubUrl(string url)
    {
        return url.Length > 60 ? url.Substring(0, 60) + "..." : url;
    }
}
