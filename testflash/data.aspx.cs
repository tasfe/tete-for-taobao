using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;
using System.IO;
using System.Drawing;
using System.Net;
using System.Web.Security;

public partial class testflash_data : System.Web.UI.Page
{

    public static string logUrl = "D:/svngroupbuy/website/ErrLog";
    protected void Page_Load(object sender, EventArgs e)
    {
        //Response.Write(HttpUtility.UrlEncode("叶儿随清风") + "<br>");
 

        string str = string.Empty;
        string nick = utils.NewRequest("a", utils.RequestType.QueryString).Replace("'", "''");
        string sql = "SELECT nick,title FROM TopTaobaoShop WHERE sid = '" + nick + "'";
        string shopName = string.Empty;
        DataTable dtShop = utils.ExecuteDataTable(sql);
        if (dtShop != null && dtShop.Rows.Count > 0)
        {
            nick = dtShop.Rows[0]["nick"].ToString();
            shopName = dtShop.Rows[0]["title"].ToString();
        }


        sql = "SELECT TOP 1 * FROM TopGroupBuy WHERE nick = '" + nick + "' AND isdelete=0 AND [endtime]>getdate() ORDER BY ID  DESC ";
        //Response.Write(sql); 
        WriteDeleteLog("flash:" + sql, "");
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
           string groupbuyname = dt.Rows[0]["name"].ToString();
           string productprice = dt.Rows[0]["productprice"].ToString();
           string goodsimageurl = dt.Rows[0]["productimg"].ToString();
           string maxcount = dt.Rows[0]["maxcount"].ToString();
           string newprice = dt.Rows[0]["zhekou"].ToString();
           string zhekou = dt.Rows[0]["groupbuyprice"].ToString();

            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Parse(dt.Rows[0]["endtime"].ToString());
            goodsimageurl=  DownPic(goodsimageurl);
            TimeSpan tdays = endDate - startDate;  //得到时间差
            string h = Math.Floor(tdays.TotalHours).ToString();
            str = goodsimageurl;
            str += "|" + productprice;
            str += "|" + Math.Round((decimal.Parse(productprice) - decimal.Parse(newprice)) / decimal.Parse(productprice) * 10, 1).ToString();
            str += "|" + zhekou;
            str += "|" + groupbuyname;
            str += "|" + dt.Rows[0]["producturl"].ToString();
            str += "|" + newprice;
            str += "|" + dt.Rows[0]["id"].ToString();
            str += "|" + dt.Rows[0]["groupbyPcount"].ToString();
            str += "|" + h;
            str += "|" + tdays.Minutes.ToString();
            str += "|" + tdays.Seconds.ToString();
            str += "|" + shopName;
            str += "|" + HttpUtility.UrlEncode(nick);
        }
        else
        {
            sql = "select id,name ,startDate,enddate,productname,productprice,productimg,producturl,productid,nick,rcount as maxcount,rcount as buycount,discountType,discountValue ,tagid,promotionid,rcount as groupbyPcount from tete_activitylist WHERE nick = '" + nick + "' AND status=1 AND enddate>getdate() ORDER BY ID  DESC";
             dt = utils.ExecuteDataTable(sql);
             if (dt.Rows.Count != 0)
             {
                 string groupbuyname = dt.Rows[0]["name"].ToString();
                 string productprice = dt.Rows[0]["productprice"].ToString();
                 string goodsimageurl = dt.Rows[0]["productimg"].ToString();
                 string maxcount = dt.Rows[0]["maxcount"].ToString();
                 string discountType = dt.Rows[0]["discountType"].ToString();
                 string discountValue = dt.Rows[0]["discountValue"].ToString();
                 string newprice = "";
                 string zhekou = "";
                 if (discountType.Trim() == "DISCOUNT")
                 {
                     try
                     {
                         newprice = (decimal.Parse(productprice) * decimal.Parse(discountValue) * 0.1m).ToString();
                         zhekou = dt.Rows[0]["discountValue"].ToString();
                     }
                     catch {
                         newprice = "0";
                         zhekou = "0";
                     }
                 }
                 else
                 {
                     try
                     { 
                         newprice = (decimal.Parse(productprice) - decimal.Parse(discountValue)).ToString();
                         zhekou = (decimal.Parse(newprice) * 0.1m).ToString();
                     }
                     catch
                     {
                         newprice = "0";
                         zhekou = "0";
                     }
                 }
                 DateTime startDate = DateTime.Now;
                 DateTime endDate = DateTime.Parse(dt.Rows[0]["endtime"].ToString());
                 goodsimageurl = DownPic(goodsimageurl);
                 TimeSpan tdays = endDate - startDate;  //得到时间差
                 string h = Math.Floor(tdays.TotalHours).ToString();
                 str = goodsimageurl;
                 str += "|" + productprice;
                 str += "|" + Math.Round((decimal.Parse(productprice) - decimal.Parse(newprice)) / decimal.Parse(productprice) * 10, 1).ToString();
                 str += "|" + zhekou;
                 str += "|" + groupbuyname;
                 str += "|" + dt.Rows[0]["producturl"].ToString();
                 str += "|" + newprice;
                 str += "|" + dt.Rows[0]["id"].ToString();
                 str += "|" + dt.Rows[0]["groupbyPcount"].ToString();
                 str += "|" + h;
                 str += "|" + tdays.Minutes.ToString();
                 str += "|" + tdays.Seconds.ToString();
                 str += "|" + shopName;
                 str += "|" + HttpUtility.UrlEncode(nick);
             }
             else
             {
                 str = "0";
             }
        }

