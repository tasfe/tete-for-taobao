using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Collections.Generic;

public partial class HourOrdersTotal : BasePage
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
            if (!VisitService.CheckTable(DataHelper.Encrypt(HttpUtility.UrlDecode(Request.Cookies["nick"].Value))))
            {
                Response.Redirect("CreateCode.aspx");
                //Response.Write("<script>alert('抱歉,您还没有添加统计代码!');</script>");
                //Response.End();
            }
            else
                ShowChart(DateTime.Now);
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
        TaoBaoGoodsOrderService orderDal = new TaoBaoGoodsOrderService();
        IList<GoodsOrderInfo> list = orderDal.GetHourOrderTotal(DateTime.Parse(date.ToShortDateString()), DateTime.Parse(date.AddDays(1).ToShortDateString()), HttpUtility.UrlDecode(Request.Cookies["nick"].Value));

        SeriseText = "[{name:'订单量', data:[";
        DateText = "[";
        int nowhour = 23;
        if (date.ToShortDateString() == DateTime.Now.ToShortDateString())
            nowhour = date.Hour;
        for (int h = 0; h <= nowhour; h++)
        {
            DateText += "'" + h + "',";
            IList<GoodsOrderInfo> thisInfo = list.Where(o => o.OrderTotal == h).ToList();

            if (thisInfo.Count == 0)
                SeriseText += "0,";
            else
                SeriseText += (int)thisInfo[0].payment + ",";
        }
        SeriseText = SeriseText.Substring(0, SeriseText.Length - 1);
        SeriseText += "]}]";

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
