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

public partial class ipclick : System.Web.UI.Page
{

    UserAdsService uasDal = new UserAdsService();
    ClickService clickDal = new ClickService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            IList<AdsInfo> adsList = CacheCollection.GetAllAdsInfo();

            Random rand = new Random();
            AdsInfo info = adsList[rand.Next(adsList.Count - 1)];

            IList<UserAdsInfo> list = uasDal.SelectAllUserAdsByAdsId(info.AdsId, 1);

            IList<ClickInfo> clickList = clickDal.SelectAllClickCount(DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("yyyyMMdd"));

            foreach (ClickInfo cinfo in clickList)
            {
                if (cinfo.ClickCount >= 10)
                {
                    IList<UserAdsInfo> hadlist = list.Where(o => o.Id == cinfo.UserAdsId).ToList();
                    if (hadlist.Count > 0)
                    {
                        list.Remove(hadlist[0]);
                    }
                }
            }

            if (list.Count == 0)
                return;

            UserAdsInfo uinfo = list[0];
            if (list.Count > 1)
                uinfo = list[rand.Next(list.Count - 1)];

            string param = "id=" + uinfo.Id + "&url=" + uinfo.AdsUrl;
            Response.Redirect("getclick.aspx?" + HttpUtility.UrlEncode(param));
        }
    }
}
