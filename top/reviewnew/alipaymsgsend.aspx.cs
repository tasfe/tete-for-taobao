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
using System.Web.Security;

public partial class top_groupbuy_alipaymsgsend : System.Web.UI.Page
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

        BindData();
    }

    private void BindData()
    {
        //数据绑定
        DataTable dtCoupon = utils.ExecuteDataTable("SELECT * FROM TCS_Alipay WHERE nick = '" + nick + "' AND isdel = 0 ORDER BY startdate DESC");
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
        string sql = string.Empty;
        string shopname = string.Empty;
        string phone = string.Empty;

        string couponid = utils.NewRequest("couponid", utils.RequestType.Form);
        string buynick = this.txtBuyerNick.Text;

        sql = "SELECT * FROM TCS_ShopConfig WITH (NOLOCK) WHERE nick = '" + nick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            shopname = dt.Rows[0]["shopname"].ToString();
        }
        else
        {
            Response.Write("<script>alert('系统错误，请重新登录！');window.location.href='alipaymsgsend.aspx';</script>");
            Response.End();
            return;
        }

        //获取该订单关联会员
        sql = "SELECT * FROM TCS_Trade WITH (NOLOCK) WHERE nick = '" + nick + "' AND buynick = '" + buynick + "' AND mobile <> ''";
        dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            phone = dt.Rows[0]["mobile"].ToString();
        }
        else
        {
            Response.Write("<script>alert('该会员的订单信息中没有记录手机号码，无法发送支付宝红包！');window.location.href='alipaymsgsend.aspx';</script>");
            Response.End();
            return;
        }

        //先看还有没有短信了
        sql = "SELECT total FROM TCS_ShopConfig WHERE nick = '" + nick + "'";
        string totalAlipay = utils.ExecuteString(sql);
        if (int.Parse(totalAlipay) > 0)
        {
            //如果有短信再开始判断
            sql = "SELECT isalipay,alipayid FROM TCS_ShopConfig WITH (NOLOCK) WHERE nick = '" + nick + "'";
            DataTable dtAlipay = utils.ExecuteDataTable(sql);
            if (dtAlipay.Rows.Count != 0)
            {
                //看看卖家是否开启了支付宝红包赠送
                if (dtAlipay.Rows[0][0].ToString() == "1")
                {
                    //判断红包是否有效
                    sql = "SELECT * FROM TCS_Alipay WITH (NOLOCK) WHERE guid = '" + dtAlipay.Rows[0][1].ToString() + "' AND DATEDIFF(d, GETDATE(), enddate) > 0 AND used < count";
                    DataTable dtAlipayDetail = utils.ExecuteDataTable(sql);
                    if (dtAlipayDetail.Rows.Count != 0)
                    {
                        //判断用户获取的优惠券是否超过了每人的最大领取数量
                        sql = "SELECT COUNT(*) FROM TCS_AlipayDetail WHERE guid = '" + dtAlipay.Rows[0][1].ToString() + "' AND buynick = '" + buynick + "'";
                        string alipayCount = utils.ExecuteString(sql);
                        if (int.Parse(alipayCount) < int.Parse(dtAlipayDetail.Rows[0]["per"].ToString()))
                        {
                            //赠送支付宝红包
                            sql = "SELECT TOP 1 * FROM TCS_AlipayDetail WITH (NOLOCK) WHERE guid = '" + dtAlipay.Rows[0][1].ToString() + "' AND issend = 0";
                            DataTable dtAlipayDetailList = utils.ExecuteDataTable(sql);
                            if (dtAlipayDetailList.Rows.Count != 0)
                            {
                                string msgAlipay = "亲，" + shopname + "赠送您支付宝红包，卡号" + dtAlipayDetailList.Rows[0]["card"].ToString() + "密码" + dtAlipayDetailList.Rows[0]["pass"].ToString() + "，您可以到支付宝绑定使用。";
                                //强行截取
                                if (msgAlipay.Length > 66)
                                {
                                    msgAlipay = msgAlipay.Substring(0, 66);
                                }

                                string result = SendMessage(phone, msgAlipay);
                                //记录短信发送记录
                                sql = "INSERT INTO TCS_MsgSend (" +
                                                    "nick, " +
                                                    "buynick, " +
                                                    "mobile, " +
                                                    "[content], " +
                                                    "yiweiid, " +
                                                    "num, " +
                                                    "typ " +
                                                " ) VALUES ( " +
                                                    " '" + nick + "', " +
                                                    " '" + buynick + "', " +
                                                    " '" + phone + "', " +
                                                    " '" + msgAlipay + "', " +
                                                    " '" + result + "', " +
                                                    " '1', " +
                                                    " 'alipay' " +
                                                ") ";

                                if (phone.Length != 0)
                                {
                                    //记录支付宝红包发送成功
                                    utils.ExecuteNonQuery(sql);

                                    sql = "UPDATE TCS_Alipay SET used = used + 1 WHERE guid = '" + dtAlipay.Rows[0][1].ToString() + "'";
                                    utils.ExecuteNonQuery(sql);

                                    //更新优惠券已经赠送数量
                                    sql = "UPDATE TCS_AlipayDetail SET issend = 1,buynick = '" + buynick + "',senddate = GETDATE(), orderid = '0' WHERE guid = '" + dtAlipay.Rows[0][1].ToString() + "' AND card = '" + dtAlipayDetailList.Rows[0]["card"].ToString() + "'";
                                    utils.ExecuteNonQuery(sql);


                                    //更新短信数量
                                    sql = "UPDATE TCS_ShopConfig SET used = used + 1,total = total-1 WHERE nick = '" + nick + "'";
                                    utils.ExecuteNonQuery(sql);
                                }
                            }
                        }
                    }
                }
            }
        }
    }





    public static string UrlEncode(string str)
    {
        StringBuilder sb = new StringBuilder();
        byte[] byStr = System.Text.Encoding.Default.GetBytes(str);
        for (int i = 0; i < byStr.Length; i++)
        {
            sb.Append(@"%" + Convert.ToString(byStr[i], 16));
        }

        return (sb.ToString());
    }

    public static string MD5AAA(string str)
    {
        return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
    }


    public static string SendMessage(string phone, string msg)
    {
        if (phone.Length == 0)
        {
            return "0";
        }

        string uid = "ZXHD-SDK-0107-XNYFLX";
        string pass = MD5AAA("WEGXBEPY").ToLower();

        msg = UrlEncode(msg);

        string param = "regcode=" + uid + "&pwd=" + pass + "&phone=" + phone + "&CONTENT=" + msg + "&extnum=11&level=1&schtime=null&reportflag=1&url=&smstype=0&key=aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        byte[] bs = Encoding.ASCII.GetBytes(param);

        HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("http://sms.pica.com:8082/zqhdServer/sendSMS.jsp" + "?" + param);

        req.Method = "GET";

        using (HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse())
        {
            using (StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.GetEncoding("GB2312")))
            {
                string content = reader.ReadToEnd();

                if (content.IndexOf("<result>0</result>") == -1)
                {
                    //发送失败
                    return content;
                }
                else
                {
                    //发送成功
                    Regex reg = new Regex(@"<sid>([^<]*)</sid>", RegexOptions.IgnoreCase);
                    MatchCollection match = reg.Matches(content);
                    string number = "888888";// match[0].Groups[1].ToString();
                    return number;
                }
            }
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