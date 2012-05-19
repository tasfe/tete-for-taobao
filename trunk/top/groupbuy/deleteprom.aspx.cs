using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

public partial class top_groupbuy_deleteprom : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //删除活动
        string appkey = "12287381";
        string secret = "d3486dac8198ef01000e7bd4504601a4";
        IDictionary<string, string> param = new Dictionary<string, string>();
        Common.Cookie cookie = new Common.Cookie();
        string session = cookie.getCookie("top_sessiongroupbuy");

        //通过数据库查询获取活动ID
        string promotion_id = "342909095";

        //创建活动
        param = new Dictionary<string, string>();
        param.Add("promotion_id", promotion_id);
        string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.marketing.promotion.delete", session, param);

        Response.Write(result);
        Response.End();
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
        MD5 md5 = System.Security.Cryptography.MD5.Create();

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
                // postData.Append(Uri.EscapeDataString(value));
                postData.Append(GetUriFormate(value));
                hasParam = true;
            }
        }
        return postData.ToString();
    }

    /// <summary>
    /// 将参数转换成 uri 格式
    /// </summary>
    /// <param name="inputString">string类型的字符串</param>
    /// <returns>编码后的string</returns>
    private static string GetUriFormate(string inputString)
    {
        StringBuilder strBuilder = new StringBuilder();
        string sourceStr = inputString;
        int len = sourceStr.Length;
        do
        {
            if (len - 21766 <= 0)
            {
                strBuilder.Append(Uri.EscapeDataString(sourceStr));
            }
            else
            {
                strBuilder.Append(Uri.EscapeDataString(sourceStr.Substring(0, 21766)));

                sourceStr = sourceStr.Substring(21766);
                len = sourceStr.Length;
                if (len - 21766 < 0)
                {
                    strBuilder.Append(Uri.EscapeDataString(sourceStr));
                }
            }
        }
        while (len - 21766 > 0);

        return strBuilder.ToString();
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
        System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
        req.Method = "POST";
        req.KeepAlive = true;
        req.Timeout = 300000;
        req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
        byte[] postData = Encoding.UTF8.GetBytes(PostData(param));
        Stream reqStream = req.GetRequestStream();
        reqStream.Write(postData, 0, postData.Length);
        reqStream.Close();
        System.Net.HttpWebResponse rsp = (System.Net.HttpWebResponse)req.GetResponse();
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