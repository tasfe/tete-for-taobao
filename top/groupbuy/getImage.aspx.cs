using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;
using System.Drawing;
using System.IO;
using System.Collections;
using System.Net;
using System.Web.Security;

public partial class top_groupbuy_getImage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string id = string.Empty;
        string sql = string.Empty;
        string groupbuyname = string.Empty; 
        string groupbuybgImageUrl=string.Empty;
        string productprice = string.Empty; 
        string rebatePrice=string.Empty; 
        string rebate=string.Empty;
        string thrift = string.Empty; 
        string peopleCount=string.Empty;
        string count=string.Empty; 
        string goodsimageurl=string.Empty; 
        string imageSaveUrl=string.Empty;
        string maxcount = string.Empty;
        string newprice = string.Empty;
        string zhekou = string.Empty;
        if (!IsPostBack)
        {
            id = utils.NewRequest("id", utils.RequestType.QueryString);
            try
            {
                id = int.Parse(id).ToString();
            }
            catch {
                Response.Write("Error id");
                return;
            }
            sql = "SELECT * FROM TopGroupBuy WHERE ID="+id;
            DataTable dt= utils.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                DateTime time = DateTime.Now;
                groupbuyname = dt.Rows[0]["name"].ToString();
                productprice = dt.Rows[0]["productprice"].ToString();
                goodsimageurl = dt.Rows[0]["productimg"].ToString();
                maxcount = dt.Rows[0]["maxcount"].ToString();
                newprice = dt.Rows[0]["zhekou"].ToString();
                zhekou = dt.Rows[0]["groupbuyprice"].ToString();
           
                DateTime startDate = DateTime.Now;
                DateTime endDate = DateTime.Parse(dt.Rows[0]["endtime"].ToString());
                if (time < DateTime.Parse(dt.Rows[0]["starttime"].ToString()))
                {
                    string groupbuyimg = "D:\\groupbuy.7fshop.com/wwwroot/top/groupbuy/pic/" + DateTime.Now.ToString("yyyy-MM-dd") + "/" + FormsAuthentication.HashPasswordForStoringInConfigFile(goodsimageurl, "MD5") + ".GIF";
                    GreateImageGifTG(groupbuyname, "D:\\groupbuy.7fshop.com/wwwroot/top/groupbuy/images/newgroupbuy.png", productprice, zhekou, Math.Round((decimal.Parse(productprice) - decimal.Parse(newprice)) / decimal.Parse(productprice) * 10, 1).ToString(), newprice, DateTime.Parse(dt.Rows[0]["starttime"].ToString()), endDate, new Random().Next(100, 300).ToString(), maxcount, goodsimageurl, groupbuyimg);//未开始

                    //输入请求的图片

                    Stream stream = OpenFile(groupbuyimg);
                    BinaryReader br = new BinaryReader(stream);
                    int b = 256;
                    byte[] bt = new byte[b];
                    int n = br.Read(bt, 0, b);
                    while (n != 0)
                    {
                        Response.BinaryWrite(bt);
                        n = br.Read(bt, 0, b);
                    }
                    br.Close();
                    stream.Close();

                }
                else if (time >= DateTime.Parse(dt.Rows[0]["starttime"].ToString()) && time <= DateTime.Parse(dt.Rows[0]["endtime"].ToString()))
                {
                    string groupbuyingimg = "D:\\groupbuy.7fshop.com/wwwroot/top/groupbuy/pic/" + DateTime.Now.ToString("yyyy-MM-dd") + "/" + FormsAuthentication.HashPasswordForStoringInConfigFile(goodsimageurl, "MD5") + "ing.GIF";
                    GreateImageGifTG2(groupbuyname, "D:\\groupbuy.7fshop.com/wwwroot/top/groupbuy/images/newgroupbuyin.png", productprice, zhekou, Math.Round((decimal.Parse(productprice) - decimal.Parse(newprice)) / decimal.Parse(productprice) * 10, 1).ToString(), newprice, startDate, endDate, new Random().Next(100, 300).ToString(), maxcount, goodsimageurl, groupbuyingimg);//团购中
                    Stream stream = OpenFile(groupbuyingimg);
                    BinaryReader br = new BinaryReader(stream);
                    int b = 256;
                    byte[] bt = new byte[b];
                    int n = br.Read(bt, 0, b);
                    while (n != 0)
                    {
                        Response.BinaryWrite(bt);
                        n = br.Read(bt, 0, b);
                    }
                    br.Close();
                    stream.Close();

                }
                else
                {
                    string groupbuyendimg = "D:\\groupbuy.7fshop.com/wwwroot/top/groupbuy/pic/" + DateTime.Now.ToString("yyyy-MM-dd") + "/" + FormsAuthentication.HashPasswordForStoringInConfigFile(goodsimageurl, "MD5") + "end.GIF";
                    GreateImageGifTG3(groupbuyname, "D:\\groupbuy.7fshop.com/wwwroot/top/groupbuy/images/newgroupbuyend.png", productprice, zhekou, Math.Round((decimal.Parse(productprice) - decimal.Parse(newprice)) / decimal.Parse(productprice) * 10, 1).ToString(), newprice, startDate, endDate, new Random().Next(100, 300).ToString(), maxcount, goodsimageurl, groupbuyendimg);//已结束


                    Stream stream = OpenFile(groupbuyendimg);
                    BinaryReader br = new BinaryReader(stream);
                    int b = 256;
                    byte[] bt = new byte[b];
                    int n = br.Read(bt, 0, b);
                    while (n != 0)
                    {
                        Response.BinaryWrite(bt);
                        n = br.Read(bt, 0, b);
                    }
                    br.Close();
                    stream.Close();



                }

            }

        }
    }




    /// <summary>
    ///  生成团购宝贝图片
    /// </summary>
    /// <param name="url">图片地址</param>
    /// <param name="type">生成团购宝贝图片 1 团购前:2 团购中; 3 团购后()所需尺寸不一样)</param>
    /// <returns></returns>
    private string DownPic(string url, string type)
    {
        string strHtml = string.Empty;
        WebRequest request = WebRequest.Create(url);
        WebResponse response = request.GetResponse();
        Stream reader = response.GetResponseStream();
        string time = DateTime.Now.ToString("yyyy-MM-dd");
        int width = 450;
        int height = 435;
        string fileName = "D:\\groupbuy.7fshop.com/wwwroot/top/groupbuy/pic/" + time + "/productImg/" + FormsAuthentication.HashPasswordForStoringInConfigFile(url, "MD5") + ".GIF";
        string fileNametemp = "D:\\groupbuy.7fshop.com/wwwroot/top/groupbuy/pic/" + time + "/productImg/" + FormsAuthentication.HashPasswordForStoringInConfigFile(url, "MD5") + "temp.GIF";
        if (type == "2")
        {
            width = 450;
            height = 405;
            fileNametemp = "D:\\groupbuy.7fshop.com/wwwroot/top/groupbuy/pic/" + time + "/productImg/" + FormsAuthentication.HashPasswordForStoringInConfigFile(url, "MD5") + "intemp.GIF";
        }
        else if (type == "3")
        {
            width = 485;
            height = 445;
            fileNametemp = "D:\\groupbuy.7fshop.com/wwwroot/top/groupbuy/pic/" + time + "/productImg/" + FormsAuthentication.HashPasswordForStoringInConfigFile(url, "MD5") + "endtemp.GIF";
        }


        //创建文件夹
        if (!Directory.Exists("D:\\groupbuy.7fshop.com/wwwroot/top/groupbuy/pic/" + time + "/productImg/"))
        {
            Directory.CreateDirectory("D:\\groupbuy.7fshop.com/wwwroot/top/groupbuy/pic/" + time + "/productImg/");
        }




        if (!File.Exists(fileName))
        {
            FileStream writer = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
            byte[] buff = new byte[5120];
            int c = 0; //实际读取的字节数
            while ((c = reader.Read(buff, 0, buff.Length)) > 0)
            {
                writer.Write(buff, 0, c);
            }
            writer.Close();
        }
        reader.Close();

        fileNametemp = MakeThumbnail(fileName, fileNametemp, width, height, "W");

        //"HW"://指定高宽缩放（可能变形） "W"://指定宽，高按比例  "H"://指定高，宽按比例      "Cut"

        return fileNametemp;
    }

    /// <summary>        
    /// 生成缩略图        
    /// </summary>        
    /// <param name="originalImagePath">源图路径（物理路径）</param>        
    /// <param name="thumbnailPath">缩略图路径（物理路径）</param>        
    /// <param name="width">缩略图宽度</param>        
    /// <param name="height">缩略图高度</param>        
    /// <param name="mode">生成缩略图的方式</param> 
    public string MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
    {
        string saveImgsrc = thumbnailPath;
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
                if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                {
                    oh = originalImage.Height;
                    ow = originalImage.Height * towidth / toheight;
                    y = 0;
                    x = (originalImage.Width - ow) / 2;
                }
                else
                {
                    ow = originalImage.Width;
                    oh = originalImage.Width * height / towidth;
                    x = 0;
                    y = (originalImage.Height - oh) / 2;
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
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
        //设置高质量,低速度呈现平滑程度            
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        //清空画布并以透明背景色填充            
        g.Clear(Color.Transparent);
        //在指定位置并且按指定大小绘制原图片的指定部分           
        g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight), new Rectangle(x, y, ow, oh), GraphicsUnit.Pixel);
        try
        {
            //以jpg格式保存缩略图                
            if (bitmap.Height > height && bitmap.Height < 600 || bitmap.Height >= 1000)
            {
                if (!File.Exists(thumbnailPath))
                {
                    bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                string time = DateTime.Now.ToString("yyyy-MM-dd");
                string fileNametemp = thumbnailPath.Replace("temp.GIF", "temp2.GIF");
                saveImgsrc = MakeThumbnail(thumbnailPath, fileNametemp, width, height, "HW");
            }
            else if (bitmap.Height >= 600 && bitmap.Height < 1000)
            {
                if (!File.Exists(thumbnailPath))
                {
                    bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                string time = DateTime.Now.ToString("yyyy-MM-dd");
                string fileNametemp = thumbnailPath.Replace("temp.GIF", "temp2.GIF");
                saveImgsrc = MakeThumbnail(thumbnailPath, fileNametemp, width, height, "Cut");
            }
            else
            {
                saveImgsrc = thumbnailPath;
                if (!File.Exists(thumbnailPath))
                {
                    bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }

        }
        catch (System.Exception e)
        {
            throw e;
        }
        finally
        {
            originalImage.Dispose();
            bitmap.Clone();
            bitmap.Dispose();
            g.Dispose();
        }

        return saveImgsrc;
    }






    /// <summary>
    /// 生成团购图片  (还没开始的团购图片)
    /// </summary>
    /// <param name="tgName">团购名称  注：字符长度别超出124个字符</param>
    /// <param name="groupbuybgImageUrl">模板背景图片</param>
    /// <param name="price">原价</param>
    /// <param name="rebatePrice">折扣价</param>
    /// <param name="rebate">折扣</param>
    /// <param name="thrift">节省</param>
    /// <param name="startDate">团购开始时间</param>
    /// <param name="endDate">团购结束时间</param>
    /// <param name="peopleCount">参团人数</param>
    /// <param name="count">限购数量</param>
    /// <param name="goodsimageurl">商品图片 注：450*435</param>
    /// <param name="imageSaveUrl">团购图片保存路径</param>
    public void GreateImageGifTG(string tgName, string groupbuybgImageUrl, string price, string rebatePrice, string rebate, string thrift, DateTime startDate, DateTime endDate, string peopleCount, string count, string goodsimageurl, string imageSaveUrl)
    {
        if (tgName.Length > 124)
        {
            Response.Write("长度超出120个字符");
            return;
        }
        bool b = true;
        ArrayList al = new ArrayList();
        int max = 31;
        int min = 30;
        int fontSize = 18;
        string fontStr = "微软雅黑";
        string fontStrAU = "Arial";
        string fontStrhk = "MS PGothic";
        string fontStrST = "宋体";
        string fontStrht = "黑体";
        SolidBrush b1 = new SolidBrush(System.Drawing.ColorTranslator.FromHtml("#333333"));//定义单色画刷
        SolidBrush b2 = new SolidBrush(System.Drawing.ColorTranslator.FromHtml("#DC442F"));//定义单色画刷　
        SolidBrush b3 = new SolidBrush(Color.White);//定义单色画刷　
        int height = 34;
        int leftwieth = 30;
        int fontRedIndex = 4;//头部字体需要加红的字数
        //绘制图头部
        #region 绘制图头部
        while (b)
        {
            int index = 0;
            if (tgName.Length > 0 && tgName.Length > max)
            {

                int c2 = 0;

                if (al.Count < 1)
                {
                    min = min - 4;
                }
                else
                {
                    min = 30;
                }

                for (int i = 0; i < tgName.Substring(0, min).Length; i++)
                {

                    if (tgName.Substring(0, min)[i] == '！' || tgName.Substring(0, min)[i] == ':' || tgName.Substring(0, min)[i] == '，' || tgName.Substring(0, min)[i] == '【' || tgName.Substring(0, min)[i] == '】' || tgName.Substring(0, min)[i] == ']' || tgName.Substring(0, min)[i] == '[')
                    {
                        c2++;
                    }
                }
                if (c2 > 2)
                {
                    index++;
                }
                if (c2 > 4)
                {
                    index++;
                }

                al.Add(tgName.Substring(0, min + index));
                tgName = tgName.Substring(min + index);
                if (tgName.Length < max)
                {
                    al.Add(tgName);
                    break;
                }
            }
            else
            {
                al.Add(tgName);
                break;
            }
        }

        #endregion


        String outputFilePath2 = groupbuybgImageUrl; //图片模板背景
        using (FileStream fs = new FileStream(outputFilePath2, FileMode.Open))
        {
            //Stream stream = OpenFile(outputFilePath2);
            Bitmap ImageFrame2 = new Bitmap(fs);
            Graphics gh = Graphics.FromImage(ImageFrame2);

            for (int i = 0; i < al.Count; i++)
            {
                if (i == 0)
                {
                    if (al[i].ToString().Length > fontRedIndex)
                    {
                        gh.DrawString("今日团购：", new Font(fontStr, fontSize, FontStyle.Bold), b2, new PointF(leftwieth, height));
                        gh.DrawString(al[i].ToString(), new Font(fontStr, fontSize, FontStyle.Bold), b1, new PointF(155, height));
                    }
                    else
                    {
                        gh.DrawString(al[i].ToString(), new Font(fontStr, fontSize, FontStyle.Bold), b1, new PointF(leftwieth, height));
                    }
                }
                else
                {
                    gh.DrawString(al[i].ToString(), new Font(fontStr, fontSize, FontStyle.Bold), b1, new PointF(leftwieth, (i + 1) * height));
                }
            }



            //这里还要做一个处理  价格如果是四位数 宽度要调整

            gh.DrawString(rebatePrice.Replace(".00", ""), new Font(fontStrAU, 36, FontStyle.Bold), b3, new PointF(56, 93));
            if (rebatePrice.Replace(".00", "").Length > 2)
            {
                if (rebatePrice.Replace(".00", "").IndexOf(".") < 1)
                {
                    if (rebatePrice.Replace(".00", "").Length <= 3)
                    {
                        gh.DrawString(".00", new Font(fontStrAU, 26, FontStyle.Bold), b3, new PointF(134, 105));
                    }
                }
            }
            else
            {
                if (rebatePrice.Replace(".00", "").IndexOf(".") < 1)
                {
                    if (rebatePrice.Replace(".00", "").Length == 1)
                    {
                        gh.DrawString(".00", new Font(fontStrAU, 26, FontStyle.Bold), b3, new PointF(87, 105));
                    }
                    else
                    {
                        gh.DrawString(".00", new Font(fontStrAU, 26, FontStyle.Bold), b3, new PointF(114, 105));
                    }
                }
            }

            //创建铅笔对象
            Pen ImagePen = new Pen(Color.Black, 1);
            //创建随机对象
            Random rand = new Random();
            //画线
            gh.DrawLine(ImagePen, new Point(46, 193), new Point(95, 197));
            //绘制价格
            if (price.ToString().IndexOf(".") < 1)
            {
                price = price + ".00";

            }
            gh.DrawString("￥" + price.ToString(), new Font(fontStr, 12), b1, new PointF(40, 182));
            //绘制折扣
            gh.DrawString(rebate.ToString(), new Font(fontStr, 12), b1, new PointF(140, 182));
            //绘制节省
            gh.DrawString("￥" + thrift.ToString(), new Font(fontStr, 12), b1, new PointF(200, 182));

            System.Drawing.Image im = System.Drawing.Image.FromFile(DownPic(goodsimageurl, "1"));

            gh.DrawImage(im, 288, 91, float.Parse(im.Width.ToString()), float.Parse(im.Height.ToString()));

            TimeSpan tdays = startDate - DateTime.Now;  //得到时间差
            string h = Math.Floor(tdays.TotalHours).ToString();

            Random rand2 = new Random();
            gh.DrawString(h, new Font(fontStr, 16), b1, new PointF(97, 314));
            gh.DrawString(tdays.Minutes.ToString(), new Font(fontStr, 16), b1, new PointF(157, 314));
            gh.DrawString(tdays.Seconds.ToString() + "." + rand2.Next(10).ToString(), new Font(fontStr, 16), b1, new PointF(200, 314));

            //im = System.Drawing.Image.FromFile("c:\\test.gif");
            //gh.DrawImage(im, 204, 410, float.Parse(im.Width.ToString()), float.Parse(im.Height.ToString()));
            ImageFrame2.Save(imageSaveUrl);
            gh.Dispose();
            ImageFrame2.Clone();
            ImageFrame2.Dispose();
            fs.Close();
            fs.Dispose();
        }
    }



    /// <summary>
    /// 生成团购图片  (团购中图片)
    /// </summary>
    /// <param name="tgName">团购名称  注：字符长度别超出124个字符</param>
    /// <param name="groupbuybgImageUrl">模板背景图片</param>
    /// <param name="price">原价</param>
    /// <param name="rebatePrice">折扣价</param>
    /// <param name="rebate">折扣</param>
    /// <param name="thrift">节省</param>
    /// <param name="startDate">团购开始时间</param>
    /// <param name="endDate">团购结束时间</param>
    /// <param name="peopleCount">参团人数</param>
    /// <param name="count">限购数量</param>
    /// <param name="goodsimageurl">商品图片 注：450*435</param>
    /// <param name="imageSaveUrl">团购图片保存路径</param>
    public void GreateImageGifTG2(string tgName, string groupbuybgImageUrl, string price, string rebatePrice, string rebate, string thrift, DateTime startDate, DateTime endDate, string peopleCount, string count, string goodsimageurl, string imageSaveUrl)
    {
        if (tgName.Length > 93)
        {
            Response.Write("长度超出93个字符");
            return;
        }
        bool b = true;
        ArrayList al = new ArrayList();
        int max = 31;
        int min = 30;
        int fontSize = 24;
        string fontStr = "微软雅黑";
        string fontStrAU = "Arial";
        string fontStrhk = "MS PGothic";
        string fontStrST = "宋体";
        string fontStrht = "黑体";
        SolidBrush b1 = new SolidBrush(System.Drawing.ColorTranslator.FromHtml("#333333"));//定义单色画刷
        SolidBrush b2 = new SolidBrush(System.Drawing.ColorTranslator.FromHtml("#DC442F"));//定义单色画刷　
        SolidBrush b3 = new SolidBrush(Color.White);//定义单色画刷　 
        SolidBrush b4 = new SolidBrush(System.Drawing.ColorTranslator.FromHtml("#66B024"));//定义单色画刷
        int height = 35;
        int leftwieth = 30;
        int fontRedIndex = 8;//头部字体需要加红的字数
        //绘制图头部
        #region 绘制图头部
        while (b)
        {
            int index = 0;
            if (tgName.Length > 0 && tgName.Length > max)
            {

                int c2 = 0;

                for (int i = 0; i < tgName.Substring(0, min).Length; i++)
                {

                    if (tgName.Substring(0, min)[i] == '！' || tgName.Substring(0, min)[i] == ':' || tgName.Substring(0, min)[i] == '，' || tgName.Substring(0, min)[i] == '【' || tgName.Substring(0, min)[i] == '】' || tgName.Substring(0, min)[i] == ']' || tgName.Substring(0, min)[i] == '[')
                    {
                        c2++;
                    }
                }
                if (c2 > 2)
                {
                    index++;
                }
                if (c2 > 4)
                {
                    index++;
                }

                al.Add(tgName.Substring(0, min + index));
                tgName = tgName.Substring(min + index);
                if (tgName.Length < max)
                {
                    al.Add(tgName);
                    break;
                }
            }
            else
            {
                al.Add(tgName);
                break;
            }
        }
        #endregion

        String outputFilePath2 = groupbuybgImageUrl; //图片模板背景
        using (FileStream fs = new FileStream(outputFilePath2, FileMode.Open))
        {
            //Stream stream = OpenFile(outputFilePath2)
            Bitmap ImageFrame2 = new Bitmap(fs);
            Graphics gh = Graphics.FromImage(ImageFrame2);

            for (int i = 0; i < al.Count; i++)
            {
                if (i == 0)
                {
                    if (al[i].ToString().Length > fontRedIndex)
                    {
                        gh.DrawString("今日团购：", new Font(fontStr, fontSize, FontStyle.Bold), b2, new PointF(leftwieth, height));
                        gh.DrawString(al[i].ToString(), new Font(fontStr, fontSize, FontStyle.Bold), b1, new PointF(155, height));
                    }
                    else
                    {
                        gh.DrawString(al[i].ToString(), new Font(fontStr, fontSize, FontStyle.Bold), b1, new PointF(leftwieth, height));
                    }
                }
                else
                {
                    gh.DrawString(al[i].ToString(), new Font(fontStr, fontSize, FontStyle.Bold), b1, new PointF(leftwieth, (i + 1) * height));
                }
            }



            //这里还要做一个处理  价格如果是四位数 宽度要调整

            gh.DrawString(rebatePrice.ToString().Replace(".00", ""), new Font(fontStrAU, 40, FontStyle.Bold), b3, new PointF(58, 99));
            if (rebatePrice.Replace(".00", "").Length > 2)
            {
                if (rebatePrice.Replace(".00", "").IndexOf(".") < 1)
                {
                    if (rebatePrice.Replace(".00", "").Length <= 3)
                    {
                        gh.DrawString(".00", new Font(fontStrAU, 28, FontStyle.Bold), b3, new PointF(130, 110));
                    }
                }
            }
            else
            {
                if (rebatePrice.Replace(".00", "").IndexOf(".") < 1)
                {

                    if (rebatePrice.Replace(".00", "").Length == 1)
                    {
                        gh.DrawString(".00", new Font(fontStrAU, 26, FontStyle.Bold), b3, new PointF(83, 110));
                    }
                    else
                    {
                        gh.DrawString(".00", new Font(fontStrAU, 26, FontStyle.Bold), b3, new PointF(110, 110));
                    }

                }
            }


            //创建铅笔对象
            Pen ImagePen = new Pen(Color.Black, 1);
            //创建随机对象
            Random rand = new Random();
            //画线
            gh.DrawLine(ImagePen, new Point(40, 188), new Point(100, 192));
            //绘制价格
            if (price.ToString().IndexOf(".") < 1)
            {
                price = price + ".00";
            }
            gh.DrawString("￥" + price.ToString(), new Font(fontStr, 16), b1, new PointF(40, 180));
            //绘制折扣
            gh.DrawString(rebate.ToString(), new Font(fontStr, 16), b1, new PointF(140, 180));
            //绘制节省
            gh.DrawString("￥" + thrift.ToString(), new Font(fontStr, 16), b1, new PointF(202, 180));

            //参加人数
            gh.DrawString(peopleCount.ToString(), new Font(fontStr, 30), b4, new PointF(peopleCount.Length > 2 ? 50 : 80, 233));

            System.Drawing.Image im = System.Drawing.Image.FromFile(DownPic(goodsimageurl, "2"));


            gh.DrawImage(im, 289, 91, float.Parse(im.Width.ToString()), float.Parse(im.Height.ToString()));


            TimeSpan tdays = endDate - startDate;  //得到时间差
            string h = Math.Floor(tdays.TotalHours).ToString();

            Random rand2 = new Random();
            gh.DrawString(h, new Font(fontStr, 20), b1, new PointF(90, 316));
            gh.DrawString(tdays.Minutes.ToString(), new Font(fontStr, 20), b1, new PointF(154, 316));
            gh.DrawString(tdays.Seconds.ToString() + "." + rand2.Next(10).ToString(), new Font(fontStr, 20), b1, new PointF(190, 316));
     
            ImageFrame2.Save(imageSaveUrl);
          
            gh.Dispose();
            ImageFrame2.Clone();
            ImageFrame2.Dispose();
            fs.Close();
            fs.Dispose();
        }
    }


    /// <summary>
    /// 生成团购图片  (团购图片)
    /// </summary>
    /// <param name="tgName">团购名称  注：字符长度别超出124个字符</param>
    /// <param name="groupbuybgImageUrl">模板背景图片</param>
    /// <param name="price">原价</param>
    /// <param name="rebatePrice">折扣价</param>
    /// <param name="rebate">折扣</param>
    /// <param name="thrift">节省</param>
    /// <param name="startDate">团购开始时间</param>
    /// <param name="endDate">团购结束时间</param>
    /// <param name="peopleCount">参团人数</param>
    /// <param name="count">限购数量</param>
    /// <param name="goodsimageurl">商品图片 注：450*435</param>
    /// <param name="imageSaveUrl">团购图片保存路径</param>
    public void GreateImageGifTG3(string tgName, string groupbuybgImageUrl, string price, string rebatePrice, string rebate, string thrift, DateTime startDate, DateTime endDate, string peopleCount, string count, string goodsimageurl, string imageSaveUrl)
    {
        if (tgName.Length > 93)
        {
            Response.Write("长度超出93个字符");
            return;
        }
        bool b = true;
        ArrayList al = new ArrayList();
        int max = 31;
        int min = 30;
        int fontSize = 24;
        string fontStr = "微软雅黑";
        string fontStrAU = "Arial";
        string fontStrhk = "MS PGothic";
        string fontStrST = "宋体";
        string fontStrht = "黑体";
        SolidBrush b1 = new SolidBrush(System.Drawing.ColorTranslator.FromHtml("#333333"));//定义单色画刷
        SolidBrush b2 = new SolidBrush(System.Drawing.ColorTranslator.FromHtml("#DC442F"));//定义单色画刷　
        SolidBrush b3 = new SolidBrush(Color.White);//定义单色画刷　 
        SolidBrush b4 = new SolidBrush(System.Drawing.ColorTranslator.FromHtml("#66B024"));//定义单色画刷
        int height = 30;
        int leftwieth = 40;
        int fontRedIndex = 8;//头部字体需要加红的字数
        //绘制图头部
        #region 绘制图头部
        while (b)
        {
            int index = 0;
            if (tgName.Length > 0 && tgName.Length > max)
            {
                int c2 = 0;
                for (int i = 0; i < tgName.Substring(0, min).Length; i++)
                {
                    if (tgName.Substring(0, min)[i] == '！' || tgName.Substring(0, min)[i] == ':' || tgName.Substring(0, min)[i] == '，' || tgName.Substring(0, min)[i] == '【' || tgName.Substring(0, min)[i] == '】' || tgName.Substring(0, min)[i] == ']' || tgName.Substring(0, min)[i] == '[')
                    {
                        c2++;
                    }
                }
                if (c2 > 2)
                {
                    index++;
                }
                if (c2 > 4)
                {
                    index++;
                }

                al.Add(tgName.Substring(0, min + index));
                tgName = tgName.Substring(min + index);
                if (tgName.Length < max)
                {
                    al.Add(tgName);
                    break;
                }
            }
            else
            {
                al.Add(tgName);
                break;
            }
        }

        #endregion
        String outputFilePath2 = groupbuybgImageUrl; //图片模板背景
        using (FileStream fs = new FileStream(outputFilePath2, FileMode.Open))
        {
            //Stream stream = OpenFile(outputFilePath2);
            Bitmap ImageFrame2 = new Bitmap(fs);
            Graphics gh = Graphics.FromImage(ImageFrame2);

            for (int i = 0; i < al.Count; i++)
            {
                if (i == 0)
                {
                    if (al[i].ToString().Length > fontRedIndex)
                    {
                        gh.DrawString("今日团购：", new Font(fontStr, fontSize, FontStyle.Bold), b2, new PointF(leftwieth, height));
                        gh.DrawString(al[i].ToString(), new Font(fontStr, fontSize, FontStyle.Bold), b1, new PointF(leftwieth + 125, height));
                    }
                    else
                    {
                        gh.DrawString(al[i].ToString(), new Font(fontStr, fontSize, FontStyle.Bold), b1, new PointF(leftwieth, height));
                    }
                }
                else
                {
                    gh.DrawString(al[i].ToString(), new Font(fontStr, fontSize, FontStyle.Bold), b1, new PointF(leftwieth, (i + 1) * height));
                }
            }


            //这里还要做一个处理  价格如果是四位数 宽度要调整

            gh.DrawString(rebatePrice.ToString().Replace(".00", ""), new Font(fontStrAU, 40, FontStyle.Bold), b3, new PointF(leftwieth + 46, 95));

            if (rebatePrice.Replace(".00", "").Length > 2)
            {
                if (rebatePrice.Replace(".00", "").IndexOf(".") < 1)
                {
                    if (rebatePrice.Replace(".00", "").Length <= 3)
                    {
                        gh.DrawString(".00", new Font(fontStrAU, 28, FontStyle.Bold), b3, new PointF(leftwieth + 114, 105));
                    }
                }
            }
            else
            {
                if (rebatePrice.Replace(".00", "").IndexOf(".") < 1)
                {
                    if (rebatePrice.Replace(".00", "").Length == 1)
                    {
                        gh.DrawString(".00", new Font(fontStrAU, 26, FontStyle.Bold), b3, new PointF(leftwieth + 70, 105));
                    }
                    else
                    {
                        gh.DrawString(".00", new Font(fontStrAU, 26, FontStyle.Bold), b3, new PointF(leftwieth + 94, 105));
                    }
                }
            }




            //创建铅笔对象
            Pen ImagePen = new Pen(Color.Black, 1);
            //创建随机对象
            Random rand = new Random();
            //画线
            gh.DrawLine(ImagePen, new Point(50, 188), new Point(103, 191));
            //绘制价格
            if (price.ToString().IndexOf(".") < 1)
            {
                price = price + ".00";
            }
            gh.DrawString("￥" + price.ToString(), new Font(fontStr, 16), b1, new PointF(50, 180));
            //绘制折扣
            gh.DrawString(rebate.ToString(), new Font(fontStr, 16), b1, new PointF(165, 180));
            //绘制节省
            gh.DrawString("￥" + thrift.ToString(), new Font(fontStr, 16), b1, new PointF(232, 180));

            //参加人数
            gh.DrawString(peopleCount.ToString(), new Font(fontStr, 30), b4, new PointF(peopleCount.Length > 2 ? 80 : 100, 233));

            System.Drawing.Image im = System.Drawing.Image.FromFile(DownPic(goodsimageurl, "3"));


            gh.DrawImage(im, leftwieth + 285, 83, float.Parse(im.Width.ToString()), float.Parse(im.Height.ToString()));


            TimeSpan tdays = endDate - startDate;  //得到时间差
            string h = Math.Floor(tdays.TotalHours).ToString();

            Random rand2 = new Random();
            gh.DrawString(h, new Font(fontStr, 20), b1, new PointF(104, 328));
            gh.DrawString(tdays.Minutes.ToString(), new Font(fontStr, 20), b1, new PointF(166, 328));
            gh.DrawString(tdays.Seconds.ToString() + "." + rand2.Next(10).ToString(), new Font(fontStr, 20), b1, new PointF(202, 328));

            ImageFrame2.Save(imageSaveUrl);
            gh.Dispose();
            ImageFrame2.Clone();
            ImageFrame2.Dispose();
            fs.Close();
            fs.Dispose();
        }
    }


}