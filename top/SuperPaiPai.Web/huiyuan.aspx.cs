using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PaiPai.DAL;
using PaiPai.Model;

public partial class huiyuan : System.Web.UI.Page
{


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

            PaiPaiShopService ppsDal = new PaiPaiShopService();
            PaiPaiShopInfo info = ppsDal.GetPaiPaiShopInfo(Nick);

            CanPoCount = info.MessgeCount.ToString();
            HadPoCount = info.HadPost.ToString();
            // 1：催单；5：已发货；9：催评
            if (info.NotPay)
            {
                NoPayImgSrc = "images/on.gif";
                NoPayJsHref = "javascript:close_setting(1)";
            }
            else
            {
                NoPayImgSrc = "images/off.gif";
                NoPayJsHref = "javascript:open_setting(1)";
            }
            if (info.NotPing)
            {
                NoPingImgSrc = "images/on.gif";
                NoPingJsHref = "javascript:close_setting(9)";
            }
            else
            {
                NoPingImgSrc = "images/off.gif";
                NoPingJsHref = "javascript:open_setting(9)";
            }

            if (info.PostGoods)
            {
                PostImgSrc = "images/on.gif";
                PostJsHref = "javascript:close_setting(5)";
            }
            else
            {
                PostImgSrc = "images/off.gif";
                PostJsHref = "javascript:open_setting(5)";
            }

        }
    }

    protected string NoPayJsHref
    {
        set;
        get;
    }

    protected string NoPingJsHref
    {
        set;
        get;
    }

    protected string PostJsHref
    {
        set;
        get;
    }

    protected string NoPayImgSrc
    {
        set;
        get;
    }

    protected string NoPingImgSrc
    {
        set;
        get;
    }

    protected string PostImgSrc
    {
        set;
        get;
    }

    protected string Nick
    {
        set;
        get;
    }

    protected string CanPoCount
    {
        set;
        get;
    }

    protected string HadPoCount
    {
        set;
        get;
    }
}