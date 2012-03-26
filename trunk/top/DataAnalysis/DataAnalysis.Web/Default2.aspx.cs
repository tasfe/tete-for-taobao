using System;
using System.Collections;
using System.Web;
using System.Collections.Generic;

public partial class Default2 : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            DateTime now = DateTime.Parse(DateTime.Now.ToShortDateString());

            string nickNo = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
            IList<TopSiteTotalInfo> siteTotalList = new SiteTotalService().GetNickOrderTotal(now.AddDays(-10), now, nickNo);
            Rpt_TotalList.DataSource = siteTotalList;
            Rpt_TotalList.DataBind();

        }
    }

    protected string GetMonthDay(string date)
    {
        string s = date.Substring(4);

        if (s.Substring(0, 1) == "0")
        {
            if (s.Substring(2, 1) == "0")
                return s.Substring(1, 1) + "/" + s.Substring(3);
            else
                return s.Substring(1, 1) + "/" + s.Substring(2);
        }
        else
        {
            if (s.Substring(2, 1) == "0")
                return s.Substring(0, 2) + "/" + s.Substring(3);
            else
                return s.Substring(0, 2) + "/" + s.Substring(2);
        }
    }

    private void InsertName()
    {
        List<TotalNameInfo> list = new List<TotalNameInfo>();
        list.Add(new TotalNameInfo { TotalName = "PV" });
        list.Add(new TotalNameInfo { TotalName = "UV" });
        list.Add(new TotalNameInfo { TotalName = "直通车流量" });
        list.Add(new TotalNameInfo { TotalName = "推广费用" });
        list.Add(new TotalNameInfo { TotalName = "CPC" });
        list.Add(new TotalNameInfo { TotalName = "询单数" });
        list.Add(new TotalNameInfo { TotalName = "丢单率" });
        list.Add(new TotalNameInfo { TotalName = "订单量" });

        list.Add(new TotalNameInfo { TotalName = "实际销售额" });
        list.Add(new TotalNameInfo { TotalName = "销售均价" });
        list.Add(new TotalNameInfo { TotalName = "客单价" });
        list.Add(new TotalNameInfo { TotalName = "平均转化率" });
        list.Add(new TotalNameInfo { TotalName = "浏览回头客" });

        list.Add(new TotalNameInfo { TotalName = "退款率" });
        list.Add(new TotalNameInfo { TotalName = "平均访问深度" });
        list.Add(new TotalNameInfo { TotalName = "流量排行" });
        list.Add(new TotalNameInfo { TotalName = "销售排行" });
        list.Add(new TotalNameInfo { TotalName = "收藏量" });

        //Rpt_Data.DataSource = list;
        //Rpt_Data.DataBind();
    }

    protected string GetGoodsName(string goodsName)
    {
        if (goodsName.Length > 2)
            return goodsName.Substring(0, 2) + ".";
        return goodsName;
    }
    protected void Btn_AddCookie_Click(object sender, EventArgs e)
    {
        HttpCookie cookie = new HttpCookie("nick", HttpUtility.UrlEncode("luckyfish8800"));
        cookie.Expires = DateTime.Now.AddDays(-1);
        Request.Cookies.Add(cookie);

        HttpCookie newcookie = new HttpCookie("nick", HttpUtility.UrlEncode("luckyfish8800"));
        newcookie.Expires = DateTime.Now.AddDays(1);
        Request.Cookies.Add(newcookie);
    }
}

public class TotalNameInfo
{
    public string TotalName { set; get; }
}
