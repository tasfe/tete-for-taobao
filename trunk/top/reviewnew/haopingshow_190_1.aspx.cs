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
    public string title = string.Empty;
    public string time = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        nick = utils.NewRequest("nick", utils.RequestType.QueryString);
        buynick = utils.NewRequest("buynick", utils.RequestType.QueryString);

        string sql = "SELECT TOP 20 * FROM TCS_TradeRate WHERE nick = '" + nick + "' AND isshow = 1 ORDER BY showindex,reviewdate DESC";
        DataTable dt = utils.ExecuteDataTable(sql);

        rptTradeRate.DataSource = dt;
        rptTradeRate.DataBind();

        sql = "SELECT * FROM TCS_ShopConfig WHERE nick = '" + nick + "'";
        dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            title = dt.Rows[0]["xiuxiutitle"].ToString();
            time = dt.Rows[0]["xiuxiutime"].ToString();
        }

        if (title.Length == 0)
        {
            title = "好评有礼";
        }
    }

    public static string hidden(string str)
    {
        string oldstr = str;
        if (str.Length > 2)
        {
            int len = str.Length;
            str = str.Substring(0, 1);
            for (int i = 0; i < (len - 2); i++)
            {
                str += "*";
            }
            str += oldstr.Substring(len - 1, 1);
        }

        return str;
    }
}