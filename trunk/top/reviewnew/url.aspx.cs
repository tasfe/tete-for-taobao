using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class top_reviewnew_url : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Write(Request.QueryString.ToString() + "<br>");
        Response.Write("亲爱的叶儿随清风，您好：<br>");
        Response.Write("优惠券：14张<br>");
        Response.Write("支付宝红包：2个<br>");
        Response.Write("包邮卡：到期时间2012-12-12<br>");
    }
}