using System;
using System.Linq;
using System.Web;
using TaoBaoAPIHelper;
using System.Collections.Generic;
using CusServiceAchievements.DAL;

public partial class ReceiveCustomer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ShowChart(DateTime.Now);
        }
    }

    private void ShowChart(DateTime date)
    {
        string nick =HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
        List<TalkContent> list = (List<TalkContent>)new TalkRecodService().GetTalkTotalHour(nick, date, date.AddDays(1));
        SeriseText = "[{name:'接待人次', data:[";
        DateText = "[";
        int nowhour = 23;
        if (date.ToShortDateString() == DateTime.Now.ToShortDateString())
            nowhour = date.Hour;
        for (int h = 0; h <= nowhour; h++)
        {
            DateText += "'" + h + "',";
            List<TalkContent> mylist = list.Where(o => o.time == h.ToString()).ToList();

            SeriseText += mylist.Count + ",";
        }

        SeriseText = SeriseText.Substring(0, SeriseText.Length - 1) + "]}]";

        DateText = DateText.Substring(0, DateText.Length - 1);
        DateText += "]";
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
}
