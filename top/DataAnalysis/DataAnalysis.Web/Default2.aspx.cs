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

public partial class Default2 : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            DateTime now = DateTime.Parse(DateTime.Now.ToShortDateString());
            List<TotalDateInfo> datelist = new List<TotalDateInfo>();

            for (DateTime i = now; i > now.AddDays(-14); i = now.AddDays(-1))
            {
                TotalDateInfo info = new TotalDateInfo();
                info.TotalDate = i.Day.ToString();
                datelist.Add(info);
            }

            //Rpt_Date.DataSource = datelist;
            //Rpt_Date.DataBind();

            string nickNo = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
            SiteTotalList = new SiteTotalService().GetNickOrderTotal(now.AddDays(-13), now, nickNo);
            Rpt_TotalList.DataSource = SiteTotalList;
            Rpt_TotalList.DataBind();

            //InsertName();
             
        }
    }

    protected IList<TopSiteTotalInfo> SiteTotalList
    {
        set { ViewState["SiteTotalList"] = value; }

        get
        {
            return (IList<TopSiteTotalInfo>)ViewState["SiteTotalList"];
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
}

public class TotalDateInfo
{
    public string TotalDate { set; get; }
}

public class TotalNameInfo
{
    public string TotalName { set; get; }
}
