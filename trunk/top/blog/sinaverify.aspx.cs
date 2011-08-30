using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using Common;
using System.Drawing.Imaging;

public partial class topTest_market_sinaverify : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string url = utils.NewRequest("u", utils.RequestType.QueryString);

        if(url == "")
            url = "http://www.7fshop.com/top/images/logo.jpg";
        string strHtml = string.Empty;

        WebRequest request = WebRequest.Create(url);
        WebResponse response = request.GetResponse();
        Stream reader = response.GetResponseStream();

        string time = DateTime.Now.ToString("yyyy-MM-dd");

        //创建文件夹
        if(!Directory.Exists(Server.MapPath("blogimages/" + time + "/")))
        {
            Directory.CreateDirectory(Server.MapPath("blogimages/" + time + "/"));
        }

        string fileName = Server.MapPath("blogimages/" + time + "/" + MD5(url) + ".jpg");

        if (!File.Exists(fileName))
        {
            FileStream writer = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
            byte[] buff = new byte[512];
            int c = 0; //实际读取的字节数
            while ((c = reader.Read(buff, 0, buff.Length)) > 0)
            {
                writer.Write(buff, 0, c);
            }
            writer.Close();
        }

        Response.ClearContent();
        Response.ContentType = "image/jpeg";
        Response.BinaryWrite(File.ReadAllBytes(fileName));
        Response.End();
    }

    public static string MD5(string str)
    {
        return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
    }
}
