﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.Security.Cryptography;

/// <summary>
/// Summary description for TaoBaoAPI
/// </summary>
public class TaoBaoAPI
{

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
    /// <param name="method">API接口方法名</param> 
    /// <param name="session">调用私有的sessionkey</param> 
    /// <param name="param">请求参数</param> 
    /// <param name="dataType">淘宝返回数据格式</param>
    /// <returns>返回字符串</returns> 
    public static string Post(string method, string session, IDictionary<string, string> param, DataType dataType)
    {
        #region -----API系统参数----

        string url = "http://gw.api.taobao.com/router/rest";
        //string appkey = "12287381";//"12159997";
        //string appSecret = "d3486dac8198ef01000e7bd4504601a4";//"614e40bfdb96e9063031d1a9e56fbed5";

        //免费版
        string appkey = "12132145";
        string appSecret = "1fdd2aadd5e2ac2909db2967cbb71e7f";

        param.Add("app_key", appkey);
        param.Add("method", method);
        param.Add("session", session);
        param.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        param.Add("format", dataType.ToString());
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

    /// <summary>
    /// 根据一个商品ID获得一个商品信息
    /// </summary>
    /// <param name="pid">淘宝商品ID</param>
    /// <returns></returns>
    public static GoodsInfo GetGoodsInfo(string pid)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("num_iid", pid);
        dic.Add("fields", "num_iid,title,nick,price");
        string text = Post("taobao.item.get", "", dic, DataType.json);
        GoodsInfo info = null;
        if (!string.IsNullOrEmpty(text))
        {
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            text = text.Replace("{\"item_get_response\":{\"item\":", "").Replace("}}", "");
            info = js.Deserialize<GoodsInfo>(text);
        }
        return info;
    }

    /// <summary>
    /// 根据nick找到店铺所有分类
    /// </summary>
    /// <param name="nickNo">用户nick</param>
    /// <returns></returns>
    public static IList<GoodsClassInfo> GetGoodsClassInfoList(string nickNo)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("nick", nickNo);
        string text = Post("taobao.sellercats.list.get", "", dic, DataType.json);
        IList<GoodsClassInfo> classList = null;
        if (!string.IsNullOrEmpty(text))
        {
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            text = text.Replace("{\"sellercats_list_get_response\":{\"seller_cats\":{\"seller_cat\":", "").Replace("]}}}", "") + "]";
            classList = js.Deserialize<List<GoodsClassInfo>>(text);
        }
        return classList;
    }

    public static IList<GoodsOrderInfo> GetGoodsOrderInfoList(DateTime start, DateTime end, string session, string orderState)
    {
        bool notlast = true;
        int page_no = 0;

        List<GoodsOrderInfo> list = new List<GoodsOrderInfo>();
        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
        while (notlast)
        {
            page_no++;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("start_created", start.ToString("yyyy-MM-dd HH:mm:ss"));
            dic.Add("end_created", end.ToString("yyyy-MM-dd HH:mm:ss"));
            dic.Add("use_has_next", "true");
            dic.Add("page_size", "100");
            dic.Add("page_no", page_no.ToString());
            dic.Add("status", orderState);//"TRADE_FINISHED");
            dic.Add("fields", "total_fee,receiver_state,receiver_city,commission_fee,payment,cod_fee,end_time,pay_time,created,post_fee,tid,commission_fee,seller_nick,orders.num_iid,orders.num,orders.status");
            string text = Post("taobao.trades.sold.get", session, dic, DataType.json);
            if (!string.IsNullOrEmpty(text))
            {
                if (text.Contains("error_response"))
                    return null;
                string index = "{\"trades_sold_get_response\":{\"has_next\":true,\"trades\":{\"trade\":[";
                if (!text.Contains(index))
                {
                    index = "{\"trades_sold_get_response\":{\"has_next\":false,\"trades\":{\"trade\":[";
                    notlast = false;
                }

                Regex regex = new Regex(",\"total_results\":\\d+}}", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                if (new Regex("\"total_results\":\\d+}}", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Match(text).Value == "\"total_results\":0}}")
                    return list;

                text = regex.Replace(text, "");

                text = text.Replace("{\"order\":", "");
                text = text.Replace("},\"pay_time", ",\"pay_time");

                text = text.Replace(index, "");
                text = "[" + text.Substring(0, text.Length - 1);
                try
                {
                    list.AddRange(js.Deserialize<List<GoodsOrderInfo>>(text));
                    //for (int i = 0; i < list.Count; i++)
                    //{
                    //    if (list[i].orders.Count > 1)
                    //    {
                    //        string s = list[i].tid;
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    LogInfo.WriteLog("获取订单转换出错", ex.Message);
                }
            }
        }
        return list;
    }

    public static bool GetPromotion(string session, string tid,string regex)
    {
        IDictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("fields", "receiver_mobile, orders.num_iid, created, consign_time, total_fee, promotion_details");
        dic.Add("tid", tid);
        string text = Post("taobao.trade.fullinfo.get", session, dic, DataType.json);
        if (!string.IsNullOrEmpty(text))
        {
            if (text.Contains("error_response"))
            {
                LogInfo.WriteLog("发送获取订单使用所有优惠信息出错", text);
            }
            if (text.Contains(regex))
                return true;
        }
        return false;
    }

    public static PingJiaInfo GetPingjia(string session, string tid)
    {
        IDictionary<string, string> param = new Dictionary<string, string>();
        param.Add("fields", "content,created,nick,result");
        param.Add("rate_type", "get");
        param.Add("role", "buyer");
        param.Add("tid", tid);

        string text = Post("taobao.traderates.get", session, param, DataType.json);
        IList<PingJiaInfo> list = new List<PingJiaInfo>();
        if (!string.IsNullOrEmpty(text))
        {
            if (text.Contains("error_response"))
                return null;
            Regex regex = new Regex("},\"total_results\":\\d+}}", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            text = regex.Replace(text, "");
            text = text.Replace("{\"traderates_get_response\":{\"trade_rates\":{\"trade_rate\":", "");
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            try
            {
                list = js.Deserialize<List<PingJiaInfo>>(text);
            }
            catch (Exception ex)
            {
                LogInfo.WriteLog("获取信息出错", ex.Message);
            }
        }
        return list[0];
    }

    #endregion
}


public enum DataType
{
    xml,
    json
}
