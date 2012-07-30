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

public partial class ManageSiteAds : System.Web.UI.Page
{

    AdsSiteService adsSiteDal = new AdsSiteService();

    SiteAdsService siteAdsDal = new SiteAdsService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DDL_SiteList.DataSource = adsSiteDal.SelectAllSite();
            DDL_SiteList.DataValueField = "SiteUrl";
            DDL_SiteList.DataTextField = "SiteName";
            DDL_SiteList.DataBind();
            DDL_SiteList.Items.Insert(0, new ListItem("所有", Guid.Empty.ToString()));

            //显示网站名称和地址
            if (!string.IsNullOrEmpty(Request.QueryString["url"]))
            {
                try
                {
                    IList<SiteAdsInfo> list = siteAdsDal.SelectAllSiteAds(Request.QueryString["url"]);
                    RPT_AdsList.DataSource = list;
                    RPT_AdsList.DataBind();

                    DDL_SiteList.SelectedValue = Request.QueryString["siteid"];
                }
                catch (Exception ex) { }
            }
        }
    }

    protected string GetAdsType(string adstype)
    {

        if (adstype == SiteAdsType.Inner.ToString())
            return "站内广告";
        if (adstype == SiteAdsType.Out.ToString())
            return "站外广告";
        if (adstype == SiteAdsType.TaoBao.ToString())
            return "淘宝广告";
        return "未添加类型";
    }

    protected void BTN_Add_Click(object sender, EventArgs e)
    {
        if (DDL_SiteList.SelectedValue != Guid.Empty.ToString())
        {
            SiteAdsInfo info = new SiteAdsInfo();
            info.Id = Guid.NewGuid();
            info.SiteUrl = DDL_SiteList.SelectedValue;
            info.AdsCode = TB_AdsCode.Value;
            info.AdsPosition = TB_AdsPosition.Text;
            info.AdsType = (SiteAdsType)int.Parse(DDL_AdsType.SelectedValue);

            siteAdsDal.InsertSiteAds(info);
            Response.Redirect("ManageSiteAds.aspx?url=" + DDL_SiteList.SelectedValue);
        }
    }

    protected void RPT_AdsList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "De")
        {
            siteAdsDal.DeleteAds(e.CommandArgument.ToString());
            Response.Redirect("ManageSiteAds.aspx?url=" + DDL_SiteList.SelectedValue);
        }
        if (e.CommandName == "Up")
        {
            SiteAdsInfo info = siteAdsDal.SelectSiteAdsById(e.CommandArgument.ToString());
            if (info != null)
            {
                TB_AdsCode.Value = info.AdsCode;
                TB_AdsPosition.Text = info.AdsPosition;
                DDL_AdsType.SelectedValue = info.AdsType.ToString();
                ViewState["id"] = info.Id.ToString();
                BTN_Up.Visible = true;
                BTN_Add.Visible = false;
            }
        }
    }

    protected void DDL_SiteList_TextChanged(object sender, EventArgs e)
    {
        IList<SiteAdsInfo> list = siteAdsDal.SelectAllSiteAds(DDL_SiteList.SelectedValue);
        RPT_AdsList.DataSource = list;
        RPT_AdsList.DataBind();
    }
    protected void BTN_Up_Click(object sender, EventArgs e)
    {
        SiteAdsInfo info = new SiteAdsInfo();

        info.Id = new Guid(ViewState["id"].ToString());
        info.SiteUrl = DDL_SiteList.SelectedValue;
        info.AdsCode = TB_AdsCode.Value;
        info.AdsPosition = TB_AdsPosition.Text;
        info.AdsType = (SiteAdsType)int.Parse(DDL_AdsType.SelectedValue);

        siteAdsDal.UpdateAds(info);
        Response.Redirect("ManageSiteAds.aspx?url=" + info.SiteUrl);
    }
}
