using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.IO;

public partial class top_market_mshop : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.Cookies["nick"] == null)
            {
                Response.Write("请登录");
                Response.End();
                return;
            }
            //解密NICK
            Rijndael_ encode = new Rijndael_("tetesoft");
            string nick = encode.Decrypt(Request.Cookies["nick"].Value);

            if (Request.Cookies["mobile"] != null && Request.Cookies["mobile"].Value == "1")
            {
                Response.Redirect("http://iphone.7fshop.com/CreateAPK.aspx?nick=" + HttpUtility.UrlEncode(nick) + "&nicksession=" + Request.Cookies["top_session"].Value + "&mobile=1");
            }
            else
            {
                RecodeLog(Request.ServerVariables["REMOTE_ADDR"] + "打开");
            }
        }
    }

    protected void CheckNull(object sender, ImageClickEventArgs e)
    {
        RecodeLog(Request.ServerVariables["REMOTE_ADDR"]);

        //Page.RegisterStartupScript("ss", "<script>open('http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-6:1;','_blank');</script>");

        Response.Redirect("want_buy.aspx");
    }

    private static void RecodeLog(string result)
    {
        string LogAddress = "D:\\点击生成手机应用记录";
        FileStream fstream;
        string dateStr = DateTime.Now.ToLongDateString().Replace("年", "").Replace("月", "").Replace("日", "");
        if (!Directory.Exists(LogAddress))
            Directory.CreateDirectory(LogAddress);
        string filepath = LogAddress + "\\" + dateStr + ".txt";

        if (File.Exists(filepath))
            fstream = new FileStream(filepath, FileMode.Append, FileAccess.Write);
        else fstream = new FileStream(filepath, FileMode.CreateNew, FileAccess.Write);

        lock (fstream)
        {
            StreamWriter swriter = new StreamWriter(fstream);
            swriter.WriteLine(result + "\t\t" + DateTime.Now);

            swriter.Close();
            fstream.Close();
        }
    }

}