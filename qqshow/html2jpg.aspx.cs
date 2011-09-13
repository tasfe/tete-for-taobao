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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Taobao.Top.Api;
using Common;
using System.Net;
using System.IO;

public partial class topTest_market_html2jpg : System.Web.UI.Page
{
    private string id = string.Empty;
    private string folderPath = string.Empty;
    private string size = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", "12132145", "1fdd2aadd5e2ac2909db2967cbb71e7f");

        id = utils.NewRequest("id", utils.RequestType.QueryString);
        string title = string.Empty;
        string width = string.Empty;
        string height = string.Empty;
        string num = string.Empty;
        string taobaoNick = string.Empty;

        //获取广告标题
        string sql = string.Empty;

        //图片缓存判断
        if (1 == 1)
        {
            sql = "SELECT name,size,nick FROM TopIdea WHERE id = " + id;
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                taobaoNick = dt.Rows[0]["nick"].ToString();

                //创建图片临时文件架
                folderPath = Server.MapPath("folder/" + MD5(dt.Rows[0]["nick"].ToString()));
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                //判断生成好的图片是否过期
                string resultPath = folderPath + "/result_" + id + ".jpg";
                if (File.Exists(resultPath) && id != "3972")
                {
                    //判断过期时间为1小时
                    System.TimeSpan ts = DateTime.Now - File.GetLastWriteTime(resultPath);
                    if (ts.Seconds < 3600)
                    {
                        RecordAndShow(folderPath);
                        return;
                    }
                }

                title = dt.Rows[0]["name"].ToString();
                size = dt.Rows[0]["size"].ToString();

                string[] arr = size.Split('*');

                width = arr[0];
                height = arr[1];

                switch (size)
                {
                    case "514*160":
                        num = "5";
                        break;
                    case "514*288":
                        num = "10";
                        break;
                    case "664*160":
                        num = "6";
                        break;
                    case "312*288":
                        num = "6";
                        break;
                    case "336*280":
                        num = "4";
                        break;
                    case "714*160":
                        num = "7";
                        break;
                    case "114*418":
                        num = "3";
                        break;
                    case "218*286":
                        num = "4";
                        break;
                    case "743*308":
                        num = "4";
                        break;
                    default:
                        num = "4";
                        break;
                }
            }
            else
            {
                return;
            }

            //获取数据库中保存的商品
            sql = "SELECT TOP " + num + " * FROM TopIdeaProduct WHERE ideaid = " + id + "";
            dt = utils.ExecuteDataTable(sql);

            //生成图片
            string proPath = string.Empty;
            System.Drawing.Image pro1;
            System.Drawing.Image pro2;

            //背景图片
            string bgPath = Server.MapPath("images/bg1.jpg");
            System.Drawing.Image CurrentBitmap = System.Drawing.Image.FromFile(bgPath);

            switch (size)
            {
                //风格1
                case "514*160":
                    bgPath = Server.MapPath("images/bg1.jpg");
                    CurrentBitmap = System.Drawing.Image.FromFile(bgPath);
                    break;
                //风格2
                case "514*288":
                    bgPath = Server.MapPath("images/bg2.jpg");
                    CurrentBitmap = System.Drawing.Image.FromFile(bgPath);
                    break;
                //风格3
                case "312*288":
                    bgPath = Server.MapPath("images/bg3.jpg");
                    CurrentBitmap = System.Drawing.Image.FromFile(bgPath);
                    break;
                //风格4
                case "714*160":
                    bgPath = Server.MapPath("images/bg4.jpg");
                    CurrentBitmap = System.Drawing.Image.FromFile(bgPath);
                    break;
                //风格5
                case "114*418":
                    bgPath = Server.MapPath("images/bg5.jpg");
                    CurrentBitmap = System.Drawing.Image.FromFile(bgPath);
                    break;
                //风格6
                case "664*160":
                    bgPath = Server.MapPath("images/bg6.jpg");
                    CurrentBitmap = System.Drawing.Image.FromFile(bgPath);
                    break;
                //风格7
                case "218*286":
                    bgPath = Server.MapPath("images/bg7.jpg");
                    CurrentBitmap = System.Drawing.Image.FromFile(bgPath);
                    break;
                //风格8
                case "743*308":
                    bgPath = Server.MapPath("images/bg8.jpg");
                    CurrentBitmap = System.Drawing.Image.FromFile(bgPath);
                    break;
            }

