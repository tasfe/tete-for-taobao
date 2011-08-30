using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Text.RegularExpressions;

public partial class testflash_callback : System.Web.UI.Page
{
    public string url = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        //string top_parameters = System.Text.Encoding.GetEncoding("gb2312").GetString(Convert.FromBase64String(utils.NewRequest("top_parameters", utils.RequestType.QueryString)));

        //Regex reg = new Regex(@"visitor_nick=([\S]*)");
        //string visitor_nick = reg.Match(top_parameters).Groups[1].ToString();

        //url = "viewer_nick=" + visitor_nick;
        //url += "&seller_nick=";
        url = HttpUtility.UrlEncode(utils.NewRequest("shop_id", utils.RequestType.QueryString));
    }
}