using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeteIosTrain;
using System.IO;

public partial class api_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Common.utils.NewRequest("act", Common.utils.RequestType.QueryString);
        if (act == "verify")
        {
            OutPutVerify();
        }
    }

    /// <summary>
    /// 输出验证码
    /// </summary>
    private void OutPutVerify()
    {
        Train t = new Train();
        string cookieStr = t.GetVerifyImg();

        Common.Cookie cookie = new Common.Cookie();
        cookie.setCookie("session", cookieStr, 999999);

        Response.End();
    }
}