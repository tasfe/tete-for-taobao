using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;
using Microsoft.Win32;
using System.Drawing;
using System.Drawing.Imaging;

public static class HttpUtil
{
    //根据文件名获取文件类型
    public static string GetContentType(string fileName)
    {
        string contentType = "application/octetstream";
        string ext = Path.GetExtension(fileName).ToLower();
        RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(ext);

        if (registryKey != null && registryKey.GetValue("Content Type") != null)
        {
            contentType = registryKey.GetValue("Content Type").ToString();
        }

        return contentType;
    }

    //根据query String获取parameter数据
    public static List<Parameter> GetQueryParameters(string queryString)
    {
        if (queryString.StartsWith("?"))
        {
            queryString = queryString.Remove(0, 1);
        }

        List<Parameter> result = new List<Parameter>();

        if (!string.IsNullOrEmpty(queryString))
        {
            string[] p = queryString.Split('&');
            foreach (string s in p)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    if (s.IndexOf('=') > -1)
                    {
                        string[] temp = s.Split('=');
                        result.Add(new Parameter(temp[0], temp[1]));
                    }
                }
            }
        }

        return result;
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
        System.Drawing.Image originalImage = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(originalImagePath));

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

}
