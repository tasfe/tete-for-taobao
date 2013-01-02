using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;

/// <summary>
/// 从淘宝获取数据
/// </summary>
public class TopAPI
{
    public static string Post(string method, string session, IDictionary<string, string> param)
    {
        #region -----API系统参数----

        string url = "http://gw.api.taobao.com/router/rest";
        string appkey = "12450498";
        string appSecret = "38c892fcaa5a971aec7a9effd105c7ba";

        //免费版
        //string appkey = "12132145";
        //string appSecret = "1fdd2aadd5e2ac2909db2967cbb71e7f";

        param.Add("method", method);
        param.Add("session", session);
        param.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        param.Add("format", "json");
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

        }
        #endregion

        return Regex.Replace(result, @"[\x00-\x08\x0b-\x0c\x0e-\x1f]", "");
    }

    static string CreateSign(IDictionary<string, string> parameters, string secret)
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

    static string PostData(IDictionary<string, string> parameters)
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

    public static List<TaoBaoGoodsInfo> GetGoodsInfoListByNick(string nick, string session)
    {
        bool notlast = true;
        int page_no = 0;

        List<TaoBaoGoodsInfo> list = new List<TaoBaoGoodsInfo>();
        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
        while (notlast)
        {
            page_no++;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("nick", nick);
            dic.Add("fields", "num_iid,title,cid,pic_url,price,seller_cids,num,modified");
            dic.Add("page_no", page_no.ToString());
            dic.Add("page_size", "200");
            string text = Post("taobao.items.onsale.get", session, dic);
            if (!string.IsNullOrEmpty(text))
            {
                if (text.Contains("error_response"))
                {
                    LogInfo.Add(nick, "批量获取用户店铺商品列表出错" + text);
                    return list;
                }

                text = text.Replace("{\"items_onsale_get_response\":{\"items\":{\"item\":", "").Replace("}}}", "");
                Regex regex = new Regex("},\"total_results\":\\d+}}");
                text = regex.Replace(text, "");

                try
                {
                    List<TaoBaoGoodsInfo> mylist = js.Deserialize<List<TaoBaoGoodsInfo>>(text);
                    list.AddRange(mylist);

                    if (mylist.Count < 200)
                    {
                        notlast = false;
                        return list;
                    }
                }
                catch (Exception ex)
                {
                    LogInfo.Add(nick, "返回json转化为商品信息集合出错,用户nick:" + nick + text + ex.Message);
                }
            }
        }
        return list;
    }

    public static IList<TaoBaoGoodsClassInfo> GetGoodsClassInfoList(string nickNo, string session)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("nick", nickNo);
        dic.Add("fields", "cid,name,parent_cid");
        string text = Post("taobao.sellercats.list.get", session, dic);

        IList<TaoBaoGoodsClassInfo> classList = null;
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
                classList = js.Deserialize<List<TaoBaoGoodsClassInfo>>(text);
            }
            catch (Exception ex)
            {
                LogInfo.Add("返回json转化为一个商品销售分类出错", text + ex.Message);
                return null;
            }
        }

        return classList;
    }

}
