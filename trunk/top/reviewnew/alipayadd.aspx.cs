using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text.RegularExpressions;

public partial class top_reviewnew_alipayadd : System.Web.UI.Page
{
    public string alipay = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        alipay = "";
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        //先判断支付宝红包格式是否合法
        string guid = Guid.NewGuid().ToString();

        if (fuAlipay.PostedFile.ContentType != "text/plain" && fuAlipay.PostedFile.FileName.IndexOf(".txt") == -1)
        {
            Response.Write("<script>alert('您上传的文件格式不正确！');history.go(-1);</script>");
            Response.End();
            return;
        }

        string filename = Server.MapPath("alipay/" + guid + ".txt");
        fuAlipay.PostedFile.SaveAs(filename);

        string content = File.ReadAllText(filename);
        if (content.IndexOf("cardno          ,password") == -1)
        {
            Response.Write("<script>alert('您上传的文件格式不正确！');history.go(-1);</script>");
            Response.End();
            return;
        }

        string[] arr = Regex.Split(content, "\r\n");
        if (arr.Length == 2)
        {
            Response.Write("<script>alert('您上传的文件格式不正确！');history.go(-1);</script>");
            Response.End();
            return;
        }

        string sql = "INSERT INTO ";
    }
}