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

            if (flag == "1")
            {
                string msg = "尊敬的" + nick + "，非常抱歉的告诉您，只有专业版或者以上版本才能使用【支付宝红包】功能，如需继续使用请<a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22904-2:1;' target='_blank'>购买高级会员服务</a>，谢谢！<br><br> PS：发送的短信需要单独购买，1毛钱1条~";
                Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
                Response.End();
                return;
            }
        }
        else
        {
            string appkey = "12159997";
            string secret = "614e40bfdb96e9063031d1a9e56fbed5";
            string version = "0";
            IDictionary<string, string> param = new Dictionary<string, string>();
            param.Add("article_code", "service-0-22904");
            param.Add("nick", nick);

            string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.vas.subscribe.get", session, param);
            if (result.IndexOf("\"article_user_subscribes\":{}") == -1)
            {
                Regex reg = new Regex(@"""item_code"":""([^""]*)""", RegexOptions.IgnoreCase);
                //更新店铺的版本号
                MatchCollection match = reg.Matches(result);
                for (int j = 0; j < match.Count; j++)
                {
                    version = match[j].Groups[1].ToString().Replace("service-0-22904-", "");

                    if (version == "9")
                    {
                        version = "3";
                    }

                    if (int.Parse(version) <= 3)
                    {
                        break;
                    }
                }

                //重新给客户插入session
                sql = "INSERT INTO TCS_ShopSession (sid, nick, typ, version, session ) VALUES ( '0', '" + nick + "', 'taobao', '" + version + "', '" + session + "' )";
                utils.ExecuteNonQuery(sql);
                Response.Redirect("alipay.aspx");
            }
        }

        //判断VIP版本，只有VIP才能使用此功能
        sql = "SELECT * FROM TCS_ShopSession WHERE nick = '" + nick + "'";
        dt = utils.ExecuteDataTable(sql);
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
        DataTable dtCoupon = utils.ExecuteDataTable("SELECT * FROM TCS_Alipay WHERE nick = '" + nick + "' AND isdel = 0");
        couponstr = "<select name='couponid'>";
        for (int i = 0; i < dtCoupon.Rows.Count; i++)
        {
            couponstr += "<option value='" + dtCoupon.Rows[i]["guid"].ToString() + "'>" + dtCoupon.Rows[i]["name"].ToString() + " - " + dtCoupon.Rows[i]["num"].ToString() + "元 已发【" + dtCoupon.Rows[i]["used"].ToString() + "】张</option>";
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
        string issend = utils.NewRequest("issend", utils.RequestType.Form);

        sql = "SELECT * FROM TCS_ShopConfig WITH (NOLOCK) WHERE nick = '" + nick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            shopname = dt.Rows[0]["shopname"].ToString();
            if (shopname == "")
            {
                shopname = nick;
            }
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
            sql = "SELECT * FROM TCS_Customer WITH (NOLOCK) WHERE nick = '" + nick + "' AND buynick = '" + buynick + "' AND mobile <> ''";
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
        }

        //先看还有没有短信了
        sql = "SELECT total FROM TCS_ShopConfig WHERE nick = '" + nick + "'";
        //Response.Write(sql + "<br>");
        string totalAlipay = utils.ExecuteString(sql);
        if (int.Parse(totalAlipay) > 0)
        {
            ////如果有短信再开始判断
            //sql = "SELECT isalipay,alipayid FROM TCS_ShopConfig WITH (NOLOCK) WHERE nick = '" + nick + "'";
            ////Response.Write(sql + "<br>");
            //DataTable dtAlipay = utils.ExecuteDataTable(sql);
            //if (dtAlipay.Rows.Count != 0)
            //{
            //看看卖家是否开启了支付宝红包赠送-不判断
            //if (dtAlipay.Rows[0][0].ToString() == "1")
            if (1 == 1)
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
                    //如果客户勾选了强行赠送则不会根据每人最大领取数量进行判断
                    if (issend == "1" || int.Parse(alipayCount) < int.Parse(dtAlipayDetail.Rows[0]["per"].ToString()))
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

                            string result = SendGuodu(phone, msgAlipay);
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

                                Response.Write("<script>alert('赠送成功！');history.go(-1);</script>");
                                Response.End();

                            }
                        }
                    }
                    else
                    {
                        Response.Write("<script>alert('该买家收到的支付宝红包已经超出了您设置的每人领取上线！');history.go(-1);</script>");
                        Response.End();
                    }
                }
                else
                {
                    Response.Write("<script>alert('请确定您的支付宝红包没过期且没有领取完毕！');history.go(-1);</script>");
                    Response.End();
                }
            }
            //}
        }
        else
        {
            Response.Write("<script>alert('请确定您的账户有足够的短信！');history.go(-1);</script>");
            Response.End();
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

        File.WriteAllText(Server.MapPath("test.txt"), "http://221.179.180.158:9001/QxtSms/QxtFirewall" + "?" + param);

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