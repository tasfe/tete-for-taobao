using System;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            VisitService vistitDal = new VisitService();
            DateTime[] darray = DataHelper.GetDateTime(DateTime.Now, 1);
            Rpt_IpPV.DataSource = vistitDal.GetIndexTotalInfoList(DataHelper.Encrypt(Session["nick"].ToString()), darray[0], darray[1]);
            Rpt_IpPV.DataBind();

            Rpt_OnlineCustomer.DataSource = vistitDal.GetIndexOnlineCustomer(DataHelper.Encrypt(Session["nick"].ToString()), 3, darray[0], darray[1]);
            Rpt_OnlineCustomer.DataBind();
        }
    }

}
