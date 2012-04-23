﻿using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// Summary description for BasePage
/// </summary>
public class BasePage : System.Web.UI.Page
{
    protected override void InitializeCulture()
    {
        string msg = "尊敬的用户您好，非常抱歉的告诉您，您尚未订购该功能，如需继续使用请购买该服务:<br><br>58元/月  <a href=' http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-8:1;' target='_blank'>立即购买</a><br><br>158元/季  <a href=' http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-8:3;' target='_blank'>立即购买</a><br><br>298元/半年 <a href=' http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-8:6;' target='_blank'>立即购买</a><br><br>568元/年 <a href=' http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-8:12;' target='_blank'>立即购买</a><br>";
        if (Request.Cookies["istongji"] == null || Request.Cookies["istongji"].Value != "1")
            Response.Redirect("buy.aspx?msg=" + msg);
        else
        {
            if (Request.Cookies["nick"] == null || string.IsNullOrEmpty(Request.Cookies["nick"].Value))
                Response.Redirect("http://fuwu.taobao.com/serv/my_service.htm");
            else
            {
            }
        }
        //else
        //{
        //    Session["nick"] = Request.Cookies["nick"].Value;
        //    Session["session"] = Request.Cookies["session"].Value;
        //}
    }
}
