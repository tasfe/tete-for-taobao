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

public partial class GroupReceiveCustomer : System.Web.UI.Page
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
        List<TopKefuTotalInfo> list = (List<TopKefuTotalInfo>)new TopKefuTotalService().GetNickCountTotal(date.ToString("yyyyMMdd"));
        SeriseText = "[{name:'接待人次', data:[";
        DateText = "[";
      
        for (int h = 0; h <= list.Count; h++)
        {
            DateText += "'" + list[h].Nick + "',";

            SeriseText += list[h].CustomerCount + ",";
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
