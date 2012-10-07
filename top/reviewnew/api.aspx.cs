using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;
using System.IO;

public partial class top_reviewnew_api : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string result = string.Empty;
        string nick = utils.NewRequest("nick", utils.RequestType.QueryString);
        nick = "迅泽点卡专营店";
        string orderid = utils.NewRequest("orderid", utils.RequestType.QueryString);
        string sql = "SELECT orderarea FROM TCS_Trade WHERE orderid = '" + orderid + "' AND nick = '" + nick + "'";
        //Response.Write(sql);
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            result = dt.Rows[0][0].ToString();
        }
        else
        {
            result = "null";
        }
        File.WriteAllText(Server.MapPath(nick + ".txt"), Request.Url.ToString());

        Response.Write(result);
    }
}