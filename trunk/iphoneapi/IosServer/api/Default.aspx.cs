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

        string[] ary = cookieStr.Split('|');

        cookie.setCookie("JSESSIONID", ary[0], 999999);

        if (ary.Length > 1)
        {
            cookie.setCookie("BIGipServerotsweb", ary[1], 999999);
            //ck = new Cookie("BIGipServerotsweb", ary[1], "/", "dynamic.12306.cn");
            //cc.Add(ck);
            //resultNew += "BIGipServerotsweb=" + ary[1];
        }

        Response.End();
    }
}