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
        string appkey = "12159997";
        string secret = "614e40bfdb96e9063031d1a9e56fbed5";
        string sql = string.Empty;

        string couponid = utils.NewRequest("couponid", utils.RequestType.Form);
        string buynick = this.txtBuyerNick.Text;

        //获取淘宝优惠券ID
        sql = "SELECT guid FROM TCS_Coupon WHERE taobaocouponid = '" + couponid + "'";
        string guid = utils.ExecuteString(sql);

        //判断优惠券赠送限制
        sql = "SELECT per FROM TCS_Coupon WITH (NOLOCK) WHERE guid = '" + guid + "' ";
        string max = utils.ExecuteString(sql);

        //判断该用户是否超过了最大赠送
        sql = "SELECT guid FROM TCS_CouponSend WITH (NOLOCK) WHERE buynick= '" + buynick + "' AND guid = '" + guid + "' AND taobaonumber NOT IN (SELECT couponnumber FROM TCS_Trade WHERE nick = '" + nick + "' AND iscoupon = 1)";
        DataTable dtCoupon = utils.ExecuteDataTable(sql);
        if (dtCoupon.Rows.Count >= int.Parse(max))
        {
            //退出    
            Response.Write("<script>alert('赠送失败，买家获得的优惠券不会超过您设定的每人获取上限！');window.location.href='msgsend.aspx';</script>");
            Response.End();
        }
        else
        {

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
                if (err.Length == 0)
                {
                    err = new Regex(@"<sub_msg>([^<]*)</sub_msg>", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
                }
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
                                    " '" + guid + "', " +
                                    " '" + buynick + "', " +
                                    " '" + number + "' " +
                                ") ";
                //Response.Write(sql);
                //Response.End();
                utils.ExecuteNonQuery(sql);

                //发送短信
                SendCouponMsg(buynick);

                //更新优惠券已经赠送数量
                sql = "UPDATE TCS_Coupon SET used = used + 1 WHERE guid = " + couponid;
                utils.ExecuteNonQuery(sql);
                Response.Write("<script>alert('赠送成功！');window.location.href='msgsend.aspx';</script>");
            }
        }
    }

    private void SendCouponMsg(string buynick)
    {
        string sql = string.Empty;
        string giftflag = string.Empty;
        string giftcontent = string.Empty;
        string shopname = string.Empty;
        string phone = string.Empty;

        sql = "SELECT * FROM TCS_ShopConfig WITH (NOLOCK) WHERE nick = '" + nick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            giftflag = dt.Rows[0]["giftflag"].ToString();
            giftcontent = dt.Rows[0]["giftcontent"].ToString();
            shopname = dt.Rows[0]["shopname"].ToString();
        }
        else
        {
            return;
        }

        //发送短信
        if (1 == 1) //短信测试中
        {
            //判断是否开启该短信发送节点
            if (giftflag == "1")
            {
                //判断是否还有短信可发
                sql = "SELECT total FROM TCS_ShopConfig WHERE nick = '" + nick + "'";
                string total = utils.ExecuteString(sql);

                sql = "SELECT * FROM TCS_Trade WITH (NOLOCK) WHERE buynick = '" + buynick + "'";
                dt = utils.ExecuteDataTable(sql);
                if (dt.Rows.Count != 0)
                {
                    phone = dt.Rows[0]["mobile"].ToString();
                }
                else
                {
                    return;
                }

                if (int.Parse(total) > 0)
                {
                    //每张物流订单最多提示一次
                    sql = "SELECT COUNT(*) FROM TCS_MsgSend WITH (NOLOCK) WHERE DATEDIFF(d, adddate, GETDATE()) = 0 AND  buynick = '" + buynick + "' AND typ = 'gift'";
                    string giftCount = utils.ExecuteString(sql);

                    if (giftCount == "0")
                    {
                        //开始发送
                        string msg = GetMsg(giftcontent, shopname, buynick);

                        //强行截取
                        if (msg.Length > 66)
                        {
                            msg = msg.Substring(0, 66);
                        }

                        string result = SendGuodu(phone, msg);

                        if (result != "0")
                        {
                            string number = "1";

                            //如果内容超过70个字则算2条
                            if (msg.Length > 66)
                            {
                                number = "2";
                            }

                            //记录短信发送记录
                            sql = "INSERT INTO TCS_MsgSend (" +
                                                "nick, " +
                                                "buynick, " +
                                                "mobile, " +
                                                "[content], " +
                                                "guoduid, " +
                                                "num, " +
                                                "typ " +
                                            " ) VALUES ( " +
                                                " '" + nick + "', " +
                                                " '" + buynick + "', " +
                                                " '" + phone + "', " +
                                                " '" + msg.Replace("'", "''") + "', " +
                                                " '" + result + "', " +
                                                " '" + number + "', " +
                                                " 'gift' " +
                                            ") ";
                            utils.ExecuteNonQuery(sql);

                            //更新短信数量
                            sql = "UPDATE TCS_ShopConfig SET used = used + " + number + ",total = total-" + number + " WHERE nick = '" + nick + "'";
                            utils.ExecuteNonQuery(sql);
                        }
                        else
                        {

                        }
                    }
                }
            }
        }
    }


    public string SendGuodu(string phone, string msg)
    {
        string uid = "haopyl";
        string pass = "hao1234";
        string result = string.Empty;

        msg = UrlEncode(msg + "【淘宝】");

        string param = "OperID=" + uid + "&OperPass=" + pass + "&SendTime=&ValidTime=&AppendID=1234&DesMobile=" + phone + "&Content=" + msg + "&ContentType=8";
        byte[] bs = Encoding.ASCII.GetBytes(param);

        HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("http://221.179.180.158:9001/QxtSms/QxtFirewall" + "?" + param);

        //File.WriteAllText(Server.MapPath("test.txt"), "http://221.179.180.158:9001/QxtSms/QxtFirewall" + "?" + param);

        req.Method = "GET";

        using (HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse())
        {
            using (StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.GetEncoding("GB2312")))
            {
                string content = reader.ReadToEnd();

                Regex reg = new Regex(@"<msgid>([^<]*)</msgid>", RegexOptions.IgnoreCase);
                if (reg.IsMatch(content))
                {
                    content = Regex.Match(content, @"<msgid>([^<]*)</msgid>").Groups[1].ToString();
                }

                return content;
            }
        }
    }

    private string GetMsg(string giftcontent, string shopname, string buynick)
    {
        string giftstr = "优惠券";

        giftcontent = giftcontent.Replace("[shopname]", shopname);
        giftcontent = giftcontent.Replace("[buynick]", buynick);
        giftcontent = giftcontent.Replace("[gift]", giftstr);//.Replace("[buynick]", buynick);

        return giftcontent;
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

    //public static string SendMessage(string phone, string msg)
    //{
    //    //有客户没有手机号也发送短信
    //    if (phone.Length == 0)
    //    {
    //        return "0";
    //    }

    //    string uid = "terrylv";
    //    string pass = "123456";

    //    msg = HttpUtility.UrlEncode(msg);

    //    string param = "username=" + uid + "&password=" + pass + "&method=sendsms&mobile=" + phone + "&msg=" + msg;
    //    byte[] bs = Encoding.ASCII.GetBytes(param);
    //    HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("http://sms3.eachwe.com/api.php");
    //    req.Method = "POST";
    //    req.ContentType = "application/x-www-form-urlencoded";
    //    req.ContentLength = bs.Length;

    //    using (Stream reqStream = req.GetRequestStream())
    //    {
    //        reqStream.Write(bs, 0, bs.Length);
    //    }

    //    using (HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse())
    //    {
    //        using (StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.GetEncoding("GB2312")))
    //        {
    //            string content = reader.ReadToEnd();

    //            if (content.IndexOf("<error>0</error>") == -1)
    //            {
    //                //发送失败
    //                return content;
    //            }
    //            else
    //            {
    //                //发送成功
    //                Regex reg = new Regex(@"<sid>([^<]*)</sid>", RegexOptions.IgnoreCase);
    //                MatchCollection match = reg.Matches(content);
    //                string number = match[0].Groups[1].ToString();
    //                return number;
    //            }
    //        }
    //    }
    //}

    public string SendMessage(string phone, string msg)
    {
        //有客户没有手机号也发送短信
        if (phone.Length < 11)
        {
            return "0";
        }

        string uid = "ZXHD-SDK-0107-XNYFLX";
        string pass = MD5AAA("WEGXBEPY").ToLower();

        msg = UrlEncode(msg);

        string param = "regcode=" + uid + "&pwd=" + pass + "&phone=" + phone + "&CONTENT=" + msg + "&extnum=11&level=1&schtime=null&reportflag=0&url=&smstype=0&key=aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        byte[] bs = Encoding.ASCII.GetBytes(param);

        HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("http://sms.pica.com/zqhdServer/sendSMS.jsp" + "?" + param);

        req.Method = "GET";

        using (HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse())
        {
            using (StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.GetEncoding("GB2312")))
            {
                string content = reader.ReadToEnd();
                //File.WriteAllText(Server.MapPath("aaa.txt"), content);

                if (content.IndexOf("<result>0</result>") == -1)
                {
                    Regex reg = new Regex(@"<result>([^<]*)</result>", RegexOptions.IgnoreCase);
                    MatchCollection match = reg.Matches(content);
                    string number = string.Empty;
                    if (reg.IsMatch(content))
                    {
                        number = match[0].Groups[1].ToString(); // match[0].Groups[1].ToString();
                    }
                    else
                    {
                        number = "888888";
                    }

                    if (number.Length > 50)
                    {
                        number = content.Substring(0, 50);
                    }
                    return number;
                }
                else
                {
                    if (content.Length > 50)
                    {
                        content = content.Substring(0, 50);
                    }

                    return content;
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