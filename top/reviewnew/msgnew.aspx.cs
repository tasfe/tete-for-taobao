﻿using System;
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

public partial class top_review_msg : System.Web.UI.Page
{
    public string session = string.Empty;
    public string nick = string.Empty;
    public string used = string.Empty;
    public string total = string.Empty;

    public string giftflag = string.Empty;
    public string giftcontent = string.Empty;
    public string shippingflag = string.Empty;
    public string shippingcontent = string.Empty;
    public string reviewflag = string.Empty;
    public string reviewcontent = string.Empty;
    public string fahuoflag = string.Empty;
    public string fahuocontent = string.Empty;
    public string reviewtime = string.Empty;
    public string oldshopname = string.Empty;
    public string shopname = string.Empty;

    public string delayflag = string.Empty;
    public string delaycontent = string.Empty;
    public string unpayflag = string.Empty;
    public string unpaycontent = string.Empty;
    public string cityflag = string.Empty;
    public string citycontent = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string id = utils.NewRequest("id", utils.RequestType.QueryString);
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
                string msg = "尊敬的" + nick + "，非常抱歉的告诉您，只有专业版或者以上版本才能使用【短信自动提醒】功能，如需继续使用请<a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22904-2:1;' target='_blank'>购买高级会员服务</a>，谢谢！<br><br> PS：发送的短信需要单独购买，1毛钱1条~";
                Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
                Response.End();
                return;
            }

            oldshopname = dt.Rows[0]["nick"].ToString();
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
                Response.Redirect("msg.aspx");
            }
        }

        if (!IsPostBack)
        {
            BindData();
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
    public static string PostXml(string url, string appkey, string appSecret, string method, string session,
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

    /// <summary>
    /// 数据绑定
    /// </summary>
    private void BindData()
    {

        //获取相关短信设置
        string sql = "SELECT * FROM TCS_ShopConfig WHERE nick = '" + nick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            used = dt.Rows[0]["used"].ToString();
            total = dt.Rows[0]["total"].ToString();
            giftflag = dt.Rows[0]["giftflag"].ToString();
            giftcontent = dt.Rows[0]["giftcontent"].ToString();
            shippingflag = dt.Rows[0]["shippingflag"].ToString();
            shippingcontent = dt.Rows[0]["shippingcontent"].ToString();
            reviewflag = dt.Rows[0]["reviewflag"].ToString();
            reviewcontent = dt.Rows[0]["reviewcontent"].ToString();
            fahuoflag = dt.Rows[0]["fahuoflag"].ToString();
            fahuocontent = dt.Rows[0]["fahuocontent"].ToString();
            shopname = dt.Rows[0]["shopname"].ToString();
            reviewtime = dt.Rows[0]["reviewtime"].ToString();

            delayflag = dt.Rows[0]["delayflag"].ToString();
            delaycontent = dt.Rows[0]["delaycontent"].ToString();
            unpayflag = dt.Rows[0]["unpayflag"].ToString();
            unpaycontent = dt.Rows[0]["unpaycontent"].ToString();
            cityflag = dt.Rows[0]["cityflag"].ToString();
            citycontent = dt.Rows[0]["citycontent"].ToString();

            //增加默认值
            if (giftcontent.Length == 0)
            {
                giftcontent = "[shopname]:亲,恭喜您获得我店[gift],下次购物可抵现金,有效期请您在淘宝中-我的优惠卡券 中查看";
            }
            if (shippingcontent.Length == 0)
            {
                shippingcontent = "[shopname]:亲,您的宝贝已到达,满分好评+优质评价即可获赠[gift]";
            }
            if (reviewcontent.Length == 0)
            {
                reviewcontent = "[shopname]:亲,您购买的宝贝已经签收多日,请帮忙确认,满分好评+优质评价,即可获赠[gift]";
            }
            if (fahuocontent.Length == 0)
            {
                fahuocontent = "[shopname]:亲,您购买的宝贝已发出,[shiptyp]+[shipnumber],请您签收后满分好评即可获赠[gift]";
            }
            if (shopname.Length == 0)
            {
                shopname = oldshopname;
            }
        }
    }


    protected void Button2_Click(object sender, EventArgs e)
    {
        string appkey = "12159997";
        string secret = "614e40bfdb96e9063031d1a9e56fbed5";

        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        session = cookie.getCookie("top_sessiongroupbuy");
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Decrypt(taobaoNick);

        //判断该店铺是B店还是C店
        IDictionary<string, string> param = new Dictionary<string, string>();
        string sql = string.Empty;

        //判断短信购买及充值情况
        param = new Dictionary<string, string>();
        param.Add("nick", nick.Trim());
        param.Add("article_code", "service-0-22904");
        string resultnew = PostXml("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.vas.subscribe.get", "", param);
        //Response.Write(resultnew);
        //Response.Write(nick);
        if (resultnew.IndexOf("invali") != -1)
        {
            //到期了
        }
        else
        {
            Regex reg = new Regex(@"<item_code>([^<]*)</item_code><deadline>([^<]*)</deadline>", RegexOptions.IgnoreCase);
            //更新日期
            MatchCollection match = reg.Matches(resultnew);
            for (int i = 0; i < match.Count; i++)
            {
                //try
                //{
                    //10元
                    if (match[i].Groups[1].ToString() == "service-0-22904-4")
                    {
                        //判断当月是否加过短信，如果没加过
                        sql = "SELECT COUNT(*) FROM TCS_PayLog WHERE nick = '" + nick + "' AND typ = 'service-0-22904-4' AND enddate = '" + match[i].Groups[2].ToString() + "'";
                        string count = utils.ExecuteString(sql);
                        if (count == "0")
                        {
                            //插入充值记录并更新短信条数
                            sql = "INSERT INTO TCS_PayLog (" +
                                            "typ, " +
                                            "enddate, " +
                                            "nick, " +
                                            "count " +
                                        " ) VALUES ( " +
                                            " '" + match[i].Groups[1].ToString() + "', " +
                                            " '" + match[i].Groups[2].ToString() + "', " +
                                            " '" + nick + "', " +
                                            " '100' " +
                                      ") ";
                            utils.ExecuteNonQuery(sql);

                            //更新短信条数
                            sql = "UPDATE TCS_ShopConfig SET total = total + 100 WHERE nick = '" + nick + "'";
                            utils.ExecuteNonQuery(sql);
                        }
                    }

                    //50元
                    if (match[i].Groups[1].ToString() == "service-0-22904-5")
                    {
                        //判断当月是否加过短信，如果没加过
                        sql = "SELECT COUNT(*) FROM TCS_PayLog WHERE nick = '" + nick + "' AND typ = 'service-0-22904-5' AND enddate = '" + match[i].Groups[2].ToString() + "'";
                        string count = utils.ExecuteString(sql);
                        if (count == "0")
                        {
                            //插入充值记录并更新短信条数
                            sql = "INSERT INTO TCS_PayLog (" +
                                            "typ, " +
                                            "enddate, " +
                                            "nick, " +
                                            "count " +
                                        " ) VALUES ( " +
                                            " '" + match[i].Groups[1].ToString() + "', " +
                                            " '" + match[i].Groups[2].ToString() + "', " +
                                            " '" + nick + "', " +
                                            " '510' " +
                                      ") ";
                            utils.ExecuteNonQuery(sql);

                            //更新短信条数
                            sql = "UPDATE TCS_ShopConfig SET total = total + 510 WHERE nick = '" + nick + "'";
                            utils.ExecuteNonQuery(sql);
                        }
                    }

                    //100元
                    if (match[i].Groups[1].ToString() == "service-0-22904-6")
                    {
                        //判断当月是否加过短信，如果没加过
                        sql = "SELECT COUNT(*) FROM TCS_PayLog WHERE nick = '" + nick + "' AND typ = 'service-0-22904-6' AND enddate = '" + match[i].Groups[2].ToString() + "'";
                        string count = utils.ExecuteString(sql);
                        if (count == "0")
                        {
                            //插入充值记录并更新短信条数
                            sql = "INSERT INTO TCS_PayLog (" +
                                            "typ, " +
                                            "enddate, " +
                                            "nick, " +
                                            "count " +
                                        " ) VALUES ( " +
                                            " '" + match[i].Groups[1].ToString() + "', " +
                                            " '" + match[i].Groups[2].ToString() + "', " +
                                            " '" + nick + "', " +
                                            " '1030' " +
                                      ") ";
                            utils.ExecuteNonQuery(sql);

                            //更新短信条数
                            sql = "UPDATE TCS_ShopConfig SET total = total + 1030 WHERE nick = '" + nick + "'";
                            utils.ExecuteNonQuery(sql);
                        }
                    }

                    //100元
                    if (match[i].Groups[1].ToString() == "service-0-22904-7")
                    {
                        //判断当月是否加过短信，如果没加过
                        sql = "SELECT COUNT(*) FROM TCS_PayLog WHERE nick = '" + nick + "' AND typ = 'service-0-22904-7' AND enddate = '" + match[i].Groups[2].ToString() + "'";
                        string count = utils.ExecuteString(sql);
                        if (count == "0")
                        {
                            //插入充值记录并更新短信条数
                            sql = "INSERT INTO TCS_PayLog (" +
                                            "typ, " +
                                            "enddate, " +
                                            "nick, " +
                                            "count " +
                                        " ) VALUES ( " +
                                            " '" + match[i].Groups[1].ToString() + "', " +
                                            " '" + match[i].Groups[2].ToString() + "', " +
                                            " '" + nick + "', " +
                                            " '5200' " +
                                      ") ";
                            utils.ExecuteNonQuery(sql);

                            //更新短信条数
                            sql = "UPDATE TCS_ShopConfig SET total = total + 5200 WHERE nick = '" + nick + "'";
                            utils.ExecuteNonQuery(sql);
                        }
                    }


                    //1000元
                    if (match[i].Groups[1].ToString() == "service-0-22904-8")
                    {
                        //判断当月是否加过短信，如果没加过
                        sql = "SELECT COUNT(*) FROM TCS_PayLog WHERE nick = '" + nick + "' AND typ = 'service-0-22904-8' AND enddate = '" + match[i].Groups[2].ToString() + "'";
                        string count = utils.ExecuteString(sql);
                        if (count == "0")
                        {
                            //插入充值记录并更新短信条数
                            sql = "INSERT INTO TCS_PayLog (" +
                                            "typ, " +
                                            "enddate, " +
                                            "nick, " +
                                            "count " +
                                        " ) VALUES ( " +
                                            " '" + match[i].Groups[1].ToString() + "', " +
                                            " '" + match[i].Groups[2].ToString() + "', " +
                                            " '" + nick + "', " +
                                            " '10500' " +
                                      ") ";
                            utils.ExecuteNonQuery(sql);

                            //更新短信条数
                            sql = "UPDATE TCS_ShopConfig SET total = total + 10500 WHERE nick = '" + nick + "'";
                            utils.ExecuteNonQuery(sql);
                        }
                    }


                    //活动连接
                    if (match[i].Groups[1].ToString() == "service-0-22904-9")
                    {
                        //更新短信条数
                        sql = "UPDATE TCS_ShopSession SET version='3' WHERE nick = '" + nick + "'";

                        utils.ExecuteNonQuery(sql);
                    }
                //}
                //catch { }
            }
        }

        Response.Redirect("msg.aspx");
    }


    private string ReplaceStr(string str)
    {
        str = str.Replace("劵", "券");

        return str;
    }
    
    /// <summary>
    /// 保存设置
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {
        string giftflag = utils.NewRequest("giftflag", utils.RequestType.Form) == "1" ? "1" : "0";
        string shippingflag = utils.NewRequest("shippingflag", utils.RequestType.Form) == "1" ? "1" : "0";
        string reviewflag = utils.NewRequest("reviewflag", utils.RequestType.Form) == "1" ? "1" : "0";
        string fahuoflag = utils.NewRequest("fahuoflag", utils.RequestType.Form) == "1" ? "1" : "0";
        string delayflag = utils.NewRequest("delayflag", utils.RequestType.Form) == "1" ? "1" : "0";
        string unpayflag = utils.NewRequest("unpayflag", utils.RequestType.Form) == "1" ? "1" : "0";
        string cityflag = utils.NewRequest("cityflag", utils.RequestType.Form) == "1" ? "1" : "0";
        string reviewtime = utils.NewRequest("reviewtime", utils.RequestType.Form);

        string sql = "UPDATE TCS_ShopConfig SET " +
            "giftflag = '" + giftflag + "', " +
            "giftcontent = '" + ReplaceStr(utils.NewRequest("giftcontent", utils.RequestType.Form)) + "', " +
            "shippingflag = '" + shippingflag + "', " +
            "shippingcontent = '" + ReplaceStr(utils.NewRequest("shippingcontent", utils.RequestType.Form)) + "', " +
            "reviewflag = '" + reviewflag + "', " +
            "shopname = '" + utils.NewRequest("shopname", utils.RequestType.Form) + "', " +
            "fahuoflag = '" + fahuoflag + "', " +
            "fahuocontent = '" + ReplaceStr(utils.NewRequest("fahuocontent", utils.RequestType.Form)) + "', " +
            "delayflag = '" + delayflag + "', " +
            "delaycontent = '" + ReplaceStr(utils.NewRequest("delaycontent", utils.RequestType.Form)) + "', " +
            "unpayflag = '" + unpayflag + "', " +
            "unpaycontent = '" + ReplaceStr(utils.NewRequest("unpaycontent", utils.RequestType.Form)) + "', " +
            "cityflag = '" + cityflag + "', " +
            "citycontent = '" + ReplaceStr(utils.NewRequest("citycontent", utils.RequestType.Form)) + "', " +
            "reviewtime = '" + utils.NewRequest("reviewtime", utils.RequestType.Form) + "', " +
            "reviewcontent = '" + ReplaceStr(utils.NewRequest("reviewcontent", utils.RequestType.Form)) + "' " +
        "WHERE nick = '" + nick + "'";

        //Response.Write(sql);
        utils.ExecuteNonQuery(sql);


        sql = "INSERT INTO TCS_ShopActLog (nick, typ, message) VALUES ('" + nick + "', 'msg', '" + sql.Replace("'", "''") + "')";
        utils.ExecuteNonQuery(sql);

        Response.Write("<script>alert('保存成功！');window.location.href='msg.aspx';</script>");
        Response.End();
        return;
    }
}