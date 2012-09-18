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
using System.Web.Security;

public partial class top_crm_alipaysend : System.Web.UI.Page
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

    public string typ = string.Empty;

    public string count1 = string.Empty;
    public string count2 = string.Empty;
    public string count3 = string.Empty;
    public string count4 = string.Empty;
    public string count5 = string.Empty;
    public string count6 = string.Empty;
    public string count7 = string.Empty;
    public string count8 = string.Empty;
    public string count9 = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        session = cookie.getCookie("top_sessiongroupbuy");
        string iscrm = cookie.getCookie("iscrm");
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Decrypt(taobaoNick);
        typ = utils.NewRequest("typ", utils.RequestType.QueryString);


        //过期判断
        if (string.IsNullOrEmpty(taobaoNick))
        {
            string msg = "尊敬的淘宝卖家，非常抱歉的告诉您，您还没有购买此服务或者登录信息已失效，如需继续使用请<a href='http://fuwu.taobao.com/serv/detail.htm?service_id=4545' target='_blank'>进入该服务</a>，谢谢！";
            Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
            Response.End();
            return;
        }

        //过期判断
        if (!IsBuy(nick))
        {
            string msg = "尊敬的" + nick + "，非常抱歉的告诉您，只有VIP版本才能使用【客户关系营销】功能，如需继续使用请<a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22904-3:1;' target='_blank'>购买高级会员服务</a>，谢谢！";
            Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
            Response.End();
            return;
        }

        BindData();
    }

    /// <summary>
    /// 判断该用户是否订购了该服务
    /// </summary>
    /// <param name="nick"></param>
    /// <returns></returns>
    private bool IsBuy(string nick)
    {
        string sql = "SELECT plus FROM TCS_ShopSession WHERE nick = '" + nick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            string plus = dt.Rows[0][0].ToString();
            if (plus.IndexOf("crm") != -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    private void BindData()
    {
        //数据绑定
        DataTable dtCoupon = utils.ExecuteDataTable("SELECT * FROM TCS_Alipay WHERE nick = '" + nick + "' AND isdel = 0");

        if (dtCoupon.Rows.Count <= 0)
        {
            Response.Write("<script>alert('请先创建1张支付宝红包才能可以给买家赠送！');window.location.href='../reviewnew/alipayadd.aspx'</script>");
            Response.End();
        }

        couponstr = "<select name='couponid'>";
        for (int i = 0; i < dtCoupon.Rows.Count; i++)
        {
            couponstr += "<option value='" + dtCoupon.Rows[i]["guid"].ToString() + "'>" + dtCoupon.Rows[i]["name"].ToString() + " - " + dtCoupon.Rows[i]["num"].ToString() + "元 已发【" + dtCoupon.Rows[i]["used"].ToString() + "】张</option>";
        }
        couponstr += "</select>";

        string sql = string.Empty;
        //sql = "SELECT COUNT(*) FROM TCS_Customer WHERE nick = '" + nick + "'";
        //count1 = utils.ExecuteString(sql);

        //sql = "SELECT COUNT(*) FROM TCS_Customer WHERE nick = '" + nick + "' AND tradecount = 0";
        //count2 = utils.ExecuteString(sql);

        //sql = "SELECT COUNT(*) FROM TCS_Customer WHERE nick = '" + nick + "' AND tradecount = 1";
        //count3 = utils.ExecuteString(sql);

        //sql = "SELECT COUNT(*) FROM TCS_Customer WHERE nick = '" + nick + "' AND tradecount = 2";
        //count4 = utils.ExecuteString(sql);

        //sql = "SELECT COUNT(*) FROM TCS_Customer WHERE nick = '" + nick + "' AND grade = 0";
        //count5 = utils.ExecuteString(sql);

        //sql = "SELECT COUNT(*) FROM TCS_Customer WHERE nick = '" + nick + "' AND grade = 1";
        //count6 = utils.ExecuteString(sql);

        //sql = "SELECT COUNT(*) FROM TCS_Customer WHERE nick = '" + nick + "' AND grade = 2";
        //count7 = utils.ExecuteString(sql);

        //sql = "SELECT COUNT(*) FROM TCS_Customer WHERE nick = '" + nick + "' AND grade = 3";
        //count8 = utils.ExecuteString(sql);

        //sql = "SELECT COUNT(*) FROM TCS_Customer WHERE nick = '" + nick + "' AND grade = 4";
        //count9 = utils.ExecuteString(sql);

        sql = "SELECT * FROM TCS_Group WHERE nick = '" + nick + "' AND isdel = 0";
        DataTable dt = utils.ExecuteDataTable(sql);

        rptGroup.DataSource = dt;
        rptGroup.DataBind();
    }

    /// <summary>
    /// 保存设置
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {
        //执行优惠券赠送行为
        string appkey = "12159997";
        string secret = "614e40bfdb96e9063031d1a9e56fbed5";
        string sql = string.Empty;

        string couponid = utils.NewRequest("couponid", utils.RequestType.Form);
        string buynick = "";// this.txtBuyerNick.Text;

        //获取淘宝优惠券ID
        sql = "SELECT guid FROM TCS_Coupon WHERE taobaocouponid = '" + couponid + "'";
        string guid = utils.ExecuteString(sql);

        string typ = utils.NewRequest("typ", utils.RequestType.Form);
        string condition = string.Empty;

        switch (typ)
        {
            case "0":
                condition = " AND b.tradecount = 0";
                break;
            case "1":
                condition = " AND b.tradecount = 1";
                break;
            case "2":
                condition = " AND b.tradecount > 1";
                break;
            case "a":
                condition = " AND b.grade = 0";
                break;
            case "b":
                condition = " AND b.grade = 1";
                break;
            case "c":
                condition = " AND b.grade = 2";
                break;
            case "d":
                condition = " AND b.grade = 3";
                break;
            case "e":
                condition = " AND b.grade = 4";
                break;
        }

        if (typ.Length > 10)
        {
            condition = " AND b.groupguid = '" + typ + "'";
        }

        int index = 0;
        int err = 0;
        string errtext = string.Empty;
        string phone = string.Empty;
        string shopname = string.Empty;

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

        sql = "SELECT * FROM TCS_Customer b WHERE b.nick = '" + nick + "' " + condition + "";
        Response.Write(sql);
        dt = utils.ExecuteDataTable(sql);
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            buynick = dt.Rows[i]["buynick"].ToString();
            phone = dt.Rows[i]["mobile"].ToString();

            //先看还有没有短信了
            sql = "SELECT total FROM TCS_ShopConfig WHERE nick = '" + nick + "'";
            //Response.Write(sql + "<br>");
            string totalAlipay = utils.ExecuteString(sql);
            if (int.Parse(totalAlipay) > 0)
            {
                //判断红包是否有效
                sql = "SELECT * FROM TCS_Alipay WITH (NOLOCK) WHERE guid = '" + couponid + "' AND DATEDIFF(d, GETDATE(), enddate) > 0 AND used < count";
                //Response.Write(sql + "<br>");
                DataTable dtAlipayDetail = utils.ExecuteDataTable(sql);
                if (dtAlipayDetail.Rows.Count != 0)
                {
                    //判断用户获取的优惠券是否超过了每人的最大领取数量
                    sql = "SELECT COUNT(*) FROM TCS_AlipayDetail WHERE guid = '" + couponid + "' AND buynick = '" + buynick + "'";
                    //Response.Write(sql + "<br>");
                    string alipayCount = utils.ExecuteString(sql);
                    if (int.Parse(alipayCount) < int.Parse(dtAlipayDetail.Rows[0]["per"].ToString()))
                    {
                        //赠送支付宝红包
                        sql = "SELECT TOP 1 * FROM TCS_AlipayDetail WITH (NOLOCK) WHERE guid = '" + couponid + "' AND issend = 0";
                        //Response.Write(sql + "<br>");
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

                                sql = "UPDATE TCS_Alipay SET used = used + 1 WHERE guid = '" + couponid + "'";
                                utils.ExecuteNonQuery(sql);

                                //更新优惠券已经赠送数量
                                sql = "UPDATE TCS_AlipayDetail SET issend = 1,buynick = '" + buynick + "',senddate = GETDATE(), orderid = '0' WHERE guid = '" + couponid + "' AND card = '" + dtAlipayDetailList.Rows[0]["card"].ToString() + "'";
                                utils.ExecuteNonQuery(sql);


                                //更新短信数量
                                sql = "UPDATE TCS_ShopConfig SET used = used + 1,total = total-1 WHERE nick = '" + nick + "'";
                                utils.ExecuteNonQuery(sql);

                                index++;
                            }
                            else
                            {
                                err++;
                            }
                        }
                        else
                        {
                            err++;
                        }
                    }
                    else
                    {
                        err++;
                    }
                }
                else
                {
                    err++;
                }
            }
            else
            {
                err++;
            }
        }

        if (err == 0)
        {
            Response.Write("<script>alert('赠送完毕，成功赠送" + index.ToString() + "张，失败" + err.ToString() + "张！');window.location.href='../reviewnew/alipay.aspx';</script>");
        }
        else
        {
            Response.Write("<script>alert('赠送完毕，成功赠送" + index.ToString() + "张，失败" + err.ToString() + "张，失败原因" + errtext + "！');window.location.href='../reviewnew/alipay.aspx';</script>");
        }
        Response.End();
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