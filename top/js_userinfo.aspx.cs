﻿using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Common;

public partial class top_js_userinfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Cookie cookie = new Cookie();
        string taobaoNick = cookie.getCookie("nick");
        string session = cookie.getCookie("top_session");
        string oldTaobaoNick = taobaoNick;
        string iscrm = cookie.getCookie("iscrm");
        string istongji = cookie.getCookie("istongji");
        string date = DateTime.Now.ToString();

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        Response.AddHeader("P3P", "CP=CURa ADMa DEVa PSAo PSDo OUR BUS UNI PUR INT DEM STA PRE COM NAV OTC NOI DSP COR");
        Response.Write("document.write('您好，" + taobaoNick + "！ <a href=logout.aspx>退出</a>');");
        if (iscrm == "1")
        {
            Response.Write("document.write('<img src=\"http://haoping.7fshop.com/top/crm/setcookie.aspx?nick=" + HttpUtility.UrlEncode(oldTaobaoNick) + "&session=" + session + "&iscrm=1&d=" + date + "\" width=0 height=0>');");
        }
        else
        {
            Response.Write("document.write('<img src=\"http://haoping.7fshop.com/top/crm/setcookie.aspx?nick=" + HttpUtility.UrlEncode(oldTaobaoNick) + "&session=" + session + "&d=" + date + "\" width=0 height=0>');");
        }

        if (istongji == "1")
        {
            Response.Write("document.write('<img src=\"http://ding.7fshop.com/getnick.ashx?nick=" + HttpUtility.UrlEncode(taobaoNick) + "&session=" + session + "&istongji=1&d=" + date + "\" width=0 height=0>');");
        }
        else
        {
            Response.Write("document.write('<img src=\"http://ding.7fshop.com/getnick.ashx?nick=" + HttpUtility.UrlEncode(taobaoNick) + "&session=" + session + "&d=" + date + "\" width=0 height=0>');");
        }

        //好评登录
        Response.Write("document.write('<img src=\"http://haoping.7fshop.com/top/reviewnew/setcookie.aspx?nick=" + HttpUtility.UrlEncode(taobaoNick) + "&d=" + date + "\" width=0 height=0>');");

        //团购登录
        Response.Write("document.write('<img src=\"http://groupbuy.7fshop.com/top/groupbuy/setcookie.aspx?nick=" + HttpUtility.UrlEncode(taobaoNick) + "&d=" + date + "\" width=0 height=0>');");
    }
}
