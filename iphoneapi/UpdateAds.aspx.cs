using System;
using System.Web;
using System.Web.UI;

public partial class UpdateAds : BasePage
{
    TeteShopService tsDal = new TeteShopService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);

            TeteShopInfo info = tsDal.GetShopInfo(nick);

            if (info == null)
            {
                Page.RegisterStartupScript("错误", "<script>alert('您的身份不合法，请确定您已购买!');</script>");
                return;
            }

            Tb_Ads.Text = info.Ads;
            Tb_Logo.Text = info.Logo;

            ViewState["guid"] = info.Guid;
        }
    }

    protected void Btn_Up_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Tb_Ads.Text.Trim()))
        {
            Page.RegisterStartupScript("提示", "<script>alert('请输入Ads链接地址!');</script>");
            return;
        }

        if (string.IsNullOrEmpty(Tb_Logo.Text.Trim()))
        {
            Page.RegisterStartupScript("提示", "<script>alert('请输入Logo链接地址!');</script>");
            return;
        }

        TeteShopInfo info = new TeteShopInfo();
        info.Guid = new Guid(ViewState["guid"].ToString());

        info.Ads =Tb_Ads.Text.Trim();
        info.Logo = Tb_Logo.Text.Trim();

        tsDal.UpdateShopInfo(info);
        Page.RegisterStartupScript("提示", "<script>alert('更新成功!');</script>");
    }
}
