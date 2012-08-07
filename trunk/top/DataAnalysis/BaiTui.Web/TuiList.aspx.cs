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

public partial class TuiList : System.Web.UI.Page
{
    TuiGoodsService tuigDal = new TuiGoodsService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int type = 1;
            if (!string.IsNullOrEmpty(Request.QueryString["type"]))
            {
                try
                {
                    type = int.Parse(Request.QueryString["type"]);
                }
                catch
                {
                }
            }

            if (type == 1)
                Lbl_TuiType.Text = "百度推广列表";
            if (type == 2)
                Lbl_TuiType.Text = "QQ推广列表";

            string nick = "nick";
            if (Request.Cookies["nick"] != null)
                nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value); //"nick"; 
            else
            {
                if (Session["snick"] != null)
                    nick = Session["snick"].ToString();
            }

            IList<TuiGoodsInfo> list = tuigDal.GetAllTuiGoodsByType(nick, type);
            Rpt_TuiList.DataSource = list;
            Rpt_TuiList.DataBind();
        }
    }
}
