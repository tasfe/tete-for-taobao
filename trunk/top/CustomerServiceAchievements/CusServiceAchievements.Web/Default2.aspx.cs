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
using Model;
using System.Collections.Generic;
using CusServiceAchievements.DAL;
using DBHelp;

public partial class Default2 : System.Web.UI.Page
{

    TalkRecodService trDal = new TalkRecodService();
    PagedDataSource pds = new PagedDataSource();

    GoodsOrderService goDal = new GoodsOrderService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);

            DateTime[] dateArr = DataHelper.GetDateTime(DateTime.Now, 1);

            try
            {
                dateArr[0] = DateTime.Parse(Request.QueryString["start"]);
                dateArr[1] = dateArr[0].AddDays(1);
            }
            catch
            {
            }
            if (Request.QueryString["suc"] == "1")
                Bind(dateArr[0], dateArr[1], nick, new[] { 1 });
            else
                Bind(dateArr[0], dateArr[1], nick);
        }
    }

    private void Bind(DateTime start, DateTime end, string nick, params int[] tid)
    {
        int TotalCount = 0;//总记录数
        int TotalPage = 1; //总页数

        int page = 1;
        try
        {
            page = int.Parse(Request.QueryString["Page"]);
            if (ViewState["page"] != null)
            {
                page = int.Parse(ViewState["page"].ToString());
                ViewState["page"] = null;
            }
        }
        catch { }

        List<GoodsOrderInfo> goodsOrderList = goDal.GetCustomerList(nick, start, end);

        IList<CustomerInfo> list = trDal.GetCustomerList(start, end, nick);

        if (list.Count > 0)
        {
            for (int i = 0; i < list.Count; i++)
            {
                IList<GoodsOrderInfo> thislist = goodsOrderList.Where(o => o.buyer_nick == list[i].CustomerNick).ToList();
                if (thislist.Count > 0)
                {
                    list[i].tid = thislist[0].tid;
                }
            }
        }

        int suc = 0;
        if (tid != null && tid.Length > 0)
        {
            list = list.Where(o => !string.IsNullOrEmpty(o.tid)).ToList();
            suc = 1;
        }

        TotalCount = list.Count;
        pds.DataSource = list;
        pds.AllowPaging = true;
        pds.PageSize = 20;

        if (TotalCount == 0)
            TotalPage = 1;
        else
        {
            if (TotalCount % pds.PageSize == 0)
                TotalPage = TotalCount / pds.PageSize;
            else
                TotalPage = TotalCount / pds.PageSize + 1;
        }

        pds.CurrentPageIndex = page - 1;
        //lblCurrentPage.Text = "共" + TotalCount.ToString() + "条记录 当前页：" + page + "/" + TotalPage;

        //lnkFrist.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=1" + "&start=" + start.ToShortDateString() + "&suc=" + suc;
        //if (!pds.IsFirstPage)
        //    lnkPrev.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + (page - 1) + "&start=" + start.ToShortDateString() + "&suc=" + suc;

        //if (!pds.IsLastPage)
        //    lnkNext.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + (page + 1) + "&start=" + start.ToShortDateString() + "&suc=" + suc;
        //lnkEnd.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + TotalPage + "&start=" + start.ToShortDateString() + "&suc=" + suc;

        Rpt_Jie.DataSource = pds;
        Rpt_Jie.DataBind();
        //TB_Start.Text = start.ToString("yyyy-MM-dd");

    }

}
