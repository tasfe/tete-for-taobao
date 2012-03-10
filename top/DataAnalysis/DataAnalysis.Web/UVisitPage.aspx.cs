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
                    darray[0] = DateTime.Parse(Request.QueryString["start"]);
                    darray[1] = DateTime.Parse(Request.QueryString["end"]);
                }
                catch
                {
                }
                if (ViewState["GoodsClassList"] == null)
                    GoodsClassList = TaoBaoAPI.GetGoodsClassInfoList(HttpUtility.UrlDecode(Request.Cookies["nick"].Value), Request.Cookies["nicksession"].Value);
                Bind(darray[0], darray[1]);
            }
        }
    }
    private void Bind(DateTime start, DateTime end)
    {
        int TotalCount = 0;//总记录数
        int TotalPage = 1; //总页数
        string ip = Request.QueryString["visitip"];
        int page = 1;
        try
        {
            page = int.Parse(Request.QueryString["Page"]);
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
                    GoodsInfo rinfo = TaoBaoAPI.GetGoodsInfo(list[i].GoodsId);
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

        lnkFrist.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=1&visitip=" + ip + "&start=" + start.ToString("yyyy-MM-dd HH") + "&end=" + end.ToString("yyyy-MM-dd HH");
        if (!pds.IsFirstPage)
            lnkPrev.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + Convert.ToString(CurPage - 1) + "&visitip=" + ip + "&start=" + start.ToString("yyyy-MM-dd HH") + "&end=" + end.ToString("yyyy-MM-dd HH");

        if (!pds.IsLastPage)
            lnkNext.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + Convert.ToString(CurPage + 1) + "&visitip=" + ip + "&start=" + start.ToString("yyyy-MM-dd HH") + "&end=" + end.ToString("yyyy-MM-dd HH");
        lnkEnd.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + TotalPage + "&visitip=" + ip + "&start=" + start.ToString("yyyy-MM-dd HH") + "&end=" + end.ToString("yyyy-MM-dd HH");

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

        Bind(start, endtime);
    }
}
