using System;
using System.Collections;
using System.Linq;
using System.Web.UI;
using System.Collections.Generic;
using System.Web;

public partial class VisitTotal : BasePage
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
        SiteTotalService stDal = new SiteTotalService();
        IList<TopSiteTotalInfo> list = stDal.GetNickOrderTotal(start, end, HttpUtility.UrlDecode(Request.Cookies["nick"].Value));

        SeriseText = "[{name:'PV量', data:[";
        string uvtotal = ",{name:'UV量',data:[";
        string backtotal = ",{name:'浏览回头客',data:[";
        DateText = "[";
        for (DateTime i = start; i <= end; i = i.AddDays(1))
        {
            DateText += "'" + i.ToString("yyyyMMdd").Substring(6, 2) + "',";
            IList<TopSiteTotalInfo> mylist = list.Where(o => o.SiteTotalDate == i.ToString("yyyyMMdd")).ToList();

            if (mylist.Count > 0)
            {
                SeriseText += mylist[0].SitePVCount + ",";
                uvtotal += mylist[0].SiteUVCount + ",";
                backtotal += mylist[0].SiteUVBack + ",";
            }
            else
            {
                uvtotal += "0,";
                SeriseText += "0,";
                backtotal += "0,";
            }

        }
        SeriseText = SeriseText.Substring(0, SeriseText.Length - 1);
        SeriseText += "]}";
        uvtotal = uvtotal.Substring(0, uvtotal.Length - 1);
        uvtotal += "]}";
        backtotal = uvtotal + backtotal.Substring(0, backtotal.Length - 1);
        SeriseText += backtotal + "]}]";

        DateText = DateText.Substring(0, DateText.Length - 1);
        DateText += "]";
        TB_Start.Text = start.ToShortDateString();
        TB_End.Text = end.ToShortDateString();
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
            TB_Start.Text = start.ToString("yyyy-MM-dd");
            TB_End.Text = endtime.ToString("yyyy-MM-dd");
        }
        ShowChart(start, endtime);
    }
}
