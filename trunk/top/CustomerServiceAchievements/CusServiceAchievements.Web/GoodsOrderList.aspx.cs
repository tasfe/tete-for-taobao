using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using TaoBaoAPIHelper;
using System.Collections.Generic;
using CusServiceAchievements.DAL;
using Model;
using System.Linq;

public partial class GoodsOrderList : BasePage
{
    TaoBaoGoodsOrderService taoGoodsOrderService = new TaoBaoGoodsOrderService();
    TalkRecodService trDal = new TalkRecodService();

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            DateTime[] darray = DataHelper.GetDateTime(DateTime.Now, 1);
            try
            {
                darray[0] = DateTime.Parse(Request.QueryString["start"]);
                darray[1] = DateTime.Parse(Request.QueryString["end"]);
            }
            catch
            {
            }
            if (!string.IsNullOrEmpty(Request.QueryString["day"]))
            {
                string date = Request.QueryString["day"];
                date = date.Substring(0, 4) + "-" + date.Substring(4, 2) + "-" + date.Substring(6);

                darray[0] = DateTime.Parse(date);
                darray[1] = DateTime.Parse(date).AddDays(1);
            }

            string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
            string session = Request.Cookies["nicksession"].Value;
            ViewState["count"] = taoGoodsOrderService.GetOrderListCount(nick, darray[0], darray[1]);
            Bind(nick, session, darray[0], darray[1], int.Parse(ViewState["count"].ToString()), 20);
        }
    }

    private void Bind(string nick, string session, DateTime start, DateTime end, int totalCount, int recordCount)
    {
        int TotalPage = totalCount % recordCount != 0 ? (totalCount / recordCount) + 1 : totalCount / recordCount; //总页数

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

        IList<GoodsOrderInfo> goodsOrderList = taoGoodsOrderService.GetOrderList(HttpUtility.UrlDecode(Request.Cookies["nick"].Value), start, end, page, recordCount);


        IList<CustomerInfo> list = trDal.GetCustomerList(start, end, nick);

        if (list.Count > 0)
        {
            for (int i = 0; i < goodsOrderList.Count; i++)
            {
                IList<CustomerInfo> thislist = list.Where(o => o.CustomerNick == goodsOrderList[i].buyer_nick).ToList();
                if (thislist.Count > 0)
                {
                    if (thislist[0].FromNick.IndexOf(":") > 0)
                    {
                        goodsOrderList[i].seller_nick = thislist[0].FromNick.Substring(thislist[0].FromNick.IndexOf(":") + 1);
                    }
                    else
                        goodsOrderList[i].seller_nick = thislist[0].FromNick;
                }
                else
                {

                }
            }
        }

        lblCurrentPage.Text = "共" + totalCount.ToString() + "条记录 当前页：" + page + "/" + (TotalPage == 0 ? 1 : TotalPage);

        lnkFrist.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=1&" + "start=" + start.ToShortDateString() + "&end=" + end.ToShortDateString();
        if (page > 1)
            lnkPrev.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + Convert.ToString(page - 1) + "&" + "start=" + start.ToShortDateString() + "&end=" + end.ToShortDateString();

        if (page != TotalPage && TotalPage != 0)
            lnkNext.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + Convert.ToString(page + 1) + "&" + "start=" + start.ToShortDateString() + "&end=" + end.ToShortDateString();
        lnkEnd.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + (TotalPage == 0 ? 1 : TotalPage) + "&" + "start=" + start.ToShortDateString() + "&end=" + end.ToShortDateString();

        Rpt_PageVisit.DataSource = goodsOrderList;
        Rpt_PageVisit.DataBind();
        TB_Start.Text = start.ToString("yyyy-MM-dd");
        TB_End.Text = end.ToString("yyyy-MM-dd");
    }

    protected void Btn_Select_Click(object sender, EventArgs e)
    {
        DateTime now = DateTime.Now;
        DateTime end = now.AddDays(1);
        DateTime start = new DateTime(now.Year, now.Month, now.Day);
        DateTime endtime = new DateTime(end.Year, end.Month, end.Day);
        try
        {
            start = DateTime.Parse(TB_Start.Text);
            endtime = DateTime.Parse(TB_End.Text);
            ViewState["start"] = start;
            ViewState["end"] = end;
        }
        catch
        {
            TB_Start.Text = start.ToString("yyyy-MM-dd");
            TB_End.Text = endtime.ToString("yyyy-MM-dd");
        }

        string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
        string session = Request.Cookies["nicksession"].Value;
        ViewState["count"] = taoGoodsOrderService.GetOrderListCount(nick, start, endtime);
        ViewState["page"] = "1";
        Bind(nick, session, start, endtime, int.Parse(ViewState["count"].ToString()), 20);
    }
}
