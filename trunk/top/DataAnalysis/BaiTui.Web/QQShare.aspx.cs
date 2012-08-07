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

public partial class QQShare : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string id = Request.QueryString["id"];

            TuiGoodsService tuiDal = new TuiGoodsService();
            try
            {
                TuiGoodsInfo info = tuiDal.GetTuiGoods(new Guid(id));

                TB_KeyWords.Text = info.Keywords.Replace("[ksp]", " ");
                ViewState["GoodsPic"] = info.GoodsPic;
                ViewState["GoodsUrl"] = "http://item.taobao.com/item.htm?id=" + info.GoodsId;
                ViewState["GoodsName"] = info.GoodsName;
                ViewState["Keys"] = info.Keywords.Replace("[ksp]", " ");

                ViewState["ShowUrl"] = Request.Url.ToString().Substring(0, Request.Url.ToString().LastIndexOf("/")) + "/?" + id.Substring(0, 8);
            }
            catch (Exception ex)
            {

            }
        }
    }

    protected string ShowUrl
    {
        get
        {
            if (ViewState["ShowUrl"] == null)
                return "";
            return ViewState["ShowUrl"].ToString();
        }
    }

    protected string Keys
    {
        get
        {
            if (ViewState["Keys"] == null)
                return "";
            return ViewState["Keys"].ToString();
        }
    }

    protected string GoodsPic
    {
        get
        {
            if (ViewState["GoodsPic"] == null)
                return "";
            return ViewState["GoodsPic"].ToString();
        }
    }

    protected string GoodsUrl
    {
        get
        {
            if (ViewState["GoodsUrl"] == null)
                return "";
            return ViewState["GoodsUrl"].ToString();
        }
    }

    protected string GoodsName
    {
        get
        {
            if (ViewState["GoodsName"] == null)
                return "";
            return ViewState["GoodsName"].ToString();
        }
    }
}
