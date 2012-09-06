using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;
using System.Text;

public partial class top_reviewnew_freesearch : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string nick = utils.NewRequest("nick", utils.RequestType.QueryString);
        string buynick = utils.NewRequest("buynick", utils.RequestType.QueryString);
        //StringBuilder builder = new StringBuilder();

        string sql = "SELECT * FROM TCS_FreeCard f INNER JOIN TCS_FreeCardAction a ON a.guid = f.cardid WHERE f.nick = '" + nick + "' AND f.buynick = '" + buynick + "'";

        DataTable dt = utils.ExecuteDataTable(sql);

        rptArticle.DataSource = dt;
        rptArticle.DataBind();
        //builder.Append(buynick + "，您目前拥有店铺【" + nick + "】的包邮卡" + dt.Rows.Count + "张<br>");
        //for (int i = 0; i < dt.Rows.Count; i++)
        //{
        //    builder.Append("<br>");
        //}
    }
}