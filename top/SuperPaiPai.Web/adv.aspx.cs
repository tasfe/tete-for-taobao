using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using PaiPai.Model;
using PaiPai.DAL;

public partial class adv : System.Web.UI.Page
{

    public string gundong = "";

    public string nick = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.Cookies["nick"] != null)
                Nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value); //"nick"; 
            else
                Nick = Session["snick"].ToString();
            if (Nick == "")
            {
                Response.Redirect("http://fuwu.paipai.com/appstore/ui/my/app/appdetail.xhtml?appId=234454");
            }

            BuyInfo info = new PaiPaiShopService().GetBuyInfo(Nick);
            Copyright = "初级版";
            ExpiredTime = info.ExpiedTime.ToString("yyyy-MM-dd");
        }
    }

    protected string Nick
    {
        set;
        get;
    }

    protected string Copyright
    {
        set;
        get;
    }

    protected string ExpiredTime
    {
        set;
        get;
    }
}