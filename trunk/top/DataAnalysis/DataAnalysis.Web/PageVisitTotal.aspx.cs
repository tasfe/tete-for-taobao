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
using System.Data.SqlClient;

public partial class PageVisitTotal : System.Web.UI.Page
{

    readonly VisitService visitDal = new VisitService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.Page.IsPostBack)
        {
            DateTime now = DateTime.Now;
            DateTime end = now.AddDays(1);
            DateTime start = new DateTime(now.Year, now.Month, now.Day);
            DateTime endtime = new DateTime(end.Year, end.Month, end.Day);

            Bind(start, endtime);
        }
    }

    private void Bind(DateTime start,DateTime end)
    {
        int page = 1;
        try
        {
            page = int.Parse(Request.QueryString["page"]);
        }
        catch { }
        Rpt_PageVisit.DataSource = visitDal.GetAllVisitPageInfoList("246bcca56c050c665b67708d33127e46", start, end, page, 20);
        Rpt_PageVisit.DataBind();
        lbPage.Text = InitPageStr(10, "PageVisitTotal.aspx",page);
    }

    private string InitPageStr(int total, string url,int page)
    {
        //分页数据初始化
        string str = string.Empty;
        int pageCount = 20;
        int pageSize = 0;
        int pageNow = 1;
        pageNow = page;
        //取总分页数
        if (total % pageCount == 0)
        {
            pageSize = total / pageCount;
        }
        else
        {
            pageSize = total / pageCount + 1;
        }

        //如果总页面大于20，则最大页面差不超过20
        int start = 1;
        int end = 20;

        if (pageSize < end)
        {
            end = pageSize;
        }
        else
        {
            if (pageNow > 15)
            {
                start = pageNow - 10;

                if (pageNow < (total - 10))
                {
                    end = pageNow + 10;
                }
                else
                {
                    end = total;
                }
            }
        }

        for (int i = start; i <= end; i++)
        {
            if (i.ToString() == pageNow.ToString())
            {
                str += i.ToString() + " ";
            }
            else
            {
                str += "<a href='" + url + "?page=" + i.ToString() + "'>[" + i.ToString() + "]</a> ";
            }
        }

        return str;
    }

}
