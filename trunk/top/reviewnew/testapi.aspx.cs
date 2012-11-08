using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Xml;
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;
using Common;

public partial class top_review_testapi : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        GetUserCoupon();
    }

    private void GetUserCoupon()
    {
        string result = string.Empty;
        string appkey = "12159997";
        string secret = "614e40bfdb96e9063031d1a9e56fbed5";
        string buynick = "qs娜娜";
        string taobaonick = "奥美姿旗舰店";
        string session = "6101503745f1265a20262d5e8288968fa2c4e3871eea348679089675";

        IDictionary<string, string> param = new Dictionary<string, string>();
        param.Add("coupon_id", "12156727");
        param.Add("buyer_nick", buynick);
        result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.promotion.coupondetail.get", session, param);
        Response.Write(result);
    }

    private void GetTradeRate()
    {
        string appkey = "12159997";
        string secret = "614e40bfdb96e9063031d1a9e56fbed5";

        string taobaonick = "a木_木a";
        string result = string.Empty;

        string sql = "SELECT session FROM TCS_ShopSession WHERE nick = '" + taobaonick + "'";
        string session = utils.ExecuteString(sql);

        IDictionary<string, string> param = new Dictionary<string, string>();
        param.Add("fields", "content,created,nick,result");
        param.Add("rate_type", "get");
        param.Add("role", "buyer");
        param.Add("tid", "226860043511902");
        result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.traderates.get", session, param);
        Response.Write(result);
    }

    private void GetTrade()
    {
        string appkey = "12159997";
        string secret = "614e40bfdb96e9063031d1a9e56fbed5";

        string session = "610252000038f8d4d76f20f9008c1c7391c2d63c0a4ff0361056151";
        string taobaonick = "我爱ivy";

        IDictionary<string, string> param = new Dictionary<string, string>();

        param.Add("fields", "receiver_mobile, orders.num_iid, created, consign_time, total_fee, promotion_details, type, receiver_name, receiver_state, receiver_city, receiver_district, receiver_address, status, buyer_area, orders.oid");
        param.Add("tid", "165816737617778");

        string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.trade.fullinfo.get", session, param);

        Response.Write(result);
    }


    private void Fresh()
    {
        string appkey = "12159997";
        string secret = "614e40bfdb96e9063031d1a9e56fbed5";

        string session = "6102a28c7dc9ad3f71b1c98c7708502a228316b5bdb250414732390";
        string taobaonick = "tangchao4010790";

        IDictionary<string, string> param = new Dictionary<string, string>();

        param.Add("fields", "id,code,name,reg_mail_no");

        string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.logistics.companies.get", session, param);
        ////<coupon_number>1323930538</coupon_number>
        //Response.Write(result + "<br>");
        Regex reg = new Regex(@"<code>([^<]*)</code><id>([^<]*)</id><name>([^<]*)</name>", RegexOptions.IgnoreCase);
        MatchCollection match = reg.Matches(result);

        for (int i = 0; i < match.Count; i++)
        {
            string sql = "SELECT COUNT(*) FROM TCS_TaobaoShippingCompany WHERE short = '" + match[i].Groups[1].ToString() + "'";
            string count = utils.ExecuteString(sql);

            if (count == "0")
            {
                if (match[i].Groups[1].ToString() != "OTHER" && match[i].Groups[1].ToString() != "POST")
                {
                    sql = "INSERT INTO TCS_TaobaoShippingCompany (name, short) VALUES ('" + match[i].Groups[3].ToString() + "', '" + match[i].Groups[1].ToString() + "')";
                    Response.Write(sql + "<br>");
                    utils.ExecuteNonQuery(sql);
                }
            }
            else
            {
                sql = "UPDATE TCS_TaobaoShippingCompany SET name = '" + match[i].Groups[3].ToString() + "' WHERE short = '" + match[i].Groups[1].ToString() + "'";
                Response.Write(sql + "<br>");
                utils.ExecuteNonQuery(sql);
            }
        }
    }


    #region TOP API
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
        req.ContentType = "application/x-www-form-urlencoded;charset=gb2312";
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
    #endregion
}