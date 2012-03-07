using System;
using System.Web;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.Cookies["nick1"] == null)
                System.Threading.Thread.Sleep(1000 * 6);
            string nickNo = DataHelper.Encrypt(HttpUtility.UrlDecode(Request.Cookies["nick"].Value));
            VisitService vistitDal = new VisitService();
            DateTime[] darray = DataHelper.GetDateTime(DateTime.Now, 1);
            Rpt_IpPV.DataSource = vistitDal.GetIndexTotalInfoList(nickNo, darray[0], darray[1]);
            Rpt_IpPV.DataBind();

            Rpt_OnlineCustomer.DataSource = vistitDal.GetIndexOnlineCustomer(nickNo, 3, darray[0], darray[1]);
            Rpt_OnlineCustomer.DataBind();
        }
    }

    protected void Btn_AddCId_Click(object sender, EventArgs e)
    {
        if (TaoBaoAPI.AddCID(HttpUtility.UrlDecode(Request.Cookies["nick"].Value), Request.Cookies["nicksession"].Value))
        {
            Page.RegisterStartupScript("恭喜", "<script>alert('添加成功!');</script>");
        }
        else
        {
            Page.RegisterStartupScript("抱歉", "<script>alert('添加失败!');</script>");
        }
    }
}
