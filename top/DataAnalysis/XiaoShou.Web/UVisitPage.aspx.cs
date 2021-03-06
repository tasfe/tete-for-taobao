﻿using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;

public partial class UVisitPage : BasePage
{

    PagedDataSource pds = new PagedDataSource();
    VisitService visitDal = new VisitService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["visitip"]))
            {
                DateTime[] darray = DataHelper.GetDateTime(DateTime.Now, 1);
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

                string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
                string session = Request.Cookies["nicksession"].Value;
                if (ViewState["GoodsClassList"] == null)
                    GoodsClassList = TaoBaoAPI.GetGoodsClassInfoList(nick, Request.Cookies["nicksession"].Value);
                Bind(nick, session, darray[0], darray[1]);
            }
        }
    }
    private void Bind(string nick, string session, DateTime start, DateTime end)
    {
        int TotalCount = 0;//总记录数
        int TotalPage = 1; //总页数
        string ip = Request.QueryString["visitip"];
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

        IList<TopVisitInfo> list = visitDal.GetVisitInfoByIp(DataHelper.Encrypt(HttpUtility.UrlDecode(Request.Cookies["nick"].Value)), ip, start, end);
        List<GoodsInfo> cachegoods = new List<GoodsInfo>();
        if (Cache["taobaogoodslist"] != null)
            cachegoods = (List<GoodsInfo>)Cache["taobaogoodslist"];
        for (int i = 0; i < list.Count; i++)
        {
            if (!string.IsNullOrEmpty(list[i].GoodsId))
            {
                List<GoodsInfo> mylist = cachegoods.Where(o => o.num_iid == list[i].GoodsId).ToList();
                if (mylist.Count == 0)
                {
                    GoodsInfo rinfo = TaoBaoAPI.GetGoodsInfo(nick, session, list[i].GoodsId);
                    list[i].GoodsName = rinfo.title;
                    cachegoods.Add(rinfo);
                    if (Cache["taobaogoodslist"] == null)
                        Cache.Insert("taobaogoodslist", cachegoods, null, DateTime.Now.AddHours(12), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    list[i].GoodsName = mylist[0].title;
                }
            }

            if (!string.IsNullOrEmpty(list[i].GoodsClassId))
            {
                IList<GoodsClassInfo> cList = GoodsClassList.Where(o => o.cid == list[i].GoodsClassId).ToList();
                if (cList.Count > 0)
                    list[i].GoodsClassName = cList[0].name;
            }
        }

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

        lnkFrist.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=1&visitip=" + ip + "&start=" + startstr + "&end=" + endstr;
        if (!pds.IsFirstPage)
            lnkPrev.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + (page - 1) + "&visitip=" + ip + "&start=" + startstr + "&end=" + endstr;

        if (!pds.IsLastPage)
            lnkNext.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + (page + 1) + "&visitip=" + ip + "&start=" + startstr + "&end=" + endstr;
        lnkEnd.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + TotalPage + "&visitip=" + ip + "&start=" + startstr + "&end=" + endstr;

        Rpt_PageVisit.DataSource = pds;
        Rpt_PageVisit.DataBind();
        TB_Start.Text = start.ToString("yyyy-MM-dd HH");
        TB_End.Text = end.ToString("yyyy-MM-dd HH");
    }

    protected IList<GoodsClassInfo> GoodsClassList
    {
        set { ViewState["GoodsClassList"] = value; }
        get
        {
            return (IList<GoodsClassInfo>)ViewState["GoodsClassList"];
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
            start = DateTime.Parse(TB_Start.Text + ":0:0");
            endtime = DateTime.Parse(TB_End.Text + ":0:0");
        }
        catch
        {
            TB_Start.Text = start.ToString("yyyy-MM-dd HH");
            TB_End.Text = endtime.ToString("yyyy-MM-dd HH");
        }
        ViewState["page"] = "1";
        string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
        string session = Request.Cookies["nicksession"].Value;
        Bind(nick, session, start, endtime);
    }

    protected string GetSubUrl(string url)
    {
        return url.Length > 60 ? url.Substring(0, 60) + "..." : url;
    }
}
