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

public partial class ShowClick : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Request.QueryString["id"]))
            Response.Redirect("http://www.7fshop.com/top/market/qubie.html");

        UserAdsInfo info = new UserAdsService().SelectUserAdsById(new Guid(Request.QueryString["id"]));
        if (info == null)
            Response.Redirect("http://www.7fshop.com/top/market/qubie.html");
        string start = DateTime.Now.AddDays(-14).ToString("yyyyMMdd");
        string end = DateTime.Now.ToString("yyyyMMdd");
        ShowChart(start, end, info);
    }

    private void ShowChart(string start,string end,UserAdsInfo info)
    {
        string id = Request.QueryString["id"];
        IList<ClickInfo> list = new ClickService().SelectAllClickCount(new Guid(id), start, end);

        SeriseText = "[{name:'点击量', data:[";
        DateText = "[";
        for (DateTime h = DateTime.Parse(DateTime.Now.AddDays(-14).ToShortDateString()); h <= DateTime.Parse(DateTime.Now.ToShortDateString()); h = h.AddDays(1))
        {
            DateText += "'" + h.Day + "',";
            IList<ClickInfo> thisInfo = list.Where(o => o.ClickDate == h.ToString("yyyyMMdd")).ToList();


            if (thisInfo.Count == 0)
            {
                if (DateTime.Parse(info.AddTime.ToShortDateString()) <= h)
                    SeriseText += "1,";
                else
                    SeriseText += "0,";
            }
            else
            {
                if (thisInfo[0].ClickCount == 0)
                {
                    if (DateTime.Parse(info.AddTime.ToShortDateString()) <= h)
                        SeriseText += "1,";
                    else
                        SeriseText += thisInfo[0].ClickCount + ",";
                }
                else
                    SeriseText += thisInfo[0].ClickCount + ",";
            }

        }
        SeriseText = SeriseText.Substring(0, SeriseText.Length - 1);
        SeriseText += "]}]";

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

    protected string ShowDate
    {
        get { return DateTime.Now.AddDays(-14).ToString("yyyy-MM-dd") + "至" + DateTime.Now.ToString("yyyy-MM-dd"); }
    }
}
