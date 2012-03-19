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

public partial class YejiTotal : System.Web.UI.Page
{
    TopKefuTotalService kftDal = new TopKefuTotalService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DateTime[] drrArr = DBHelp.DataHelper.GetDateTime(DateTime.Now.AddDays(-29),31);
            IList<TopKefuTotalInfo> list = kftDal.GetTotalinfoList(drrArr[0], drrArr[1]);
            Rpt_KefuTotal.DataSource = list;
            Rpt_KefuTotal.DataBind();
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
        }
        catch
        {
            TB_Start.Text = start.ToString("yyyy-MM-dd");
            TB_End.Text = endtime.ToString("yyyy-MM-dd");
        }

        IList<TopKefuTotalInfo> list = kftDal.GetTotalinfoList(start, endtime);
        Rpt_KefuTotal.DataSource = list;
        Rpt_KefuTotal.DataBind();
    }
}
