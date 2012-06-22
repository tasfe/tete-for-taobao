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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string tid = Request.QueryString["id"];
            IList<TaoBaoGoodsClassInfo> list = tbgcDal.SelectAllGoodsClass("0");

            RPT_GOODSCLASS.DataSource = list;
            RPT_GOODSCLASS.DataBind();
        }
    }
}
