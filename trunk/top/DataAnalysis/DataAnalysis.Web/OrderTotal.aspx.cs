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

public partial class OrderTotal : BasePage
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

        SeriseText = "[{name:'订单量', data:[";
        string orderPay = ",{name:'订单金额',data:[";
        string cusPay = ",{name:'每单均价',data:[";
        DateText = "[";
        for (DateTime i = start; i <= end; i = i.AddDays(1))
        {
            DateText += "'" + i.ToString("yyyyMMdd").Substring(6, 2) + "',";
            IList<TopSiteTotalInfo> mylist = list.Where(o => o.SiteTotalDate == i.ToString("yyyyMMdd")).ToList();

            if (mylist.Count > 0)
            {

                SeriseText += mylist[0].SiteOrderCount + ",";
                orderPay += mylist[0].SiteOrderPay + ",";
                if (mylist[0].SiteOrderCount == 0)
                    cusPay += "0,";
                else
                    cusPay += ((double)mylist[0].SiteOrderPay / mylist[0].SiteOrderCount).ToString(".00") + ",";
            }
            else
            {
                orderPay += "0,";
                SeriseText += "0,";
                cusPay += "0,";
            }

        }
        SeriseText = SeriseText.Substring(0, SeriseText.Length - 1);
        SeriseText += "]}";
        orderPay = orderPay.Substring(0, orderPay.Length - 1);
        orderPay += "]}";
        cusPay = orderPay + cusPay.Substring(0, cusPay.Length - 1);
        SeriseText += cusPay + "]}]";

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
            ViewState["start"] = start;
            ViewState["end"] = end;
        }
        catch
        {
            TB_Start.Text = start.ToString("yyyy-MM-dd");
            TB_End.Text = endtime.ToString("yyyy-MM-dd");
        }
        ShowChart(start, end);
    }
}
