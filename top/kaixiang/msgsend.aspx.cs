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

public partial class top_groupbuy_msgsend : System.Web.UI.Page
{
    public string couponstr = string.Empty;
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

    protected void Page_Load(object sender, EventArgs e)
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        session = cookie.getCookie("top_sessionkai");
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Decrypt(taobaoNick);

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

        BindData();
    }

    private void BindData()
    {
        //数据绑定
        DataTable dtCoupon = utils.ExecuteDataTable("SELECT * FROM TCS_Coupon WHERE nick = '" + nick + "' AND isdel = 0 ORDER BY startdate DESC");
        couponstr = "<select name='couponid'>";
        for (int i = 0; i < dtCoupon.Rows.Count; i++)
        {
            couponstr += "<option value='" + dtCoupon.Rows[i]["taobaocouponid"].ToString() + "'>" + dtCoupon.Rows[i]["name"].ToString() + " - " + dtCoupon.Rows[i]["num"].ToString() + "元</option>";
        }
        couponstr += "</select>";
    }

     /// <summary>
    /// 保存设置
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {
        //执行优惠券赠送行为
        string appkey = "12223169";
        string secret = "ff3d3442ab809930d187623ffad8e91e";
        string sql = string.Empty;

        string couponid = utils.NewRequest("couponid", utils.RequestType.Form);
        string buynick = this.txtBuyerNick.Text;

        IDictionary<string, string> param = new Dictionary<string, string>();
        param.Add("coupon_id", couponid);
        param.Add("buyer_nick", buynick);

        string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.promotion.coupon.send", session, param);
        Regex reg = new Regex(@"<coupon_number>([^<]*)</coupon_number>", RegexOptions.IgnoreCase);
        MatchCollection match = reg.Matches(result);

        //如果失败
        if (!reg.IsMatch(result))
        {
            string err = new Regex(@"<reason>([^<]*)</reason>", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
            Response.Write("<script>alert('【系统错误】：" + err + "，请稍后再试或者联系客服人员！');window.location.href='msgsend.aspx';</script>");
        }
        else
        {
            string number = match[0].Groups[1].ToString();

            //赠送优惠券
            sql = "INSERT INTO TCS_CouponSend (" +
                                "nick, " +
                                "guid, " +
                                "buynick, " +
                                "taobaonumber " +
                            " ) VALUES ( " +
                                " '" + nick + "', " +
                                " '" + couponid + "', " +
                                " '" + buynick + "', " +
                                " '" + number + "' " +
                            ") ";
            utils.ExecuteNonQuery(sql);

            //更新优惠券已经赠送数量
            sql = "UPDATE TCS_Coupon SET used = used + 1 WHERE guid = " + couponid;
            utils.ExecuteNonQuery(sql);
            Response.Write("<script>alert('赠送成功！');window.location.href='msgsend.aspx';</script>");
        }
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