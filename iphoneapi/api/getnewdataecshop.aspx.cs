using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web.Security;
using Common;

public partial class iphoneapi_api_getnewdataecshop : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        InitData();
    }

    private void InitData()
    {
        string html = string.Empty;
        string sql = string.Empty;
        string url = string.Empty;
        string cateid = string.Empty;
        string catename = string.Empty;

        string itemid = string.Empty;
        string itemname = string.Empty;
        string itemimg = string.Empty;

        string uid = "huahua";

        //获取分类数据
        url = "http://www.aa7a.cn/apiios.php?act=showcate";
        html = Post(url);

        //清理老数据
        sql = "DELETE FROM TeteShopCategory WHERE nick = '" + uid + "'";
        utils.ExecuteNonQuery(sql);

        sql = "DELETE FROM TeteShopItem WHERE nick = '" + uid + "'";
        utils.ExecuteNonQuery(sql);



        Regex reg = new Regex(@"<cat_id>([\s\S]*?)</cat_id>[\s]*<cat_name>([\s\S]*?)</cat_name>", RegexOptions.IgnoreCase);
        MatchCollection match = reg.Matches(html);
        //Response.Write(match.Count.ToString());
        for (int i = 0; i < match.Count; i++)
        {
            cateid = replace(match[i].Groups[1].ToString());
            catename = replace(match[i].Groups[2].ToString());
            url = "http://www.aa7a.cn/apiios.php?act=showitem&cid=" + cateid;
            html = Post(url);

            sql = "INSERT INTO TeteShopCategory (" +
                                    "cateid, " +
                                    "catename, " +
                                    "oldname, " +
                                    "parentid, " +
                                    "nick " +
                                " ) VALUES ( " +
                                    " '" + cateid + "', " +
                                    " '" + catename + "', " +
                                    " '" + catename + "', " +
                                    " '0', " +
                                    " '" + uid + "' " +
                              ") ";
            Response.Write(sql + "<br>");
            utils.ExecuteNonQuery(sql);

            Regex reg1 = new Regex(@"<goods_id>([\s\S]*?)</goods_id>[\s]*<goods_name>([\s\S]*?)</goods_name>[\s]*<goods_img>([\s\S]*?)</goods_img>", RegexOptions.IgnoreCase);
            MatchCollection matchItem = reg1.Matches(html);
            for (int j = 0; j < matchItem.Count; j++)
            {
                itemid = replace(matchItem[j].Groups[1].ToString());
                itemname = replace(matchItem[j].Groups[2].ToString());
                itemimg = replace(matchItem[j].Groups[3].ToString());

                sql = "INSERT INTO TeteShopItem (" +
                                    "cateid, " +
                                    "itemid, " +
                                    "itemname, " +
                                    "picurl, " +
                                    "linkurl, " +
                                    "price, " +
                                    "nick " +
                                " ) VALUES ( " +
                                    " '" + cateid + "', " +
                                    " '" + itemid + "', " +
                                    " '" + itemname + "', " +
                                    " 'http://www.aa7a.cn/" + itemimg + "', " +
                                    " 'http://www.aa7a.cn/goods-" + itemid + ".html', " +
                                    " '0', " +
                                    " '" + uid + "' " +
                              ") ";
                Response.Write(sql + "<br>");
                utils.ExecuteNonQuery(sql);
            }
        }

        Response.End();
    }


    private string replace(string str)
    {
        str = str.Replace("<![CDATA[", "");
        str = str.Replace("]]>", "");

        return str;
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
    public static string Post(string url)
    {
        string result = string.Empty;
        #region ---- 完成 HTTP POST 请求----
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        req.Method = "GET";
        req.KeepAlive = true;
        req.Timeout = 300000;
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