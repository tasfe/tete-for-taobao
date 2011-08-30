using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Common;

public partial class top_market_resultshow : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BindData();
    }

    private void BindData()
    {
        //获取用户信息
        Cookie cookie = new Cookie();
        string taobaoNick = cookie.getCookie("nick");

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        //判断是否删除
        string id = utils.NewRequest("id", utils.RequestType.QueryString);

        if (id != "" && !utils.IsInt32(id))
        {
            Response.Write("非法参数");
            Response.End();
            return;
        }

        DataTable dt = utils.ExecuteDataTable("SELECT COUNT(url) AS viewcount,url FROM TopIdeaLog WHERE ideaid = '" + id + "' GROUP BY url");

        DataView dv = dt.DefaultView;
        dv.Sort = "viewcount DESC";

        rptLogList.DataSource = dv;
        rptLogList.DataBind();
    }
}
