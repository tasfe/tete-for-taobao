using System;
using Model;
using System.Collections.Generic;
using System.Web;
using System.Linq;

public partial class tijian_TijianAction : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
            string paramvalue = Request.QueryString["paramvalue"];

            string[] vas = paramvalue.Replace("{", "").Replace("}", "").Split(',');

            InitValue(nick);
            InitRealValue(vas);
        }
    }

    private void InitValue(string nick)
    {
        TijianParamValueService tijianDal = new TijianParamValueService();

        IList<TijianParamInfo> list = tijianDal.GetParamInfo(nick);
        if (list.Count == 0)
            DataHelper.GetParam(list);
        Lb_L_Liulan.Text = list.First(o => o.ParamName == "客户浏览比率").ParamValue.ToString();
        Lb_L_SeeBack.Text = list.First(o => o.ParamName == "浏览回头率").ParamValue.ToString();
        Lb_L_SeeDeep.Text = list.First(o => o.ParamName == "页面访问深度").ParamValue.ToString();
        Lb_L_SellGuanlian.Text = list.First(o => o.ParamName == "销售关联度").ParamValue.ToString();
    }

    private void InitRealValue(string[] vas)
    {
        Lb_Liulan.Text = vas[0];
        Lb_SellGuanlian.Text = vas[1];
        Lb_Zhuanhuan.Text = vas[2];
        Lb_SeeBack.Text = vas[3];
        Lb_BuyBack.Text = vas[4];
        Lb_SeeDeep.Text = vas[5];
        Lb_TopGoods.Text = vas[6];
    }
}
