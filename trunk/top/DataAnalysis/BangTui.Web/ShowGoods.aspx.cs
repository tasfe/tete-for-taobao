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

public partial class ShowGoods : System.Web.UI.Page
{
    TaoBaoGoodsClassService tbgcDal = new TaoBaoGoodsClassService();
    UserAdsService uadsDal = new UserAdsService();
    PagedDataSource pds = new PagedDataSource();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string cid = Request.QueryString["cid"];
            string adsId = Request.QueryString["adsid"];

            //IList<TaoBaoGoodsClassInfo> list = tbgcDal.SelectAllGoodsClass("0");

            RPT_GOODSCLASS.DataSource = new List<TaoBaoGoodsClassInfo>();
            RPT_GOODSCLASS.DataBind();


            //获取分类下所有用户投放的广告
            IList<UserAdsInfo> adsList = uadsDal.SelectAllUserAdsByAdsId(new Guid(adsId), 1);
            //按时间倒序排列
            adsList = adsList.OrderByDescending(o => o.AddTime).ToList();

            if (Request.Cookies["nick"] != null)
            {
                ShowLoginAds(adsList);
            }
            else
            {
                if (Session["snick"] != null)
                {
                    ShowLoginAds(adsList);
                }
            }

            int TotalCount = adsList.Count;
            int TotalPage = 1; //总页数

            int page = 1;
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["Page"]))
                    page = int.Parse(Request.QueryString["Page"]);
            }
            catch { }

            pds.DataSource = adsList;
            pds.AllowPaging = true;
            pds.PageSize = 80;

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
            lblCurrentPage.Text = "共" + TotalCount.ToString() + "条记录 当前页：" + page + "/" + TotalPage;

            string paramArray = "&adsId=" + adsId;

            lnkFrist.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=1" + paramArray;
            if (!pds.IsFirstPage)
                lnkPrev.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + (page - 1) + paramArray;

            if (!pds.IsLastPage)
                lnkNext.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + (page + 1) + paramArray;
            lnkEnd.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + TotalPage + paramArray;


            RPT_AdsList.DataSource = pds;
            RPT_AdsList.DataBind();

        }
    }

    private void ShowLoginAds(IList<UserAdsInfo> adsList)
    {
        IList<UserAdsInfo> list = adsList.Where(o => o.Nick == HttpUtility.UrlDecode(Request.Cookies["nick"].Value)).ToList();
        foreach (UserAdsInfo info in list)
        {
            adsList.Remove(info);
        }

        foreach (UserAdsInfo info in list)
        {
            adsList.Insert(0, info);
        }
    }

    protected string GetParam(string id, string url)
    {
        return HttpUtility.UrlEncode(("id=" + id + "&url=" + url));
    }

    protected string GetPic(string pic)
    {
        if (pic.Contains("http://"))
            return pic+"_210x210.jpg";
        return pic;
    }
}
