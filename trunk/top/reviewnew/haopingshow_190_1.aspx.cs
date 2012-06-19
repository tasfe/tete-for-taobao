using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Text.RegularExpressions;
using System.Data;


public partial class top_reviewnew_haopingshow_190_1 : System.Web.UI.Page
{
    public string nick = string.Empty;
    public string buynick = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        nick = utils.NewRequest("nick", utils.RequestType.QueryString);
        buynick = utils.NewRequest("buynick", utils.RequestType.QueryString);

        string sql = "SELECT TOP 20 * FROM TCS_TradeRate WHERE nick = '" + nick + "' AND isshow = 1 ORDER BY showindex,reviewdate DESC";
        DataTable dt = utils.ExecuteDataTable(sql);

        rptTradeRate.DataSource = dt;
        rptTradeRate.DataBind();
    }

    public static string hidden(string str)
    {
        if (str.Length > 2)
        {
            int len = str.Length;
            str = str.Substring(0, 1);
            for (int i = 0; i < (len - 2); i++)
            {
                str += "*";
            }
            str += str.Substring(len - 2, 1);
        }

        return str;
    }
}