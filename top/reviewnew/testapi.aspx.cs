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

public partial class top_review_testapi : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string appkey = "12159997";
        string secret = "614e40bfdb96e9063031d1a9e56fbed5";

        string session = "6101a28530ce2b42ef7b281d0379338df80b16652ef263d150153910";
        string taobaonick = "红色时代灯饰";


        //91599347271901

        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", appkey, secret);

        ItemsOnsaleGetRequest request = new ItemsOnsaleGetRequest();
        request.Fields = "num_iid,title,price,pic_url,seller_cids";
        request.PageSize = 200;
        request.PageNo = 1;

        string str = "0";

        PageList<Item> product = client.ItemsOnsaleGet(request, session);
        for (int i = 0; i < product.Content.Count; i++)
        {
            str += "," + product.Content[i].NumIid;
        }

        //IDictionary<string, string> param = new Dictionary<string, string>();

        ////param.Add("coupon_id", "11815000");
        //param.Add("fields", "receiver_mobile, orders.num_iid, created, consign_time, total_fee, promotion_details, type, receiver_name, receiver_state, receiver_city, receiver_district, receiver_address, status, buyer_area");
        //param.Add("tid", "");

        //string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.promotion.coupondetail.get", session, param);
        //////<coupon_number>1323930538</coupon_number>

        Response.Write(str);

        //param = new Dictionary<string, string>();
        //param.Add("num_iid", "16791228388");
        //param.Add("food_security.prd_license_no", "130917020079");
        ////param.Add("food_security.design_code", "130917020079");
        //param.Add("food_security.factory", "黄骅市绿之源食品有限公司");
        //param.Add("food_security.factory_site", "河北省沧州市黄骅市孔店冬枣市场");
        //param.Add("food_security.contact", "5469827");
        //param.Add("food_security.mix", "鲜冬枣 棕榈油");
        //param.Add("food_security.plan_storage", "低于24度阴凉干燥处存放");
        //param.Add("food_security.period", "300天");
        //param.Add("food_security.food_additive", "无");
        //param.Add("food_security.product_date_start", "2012-06-01");
        //param.Add("food_security.product_date_end", "2012-07-06");
        //param.Add("food_security.stock_date_start", "2012-07-06");
        //string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.item.update", session, param);
        //Response.Write("<textarea>" + result + "</textarea>");
        //return;
        //param = new Dictionary<string, string>();
        //param.Add("promotion_id", "97049550");
        //result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.marketing.promotions.delete", session, param);
        //Response.Write("<textarea>" + result + "</textarea>");

        ////IDictionary<string, string> param = new Dictionary<string, string>();
        ////param.Add("tid", "88346138077381");
        ////param.Add("seller_nick", "天生一对6695");

        //////物流接口暂时停用，因为会影响错误率
        //string result = string.Empty;

        ////result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.logistics.trace.search", session, param);

        ////Response.Write("<textarea>" + result + "</textarea>");

        //////88345525137141
        //////88346168134906

        //param = new Dictionary<string, string>();
        //param.Add("tid", "76048779201050");
        //param.Add("fields", "delivery_start,delivery_end,status");

        ////物流接口暂时停用，因为会影响错误率
        //result = string.Empty;

        //result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.logistics.orders.get", session, param);

        //Response.Write("<textarea>" + result + "</textarea>");

        //string txt = @"<?xml version=""1.0"" encoding=""utf-8"" ?><error_response><args list=""true""><arg><key>app_key</key><value>12159997</value></arg><arg><key>fields</key><value>delivery_start,delivery_end,status</value></arg><arg><key>format</key><value>xml</value></arg><arg><key>method</key><value>taobao.logistics.orders.get</value></arg><arg><key>session</key><value>508312894419b8bd5d07d89e7c65e1c439e36KFsf109a97618794262</value></arg><arg><key>sign</key><value>901E73FB0655390CA336769BA149C420</value></arg><arg><key>sign_method</key><value>md5</value></arg><arg><key>tid</key><value>76048779201050</value></arg><arg><key>timestamp</key><value>2011-09-05 11:38:11</value></arg><arg><key>v</key><value>2.0</value></arg></args><code>550</code><msg>Remote service error</msg><sub_code>isv.invalid-parameter:trade_id:P07</sub_code><sub_msg>查询不到结果,或者交易id不存在</sub_msg></error_response><!--top202077.cm3-->";

        //Response.Write(txt.IndexOf("不存在").ToString());
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