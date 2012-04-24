using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.Security.Cryptography;
using System.Linq;
using System.Web;

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
    public static string Post(string nick, string method, string session, IDictionary<string, string> param, DataFormatType dataType, string appkey, string appSecret)
    {
        #region -----API系统参数----

        string url = "http://gw.api.taobao.com/router/rest";
        //string appkey = "12287381";//"12159997";
        //string appSecret = "d3486dac8198ef01000e7bd4504601a4";//"614e40bfdb96e9063031d1a9e56fbed5";

        //免费版
        //string appkey = "12132145";
        //string appSecret = "1fdd2aadd5e2ac2909db2967cbb71e7f";

        param.Add("method", method);
        param.Add("session", session);
        param.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        param.Add("format", dataType.ToString());
        param.Add("v", "2.0");
        param.Add("sign_method", "md5");

        param.Add("app_key", appkey);
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
        try
        {
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
        }
        catch (Exception ex)
        {
            LogInfo.WriteLog("session:" + session + "获取淘宝服务响应错误", ex.Message);
        }
        #endregion

        return Regex.Replace(result, @"[\x00-\x08\x0b-\x0c\x0e-\x1f]", "");
    }

    /// <summary>
    /// 根据一个商品ID获得一个商品信息
    /// </summary>
    /// <param name="pid">淘宝商品ID</param>
    /// <returns></returns>
    public static GoodsInfo GetGoodsInfo(string pid, string nick, string session, string appkey, string appSecret)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("num_iid", pid);
        dic.Add("fields", "num_iid,title,nick,price,pic_url");
        string text = Post(nick, "taobao.item.get", session, dic, DataFormatType.json, appkey, appSecret);
        GoodsInfo info = new GoodsInfo();
        if (!string.IsNullOrEmpty(text))
        {
            if (text.Contains("error_response"))
            {
                LogInfo.Add("获取一个商品信息出错", text);
                return info;
            }
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            text = text.Replace("{\"item_get_response\":{\"item\":", "").Replace("}}", "");

            try
            {
                info = js.Deserialize<GoodsInfo>(text);
            }
            catch (Exception ex)
            {
                LogInfo.WriteLog("返回json转化为一个商品信息出错,商品id:" + pid, text + ex.Message);
            }
        }
        return info;
    }

    public static List<GoodsInfo> GetGoodsInfoList(string nick, string session, string pids, string appkey, string appSecret)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("num_iids", pids);
        dic.Add("fields", "num_iid,title,nick,price,pic_url");
        string text = Post(nick, "taobao.items.list.get", session, dic, DataFormatType.json, appkey, appSecret);
        List<GoodsInfo> list = new List<GoodsInfo>();
        if (!string.IsNullOrEmpty(text))
        {
            if (text.Contains("error_response"))
            {
                LogInfo.Add("批量获取商品信息出错", text);
                return list;
            }
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            text = text.Replace("{\"items_list_get_response\":{\"items\":{\"item\":", "").Replace("}}}", "");

            try
            {
                list = js.Deserialize<List<GoodsInfo>>(text);
            }
            catch (Exception ex)
            {
                LogInfo.WriteLog("返回json转化为商品信息集合出错,商品id:" + pids, text + ex.Message);
            }
        }
        return list;
    }

    /// <summary>
    /// 根据nick找到店铺所有分类
    /// </summary>
    /// <param name="nickNo">用户nick</param>
    /// <returns></returns>
    public static IList<GoodsClassInfo> GetGoodsClassInfoList(string nick, string session, string appkey, string appSecret)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("nick", nick);
        dic.Add("fields", "cid,name,parent_cid,sort_order");
        string text = Post(nick, "taobao.sellercats.list.get", session, dic, DataFormatType.json, appkey, appSecret);

        IList<GoodsClassInfo> classList = null;
        if (!string.IsNullOrEmpty(text))
        {
            if (text.Contains("error_response"))
            {
                LogInfo.Add("查找商品销售分类出错", text);
                return null;
            }
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            text = text.Replace("{\"sellercats_list_get_response\":{\"seller_cats\":{\"seller_cat\":", "").Replace("]}}}", "") + "]";

            try
            {
                classList = js.Deserialize<List<GoodsClassInfo>>(text);
            }
            catch (Exception ex)
            {
                LogInfo.Add(nick + "返回json转化为一个商品销售分类出错", text + ex.Message);
                return null;
            }
        }
        return classList;
    }

    public static bool GetPromotion(string nick, string session, string tid, string regex, string appkey, string appSecret)
    {
        IDictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("fields", "receiver_mobile, orders.num_iid, created, consign_time, total_fee, promotion_details");
        dic.Add("tid", tid);
        string text = Post(nick, "taobao.trade.fullinfo.get", session, dic, DataFormatType.json, appkey, appSecret);
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

    /// <summary>
    /// 获取当前会话用户出售中的商品列表 
    /// </summary>
    /// <param name="nick"></param>
    /// <param name="session"></param>
    /// <returns></returns>
    public static List<GoodsInfo> GetGoodsInfoListByNick(string nick, string session, string appkey, string appSecret)
    {
        bool notlast = true;
        int page_no = 0;

        List<GoodsInfo> list = new List<GoodsInfo>();
        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
        while (notlast)
        {
            page_no++;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("nick", nick);
            dic.Add("fields", "num_iid,title,cid,pic_url,price");
            dic.Add("page_no", page_no.ToString());
            dic.Add("page_size", "200");
            string text = Post(nick, "taobao.items.onsale.get", session, dic, DataFormatType.json, appkey, appSecret);
            if (!string.IsNullOrEmpty(text))
            {
                if (text.Contains("error_response"))
                {
                    LogInfo.WriteLog("批量获取用户店铺商品列表出错", text);
                    return list;
                }

                text = text.Replace("{\"items_onsale_get_response\":{\"items\":{\"item\":", "").Replace("}}}", "");
                Regex regex = new Regex("},\"total_results\":\\d+}}");
                text = regex.Replace(text, "");

                try
                {
                    List<GoodsInfo> mylist = js.Deserialize<List<GoodsInfo>>(text);
                    list.AddRange(mylist);

                    if (mylist.Count < 200)
                    {
                        notlast = false;
                        return list;
                    }
                }
                catch (Exception ex)
                {
                    LogInfo.WriteLog("返回json转化为商品信息集合出错", "用户nick:" + nick + text + ex.Message);
                }
            }
        }
        return list;
    }

    #endregion


    public static string GetShopInfo(string nick, string session, string appkey, string appSecret)
    {
        Dictionary<string, string> param = new Dictionary<string, string>();

        param.Add("fields", "sid");
        param.Add("nick", nick);//美杜莎之心
        //6102b061e6fe4c1b437274d442350197c9fb5846db06ca8204200856
        string text = Post(nick, "taobao.shop.get", session, param, DataFormatType.json, appkey, appSecret);
        if (text.Contains("error_response"))
        {
            LogInfo.WriteLog("获取用户店铺信息出错", text);
            return "";
        }
        Regex regex = new Regex("\\d+");
        return regex.Match(text).Value;
    }
}

public enum DataFormatType
{
    xml,
    json
}
