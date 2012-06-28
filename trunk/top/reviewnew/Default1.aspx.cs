using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Net;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Data;

public partial class top_reviewnew_Default : System.Web.UI.Page
{
    public string session = string.Empty;
    public string nick = string.Empty;
    public string endsenddate = string.Empty;
    public string enddate = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        session = cookie.getCookie("top_sessiongroupbuy");
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Decrypt(taobaoNick);
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string appkey = "12690738";
        string secret = "66d488555b01f7b85f93d33bc2a1c001";

        IDictionary<string, string> param = new Dictionary<string, string>();
        param.Add("fields", "tid,oid,role,nick,result,created,rated_nick,item_title,item_price,content,reply,num_iid");
        param.Add("rate_type", "get");
        param.Add("role", "buyer");

        string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.traderates.get", session, param);

        Regex reg = new Regex(@"<trade_rate>([\s\S]*)</trade_rate>", RegexOptions.IgnoreCase);

        MatchCollection mat = reg.Matches(result);
        for (int i = 0; i < mat.Count; i++) 
        {
            string sql = "INSERT INTO TCS_TradeRate (" +
                "orderid, " +
                "content, " +
                "reviewdate, " +
                "buynick, " +
                "nick, " +
                "itemid, " +
                "result " +
            " ) VALUES ( " +
                " '" + GetValueByProperty(mat[i].Groups[1].ToString(), "tid") + "', " +
                " '" + GetValueByProperty(mat[i].Groups[1].ToString(), "content") + "', " +
                " '" + GetValueByProperty(mat[i].Groups[1].ToString(), "created") + "', " +
                " '" + GetValueByProperty(mat[i].Groups[1].ToString(), "nick") + "', " +
                " '" + GetValueByProperty(mat[i].Groups[1].ToString(), "rated_nick") + "', " +
                " '" + GetValueByProperty(mat[i].Groups[1].ToString(), "num_iid") + "', " +
                " '" + GetValueByProperty(mat[i].Groups[1].ToString(), "result") + "' " +
            ") ";

            Response.Write(sql + "<br>");
            utils.ExecuteNonQuery(sql);
        }
    }

    public static string GetValueByProperty(string str, string prop)
    {
        Regex reg = new Regex("<" + prop + ">([^<]*)</" + prop + ">", RegexOptions.IgnoreCase);
        if (reg.IsMatch(str))
        {
            return reg.Matches(str)[0].Groups[1].ToString().Replace("'", "''");
        }
        else
        {
            return "";
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