using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using TaoBaoAPIHelper;
using Model;
using System.Collections.Generic;
using CusServiceAchievements.DAL;
using DBHelp;

public partial class Default2 : BasePage
{

    TalkRecodService trDal = new TalkRecodService();
    PagedDataSource pds = new PagedDataSource();

    GoodsOrderService goDal = new GoodsOrderService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            #region 

            string s = Request.QueryString["data"].Replace("[jia]", "+");

            Qijia.PCI.MethodPCI pci = new Qijia.PCI.MethodPCI();

            string obj = pci.GetYouWant(s).ToString();

            #endregion

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

        List<GoodsOrderInfo> goodsOrderList =  goDal.GetCustomerList(nick, start, end);

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

        if (tid != null && tid.Length > 0)
        {
            list = list.Where(o => !string.IsNullOrEmpty(o.tid)).ToList();
        }

        TotalCount = list.Count;
        pds.DataSource = list;
        pds.AllowPaging = true;
        pds.PageSize = 10;

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

        Rpt_Jie.DataSource = pds;
        Rpt_Jie.DataBind();
    }
}
