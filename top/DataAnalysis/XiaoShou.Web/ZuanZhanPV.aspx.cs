using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;

public partial class ZuanZhanPV : BasePage
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
        List<TopSiteTotalInfo> list = new SiteTotalService().GetZuanZhanTotal(start.ToString("yyyyMMdd"), end.ToString("yyyyMMdd"), nick);
        SeriseText = "[{name:'钻展流量', data:[";
        DateText = "[";

        for (DateTime i = start; i <= end; i = i.AddDays(1))
        {
            DateText += "'" + i.ToString("yyyyMMdd").Substring(6, 2) + "',";
            IList<TopSiteTotalInfo> mylist = list.Where(o => o.SiteTotalDate == i.ToString("yyyyMMdd")).ToList();
            if (mylist.Count == 0)
                SeriseText += "0,";
            else
                SeriseText += mylist[0].SiteZuanZhan + ",";
        }

        SeriseText = SeriseText.Substring(0, SeriseText.Length - 1) + "]}]";

        DateText = DateText.Substring(0, DateText.Length - 1);
        DateText += "]";
        TB_Start.Text = start.ToString("yyyy-MM-dd");
        TB_End.Text = end.ToString("yyyy-MM-dd");
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
}
