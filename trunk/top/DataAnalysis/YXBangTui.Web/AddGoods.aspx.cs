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

public partial class AddGoods : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string nick = "";
        if (Request.Cookies["nick"] != null)
            nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value); //"nick"; 
        else
            nick = Session["snick"].ToString();
        if (nick == "")
        {
            Response.Write("请重新登录");
            return;
        }

        IList<CateInfo> cateList = new CateService().SelectAllCateByNick(nick);
        DDL_Cate.DataSource = cateList;
        DDL_Cate.DataTextField = "CateName";
        DDL_Cate.DataValueField = "CateId";
        DDL_Cate.DataBind();

    }

    protected void Btn_Add_Click(object sender, EventArgs e)
    {
        string nick = "";
        if (Request.Cookies["nick"] != null)
            nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value); //"nick"; 
        else
            nick = Session["snick"].ToString();
        if (nick == "")
        {
            Response.Write("请重新登录");
            return;
        }
        if (CheckVali())
        {
            TaoBaoGoodsInfo info = new TaoBaoGoodsInfo();

            try
            {
                info.seller_cids = DDL_Cate.SelectedValue;
                info.num_iid = TB_ID.Text;
                info.title = TB_GoodsName.Text.Trim();
                info.price = decimal.Parse(TB_Price.Text.Trim());
                info.pic_url = TB_Url.Text.Trim();
                info.cid = "";
                info.num = int.Parse(TB_Count.Text);

                info.modified = DateTime.Now;

                new GoodsService().InsertGoodsInfo(info, nick);

                Response.Redirect("UserAddAds.aspx");
            }
            catch (Exception ex)
            {
                Page.RegisterStartupScript("error", "<script>alert('" + ex.Message + "');</script>");
            }
        }
        else
        {
            Page.RegisterStartupScript("error", "<script>alert('请完整填写商品信息');</script>");
        }
    }

    private bool CheckVali()
    {
        if (string.IsNullOrEmpty(TB_ID.Text.Trim()) || string.IsNullOrEmpty(TB_GoodsName.Text.Trim()) || string.IsNullOrEmpty(TB_Price.Text.Trim()) || string.IsNullOrEmpty(TB_Url.Text.Trim()))
        {
            return false;
        }

        return true;
    }
}
