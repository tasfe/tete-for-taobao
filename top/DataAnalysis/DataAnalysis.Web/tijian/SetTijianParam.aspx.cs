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

public partial class tijian_SetTijianParam : BasePage
{
    TijianParamValueService tijianDal = new TijianParamValueService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string nick =HttpUtility.UrlDecode( Request.Cookies["nick"].Value);
            IList<TijianParamInfo> list = tijianDal.GetParamInfo(nick);

            if (list.Count == 0)
            {
                DataHelper.GetParam(list);
            }

            Rpt_tijian.DataSource = list;
            Rpt_tijian.DataBind();
        }
    }
    protected void btn_Up_Click(object sender, EventArgs e)
    {
        string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
        IList<TijianParamInfo> list = new List<TijianParamInfo>();
        foreach (RepeaterItem item in Rpt_tijian.Items)
        {
            if (item is IDataItemContainer)
            {
                string pname = (item.FindControl("Lb_ParamName") as Label).Text;
                string pvalue = (item.FindControl("Tb_ParamValue") as TextBox).Text;
                try
                {
                    list.Add(new TijianParamInfo { Nick = nick, ParamName = pname, ParamValue = decimal.Parse(pvalue) });
                }
                catch (Exception ex)
                {
                    Page.RegisterStartupScript("错误提示", "<script>alert('比例应为数据类型，如1、2.2、3.33')</script>");
                    return;
                }
            }
        }

        tijianDal.InsertTijianInfo(list);
    }
}
