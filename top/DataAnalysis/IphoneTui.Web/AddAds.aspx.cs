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

public partial class AddAds : System.Web.UI.Page
{
    AdsSiteService adsSiteDal = new AdsSiteService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DDL_SiteList.DataSource = adsSiteDal.SelectAllSite();
            DDL_SiteList.DataValueField = "SiteId";
            DDL_SiteList.DataTextField = "SiteName";
            DDL_SiteList.DataBind();
            DDL_SiteList.Items.Insert(0, new ListItem("所有", Guid.Empty.ToString()));

            //显示网站名称和地址
            if (!string.IsNullOrEmpty(Request.QueryString["siteid"]))
            {
                try
                {
                    DDL_SiteList.SelectedValue = Request.QueryString["siteid"];
                    IList<SiteAdsInfo> list = new SiteAdsService().SelectSiteAdsById(new Guid(DDL_SiteList.SelectedValue));
                    RPT_AdsList.DataSource = list;
                    RPT_AdsList.DataBind();

                }
                catch (Exception ex) { }
            }

            DDL_AdsList.DataSource = CacheCollection.GetAllAdsInfo();
            DDL_AdsList.DataValueField = "AdsId";
            DDL_AdsList.DataTextField = "AdsName";
            DDL_AdsList.DataBind();
        }
    }

    protected void BTN_Add_Click(object sender, EventArgs e)
    {
        if (DDL_SiteList.SelectedValue != Guid.Empty.ToString())
        {
            SiteAdsInfo info = new SiteAdsInfo();
            info.AdsId = new Guid(DDL_AdsList.SelectedValue);
            info.SiteId = new Guid(DDL_SiteList.SelectedValue);
            info.AdsPosition = TB_AdsName.Text.Trim();
            info.AdsCount = int.Parse(TB_Count.Text.Trim());
            info.AdsCalCount = info.AdsCount;
            info.PositionCode = int.Parse(TB_PostCode.Text.Trim());
            info.Id = Guid.NewGuid();

            FUP_Up.SaveAs(Server.MapPath("~/adsimg") + "/" + info.Id + ".jpg");
            info.AdsPic = "/adsimg/" + info.Id + ".jpg";

            new SiteAdsService().InsertSiteAds(info);
            CacheCollection.RemoveCacheByKey(CacheCollection.KEY_ALLSITEADS);
            Response.Redirect("AddAds.aspx?siteid=" + info.SiteId);
        }
    }

    protected string GetSiteName(string siteId)
    {
        return CacheCollection.GetAllSiteList().Where(o => o.SiteId == new Guid(siteId)).ToList()[0].SiteName;
    }

    protected void RPT_AdsList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Up")
        {
            SiteAdsInfo info = CacheCollection.GetAllSiteAdsInfo().Where(o => o.Id == new Guid(e.CommandArgument.ToString())).ToList()[0];
           
            TB_AdsName.Text = info.AdsPosition;
            DDL_AdsList.SelectedValue = info.AdsId.ToString();
            TB_PostCode.Text = info.PositionCode.ToString();
            TB_Count.Text = info.AdsCount.ToString();
            BTN_Up.Visible = true;
            BTN_Add.Visible = false;
            ViewState["siteadsId"] = info.Id;
        }
        if (e.CommandName == "De")
        {
            adsSiteDal.DeleteAds(new Guid(e.CommandArgument.ToString()));
            CacheCollection.RemoveCacheByKey(CacheCollection.KEY_ALLADSINFO);
            Response.Redirect("AddAds.aspx?siteid="  + (string.IsNullOrEmpty(Request.QueryString["siteid"]) ? DDL_SiteList.SelectedValue : Request.QueryString["siteid"]));
        }

    }
    protected void BTN_Up_Click(object sender, EventArgs e)
    {
        SiteAdsInfo info = new SiteAdsInfo();
        info.Id = new Guid(ViewState["siteadsId"].ToString());
        info.AdsId = new Guid(DDL_AdsList.SelectedValue);
        info.SiteId = new Guid(DDL_SiteList.SelectedValue);
        info.AdsPosition = TB_AdsName.Text.Trim();
        info.AdsCount = int.Parse(TB_Count.Text.Trim());
        info.AdsCalCount = info.AdsCount;
        info.PositionCode = int.Parse(TB_PostCode.Text.Trim());

        new SiteAdsService().UpdateAds(info);
        CacheCollection.RemoveCacheByKey(CacheCollection.KEY_ALLSITEADS);
        Response.Redirect("AddAds.aspx?siteid=" + (string.IsNullOrEmpty(Request.QueryString["siteid"]) ? DDL_SiteList.SelectedValue : Request.QueryString["siteid"]));
    }

    protected void DDL_SiteList_TextChanged(object sender, EventArgs e)
    {
        IList<SiteAdsInfo> list = new SiteAdsService().SelectSiteAdsById(new Guid(DDL_SiteList.SelectedValue));
        RPT_AdsList.DataSource = list;
        RPT_AdsList.DataBind();
    }

    protected string GetAdsSize(string adsId)
    {
        AdsInfo info = CacheCollection.GetAllAdsInfo().Where(o => o.AdsId.ToString() == adsId).ToList()[0];
        return info.AdsSize;
    }

    protected string GetAdsName(string adsId)
    {
        AdsInfo info = CacheCollection.GetAllAdsInfo().Where(o => o.AdsId.ToString() == adsId).ToList()[0];
        return info.AdsName;
    }
}
