using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PaiPai.DAL;
using PaiPai.Model;

public partial class msglog : System.Web.UI.Page
{

    PagedDataSource pds = new PagedDataSource();
    PostMsgRecordService pmrDal = new PostMsgRecordService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.Cookies["nick"] != null)
                Nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value); //"nick"; 
            else
                Nick = Session["snick"].ToString();
            if (Nick == "")
            {
                Response.Redirect("http://fuwu.paipai.com/appstore/ui/my/app/appdetail.xhtml?appId=234454");
            }

            int type=5;
            if (!string.IsNullOrEmpty(Request.QueryString["type"]))
                type = int.Parse(Request.QueryString["type"]);

            if (type == 1)
            {
                ViewState["PayCss"] = "current";
            }
            if (type == 5)
            {
                ViewState["PostCss"] = "current";
            }
            if (type == 9)
            {
                ViewState["PingCss"] = "current";
            }

            PaiPaiShopService ppsDal = new PaiPaiShopService();
            PaiPaiShopInfo info = ppsDal.GetPaiPaiShopInfo(Nick);

            CanPoCount = info.MessgeCount.ToString();
            HadPoCount = info.HadPost.ToString();
            DateTime start;
            DateTime end;
            if (string.IsNullOrEmpty(Request.QueryString["start"]))
            {
                start = DateTime.Now.AddDays(-15);
                end = DateTime.Now.AddDays(1);
            }
            else
            {
                start = DateTime.Parse(Request.QueryString["start"]);
                end = DateTime.Parse(Request.QueryString["end"]);
                if (end.Day == DateTime.Now.Day)
                    end = end.AddDays(1);
            }
            ViewState["type"] = type;
            Bind(Request.QueryString["uid"], start, end, type);
        }
    }

    private void Bind(string buyer, DateTime start, DateTime end, int type)
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

        IList<PostMsgRecordInfo> list = new List<PostMsgRecordInfo>();
        if (string.IsNullOrEmpty(buyer))
            list = pmrDal.GetMsgRecords(nick, start, end, type);
        else
            list = pmrDal.GetMsgRecords(nick, buyer, start, end, type);
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
        if (!string.IsNullOrEmpty(buyer))
            paramArray += "&uid=" + buyer;
        if (start != DateTime.MinValue)
            paramArray += "&start=" + start.ToShortDateString();
        if (end != DateTime.MinValue)
            paramArray += "&end=" + end.ToShortDateString();

        lnkFrist.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=1" + paramArray;
        if (!pds.IsFirstPage)
            lnkPrev.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + (page - 1) + paramArray;

        if (!pds.IsLastPage)
            lnkNext.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + (page + 1) + paramArray;
        lnkEnd.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + TotalPage + paramArray;

        TB_StartTime.Text = start.ToShortDateString();
        TB_EndTime.Text = end.ToShortDateString();
        Rpt_Msg.DataSource = pds;
        Rpt_Msg.DataBind();
    }

    protected string PostCss
    {
        get
        {
            return ViewState["PostCss"] == null ? "" : ViewState["PostCss"].ToString();
        }
    }

    protected string PingCss
    {
        get
        {
            return ViewState["PingCss"] == null ? "" : ViewState["PingCss"].ToString();
        }
    }

    protected string PayCss
    {
        get
        {
            return ViewState["PayCss"] == null ? "" : ViewState["PayCss"].ToString();
        }
    }

    protected string Nick
    {
        set;
        get;
    }

    protected string CanPoCount
    {
        set;
        get;
    }

    protected string HadPoCount
    {
        set;
        get;
    }
    protected void Btn_Select_Click(object sender, EventArgs e)
    {
        int type = int.Parse(ViewState["type"].ToString());
        ViewState["page"] = 1;
        Bind(txtQQ.Text.Trim(), DateTime.Parse(TB_StartTime.Text), DateTime.Parse(TB_EndTime.Text), type);
    }
}