        WriteDeleteLog("flashStr:" + str, "");
        //str = "http://img04.taobaocdn.com/imgextra/i4/T1EnVkXknLDtPAjkA._113138.jpg_310x310.jpg|1234.10|5.0|617.00";

        Response.Write(str);

    }

    /// <summary>
    /// 写日志
    /// </summary>
    /// <param name="value">日志内容</param>
    /// <param name="type">类型 0(成功日志),1(错误日志) 可传空文本默认为0</param>
    /// <returns></returns>
    public static void WriteDeleteLog(string message, string type)
    {
        string tempStr = logUrl + "/Write" + DateTime.Now.ToString("yyyyMMdd");//文件夹路径
        string tempFile = tempStr + "/flash" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
        if (type == "1")
        {
            tempFile = tempStr + "/flashErr" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
        }
        if (!Directory.Exists(tempStr))
        {
            Directory.CreateDirectory(tempStr);
        }

        if (System.IO.File.Exists(tempFile))
        {
            ///如果日志文件已经存在，则直接写入日志文件
            StreamWriter sr = System.IO.File.AppendText(tempFile);
            sr.WriteLine("\n");
            sr.WriteLine(DateTime.Now + "\n" + message);
            sr.Close();
        }
        else
        {
            ///创建日志文件
            StreamWriter sr = System.IO.File.CreateText(tempFile);
            sr.WriteLine(DateTime.Now + "\n" + message);
            sr.Close();
        }

    }


    /// <summary>
    ///  生成团购宝贝图片
    /// </summary>
    /// <param name="url">图片地址</param>
    /// <returns></returns>
    private string DownPic(string url)
    {
        string strHtml = string.Empty;
        WebRequest request = WebRequest.Create(url);
        WebResponse response = request.GetResponse();
        Stream reader = response.GetResponseStream();
        string time = DateTime.Now.ToString("yyyy-MM-dd");
        int width = 440;
        int height = 300;
        string fileName = "D:\\groupbuy.7fshop.com/wwwroot/top/groupbuy/pic/" + time + "/productImg/" + FormsAuthentication.HashPasswordForStoringInConfigFile(url, "MD5") + ".GIF";
        string fileNametemp = "D:\\groupbuy.7fshop.com/wwwroot/top/groupbuy/pic/" + time + "/productImg/" + FormsAuthentication.HashPasswordForStoringInConfigFile(url, "MD5") + "440300temp.GIF";

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
        fileNametemp = fileNametemp.Replace("D:\\groupbuy.7fshop.com/wwwroot/top/groupbuy/pic/", "http://groupbuy.7fshop.com/top/groupbuy/pic/");
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

}