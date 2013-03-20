using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PaiPai.DAL;
using PaiPai.Model;

public partial class msgset : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (string.IsNullOrEmpty(Request.QueryString["type"]))
            {
                Response.Redirect("");
                return;
            }

            int type = int.Parse(Request.QueryString["type"]);
            if (Request.Cookies["nick"] != null)
                Nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value); //"nick"; 
            else
                Nick = Session["snick"].ToString();
            if (Nick == "")
            {
                Response.Redirect("");
            }
            PaiPaiShopService ppsDal = new PaiPaiShopService();
            PaiPaiShopInfo info = ppsDal.GetPaiPaiShopInfo(Nick);
            if (type == 1)
            {
                panel3.Visible = true;
                template_content1.Value = info.NotPayPostModel;
                Text1.Value = info.NotPayExpiredMinutes.ToString();
            }

            else if (type == 5)
            {
                panel1.Visible = true;
                template_content5.Value = info.PostGoodsPostModel;
            }

            else if (type == 9)
            {
                panel2.Visible = true;
                template_content9.Value = info.NotPingPostModel;
                pay_hour.Value = info.ExpiredDays.ToString();
            }
            else
            {
                Response.Redirect("http://fuwu.paipai.com/appstore/ui/my/app/appdetail.xhtml?appId=234454");
                return;
            }
            
            ViewState["PaiPaiInfo"] = info;
            CanPoCount = info.MessgeCount.ToString();
            HadPoCount = info.HadPost.ToString();
            ViewState["nick"] = Nick;
        }
    }

    protected PaiPaiShopInfo PaiPaiInfo
    {
        get { return (PaiPaiShopInfo)ViewState["PaiPaiInfo"]; }
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
    protected void Btn_Save_Click(object sender, EventArgs e)
    {
        PaiPaiShopService ppsDal = new PaiPaiShopService();
        int type = int.Parse(Request.QueryString["type"]);
        if (type == 1)
        {
            panel3.Visible = true;

            int minutes = PaiPaiInfo.NotPayExpiredMinutes;
            try
            {
                minutes = int.Parse(Text1.Value);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('请在买家下单后面的输入框输入数字');</script>");
                return;
            }

            if (!template_content1.Value.Contains("{ShopName}"))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('请您把{ShopName}添加到模板内容中');</script>");
                return;
            }

            ppsDal.UpdateNotPay(PaiPaiInfo.NotPay, template_content1.Value, minutes, ViewState["nick"].ToString());
        }

        else if (type == 5)
        {
            panel1.Visible = true;
            if (!template_content5.Value.Contains("{ShopName}"))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('请您把{ShopName}添加到模板内容中');</script>");
                return;
            }
            if (!template_content5.Value.Contains("{ExpressName}") || !template_content5.Value.Contains("{ExpressNo}"))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('请您把{ExpressName}{ExpressNo}添加到模板内容中');</script>");
                return;
            }
            ppsDal.UpdatePostGoods(PaiPaiInfo.PostGoods, template_content5.Value, ViewState["nick"].ToString());
        }

        else if (type == 9)
        {
            panel2.Visible = true;

            int days = PaiPaiInfo.ExpiredDays;
            try
            {
                days = int.Parse(pay_hour.Value);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('请在发货后面的输入框输入数字');</script>");
                return;
            }
            if (!template_content9.Value.Contains("{ShopName}"))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('请您把{ShopName}添加到模板内容中');</script>");
                return;
            }
            ppsDal.UpdateNotPing(PaiPaiInfo.NotPing, template_content9.Value, days, ViewState["nick"].ToString());
        }

        else
        {

        }

        ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('设置成功');</script>");
    }
}