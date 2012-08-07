using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //if (Request.Cookies["nick"] == null)
            //{
            //    Response.Write("请重新登录");
            //    Response.End();
            //    return;
            //}
            //Lbl_UserInfo.Text = "用 户 名：" + HttpUtility.UrlDecode(Request.Cookies["nick"].Value) + "  过期时间：" + DateTime.Now.ToString("yyyy-MM-dd") + "  点击这里续费";
        }
    }
}
