using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;

public partial class taobaotetesoft_caiji_show : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string id = utils.NewRequest("id", utils.RequestType.QueryString);
        if (id == "mm_10128191_208985_7882407")
        {
            Response.Redirect("http://www.fenseshenghuo.com/bkqq/adUpFile/ad/0-2.jpg");
        }
        else
        {
            Response.Redirect("http://gg.7fshop.com/zhaozu.jpg");
        }
    }
}