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
using TaoBaoAPIHelper;
using System.Collections.Generic;
using CusServiceAchievements.DAL;
using Model;

public partial class GroupReceiveCustomer : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ShowChart(DateTime.Now.AddDays(-1));
        }
    }

    private void ShowChart(DateTime date)
    {
        string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
        List<TopKefuTotalInfo> list = (List<TopKefuTotalInfo>)new TopKefuTotalService().GetNickCountTotal(date.ToString("yyyyMMdd"), nick);
        SeriseText = "[{name:'接待人次', data:[";
        DateText = "[";
      
        for (int h = 0; h < list.Count; h++)
        {
            DateText += "'" + (list[h].Nick.IndexOf(':') >= 0 ? list[h].Nick.Substring(list[h].Nick.IndexOf(':') + 1) : "主旺旺") + "',";

            SeriseText += list[h].CustomerCount + ",";
        }

        SeriseText = SeriseText.Substring(0, SeriseText.Length - 1) + "]}]";

        DateText = DateText.Substring(0, DateText.Length - 1);
        DateText += "]";
        TB_Start.Text = date.ToString("yyyy-MM-dd");
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
        }
        catch
        {
            TB_Start.Text = now.ToString("yyyy-MM-dd");
        }

        ShowChart(now);
    }

}
