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

public partial class ZhiTongHourPV : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!VisitService.CheckTable(DataHelper.Encrypt(HttpUtility.UrlDecode(Request.Cookies["nick"].Value))))
            {
                Response.Redirect("CreateCode.aspx");
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
                    };
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
        IList<HourTotalInfo> list = visitDal.GetHourZhiTongOrZuanZhanPVTotal(DataHelper.Encrypt(HttpUtility.UrlDecode(Request.Cookies["nick"].Value)), date, true);

        IList<HourTotalInfo> zuanlist = visitDal.GetHourZhiTongOrZuanZhanPVTotal(DataHelper.Encrypt(HttpUtility.UrlDecode(Request.Cookies["nick"].Value)), date, false);


        SeriseText = "[{name:'直通车量', data:[";
        string iptotal = ",{name:'钻展流量',data:[";
        DateText = "[";
        int nowhour = 23;
        if (date.ToShortDateString() == DateTime.Now.ToShortDateString())
            nowhour = date.Hour;
        for (int h = 0; h <= nowhour; h++)
        {
            DateText += "'" + h + "',";
            IList<HourTotalInfo> thisInfo = list.Where(o => o.Hour == h).ToList();
            IList<HourTotalInfo> thiszuanInfo = list.Where(o => o.Hour == h).ToList();

            if (thisInfo.Count == 0)
                SeriseText += "0,";
            else
                SeriseText += thisInfo[0].PVCount + ",";

            if (thiszuanInfo.Count == 0)
                iptotal += "0,";
            else
                iptotal += thiszuanInfo[0].PVCount + ",";

        }
        SeriseText = SeriseText.Substring(0, SeriseText.Length - 1) + "]}";
        iptotal = iptotal.Substring(0, iptotal.Length - 1) + "]}";
        SeriseText = SeriseText + iptotal + "]";

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
