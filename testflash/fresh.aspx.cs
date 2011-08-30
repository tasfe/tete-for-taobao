using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;
using System.Net;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Data;

public partial class testflash_fresh : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string nick = "叶儿随清风";
        string buynick = "美杜莎之心";
        string groupbuyid = "10";
        //通过接口将该用户加入人群
        string sql = "SELECT session FROM TopTaobaoShop WHERE nick = '" + nick + "'";
        string session = utils.ExecuteString(sql);

        //获取团购活动的商品信息
        sql = "SELECT productid FROM TopGroupBuy WHERE id =" + groupbuyid;
        string groupproductid = utils.ExecuteString(sql);

        string appkey = "12223169";
        string secret = "ff3d3442ab809930d187623ffad8e91e";
        IDictionary<string, string> param = new Dictionary<string, string>();
        param.Add("fields", "buyer_nick,tid,status");
        param.Add("start_modified", "2011-04-12 17:00:12");
        param.Add("end_modified", "2011-04-12 17:28:12");

        string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.trades.sold.increment.get", session, param);

        //Response.Write(result);
        Regex reg = new Regex(@"<buyer_nick>([^<]*)</buyer_nick><status>([^<]*)</status><tid>([^<]*)</tid>", RegexOptions.IgnoreCase);
        MatchCollection match = reg.Matches(result);
        for (int i = 0; i < match.Count; i++)
        {
            if (match[i].Groups[1].ToString() == buynick)
            {
                //判断该用户的订单是否包含该团购商品
                param = new Dictionary<string, string>();
                param.Add("fields", "orders.num_iid,orders.num");
                param.Add("tid", match[i].Groups[3].ToString());
                string resultpro = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.trade.fullinfo.get", session, param);

                //Response.Write(resultpro);

                //判断该订单里面是否包含团购商品并统计订购数量
                Regex regpro = new Regex(@"<num>([^<]*)</num><num_iid>([^<]*)</num_iid>", RegexOptions.IgnoreCase);
                MatchCollection matchpro = regpro.Matches(resultpro);
                for (int j = 0; j < matchpro.Count; j++)
                {
                    //Response.Write(matchpro[j].Groups[2].ToString() + "-" + groupproductid);
                    //判断该订单是否包含团购商品
                    if (matchpro[j].Groups[2].ToString() == groupproductid)
                    {
                        //如果包含则记录到本地数据库
                        RecordBuyData(buynick, groupbuyid, match[i].Groups[2].ToString(), matchpro[j].Groups[1].ToString(), match[i].Groups[3].ToString());
                    }
                    else
                    { 
                        //取消 
                        return;
                    }
                }
            }
        }

        //刷新判断买家是否在30分钟内完成订单，如果没有则取消资格和相关订单

        //如果买家在30分钟内有购买行为，则取消资格并更新团购总数量和团购详细信息

        //判断活动是否结束（时间限制和数量限制），如果结束则删除相关活动和人群，更新团购数量，更新商品详细页面展示图片信息

    }

    private void RecordBuyData(string buynick, string groupbuyid, string status, string num, string orderid)
    {
        //更新购买记录
        string sql = "UPDATE TopGroupBuyDetail SET count = count + " + num + ",ordernumber = ordernumber + '|" + orderid + "', paystatus = paystatus + '|" + status + "' WHERE buynick = '" + buynick + "' AND groupbuyid = " + groupbuyid + "";
        utils.ExecuteNonQuery(sql);
        //Response.Write(sql);
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
}