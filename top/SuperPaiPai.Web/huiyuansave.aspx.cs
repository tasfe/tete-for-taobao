using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PaiPai.DAL;
using PaiPai.Model;

public partial class huiyuansave : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string nick = "";
            if (Request.Cookies["nick"] != null)
                nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value); //"nick"; 
            else
                nick = Session["snick"].ToString();
            if (nick == "")
            {
                Response.Redirect("http://fuwu.paipai.com/appstore/ui/my/app/appdetail.xhtml?appId=234454");
            }
            string typ = Request.QueryString["typ"];

            PaiPaiShopService ppsDal = new PaiPaiShopService();
            PaiPaiShopInfo info = ppsDal.GetPaiPaiShopInfo(nick);


            if (typ == "1")
            {
                if (info.MessgeCount <= 0 && !info.NotPay)
                    Response.Write("{\"errorType\":\"integral_not_enough\",\"msg\":\"余额不足，请先充值!\",\"success\":false}");
                else
                {
                    if (info.NotPay)
                    {
                        ppsDal.UpdateNotPay(false, info.NotPayPostModel, info.NotPayExpiredMinutes, nick);
                        Response.Write("{\"success\":true}");
                    }
                    else
                    {
                        if (info.MessgeCount > 0)
                        {
                            ppsDal.UpdateNotPay(true, info.NotPayPostModel, info.NotPayExpiredMinutes, nick);
                            Response.Write("{\"success\":true}");
                        }
                    }
                }
            }

            if (typ == "5")
            {
                if (info.MessgeCount <= 0 && !info.PostGoods)
                    Response.Write("{\"errorType\":\"integral_not_enough\",\"msg\":\"余额不足，请先充值!\",\"success\":false}");
                else
                {
                    if (info.PostGoods)
                    {
                        ppsDal.UpdatePostGoods(false, info.PostGoodsPostModel, nick);
                        Response.Write("{\"success\":true}");
                    }
                    else
                    {
                        if (info.MessgeCount > 0)
                        {
                            ppsDal.UpdatePostGoods(true, info.PostGoodsPostModel, nick);
                            Response.Write("{\"success\":true}");
                        }
                    }
                }
            }

            if (typ == "9")
            {
                if (info.MessgeCount <= 0 && !info.NotPing)
                    Response.Write("{\"errorType\":\"integral_not_enough\",\"msg\":\"余额不足，请先充值!\",\"success\":false}");
                else
                {
                    if (info.NotPing)
                    {
                        ppsDal.UpdateNotPing(false, info.NotPingPostModel,info.ExpiredDays, nick);
                        Response.Write("{\"success\":true}");
                    }
                    else
                    {
                        if (info.MessgeCount > 0)
                        {
                            ppsDal.UpdateNotPing(true, info.NotPingPostModel, info.ExpiredDays, nick);

                            Response.Write("{\"success\":true}");
                        }
                    }
                }
            } 

            Response.End();
        }
    }
}