            string nickid = string.Empty;
            string sqlNew = "SELECT sellerUin FROM TopPaipaiShop WHERE sellerUin = '" + taobaoNick + "'";
            DataTable dtNew = utils.ExecuteDataTable(sqlNew);
            if (dtNew.Rows.Count != 0)
            {
                nickid = "http://shop.paipai.com/" + dtNew.Rows[0][0].ToString();
            }
            else
            {
                nickid = "http://www.paipai.com/";
            }

            using (Graphics graphics = Graphics.FromImage(CurrentBitmap))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                //文字
                Font font = new Font("宋体", 12);
                if (size == "514*160" || size == "514*288" || size == "714*160" || size == "664*160")
                {
                    graphics.DrawString(title + " " + nickid + "", font, new SolidBrush(Color.Black), 16, 6);
                }
                else if (size == "743*308")
                {
                    font = new Font("宋体", 13, FontStyle.Bold);
                    graphics.DrawString(title, font, new SolidBrush(Color.White), 38, 10);
                }
                else
                {
                    graphics.DrawString(title, font, new SolidBrush(Color.Black), 16, 6);
                }

                switch(size)
                {
                    //风格1
                    case "514*160":
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            saveimage(dt.Rows[i]["itempicurl"].ToString() + ".2.jpg", folderPath);
                            proPath = folderPath + "/proTmp.jpg";
                            pro1 = System.Drawing.Image.FromFile(proPath);
                            graphics.DrawImage(pro1, 17 + i * 100, 29, 80, 80);
                            graphics.DrawString(left(dt.Rows[i]["itemname"].ToString(), 8), font, new SolidBrush(ColorTranslator.FromHtml("#0394EF")), 14 + i * 100, 112);
                            graphics.DrawString(left(dt.Rows[i]["itemname"].ToString(), 8, 16), font, new SolidBrush(ColorTranslator.FromHtml("#0394EF")), 14 + i * 100, 126);
                            pro1.Dispose();
                        }
                        break;
                    //风格2
                    case "514*288":
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            saveimage(dt.Rows[i]["itempicurl"].ToString() + ".2.jpg", folderPath);
                            proPath = folderPath + "/proTmp.jpg";
                            pro1 = System.Drawing.Image.FromFile(proPath);
                            graphics.DrawImage(pro1, 17 + (i % 5) * 100, 29 + (i / 5 * 130), 80, 80);
                            graphics.DrawString(left(dt.Rows[i]["itemname"].ToString(), 8), font, new SolidBrush(ColorTranslator.FromHtml("#0394EF")), 14 + (i%5) * 100, 112 + (i / 5 * 130));
                            graphics.DrawString(left(dt.Rows[i]["itemname"].ToString(), 8, 16), font, new SolidBrush(ColorTranslator.FromHtml("#0394EF")), 14 + (i % 5) * 100, 126 + (i / 5 * 130));
                            pro1.Dispose();
                        }
                        break;
                    //风格3
                    case "312*288":
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            saveimage(dt.Rows[i]["itempicurl"].ToString() + ".2.jpg", folderPath);
                            proPath = folderPath + "/proTmp.jpg";
                            pro1 = System.Drawing.Image.FromFile(proPath);
                            graphics.DrawImage(pro1, 17 + (i % 3) * 100, 29 + (i / 3 * 130), 80, 80);
                            graphics.DrawString(left(dt.Rows[i]["itemname"].ToString(), 8), font, new SolidBrush(ColorTranslator.FromHtml("#0394EF")), 14 + (i % 3) * 100, 112 + (i / 3 * 130));
                            graphics.DrawString(left(dt.Rows[i]["itemname"].ToString(), 8, 16), font, new SolidBrush(ColorTranslator.FromHtml("#0394EF")), 14 + (i % 3) * 100, 126 + (i / 3 * 130));
                            pro1.Dispose();
                        }
                        break;
                    //风格4
                    case "714*160":
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            saveimage(dt.Rows[i]["itempicurl"].ToString() + ".2.jpg", folderPath);
                            proPath = folderPath + "/proTmp.jpg";
                            pro1 = System.Drawing.Image.FromFile(proPath);
                            graphics.DrawImage(pro1, 17 + i * 100, 29, 80, 80);
                            graphics.DrawString(left(dt.Rows[i]["itemname"].ToString(), 8), font, new SolidBrush(ColorTranslator.FromHtml("#0394EF")), 14 + i * 100, 112);
                            graphics.DrawString(left(dt.Rows[i]["itemname"].ToString(), 8, 16), font, new SolidBrush(ColorTranslator.FromHtml("#0394EF")), 14 + i * 100, 126);
                            pro1.Dispose();
                        }
                        break;
                    //风格5
                    case "114*418":
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            saveimage(dt.Rows[i]["itempicurl"].ToString() + ".2.jpg", folderPath);
                            proPath = folderPath + "/proTmp.jpg";
                            pro1 = System.Drawing.Image.FromFile(proPath);
                            graphics.DrawImage(pro1, 17, 29 + i * 130, 80, 80);
                            graphics.DrawString(left(dt.Rows[i]["itemname"].ToString(), 8), font, new SolidBrush(ColorTranslator.FromHtml("#0394EF")), 14, 112 + i * 130);
                            graphics.DrawString(left(dt.Rows[i]["itemname"].ToString(), 8, 16), font, new SolidBrush(ColorTranslator.FromHtml("#0394EF")), 14, 126 + i * 130);
                            pro1.Dispose();
                        }
                        break;
                    //风格6
                    case "664*160":
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            saveimage(dt.Rows[i]["itempicurl"].ToString() + ".2.jpg", folderPath);
                            proPath = folderPath + "/proTmp.jpg";
                            pro1 = System.Drawing.Image.FromFile(proPath);
                            graphics.DrawImage(pro1, 17 + i * 100, 29, 80, 80);
                            graphics.DrawString(left(dt.Rows[i]["itemname"].ToString(), 8), font, new SolidBrush(ColorTranslator.FromHtml("#0394EF")), 14 + i * 100, 112);
                            graphics.DrawString(left(dt.Rows[i]["itemname"].ToString(), 8, 16), font, new SolidBrush(ColorTranslator.FromHtml("#0394EF")), 14 + i * 100, 126);
                            pro1.Dispose();
                        }
                        break;
                    //风格7
                    case "218*286":
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            saveimage(dt.Rows[i]["itempicurl"].ToString() + ".2.jpg", folderPath);
                            proPath = folderPath + "/proTmp.jpg";
                            pro1 = System.Drawing.Image.FromFile(proPath);
                            graphics.DrawImage(pro1, 17 + (i % 2) * 100, 29 + (i / 2 * 130), 80, 80);
                            graphics.DrawString(left(dt.Rows[i]["itemname"].ToString(), 8), font, new SolidBrush(ColorTranslator.FromHtml("#0394EF")), 14 + (i % 2) * 100, 112 + (i / 2 * 130));
                            graphics.DrawString(left(dt.Rows[i]["itemname"].ToString(), 8, 16), font, new SolidBrush(ColorTranslator.FromHtml("#0394EF")), 14 + (i % 2) * 100, 126 + (i / 2 * 130));
                            pro1.Dispose();
                        }
                        break;
                    //风格7
                    case "743*308":
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            saveimage(dt.Rows[i]["itempicurl"].ToString() + ".3.jpg", folderPath);
                            proPath = folderPath + "/proTmp.jpg";

                            Pen pen = new Pen(new SolidBrush(ColorTranslator.FromHtml("#cccccc")), 1);
                            pen.DashStyle = DashStyle.Solid;
                            Point p0 = new Point(15 + i * 180, 45);
                            Point p1 = new Point(15 + i * 180 + 161, 45 + 161);
                            graphics.DrawRectangle(pen, new Rectangle(p0.X, p0.Y, Math.Abs(p0.X - p1.X), Math.Abs(p0.Y - p1.Y)));
                            //商品
                            pro1 = System.Drawing.Image.FromFile(proPath);
                            graphics.DrawImage(pro1, 16 + i * 180, 46, 160, 160);
                            //购买按钮
                            pro2 = System.Drawing.Image.FromFile(Server.MapPath("/top/show1/buy1.png"));
                            graphics.DrawImage(pro2, 44 + i * 180, 246, 108, 28);
                            //文字
                            font = new Font("宋体", 12);
                            graphics.DrawString(left(dt.Rows[i]["itemname"].ToString(), 0, 12), font, new SolidBrush(ColorTranslator.FromHtml("#3f3f3f")), 22 + i * 180, 212);
                            //价格
                            //product.Price = (decimal.Parse(match[j].Groups[2].ToString()) / 100).ToString();
                            font = new Font("Arial", 12, FontStyle.Bold);
                            graphics.DrawString("￥" + (decimal.Parse(dt.Rows[i]["itemprice"].ToString()) / 100).ToString() + "元", font, new SolidBrush(ColorTranslator.FromHtml("#fe596a")), 66 + i * 180, 228);
                            pro1.Dispose();
                            pro2.Dispose();
                        }
                        //增加店铺链接
                        font = new Font("Arial", 13, FontStyle.Bold);
                        graphics.DrawString("更多详情请见 " + nickid, font, new SolidBrush(ColorTranslator.FromHtml("#ff6600")), 435, 284);
                        break;
                }
            }

            ImageCodecInfo ici;
            System.Drawing.Imaging.Encoder enc;
            EncoderParameter ep;
            EncoderParameters epa;

            //   Initialize   the   necessary   objects   
            ici = GetEncoderInfo("image/png");
            enc = System.Drawing.Imaging.Encoder.Quality;//设置保存质量   
            epa = new EncoderParameters(1);
            //   Set   the   compression   level   
            ep = new EncoderParameter(enc, 100L);//质量等级为25%   
            epa.Param[0] = ep;


            CurrentBitmap.Save(folderPath + "/result_"+id+".jpg", ici, epa);

            epa.Dispose();
            ep.Dispose();
        }

        RecordAndShow(folderPath);
    }

    private void RecordAndShow(string folderPath)
    {
        //记录广告浏览次数
        string sql = "UPDATE TopIdea SET viewcount = viewcount + 1 WHERE id = " + id;
        utils.ExecuteNonQuery(sql);
        //记录浏览日志
        //string url = Request.UrlReferrer == null ? "" : Request.UrlReferrer.ToString();
        //sql = "INSERT INTO TopIdeaLog (ideaid, url) VALUES ('" + id + "', '" + url + "')";
        //utils.ExecuteNonQuery(sql);

        Response.ClearContent();
        Response.ContentType = "image/png";
        Response.BinaryWrite(File.ReadAllBytes(folderPath + "/result_" + id + ".jpg"));
    }

    /// <summary> 
    /// MD5 加密函数 
    /// </summary> 
    /// <param name="str"></param> 
    /// <param name="code"></param> 
    /// <returns></returns> 
    public static string MD5(string str)
    {
        return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").Substring(8, 16);
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

    public static string left(string str, int len)
    {
        if (str.Length < len)
        {
            return str;
        }
        else
        {
            return str.Substring(0, len);
        }
    }

    public static string left(string str, int len, int lenEnd)
    {
        if (str.Length < len)
        {
            return "";
        }
        else
        {
            if (lenEnd < str.Length)
            {
                return str.Substring(len, lenEnd - len);
            }
            else
            {
                return str.Substring(len, str.Length - len);
            }
        }
    }

    public void saveimage(string url, string folderPath)
    {
        WebClient mywebclient = new WebClient();
        string filepath =folderPath + "/proTmp.jpg";
        try
        {
            mywebclient.DownloadFile(url, filepath);
        }
        catch
        {
            
        }
    }

}
