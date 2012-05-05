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

public partial class UpdateCarriage : System.Web.UI.Page
{
    ExpressService esDal = new ExpressService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ProvinceService pvDal = new ProvinceService();

            IList<ExpressInfo> list = esDal.GetAllExpressInfo("");
            ddl_Express.DataSource = list;
            ddl_Express.DataTextField = "ExpressName";
            ddl_Express.DataValueField = "ExpressId";
            ddl_Express.DataBind();

            IList<ProvinceInfo> plist = pvDal.GetAllProvince();
            ddl_Pr.DataSource = plist;
            ddl_Pr.DataTextField = "ProvinceName";
            ddl_Pr.DataValueField = "ID";
            ddl_Pr.DataBind();
        }
    }
    protected void ddl_Pr_SelectedIndexChanged(object sender, EventArgs e)
    {
        CityService csDal = new CityService();
        IList<CityInfo> list = csDal.GetAllCity(new Guid(ddl_Pr.SelectedValue));

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
}
