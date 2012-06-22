﻿using System;
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

public partial class AddShopAds : System.Web.UI.Page
{
    UserAdsService userAdsDal = new UserAdsService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string nick = "nick"; //HttpUtility.UrlDecode(Request.Cookies["Nick"].Value);

            ViewState["shopcid"] = "50023878";

        }
    }
    protected void BTN_Tui_Click(object sender, EventArgs e)
    {
        string nick = "nick"; //HttpUtility.UrlDecode(Request.Cookies["Nick"].Value);

        //购买类型
        IList<BuyInfo> buyList = CacheCollection.GetAllBuyInfo().Where(o => o.Nick == nick).ToList();

        //提交的广告
        IList<UserAdsInfo> useradsList = userAdsDal.SelectAllUserAds(nick);

        foreach (BuyInfo binfo in buyList)
        {
            //未过期的
            if (!binfo.IsExpied)
            {
                //收费类型
                FeeInfo feeInfo = CacheCollection.GetAllFeeInfo().Where(o => o.FeeId == binfo.FeeId).ToList()[0];

                if (feeInfo.AdsCount > 0)
                {

                    //已经投放了的广告集合
                    IList<UserAdsInfo> myUseradsList = useradsList.Where(o => o.FeeId == feeInfo.FeeId).ToList();

                    //真正可以添加的广告数量
                    int realcount = feeInfo.AdsCount - myUseradsList.Count;
                    UserAdsInfo info = new UserAdsInfo();
                    info.AdsTitle = TB_ShppName.Text.Trim(); ;
                    info.Id = Guid.NewGuid();
                    info.UserAdsState = 0;
                    info.AdsUrl = TB_ShowUrl.Text.Trim();
                    string cateId = ViewState["shopcid"].ToString();

                    TaoBaoGoodsClassInfo tcinfo = new TaoBaoGoodsClassService().SelectGoodsClass(cateId);
                    info.CateIds = ViewState["shopcid"].ToString();
                    string cname = tcinfo.name;
                    info.SellCateName = GetTaoBaoCName(info.CateIds, ref cname);
                    info.AliWang = nick;
                    info.Nick = nick;
                    info.AdsPic = TB_ShowUrl.Text.Trim();
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
                                    //string taoId = list[i].CateIds;
                                    info.CateIds = "";  // GetTaoBaoCId(taoId, ref taoId);
                                }
                            }
                            else
                            {
                                Page.RegisterStartupScript("通知", "<script>alert('请联系我们的客服人员为您添加广告');</script>");
                            }
                        }
                        if (feeInfo.AdsType == 1)
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
                    }

                    userAdsDal.InsertUserAds(info);
                }
            }
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

    private string GetTaoBaoCName(string taoId, ref string cname)
    {
        TaoBaoGoodsClassInfo info = new TaoBaoGoodsClassService().SelectGoodsClass(taoId);
        if (info != null)
        {
            if (info.parent_cid != "0")
            {
                cname = info.name + "->" + cname;
                GetTaoBaoCName(info.parent_cid, ref cname);
            }
            return cname;

        }
        return cname;
    }
}
