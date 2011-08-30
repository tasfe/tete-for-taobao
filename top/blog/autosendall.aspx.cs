using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using Common;
using System.Data;
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;
using Common;

public partial class top_blog_autosendall : System.Web.UI.Page
{
    public string vtoken = string.Empty;
    public string cookie1 = string.Empty;
    public string cookie2 = string.Empty;
    public string key = string.Empty;
    public int index;

    public string title = string.Empty;
    public string content = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string sql = "SELECT * FROM TopBlogAuto WHERE isauto = 1 AND keywords <> '' AND uids <> ''";
        DataTable dt = utils.ExecuteDataTable(sql);
        string num = string.Empty;
        string used = string.Empty;

        //循环遍历自动发送
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            index = i;
            //判断这个会员的会员等级
            sql = "SELECT * FROM TopTaobaoShop WHERE nick = '" + dt.Rows[i]["nick"].ToString() + "'";
            DataTable dt2 = utils.ExecuteDataTable(sql);
            if (dt2.Rows.Count != 0)
            {
                string version = dt2.Rows[0]["versionNo"].ToString();
                switch (version)
                {
                    case "1":
                        num = "10";
                        break;
                    case "2":
                        num = "30";
                        break;
                    case "3":
                        num = "100";
                        break;
                    default:
                        num = "10";
                        break;
                }
            }

            //先判断这个会员发送的文章是否到上限
            sql = "SELECT COUNT(*) FROM TopBlog WHERE nick = '" + dt.Rows[i]["nick"].ToString() + "' AND DATEDIFF(d,GETDATE(),ADDDATE) = 0 AND sendStatus <> 2";
            DataTable dt1 = utils.ExecuteDataTable(sql);
            used = dt1.Rows[0][0].ToString();

            if (int.Parse(used) >= int.Parse(num))
            {
                continue;
            }

            //再判断是否手动发送过，如果手动发送过则不启动自动发送
            sql = "SELECT COUNT(*) FROM TopBlog WHERE nick = '" + dt.Rows[i]["nick"].ToString() + "' AND DATEDIFF(d,GETDATE(),ADDDATE) = 0 AND isauto = 0";
            dt1 = utils.ExecuteDataTable(sql);
            used = dt1.Rows[0][0].ToString();

            if (used != "0")
            {
                continue;
            }

