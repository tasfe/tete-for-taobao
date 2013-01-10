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

            DDL_App.DataSource = CacheCollection.GetAllSiteList();
            DDL_App.DataTextField = "SiteName";
            DDL_App.DataValueField = "SiteId";

            DDL_AdsType.DataSource = CacheCollection.GetAllAdsInfo();
            DDL_AdsType.DataTextField = "AdsName";
            DDL_AdsType.DataValueField = "AdsId";

            DDL_App.DataBind();
            DDL_App.Items.Insert(0, new ListItem("请选择", Guid.Empty.ToString()));
            DDL_AdsType.DataBind();

            TaoBaoShopInfo info = new ShopService().SelectShopByNick(nick);

            ViewState["shopcid"] = "50023878";

            if (info != null)
            {
                ViewState["shopcid"] = info.CateId;
                TB_ShppName.Text = info.Name;
                TB_Description.Text = info.Description;
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

        //新的添加广告方法
        if (buyList.Count > 0)
        {
            if (!buyList[0].IsExpied)
            {
                SiteAdsInfo sadsinfo = CacheCollection.GetAllSiteAdsInfo().Where(o => o.Id.ToString() == DDL_Position.SelectedValue).ToList()[0];

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

                UserAdsInfo info = new UserAdsInfo();
                info.AdsTitle = TB_ShppName.Text.Trim();
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
                //不能继续投放
                if (calScore < sadsinfo.Score)
                {

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

                            info.AddTime = DateTime.Now;
                            info.AdsId = new Guid(DDL_Position.SelectedValue);
                            //不需要旺旺
                            info.AliWang = ""; //nick;
                            info.FeeId = buyList[0].FeeId;
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
                    else
                    {
                        info.AddTime = DateTime.Now;
                        info.AdsId = new Guid(DDL_Position.SelectedValue);
                        info.AliWang = nick;
                        info.FeeId = buyList[0].FeeId;
                        info.AdsShowStartTime = DateTime.Now;
                        info.AdsShowFinishTime = DateTime.Now.AddDays(feeInfo.ShowDays);
                        info.Nick = nick;
                        info.UserAdsState = 1;
                        string taoId = info.CateIds;
                        info.CateIds = GetTaoBaoCId(taoId, taoId);
                    }
                    notou = false;
                }

                string uid = CacheCollection.GetAllSiteList().Where(o => o.SiteId.ToString() == DDL_App.SelectedValue).ToList()[0].SiteUrl;
                int area = CacheCollection.GetAllSiteAdsInfo().Where(o => o.Id.ToString() == DDL_Position.SelectedValue).ToList()[0].PositionCode;
                int typ = 0;
                if (DDL_AdsType.SelectedValue == "1A4AB7A8-49A1-41FC-A5A6-788FD582DB82" || DDL_AdsType.SelectedValue == "7CE41706-582D-4270-82C4-81420F923D6A")
                    typ = 0;
                if (DDL_AdsType.SelectedValue == "68A5E23C-6CFD-427D-BD0E-1E2B1B160875" || DDL_AdsType.SelectedValue == "C1F928EF-CA82-4FED-B960-CA33FCE417E9")
                    typ = 1;
                if (DDL_AdsType.SelectedValue == "813F256D-D83A-43CE-8CC4-273C965DBF84")
                    typ = 2;
                Guid ggid = Guid.Empty;

                if (info.AdsId != Guid.Empty)
                {
                    ggid = PostIphone.InsertAds(uid, area, typ, info.AdsPic, info.AdsUrl, info.Price, info.AdsTitle);
                    if (ggid == Guid.Empty)
                    {
                        Page.RegisterStartupScript("通知", "<script>alert('发送添加广告错误');</script>");
                        return;
                    }
                    userAdsDal.InsertUserAds(info, ggid);
                }
            }
        }

        //if (notou)
        //    Response.Redirect("UserAdsList.aspx");
        //else
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
