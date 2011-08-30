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

public partial class testflash_addbuyinfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string id = utils.NewRequest("id", utils.RequestType.QueryString);
        string buynick = utils.NewRequest("buynick", utils.RequestType.QueryString);
        //buynick = "美杜莎之心";

        //判读是否超出活动时间或者活动尚未开始
        string sql = "SELECT * FROM TopGroupBuy WHERE id = " + id;
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            if (DateTime.Now < DateTime.Parse(dt.Rows[0]["starttime"].ToString()) || DateTime.Now > DateTime.Parse(dt.Rows[0]["endtime"].ToString()))
            { 
                //活动还未开始
                Response.Write("活动还未开始");
                return;
            }
        }
        else
        {
            Response.Write("该活动不存在");
            return;
        }

        //判断是否超出团购数量上限
        if (int.Parse(dt.Rows[0]["buycount"].ToString()) >= int.Parse(dt.Rows[0]["maxcount"].ToString()))
        {
            Response.Write("团购商品已经卖完");
            return;
        }

        //获取买家信息并加入数据库和该活动关联人群
        sql = "SELECT COUNT(*) FROM TopGroupBuyDetail WHERE groupbuyid = " + id + " AND iscancel = 0 AND buynick = '" + buynick + "'";
        string count = utils.ExecuteString(sql);

        //Response.Write(sql);

        if (count == "0")
        {
            //数据库记录
            sql = "INSERT INTO TopGroupBuyDetail (" +
                           "groupbuyid," +
                           "buynick" +
                       " ) VALUES ( " +
                           " '" + id + "'," +
                           " '" + buynick + "'" +
                    ") ";
            utils.ExecuteNonQuery(sql);

            //通过接口将该用户加入人群
            sql = "SELECT session FROM TopTaobaoShop WHERE nick = '" + dt.Rows[0]["nick"].ToString() + "'";
            string session = utils.ExecuteString(sql);
            string appkey = "12223169";
            string secret = "ff3d3442ab809930d187623ffad8e91e";
            IDictionary<string, string> param = new Dictionary<string, string>();
            param.Add("tag_id", dt.Rows[0]["tagid"].ToString());
            param.Add("nick", buynick);

            string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.marketing.taguser.add", session, param);

            //Response.Write(result);
        }
        //给出提示并提示买家跳转
        Response.Redirect("http://item.taobao.com/item.htm?id=" + dt.Rows[0]["productid"].ToString());
        //买家需在30分钟付款否则取消团购资格和未付款订单

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