            //根据关联帐号个数开始发送
            string[] uidList = dt.Rows[i]["uids"].ToString().Split(',');
            for (int j = 0; j < uidList.Length; j++)
            {
                AutoSendBlog(uidList[j], dt);
            }
        }
    }

    /// <summary>
    /// 自动发送博文
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="dt"></param>
    private void AutoSendBlog(string uid, DataTable sendInfo)
    {
        //根据帐号获取密码
        string sql = "SELECT * FROM TopBlogAccountNew WHERE uid = '" + uid + "' AND typ = 'sina'";
        DataTable dt = utils.ExecuteDataTable(sql);

        if (dt.Rows.Count > 0)
        {
            string pass = dt.Rows[0]["pass"].ToString();
            Response.Write("使用帐号" + uid + "自动发送中...<br>");
            //发送
            SendBlogByAccount(uid, pass, sendInfo);
        }
        else
        {
            return;
        }
    }

    private string SendBlogByAccount(string uid, string pass, DataTable sendInfo)
    {
        //准备生成
        string strHtml = string.Empty;

        StreamReader sr = null; //用来读取流
        StreamWriter sw = null; //用来写文件
        Encoding code = Encoding.GetEncoding("utf-8"); //定义编码 
        ASCIIEncoding encoding = new ASCIIEncoding();
        //构造POST请求
        string postData = "service=sso&client=ssologin.js%28v1.3.9%29&entry=blog&encoding=utf-8&gateway=1&savestate=0&from=&useticket=0&username=" + uid + "&password=" + pass + "";
        byte[] data = code.GetBytes(postData);
        //构造web请求，发送请求，获取响应
        HttpWebRequest HttpWebRequest = null;
        string url = "http://login.sina.com.cn/sso/login.php?client=ssologin.js(v1.3.9)";
        HttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        HttpWebRequest.Method = "POST";
        HttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; InfoPath.2; .NET4.0C; .NET4.0E)";
        HttpWebRequest.ContentType = "application/x-www-form-urlencoded";
        HttpWebRequest.ContentLength = data.Length;

        Stream newStream = HttpWebRequest.GetRequestStream();
        newStream.Write(data, 0, data.Length);
        newStream.Close();
        //发送
        WebResponse HttpWebResponse = null;
        HttpWebResponse = HttpWebRequest.GetResponse();
        string aaa = HttpWebResponse.Headers.Get("Set-Cookie").ToString();
        //获得流
        sr = new StreamReader(HttpWebResponse.GetResponseStream(), code);
        strHtml = sr.ReadToEnd();

        string cookieStr = string.Empty;
        cookieStr += getKeyData(@"SUE=([^;]*);", aaa);
        cookieStr += getKeyData(@"SUP=([^;]*);", aaa);
        cookieStr += getKeyData(@"domain=([^;]*);", aaa);
        cookieStr += getKeyData(@"tgc=([^;]*);", aaa);

        cookie1 = cookieStr;


        Response.Write("使用帐号" + uid + "获取文章信息中...<br>");
        return GetArticleInfo(sendInfo);
    }

    private string SendBlog(string uid, DataTable sendInfo)
    {
        AddBlogCookie();


        //string title = this.tbTitle.Text;
        //string content = HttpUtility.HtmlDecode(this.FCKeditor1.Text);
        string date = DateTime.Now.ToString("yyyy-MM-dd");
        string time = DateTime.Now.ToString("HH:mm:ss");

        //替换关键字连接
        //content = changeKey(content);
        content = HttpUtility.UrlEncode(content);

        //拼装COOKIE
        //Common.Cookie cookie = new Common.Cookie();
        //string sinaCookie = cookie.getCookie("sinaCookie");
        string trueSinaCookie = cookie1;// +cookie2;// +cookie.getCookie("sinaCookieAdd").Replace("|", ";");

        //准备生成
        string strHtml = string.Empty;

        StreamReader sr = null; //用来读取流
        StreamWriter sw = null; //用来写文件
        Encoding code = Encoding.GetEncoding("utf-8"); //定义编码 
        ASCIIEncoding encoding = new ASCIIEncoding();
        //构造POST请求
        string postData = "ptype=&teams=&worldcuptags=&album=&album_cite=&blog_id=&is_album=0&stag=&sno=&book_worksid=&channel_id=&url=&channel=&newsid=&fromuid=&wid=&articletj=&vtoken=" + vtoken + "&is_media=0&is_stock=0&is_tpl=0&assoc_article=&assoc_style=1&assoc_article_data=&article_BGM=&xRankStatus=&commentGlobalSwitch=&commenthideGlobalSwitch=&articleStatus_preview=1&source=&topic_id=0&topic_channel=0&topic_more=&utf8=1&date_pub=&blog_title=" + title + "&time=&blog_body=" + content + "&blog_class=00&tag=&x_cms_flag=0&sina_sort_id=117&join_circle=1";
        byte[] data = code.GetBytes(postData);
        //构造web请求，发送请求，获取响应
        HttpWebRequest HttpWebRequest = null;
        string url = "http://control.blog.sina.com.cn/admin/article/article_post.php";
        HttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        HttpWebRequest.Method = "POST";
        HttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; InfoPath.2; .NET4.0C; .NET4.0E)";
        HttpWebRequest.ContentType = "application/x-www-form-urlencoded";
        HttpWebRequest.ContentLength = data.Length;
        HttpWebRequest.Referer = "http://control.blog.sina.com.cn/admin/article/article_add.php";
        //HttpWebRequest.Headers.Set("Cookie", "EditorToolType=base; UOR=www.baidu.com,blog,; SINAGLOBAL=121.11.253.221.135441289973113118; ULV=1290068456224:6:6:6:121.11.253.221.67501290065988660:1290067272974; vjuids=1abca039b.12c5865e028.0.9fd135f2d15328; vjlast=1289973260.1290066021.13; ALLYESID4=00101117135324204575428; FocusMediaIpCgiCookie=%u4E0A%u6D77%7C%7C%u4E0A%u6D77; FocusMediaRotatorInputCookie=12; FocusMediaRotatorCookie=14; Apache=121.11.253.221.67501290065988660; _s_upa=8; PHPSESSID=928ab7e0a9809f461df8907c6b16d207; SUE=es%3D791918fdc6ef7b2feb43f8106fdbb22c%26ev%3Dv0%26es2%3Da006e7bd85d7ca9e820b661097199b3e; SUP=cv%3D1%26bt%3D1290068513%26et%3D1290154913%26lt%3D1%26uid%3D1665537805%26user%3Dgolddonkey%2540126.com%26ag%3D4%26name%3Dgolddonkey%2540126.com%26nick%3Dgolddonkey%26sex%3D%26ps%3D0%26email%3D%26dob%3D%26ln%3Dgolddonkey%2540126.com; ucMemList_1665537805=; SessionID=a4a0b37f3dfd01d55a692d575157f1f7; SINABLOGNUINFO=1665537805.6346170d.tetesoft; SSCSum=3; SinaRot//=9; tblogt=0; BILS=c; CoupletMediahttp://blog.sina.com.cn/=0; rpb_1_1=1290068472146; iCast_Rotator_1_2=1290066012958; ad680=1290121200568;");
        //HttpWebRequest.Headers.Set("Cookie", " SUE=es%3D791918fdc6ef7b2feb43f8106fdbb22c%26ev%3Dv0%26es2%3Da006e7bd85d7ca9e820b661097199b3e; SUP=cv%3D1%26bt%3D1290068513%26et%3D1290154913%26lt%3D1%26uid%3D1665537805%26user%3Dgolddonkey%2540126.com%26ag%3D4%26name%3Dgolddonkey%2540126.com%26nick%3Dgolddonkey%26sex%3D%26ps%3D0%26email%3D%26dob%3D%26ln%3Dgolddonkey%2540126.com; ");
        //HttpWebRequest.Headers.Set("Cookie", "SUE=es%3D13f20d779254ea584d3779e08e09258b%26ev%3Dv0%26es2%3D949cd07b74e1e7b7d546532a5cad7f4b;SUP=cv%3D1%26bt%3D1290147276%26et%3D1290233676%26lt%3D1%26uid%3D1665537805%26user%3Dgolddonkey%2540126.com%26ag%3D4%26name%3Dgolddonkey%2540126.com%26nick%3Dgolddonkey%26sex%3D%26ps%3D0%26email%3D%26dob%3D%26ln%3Dgolddonkey%2540126.com; ");
        HttpWebRequest.Headers.Set("Cookie", trueSinaCookie);

        Stream newStream = HttpWebRequest.GetRequestStream();
        newStream.Write(data, 0, data.Length);
        newStream.Close();
        //发送
        WebResponse HttpWebResponse = null;
        HttpWebResponse = HttpWebRequest.GetResponse();
        //获得流
        sr = new StreamReader(HttpWebResponse.GetResponseStream(), code);
        strHtml = sr.ReadToEnd();

        //获取文章地址
        string address = new Regex(@"""data"":""([^""]*)""", RegexOptions.IgnoreCase).Match(strHtml).Groups[1].ToString();
        address = "http://blog.sina.com.cn/s/blog_" + address + ".html";

        if (address != "http://blog.sina.com.cn/s/blog_.html")
        {
            Response.Write("使用帐号" + uid + "发送成功！" + address + "...<br>");
            //插入数据库
            RecordData(address, uid, sendInfo.Rows[index]["nick"].ToString());

            return address;
            //Response.Redirect("success.aspx?adr=" + HttpUtility.UrlEncode(address));
        }
        else
        {
            //{"code":"B00010","data":null}为博客暂未开通

            Response.Write("使用帐号" + uid + "发送失败！... " + strHtml + "<br>");
            return "";
            //lbErrMessage.Text = "<font color=red>文章发送失败，请检查您的帐号密码是否正确，或稍等一会再重新发送!!</font> <br> <!-- [" + strHtml + "] -->";
        }
    }

    private string GetArticleInfo(DataTable sendInfo)
    {
        //判断这个会员的会员等级
        string sqlaaa = "SELECT * FROM TopTaobaoShop WHERE nick = '" + sendInfo.Rows[index]["nick"].ToString() + "'";
        string num = string.Empty;
        DataTable dt2 = utils.ExecuteDataTable(sqlaaa);
        if (dt2.Rows.Count != 0)
        {
            string version = dt2.Rows[0]["versionNo"].ToString();
            switch (version)
            {
                case "1":
                    num = "10";
                    break;
                case "2":
                    num = "30";
                    break;
                case "3":
                    num = "100";
                    break;
                default:
                    num = "10";
                    break;
            }
        }

        //先判断这个会员发送的文章是否到上限
        sqlaaa = "SELECT COUNT(*) FROM TopBlog WHERE nick = '" + sendInfo.Rows[index]["nick"].ToString() + "' AND DATEDIFF(d,GETDATE(),ADDDATE) = 0 AND sendStatus <> 2";
        DataTable dt1 = utils.ExecuteDataTable(sqlaaa);
        string used = dt1.Rows[0][0].ToString();

        Response.Write(used + "-" + sendInfo.Rows[index]["nick"].ToString() + "<br>");

        if (int.Parse(used) >= int.Parse(num))
        {
            return "";
        }

        //拆分关键字
        string keyword = sendInfo.Rows[index]["keywords"].ToString().Split(',')[0];

        //根据关键字获取搜索结果页
        string url = "http://uni.sina.com.cn/c.php?k=" + HttpUtility.UrlEncode(keyword) + "&t=blog&ts=bpost&stype=title";
        string strHtml = getUrl(url, "gb2312");
        //取第2页
        url = "http://uni.sina.com.cn/c.php?k=" + HttpUtility.UrlEncode(keyword) + "&t=blog&ts=bpost&stype=title&page=2";
        strHtml += getUrl(url, "gb2312");

        Regex reg = new Regex(@"<span class=""title01""><a target=""_blank"" href=""([^""]*)"">([\s\S]*?)</a></span>", RegexOptions.IgnoreCase);
        MatchCollection match = reg.Matches(strHtml);

        for (int j = 0; j < match.Count; j++)
        {
            //清空
            title = string.Empty;
            content = string.Empty;

            title = match[j].Groups[2].ToString().Replace("<strong>", "").Replace("</strong>", "");

            //如果该博文已发过则过滤掉
            string sqlBlog = "SELECT COUNT(*) FROM TopBlog WHERE nick = '" + sendInfo.Rows[index]["nick"].ToString() + "' AND blogtitle = '" + title + "'";
            string blogCount = utils.ExecuteString(sqlBlog);
            if (blogCount != "0")
            {
                continue;
            }

            Response.Write("文章标题--" + title + "...<br>");

            //根据搜索结果页获取文章内容
            string articleUrl = match[j].Groups[1].ToString();
            strHtml = getUrl(articleUrl, "utf-8");

            //对文章内容进行处理
            Regex regContent = new Regex(@"<div id=""sina_keyword_ad_area2"" class=""[^""]*"">([\s\S]*?)</div>[\s]*<!-- 正文结束 -->", RegexOptions.IgnoreCase);
            MatchCollection matchArticle = regContent.Matches(strHtml);

            //替换博客里面的图片
            string str = matchArticle[0].Groups[1].ToString();
            Regex regImg = new Regex(@"<img([\s\S]*?)src=""([^""]*)"" real_src =""([^""]*)""", RegexOptions.IgnoreCase);
            str = regImg.Replace(str, @"<img$1src=""$3""");

            //替换掉博客内容里面的连接
            Regex regLink = new Regex(@"<a[^>]*>([\s\S]*?)</a>", RegexOptions.IgnoreCase);
            str = regLink.Replace(str, "$1");

            //头部增加广告文件
            string sqlAds = "SELECT TOP 1 * FROM TOPidea WHERE nick = '" + sendInfo.Rows[index]["nick"].ToString() + "'";
            string nickid = string.Empty;
            DataTable dtAds = utils.ExecuteDataTable(sqlAds);
            if (dtAds.Rows.Count != 0)
            {
                //内容头部增加特特广告图片
                string sqlNew = "SELECT sid FROM TopTaobaoShop WHERE nick = '" + sendInfo.Rows[index]["nick"].ToString() + "'";
                DataTable dtNew = utils.ExecuteDataTable(sqlNew);
                if (dtNew.Rows.Count != 0)
                {
                    nickid = "http://shop" + dtNew.Rows[0]["sid"].ToString() + ".taobao.com/";
                }
                else
                {
                    nickid = "http://www.taobao.com/";
                }
                //string nickidEncode = "http://www.7fshop.com/click/?s=" + EncodeStr(new string[] { ads, "0", nickid });
                str = "<a href='" + nickid + "' target='_blank'><img src='http://www.7fshop.com/show/html2jpg.aspx?id=" + dtAds.Rows[0]["id"].ToString() + "' border=0 /></a><br>" + str;
            } 

            //替换掉博客文章里面的关键字
            string sql = "SELECT * FROM TopBlogLink WHERE nick = '" + sendInfo.Rows[index]["nick"].ToString() + "'";
            DataTable dt = utils.ExecuteDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["link"].ToString() != "" && dt.Rows[i]["keyword"].ToString() != "")
                {
                    str = str.Replace(dt.Rows[i]["keyword"].ToString(), "<a href='" + dt.Rows[i]["link"].ToString() + "' target='_blank'>" + dt.Rows[i]["keyword"].ToString() + "</a>");
                }
            }

            content = str;

            //Response.Write(content);
            //break;
            //return "";

            SendBlog(sendInfo.Rows[index]["nick"].ToString(), sendInfo);
            return "";
        }

        return "";
    }

    private string getUrl(string url, string codeStr)
    {
        //准备生成
        string strHtml = string.Empty;
        StreamReader sr = null; //用来读取流
        Encoding code = Encoding.GetEncoding(codeStr); //定义编码 
        ASCIIEncoding encoding = new ASCIIEncoding();
        //构造web请求，发送请求，获取响应
        WebRequest HttpWebRequest = null;
        HttpWebRequest = WebRequest.Create(url);
        //发送
        WebResponse HttpWebResponse = null;
        HttpWebResponse = HttpWebRequest.GetResponse();
        //获得流
        sr = new StreamReader(HttpWebResponse.GetResponseStream(), code);
        strHtml = sr.ReadToEnd();

        return strHtml;
    }

    //private string changeKey(string content)
    //{
    //    Common.Cookie cookie1 = new Common.Cookie();
    //    string taobaoNick1 = cookie1.getCookie("nick");
    //    string nickid = string.Empty;

    //    //COOKIE过期判断
    //    if (taobaoNick1 == "")
    //    {
    //        //SESSION超期 跳转到登录页
    //        Response.Write("<script>parent.location.href='http://container.open.taobao.com/container?appkey=12159997'</script>");
    //        Response.End();
    //    }
    //    Rijndael_ encode = new Rijndael_("tetesoft");
    //    taobaoNick1 = encode.Decrypt(taobaoNick1);
    //    string sqlNew = "SELECT sid FROM TopTaobaoShop WHERE nick = '" + taobaoNick1 + "'";
    //    DataTable dtNew = utils.ExecuteDataTable(sqlNew);
    //    if (dtNew.Rows.Count != 0)
    //    {
    //        nickid = "http://shop" + dtNew.Rows[0]["sid"].ToString() + ".taobao.com/";
    //    }
    //    else
    //    {
    //        nickid = "http://www.taobao.com/";
    //    }

    //    string[] arr = key.Split(' ');

    //    for (int i = 0; i < arr.Length; i++)
    //    {
    //        content = content.Replace(arr[i], "<a href='" + nickid + "' target='_blank'>" + arr[i] + "</a>");
    //    }

    //    //内容底部增加淘宝商品链接
    //    content += BindDataProduct();

    //    return content;
    //}

    private void AddBlogCookie()
    {
        string trueSinaCookie = cookie1; // sinaCookie.Replace("|", ";");

        //准备生成
        string strHtml = string.Empty;
        StreamReader sr = null; //用来读取流
        Encoding code = Encoding.GetEncoding("utf-8"); //定义编码 
        ASCIIEncoding encoding = new ASCIIEncoding();
        //构造web请求，发送请求，获取响应
        HttpWebRequest HttpWebRequest = null;
        HttpWebRequest = (HttpWebRequest)WebRequest.Create("http://control.blog.sina.com.cn/admin/article/article_add.php?index");
        HttpWebRequest.Headers.Set("Cookie", trueSinaCookie);
        //发送
        WebResponse HttpWebResponse = null;
        HttpWebResponse = HttpWebRequest.GetResponse();
        //获得流
        sr = new StreamReader(HttpWebResponse.GetResponseStream(), code);
        strHtml = sr.ReadToEnd();

        vtoken = new Regex(@"<input type=""hidden"" name=""vtoken"" value=""([^""]*)""/>", RegexOptions.IgnoreCase).Match(strHtml).Groups[1].ToString();
    }

    /// <summary>
    /// 数据库记录
    /// </summary>
    private void RecordData(string url, string uid, string taobaoNick)
    {
        string sql = "INSERT INTO TopBlog (" +
                                "blogtitle, " +
                                "blogurl, " +
                                "nick, " +
                                "status, " +
                                "uid, " +
                                "isauto, " +
                                "typ" +
                            " ) VALUES ( " +
                                " '" + title.Replace("'", "''") + "', " +
                                " '" + url + "', " +
                                " '" + taobaoNick + "', " +
                                " '1', " +
                                " '" + uid + "', " +
                                " '1', " +
                                " 'sina' " +
                          ") ";

        utils.ExecuteNonQuery(sql);


        //记录该帐号发送文章数量
        sql = "UPDATE TopBlogAccountNew SET count = count + 1 WHERE uid = '" + uid + "'";
        utils.ExecuteNonQuery(sql);
    }

    private string getKeyData(string pat, string str)
    {
        string verify = string.Empty;
        Regex reg = new Regex(pat);
        if (reg.IsMatch(str))
        {
            Match match = reg.Match(str);
            verify = match.Groups[0].ToString();
        }
        return verify;
    }
}