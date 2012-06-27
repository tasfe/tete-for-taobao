using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;

public partial class taobaotetesoft_caiji_redirect : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string id = utils.NewRequest("id", utils.RequestType.QueryString);
        string url = "http://fuwu.taobao.com/ser/detail.htm?service_id=764";

        if (id == "mm_10128191_208985_7882407")
        {
            url = "http://bang.7fshop.com/showgoods.aspx?adsid=F59935B3-4136-4A36-8E02-C9A01168512E";
        }
        else
        {
        }

        Response.Redirect(url);
    }
}