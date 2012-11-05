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
                    IList<AdsInfo> list = adsSiteDal.SelectAllAdsBySiteId(new Guid(Request.QueryString["siteid"]));
                    RPT_AdsList.DataSource = list;
                    RPT_AdsList.DataBind();

                    DDL_SiteList.SelectedValue = Request.QueryString["siteid"];
                }
                catch (Exception ex) { }
            }
        }
    }

    private bool CheckNull()
    {
        if (string.IsNullOrEmpty(TB_AdsName.Text.Trim()) || string.IsNullOrEmpty(TB_AdsSize.Text.Trim()))
            return false;
        return true;
    }

    protected void BTN_Add_Click(object sender, EventArgs e)
    {
        if (CheckNull())
        {
            if (DDL_SiteList.SelectedValue != Guid.Empty.ToString())
            {
                AdsInfo info = new AdsInfo(new Guid(DDL_SiteList.SelectedValue), TB_AdsName.Text.Trim(), TB_AdsSize.Text.Trim());
                info.AdsType = int.Parse(DDL_AdsType.SelectedValue);

                FUP_Up.SaveAs(Server.MapPath("~/adsimg") + "/" + info.AdsId + ".jpg");
                info.AdsPic = "/adsimg/" + info.AdsId + ".jpg";

                adsSiteDal.AddAds(info);
                CacheCollection.RemoveCacheByKey(CacheCollection.KEY_ALLADSINFO);
                Response.Redirect("AddAds.aspx?siteid=" + info.SiteId);
            }
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
            AdsInfo info = adsSiteDal.SelectAdsInfo(new Guid(e.CommandArgument.ToString()));

            TB_AdsName.Text = info.AdsName;
            TB_AdsSize.Text = info.AdsSize;
            DDL_AdsType.SelectedValue = info.AdsType.ToString();
            ViewState["adsid"] = info.AdsId.ToString();
            BTN_Up.Visible = true;
            BTN_Add.Visible = false;
        }
        if (e.CommandName == "De")
        {
            adsSiteDal.DeleteAds(new Guid(e.CommandArgument.ToString()));
            CacheCollection.RemoveCacheByKey(CacheCollection.KEY_ALLADSINFO);
            Response.Redirect("AddAds.aspx?siteid=" + Request.QueryString["siteid"]);
        }

    }
    protected void BTN_Up_Click(object sender, EventArgs e)
    {
        if (CheckNull())
        {
            AdsInfo info = new AdsInfo();
            info.AdsId = new Guid(ViewState["adsid"].ToString());
            info.AdsName = TB_AdsName.Text.Trim();
            info.AdsSize = TB_AdsSize.Text.Trim();
            info.AdsType = int.Parse(DDL_AdsType.SelectedValue);

            adsSiteDal.UpdateAds(info);
            CacheCollection.RemoveCacheByKey(CacheCollection.KEY_ALLADSINFO);
            Response.Redirect("AddAds.aspx?siteid=" + Request.QueryString["siteid"]);
        }
    }
    protected void DDL_SiteList_TextChanged(object sender, EventArgs e)
    {
        IList<AdsInfo> list = adsSiteDal.SelectAllAdsBySiteId(new Guid(DDL_SiteList.SelectedValue));
        RPT_AdsList.DataSource = list;
        RPT_AdsList.DataBind();
    }

    protected string GetAdsType(string type)
    {
        if (type == "1")
            return "不限个数";
        if (type == "5")
            return "单个";
        return "";
    }
}
