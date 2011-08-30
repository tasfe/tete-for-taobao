using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;
using QWeiboSDK;
using Oauth4Web;
using LeoShi.Soft.OpenSinaAPI;
using System.Web;
using System.Net;
using System.IO;
using System.Web.Security;
using System.Drawing.Imaging;

namespace tetegroupbuyMicroBlog
{
    public partial class Form1 : Form
    {
        private string baseurl = string.Empty;
        private string tokenKey = string.Empty;
        private string tokenSecret = string.Empty; 

        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;

            baseurl = "http://groupbuy.7fshop.com"; 

            //新品上架自动发微博
            Thread newThread = new Thread(CheckNew);
            newThread.Start();

            ////宝贝售出自动发微博
            //Thread newThread1 = new Thread(CheckNew1);
            //newThread1.Start();

            ////买家好评自动发微博-权限不够
            //Thread newThread2 = new Thread(CheckNew2);
            //newThread2.Start();

            ////橱窗推荐自动发微博
            //Thread newThread3 = new Thread(CheckNew3);
            //newThread3.Start();
        }

        /// <summary>
        /// 宝贝售出自动发微博
        /// </summary>
        private void CheckNew1()
        {
            string appkey = "12159997";
            string secret = "614e40bfdb96e9063031d1a9e56fbed5";
            TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", appkey, secret);
            string session = string.Empty;

            DBSql db = new DBSql();
            string sql = "SELECT b.*, s.sessionblog FROM TopMicroBlogAuto b INNER JOIN TopTaobaoShop s ON s.nick = b.nick";
            textBox1.AppendText("\r\n" + sql);
            DataTable dt = db.GetTable(sql);
            textBox1.AppendText("\r\n" + dt.Rows.Count.ToString());
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                session = dt.Rows[i]["sessionblog"].ToString();

                textBox1.AppendText("\r\n" + session);
                TradesSoldIncrementGetRequest request = new TradesSoldIncrementGetRequest();
                request.Fields = "orders.title,orders.pic_path,orders.price,orders.num_iid";
                request.PageSize = 200;
                request.PageNo = 1;
                request.StartModified = DateTime.Now.AddHours(-1);
                request.EndModified = DateTime.Now;
                try
                {
                    PageList<Trade> product = client.TradesSoldIncrementGet(request, session);
                    if (product.Content.Count != 0)
                    {
                        for (int j = 0; j < product.Content.Count; j++)
                        {
                            string str = CreateContentNew(dt.Rows[i]["content2"].ToString(), product.Content[j].Orders[0]);

                            textBox1.AppendText("\r\n" + str);
                            SendMicroBlog(dt.Rows[i]["nick"].ToString(), str, product.Content[j].Orders[0].PicPath, "2");
                            Thread.Sleep(10000);
                        }
                    }
                }
                catch
                {
                    //SESSION失效
                    textBox1.AppendText("\r\nsession失效");
                    continue;
                }
            }
            //买家好评自动发微博-权限不够
            Thread newThread2 = new Thread(CheckNew2);
            newThread2.Start();
        }

        /// <summary>
        /// 买家好评自动发微博
        /// </summary>
        private void CheckNew2()
        {
            string appkey = "12159997";
            string secret = "614e40bfdb96e9063031d1a9e56fbed5";
            TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", appkey, secret);
            string session = string.Empty;

            DBSql db = new DBSql();
            string sql = "SELECT b.*, s.sessionblog FROM TopMicroBlogAuto b INNER JOIN TopTaobaoShop s ON s.nick = b.nick";
            textBox1.AppendText("\r\n" + sql);
            DataTable dt = db.GetTable(sql);
            textBox1.AppendText("\r\n" + dt.Rows.Count.ToString());
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                session = dt.Rows[i]["sessionblog"].ToString();
                textBox1.AppendText("\r\n" + session);
                TraderatesGetRequest request = new TraderatesGetRequest();
                request.Fields = "tid";
                request.PageSize = 20;
                request.PageNo = 1;
                request.RateType = "get";
                request.Role = "seller";
                try
                {
                    PageList<TradeRate> traderate = client.TraderatesGet(request, session);
                    if (traderate.Content.Count != 0)
                    {
                        //获取商品详细信息
                        for (int j = 0; j < traderate.Content.Count; j++)
                        {
                            TradeFullinfoGetRequest request1 = new TradeFullinfoGetRequest();
                            request1.Fields = "orders.title,orders.pic_path,orders.price,orders.num_iid";

                            request1.Tid = traderate.Content[j].Tid;
                            Trade product = client.TradeFullinfoGet(request1, session);

                            string str = CreateContentNew(dt.Rows[i]["content3"].ToString(), product.Orders[0]);

                            textBox1.AppendText("\r\n" + str);
                            SendMicroBlog(dt.Rows[i]["nick"].ToString(), str, product.Orders[0].PicPath, "3");
                            Thread.Sleep(10000);
                        }
                    }
                }
                catch
                {
                    //SESSION失效
                    textBox1.AppendText("\r\nsession失效");
                    continue;
                }
            }

            
            this.Dispose();
            this.Close();

            Application.Exit();
            Application.ExitThread();

            GC.Collect();
        }


        /// <summary>
        /// 活动开始自动发微博
        /// </summary>
        private void CheckNew()
        {
            string appkey = "12159997";
            string secret = "614e40bfdb96e9063031d1a9e56fbed5";
            TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", appkey, secret);
            string session = string.Empty;


            DBSql db = new DBSql();
            string sql = "SELECT b.*, s.sessionblog FROM TopMicroBlogAuto b INNER JOIN TopTaobaoShop s ON s.nick = b.nick "; //查询用户
            textBox1.AppendText("\r\n" + sql);
            DataTable dt = db.GetTable(sql);
            DataTable dtProduct = null;
 
            //textBox1.AppendText("\r\n" + dt.Rows.Count.ToString());
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sql = "SELECT   id, [name] ,productname ,productprice ,productimg ,producturl ," +
                        "[productid] ,[nick]  ,[buycount] ,[groupbuyprice] ,[tagid] ,[promotionid],mintime ," +
                        "[updatedate] ,[lastupdate]  ,[isdelete] ,[isfromflash],isSend  FROM [TopGroupBuy] WHERE nick='" +
                        dt.Rows[i]["nick"].ToString() + "' AND (isSend =0 OR isSend is null )"; //查询用户团购开始商品

                dtProduct = db.GetTable(sql);


                if (dtProduct != null && dtProduct.Rows.Count > 0)
                {
                    for (int j = 0; j < dtProduct.Rows.Count; j++)
                    {
                        string str = CreateContent(dt.Rows[i]["content1"].ToString(), dtProduct.Rows[j]["name"].ToString(), dtProduct.Rows[j]["groupbuyprice"].ToString(), dtProduct.Rows[j]["productid"].ToString());

                        textBox1.AppendText("\r\n" + i.ToString() + "、" + dt.Rows[i]["nick"].ToString() + "-" + str);
                        SendMicroBlog(dt.Rows[i]["nick"].ToString(), str, dtProduct.Rows[j]["productimg"].ToString(), "1");

                        sql = "update [TopGroupBuy] set isSend=1 where id='" + dtProduct.Rows[j]["id"].ToString() + "'";
                        db.ExecSql(sql);
                        Thread.Sleep(10000);
                    }
                }
            }


            
            ////宝贝售出自动发微博
            Thread newThread1 = new Thread(CheckNew1);
            newThread1.Start();
        }

        private string CreateContent(string str, string title,string price,string numlid)
        {
            str = str.Replace("[宝贝标题]", title);
            str = str.Replace("[宝贝价格]", price);
            str = str.Replace("[宝贝链接]", "http://groupbuy.7fshop.com/redirect/?" + numlid);

            return str;
        }

        private string CreateContentNew(string str, Order item)
        {
            str = str.Replace("[宝贝标题]", item.Title);
            str = str.Replace("[宝贝价格]", item.Price);
            str = str.Replace("[宝贝链接]", "http://groupbuy.7fshop.com/redirect/?" + item.NumIid);

            return str;
        }

        private void SendMicroBlog(string nick, string content, string filepath, string index)
        {
            //过滤过长的内容
            if (content.Length > 140)
            {
                content = content.Substring(0, 140);
            }

            string appKey = "d3225497956249cbb13a7cb7375d62bd";
            string appSecret = "6cf7a3274cb676328e77dff3e203061d";
            string sql = "SELECT * FROM TopMicroBlogAccount WHERE nick = '" + nick + "'";
            DBSql db = new DBSql();
            DataTable dt = db.GetTable(sql);

            textBox1.AppendText("\r\n" + dt.Rows.Count.ToString());
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try
                {
                    if (dt.Rows[i]["typ"].ToString() == "qq")
                    {
                        
                        //MessageBox.Show(content);
                        //发送微博
                        List<Parameter> parameters = new List<Parameter>();
                        parameters.Add(new Parameter("content", content));

                        //身份验证
                        OauthKey oauthKey = new OauthKey();
                        oauthKey.customKey = appKey;
                        oauthKey.customSecrect = appSecret;
                        oauthKey.tokenKey = dt.Rows[i]["tokenKey"].ToString();
                        oauthKey.tokenSecrect = dt.Rows[i]["tokenSecrect"].ToString();

                        //图片信息
                        List<Parameter> files = new List<Parameter>();
                        if (filepath != "")
                        {
                            files.Add(new Parameter("pic", DownPic(filepath)));
                        }

                        QWeiboRequest request = new QWeiboRequest();
                        int nKey = 0;
                        if (request.AsyncRequest("http://open.t.qq.com/api/t/add_pic", "POST", oauthKey, parameters, files, new AsyncRequestCallback(RequestCallback), out nKey))
                        {
                            //textOutput.Text = "请求中...";
                        }

                        sql = "UPDATE TopMicroBlogAuto SET num" + index + " = num" + index + " + 1 WHERE nick = '" + dt.Rows[i]["nick"].ToString() + "'";
                        db.ExecSql(sql);
                        textBox1.AppendText("\r\n" + sql);

                        sql = "INSERT INTO TopMicroBlogSendLog (result, nick, uid, typ, content, auto) VALUES ('','" + nick + "','" + dt.Rows[i]["uid"].ToString() + "','qq','" + content + "','" + index + "')";
                        db.ExecSql(sql);
                        textBox1.AppendText("\r\nqq-" + sql);
                    }
                    else if (dt.Rows[i]["typ"].ToString() == "sina")
                    {

                        string url = string.Empty;
                        string result = string.Empty;
                        HttpPost httpRequest = HttpRequestFactory.CreateHttpRequest(Method.POST) as HttpPost;
                        httpRequest.AppKey = "1421367737";
                        httpRequest.AppSecret = "2be4da41eb329b6327b7b2ac56ffbe6e";
                        httpRequest.Token =  dt.Rows[i]["tokenKey"].ToString();
                        httpRequest.TokenSecret =dt.Rows[i]["tokenSecrect"].ToString();
                        if (filepath != "")
                        {
                            url = "http://api.t.sina.com.cn/statuses/upload.xml?";
                            result = httpRequest.RequestWithPicture(url, "status=" + HttpUtility.UrlPathEncode(content.Replace("=", "")), DownPic(filepath));
                            //result = httpRequest.Request(url, "status=" + HttpUtility.UrlEncode(content,Encoding.UTF8));
                        }
                        else
                        {
                            url = "http://api.t.sina.com.cn/statuses/update.xml?";
                            result = httpRequest.Request(url, "status=" + HttpUtility.UrlEncode(content, Encoding.UTF8));
                        }

                        sql = "UPDATE TopMicroBlogAuto SET num" + index + " = num" + index + " + 1 WHERE nick = '" + dt.Rows[i]["nick"].ToString() + "'";
                        db.ExecSql(sql);
                        textBox1.AppendText("\r\n" + sql);

                        sql = "INSERT INTO TopMicroBlogSendLog (result, nick, uid, typ, content, auto) VALUES ('','" + nick + "','" + dt.Rows[i]["uid"].ToString() + "','sina','" + content + "','" + index + "')";
                        db.ExecSql(sql);

                        textBox1.AppendText("\r\nsina-" + sql);
                    }

                }
                catch (Exception e)
                {
                    textBox1.AppendText("\r\n" + e.Message);
                    textBox1.AppendText("\r\n" + e.StackTrace);
                    continue;
                }
            }

        }


        private string DownPic(string url)
        {
            string strHtml = string.Empty;
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream reader = response.GetResponseStream();

            string time = DateTime.Now.ToString("yyyy-MM-dd");

            //创建文件夹
            if (!Directory.Exists("pic/" + time + "/"))
            {
                Directory.CreateDirectory("pic/" + time + "/");
            }

            string fileName = "pic/" + time + "/" + MD5(url) + ".GIF";
            


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

            return fileName;
        }

        public static string MD5(string str)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
        }


        public string UrlEncode(string value)
        {
            StringBuilder result = new StringBuilder();
            string unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
            foreach (char symbol in value)
            {
                if (unreservedChars.IndexOf(symbol) != -1)
                {
                    result.Append(symbol);
                }
                else
                {
                    result.Append('%' + String.Format("{0:X2}", (int)symbol));
                }
            }

            return result.ToString();
        }

        protected void RequestCallback(int key, string content)
        {
            Encoding utf8 = Encoding.GetEncoding(65001);
            Encoding defaultChars = Encoding.Default;
            byte[] temp = utf8.GetBytes(content);
            byte[] temp1 = Encoding.Convert(utf8, defaultChars, temp);
            string result = defaultChars.GetString(temp1);

            textBox1.AppendText("\r\n----------" + result);
        }

        private bool GetRequestToken(string customKey, string customSecret)
        {
            string url = "https://open.t.qq.com/cgi-bin/request_token";
            List<Parameter> parameters = new List<Parameter>();
            OauthKey oauthKey = new OauthKey();
            oauthKey.customKey = customKey;
            oauthKey.customSecrect = customSecret;
            oauthKey.callbackUrl = baseurl + "/top/groupbuy/record.aspx?typ=qq";

            QWeiboRequest request = new QWeiboRequest();
            return ParseToken(request.SyncRequest(url, "GET", oauthKey, parameters, null));
        }

        private bool GetAccessToken(string customKey, string customSecret, string requestToken, string requestTokenSecrect, string verify)
        {
            string url = "https://open.t.qq.com/cgi-bin/access_token";
            List<Parameter> parameters = new List<Parameter>();
            OauthKey oauthKey = new OauthKey();
            oauthKey.customKey = customKey;
            oauthKey.customSecrect = customSecret;
            oauthKey.tokenKey = requestToken;
            oauthKey.tokenSecrect = requestTokenSecrect;
            oauthKey.verify = verify;

            QWeiboRequest request = new QWeiboRequest();
            return ParseToken(request.SyncRequest(url, "GET", oauthKey, parameters, null));
        }

        private bool ParseToken(string response)
        {
            if (string.IsNullOrEmpty(response))
            {
                return false;
            }

            string[] tokenArray = response.Split('&');

            if (tokenArray.Length < 2)
            {
                return false;
            }

            string strTokenKey = tokenArray[0];
            string strTokenSecrect = tokenArray[1];

            string[] token1 = strTokenKey.Split('=');
            if (token1.Length < 2)
            {
                return false;
            }
            tokenKey = token1[1];

            string[] token2 = strTokenSecrect.Split('=');
            if (token2.Length < 2)
            {
                return false;
            }
            tokenSecret = token2[1];

            return true;
        }
    }
}
