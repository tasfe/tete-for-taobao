﻿using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Common;
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net;

public partial class top_containersale : System.Web.UI.Page
{
    public string top_session = string.Empty;
    public string nick = string.Empty;
    public string versionNo = string.Empty;
    public string top_appkey = string.Empty;
    public string app_secret = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        //签名验证
        top_appkey = "12450498";
        app_secret = "38c892fcaa5a971aec7a9effd105c7ba";



        string top_parameters = utils.NewRequest("top_parameters", utils.RequestType.QueryString).Replace(" ", "+");
        top_session = utils.NewRequest("top_session", utils.RequestType.QueryString).Replace(" ", "+");
        versionNo = utils.NewRequest("versionNo", utils.RequestType.QueryString);
        string top_sign = utils.NewRequest("top_sign", utils.RequestType.QueryString).Replace(" ", "+"); //字符串中的+在获取后会被替换成空格，要再替换回来
        string sign = utils.NewRequest("sign", utils.RequestType.QueryString).Replace(" ", "+");
        string leaseId = utils.NewRequest("leaseId", utils.RequestType.QueryString).Replace(" ", "+"); //可以从 QueryString 来获取,也可以固定 
        string timestamp = utils.NewRequest("timestamp", utils.RequestType.QueryString).Replace(" ", "+"); //可以从 QueryString 来获取 
        string agreementsign = utils.NewRequest("agreementsign", utils.RequestType.QueryString).Replace(" ", "+");


        if (!Taobao.Top.Api.Util.TopUtils.VerifyTopResponse(top_parameters, top_session, top_sign, top_appkey, app_secret))
        {
            Response.Write("top签名验证不通过，请不要非法注入");
            Response.End();
            return;
        }

        //验证客户版本参数是否正确
        if (versionNo != "")
        {
            if (!VersionVerify(app_secret, sign, top_appkey, leaseId, timestamp, versionNo))
            {
                //Response.Write("客户版本验证不通过，请不要自行修改参数");
                //Response.End();
                //return;
            }
        }
        else
        {
            versionNo = "1";
        }

        nick = Taobao.Top.Api.Util.TopUtils.DecodeTopParams(top_parameters)["visitor_nick"];
        if (nick == null || nick == "")
        {
            Response.Write("top签名验证不通过，请不要非法注入");
            Response.End();
            return;
        }

        if (agreementsign != "")
        {
            ////加密NICK
            //Rijndael_ encode = new Rijndael_("tetesoft");
            //nick = encode.Encrypt(nick);

            //Common.Cookie cookie = new Common.Cookie();
            //cookie.setCookie("top_sessionblog", top_session, 999999);
            //cookie.setCookie("top_sessiongroupbuy", top_session, 999999);
            //cookie.setCookie("nick", nick, 999999);

            //Response.Redirect("indexnew.html");
            //return;
        }

