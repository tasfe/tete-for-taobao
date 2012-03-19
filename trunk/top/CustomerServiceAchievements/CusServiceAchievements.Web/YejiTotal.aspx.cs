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
using Model;
using CusServiceAchievements.DAL;

public partial class YejiTotal : System.Web.UI.Page
{
    TopKefuTotalService kftDal = new TopKefuTotalService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DateTime[] drrArr = DBHelp.DataHelper.GetDateTime(DateTime.Now.AddDays(-29),30);
            IList<TopKefuTotalInfo> list = kftDal.GetTotalinfoList(drrArr[0], drrArr[1]);
            Rpt_KefuTotal.DataSource = list;
            Rpt_KefuTotal.DataBind();
        }
    }

    protected void Btn_Select_Click(object sender, EventArgs e)
    {

    }
}
