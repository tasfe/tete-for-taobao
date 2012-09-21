using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Data;
using Common;

public partial class taobaoke_tmall : System.Web.UI.Page
{
    public string html = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string appkey = "21088121";
        string secret = "f115a6790148d314cf3214aa029f7eda";

        IDictionary<string, string> param = new Dictionary<string, string>();
        param.Add("cid", "16");
        string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "tmall.selected.items.search", "", param);

        Regex reg = new Regex(@"<track_iid>([^\<]*)</track_iid>", RegexOptions.IgnoreCase);
        MatchCollection mat = reg.Matches(result);
        string track_iids = string.Empty;

        for (int i = 0; i < mat.Count; i++)
        {
            track_iids += "," + mat[i].Groups[1].ToString();

            if (i > 30)
            {
                break;
            }
        }
        track_iids = track_iids.Substring(1, track_iids.Length-1);

        param = new Dictionary<string, string>();
        param.Add("fields", "num_iid,click_url,pic_url,title,price,volume");
        param.Add("track_iids", track_iids);
        param.Add("nick", "叶儿随清风");
        result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.taobaoke.items.convert", "", param);
        reg = new Regex(@"<taobaoke_item><click_url>([^\<]*)</click_url><num_iid>([^\<]*)</num_iid><pic_url>([^\<]*)</pic_url><price>([^\<]*)</price><title>([^\<]*)</title><volume>([^\<]*)</volume></taobaoke_item>", RegexOptions.IgnoreCase);

        mat = reg.Matches(result);

        for (int i = 0; i < mat.Count; i++)
        {
            html += "<div style='float:left; width:190px; height:240px'><a href='" + mat[i].Groups[1].ToString() + "' target=_blank><img src='" + mat[i].Groups[3].ToString() + "_180x180.jpg' width=180 height=180 border=0></a><br>" + mat[i].Groups[5].ToString() + "<br>￥" + mat[i].Groups[4].ToString() + "元 售出" + mat[i].Groups[6].ToString() + "件</div>";
        }
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