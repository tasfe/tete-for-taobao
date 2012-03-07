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

public partial class PingjiaTotal : System.Web.UI.Page
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
        TaoBaoGoodsOrderService tboDal = new TaoBaoGoodsOrderService();
        IList<PingJiaInfo> list = tboDal.GetPingjiaTotal(start, end, HttpUtility.UrlDecode(Request.Cookies["nick"].Value));

        SeriseText = "[{name:'好评', data:[";
        string zhong = ",{name:'中评',data:[";
        string cha = ",{name:'差评',data:[";
        string nop = ",{name:'尚未评价',data:[";
        DateText = "[";
        for (DateTime i = start; i <= end; i = i.AddDays(1))
        {
            DateText += "'" + i.ToString("yyyyMMdd").Substring(6, 2) + "',";
            IList<PingJiaInfo> mylist = list.Where(o=> o.pdate == i.ToString("yyyyMMdd")).ToList();

            if (mylist.Count > 0) 
            {
                if (mylist.Where(o => o.result == "good").ToList().Count > 0)
                {
                    SeriseText += mylist[0].pcount + ",";
                }
                else
                {
                    SeriseText += "0,";
                }
                if (mylist.Where(o => o.result == "neutral").ToList().Count > 0)
                {
                    zhong += mylist[0].pcount + ",";
                }
                else
                {
                    zhong += "0,";
                }
                if (mylist.Where(o => o.result == "bad").ToList().Count > 0)
                {
                    cha += mylist[0].pcount + ",";
                }
                else
                {
                    cha += "0,";
                }
                if (mylist.Where(o => o.result == "").ToList().Count > 0)
                {
                    nop += mylist[0].pcount + ",";
                }
                else
                {
                    nop += "0,";
                }
            }
            else
            {
                zhong += "0,";
                SeriseText += "0,";
                cha += "0,";
                nop += "0,";
            }
            
        }
        SeriseText = SeriseText.Substring(0, SeriseText.Length - 1);
        SeriseText += "]}";
        zhong = zhong.Substring(0, zhong.Length - 1);
        zhong += "]}";
        cha = zhong + cha.Substring(0, cha.Length - 1);
        cha += "]}";
        nop = cha + nop.Substring(0, nop.Length - 1);
        SeriseText += nop + "]}]";

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
        ShowChart(DateTime.Parse(DateTime.Now.AddDays(-2).ToShortDateString()), DateTime.Now);
    }
    protected void Btn_7Days_Click(object sender, EventArgs e)
    {
        ShowChart(DateTime.Parse(DateTime.Now.AddDays(-6).ToShortDateString()), DateTime.Now);
    }
    protected void Btn_30Days_Click(object sender, EventArgs e)
    {
        ShowChart(DateTime.Parse(DateTime.Now.AddDays(-29).ToShortDateString()), DateTime.Now);
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
