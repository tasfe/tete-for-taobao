using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;

public partial class top_market_resulthownew : System.Web.UI.Page
{
    public string id = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        //判断是否删除
        id = utils.NewRequest("id", utils.RequestType.QueryString);

        if (id != "" && !utils.IsInt32(id))
        {
            Response.Write("非法参数");
            Response.End();
            return;
        }
    }
}