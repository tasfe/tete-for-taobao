using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;

public partial class weibo_info : System.Web.UI.Page
{
    public string str = string.Empty;
    public string order = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Common.Cookie cookie = new Common.Cookie();
        string uid = cookie.getCookie("uid");

        string sql = "SELECT COUNT(*) FROM TopMicroBlogListen WHERE listen = '" + uid + "' AND DATEDIFF(d, adddate, GETDATE() ) = 0";
        string count = utils.ExecuteString(sql);


        str = "今天特特共帮您增加了【" + count + "】个粉丝..";



        sql = "SELECT COUNT(*) FROM TopMicroBlogAccount WHERE score > (SELECT score FROM TopMicroBlogAccount WHERE uid = '" + uid + "')";
        order = utils.ExecuteString(sql);

        order = (int.Parse(order) + 1).ToString();
    }
}