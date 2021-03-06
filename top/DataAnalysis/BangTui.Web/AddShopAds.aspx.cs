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
            string nick="";
            if (Request.Cookies["nick"] != null)
                nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value); //"nick"; 
            else
                nick = Session["snick"].ToString();
            if (nick == "")
            {
                Response.Write("请重新登录");
                return;
            }

            TaoBaoShopInfo info = new ShopService().SelectShopByNick(nick);

            ViewState["shopcid"] = "50023878";

            if (info != null)
            {
                ViewState["shopcid"] = info.CateId;
                TB_ShppName.Text = info.Name;
                TB_Description.Text = info.Description;
                TB_AliWang.Text = nick;
                TB_ShowUrl.Text = "http://shop" + info.ShopId + ".taobao.com/";
                ViewState["logourl"] = " http://logo.taobao.com/shop-logo" + info.ShopLogo;
            }
        }
    }

    protected string ShopImg
    {
        get
        {
            return ViewState["logourl"] == null ? "" : ViewState["logourl"].ToString();
        }
    }

    protected void BTN_Tui_Click(object sender, EventArgs e)
    {
        string nick = "";
        if (Request.Cookies["nick"] != null)
            nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value); //"nick"; 
        else
            nick = Session["snick"].ToString();

        //购买类型
        IList<BuyInfo> buyList = CacheCollection.GetAllBuyInfo().Where(o => o.Nick == nick).ToList();

        //投放的广告
        IList<UserAdsInfo> useradsList = userAdsDal.SelectAllUserAds(nick);

        bool notou = true;

        int i = 0;

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
                    IList<UserAdsInfo> myUseradsList = useradsList.Where(o => o.FeeId == feeInfo.FeeId && o.UserAdsState == 1).ToList();

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
                    string cname = tcinfo == null ? "" : tcinfo.name;
                    info.SellCateName = GetTaoBaoCName(info.CateIds, cname);
                    info.AliWang = nick;
                    info.Nick = nick;
                    info.AdsPic = ShopImg;
                    //店铺图标
                    if (FUD_Img.HasFile && CheckImg())
                    {
                        Guid imgurl = Guid.NewGuid();
                        FUD_Img.SaveAs(Server.MapPath("~/adsimg") + "/" + imgurl + ".jpg");
                        info.AdsPic = "/adsimg/" + imgurl + ".jpg";
                    }

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
                            info.CateIds = GetTaoBaoCId(taoId, taoId);
                        }

                        notou = false;
                    }

                    userAdsDal.InsertUserAds(info);
                    break;
                }
            }
        }

        if (notou)
            Response.Redirect("UserAdsList.aspx");
        else
            Response.Redirect("UserAdsList.aspx?istou=1");
    }

    private bool CheckImg()
    {
        if (FUD_Img.PostedFile.ContentType.IndexOf("jpeg") != -1 || FUD_Img.PostedFile.ContentType.IndexOf("jpg") != -1 || FUD_Img.PostedFile.ContentType.IndexOf("png") != -1 || FUD_Img.PostedFile.ContentType.IndexOf("gif") != -1)
        {
            return true;
        }
        else
        {
            return false;
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
        Random rand = new Random();
        return list[rand.Next(list.Count)].AdsId;
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
}
