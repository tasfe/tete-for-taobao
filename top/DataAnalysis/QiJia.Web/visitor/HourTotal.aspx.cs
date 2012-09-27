using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Web;

public partial class HourPVTotal : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //int day = 1;
            //try
            //{
            //    if (Request.QueryString["days"] != null)
            //    {
            //        day = int.Parse(Request.QueryString["days"]);
            //    }
            //}
            //catch { }
            if (!VisitService.CheckTable(Encrypt(HttpUtility.UrlDecode(Request.Cookies["nick"].Value))))
            {
                //Response.Redirect("CreateCode.aspx");
                Response.Write("暂时没有人访问!");
            }
            else
            {
                DateTime now = DateTime.Now;
                if (!string.IsNullOrEmpty(Request.QueryString["day"]))
                {
                    string date = Request.QueryString["day"];
                    if (now.ToString("yyyyMMdd") != date)
                    {
                        date = date.Substring(0, 4) + "-" + date.Substring(4, 2) + "-" + date.Substring(6);
                        now = DateTime.Parse(date);
                    }
                }
                ShowChart(now);
            }
        }
    }

    private void ShowChart(DateTime date)
    {

        if (date.ToShortDateString() != DateTime.Now.ToShortDateString())
            Btn_Totay.Visible = true;
        //大于今天
        if (DateTime.Parse(date.ToShortDateString()) > DateTime.Parse(DateTime.Now.ToShortDateString()))
        {
            TB_Start.Text = HF_Date.Value;
            return;
        }
        VisitService visitDal = new VisitService();
        IList<HourTotalInfo> list = visitDal.GetHourPVTotal(Encrypt(HttpUtility.UrlDecode(Request.Cookies["nick"].Value)), date);
        IList<HourTotalInfo> ipList = visitDal.GetHourIPTotal(Encrypt(HttpUtility.UrlDecode(Request.Cookies["nick"].Value)), date);

        SeriseText = "[{name:'PV量', data:[";
        string iptotal = ",{name:'IP量',data:[";
        string avg = ",{name:'人均浏览次数',data:[";
        DateText = "[";
        int nowhour = 23;
        if (date.ToShortDateString() == DateTime.Now.ToShortDateString())
            nowhour = date.Hour;
        for (int h = 0; h <= nowhour; h++)
        {
            DateText += "'" + h + "',";
            IList<HourTotalInfo> thisInfo = list.Where(o => o.Hour == h).ToList();
            IList<HourTotalInfo> thisIpInfo = ipList.Where(o => o.Hour == h).ToList();

            if (thisIpInfo.Count == 0)
                iptotal += "0,";
            else
                iptotal += thisIpInfo[0].PVCount + ",";

            if (thisInfo.Count == 0)
                SeriseText += "0,";
            else
                SeriseText += thisInfo[0].PVCount + ",";

            if (thisIpInfo.Count == 0)
                avg += "0,";
            else
                avg += ((double)thisInfo[0].PVCount / thisIpInfo[0].PVCount).ToString(".00") + ",";

        }
        SeriseText = SeriseText.Substring(0, SeriseText.Length - 1);
        SeriseText += "]}";
        iptotal = iptotal.Substring(0, iptotal.Length - 1);
        iptotal += "]}";
        avg = iptotal + avg.Substring(0, avg.Length - 1);
        SeriseText += avg + "]}]";

        DateText = DateText.Substring(0, DateText.Length - 1);
        DateText += "]";
        TB_Start.Text = date.ToShortDateString();
        HF_Date.Value = date.ToShortDateString(); 
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
        try
        {
            now = DateTime.Parse(TB_Start.Text);
            if (now.ToShortDateString() == DateTime.Now.ToShortDateString())
                now = DateTime.Now;
        }
        catch { TB_Start.Text = now.ToShortDateString(); }
        ShowChart(now);
    }
    protected void Btn_LastDays_Click(object sender, EventArgs e)
    {
        ShowChart(DateTime.Now.AddDays(-1));
    }
    protected void Btn_Totay_Click(object sender, EventArgs e)
    {
        ShowChart(DateTime.Now);
    }
}
