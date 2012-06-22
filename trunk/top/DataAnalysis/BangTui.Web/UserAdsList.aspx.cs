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
            string nick = "nick";
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

        }
        if (e.CommandName == "Insert")
        {

        }
        if (e.CommandName == "Result")
        {

        }
        if (e.CommandName == "See")
        {

        }
        if (e.CommandName == "Stop")
        {

        }
        if (e.CommandName == "Add")
        {

        }
    }
}
