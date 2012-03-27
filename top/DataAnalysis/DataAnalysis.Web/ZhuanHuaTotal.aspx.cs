﻿using System;
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

public partial class ZhuanHuaTotal : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ShowChart(DateTime.Now.AddDays(-7), DateTime.Now);
        }
    }

    private void ShowChart(DateTime start, DateTime end)
    {
        //两个时间差
        if (start > end)
        {
            Page.RegisterStartupScript("error", "<script>alert('时间选择有误!');</script>");
            return;
        }

        string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
        IList<TopSiteTotalInfo> list = new SiteTotalService().GetZhuanHuaTotal(start.ToString("yyyyMMdd"), end.ToString("yyyyMMdd"), nick);
        SeriseText = "[{name:'下单人数', data:[";
        string sucss = ",{name:'访客数', data:[";
        //string onesell = ",{name:'转化率',data:[";
        
        DateText = "[";

        for (DateTime i = start; i <= end; i = i.AddDays(1))
        {
            DateText += "'" + i.ToString("yyyyMMdd").Substring(6, 2) + "',";
            IList<TopSiteTotalInfo> mylist = list.Where(o => o.SiteTotalDate == i.ToString("yyyyMMdd")).ToList();
            if (mylist.Count == 0)
            {
                SeriseText += "0,";
                sucss += "0,";
                //onesell += "0,";
            }
            else
            {
                SeriseText += mylist.Sum(o => o.SiteBuyCustomTotal) + ",";
                sucss += mylist.Sum(o => o.SiteUVCount) + ",";
                //onesell += Math.Round(((decimal)mylist.Sum(o => o.SiteBuyCustomTotal) / mylist.Sum(o => o.SiteUVCount)) * 100, 2).ToString() + "%,";
            }
        }

        SeriseText = SeriseText.Substring(0, SeriseText.Length - 1) + "]}";
        //onesell = onesell.Substring(0, onesell.Length - 1) + "]}";
        //SeriseText = SeriseText + onesell + sucss.Substring(0, sucss.Length - 1) + "]}]";
        SeriseText = SeriseText + sucss.Substring(0, sucss.Length - 1) + "]}]";

        DateText = DateText.Substring(0, DateText.Length - 1);
        DateText += "]";
        TB_Start.Text = start.ToString("yyyy-MM-dd");
        TB_End.Text = end.ToString("yyyy-MM-dd");
        Rpt_Zhuan.DataSource = list;
        Rpt_Zhuan.DataBind();
    }

    protected string DateText
    {
        get { return ViewState["dateText"].ToString(); }
        set { ViewState["dateText"] = value; }
    }

    protected string SeriseText
    {
        get { return ViewState["seriseText"].ToString(); }
        set { ViewState["seriseText"] = value; }
    }

    protected void Btn_3Days_Click(object sender, EventArgs e)
    {
        ShowChart(DateTime.Now.AddDays(-2), DateTime.Now);
    }
    protected void Btn_7Days_Click(object sender, EventArgs e)
    {
        ShowChart(DateTime.Now.AddDays(-6), DateTime.Now);
    }
    protected void Btn_30Days_Click(object sender, EventArgs e)
    {
        ShowChart(DateTime.Now.AddDays(-29), DateTime.Now);
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
        }
        ShowChart(start, endtime);
    }

    protected string GetDate(string date)
    {
        return date.Substring(0, 4) + "-" + date.Substring(4, 2) + "-" + date.Substring(6);
    }

    protected string GetZhuan(string buycount, string seecount)
    {
        if (seecount == "0") return "0";
        return Math.Round((decimal.Parse(buycount) / int.Parse(seecount)) * 100, 2).ToString() + "%";
    }
}
