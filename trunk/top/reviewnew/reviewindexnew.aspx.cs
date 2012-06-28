using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;
using System.Net;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Data;
using Taobao.Top.Api.Request;
using Taobao.Top.Api;
using Taobao.Top.Api.Domain;


public partial class top_reviewnew_reviewindex : System.Web.UI.Page
{
    public string session = string.Empty;
    public string nick = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string id = utils.NewRequest("id", utils.RequestType.QueryString);
        string act = utils.NewRequest("act", utils.RequestType.QueryString);
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        session = cookie.getCookie("top_sessiongroupbuy");
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Decrypt(taobaoNick);

        //过期判断
        if (string.IsNullOrEmpty(taobaoNick))
        {
            string msg = "尊敬的淘宝卖家，非常抱歉的告诉您，您还没有购买此服务或者登录信息已失效，如需继续使用请<a href='http://fuwu.taobao.com/serv/detail.htm?service_id=4545' target='_blank'>进入该服务</a>，谢谢！";
            Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
            Response.End();
            return;
        }

        //判断VIP版本，只有VIP才能使用此功能
        string sql = "SELECT * FROM TCS_ShopSession WHERE nick = '" + nick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            string flag = dt.Rows[0]["version"].ToString();
            if (flag == "0")
            {
                Response.Redirect("xufei.aspx");
                Response.End();
                return;
            }
        }

        string action = utils.NewRequest("action", utils.RequestType.Form);
        if (action == "save")
        {
            SaveIndexInfo();
            return;
        }

        if (act == "add")
        {
            InitTradeRateData(id);
            return;
        }

        if (act == "del")
        {
            DelTradeRateData(id);
            return;
        }

        BindData();
    }

    private void SaveIndexInfo()
    {
        string sql = "SELECT TOP 20 * FROM TCS_TradeRate WHERE nick = '" + nick + "' AND isshow = 1 ORDER BY showindex,reviewdate DESC";
        DataTable dt = utils.ExecuteDataTable(sql);
        string index = string.Empty;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            index = utils.NewRequest("index_" + dt.Rows[i]["orderid"].ToString(), utils.RequestType.Form);
            if (index.Length != 0)
            {
                sql = "UPDATE TCS_TradeRate SET showindex = '" + index + "' WHERE orderid = '" + dt.Rows[i]["orderid"].ToString() + "'";
                Response.Write(sql + "<br>");
                utils.ExecuteNonQuery(sql);
            }
        }
        Response.Redirect("reviewindexnew.aspx");
    }

    /// <summary>
    /// 删除首页展示的评价信息
    /// </summary>
    /// <param name="id"></param>
    private void DelTradeRateData(string id)
    {
        string sql = "UPDATE TCS_TradeRate SET isshow = 0 WHERE orderid = '" + id + "' AND nick = '" + nick + "'";
        utils.ExecuteNonQuery(sql);

        Response.Redirect("reviewindexnew.aspx");
    }

    /// <summary>
    /// 加入首页展示的评价信息
    /// </summary>
    /// <param name="id"></param>
    private void InitTradeRateData(string id)
    {
        //根据商品ID获取商品详细信息
        string sql = "SELECT itemid FROM TCS_TradeRate WHERE orderid = '" + id + "' AND nick = '" + nick + "'";
        string itemid = utils.ExecuteString(sql);

        //发送请求获取
        string appkey = "12690738";
        string secret = "66d488555b01f7b85f93d33bc2a1c001";
        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", appkey, secret);
        ItemGetRequest request = new ItemGetRequest();
        request.Fields = "title,price,pic_url";
        request.NumIid = long.Parse(itemid);
        Item product = client.ItemGet(request, session);

        //获取最近30天商品售出数量
        sql = "SELECT COUNT(*) FROM TCS_TradeRate WHERE itemid = '" + itemid + "'";
        string sale = utils.ExecuteString(sql);

        ////获取优惠券赠送信息
        //sql = "SELECT * FROM TCS_Coupon WHERE guid = (SELECT couponid FROM TCS_ShopConfig WHERE nick = '" + nick + "')";
        string showcontent = string.Empty;
        //DataTable dt = utils.ExecuteDataTable(sql);
        //if (dt.Rows.Count != 0)
        //{
        //    showcontent = "恭喜该用户获得本店送出的满" + dt.Rows[0]["condition"].ToString() + "减" + dt.Rows[0]["num"].ToString() + "元的优惠券！";
        //}

        //获取用户等级
        sql = "SELECT buynick FROM TCS_TradeRate WHERE orderid = '" + id + "' AND nick = '" + nick + "'";
        string buynick = utils.ExecuteString(sql);

        sql = "SELECT buyerlevel FROM TCS_Customer WHERE buynick = '" + buynick + "'";
        string userlevel = utils.ExecuteString(sql);

        sql = "UPDATE TCS_TradeRate SET isshow = 1,itemname='" + product.Title + "',itemsrc='" + product.PicUrl + "',price='" + product.Price + "',sale='" + sale + "',showcontent = '" + showcontent + "',userlevel='" + userlevel + "',showindex=100 WHERE orderid = '" + id + "' AND nick = '" + nick + "'";
        //Response.Write(sql);
        
        utils.ExecuteNonQuery(sql);

        Response.Redirect("reviewindexnew.aspx");
    }

    public static string left(string str)
    {
        string newstr = string.Empty;
        if (str.Length < 25)
        {
            newstr = str;
        }
        else
        {
            newstr = "<span title='" + str + "'>" + str.Substring(0, 25) + "..</span>";
        }
        return newstr;
    }

    private void BindData()
    {
        string sql = "SELECT TOP 20 * FROM TCS_TradeRate WHERE nick = '" + nick + "' AND isshow = 1 ORDER BY showindex,reviewdate DESC";
        DataTable dt = utils.ExecuteDataTable(sql);

        rptArticle.DataSource = dt;
        rptArticle.DataBind();
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