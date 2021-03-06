﻿using System;
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

public partial class top_review_setting : System.Web.UI.Page
{
    public string couponstr = string.Empty;
    public string couponid = string.Empty;
    public string session = string.Empty;
    public string nick = string.Empty;
    public string itemid = string.Empty;
    public string mindate = string.Empty;
    public string maxdate = string.Empty;
    public string itemstr = string.Empty;
    public string isfree = string.Empty;
    public string iscoupon = string.Empty;
    public string issendmsg = string.Empty;
    public string iskefu = string.Empty;
    public string iscancelauto = string.Empty;
    public string iskeyword = string.Empty;
    public string versionpub = string.Empty;
    public string isalipay = string.Empty;
    public string alipaystr = string.Empty;
    public string alipayid = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        session = cookie.getCookie("top_sessionsale");
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Decrypt(taobaoNick);

        BindData();
    }

    /// <summary>
    /// 数据绑定
    /// </summary>
    private void BindData()
    {
        string sql = "SELECT * FROM TCS_ShopConfig WHERE nick = '" + nick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            mindate = dt.Rows[0]["mindate"].ToString();
            maxdate = dt.Rows[0]["maxdate"].ToString();
            iscoupon = dt.Rows[0]["iscoupon"].ToString();
            issendmsg = dt.Rows[0]["issendmsg"].ToString();
            iskefu = dt.Rows[0]["iskefu"].ToString();
            iscancelauto = dt.Rows[0]["iscancelauto"].ToString();
            iskeyword = dt.Rows[0]["iskeyword"].ToString();
            couponid = dt.Rows[0]["couponid"].ToString();
            isalipay = dt.Rows[0]["isalipay"].ToString();
            alipayid = dt.Rows[0]["alipayid"].ToString();
        }
        else
        { 
            //设置默认值
            mindate = "3";
            maxdate = "6";
            iscoupon = "0";
            issendmsg = "1";
            iscancelauto = "1";
            iskeyword = "0";
            isalipay = "0";
            
            ////默认B店开启审核
            //string typ = utils.ExecuteString("SELECT typ FROM TCS_ShopConfig WHERE nick = '" + nick + "'");
            //if (typ == "C")
            //{
                iskefu = "0";
            //}
            //else
            //{
            //    iskefu = "1";
            //}

            //如果订购的不是VIP版，则默认将审核关闭
            sql = "SELECT * FROM TCS_ShopSession WHERE nick = '" + nick + "'";
            DataTable dtTest = utils.ExecuteDataTable(sql);
            if (dtTest.Rows.Count != 0)
            {
                string flag = dtTest.Rows[0]["version"].ToString();
                if (flag != "3")
                {
                    iskefu = "0";
                    iskeyword = "0";
                }
            }
        }

        //数据绑定
        DataTable dtCoupon = utils.ExecuteDataTable("SELECT * FROM TCS_Coupon WHERE nick = '" + nick + "' AND isdel = 0");
        couponstr = "<select name='couponid'>";
        for (int i = 0; i < dtCoupon.Rows.Count; i++)
        {
            if (dtCoupon.Rows[i]["guid"].ToString().Trim() == couponid.Trim())
            {
                couponstr += "<option value='" + dtCoupon.Rows[i]["guid"].ToString() + "' selected>" + dtCoupon.Rows[i]["name"].ToString() + " - " + dtCoupon.Rows[i]["num"].ToString() + "元</option>";
            }
            else
            {
                couponstr += "<option value='" + dtCoupon.Rows[i]["guid"].ToString() + "'>" + dtCoupon.Rows[i]["name"].ToString() + " - " + dtCoupon.Rows[i]["num"].ToString() + "元</option>";
            }
        }
        couponstr += "</select>";



        dtCoupon = utils.ExecuteDataTable("SELECT * FROM TCS_Alipay WHERE nick = '" + nick + "' AND isdel = 0");
        alipaystr = "<select name='alipayid'>";
        for (int i = 0; i < dtCoupon.Rows.Count; i++)
        {
            if (dtCoupon.Rows[i]["guid"].ToString().Trim() == couponid.Trim())
            {
                alipaystr += "<option value='" + dtCoupon.Rows[i]["guid"].ToString() + "' selected>" + dtCoupon.Rows[i]["name"].ToString() + " - " + dtCoupon.Rows[i]["num"].ToString() + "元</option>";
            }
            else
            {
                alipaystr += "<option value='" + dtCoupon.Rows[i]["guid"].ToString() + "'>" + dtCoupon.Rows[i]["name"].ToString() + " - " + dtCoupon.Rows[i]["num"].ToString() + "元</option>";
            }
        }
        alipaystr += "</select>";
    }

    public static string check(string str, string val)
    {
        if (str == val)
        {
            return "checked";
        }
        return "";
    }

    /// <summary>
    /// 保存设置
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {
        string appkey = "12159997";
        string secret = "614e40bfdb96e9063031d1a9e56fbed5";

        //先判断是否有记录
        string sql = "SELECT COUNT(*) FROM TCS_ShopConfig WHERE nick = '" + nick + "'";
        string count = utils.ExecuteString(sql);
        if (count == "0")
        {
            sql = "INSERT INTO TCS_ShopConfig (" +
                        "nick, " +
                        "iscoupon, " +
                        "couponid, " +
                        "iskefu, " +
                        "mindate, " +
                        "maxdate, " +
                        "iscancelauto, " +
                        "iskeyword, " +
                        "sessionold, " +
                        "isalipay, " +
                        "alipayid, " +
                        "total, " +
                        "issendmsg " +
                    " ) VALUES ( " +
                        " '" + nick + "', " +
                        " '" + utils.NewRequest("iscoupon", utils.RequestType.Form) + "', " +
                        " '" + utils.NewRequest("couponid", utils.RequestType.Form) + "', " +
                        " '" + utils.NewRequest("iskefu", utils.RequestType.Form) + "', " +
                        " '" + utils.NewRequest("mindate", utils.RequestType.Form) + "', " +
                        " '" + utils.NewRequest("maxdate", utils.RequestType.Form) + "', " +
                        " '" + utils.NewRequest("iscancelauto", utils.RequestType.Form) + "', " +
                        " '" + utils.NewRequest("iskeyword", utils.RequestType.Form) + "', " +
                        " '" + session + "', " +
                        " '" + utils.NewRequest("isalipay", utils.RequestType.Form) + "', " +
                        " '" + utils.NewRequest("alipayid", utils.RequestType.Form) + "', " +
                        " '100', " +
                        " '" + utils.NewRequest("issendmsg", utils.RequestType.Form) + "' " +
                    ") ";
            utils.ExecuteNonQuery(sql);
        }
        else
        {
            sql = "UPDATE TCS_ShopConfig SET " +
                        "iscoupon = '" + utils.NewRequest("iscoupon", utils.RequestType.Form) + "', " +
                        "couponid = '" + utils.NewRequest("couponid", utils.RequestType.Form) + "', " +
                        "iskefu = '" + utils.NewRequest("iskefu", utils.RequestType.Form) + "', " +
                        "mindate = '" + utils.NewRequest("mindate", utils.RequestType.Form) + "', " +
                        "maxdate = '" + utils.NewRequest("maxdate", utils.RequestType.Form) + "', " +
                        "updatedate = GETDATE(), " +
                        "iscancelauto = '" + utils.NewRequest("iscancelauto", utils.RequestType.Form) + "', " +
                        "iskeyword = '" + utils.NewRequest("iskeyword", utils.RequestType.Form) + "', " +
                        "isalipay = '" + utils.NewRequest("isalipay", utils.RequestType.Form) + "', " +
                        "alipayid = '" + utils.NewRequest("alipayid", utils.RequestType.Form) + "', " +
                        "sessionold = '" + session + "', " +
                        "issendmsg = '" + utils.NewRequest("issendmsg", utils.RequestType.Form) + "' " +
                    "WHERE nick = '" + nick + "'";
            //Response.Write(sql);
            utils.ExecuteNonQuery(sql);
        }

        sql = "INSERT INTO TCS_ShopActLog (nick, typ, message) VALUES ('" + nick + "', 'setting', '" + sql.Replace("'", "''") + "')";
        utils.ExecuteNonQuery(sql);

        Response.Write("<script>alert('保存成功！');window.location.href='setting.aspx';</script>");
        Response.End();
        return;
    }


    private string left(string str, int len)
    {
        if (str.Length > len)
        {
            str = str.Substring(0, len);
        }

        return str;
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
}