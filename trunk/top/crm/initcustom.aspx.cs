using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Common;
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;
using System.Net;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Xml;

public partial class top_crm_initcustom : System.Web.UI.Page
{
    public string session = string.Empty;
    public string nick = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string id = utils.NewRequest("id", utils.RequestType.QueryString);
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        session = cookie.getCookie("top_session");
        string iscrm = cookie.getCookie("iscrm");
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Decrypt(taobaoNick);
        return;
        //过期判断
        if (string.IsNullOrEmpty(taobaoNick))
        {
            string msg = "尊敬的淘宝卖家，非常抱歉的告诉您，您还没有购买此服务或者登录信息已失效，如需继续使用请<a href='http://fuwu.taobao.com/serv/detail.htm?service_id=764' target='_blank'>进入该服务</a>，谢谢！";
            Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
            Response.End();
            return;
        }

        //过期判断
        if (iscrm != "1")
        {

            string msg = "尊敬的" + nick + "，非常抱歉的告诉您，只有VIP版本才能使用【客户关系营销】功能，如需继续使用请<a href='http://fuwu.taobao.com/ser/detail.htm?service_code=service-0-22904&frm=haoping' target='_blank'>购买高级会员服务</a>，谢谢！";
            Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
            Response.End();
            return;
        }
    }

    protected void Button1_Click1(object sender, EventArgs e)
    {
        int index = 0;
        //执行优惠券赠送行为
        string appkey = "12132145";
        string secret = "1fdd2aadd5e2ac2909db2967cbb71e7f";

        string sql = "SELECT session FROM TCS_CrmConfig WHERE nick = '" + nick + "'";
        string session = utils.ExecuteString(sql);

        //判断如果客户订购过好评有礼的服务则调用好评的SESSION
        sql = "SELECT * FROM TCS_ShopSession WHERE nick = '" + nick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            appkey = "12159997";
            secret = "614e40bfdb96e9063031d1a9e56fbed5";
            session = dt.Rows[0]["session"].ToString();
        }

        sql = "SELECT updatedate FROM TCS_CrmConfig WHERE nick = '" + nick + "'";
        string lastdate = utils.ExecuteString(sql);


        for (int j = 1; j < 9999; j++)
        {
            IDictionary<string, string> param = new Dictionary<string, string>();
            param.Add("min_last_trade_time", lastdate);
            param.Add("max_last_trade_time", DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
            param.Add("page_size", "100");
            param.Add("current_page", j.ToString());

            //Response.Write(DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd") + " 00:00:00<br>");


            //nick = this.TextBox1.Text;

            string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.crm.members.search", session, param);
            Regex reg = new Regex(@"<crm_member>([\s\S]*?)</crm_member>", RegexOptions.IgnoreCase);
            MatchCollection match = reg.Matches(result);
            //Response.Write(result + "<hr>");
            for (int i = 0; i < match.Count; i++)
            {
                string str = match[i].Groups[0].ToString();
                string buyer_id = GetValueByProperty(str, "buyer_id");
                string buyer_nick = GetValueByProperty(str, "buyer_nick");
                string group_ids = GetValueByProperty(str, "group_ids");
                string item_num = GetValueByProperty(str, "item_num");
                string grade = GetValueByProperty(str, "grade");
                string relation_source = GetValueByProperty(str, "relation_source");
                string last_trade_time = GetValueByProperty(str, "last_trade_time");
                string status = GetValueByProperty(str, "status");
                string trade_amount = GetValueByProperty(str, "trade_amount");
                string trade_count = GetValueByProperty(str, "trade_count");
                string province = GetValueByProperty(str, "province");
                string city = GetValueByProperty(str, "city");
                string avg_price = GetValueByProperty(str, "avg_price");

                //获取该会员的详细资料taobao.user.get
                param = new Dictionary<string, string>();
                param.Add("fields", "sex,buyer_credit.level,created,last_visit,birthday,email");
                param.Add("nick", buyer_nick);

                string nickresult = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.user.get", session, param);

                string sex = GetValueByProperty(nickresult, "sex");
                string level = GetValueByProperty(nickresult, "level");
                string created = GetValueByProperty(nickresult, "created");
                string last_visit = GetValueByProperty(nickresult, "last_visit");
                string birthday = GetValueByProperty(nickresult, "birthday");
                string email = GetValueByProperty(nickresult, "email");

                //Response.Write(nickresult + "<br>");

                //获取会员在本店的订单地址记录
                param = new Dictionary<string, string>();
                param.Add("fields", "receiver_name,receiver_state,receiver_city,receiver_district,receiver_address,receiver_mobile");
                param.Add("nick", nick);
                param.Add("buyer_nick", buyer_nick);
                param.Add("page_size", "1");
                param.Add("start_created", DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd") + " 00:00:00");
                param.Add("end_created", DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");

                nickresult = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.trades.sold.get", session, param);

                string receiver_name = GetValueByProperty(nickresult, "receiver_name");
                string receiver_state = GetValueByProperty(nickresult, "receiver_state");
                string receiver_city = GetValueByProperty(nickresult, "receiver_city");
                string receiver_district = GetValueByProperty(nickresult, "receiver_district");
                string receiver_address = GetValueByProperty(nickresult, "receiver_address");
                string receiver_mobile = GetValueByProperty(nickresult, "receiver_mobile");

                //Response.Write(nickresult + "<br><br>");

                sql = "SELECT COUNT(*) FROM TCS_Customer WHERE nick = '"+nick+"' AND buynick = '"+buyer_nick+"'";
                string count = utils.ExecuteString(sql);
                if (count == "0")
                {
                    sql = "INSERT INTO TCS_Customer (" +
                                            "nick, " +
                                            "buynick, " +
                                            "status, " +
                                            "tradecount, " +
                                            "tradeamount, " +
                                            "groupid, " +
                                            "lastorderdate, " +
                                            "province, " +
                                            "city, " +
                                            "avgprice, " +
                                            "source, " +
                                            "buyerid, " +
                                            "grade, " +
                                            "sex, " +
                                            "buyerlevel, " +
                                            "created, " +
                                            "lastlogin, " +
                                            "email, " +
                                            "birthday, " +
                                            "mobile, " +
                                            "truename, " +
                                            "address, " +
                                            "sheng, " +
                                            "shi, " +
                                            "qu " +
                                        " ) VALUES ( " +
                                            " '" + nick + "', " +
                                            " '" + buyer_nick + "', " +
                                            " '" + status + "', " +
                                            " '" + trade_count + "', " +
                                            " '" + trade_amount + "', " +
                                            " '" + group_ids + "', " +
                                            " '" + last_trade_time + "', " +
                                            " '" + province + "', " +
                                            " '" + city + "', " +
                                            " '" + avg_price + "', " +
                                            " '" + relation_source + "', " +
                                            " '" + buyer_id + "', " +
                                            " '" + grade + "'," +
                                            " '" + sex + "', " +
                                            " '" + level + "', " +
                                            " '" + created + "', " +
                                            " '" + last_visit + "', " +
                                            " '" + email + "', " +
                                            " '" + birthday + "'," +
                                            " '" + receiver_mobile + "', " +
                                            " '" + receiver_name + "', " +
                                            " '" + receiver_address + "', " +
                                            " '" + receiver_state + "', " +
                                            " '" + receiver_city + "', " +
                                            " '" + receiver_district + "'" +
                                        ") ";
                    //Response.Write(sql + "<br><br>");
                    utils.ExecuteNonQuery(sql);
                    index++;
                }
            }

            if (match.Count < 100)
            {
                break;
            }
        }

        //更新最后同步会员时间
        string sqlnew = "UPDATE TCS_CrmConfig SET updatedate = GETDATE() WHERE nick = '" + nick + "'";
        utils.ExecuteNonQuery(sqlnew);

        Response.Write("导入会员OK，共有" + index.ToString() + "条！！");
    }

    public static string GetValueByProperty(string str, string prop)
    {
        Regex reg = new Regex(@"<" + prop + @">([^<]*)</" + prop + @">", RegexOptions.IgnoreCase);
        if (reg.IsMatch(str))
        {
            return reg.Match(str).Groups[1].ToString();
        }
        else
        {
            return "";
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