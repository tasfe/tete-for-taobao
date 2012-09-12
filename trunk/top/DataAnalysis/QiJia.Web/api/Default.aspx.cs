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
using Qijia.PCI;

public partial class api_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //string data = Request.QueryString["data"];

            //Request.Files["image"].SaveAs(Server.MapPath("~/temp")+"\\s.jpg");

            string data = Request.QueryString["data"].Replace("[jia]", "+");

            MethodPCI pci = new MethodPCI();

            string obj = pci.GetYouWant(data).ToString();

            Response.Write(obj);
            Response.End();
        }
    }

    private static void GetData(string data)
    { 

    }
}
