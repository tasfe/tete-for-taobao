using System;
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
using System.Web.Security;

public partial class top_reviewnew_search : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.TextBox9.Text = DateTime.Now.AddDays(-1).ToShortDateString();
        }
    }

    protected void Button9_Click(object sender, EventArgs e)
    {
        if (TextBox24.Text != "xiaoman")
        {
            return;
        }

        string sql = "INSERT INTO TCS_MsgSend (nick, buynick, typ, content) VALUES ('" + this.TextBox21.Text + "','" + this.TextBox21.Text + "','muti','" + this.TextBox25.Text.Replace("'", "''") + "')";
        utils.ExecuteNonQuery(sql);

        sql = "UPDATE TCS_ShopConfig SET total = total - " + this.TextBox22.Text + " WHERE nick = '" + this.TextBox21.Text + "'";
        utils.ExecuteNonQuery(sql);
    }

    protected void Button8_Click(object sender, EventArgs e)
    {
        if (TextBox23.Text != "xiaoman")
        {
            return;
        }
        string appkey = "12159997";
        string secret = "614e40bfdb96e9063031d1a9e56fbed5";
        string session = string.Empty;
        string nick = this.TextBox20.Text;
        string result = string.Empty;

        IDictionary<string, string> param = new Dictionary<string, string>();

        param = new Dictionary<string, string>();
        param.Add("type", "get,notify,syn");
        param.Add("nick", nick);
        result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.increment.customer.stop", session, param);

        Response.Write(result);
    }

    protected void Button7_Click(object sender, EventArgs e)
    {
        if (TextBox19.Text != "xiaoman")
        {
            return;
        }

        string nick = this.TextBox16.Text;
        string buynick = this.TextBox17.Text;
        string orderid = this.TextBox18.Text;

        string msg = "{\"packet\":{\"code\":202,\"msg\":{\"notify_trade\":{\"topic\":\"trade\",\"status\":\"TradeRated\",\"user_id\":833921326,\"nick\":\"" + nick + "\",\"modified\":\"2012-10-31 18:27:41\",\"buyer_nick\":\"" + buynick + "\",\"payment\":\"950.00\",\"oid\":" + orderid + ",\"is_3D\":true,\"tid\":" + orderid + ",\"type\":\"guarantee_trade\",\"seller_nick\":\"" + nick + "\"}}}}";

        InsertMsgLogInfo(nick, "TradeRated", msg);
    }

    protected void Button6_Click(object sender, EventArgs e)
    {
        if (TextBox15.Text != "xiaoman")
        {
            return;
        }

        string phone = this.TextBox13.Text;
        string msg = this.TextBox14.Text;
        string nick = "美杜莎之心";

        string result = SendGuodu(phone, msg);

        //记录短信发送记录 
        string sql = "INSERT INTO TCS_MsgSend (" +
                            "nick, " +
                            "buynick, " +
                            "mobile, " +
                            "[content], " +
                            "guoduid, " +
                            "num, " +
                            "typ " +
                        " ) VALUES ( " +
                            " '" + nick + "', " +
                            " '" + nick + "', " +
                            " '" + phone + "', " +
                            " '" + msg.Replace("'", "''") + "', " +
                            " '" + result + "', " +
                            " '1', " +
                            " 'test' " +
                        ") ";
        utils.ExecuteNonQuery(sql);
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



    public string SendGuodu(string phone, string msg)
    {
        string uid = "haopyl";
        string pass = "hao1234";
        string result = string.Empty;

        msg = UrlEncode(msg);

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

                return content;
            }
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (TextBox2.Text != "xiaoman")
        {
            return;
        }

        string sql = "SELECT * FROM TCS_ShopConfig WHERE nick = '" + this.TextBox1.Text + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            this.TextBox11.Text = dt.Rows[0]["phone"].ToString();
            this.TextBox12.Text = dt.Rows[0]["qq"].ToString();
        }
    }


    protected void Button5_Click(object sender, EventArgs e)
    {
        if (TextBox2.Text != "xiaoman")
        {
            return;
        }

        string sql = "UPDATE TCS_ShopConfig SET phone = '" + this.TextBox11.Text + "',qq='" + this.TextBox12.Text + "' WHERE nick = '" + this.TextBox1.Text + "'";
        utils.ExecuteNonQuery(sql);

        Response.Write("修改成功！");
    }


    protected void Button4_Click(object sender, EventArgs e)
    {
        if (TextBox10.Text != "xiaoman")
        {
            return;
        }

        string appkey = "12159997";
        string secret = "614e40bfdb96e9063031d1a9e56fbed5";
        string session = string.Empty;
        string nick = this.TextBox7.Text;
        string date = TextBox9.Text;
        string result = string.Empty;
        int total = 0;

        IDictionary<string, string> param = new Dictionary<string, string>();

        string sql = "SELECT * FROM TCS_ShopSession WHERE nick = '" + nick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            for (int i = 1; i < 200; i++)
            {
                param = new Dictionary<string, string>();
                param.Add("status", "TradeRated");
                param.Add("nick", nick);
                param.Add("start_modified", date + " 00:00:00");
                param.Add("end_modified", date + " 23:59:59");
                param.Add("page_no", i.ToString());
                param.Add("page_size", "200");
                result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.increment.trades.get", session, param);

                Regex reg1 = new Regex(@"\{""buyer_nick""[^\}]*\}", RegexOptions.IgnoreCase);
                MatchCollection match1 = reg1.Matches(result);
                for (int k = 0; k < match1.Count; k++)
                {
                    Console.Write(match1[k].Groups[0].ToString() + "\r\n");
                    string resultNew = "\"msg\":{\"notify_trade\":" + match1[k].Groups[0].ToString() + "}";
                    InsertMsgLogInfo(nick, "TradeRated", resultNew);
                }

                total += match1.Count;
                if (match1.Count < 200)
                {
                    break;
                }
            }
        }
        Response.Write("共导入【" + total.ToString() + "】条评价数据，系统正在处理中，请不要重复点击！");
    }

    public void InsertMsgLogInfo(string nick, string typ, string result)
    {
        string sql = "INSERT INTO TCS_TaobaoMsgLog (" +
                            "nick, " +
                            "typ, " +
                            "result " +
                        " ) VALUES ( " +
                            " '" + nick + "', " +
                            " '" + typ + "', " +
                            " '" + result + "' " +
                        ") ";
        Response.Write(sql + "<br>");
        utils.ExecuteNonQuery(sql);
    }


    protected void Button3_Click(object sender, EventArgs e)
    {
        string sql = "SELECT * FROM TCS_ShopSession WHERE nick = '" + this.TextBox8.Text + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            Response.Write("该客户已经进过服务");
        }
        else
        {
            Response.Write("该客户没有进过服务！！");
        }
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        if (TextBox6.Text != "xiaoman")
        {
            return;
        }

        string sql = "INSERT INTO [TCS_PayLog]([typ],[adddate],[nextdate],[enddate],[nick],[mouth],[count])VALUES('" + TextBox5.Text + "',GETDATE(),GETDATE(),GETDATE(),'" + TextBox3.Text + "',12," + TextBox4.Text + ")";
        utils.ExecuteNonQuery(sql);

        sql = "UPDATE TCS_ShopConfig SET total = total + " + TextBox4.Text + " WHERE nick = '" + TextBox3.Text + "'";
        utils.ExecuteNonQuery(sql);

        sql = "SELECT total FROM TCS_ShopConfig WHERE nick = '" + TextBox3.Text + "'";
        string count = utils.ExecuteString(sql);

        Response.Write("该客户短信剩余条数为【" + count + "】条");
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