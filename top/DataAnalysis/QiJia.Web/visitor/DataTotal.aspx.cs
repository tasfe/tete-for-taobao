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

public partial class visitor_DataTotal : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string nickNo = Encrypt(HttpUtility.UrlDecode(Request.Cookies["nick"].Value));
            DateTime[] darray = GetDateTime(DateTime.Now, 1);
            VisitService vistitDal = new VisitService();
            Rpt_OnlineCustomer.DataSource = vistitDal.GetIndexOnlineCustomer(nickNo,10, darray[0], darray[1]);
            Rpt_OnlineCustomer.DataBind();
        }
    }
}
