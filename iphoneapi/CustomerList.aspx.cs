using System;
using System.Collections;
using System.Web;
using System.Collections.Generic;
using System.Web.UI.WebControls;

public partial class CustomerList : BasePage
{

    TeteUserTokenService tutDal = new TeteUserTokenService();
    PagedDataSource pds = new PagedDataSource();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Bind();
        }
    }

    private void Bind()
    {
        string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);

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

        IList<TeteUserTokenInfo> list = tutDal.GetAllTeteUserToken(nick);
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

        lnkFrist.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=1";
        if (!pds.IsFirstPage)
            lnkPrev.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + (page - 1);

        if (!pds.IsLastPage)
            lnkNext.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + (page + 1);
        lnkEnd.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + TotalPage;

        Rpt_CustomerList.DataSource = pds;
        Rpt_CustomerList.DataBind();
    }
}
