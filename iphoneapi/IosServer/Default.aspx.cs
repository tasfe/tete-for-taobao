using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeteIosTrain;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }


    protected void Button1_Click(object sender, EventArgs e)
    {
        Common.Cookie cookie = new Common.Cookie();
        string session = cookie.getCookie("session");

        Train send = new Train();
        string result = send.SendLoginRequest(TextBox1.Text, TextBox2.Text, TextBox3.Text, session);

        Response.Write(result);
        Response.End();
    }
}