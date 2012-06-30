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
            string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);

            IList<CateInfo> cateList = new CateService().SelectAllCateByNick(nick);
            DDL_GoodsClass.DataSource = cateList;
            DDL_GoodsClass.DataTextField = "CateName";
            DDL_GoodsClass.DataValueField = "CateId";
            DDL_GoodsClass.DataBind();

            DDL_GoodsClass.Items.Insert(0, new ListItem("全部", "0"));

            IList<GoodsInfo> list = goodsDal.SelectAllGoodsByNick(nick);
            Rpt_GoodsList.DataSource = list;
            Rpt_GoodsList.DataBind();
        }
    }

    protected void BTN_ShowGoods_Click(object sender, EventArgs e)
    {
        IList<UserAdsInfo> list = new List<UserAdsInfo>();

        string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);

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


        IList<BuyInfo> buyList = CacheCollection.GetAllBuyInfo().Where(o => o.Nick == nick).ToList();

        //投放的广告
        IList<UserAdsInfo> useradsList = userAdsDal.SelectAllUserAds(nick);

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
                    int count = feeInfo.AdsCount > list.Count ? list.Count : feeInfo.AdsCount;

                    //已经投放了的该收费类型的广告集合
                    IList<UserAdsInfo> myUseradsList = useradsList.Where(o => o.FeeId == feeInfo.FeeId && o.UserAdsState == 1).ToList();

                    //真正可以添加的广告数量
                    int realcount = 0;
                    if (feeInfo.AdsCount - myUseradsList.Count <= 0)
                        realcount = 0;
                    else
                        realcount = (feeInfo.AdsCount - myUseradsList.Count) >= count ? count : feeInfo.AdsCount - myUseradsList.Count;

                    if (realcount > 0)
                    {
                        if (feeInfo.AdsType == 5)
                        {
                            //这里需要查询空闲的广告（或者说是可以用的广告位）
                            IList<UserAdsInfo> usedadsList = userAdsDal.SelectAllUsedAds();
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
                                    list[i].AddTime = DateTime.Now;
                                    list[i].AdsId = GetRand(hasads);
                                    //不需要旺旺
                                    list[i].AliWang = ""; //nick;
                                    list[i].FeeId = binfo.FeeId;
                                    list[i].AdsShowStartTime = DateTime.Now;
                                    list[i].AdsShowFinishTime = DateTime.Now.AddDays(feeInfo.ShowDays);
                                    list[i].Nick = nick;
                                    list[i].UserAdsState = 1;
                                    //不需要分类
                                    //string taoId = list[i].CateIds;
                                    list[i].CateIds = "";  // GetTaoBaoCId(taoId, ref taoId);
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

                            list[i].AddTime = DateTime.Now;
                            list[i].AdsId = GetRand(CacheCollection.GetAllAdsInfo().Where(o => o.AdsType == 1).ToList());
                            list[i].AliWang = nick;
                            list[i].FeeId = binfo.FeeId;
                            list[i].AdsShowStartTime = DateTime.Now;
                            list[i].AdsShowFinishTime = DateTime.Now.AddDays(feeInfo.ShowDays);
                            list[i].Nick = nick;
                            list[i].UserAdsState = 1;
                            string taoId = list[i].CateIds;
                            list[i].CateIds = GetTaoBaoCId(taoId, taoId);

                        }
                    }
                }
            }
        }

        foreach (UserAdsInfo info in list)
        {
            userAdsDal.InsertUserAds(info);
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

    /// <summary>
    /// 获取随机数
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    private Guid GetRand(IList<AdsInfo> list)
    {
        if (list.Count == 1)
            return list[0].AdsId;
        Random rand = new Random();
        return list[rand.Next(list.Count - 1)].AdsId;
    }

    protected void BTN_SELECT_Click(object sender, EventArgs e)
    {
        string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
        IList<GoodsInfo> list = new List<GoodsInfo>();
        if (string.IsNullOrEmpty(TB_StartTime.Text) || string.IsNullOrEmpty(TB_EndTime.Text))
        {
            list = goodsDal.SearchGoods(nick, TB_GoodsName.Text.Trim(), DDL_GoodsClass.SelectedValue == "0" ? "" : DDL_GoodsClass.SelectedValue);
        }

        if (!string.IsNullOrEmpty(TB_StartTime.Text) && !string.IsNullOrEmpty(TB_EndTime.Text))
        {
            list = goodsDal.SearchGoods(nick, TB_GoodsName.Text.Trim(), DDL_GoodsClass.SelectedValue == "0" ? "" : DDL_GoodsClass.SelectedValue, new DateTime[] { DateTime.Parse(TB_StartTime.Text), DateTime.Parse(TB_EndTime.Text) });
        }
        Rpt_GoodsList.DataSource = list;
        Rpt_GoodsList.DataBind();
    }
}
