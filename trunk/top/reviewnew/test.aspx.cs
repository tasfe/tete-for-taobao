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

public partial class crm_test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //string top_session = "6102b21db4b39e9aaec471c5d6f3217531c5f8ee2c7bd4b13009583";
        //IDictionary<string, string> param = new Dictionary<string, string>();
        //string result = Post("http://gw.api.taobao.com/router/rest", "12159997", "614e40bfdb96e9063031d1a9e56fbed5", "taobao.increment.customer.permit", top_session, param);

        //Response.Write("<html>" + result + "</html>");
        //Response.End();

        SendMessage("13816190083", "+心怡yy+:亲，您购买的货物已经发出,5分满分好评+优质评价,即可获赠掌上游戏机一个和优惠券喔,是要全部5分才有喔&!");
    }

    public static string UrlEncode(string str)
    {
        StringBuilder sb = new StringBuilder();
        byte[] byStr = System.Text.Encoding.Default.GetBytes(str);
        for (int i = 0; i < byStr.Length; i++)
        {
            sb.Append(@"%" + Convert.ToString(byStr[i], 16));
        }

        return (sb.ToString());
    }


    private string SendMessage(string phone, string msg)
    {
        string uid = "ZXHD-SDK-0107-XNYFLX";
        string pass = MD5AAA("WEGXBEPY");

        msg = UrlEncode(msg);

        string param = "regcode=" + uid + "&pwd=" + pass + "&phone=" + phone + "&CONTENT=" + msg + "&extnum=11&level=1&schtime=null&reportflag=1&url=&smstype=4&key=aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        //byte[] bs = Encoding.ASCII.GetBytes(param);
        //param = "regcode=短信帐号&pwd=(明码做md5加密后的字符串作密码))&key=aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa&CNAME=中文公司名&ENAME=英文公司名&CSNAME=中文简称&ESNAME=英文简称&ENTERPRISETYPEID=01&ADDR=联系地址&LINKTEL=联系电话&LINKMAN=&EMAIL=邮箱地址&FAX=传真&POSTCODE=邮编(6字符长度)&MOBILETEL=联系手机";
        byte[] bs = Encoding.ASCII.GetBytes(param);
        HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("http://sms.pica.com:8082/zqhdServer/sendSMS.jsp" + "?" + param);
        req.ContentType = "gb2312";

        Response.Write("http://sms.pica.com:8082/zqhdServer/sendSMS.jsp" + "?" + param);
        Response.End();
        
        req.Method = "GET";
        //req.ContentType = "application/x-www-form-urlencoded";
        //req.ContentLength = bs.Length;

        //using (Stream reqStream = req.GetRequestStream())
        //{
        //    reqStream.Write(bs, 0, bs.Length);
        //}
        using (HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse())
        {
            using (StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.GetEncoding("GB2312")))
            {
                string content = reader.ReadToEnd();
                Response.Write(content + "!!!!!!!!");
                Response.End();

                if (content.IndexOf("<error>0</error>") == -1)
                {
                    //发送失败
                    Response.Write(content);
                    Response.End();
                    return "0";
                }
                else
                {
                    //发送成功
                    Regex reg = new Regex(@"<sid>([^<]*)</sid>", RegexOptions.IgnoreCase);
                    MatchCollection match = reg.Matches(content);
                    string number = match[0].Groups[1].ToString();
                    return number;
                }
            }
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


    public static string MD5AAA(string str)
    {
        return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower();
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