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

public partial class UpdateGoods : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Btn_UpdateClass_Click(object sender, EventArgs e)
    {

        string nick = "";
        string session = "";
        if (Request.Cookies["nick"] != null)
        {
            nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value); //"nick"; 
            session = Request.Cookies["nicksession"].Value;
        }
        else
        {
            nick = Session["snick"].ToString();
            session = Session["ssession"].ToString();
        }
        if (nick == "" || session == "")
        {
            Response.Write("请重新登录");
            return;
        }

        IList<TaoBaoGoodsClassInfo> classList = TopAPI.GetGoodsClassInfoList(nick, session);

        if (classList != null)
        {
            TaoBaoGoodsClassService tbgcDal = new TaoBaoGoodsClassService();
            tbgcDal.DeleteClassByNick(nick);

            foreach (TaoBaoGoodsClassInfo cinfo in classList)
            {
                tbgcDal.InsertGoodsClass(cinfo, nick);
            }

            Page.RegisterStartupScript("Info", "<script>alert('更新宝贝分类成功');</script>");
        }


    }
    protected void Btn_UpdateGoods_Click(object sender, EventArgs e)
    {
        string nick = "";
        string session = "";
        if (Request.Cookies["nick"] != null)
        {
            nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value); //"nick"; 
            session = Request.Cookies["nicksession"].Value;
        }
        else
        {
            nick = Session["snick"].ToString();
            session = Session["ssession"].ToString();
        }
        if (nick == "" || session == "")
        {
            Response.Write("请重新登录");
            return;
        }

        GoodsService goodsDal = new GoodsService();
        goodsDal.DeleteGoodsByNick(nick);

        IList<TaoBaoGoodsInfo> list = TopAPI.GetGoodsInfoListByNick(nick, session);

        foreach (TaoBaoGoodsInfo info in list)
        {
            goodsDal.InsertGoodsInfo(info, nick);
        }
        Page.RegisterStartupScript("Info", "<script>alert('更新宝贝信息成功');</script>");
    }
}
