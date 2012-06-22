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

public partial class ShowAds : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["adsid"]))
            {
                IList<AdsInfo> list = CacheCollection.GetAllAdsInfo().Where(o => o.AdsId.ToString() == Request.QueryString["adsid"]).ToList();
                if (list.Count > 0)
                {
                    AdsInfo info = list[0];
                    ViewState["ImgUrl"] = info.AdsPic;
                }
            }
        }
    }

    protected string ImgUrl
    {
        get
        {
            return ViewState["ImgUrl"].ToString();
        }
    }
}
