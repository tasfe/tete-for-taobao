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

public partial class AddSite : System.Web.UI.Page
{
    AdsSiteService adsSiteDal = new AdsSiteService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            RPT_SiteList.DataSource = CacheCollection.GetAllSiteList();
            RPT_SiteList.DataBind();
        }
    }

    protected void BTN_Add_Click(object sender, EventArgs e)
    {
        if (CheckNull())
        {
            SiteInfo info = new SiteInfo(TB_SiteName.Text.Trim(), TB_SiteUrl.Text.Trim());
            adsSiteDal.AddSite(info);
            CacheCollection.RemoveCacheByKey(CacheCollection.KEY_ALLSITEINFO);
            Response.Redirect("AddSite.aspx");
        }
    }

    protected void BTN_Up_Click(object sender, EventArgs e)
    {
        if (CheckNull())
        {
            SiteInfo info = new SiteInfo(TB_SiteName.Text.Trim(), TB_SiteUrl.Text.Trim());
            info.SiteId = new Guid(ViewState["siteid"].ToString());

            adsSiteDal.UpdateSite(info);
            CacheCollection.RemoveCacheByKey(CacheCollection.KEY_ALLSITEINFO);
            Response.Redirect("AddSite.aspx");
        }
    }

    private bool CheckNull()
    {
        if (string.IsNullOrEmpty(TB_SiteName.Text.Trim()) || string.IsNullOrEmpty(TB_SiteUrl.Text.Trim()))
            return false;
        return true;
    }

    protected void RPT_SiteList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        
        if (e.CommandName == "Up")
        {
            SiteInfo info = adsSiteDal.SelectSite(new Guid(e.CommandArgument.ToString()));

            TB_SiteName.Text = info.SiteName;
            TB_SiteUrl.Text = info.SiteUrl;
            ViewState["siteid"] = info.SiteId.ToString();
            BTN_Up.Visible = true;
            BTN_Add.Visible = false;
        }
        if (e.CommandName == "De")
        {
            adsSiteDal.DeleteSite(new Guid(e.CommandArgument.ToString()));
            Response.Redirect("AddSite.aspx");
        }

        if (e.CommandName == "Manage")
        {
            Response.Redirect("AddAds.aspx?siteId=" + e.CommandArgument.ToString());
        }
    }
}
