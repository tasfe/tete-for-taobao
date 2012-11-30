using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web.Security;

public partial class api_getnewdata : System.Web.UI.Page
{
    public string nick = string.Empty;
    public string session = string.Empty;
    public string st = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        //Common.Cookie cookie = new Common.Cookie();
        //string taobaoNick = cookie.getCookie("nick");
        //session = cookie.getCookie("top_session");
        //st = cookie.getCookie("short");
        //Rijndael_ encode = new Rijndael_("tetesoft");
        //nick = encode.Decrypt(taobaoNick);

        //Act(st, nick);
        //Response.Write("数据更新完毕！");
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string uid = this.TextBox1.Text;
        string cateStr = this.TextBox2.Text;
        Act(uid, cateStr);
        Response.Write("数据更新完毕！");
    }

    private void Act(string uid, string cateStr)
    {
        string sql = "SELECT * FROM TeteShop WHERE nick = '" + uid + "'";

        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            string appkey = dt.Rows[0]["appkey"].ToString();
            string secret = dt.Rows[0]["appsecret"].ToString();

            string[] ary = cateStr.Split('|');
            for (int i = ary.Length-1; i >=0; i--)
            {
                string[] aryList = ary[i].Split(',');

                sql = "SELECT COUNT(*) FROM TeteShopCategory WHERE nick = '" + uid + "' AND catename = '" + aryList[0] + "'";
                string count1 = utils.ExecuteString(sql);

                if (count1 == "0")
                {
                    sql = "INSERT INTO TeteShopCategory (" +
                                            "cateid, " +
                                            "catename, " +
                                            "oldname, " +
                                            "parentid, " +
                                            "nick " +
                                        " ) VALUES ( " +
                                            " '" + i.ToString() + "', " +
                                            " '" + aryList[0] + "', " +
                                            " '" + aryList[0] + "', " +
                                            " '0', " +
                                            " '" + uid + "' " +
                                      ") ";
                    utils.ExecuteNonQuery(sql);
                }

                for (int j = 0; j < aryList.Length; j++)
                {
                    for (int page = 0; page <= 10; page++)
                    {
                        IDictionary<string, string> param = new Dictionary<string, string>();
                        param.Add("fields", "num_iid,title,pic_url,click_url,price");
                        param.Add("keyword", aryList[j]);
                        param.Add("page_no", page.ToString());
                        param.Add("sort", "commissionNum_desc");
                        param.Add("is_mobile", "true");
                        param.Add("page_size", "40");
                        string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.taobaoke.items.get", "", param);

                        Regex reg = new Regex(@"<click_url>([^<]*)</click_url><num_iid>([^<]*)</num_iid><pic_url>([^<]*)</pic_url><price>([^<]*)</price><title>([^<]*)</title>", RegexOptions.IgnoreCase);
                        MatchCollection match = reg.Matches(result);

                        for (int k = 0; k < match.Count; k++)
                        {
                            try
                            {
                                //判断商品是否存在
                                sql = "SELECT COUNT(*) FROM TeteShopItem WHERE itemid = '" + match[k].Groups[2].ToString() + "'";
                                string count = utils.ExecuteString(sql);
                                if (count == "0")
                                {
                                    string fileName = Server.MapPath("tmpimg/" + strMD5(match[k].Groups[3].ToString() + "_100x100.jpg"));
                                    //保存临时图片获取图片尺寸
                                    if (!File.Exists(fileName))
                                    {
                                        WebClient c = new WebClient();
                                        c.DownloadFile(match[k].Groups[3].ToString() + "_100x100.jpg", fileName);
                                    }
                                    System.Drawing.Image img = System.Drawing.Image.FromFile(fileName);


                                    sql = "INSERT INTO TeteShopItem (" +
                                                "cateid, " +
                                                "itemid, " +
                                                "itemname, " +
                                                "picurl, " +
                                                "linkurl, " +
                                                "price, " +
                                                "width, " +
                                                "height, " +
                                                "nick " +
                                            " ) VALUES ( " +
                                                " '" + i.ToString() + "', " +
                                                " '" + match[k].Groups[2].ToString() + "', " +
                                                " '" + ReplaceTitleHtml(match[k].Groups[5].ToString()) + "', " +
                                                " '" + match[k].Groups[3].ToString() + "', " +
                                                " '" + match[k].Groups[1].ToString() + "', " +
                                                " '" + match[k].Groups[4].ToString() + "', " +
                                                " '" + img.Width + "', " +
                                                " '" + img.Height + "', " +
                                                " '" + uid + "' " +
                                          ") ";
                                    utils.ExecuteNonQuery(sql);
                                }
                            }
                            catch { }
                        }
                    }
                }
            }
        }
    }





    public static string strMD5(string str)
    {
        return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
    }

    private string ReplaceTitleHtml(string title)
    {
        string str = Regex.Replace(title, @"&lt;[^&]*&gt;", "");

        return str;
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

    public static string SendPostData(string url, string data)
    {
        string result = string.Empty;
        #region ---- 完成 HTTP POST 请求----
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        req.Method = "POST";
        req.KeepAlive = true;
        req.Timeout = 300000;
        req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
        byte[] postData = Encoding.UTF8.GetBytes(data);
        Stream reqStream = req.GetRequestStream();
        reqStream.Write(postData, 0, postData.Length);
        reqStream.Close();
        HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
        Encoding encoding = Encoding.UTF8;
        Stream stream = null;
        StreamReader reader = null;
        stream = rsp.GetResponseStream();
        reader = new StreamReader(stream, encoding);
        result = reader.ReadToEnd();
        if (reader != null) reader.Close();
        if (stream != null) stream.Close();
        if (rsp != null) rsp.Close();
        #endregion
        return result;
    }
}