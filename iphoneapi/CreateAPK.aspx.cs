using System;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Xml;
using System.Web;
using Common;
using System.IO;
using ThoughtWorks.QRCode.Codec;
using System.Drawing;
using System.Security.Cryptography;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Linq;

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
                    xe.InnerText = Encrypt(Request.QueryString["nick"]);
                }
            }

            xmlDoc.Save(@"D:\APKTool\" + dir + @"\res\values\strings.xml");

            Btn_Create.Visible = true;

        }
        else
        {
            Page.RegisterStartupScript("error", "<script>alert('图片格式不对,如果您需要我们为您制作图片,请联系我们客服');</script>");
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
        Page.RegisterStartupScript("恭喜", "<script>alert('生成成功!');</script>");
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
        p.StandardInput.WriteLine("copy " + dir + @"\dist\update_signed.zip userAPK\" + dir + ".apk /y");
        //用完删除
        p.StandardInput.WriteLine("del " + dir + @"\dist\update_signed.zip");
        //p.StandardInput.WriteLine("cd..");
        p.StandardInput.WriteLine("exit");
        strOutput = p.StandardOutput.ReadToEnd();
        //Console.WriteLine(strOutput);
        p.WaitForExit();
        p.Close();

        string fpath = Server.MapPath("~/apkimg") + "/" + dir + ".jpg";
        if (!File.Exists(fpath))
        {
            System.Drawing.Image img = GCode("http://www.7fshop.com/userAPK/" + Request.Cookies["nick"].Value + ".apk");
            img.Save(fpath);
            MakeThumbnail(Server.MapPath("~/apkimg") + "/" + dir + ".jpg", Server.MapPath("~/apkimg") + "/" + dir + "_s.jpg", 160, 160, "Cut");
        }

        Img_Apk.ImageUrl = "http://iphone.7fshop.com/apkimg/" + dir + "_s.jpg";
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
        string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
        TeteShopInfo info = new TeteShopService().GetShopInfo(Encrypt(nick));
        if (info == null)
        {
            Page.RegisterStartupScript("抱歉", "<script>alert('您还未购买!');</script>");
            return;
        }

        if (TaoBaoAPI.AddCID(nick, Request.Cookies["nicksession"].Value, info.Appkey, info.Appsecret, "http://iphone.7fshop.com/apkimg/" + nick + "_s.jpg"))
        {
            Page.RegisterStartupScript("恭喜", "<script>alert('添加成功!');</script>");

            Lbl_Over.Visible = true;
        }
        else
        {
            Page.RegisterStartupScript("抱歉", "<script>alert('添加失败!');</script>");
        }
    }

    ///<summary>
    /// 生成缩略图
    /// </summary>
    /// <param name="originalImagePath">源图路径（物理路径）</param>
    /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
    /// <param name="width">缩略图宽度</param>
    /// <param name="height">缩略图高度</param>
    /// <param name="mode">生成缩略图的方式</param>    
    public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
    {
        System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);

        int towidth = width;
        int toheight = height;

        int x = 0;
        int y = 0;
        int ow = originalImage.Width;
        int oh = originalImage.Height;

        switch (mode)
        {
            case "HW"://指定高宽缩放（可能变形）                
                break;
            case "W"://指定宽，高按比例                    
                toheight = originalImage.Height * width / originalImage.Width;
                break;
            case "H"://指定高，宽按比例
                towidth = originalImage.Width * height / originalImage.Height;
                break;
            case "Cut"://指定高宽裁减（不变形）  

                //if(originalImage.Width < width && originalImage.Height < height)
                //如果宽高都不够则把图片外部加空白
                if (originalImage.Width < width && originalImage.Height < height)
                {
                    x = (towidth - ow) / 2;
                    y = (toheight - oh) / 2;
                }
                else
                {
                    //如果是上下加空白
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height * towidth / originalImage.Width;
                        ow = towidth;
                        x = 0;
                        y = (toheight - oh) / 2;
                    }
                    //如果是左右加空白
                    else if ((double)originalImage.Width / (double)originalImage.Height < (double)towidth / (double)toheight)
                    {
                        ow = originalImage.Width * toheight / originalImage.Height;
                        oh = toheight;
                        x = (towidth - ow) / 2;
                        y = 0;
                    }
                    //如果只是缩放
                    else
                    {
                        x = 0;
                        y = 0;
                        ow = towidth;
                        oh = toheight;
                    }
                }
                break;
            default:
                break;
        }

        //新建一个bmp图片
        System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

        //新建一个画板
        Graphics g = System.Drawing.Graphics.FromImage(bitmap);

        //设置高质量插值法
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

        //设置高质量,低速度呈现平滑程度
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

        //清空画布并以透明背景色填充
        g.Clear(Color.White);

        //在指定位置并且按指定大小绘制原图片的指定部分
        g.DrawImage(originalImage, new Rectangle(x, y, ow, oh), new Rectangle(0, 0, originalImage.Width, originalImage.Height), GraphicsUnit.Pixel);

        //Bitmap newmap = ImgSharpChange(bitmap);


        ImageCodecInfo ici;
        System.Drawing.Imaging.Encoder enc;
        EncoderParameter ep;
        EncoderParameters epa;

        //   Initialize   the   necessary   objects   
        ici = GetEncoderInfo("image/jpeg");
        enc = System.Drawing.Imaging.Encoder.Quality;//设置保存质量   
        epa = new EncoderParameters(1);
        //   Set   the   compression   level   
        ep = new EncoderParameter(enc, 100L);//质量等级为25%   
        epa.Param[0] = ep;

        try
        {
            bitmap.Save(thumbnailPath, ici, epa);
        }
        catch (System.Exception e)
        {
            //ShowErrorPage(e.Message.ToString());
        }
        finally
        {
            originalImage.Dispose();
            bitmap.Dispose();
            g.Dispose();
            GC.Collect();
        }
    }

    private static ImageCodecInfo GetEncoderInfo(String mimeType)
    {
        int j;
        ImageCodecInfo[] encoders;
        encoders = ImageCodecInfo.GetImageEncoders();
        for (j = 0; j < encoders.Length; ++j)
            if (encoders[j].MimeType == mimeType)
                return encoders[j];
        return null;
    }

    private void UpdateGoods()
    {
        string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
        string session = Request.Cookies["nicksession"].Value;
        IList<TeteShopInfo> list = CacheCollection.GetNickSessionList().Where(o => o.Short == nick && o.Session == session).ToList();

        TeteShopInfo info = null;
        if (list.Count > 0)
        {
            info = list[0];
        }
        if (info == null)
        {
            Page.RegisterStartupScript("错误", "<script>alert('您的身份不合法，请确定您已购买!');</script>");
            return;
        }
        TeteShopCategoryService cateDal = new TeteShopCategoryService();

        IList<TeteShopCategoryInfo> cateList = cateDal.GetAllTeteShopCategory(Encrypt(nick));
        IList<GoodsClassInfo> classList = TaoBaoAPI.GetGoodsClassInfoList(info.Short, session, info.Appkey, info.Appsecret);

        if (classList == null)
        {
            Page.RegisterStartupScript("错误", "<script>alert('获取店铺分类出错!');</script>");
            return;
        }

        List<TeteShopCategoryInfo> addList = new List<TeteShopCategoryInfo>();

        List<TeteShopCategoryInfo> upList = new List<TeteShopCategoryInfo>();

        foreach (GoodsClassInfo cinfo in classList)
        {
            List<TeteShopCategoryInfo> clist = cateList.Where(o => o.Cateid == cinfo.cid).ToList();
            if (clist.Count > 0)
            {
                InitCate(nick, cinfo, clist[0]);
                clist[0].Catecount = classList.Count(o => o.parent_cid == cinfo.cid);
                upList.Add(clist[0]);
            }

            else
            {
                TeteShopCategoryInfo ainfo = new TeteShopCategoryInfo();
                InitCate(nick, cinfo, ainfo);
                ainfo.Catecount = classList.Count(o => o.parent_cid == cinfo.cid);

                addList.Add(ainfo);
            }
        }

        //添加
        foreach (TeteShopCategoryInfo cinfo in addList)
        {
            cateDal.AddTeteShopCategory(cinfo);
        }

        //修改
        foreach (TeteShopCategoryInfo cinfo in upList)
        {
            cateDal.ModifyTeteShopCategory(cinfo);
        }
        //更新商品
        ActionGoods(nick, session, info);

        //更新商品分类包含商品数量
        IList<TeteShopCategoryInfo> nowCateList = cateDal.GetAllTeteShopCategory(Encrypt(nick));
        TeteShopItemService itemDal = new TeteShopItemService();
        for (int i = 0; i < nowCateList.Count; i++)
        {
            int count = itemDal.GetItemCountByCId(nowCateList[i].Cateid);
            nowCateList[i].Catecount = count;
        }

        //修改
        foreach (TeteShopCategoryInfo cinfo in nowCateList)
        {
            cateDal.ModifyTeteShopCategory(cinfo);
        }

    }

    private static void ActionGoods(string nick, string session, TeteShopInfo info)
    {
        TeteShopItemService itemDal = new TeteShopItemService();

        List<GoodsInfo> glist = TaoBaoAPI.GetGoodsInfoListByNick(info.Short, session, info.Appkey, info.Appsecret);
        IList<TeteShopItemInfo> itemList = itemDal.GetAllTeteShopItem(Encrypt(nick));

        List<TeteShopItemInfo> addList = new List<TeteShopItemInfo>();
        List<TeteShopItemInfo> upList = new List<TeteShopItemInfo>();

        foreach (GoodsInfo cinfo in glist)
        {
            List<TeteShopItemInfo> clist = itemList.Where(o => o.Itemid == cinfo.num_iid).ToList();
            if (clist.Count > 0)
            {
                InitItem(nick, cinfo, clist[0]);
                upList.Add(clist[0]);
            }

            else
            {
                TeteShopItemInfo ainfo = new TeteShopItemInfo();
                InitItem(nick, cinfo, ainfo);

                addList.Add(ainfo);
            }
        }

        //添加
        foreach (TeteShopItemInfo cinfo in addList)
        {
            itemDal.AddTeteShopItem(cinfo);
        }

        //修改
        foreach (TeteShopItemInfo cinfo in upList)
        {
            itemDal.ModifyTeteShopItem(cinfo);
        }
    }

    private static void InitItem(string nick, GoodsInfo cinfo, TeteShopItemInfo ainfo)
    {
        ainfo.Itemid = cinfo.num_iid;
        ainfo.Nick = Encrypt(nick);
        ainfo.Price = (double)cinfo.price;
        ainfo.Picurl = cinfo.pic_url;
        ainfo.Itemname = cinfo.title;
        ainfo.Cateid = cinfo.seller_cids;
        ainfo.Linkurl = "http://item.taobao.com/item.htm?id=" + ainfo.Itemid;
    }

    private static void InitCate(string nick, GoodsClassInfo cinfo, TeteShopCategoryInfo ainfo)
    {
        ainfo.Cateid = cinfo.cid;
        ainfo.Parentid = cinfo.parent_cid;
        ainfo.Catename = cinfo.name;
        ainfo.Catepicurl = cinfo.pic_url;
        ainfo.Nick = Encrypt(nick);
    }

}
