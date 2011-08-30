using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;

public partial class top_microblog_success : System.Web.UI.Page
{
    public string blogUrl = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        blogUrl = utils.NewRequest("adr", utils.RequestType.QueryString);

        BindData();
    }

    /// <summary>
    /// 数据绑定
    /// </summary>
    private void BindData()
    {

    }
}