        //判断跳转
        GetData(nick);
    }

    private bool VersionVerify(string app_secret, string top_sign, string appkey, string leaseId, string timestamp, string versionNo)
    {
        StringBuilder result = new StringBuilder();
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        result.Append(app_secret).Append("appkey").Append(appkey).Append("leaseId").Append(leaseId).Append("timestamp").Append(timestamp).Append("versionNo").Append(versionNo).Append(app_secret);
        byte[] bytes = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(result.ToString()));

        result.Remove(0, result.Length);
        for (int i = 0; i < bytes.Length; i++)
        {
            string hex = bytes[i].ToString("X");
            if (hex.Length == 1)
            {
                result.Append("0");
            }
            result.Append(hex);
        }

        //return true;
        return (top_sign == result.ToString());//是否合法
    }

    private void GetData(string nick)
    {
        string session = top_session;

        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", top_appkey, app_secret);

        //获取店铺基本信息
        UserGetRequest request = new UserGetRequest();
        request.Fields = "user_id,nick,seller_credit";
        request.Nick = nick;
	string oldNick = nick;
        User user = client.UserGet(request, session);

        if (CheckUserExits(nick))
        {
            //更新登录次数和最近登陆时间
            string sql = "UPDATE TCS_ShopSession SET session='" + top_session + "',version='" + versionNo + "' WHERE nick = '" + nick + "'";
            utils.ExecuteNonQuery(sql);
        }
        else
        {
            //记录该会员的店铺信息
            InsertUserInfo(nick);
        }

        IDictionary<string, string> param = new Dictionary<string, string>();
        string result = Post("http://gw.api.taobao.com/router/rest", top_appkey, app_secret, "taobao.increment.customer.permit", top_session, param);

        //更新用户订购信息
        CheckUser("0", nick);

        //加密NICK
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Encrypt(nick);

        Common.Cookie cookie = new Common.Cookie();
        cookie.setCookie("top_sessionsale", top_session, 999999);
        cookie.setCookie("nick", nick, 999999);

        Response.Redirect("indexsale.html");
    }


    /// <summary>
    /// 记录该会员的店铺信息
    /// </summary>
    private void InsertUserInfo(string nick)
    {
        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", top_appkey, app_secret);
        //记录店铺基本信息
        string ip = Request.UserHostAddress;
        ShopGetRequest request = new ShopGetRequest();
        request.Fields = "sid,cid,title,nick,desc,bulletin,pic_path,created,modified";
        request.Nick = nick;
        Shop shop;
        try
        {
            shop = client.ShopGet(request);
        }
        catch
        {
            Response.Write("没有店铺的淘宝会员不可使用该应用，如果您想继续使用，请先去淘宝网开个属于您自己的店铺！<br> <a href='http://www.taobao.com/'>返回</a>");
            Response.End();
            return;
        }

        //获取版本号
        string version = "9";
        //IDictionary<string, string> param = new Dictionary<string, string>();
        //param.Add("article_code", "service-0-22904");
        //param.Add("nick", nick);

        //string result = PostJson("http://gw.api.taobao.com/router/rest", top_appkey, app_secret, "taobao.vas.subscribe.get", top_session, param);
        //if (result.IndexOf("\"article_user_subscribes\":{}") == -1)
        //{
        //    Regex reg = new Regex(@"""item_code"":""([^""]*)""", RegexOptions.IgnoreCase);
        //    //更新店铺的版本号
        //    MatchCollection match = reg.Matches(result);
        //    for (int j = 0; j < match.Count; j++)
        //    {
        //        version = match[j].Groups[1].ToString().Replace("service-0-22904-", "");

        //        if (version == "9")
        //        {
        //            version = "3";
        //        }

        //        if (int.Parse(version) <= 3)
        //        {
        //            break;
        //        }
        //    }
        //}

        //记录到本地数据库
        string sql = "INSERT INTO TCS_ShopSession (" +
                       "sid, " +
                       "nick, " +
                       "typ, " +
                       "version, " +
                       "session" +
                   " ) VALUES ( " +
                       " '" + shop.Sid + "', " +
                       " '" + shop.Nick + "', " +
                       " 'taobao', " +
                       " '" + version + "', " +
                       " '" + top_session + "' " +
                 ") ";

        utils.ExecuteNonQuery(sql);
    }

    /// <summary>
    /// 判断该TAOBAO会员的店铺是否有记录
    /// </summary>
    /// <param name="nick"></param>
    /// <returns></returns>
    private bool CheckUserExits(string nick)
    {
        string sql = "SELECT * FROM TCS_ShopSession WHERE nick = '" + nick + "'";

        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void CheckUser(string t, string u)
    {
        
    }

    #region top api
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

    public static string PostJson(string url, string appkey, string appSecret, string method, string session,
    IDictionary<string, string> param)
    {
        #region -----API系统参数----
        param.Add("app_key", appkey);
        param.Add("method", method);
        param.Add("session", session);
        param.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        param.Add("format", "json");
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
    #endregion
}
