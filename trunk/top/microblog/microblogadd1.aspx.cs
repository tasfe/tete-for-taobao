using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;

public partial class top_microblog_microblogadd1 : System.Web.UI.Page
{
    public string topic = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        topic = utils.NewRequest("t", utils.RequestType.Form);
        topic = HttpUtility.UrlDecode(topic);
    }
}