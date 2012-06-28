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

public partial class ShowGoods : System.Web.UI.Page
{
    TaoBaoGoodsClassService tbgcDal = new TaoBaoGoodsClassService();
    UserAdsService uadsDal = new UserAdsService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string cid = Request.QueryString["cid"];
            string adsId = Request.QueryString["adsid"];

            IList<TaoBaoGoodsClassInfo> list = tbgcDal.SelectAllGoodsClass("0");

            RPT_GOODSCLASS.DataSource = list;
            RPT_GOODSCLASS.DataBind();

            //获取分类下所有用户投放的广告
            IList<UserAdsInfo> adsList = uadsDal.SelectAllUserAdsByAdsId(new Guid(adsId), 1);
            //按时间倒序排列
            adsList = adsList.OrderByDescending(o => o.AddTime).ToList();
            RPT_AdsList.DataSource = adsList;
            RPT_AdsList.DataBind();

        }
    }
}
