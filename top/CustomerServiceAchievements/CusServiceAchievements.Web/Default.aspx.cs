using System;
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

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string s = HttpUtility.UrlEncode("上宫庄健客专卖店");
        Response.Write(s);

        //TaoBaoAPIHelper.TaoBaoAPI.GetGoodsInfoListByNick("luckyfish8800", "6101312587cbdace711a1e26d5877064329a4dd05d9c96326907498");
        //TaoBaoAPIHelper.TaoBaoAPI.GetGoodsInfoListByNick("上宫庄健客专卖店", "610242312a8c4906036867f3ce62e7516cbe073aadea77b667819556");

        //TaoBaoAPIHelper.TaoBaoAPI.GetNickGroupList("luckyfish8800:售后", "6101312587cbdace711a1e26d5877064329a4dd05d9c96326907498");

        //string s = TaoBaoAPIHelper.TaoBaoAPI.GetShopInfo("上宫庄健客专卖店");
        //return;
    }
}
