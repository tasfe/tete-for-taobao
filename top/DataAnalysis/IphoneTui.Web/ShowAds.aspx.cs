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

public partial class ShowAds : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["adsid"]))
            {
                IList<SiteAdsInfo> list = CacheCollection.GetAllSiteAdsInfo().Where(o => o.AdsId.ToString() == Request.QueryString["adsid"]).ToList();
                if (list.Count > 0)
                {
                    SiteAdsInfo info = list[0];
                    
                    IList<SiteInfo> sitelist = CacheCollection.GetAllSiteList().Where(o => o.SiteId == info.SiteId).ToList();

                    Lbl_AppName.Text = sitelist[0].SiteName;
                    Lbl_AdsPosition.Text = info.AdsPosition;

                    ViewState["SiteUrl"] = sitelist[0].SiteUrl;
                }
            }
        }
    }
}
