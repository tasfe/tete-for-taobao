using System;
using System.Collections;
using System.Web;
using System.Collections.Generic;
using Model;
using CusServiceAchievements.DAL;
using System.Linq;
using System.Web.UI.WebControls;

public partial class UpdateCarriage : BasePage
{
    //ExpressService esDal = new ExpressService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //ProvinceService pvDal = new ProvinceService();

            IList<ExpressInfo> list = CacheCollection.GetAllExpressInfo(); //esDal.GetAllExpressInfo("");
            ddl_Express.DataSource = list;
            ddl_Express.DataTextField = "ExpressName";
            ddl_Express.DataValueField = "ExpressId";
            ddl_Express.DataBind();

            IList<ProvinceInfo> plist = CacheCollection.GetAllProvinceInfo(); //pvDal.GetAllProvince();
            ddl_Pr.DataSource = plist;
            ddl_Pr.DataTextField = "ProvinceName";
            ddl_Pr.DataValueField = "ID";
            ddl_Pr.DataBind();

            string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
            ExpressCarriageService ecsDal = new ExpressCarriageService();
            IList<ExpressCarriageInfo> ecList = ecsDal.GetAllExpressCarriageInfo(nick);
            Rpt_ExpressCarriage.DataSource = ecList;
            Rpt_ExpressCarriage.DataBind();
        }
    }
    protected void ddl_Pr_SelectedIndexChanged(object sender, EventArgs e)
    {
        //CityService csDal = new CityService();
        IList<CityInfo> list = CacheCollection.GetAllProvinceInfo().Where(o=>o.ID==new Guid(ddl_Pr.SelectedValue)).ToList()[0].CityList; //csDal.GetAllCity(new Guid(ddl_Pr.SelectedValue));

        list.Insert(0, new CityInfo { ID = Guid.Empty, CityName = "所有城市" });

        ddl_City.DataSource = list;
        ddl_City.DataTextField = "CityName";
        ddl_City.DataValueField = "ID";
        ddl_City.DataBind();

    }
    protected void Btn_Ok_Click(object sender, EventArgs e)
    {
        ExpressCarriageService ecDal = new ExpressCarriageService();
        ExpressCarriageInfo info = new ExpressCarriageInfo();
        info.CityId = new Guid(ddl_City.SelectedValue);
        info.ExpressId = new Guid(ddl_Express.SelectedValue);

        info.Goods = int.Parse(Rbl_Huo.SelectedValue);

        info.Nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
        try
        {
            info.Carriage = decimal.Parse(TB_Carriage.Text);
        }
        catch (Exception ex)
        {
        }

        ExpressCarriageInfo hadinfo = ecDal.GetExpressCarriageInfo(info.Nick, info.CityId, info.ExpressId);

        if (hadinfo == null)
        {
            ecDal.AddExpressCarriage(info);
        }
        else
        {
            ecDal.UpdateExpressCarriageInfo(hadinfo.ID, info.Carriage);
        }
    }

    protected string GetCity(string cid)
    {
        IList<ProvinceInfo> list = CacheCollection.GetAllProvinceInfo();

        foreach (ProvinceInfo info in list)
        {
            foreach (CityInfo cinfo in info.CityList)
            {
                if (cinfo.ID.ToString() == cid)
                {
                    return info.ProvinceName + cinfo.CityName;
                }
            }
        }

        return "未知";
    }

    protected string GetExpress(string eid)
    {
        return CacheCollection.GetAllExpressInfo().Where(o => o.ExpressId.ToString() == eid).ToList()[0].ExpressName;
    }

    public string GetGoods(string goods)
    {
        if (goods == "1")
            return "货到";
        return "款到";
    }

    protected void Btn_UpAll_Click(object sender, EventArgs e)
    {
        ExpressCarriageService ecDal = new ExpressCarriageService();
        IList<ExpressCarriageInfo> list = new List<ExpressCarriageInfo>();
        string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
        foreach (RepeaterItem item in Rpt_ExpressCarriage.Items)
        {
            TextBox tb = (TextBox)item.FindControl("Tb_Carri");
            Label lb = (Label)item.FindControl("Lbl_Id");
            Label lbc = (Label)item.FindControl("Lbl_Carri");
            ExpressCarriageInfo info = new ExpressCarriageInfo();
            info.ID = new Guid(lb.Text);
            info.Carriage = decimal.Parse(lbc.Text);
            try
            {
                info.Carriage = decimal.Parse(tb.Text);
            }
            catch (Exception ex)
            {
            }
            list.Add(info);
        }

        foreach (ExpressCarriageInfo info in list)
        {
            ecDal.UpdateExpressCarriageInfo(info.ID, info.Carriage);
        }
    }
}
