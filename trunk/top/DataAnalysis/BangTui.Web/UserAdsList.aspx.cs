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

public partial class UserAdsList : System.Web.UI.Page
{
    UserAdsService uasDal = new UserAdsService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string nick = HttpUtility.UrlDecode(Request.Cookies["Nick"].Value); //"nick";
            IList<UserAdsInfo> list = uasDal.SelectAllUserAds(nick);

            if (Request.QueryString["istou"] == "1")
                list = list.Where(o => o.UserAdsState == 1).ToList();
            else
                list = list.Where(o => o.UserAdsState != 1).ToList();
            RPT_AdsList.DataSource = list;
            RPT_AdsList.DataBind();
        }
    }

    protected string GetSite(string adsId)
    {
        IList<AdsInfo> list = CacheCollection.GetAllAdsInfo().Where(o => o.AdsId.ToString() == adsId).ToList();
        if (list.Count == 0)
            return "";

        return CacheCollection.GetAllSiteList().Where(o => o.SiteId == list[0].SiteId).ToList()[0].SiteUrl;
    }

    protected string GetTitle(string adsId)
    {
        IList<AdsInfo> list = CacheCollection.GetAllAdsInfo().Where(o => o.AdsId.ToString() == adsId).ToList();
        if (list.Count == 0)
            return "";
        return list[0].AdsName;
    }

    protected string GetSize(string adsId)
    {
        IList<AdsInfo> list = CacheCollection.GetAllAdsInfo().Where(o => o.AdsId.ToString() == adsId).ToList();
        if (list.Count == 0)
            return "";
        return list[0].AdsSize;
    }

    protected string GetAdsType(string adsId)
    {
        IList<AdsInfo> list = CacheCollection.GetAllAdsInfo().Where(o => o.AdsId.ToString() == adsId).ToList();
        if (list.Count == 0)
            return "";
        if (list[0].AdsType == 1)
            return "列表-图文";
        return "单个投放";
    }

    protected string GetState(string adsState)
    {
        if (adsState == "0")
            return "未投放";
        if (adsState == "1")
            return "投放中";
        if (adsState == "2")
            return "暂停投放";
        return "";
    }

    protected void RPT_AdsList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (Request.QueryString["istou"] != "1")
        {
            e.Item.FindControl("Btn_Stop").Visible = false;
            e.Item.FindControl("Btn_Result").Visible = false;
            e.Item.FindControl("Btn_See").Visible = false;
            e.Item.FindControl("Btn_Add").Visible = false;
        }
        else
        {
            e.Item.FindControl("Btn_Insert").Visible = false;
        }
    }

    protected void RPT_AdsList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "De")
        {
            Guid id = new Guid(e.CommandArgument.ToString());
            uasDal.DelteUserAds(id);
            Response.Redirect("UserAdsList.aspx");
        }
        if (e.CommandName == "Insert")
        {
            Guid id = new Guid(e.CommandArgument.ToString());
            UserAdsInfo info = uasDal.SelectUserAdsById(id);
            string nick = HttpUtility.UrlDecode(Request.Cookies["Nick"].Value);
            IList<BuyInfo> buyList = CacheCollection.GetAllBuyInfo().Where(o => o.Nick == nick).ToList();

            //投放的广告
            IList<UserAdsInfo> useradsList = uasDal.SelectAllUserAds(nick);

            //购买类型
            foreach (BuyInfo binfo in buyList)
            {
                //未过期的
                if (!binfo.IsExpied)
                {
                    //收费类型
                    FeeInfo feeInfo = CacheCollection.GetAllFeeInfo().Where(o => o.FeeId == binfo.FeeId).ToList()[0];

                    if (feeInfo.AdsCount > 0)
                    {

                        //已经投放了的该收费类型的广告集合
                        IList<UserAdsInfo> myUseradsList = useradsList.Where(o => o.FeeId == feeInfo.FeeId).ToList();

                        //真正可以添加的广告数量
                        int realcount = feeInfo.AdsCount - myUseradsList.Count;

                        if (realcount > 0)
                        {
                            if (feeInfo.AdsType == 5)
                            {
                                //这里需要查询空闲的广告（或者说是可以用的广告位）
                                IList<UserAdsInfo> usedadsList = uasDal.SelectAllUsedAds();
                                IList<AdsInfo> allads = CacheCollection.GetAllAdsInfo().Where(o => o.AdsType == 5).ToList();
                                if (allads.Count > usedadsList.Count)
                                {
                                    //得到没有使用的广告位
                                    IList<AdsInfo> hasads = new List<AdsInfo>();
                                    foreach (AdsInfo ainfo in allads)
                                    {
                                        if (usedadsList.Where(o => o.AdsId == ainfo.AdsId).ToList().Count == 0)
                                            hasads.Add(ainfo);
                                    }


                                    for (int i = 0; i < realcount; i++)
                                    {
                                        info.AddTime = DateTime.Now;
                                        info.AdsId = GetRand(hasads);
                                        //不需要旺旺
                                        info.AliWang = ""; //nick;
                                        info.FeeId = binfo.FeeId;
                                        info.AdsShowStartTime = DateTime.Now;
                                        info.AdsShowFinishTime = DateTime.Now.AddDays(feeInfo.ShowDays);
                                        info.Nick = nick;
                                        info.UserAdsState = 1;
                                        //不需要分类
                                        //string taoId = info.CateIds;
                                        info.CateIds = "";  // GetTaoBaoCId(taoId, ref taoId);
                                    }
                                }
                                else
                                {
                                    Page.RegisterStartupScript("通知", "<script>alert('请联系我们的客服人员为您添加广告');</script>");
                                }
                                continue;
                            }

                            //可以放多少个
                            for (int i = 0; i < realcount; i++)
                            {

                                info.AddTime = DateTime.Now;
                                info.AdsId = GetRand(CacheCollection.GetAllAdsInfo().Where(o => o.AdsType == 1).ToList());
                                info.AliWang = nick;
                                info.FeeId = binfo.FeeId;
                                info.AdsShowStartTime = DateTime.Now;
                                info.AdsShowFinishTime = DateTime.Now.AddDays(feeInfo.ShowDays);
                                info.Nick = nick;
                                info.UserAdsState = 1;
                                string taoId = info.CateIds;
                                info.CateIds = GetTaoBaoCId(taoId, ref taoId);
                            }

                            uasDal.UpdateUserAdsState(1, id);
                            break;
                        }
                    }
                }
            }
        }
        if (e.CommandName == "Result")
        {
            Guid id = new Guid(e.CommandArgument.ToString());
        }
        if (e.CommandName == "See")
        {
            Guid id = new Guid(e.CommandArgument.ToString());
        }
        if (e.CommandName == "Stop")
        {
            Guid id = new Guid(e.CommandArgument.ToString());
            uasDal.UpdateUserAdsState(2, id);
            Response.Redirect("UserAdsList.aspx");
        }
        if (e.CommandName == "Add")
        {

        }
    }

    /// <summary>
    /// 获取随机数
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    private Guid GetRand(IList<AdsInfo> list)
    {
        if (list.Count == 1)
            return list[0].AdsId;
        Random rand = new Random(list.Count - 1);
        return list[rand.Next()].AdsId;
    }

    private string GetTaoBaoCId(string taoId, ref string cids)
    {
        TaoBaoGoodsClassInfo info = new TaoBaoGoodsClassService().SelectGoodsClass(taoId);
        if (info != null)
        {
            if (info.parent_cid != "0")
            {
                cids += "," + info.parent_cid;
                GetTaoBaoCId(info.parent_cid, ref cids);
            }
            return cids;

        }
        return cids;
    }
}
