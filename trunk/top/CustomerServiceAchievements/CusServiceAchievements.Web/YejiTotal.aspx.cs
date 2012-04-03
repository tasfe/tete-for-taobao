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
using Model;
using CusServiceAchievements.DAL;

public partial class YejiTotal : BasePage
{
    TopKefuTotalService kftDal = new TopKefuTotalService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
            DateTime[] drrArr = DBHelp.DataHelper.GetDateTime(DateTime.Now.AddDays(-29),31);
            IList<TopKefuTotalInfo> list = kftDal.GetTotalinfoList(drrArr[0], drrArr[1], nick);
            Rpt_KefuTotal.DataSource = list;
            Rpt_KefuTotal.DataBind();
            TB_Start.Text = drrArr[0].ToString("yyyy-MM-dd");
            TB_End.Text = drrArr[1].ToString("yyyy-MM-dd");
        }
    }

    protected void Btn_Select_Click(object sender, EventArgs e)
    {
        DateTime now = DateTime.Now.AddDays(-29);
        DateTime end = now.AddDays(31);
        DateTime start = new DateTime(now.Year, now.Month, now.Day);
        DateTime endtime = new DateTime(end.Year, end.Month, end.Day);
        try
        {
            start = DateTime.Parse(TB_Start.Text);
            endtime = DateTime.Parse(TB_End.Text);
            TB_Start.Text = start.ToString("yyyy-MM-dd");
            TB_End.Text = endtime.ToString("yyyy-MM-dd");
        }
        catch
        {
            TB_Start.Text = start.ToString("yyyy-MM-dd");
            TB_End.Text = endtime.ToString("yyyy-MM-dd");
        }
        string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
        IList<TopKefuTotalInfo> list = kftDal.GetTotalinfoList(start, endtime, nick);
        Rpt_KefuTotal.DataSource = list;
        Rpt_KefuTotal.DataBind();
    }
    protected void Btn_LastMonth_Click(object sender, EventArgs e)
    {
        DateTime lastmonth = DateTime.Now.AddMonths(-1);
        DateTime start = new DateTime(lastmonth.Year,lastmonth.Month,1);
        string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
        IList<TopKefuTotalInfo> list = kftDal.GetTotalinfoList(start, start.AddMonths(1).AddDays(-1), nick);
        Rpt_KefuTotal.DataSource = list;
        Rpt_KefuTotal.DataBind();
        TB_Start.Text = start.ToString("yyyy-MM-dd");
        TB_End.Text = start.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
    }
}
