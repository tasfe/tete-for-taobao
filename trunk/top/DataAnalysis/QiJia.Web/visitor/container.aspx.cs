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

public partial class visitor_container : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                HttpCookie cookie = new HttpCookie("nick", Request.QueryString["id"]);
                cookie.Expires = DateTime.Now.AddDays(1);

                Response.Cookies.Add(cookie);
                IList<string> list = CacheCollection.GetShopIdList().Where(o => o == Request.QueryString["id"]).ToList();
                if (list.Count == 0)
                {
                    ShopTotalInfo info = new ShopTotalInfo(Request.QueryString["id"], DateTime.Now);
                    new ShopTotalService().InsertShopTotal(info);
                }

                Response.Redirect("index.html");
            }
        }
    }
}
