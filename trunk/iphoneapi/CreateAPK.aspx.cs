using System;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Xml;
using System.Web;
using Common;
using System.IO;
using ThoughtWorks.QRCode.Codec;
using System.Drawing;

public partial class CreateAPK : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.Cookies["nick"] == null)
            {
                return;
            }
            
            string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
            if (!Directory.Exists(@"D:\APKTool\" + nick))
            {
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
                //p.StandardInput.WriteLine("apktool d TeceraNew.apk");
                //创建目录
                p.StandardInput.WriteLine("md " + nick);
                //复制文件到该目录
                p.StandardInput.WriteLine(@"xcopy TeceraNew\*.* " + nick + " /E /y");

                p.StandardInput.WriteLine("exit");
                strOutput = p.StandardOutput.ReadToEnd();
                //Console.WriteLine(strOutput);
                p.WaitForExit();
                p.Close();

            }
        }
    }

    protected void Unnamed1_Click(object sender, EventArgs e)
    {
        if ((CheckFileIsSave(Fud_logo, "jpg") || CheckFileIsSave(Fud_logo, "jpeg")) && CheckFileIsSave(Fud_load, "png") && CheckFileIsSave(Fud_head, "png"))
        {
            string dir = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);

            try
            {
                Fud_logo.PostedFile.SaveAs(@"D:\APKTool\" + dir + @"\res\drawable-hdpi\img_top.jpg");
                Fud_head.PostedFile.SaveAs(@"D:\APKTool\" + dir + @"\res\drawable-hdpi\icon.png");

                Fud_load.PostedFile.SaveAs(@"D:\APKTool\" + dir + @"\res\drawable-hdpi\img_first.png");
            }
            catch (Exception ex)
            {
                Page.RegisterStartupScript("error", "<script>alert('" + dir + "图片上传失败,请重试!" + ex.Message.ToString() + "');</script>");
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
                }

                if (xe.GetAttribute("name") == "user_nick")
                {
                    xe.InnerText = dir;
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

    private void CreateUserAPK(string dir)
    {
        //string dir = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
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

        //替换相关文件
        p.StandardInput.WriteLine(@"copy " + dir + @"\res\drawable-hdpi\icon.png " + dir + @"\res\drawable-ldpi\icon.png /y");

        p.StandardInput.WriteLine(@"copy " + dir + @"\res\drawable-hdpi\icon.png " + dir + @"\res\drawable-mdpi\icon.png /y");

        //build文件夹
        p.StandardInput.WriteLine(@"copy " + dir + @"\res\drawable-hdpi\icon.png " + dir + @"\build\apk\res\drawable-hdpi\icon.png /y");
        p.StandardInput.WriteLine(@"copy " + dir + @"\res\drawable-hdpi\img_top.jpg " + dir + @"\build\apk\res\drawable-hdpi\img_top.jpg /y");
        p.StandardInput.WriteLine(@"copy " + dir + @"\res\drawable-hdpi\img_first.png " + dir + @"\build\apk\res\drawable-hdpi\img_first.png /y");

        p.StandardInput.WriteLine(@"copy " + dir + @"\res\drawable-hdpi\icon.png " + dir + @"\build\apk\res\drawable-ldpi\icon.png /y");

        p.StandardInput.WriteLine(@"copy " + dir + @"\res\drawable-hdpi\icon.png " + dir + @"\build\apk\res\drawable-mdpi\icon.png /y");

        //重新生成APK
        p.StandardInput.WriteLine("apktool b " + dir);

        p.StandardInput.WriteLine("cd " + dir + @"\dist");
        //添加签名
        p.StandardInput.WriteLine("ren TeceraNew.apk TeceraNew.zip");
        p.StandardInput.WriteLine("Sign.bat");
        p.StandardInput.WriteLine("cd..");
        p.StandardInput.WriteLine("exit");
        strOutput = p.StandardOutput.ReadToEnd();
        //Console.WriteLine(strOutput);
        p.WaitForExit();
        p.Close();
    }

    protected void Btn_Create_Click(object sender, EventArgs e)
    {
        string dir = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
        if (!File.Exists(@"D:\APKTool\" + dir + ".bat"))
        {
            FileStream fs = new FileStream(@"D:\APKTool\" + dir + ".bat", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
            sw.WriteLine("@ECHO OFF");
            sw.WriteLine("apktool b " + dir);
            sw.WriteLine("Echo create Complete");
            sw.Close();
            fs.Close();
        }
        CreateUserAPK(dir);
        Lbl_Suc.Visible = true;
        Btn_Sign.Visible = true;

    }

    protected void Btn_Sign_Click(object sender, EventArgs e)
    {
        //这边复制成nick加apk文件
        string dir = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
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
        p.StandardInput.WriteLine("del " + dir + @"\dist\TeceraNew.zip");
        p.StandardInput.WriteLine("del " + @"userAPK\" + Request.Cookies["nick"].Value.Replace("=", ".") + ".apk");
        p.StandardInput.WriteLine("copy " + dir + @"\dist\update_signed.zip userAPK\" + Request.Cookies["nick"].Value.Replace("=", ".") + ".apk /y");
        //用完删除
        p.StandardInput.WriteLine("del " + dir + @"\dist\update_signed.zip");
        //p.StandardInput.WriteLine("cd..");
        p.StandardInput.WriteLine("exit");
        strOutput = p.StandardOutput.ReadToEnd();
        //Console.WriteLine(strOutput);
        p.WaitForExit();
        p.Close();

        string fpath = Server.MapPath("~/apkimg") + "/" + Request.Cookies["nick"].Value + ".jpg";
        if (!File.Exists(fpath))
        {
            System.Drawing.Image img = GCode("http://www.7fshop.com/userAPK/" + Request.Cookies["nick"].Value.Replace("=", ".") + ".apk");
            img.Save(fpath);
        }

        Img_Apk.ImageUrl = "http://iphone.tetesoft.com/apkimg/" + Request.Cookies["nick"].Value + ".jpg";
        Img_Apk.Visible = true;
        Btn_AddCa.Visible = true;
    }

    private System.Drawing.Image GCode(string data)
    {
        QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();

        qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;

        qrCodeEncoder.QRCodeScale = 4;
        qrCodeEncoder.QRCodeVersion = 8;

        qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
        Bitmap pbImg = qrCodeEncoder.Encode(data);
        int width = pbImg.Width / 10;
        int dwidth = width * 2;
        Bitmap bmp = new Bitmap(pbImg.Width + dwidth, pbImg.Height + dwidth);
        Graphics g = Graphics.FromImage(bmp);
        Color c = System.Drawing.Color.White;
        g.FillRectangle(new SolidBrush(c), 0, 0, pbImg.Width + dwidth, pbImg.Height + dwidth);
        g.DrawImage(pbImg, width, width);
        g.Dispose();

        return bmp;
    }


    protected void Btn_AddCa_Click(object sender, EventArgs e)
    {

        Rijndael_ encode = new Rijndael_("tetesoft");
        string nick = encode.Decrypt(Request.Cookies["nick"].Value);
        if(true)
        //if (TaoBaoAPI.AddCID(nick, Request.Cookies["nicksession"].Value))
        {
            Page.RegisterStartupScript("恭喜", "<script>alert('添加成功!');</script>");

            Lbl_Over.Visible = true;
        }
        else
        {
            Page.RegisterStartupScript("抱歉", "<script>alert('添加失败!');</script>");
        }
    }
}
