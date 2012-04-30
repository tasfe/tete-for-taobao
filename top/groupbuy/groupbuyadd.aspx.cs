using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;
using System.Net;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Collections;
using System.Web.Security;
using System.Data;
using System.Threading;

public partial class top_groupbuy_groupbuyadd : System.Web.UI.Page
{
    public string startdate = string.Empty;
    public string todate = string.Empty;
    public string enddate = string.Empty;
    public string nowstr = string.Empty;
    public static string logUrl = "D:/svngroupbuy/website/ErrLog";
    public string nick = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Decrypt(taobaoNick);

        Response.Write(cookie.getCookie("top_sessiongroupbuy"));
        string sql = "select enddate from TopTaobaoShop where nick='" + nick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
 
        if (dt != null && dt.Rows.Count > 0)
        {
            shopgroupbuyEnddate.Value = dt.Rows[0]["enddate"].ToString();
        }
        //过期判断
        if (string.IsNullOrEmpty(taobaoNick))
        {
            string msg = "尊敬的淘宝卖家，非常抱歉的告诉您，您还没有购买此服务或者登录信息已失效，如需继续使用请<a href='http://fuwu.taobao.com/serv/detail.htm?service_id=11807' target='_blank'>进入该服务</a>，谢谢！";
            Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
            Response.End();
            return;
        }

       
        FormatData();
    }

    /// <summary>
    /// 根据当前时间初始化页面控件值
    /// </summary>
    private void FormatData()
    {
        nowstr = DateTime.Now.ToString();
        //获取服务器当前日期
        DateTime now = DateTime.Now;
        DateTime to = DateTime.Now.AddDays(1);
        DateTime end = DateTime.Now.AddDays(13);

        startdate = now.Month.ToString() + "/" + now.Day.ToString() + "/" + now.Year.ToString();
        todate = to.Month.ToString() + "/" + to.Day.ToString() + "/" + to.Year.ToString();
        enddate = end.Month.ToString() + "/" + end.Day.ToString() + "/" + end.Year.ToString();

        //选择当前时间节点
        int hour = DateTime.Now.Hour - 1;

        //如果是11点则选择0
        if (hour == 24)
        {
            hour = 0;
        }
        if (hour < 0)
        {
            hour = 0;
        }

        startSelect.SelectedIndex = hour;
        endSelect.SelectedIndex = hour;
    }

    /// <summary>
    /// 完成创建
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        string session = cookie.getCookie("top_sessiongroupbuy");
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Decrypt(taobaoNick);
 
        string sql = "SELECT * FROM TopGroupBuy WHERE nick = '" + nick + "' WHERE  isdelete=0 ORDER BY ID DESC";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt != null && dt.Rows.Count > 10)
        {
            Response.Write("<b>活动创建失败，错误原因：</b><br><font color='red'>您最多只能创建10个活动</font><br>");
            Response.End();
            return;
        }
 

        //后端判断数据但是否合法
        if (DataJudge())
        {
            ShowErr("录入数据不合法");
            return;
        }

        //插入数据库
        string groupbuyname = utils.NewRequest("groupbuylistname", utils.RequestType.Form).Replace("'", "''");
        string starttime = utils.NewRequest("starttime", utils.RequestType.Form) + " " + utils.NewRequest("startSelect", utils.RequestType.Form).Replace("'", "''") + ":00";
        string endtime = utils.NewRequest("endtime", utils.RequestType.Form) + " " + utils.NewRequest("endSelect", utils.RequestType.Form).Replace("'", "''") + ":00";
        string groupbuyprice = utils.NewRequest("price", utils.RequestType.Form);// price
        string productid = utils.NewRequest("productid", utils.RequestType.Form);//productid
        string maxcount = utils.NewRequest("maxcount", utils.RequestType.Form);
        string zhekou = utils.NewRequest("zhekou", utils.RequestType.Form);//zhekou
        string mintime = utils.NewRequest("mintime", utils.RequestType.Form);
        string isfromflash = utils.NewRequest("isfromflash", utils.RequestType.Form);
        string rcount = utils.NewRequest("rcount", utils.RequestType.Form);//rcount
        string template1 = utils.NewRequest("template", utils.RequestType.Form);//
        string xiaogou = utils.NewRequest("xiangou", utils.RequestType.Form);//
 
 
        string[] aryPrice = groupbuyprice.Split(',');
        string[] aryProductid = productid.Split(',');
        string[] aryZhekou = zhekou.Split(',');
        string[] aryRcount = rcount.Split(',');
        string[] aryXiaogou = xiaogou.Split(',');
        string[] aryGroupbuyname = groupbuyname.Split(',');
        string ismuch = "0";//不是多个商品模板
        string groupbuyGuid = "";
        if (template1.Trim() == "")
        {
            template1 = "1";
        }
        if (template1.Trim() != "1")
        {
            //如果不是单个商品模板

            ismuch = "1";
            //如果是多个商品团购模板，设置团购标示
            groupbuyGuid = Guid.NewGuid().ToString();//团购标示
        }


        if (groupbuyprice == "" || aryPrice.Length < 1)
        {
            ShowErr("录入数据不完整");
            return;
        }


        for (int p = 0; p < aryPrice.Length; p++)
        {
            groupbuyname = aryGroupbuyname[p].ToString();
            Thread.Sleep(1000);//一秒一次， 太快 淘宝有限制  
            //通过借口获取淘宝相关数据
            TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", "12287381", "d3486dac8198ef01000e7bd4504601a4");
            ItemGetRequest request = new ItemGetRequest();
            request.Fields = "num_iid,title,price,pic_url";
            request.NumIid = long.Parse(aryProductid[p].ToString());

            Item product = client.ItemGet(request, session);

            if (decimal.Parse(aryZhekou[p].ToString()) >= decimal.Parse(product.Price))
            {
                Response.Write("<b>活动创建失败，错误原因：</b><br><font color='red'>" + aryProductid[p].ToString() + "商品优惠价必须大于零并且小于原价</font><br><a href='groupbuyadd.aspx'>重新添加</a>");
                Response.End();
                return;
            }

            if (decimal.Parse(aryZhekou[p].ToString()) < (decimal.Parse(product.Price) * 0.7m))
            {
                Response.Write("<b>活动创建失败，错误原因：</b><br><font color='red'>" + aryProductid[p].ToString() + "商品优惠价必须大于原价7折并且小于原价</font><br><a href='groupbuyadd.aspx'>重新添加</a>");
                Response.End();
                return;
            }
            string newprice = Math.Round(decimal.Parse(product.Price) - decimal.Parse(aryZhekou[p].ToString()), 2).ToString(); //折扣值

            //创建活动及相关人群
            string appkey = "12287381";
            string secret = "d3486dac8198ef01000e7bd4504601a4";



            //创建活动相关人群
            string guid = Guid.NewGuid().ToString().Substring(0, 4);
            IDictionary<string, string> param = new Dictionary<string, string>();
            // param.Add("tag_name", nick + "_团购人群_" + guid);
            // param.Add("description", nick + "_团购人群描述_" + guid);
            // string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.marketing.tag.add", session, param);
            string tagid = "1"; //new Regex(@"<tag_id>([^<]*)</tag_id>", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
            // WriteLog("添加代码:" + result, "");
            //如果设置的是不需要参团就能购买
            //if (isfromflash == "0")
            //{
            //    tagid = "1";
            //}

            //创建活动
            param = new Dictionary<string, string>();
            param.Add("num_iids", "12241171707");
            param.Add("discount_type", "PRICE");
            param.Add("discount_value", "97");
            param.Add("start_date", "2012-4-29 12:51:36");
            param.Add("end_date", "2012-5-31 12:51:38");
            param.Add("promotion_title", "团购活动");
            param.Add("decrease_num", "1");
             

            param.Add("tag_id", tagid);
            string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.marketing.promotion.add", session, param);

            
            WriteLog("添加代码:" + result, "");
            if (result.IndexOf("error_response") != -1)
            {
                string err = new Regex(@"<sub_msg>([^<]*)</sub_msg>", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
                if (err == "")
                {
                    Response.Write("<b>活动创建失败，错误原因：</b><br><font color='red'>您的session已经失效，需要重新授权</font><br><a href='http://container.api.taobao.com/container?appkey=12287381&scope=promotion' target='_parent'>重新授权</a>");
                    Response.End();
                }

                Response.Write("<b>活动创建失败，错误原因：</b><br><font color='red'>" + err + "</font><br><a href='groupbuyadd.aspx'>重新添加</a>");
                Response.End();
                return;
            }

            string promotionid = new Regex(@"<promotion_id>([^<]*)</promotion_id>", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
           

            string productname = product.Title;
            string productprice = product.Price;
            string productimg = product.PicUrl;
            string producturl = "http://item.taobao.com/item.htm?id=" + aryProductid[p].ToString();


            string time = DateTime.Now.ToString("yyyy-MM-dd");

            //创建文件夹 
            if (!Directory.Exists("D:\\groupbuy.7fshop.com/wwwroot/top/groupbuy/pic/" + time + "/"))
            {
                Directory.CreateDirectory("D:\\groupbuy.7fshop.com/wwwroot/top/groupbuy/pic/" + time + "/");
            }

            string groupbuyimg = "D:\\groupbuy.7fshop.com/wwwroot/top/groupbuy/pic/" + time + "/" + FormsAuthentication.HashPasswordForStoringInConfigFile(productimg, "MD5") + ".GIF";
            string sitegroupbuyimg = "http://groupbuy.7fshop.com/top/groupbuy/pic/" + time + "/" + FormsAuthentication.HashPasswordForStoringInConfigFile(productimg, "MD5") + ".GIF";
            string groupbuyingimg = "D:\\groupbuy.7fshop.com/wwwroot/top/groupbuy/pic/" + time + "/" + FormsAuthentication.HashPasswordForStoringInConfigFile(productimg, "MD5") + "ing.GIF";
            string sitegroupbuyingimg = "http://groupbuy.7fshop.com/top/groupbuy/pic/" + time + "/" + FormsAuthentication.HashPasswordForStoringInConfigFile(productimg, "MD5") + "ing.GIF";
            string groupbuyendimg = "D:\\groupbuy.7fshop.com/wwwroot/top/groupbuy/pic/" + time + "/" + FormsAuthentication.HashPasswordForStoringInConfigFile(productimg, "MD5") + "end.GIF";
            string sitegroupbuyendimg = "http://groupbuy.7fshop.com/top/groupbuy/pic/" + time + "/" + FormsAuthentication.HashPasswordForStoringInConfigFile(productimg, "MD5") + "end.GIF";
            #region

            sql = "INSERT INTO TopGroupBuy (" +
                          "name," +
                          "starttime," +
                          "endtime," +
                          "productname," +
                          "productprice," +
                          "productimg," +
                          "producturl," +
                          "productid," +
                          "nick," +
                          "maxcount," +
                          "buycount," +
                          "tagid," +
                          "promotionid," +
                          "mintime," +
                          "zhekou," +
                          "isfromflash," +
                          "groupbuyImg," +
                          "groupbuyingImg," +
                          "groupbuyendImg," +
                          "groupbyPcount," +
                          "ismuch," +
                          "template," +
                          "groupbuyGuid," +
                          "groupbuyprice" +
                      " ) VALUES ( " +
                          " '" + groupbuyname + "'," +
                          " '" + starttime + "'," +
                          " '" + endtime + "'," +
                          " '" + productname + "'," +
                          " '" + aryPrice[p].ToString() + "'," +
                          " '" + productimg + "'," +
                          " '" + producturl + "'," +
                          " '" + aryProductid[p].ToString() + "'," +
                          " '" + nick + "'," +
                          " '" + maxcount + "'," +
                          " '0'," +
                          " '" + tagid + "'," +
                          " '" + promotionid + "'," +
                          " '" + mintime + "'," +
                          " '" + newprice + "'," +
                          " '" + isfromflash + "'," +
                          " '" + sitegroupbuyimg + "'," +
                          " '" + sitegroupbuyingimg + "'," +
                          " '" + sitegroupbuyendimg + "'," +
                          " '" + aryRcount[p].ToString() + "'," +
                          " " + ismuch + "," +
                          " " + template1 + "," +
                          " '" + groupbuyGuid+ "'," +
                          " '" + aryZhekou[p].ToString() + "'" +
                    ") ";
 
            utils.ExecuteNonQuery(sql);



            //将测试用户加入该活动关联人群组
            //IDictionary<string, string> param = new Dictionary<string, string>();
            //param.Add("tag_id", tagid);
            //param.Add("nick", "美杜莎之心");

            //string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.marketing.taguser.add", session, param);

            //Response.Write(result);

            //查询活动详细
            //IDictionary<string, string> param = new Dictionary<string, string>();
            //param.Add("fields", "num_iids,discount_type,discount_value,start_date,end_date,promotion_title,tag_id");
            //param.Add("num_iid", "7591980225");

            //string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.marketing.promotions.get", session, param);

            //Response.Write(result);

            //更新商品描述关联的广告图片

            try
            {
                CreateGroupbuyImg();

                if (DateTime.Now < DateTime.Parse(starttime))
                {
                    GreateImageGifTG(groupbuyname, "D:\\groupbuy.7fshop.com/wwwroot/top/groupbuy/images/newgroupbuy.png", productprice, zhekou, Math.Round((decimal.Parse(productprice) - decimal.Parse(newprice)) / decimal.Parse(productprice) * 10, 1).ToString(), newprice, DateTime.Parse(starttime), DateTime.Parse(endtime), rcount, maxcount, productimg, groupbuyimg);
                }
                GreateImageGifTG2(groupbuyname, "D:\\groupbuy.7fshop.com/wwwroot/top/groupbuy/images/newgroupbuyin.png", productprice, zhekou, Math.Round((decimal.Parse(productprice) - decimal.Parse(newprice)) / decimal.Parse(productprice) * 10, 1).ToString(), newprice, DateTime.Parse(starttime), DateTime.Parse(endtime), rcount, maxcount, productimg, groupbuyingimg);

                GreateImageGifTG3(groupbuyname, "D:\\groupbuy.7fshop.com/wwwroot/top/groupbuy/images/newgroupbuyend.png", productprice, zhekou, Math.Round((decimal.Parse(productprice) - decimal.Parse(newprice)) / decimal.Parse(productprice) * 10, 1).ToString(), newprice, DateTime.Parse(starttime), DateTime.Parse(endtime), rcount, maxcount, productimg, groupbuyendimg);
            }
            catch(Exception ex)
            {
                WriteLog("" + ex.Message, "1");
            }
        }


        sql = "SELECT TOP 1 ID FROM TopGroupBuy WHERE nick = '" + nick + "' ORDER BY ID DESC";
        #endregion 
        string groupid = utils.ExecuteString(sql);

        Response.Redirect("success.aspx?id=" + groupid);
    }


    /// <summary>
    /// 写日志
    /// </summary>
    /// <param name="value">日志内容</param>
    /// <param name="type">类型 0(成功日志),1(错误日志) 可传空文本默认为0</param>
    /// <returns></returns>
    public static void WriteLog(string message, string type)
    {
        string tempStr = logUrl + "/Groupby" + DateTime.Now.ToString("yyyyMMdd");//文件夹路径
        string tempFile = tempStr + "/Groupbypromotion" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
        if (type == "1")
        {
            tempFile = tempStr + "/GroupbypromotionErr" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
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
    /// 创建活动关联的广告图片
    /// </summary>
    private void CreateGroupbuyImg()
    {
        return;
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

     


    private bool DataJudge()
    {
        return false;
    }

    private bool ActCountJudge()
    {
         string sql = "SELECT * FROM TopGroupBuy WHERE nick = '" + nick + "' ORDER BY ID DESC";
         DataTable dt = utils.ExecuteDataTable(sql);
         if (dt != null && dt.Rows.Count > 10)
         {
             return true;
         }
        return false;
    }

    private void ShowErr(string p)
    {
        return;
    }











    /// <summary> 
    /// 给TOP请求签名 API v2.0 
    /// </summary> 
    /// <param name="parameters">所有字符型的TOP请求参数</param> 
    /// <param name="secret">签名密钥</param> 
    /// <returns>签名</returns> 
    protected static string CreateSign(IDictionary<string, string> parameters, string secret)
    {
        parameters.Remove("sign");
        IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters);
        IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();
        StringBuilder query = new StringBuilder(secret);
        while (dem.MoveNext())
        {
            string key = dem.Current.Key;
            string value = dem.Current.Value;
            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
            {
                query.Append(key).Append(value);
            }
        }
        query.Append(secret);
        MD5 md5 = MD5.Create();
        byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(query.ToString()));
        StringBuilder result = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
        {
            string hex = bytes[i].ToString("X");
            if (hex.Length == 1)
            {
                result.Append("0");
            }
            result.Append(hex);
        }
        return result.ToString();
    }
    /// <summary> 
    /// 组装普通文本请求参数。 
    /// </summary> 
    /// <param name="parameters">Key-Value形式请求参数字典</param> 
    /// <returns>URL编码后的请求数据</returns> 
    protected static string PostData(IDictionary<string, string> parameters)
    {
        StringBuilder postData = new StringBuilder();
        bool hasParam = false;
        IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
        while (dem.MoveNext())
        {
            string name = dem.Current.Key;
            string value = dem.Current.Value;
            // 忽略参数名或参数值为空的参数 
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
            {
                if (hasParam)
                {
                    postData.Append("&");
                }
                postData.Append(name);
                postData.Append("=");
                postData.Append(Uri.EscapeDataString(value));
                hasParam = true;
            }
        }
        return postData.ToString();
    }
    /// <summary> 
    /// TOP API POST 请求 
    /// </summary> 
    /// <param name="url">请求容器URL</param> 
    /// <param name="appkey">AppKey</param> 
    /// <param name="appSecret">AppSecret</param> 
    /// <param name="method">API接口方法名</param> 
    /// <param name="session">调用私有的sessionkey</param> 
    /// <param name="param">请求参数</param> 
    /// <returns>返回字符串</returns> 
    public static string Post(string url, string appkey, string appSecret, string method, string session,
    IDictionary<string, string> param)
    {
        #region -----API系统参数----
        param.Add("app_key", appkey);
        param.Add("method", method);
        param.Add("session", session);
        param.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        param.Add("format", "xml");
        param.Add("v", "2.0");
        param.Add("sign_method", "md5");
        param.Add("sign", CreateSign(param, appSecret));
        #endregion
        string result = string.Empty;
        #region ---- 完成 HTTP POST 请求----
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        req.Method = "POST";
        req.KeepAlive = true;
        req.Timeout = 300000;
        req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
        byte[] postData = Encoding.UTF8.GetBytes(PostData(param));
        Stream reqStream = req.GetRequestStream();
        reqStream.Write(postData, 0, postData.Length);
        reqStream.Close();
        HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
        Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
        Stream stream = null;
        StreamReader reader = null;
        stream = rsp.GetResponseStream();
        reader = new StreamReader(stream, encoding);
        result = reader.ReadToEnd();
        if (reader != null) reader.Close();
        if (stream != null) stream.Close();
        if (rsp != null) rsp.Close();
        #endregion
        return Regex.Replace(result, @"[\x00-\x08\x0b-\x0c\x0e-\x1f]", "");
    }
}