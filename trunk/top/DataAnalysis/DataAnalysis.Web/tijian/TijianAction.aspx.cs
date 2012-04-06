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
        Lb_Liulan.Text = vas[0].Substring(2,1);
        if (vas[0].Substring(0,1) == "0")
            div_Liulan.Visible = true;

        Lb_SellGuanlian.Text = vas[1].Substring(2, 1);
        if (vas[1].Substring(0, 1) == "0")
            div_SellGuanlian.Visible = true;

        Lb_Zhuanhuan.Text = vas[2].Substring(2, 1);
        if (vas[2].Substring(0, 1) == "0")
            div_Zhuanhuan.Visible = true;

        Lb_SeeBack.Text = vas[3].Substring(2, 1);
        if (vas[3].Substring(0, 1) == "0")
            div_SeeBack.Visible = true;

        Lb_BuyBack.Text = vas[4].Substring(2, 1);
        if (vas[4].Substring(0, 1) == "0")
            div_BuyBack.Visible = true;

        Lb_SeeDeep.Text = vas[5].Substring(2, 1);
        if (vas[5].Substring(0, 1) == "0")
            div_SeeDeep.Visible = true;

        Lb_TopGoods.Text = vas[6].Substring(2, 1);
        if (vas[6].Substring(0, 1) == "0")
            div_TopGoods.Visible = true;
    }
}
