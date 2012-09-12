﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Net;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Data;

public partial class top_review_couponmodify : System.Web.UI.Page
{
    public string session = string.Empty;
    public string nick = string.Empty;
    public string endsenddate = string.Empty;
    public string enddate = string.Empty;
    public string coupon_name = string.Empty;
    public string price = string.Empty;
    public string condition = string.Empty;
    public string total = string.Empty;
    public string per = string.Empty;
    public string id = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        id = utils.NewRequest("id", utils.RequestType.QueryString);
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        session = cookie.getCookie("top_sessiongroupbuy");
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Decrypt(taobaoNick);

        if (!IsPostBack)
        {
            if (id != "")
            {
                BindData();
            }
        }
    }

    /// <summary>
    /// 数据绑定
    /// </summary>
    private void BindData()
    {
        string sql = "SELECT * FROM TCS_Coupon WHERE guid = '" + id + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            endsenddate = DateTime.Parse(dt.Rows[0]["enddate"].ToString()).ToString("yyyy-MM-dd");
            enddate = DateTime.Parse(dt.Rows[0]["endsenddate"].ToString()).ToString("yyyy-MM-dd");
            coupon_name = dt.Rows[0]["name"].ToString();
            price = dt.Rows[0]["num"].ToString();
            condition = dt.Rows[0]["condition"].ToString();
            total = dt.Rows[0]["count"].ToString();
            per = dt.Rows[0]["per"].ToString();
        }
        else
        {
            Response.Write("<script>alert('您所选择的优惠券不存在！');history.go(-1);</script>");
            Response.End();
        }
    }

    //创建新优惠券
    protected void Button1_Click(object sender, EventArgs e)
    {
        string appkey = "12159997";
        string secret = "614e40bfdb96e9063031d1a9e56fbed5";
        
        string price = utils.NewRequest("price", utils.RequestType.Form);
        string pricebak = utils.NewRequest("pricebak", utils.RequestType.Form);
        string condition = utils.NewRequest("condition", utils.RequestType.Form);
        string conditionbak = utils.NewRequest("conditionbak", utils.RequestType.Form);
        //反过来获取，不用修改主程序
        string end_time = utils.NewRequest("endsenddate", utils.RequestType.Form);
        string endsenddate = utils.NewRequest("end_time", utils.RequestType.Form);
        string end_timebak = utils.NewRequest("end_timebak", utils.RequestType.Form);
        string coupon_name = utils.NewRequest("coupon_name", utils.RequestType.Form);
        string total = utils.NewRequest("total", utils.RequestType.Form);
        string per = utils.NewRequest("per", utils.RequestType.Form);
        string coupon_id = string.Empty;

        if (endsenddate != end_timebak || price != pricebak || condition != conditionbak)
        {
            //创建活动相关人群
            string guid = Guid.NewGuid().ToString();
            IDictionary<string, string> param = new Dictionary<string, string>();
            param.Add("denominations", price);
            param.Add("end_time", endsenddate);
            param.Add("condition", condition);
            string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.promotion.coupon.add", session, param);

            if (result.IndexOf("Insufficient") != -1)
            {
                Response.Write("<b>优惠券创建失败，错误原因：</b><br><font color='red'>您的session已经失效，需要重新授权</font><br><a href='http://container.api.taobao.com/container?appkey=12159997&scope=promotion' target='_parent'>重新授权</a>");
                Response.End();
                return;
            }

            if (result.IndexOf("error_response") != -1)
            {
                if (result.IndexOf("end_time") != -1)
                {
                    Response.Write("<b>优惠券创建失败，错误原因：</b><br><font color='red'>错误的日期格式，正确的日期格式为：2011-01-01</font><br><a href='javascript:history.go(-1)'>重新添加</a>");
                    Response.End();
                    return;
                }

                string err = new Regex(@"<sub_msg>([^<]*)</sub_msg>", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
                if (err.Length == 0)
                {
                    err = "淘宝系统错误，请稍后重试！";
                }

                Response.Write("<b>优惠券创建失败，错误原因：</b><br><font color='red'>" + result + "</font><br><a href='javascript:history.go(-1)'>重新添加</a>");
                Response.End();
                return;
            }

            coupon_id = new Regex(@"<coupon_id>([^<]*)</coupon_id>", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();


            if (coupon_id == "")
            {
                Response.Write("<b>优惠券创建失败，错误原因：</b><br><font color='red'>" + result + "</font><br><a href='javascript:history.go(-1)'>重新添加</a>");
                Response.End();
                return;
            }

            try
            {
                DateTime d = DateTime.Parse(end_time);
            }
            catch
            {
                Response.Write("<b>优惠券创建失败，错误原因：</b><br><font color='red'>“" + end_time + "”该时间不存在或者格式错误</font><br><a href='javascript:history.go(-1)'>重新添加</a>");
                Response.End();
                return;
            }


            try
            {
                DateTime d = DateTime.Parse(endsenddate);
            }
            catch
            {
                Response.Write("<b>优惠券创建失败，错误原因：</b><br><font color='red'>“" + endsenddate + "”该时间不存在或者格式错误</font><br><a href='javascript:history.go(-1)'>重新添加</a>");
                Response.End();
                return;
            }
        }

        string sql = "UPDATE TCS_Coupon SET " +
                        "name = '" + coupon_name + "', " ;

                        //如果没修改则不该这条

                        if (endsenddate != end_timebak || price != pricebak || condition != conditionbak)
                        {
                            sql += "taobaocouponid = '" + coupon_id + "', ";
                        }

                        sql += "num = '" + price + "', " +
                        "enddate = '" + end_time + "', " +
                        "endsenddate = '" + endsenddate + "', " +
                        "count = '" + total + "', " +
                        "per = '" + per + "', " +
                        "condition = '" + condition + "' " +
                    "WHERE guid = '" + id + "' AND nick = '" + nick + "'";
        utils.ExecuteNonQuery(sql);

        //Response.Write("<br><br>" + sql);
        Response.Redirect("couponlist.aspx");
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