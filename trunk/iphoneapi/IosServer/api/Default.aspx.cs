using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeteIosTrain;
using System.IO;
using System.Text.RegularExpressions;

public partial class api_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Common.utils.NewRequest("act", Common.utils.RequestType.QueryString);
        if (act == "verify")
        {
            OutPutVerify();
        }

        if (act == "login")
        {
            LoginPost();
        }
    }

    /// <summary>
    /// 登录请求
    /// </summary>
    private void LoginPost()
    {
        string uid = Common.utils.NewRequest("uid", Common.utils.RequestType.Form);
        string pass = Common.utils.NewRequest("pass", Common.utils.RequestType.Form);
        string verify = Common.utils.NewRequest("verify", Common.utils.RequestType.Form);
        string session = Common.utils.NewRequest("session", Common.utils.RequestType.Form);

        //JSESSIONID=2D196E71DE9C9DDF0BE726984B66C03C;
        string str1 = new Regex(@"JSESSIONID=([^;]*);", RegexOptions.IgnoreCase).Match(session).Groups[1].ToString();
        string str2 = new Regex(@"BIGipServerotsweb=([^;]*);", RegexOptions.IgnoreCase).Match(session).Groups[1].ToString();

        Train send = new Train();
        string result = send.SendLoginRequest(uid, pass, verify, str1 + "|" + str2);

        if (result.IndexOf("登录人数") != -1)
        {
            Response.Write("<result>busy</result>");
        }
        else if (result.IndexOf("请输入正确的验证码") != -1)
        {
            Response.Write("<result>请输入正确的验证码</result>");
        }
        else if (result.IndexOf("邮箱不存在") != -1)
        {
            Response.Write("<result>邮箱不存在</result>");
        }
        else if (result.IndexOf("密码输入错误") != -1)
        {
            Response.Write("<result>密码输入错误</result>");
        }
        else
        {
            if (result.IndexOf("我的12306") == -1)
            {
                Response.Write("<result>密码输入错误超过4次，用户将锁定20分钟，请稍后再试！</result>");
            }
            else
            {
                Response.Write("<result>ok</result>");
            }
        }
        Response.End();
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