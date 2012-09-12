using System;
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

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Qijia.PCI.MethodPCI pci = new Qijia.PCI.MethodPCI();
            string data = "method=UpdateGoods&goodsId=11&tempType=1&datatype=json";

            Qijia.PCI.PasswordParam pp = new Qijia.PCI.PasswordParam();
            string sign = pp.Encrypt3DES(data);

            string realdata = pp.Encrypt3DES(data + "&sign=" + sign);
            //data = data.Replace("+", "[jia]");

            string s = pci.GetYouWant(realdata).ToString();
            //Response.ContentType = "text/xml";
            Response.Write(s);
            Response.End();
        }
    }
}
