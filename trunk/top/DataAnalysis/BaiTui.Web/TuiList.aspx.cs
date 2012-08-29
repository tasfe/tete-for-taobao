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
using System.Collections.Generic;

public partial class TuiList : System.Web.UI.Page
{
    TuiGoodsService tuigDal = new TuiGoodsService();

    PagedDataSource pds = new PagedDataSource();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int type = 1;
            if (!string.IsNullOrEmpty(Request.QueryString["type"]))
            {
                try
                {
                    type = int.Parse(Request.QueryString["type"]);
                }
                catch
                {
                }
            }

            if (type == 1)
                Lbl_TuiType.Text = "百度推广列表";
            if (type == 2)
                Lbl_TuiType.Text = "QQ推广列表";

            string nick = "nick";
            if (Request.Cookies["nick"] != null)
                nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value); //"nick"; 
            else
            {
                if (Session["snick"] != null)
                    nick = Session["snick"].ToString();
            }

            //IList<TuiGoodsInfo> list = tuigDal.GetAllTuiGoodsByType(nick, type);
            //Rpt_TuiList.DataSource = list;
            //Rpt_TuiList.DataBind();
            Bind(type);
        }
    }

    private void Bind(int type)
    {
        int TotalCount = 0;//总记录数
        int TotalPage = 1; //总页数
        string nick = "";
        if (Request.Cookies["nick"] != null)
            nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value); //"nick"; 
        else
            nick = Session["snick"].ToString();
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

        IList<TuiGoodsInfo> list = tuigDal.GetAllTuiGoodsByType(nick, type);

        TotalCount = list.Count;
        pds.DataSource = list;
        pds.AllowPaging = true;
        pds.PageSize = 10;

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

        string paramArray = "&type=" + type;

        lnkFrist.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=1" + paramArray;
        if (!pds.IsFirstPage)
            lnkPrev.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + (page - 1) + paramArray;

        if (!pds.IsLastPage)
            lnkNext.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + (page + 1) + paramArray;
        lnkEnd.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + TotalPage + paramArray;

        Rpt_TuiList.DataSource = pds;
        Rpt_TuiList.DataBind();
    }
}
