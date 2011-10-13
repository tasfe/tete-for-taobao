using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using Common;
using QWeiboSDK;
using System.Text;
using System.Data;

public partial class weibo_js_userscore : System.Web.UI.Page
{
    private string appKey = string.Empty;
    private string appSecret = string.Empty;
    private string tokenKey = string.Empty;
    private string tokenSecret = string.Empty;
    private string uid = string.Empty;
    public string str = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        appKey = "d3225497956249cbb13a7cb7375d62bd";
        appSecret = "6cf7a3274cb676328e77dff3e203061d";

        Common.Cookie cookie = new Common.Cookie();
        tokenKey = cookie.getCookie("tokenKey");
        tokenSecret = cookie.getCookie("tokenSecret");
        uid = cookie.getCookie("uid");

        string score = utils.ExecuteString("SELECT score FROM TopMicroBlogAccoount WHERE uid = '" + uid + "'");
        Response.Write("document.write('您目前的剩余积分是【"+score+"】');");
    }
}