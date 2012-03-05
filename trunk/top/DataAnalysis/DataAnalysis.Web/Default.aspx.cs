using System;
using System.Web;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string nickNo = DataHelper.Encrypt(HttpUtility.UrlDecode(Request.Cookies["nick"].Value));
            VisitService vistitDal = new VisitService();
            DateTime[] darray = DataHelper.GetDateTime(DateTime.Now, 1);
            Rpt_IpPV.DataSource = vistitDal.GetIndexTotalInfoList(nickNo, darray[0], darray[1]);
            Rpt_IpPV.DataBind();

            Rpt_OnlineCustomer.DataSource = vistitDal.GetIndexOnlineCustomer(nickNo, 3, darray[0], darray[1]);
            Rpt_OnlineCustomer.DataBind();
        }
    }

}
