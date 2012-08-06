using System;
using System.Collections;
using System.Linq;
using System.Web.UI;
using System.Collections.Generic;
using System.Web;

public partial class BackTotal : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!VisitService.CheckTable(DataHelper.Encrypt(HttpUtility.UrlDecode(Request.Cookies["nick"].Value))))
            {
                Response.Redirect("CreateCode.aspx");
                //Response.Write("<script>alert('抱歉,您还没有添加统计代码!');</script>");
                //Response.End();
            }
            else
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
        TaoBaoGoodsOrderService orderDal = new TaoBaoGoodsOrderService();
        IList<BackTotalInfo> list = orderDal.GetAllBackTotalList(start, end, HttpUtility.UrlDecode(Request.Cookies["nick"].Value));

        SeriseText = "[{name:'完成订单量', data:[";
        string iptotal = ",{name:'回头客数量',data:[";
        string avg = ",{name:'回头客占比',data:[";
        DateText = "[";
        for (DateTime i = start; i <= end; i=i.AddDays(1))
        {
            DateText += "'" + i.ToString("yyyyMMdd").Substring(6, 2) + "',";
            IList<BackTotalInfo> mylist = list.Where(o => o.PDate == i.ToString("yyyyMMdd")).ToList();
            if (mylist.Count > 0)
            {
                BackTotalInfo info = mylist[0];
                iptotal += info.PCount + ",";
                SeriseText += info.AllCount + ",";
                if (info.AllCount == 0)
                    avg += "0,";
                else
                    avg += ((double)info.PCount / info.AllCount).ToString(".00") + ",";
            }
            else
            {
                iptotal += "0,";
                SeriseText += "0,";
                avg += "0,";
            }
        }
        SeriseText = SeriseText.Substring(0, SeriseText.Length - 1);
        SeriseText += "]}";
        iptotal = iptotal.Substring(0, iptotal.Length - 1);
        iptotal += "]}";
        avg = iptotal + avg.Substring(0, avg.Length - 1);
        SeriseText += avg + "]}]";

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

    protected void Btn_3Days_Click(object sender, EventArgs e)
    {
        ShowChart(DateTime.Parse (DateTime.Now.AddDays(-2).ToShortDateString()),DateTime.Now);
    }
    protected void Btn_7Days_Click(object sender, EventArgs e)
    {
        ShowChart(DateTime.Parse(DateTime.Now.AddDays(-6).ToShortDateString()), DateTime.Now);
    }
    protected void Btn_30Days_Click(object sender, EventArgs e)
    {
        ShowChart(DateTime.Parse(DateTime.Now.AddDays(-29).ToShortDateString()), DateTime.Now);
    }
}
