using System;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Xml;
using System.Web;
using Common;

public partial class CreateAPK : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.Cookies["nick"] == null)
            {
                return;
            }

            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            string strOutput = null;
            //进入目录
            p.StandardInput.WriteLine("d:");
            p.StandardInput.WriteLine(@"cd D:\APKTool");

            //复制APK
            //p.StandardInput.WriteLine(@"copy sourceAPK\TeceraNew.apk TeceraNew.apk /y");

            //解压APK
            p.StandardInput.WriteLine("apktool d TeceraNew.apk");
            //创建目录
            //解密NICK
            Rijndael_ encode = new Rijndael_("tetesoft");
            string nick = encode.Decrypt(Request.Cookies["nick"].Value);
            p.StandardInput.WriteLine("md " + nick);
            //复制文件到该目录
            p.StandardInput.WriteLine(@"xcopy TeceraNew\*.* " + nick + " /E");

            p.StandardInput.WriteLine("exit");
            strOutput = p.StandardOutput.ReadToEnd();
            Console.WriteLine(strOutput);
            p.WaitForExit();
            p.Close();


        }
    }

    protected void Unnamed1_Click(object sender, EventArgs e)
    {
        if ((CheckFileIsSave(Fud_logo, "jpg") || CheckFileIsSave(Fud_logo, "jpeg")) && CheckFileIsSave(Fud_load, "png") && CheckFileIsSave(Fud_head, "png"))
        {
            //解密NICK
            Rijndael_ encode = new Rijndael_("tetesoft");
            string dir = encode.Decrypt(Request.Cookies["nick"].Value);
            //string dir = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);

            try
            {
                Fud_logo.PostedFile.SaveAs(@"D:\APKTool\" + dir + @"\res\drawable-hdpi\img_top.jpg");
                Fud_head.PostedFile.SaveAs(@"D:\APKTool\" + dir + @"\res\drawable-hdpi\icon.png");

                Fud_load.PostedFile.SaveAs(@"D:\APKTool" + dir + @"\res\drawable-hdpi\img_first.png");
            }
            catch (Exception ex)
            {
                Page.RegisterStartupScript("error", "<script>alert('图片上传失败,请重试!" + ex.Message.ToString() + "');</script>");
                return;
            }
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(@"D:\APKTool\" + dir + @"\res\values\strings.xml");

            XmlNodeList xnl = xmlDoc.SelectSingleNode("resources").ChildNodes;

            foreach (XmlNode xn in xnl)
            {
                XmlElement xe = (XmlElement)xn;
                if (xe.GetAttribute("name") == "app_name")
                {
                    xe.InnerText = Tb_AppName.Text;
                    break;
                }
            }

            xmlDoc.Save(@"D:\APKTool\" + dir + @"\res\values\strings.xml");

            Btn_Create.Visible = true;
        }
        else
        {
            Page.RegisterStartupScript("error", "<script>alert('图片格式不对');</script>");
        }
    }

    private static bool CheckFileIsSave(FileUpload FileUpload1,string type)
    {
        if (FileUpload1.PostedFile.ContentType.IndexOf(type) != -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    protected void Unnamed2_Click(object sender, EventArgs e)
    {
        //解密NICK
        Rijndael_ encode = new Rijndael_("tetesoft");
        string dir = encode.Decrypt(Request.Cookies["nick"].Value);
        //string dir = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
        Process p = new Process();
        p.StartInfo.FileName = "cmd.exe";
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardInput = true;
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.RedirectStandardError = true;
        p.StartInfo.CreateNoWindow = true;
        p.Start();
        //string strOutput = null;
        //进入目录
        p.StandardInput.WriteLine("d:");
        p.StandardInput.WriteLine(@"cd D:\APKTool");

        //替换相关文件
        p.StandardInput.WriteLine(@"copy " + dir + @"\res\drawable-hdpi\icon.png " + dir + @"\res\drawable-ldpi\icon.png /y");

        p.StandardInput.WriteLine(@"copy " + dir + @"\res\drawable-hdpi\icon.png " + dir + @"\res\drawable-mdpi\icon.png /y");

        //重新生成APK
        p.StandardInput.WriteLine("apktool b " + dir);
        //这边复制成nick加apk文件
        p.StandardInput.WriteLine(@"copy " + dir + @"\dist\TeceraNew.apk userAPK\" + dir + ".apk /y");

        p.StandardInput.WriteLine("exit");
        //strOutput = p.StandardOutput.ReadToEnd();
        //Console.WriteLine(strOutput);
        //p.WaitForExit();
        p.Close();
    }
}
