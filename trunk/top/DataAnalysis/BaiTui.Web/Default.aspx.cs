using System;

public partial class _Default : System.Web.UI.Page 
{
    TuiGoodsService tuiDal = new TuiGoodsService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            System.Collections.Specialized.NameValueCollection s = Request.QueryString;

            if (s.Count != 1)
                return;

            string id = s.ToString();

            string goodsId = tuiDal.GetGoodsId(id);

            Response.Redirect("http://item.taobao.com/item.htm?id=" + goodsId);
        }
    }
}
