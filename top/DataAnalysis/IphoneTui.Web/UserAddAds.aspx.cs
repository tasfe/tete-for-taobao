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

public partial class UserAddAds : System.Web.UI.Page
{
    GoodsService goodsDal = new GoodsService();

    PagedDataSource pds = new PagedDataSource();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string nick = "";
            if (Request.Cookies["nick"] != null)
                nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value); //"nick"; 
            else
                nick = Session["snick"].ToString();
            if (nick == "")
            {
                Response.Write("请重新登录");
                return;
            }

            DDL_App.DataSource = CacheCollection.GetAllSiteList();
            DDL_App.DataTextField = "SiteName";
            DDL_App.DataValueField = "SiteId";

            DDL_AdsType.DataSource = CacheCollection.GetAllAdsInfo();
            DDL_AdsType.DataTextField = "AdsName";
            DDL_AdsType.DataValueField = "AdsId";

            DDL_App.DataBind();
            DDL_App.Items.Insert(0, new ListItem("请选择", Guid.Empty.ToString()));
            DDL_AdsType.DataBind();

            IList<CateInfo> cateList = new CateService().SelectAllCateByNick(nick);
            DDL_GoodsClass.DataSource = cateList;
            DDL_GoodsClass.DataTextField = "CateName";
            DDL_GoodsClass.DataValueField = "CateId";
            DDL_GoodsClass.DataBind();

            DDL_GoodsClass.Items.Insert(0, new ListItem("全部", "0"));

            //IList<GoodsInfo> list = goodsDal.SelectAllGoodsByNick(nick);
            //Rpt_GoodsList.DataSource = list;
            //Rpt_GoodsList.DataBind();

            if (string.IsNullOrEmpty(Request.QueryString["start"]))
                Bind(Request.QueryString["goods"], Request.QueryString["gclass"], DateTime.MinValue, DateTime.MinValue);
            else
                Bind(Request.QueryString["goods"], Request.QueryString["gclass"], DateTime.Parse(Request.QueryString["start"]), DateTime.Parse(Request.QueryString["end"]));
        }
    }

    private void Bind(string goodsName,string className,DateTime start,DateTime end)
    {
        int TotalCount = 0;//总记录数
        int TotalPage = 1; //总页数
        string nick = "";
        if (Request.Cookies["nick"] != null)
            nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value); //"nick"; 
        else
            nick = Session["snick"].ToString();
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

        IList<GoodsInfo> list = new List<GoodsInfo>();

        if (start==DateTime.MinValue)
        {
            list = goodsDal.SearchGoods(nick, goodsName, className);
        }

        if (!string.IsNullOrEmpty(TB_StartTime.Text) && !string.IsNullOrEmpty(TB_EndTime.Text))
        {
            list = goodsDal.SearchGoods(nick, goodsName, className, new DateTime[] { start, end });
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
        lblCurrentPage.Text = "共" + TotalCount.ToString() + "条记录 当前页：" + page + "/" + TotalPage;

        string paramArray = string.Empty;
        if (!string.IsNullOrEmpty(goodsName))
            paramArray += "&goods=" + goodsName;
        if (!string.IsNullOrEmpty(className))
            paramArray += "&gclass=" + className;
        if (start != DateTime.MinValue)
            paramArray += "&start=" + start.ToShortDateString();
        if (end != DateTime.MinValue)
            paramArray += "&start=" + end.ToShortDateString();

        lnkFrist.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=1" + paramArray;
        if (!pds.IsFirstPage)
            lnkPrev.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + (page - 1) + paramArray;

        if (!pds.IsLastPage)
            lnkNext.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + (page + 1) + paramArray;
        lnkEnd.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + TotalPage + paramArray;

        Rpt_GoodsList.DataSource = pds;
        Rpt_GoodsList.DataBind();
    }

    protected void BTN_ShowGoods_Click(object sender, EventArgs e)
    {
        IList<UserAdsInfo> list = new List<UserAdsInfo>();

        string nick = "";
        if (Request.Cookies["nick"] != null)
            nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value); //"nick"; 
        else
            nick = Session["snick"].ToString();

        CateService cateDal = new CateService();
        IList<CateInfo> cateList = cateDal.SelectAllCateByNick(nick).ToList();

        UserAdsService userAdsDal = new UserAdsService();

        foreach (RepeaterItem item in Rpt_GoodsList.Items)
        {
            CheckBox cb = (CheckBox)item.FindControl("CBOX_Goods");
            if (cb.Checked)
            {
                UserAdsInfo info = new UserAdsInfo();
                info.AdsTitle = ((Label)item.FindControl("LB_GoodsName")).Text;
                info.Id = Guid.NewGuid();
                info.UserAdsState = 0;
                info.AdsUrl = "http://item.taobao.com/item.htm?id=" + ((Label)item.FindControl("LB_GoodsId")).Text;
                string cateId = ((Label)item.FindControl("LB_CateId")).Text;

                IList<CateInfo> thiscList = cateList.Where(o => o.CateId == cateId).ToList();

                info.CateIds = ((Label)item.FindControl("LB_TaoBaoCId")).Text;
                string cname = thiscList.Count == 0 ? "" : thiscList[0].CateName;
                info.SellCateName = GetTaoBaoCName(info.CateIds, cname);
                info.AliWang = nick;
                info.Nick = nick;
                info.AdsPic = ((Label)item.FindControl("LB_Img")).Text;
                info.Price = decimal.Parse(((Label)item.FindControl("LB_Price")).Text);
                list.Add(info);
            }
        }
        if (list.Count == 0)
            return;

        SiteAdsInfo sadsinfo = CacheCollection.GetAllSiteAdsInfo().Where(o => o.Id.ToString() == DDL_Position.SelectedValue).ToList()[0];
        int needScore = sadsinfo.Score * list.Count;

        //购买类型
        IList<BuyInfo> buyList = CacheCollection.GetAllBuyInfo().Where(o => o.Nick == nick).ToList();

        //投放的广告
        IList<UserAdsInfo> useradsList = userAdsDal.SelectAllUserAds(nick);

        //新的添加广告方法
        if (buyList.Count > 0)
        {
            if (!buyList[0].IsExpied)
            {

                FeeInfo feeInfo = CacheCollection.GetAllFeeInfo().Where(o => o.FeeId == buyList[0].FeeId).ToList()[0];
                //已经投放了的该收费类型的广告集合
                IList<UserAdsInfo> myUseradsList = useradsList.Where(o => o.FeeId == feeInfo.FeeId && o.UserAdsState == 1).ToList();

                int calScore = 0;
                calScore = feeInfo.Score;
                for (int i = 0; i < myUseradsList.Count; i++)
                {
                    SiteAdsInfo sainfo = CacheCollection.GetAllSiteAdsInfo().Where(o => o.Id == myUseradsList[i].AdsId).ToList()[0];
                    calScore -= sainfo.Score;
                }

                int canTou = calScore / needScore; //可以投放的个数

                //不能继续投放
                if (canTou == 0)
                {
                    Page.RegisterStartupScript("通知", "<script>alert('您不能投放该类型广告');</script>");
                }
                //可以投放
                else
                {
                    if (sadsinfo.AdsCount != -1)
                    {
                        //这里需要查询空闲的广告（或者说是可以用的广告位）
                        IList<UserAdsInfo> usedadsList = userAdsDal.SelectAllUsedAds();
                        IList<SiteAdsInfo> allads = CacheCollection.GetAllSiteAdsInfo().Where(o => o.Id.ToString() == DDL_Position.SelectedValue).ToList();
                        if (allads.Count > 0 && allads[0].AdsCalCount > 0)
                        {
                            //可以放多少个
                            for (int i = 0; i < canTou; i++)
                            {

                                list[i].AddTime = DateTime.Now;
                                list[i].AliWang = nick;
                                list[i].FeeId = feeInfo.FeeId;
                                list[i].AdsShowStartTime = DateTime.Now;
                                list[i].AdsShowFinishTime = DateTime.Now.AddDays(feeInfo.ShowDays);
                                list[i].Nick = nick;
                                list[i].UserAdsState = 1;
                                string taoId = list[i].CateIds;
                                list[i].CateIds = GetTaoBaoCId(taoId, taoId);
                                list[i].AdsId = new Guid(DDL_Position.SelectedValue);
                            }
                        }
                        else
                        {
                            Page.RegisterStartupScript("通知", "<script>alert('请联系我们的客服人员为您添加广告');</script>");
                        }
                    }
                    if (feeInfo.AdsType == 1)
                    {
                        //可以放多少个
                        for (int i = 0; i < canTou; i++)
                        {

                            list[i].AddTime = DateTime.Now;
                            list[i].AliWang = nick;
                            list[i].FeeId = feeInfo.FeeId;
                            list[i].AdsShowStartTime = DateTime.Now;
                            list[i].AdsShowFinishTime = DateTime.Now.AddDays(feeInfo.ShowDays);
                            list[i].Nick = nick;
                            list[i].UserAdsState = 1;
                            string taoId = list[i].CateIds;
                            list[i].CateIds = GetTaoBaoCId(taoId, taoId);
                            list[i].AdsId = new Guid(DDL_Position.SelectedValue);
                        }
                    }
                }
            }
        }
        string uid = CacheCollection.GetAllSiteList().Where(o=>o.SiteId.ToString()== DDL_App.SelectedValue).ToList()[0].SiteUrl;
        int area = CacheCollection.GetAllSiteAdsInfo().Where(o=>o.Id.ToString()==DDL_Position.SelectedValue).ToList()[0].PositionCode;
        int typ=0;
        if (DDL_AdsType.SelectedValue.ToLower() == "1A4AB7A8-49A1-41FC-A5A6-788FD582DB82".ToLower() || DDL_AdsType.SelectedValue.ToLower() == "7CE41706-582D-4270-82C4-81420F923D6A".ToLower())
           typ=0;
        if (DDL_AdsType.SelectedValue.ToLower() == "68A5E23C-6CFD-427D-BD0E-1E2B1B160875".ToLower() || DDL_AdsType.SelectedValue.ToLower() == "C1F928EF-CA82-4FED-B960-CA33FCE417E9".ToLower())
           typ=1;
        if (DDL_AdsType.SelectedValue.ToLower() == "813F256D-D83A-43CE-8CC4-273C965DBF84".ToLower())
           typ=2;

        foreach (UserAdsInfo info in list)
        {
            Guid ggid = info.Id;
           
            if (info.AdsId != Guid.Empty)
            {
                ggid = PostIphone.InsertAds(uid, area, typ, info.AdsPic, info.AdsUrl, info.Price, info.AdsTitle);
                if (ggid == Guid.Empty)
                {
                    Page.RegisterStartupScript("通知", "<script>alert('发送添加广告错误');</script>");
                    continue;
                }
                userAdsDal.InsertUserAds(info, ggid);
            }
        }

        Response.Redirect("UserAdsList.aspx?istou=1");

    }

    private string GetTaoBaoCId(string taoId, string cids)
    {
        string returncids = cids;
        TaoBaoGoodsClassInfo info = new TaoBaoGoodsClassService().SelectGoodsClass(taoId);
        if (info != null)
        {
            if (info.parent_cid != "0")
            {
                returncids += "," + info.parent_cid;
                GetTaoBaoCId(info.parent_cid, returncids);
            }
            return returncids;

        }
        return returncids;
    }

    private string GetTaoBaoCName(string taoId, string cname)
    {
        string returnName = cname;
        TaoBaoGoodsClassInfo info = new TaoBaoGoodsClassService().SelectGoodsClass(taoId);
        if (info != null)
        {
            if (info.parent_cid != "0")
            {
                returnName = info.name + (cname == "" ? "" : ("->" + cname));
                GetTaoBaoCName(info.parent_cid, returnName);
            }
            return returnName;

        }
        return returnName;
    }

    protected void BTN_SELECT_Click(object sender, EventArgs e)
    {
        IList<GoodsInfo> list = new List<GoodsInfo>();
        if (string.IsNullOrEmpty(TB_StartTime.Text) || string.IsNullOrEmpty(TB_EndTime.Text))
        {
            ViewState["page"] = 1;
            Bind(TB_GoodsName.Text.Trim(), DDL_GoodsClass.SelectedValue == "0" ? "" : DDL_GoodsClass.SelectedValue, DateTime.MinValue, DateTime.MinValue);
        }

        if (!string.IsNullOrEmpty(TB_StartTime.Text) && !string.IsNullOrEmpty(TB_EndTime.Text))
        {
            ViewState["page"] = 1;
            Bind(TB_GoodsName.Text.Trim(), DDL_GoodsClass.SelectedValue == "0" ? "" : DDL_GoodsClass.SelectedValue, DateTime.Parse(TB_StartTime.Text), DateTime.Parse(TB_EndTime.Text));
        }
    }

    protected void Btn_ShowAddGoods_Click(object sender, EventArgs e)
    {
        Response.Redirect("AddGoods.aspx");
    }

    protected void DDL_App_TextChanged(object sender, EventArgs e)
    {
        if (DDL_App.SelectedValue == Guid.Empty.ToString())
            return;
        DDL_Position.DataSource = CacheCollection.GetAllSiteAdsInfo().Where(o => o.SiteId.ToString() == DDL_App.SelectedValue).ToList();
        DDL_Position.DataTextField = "AdsPosition";
        DDL_Position.DataValueField = "Id";

        DDL_Position.DataBind();
    }
}
