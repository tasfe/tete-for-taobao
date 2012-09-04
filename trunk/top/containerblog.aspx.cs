using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Common;
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net;

public partial class top_containerblog : System.Web.UI.Page
{
    public string top_session = string.Empty;
    public string nick = string.Empty;
    public string versionNo = string.Empty;
    public string isFirst = string.Empty;
    public string sendMsg = string.Empty;
    public string refreshToken = string.Empty;
    public string ip = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        //签名验证
        string top_appkey = "12159997";
        string top_parameters = utils.NewRequest("top_parameters", utils.RequestType.QueryString).Replace(" ", "+");
        top_session = utils.NewRequest("top_session", utils.RequestType.QueryString).Replace(" ", "+");
        string app_secret = "614e40bfdb96e9063031d1a9e56fbed5";
        string top_sign = utils.NewRequest("top_sign", utils.RequestType.QueryString).Replace(" ", "+"); //字符串中的+在获取后会被替换成空格，要再替换回来
        string sign = utils.NewRequest("sign", utils.RequestType.QueryString).Replace(" ", "+");
        string laiyuan = utils.NewRequest("laiyuan", utils.RequestType.QueryString).Replace(" ", "+");

        versionNo = utils.NewRequest("versionNo", utils.RequestType.QueryString);
        string leaseId = utils.NewRequest("leaseId", utils.RequestType.QueryString).Replace(" ", "+"); //可以从 QueryString 来获取,也可以固定 
        string timestamp = utils.NewRequest("timestamp", utils.RequestType.QueryString).Replace(" ", "+"); //可以从 QueryString 来获取 
        string agreementsign = utils.NewRequest("agreementsign", utils.RequestType.QueryString).Replace(" ", "+");


        if (!Taobao.Top.Api.Util.TopUtils.VerifyTopResponse(top_parameters, top_session, top_sign, top_appkey, app_secret))
        {
            Response.Write("top签名验证不通过，请不要非法注入");
            Response.End();
            return;
        }

        nick = Taobao.Top.Api.Util.TopUtils.DecodeTopParams(top_parameters)["visitor_nick"];
        refreshToken = Taobao.Top.Api.Util.TopUtils.DecodeTopParams(top_parameters)["refresh_token"];
        ip = Request.UserHostAddress;

        File.WriteAllText(Server.MapPath("customer/" + nick + ".txt"), Request.Url.ToString());

        //验证客户版本参数是否正确
        if (versionNo != "")
        {
            if (!VersionVerify(app_secret, sign, top_appkey, leaseId, timestamp, versionNo))
            {
                //Response.Write("客户版本验证不通过，请不要自行修改参数");
                //Response.End();
                //return;
            }
        }
        else
        {
            versionNo = GetVersion(nick);
        }

        if (nick == null || nick == "")
        {
            Response.Write("top签名验证不通过，请不要非法注入");
            Response.End();
            return;
        }

        //if (agreementsign != "")
        //{
        //    //加密NICK
        //    Rijndael_ encode = new Rijndael_("tetesoft");
        //    nick = encode.Encrypt(nick);

        //    Common.Cookie cookie = new Common.Cookie();
        //    cookie.setCookie("top_sessionblog", top_session, 999999);
        //    cookie.setCookie("top_sessiongroupbuy", top_session, 999999);
        //    cookie.setCookie("nick", nick, 999999);

        //    Response.Redirect("indexnew.html");
        //    return;
        //}


        //判断跳转
        GetData(nick);
    }

    /// <summary>
    /// 当没有版本号传入的时候获取客户版本号
    /// </summary>
    /// <returns></returns>
    private string GetVersion(string u)
    {
        string appkey = "12159997";
        string secret = "614e40bfdb96e9063031d1a9e56fbed5";

        //判断该店铺是B店还是C店
        IDictionary<string, string> param = new Dictionary<string, string>();
        string sql = string.Empty;
        //判断短信购买及充值情况
        param.Add("nick", u);
        param.Add("article_code", "service-0-22904");
        string resultnew = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.vas.subscribe.get", "", param);
        if (resultnew.IndexOf("invali") != -1)
        {
            //到期了
            return "-1";
        }
        else
        {
            Regex reg = new Regex(@"<item_code>([^<]*)</item_code><deadline>([^<]*)</deadline>", RegexOptions.IgnoreCase);
            //更新日期
            MatchCollection match = reg.Matches(resultnew);
            for (int i = 0; i < match.Count; i++)
            {
                try
                {
                    //10元
                    if (match[i].Groups[1].ToString() == "service-0-22904-1")
                    {
                        return "2";
                    }
                    //10元
                    if (match[i].Groups[1].ToString() == "service-0-22904-2")
                    {
                        return "2";
                    }
                    //10元
                    if (match[i].Groups[1].ToString() == "service-0-22904-3")
                    {
                        return "3";
                    }
                    //10元
                    if (match[i].Groups[1].ToString() == "service-0-22904-9")
                    {
                        return "3";
                    }
                }
                catch { }
            }
        }

        File.WriteAllText(Server.MapPath(u + ".txt"), Request.Url + "?" + Request.QueryString);

        return "2";
    }

    private bool VersionVerify(string app_secret, string top_sign, string appkey, string leaseId, string timestamp, string versionNo)
    {
        StringBuilder result = new StringBuilder();
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        result.Append(app_secret).Append("appkey").Append(appkey).Append("leaseId").Append(leaseId).Append("timestamp").Append(timestamp).Append("versionNo").Append(versionNo).Append(app_secret);
        byte[] bytes = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(result.ToString()));

        result.Remove(0, result.Length);
        for (int i = 0; i < bytes.Length; i++)
        {
            string hex = bytes[i].ToString("X");
            if (hex.Length == 1)
            {
                result.Append("0");
            }
            result.Append(hex);
        }

        //return true;
        return (top_sign == result.ToString());//是否合法
    }

    private void GetData(string nick)
    {
        string session = top_session;

        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", "12159997", "614e40bfdb96e9063031d1a9e56fbed5");

        //获取店铺基本信息
        UserGetRequest request = new UserGetRequest();
        request.Fields = "user_id,nick,seller_credit";
        request.Nick = nick;
        string oldNick = nick;
        User user = client.UserGet(request, session);

        if (versionNo == "1")
        {
            versionNo = "2";
        }

        if (CheckUserExits(nick))
        {
            string plus = string.Empty;

            if (versionNo == "2")
            {
                plus = "freecard";
            }

            if (versionNo == "3")
            {
                plus = "crm|freecard";
            }

            //更新登录次数和最近登陆时间
            string sql = "UPDATE TCS_ShopSession SET session='" + top_session + "',version='" + versionNo + "',plus='" + plus + "',token='" + refreshToken + "',ip='" + ip + "' WHERE nick = '" + nick + "'";
            utils.ExecuteNonQuery(sql);

            //更新特殊用户
            sql = "UPDATE TCS_ShopSession SET version = 2 WHERE nick = '玩具第一城'";
            utils.ExecuteNonQuery(sql);

            //更新特殊用户
            sql = "UPDATE TCS_ShopSession SET version = 3 WHERE nick = '魔女茶花'";
            utils.ExecuteNonQuery(sql);
        }
        else
        {
            //记录该会员的店铺信息
            InsertUserInfo(nick);
        }


        InsertConfigInfo(nick, session, versionNo);


        IDictionary<string, string> param = new Dictionary<string, string>();
        string result = Post("http://gw.api.taobao.com/router/rest", "12159997", "614e40bfdb96e9063031d1a9e56fbed5", "taobao.increment.customer.permit", top_session, param);

        //更新用户订购信息
        CheckUser("0", nick);

        //加密NICK
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Encrypt(nick);

        Common.Cookie cookie = new Common.Cookie();
        cookie.setCookie("top_sessionblog", top_session, 999999);
        cookie.setCookie("top_sessiongroupbuy", top_session, 999999);
        cookie.setCookie("nick", nick, 999999);

        //Response.Redirect("http://www.7fshop.com/top/market/setcookie.aspx?t=1&nick=" + HttpUtility.UrlEncode(nick));
        if (isFirst == "1")
        {
            Response.Write("<span style='font-size:18px; font-weight:bold'>好评有礼真情回馈,恭喜您获得我们送出的首次订购赠送的<font color=red>【" + sendMsg + "】</font>条短信,感谢您的使用，您的支持是我们的最大动力！</span><hr><input type=button value='开始使用服务' onclick='window.location.href=\"indexnew.html\"'>");
            Response.End();
        }
        else
        {
            Response.Redirect("indexnew.html");
        }
    }

    private void InsertConfigInfo(string nick,string session, string version)
    {
        string giftMsg = "0";
        //如果是头一次进入则赠送VIP和专业版短信
        if (version == "1" || version == "2")
        {
            giftMsg = "100";
        }

        if (version == "3")
        {
            giftMsg = "200";
        }
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
                        " '0', " +
                        " '', " +
                        " '0', " +
                        " '3', " +
                        " '6', " +
                        " '1', " +
                        " '0', " +
                        " '" + session + "', " +
                        " '0', " +
                        " '0', " +
                        " '" + giftMsg + "', " +
                        " '0' " +
                    ") ";
            utils.ExecuteNonQuery(sql);

            //插入充值记录并更新短信条数
            sql = "INSERT INTO TCS_PayLog (" +
                            "typ, " +
                            "enddate, " +
                            "nick, " +
                            "count " +
                        " ) VALUES ( " +
                            " '好评有礼真情回馈', " +
                            " GETDATE(), " +
                            " '" + nick + "', " +
                            " '" + giftMsg + "' " +
                      ") ";
            utils.ExecuteNonQuery(sql);

            isFirst = "1";
            sendMsg = giftMsg;
        }
        else
        { 
            
        }
    }


    /// <summary>
    /// 记录该会员的店铺信息
    /// </summary>
    private void InsertUserInfo(string nick)
    {
        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", "12159997", "614e40bfdb96e9063031d1a9e56fbed5");
        //记录店铺基本信息
        string ip = Request.UserHostAddress;
        ShopGetRequest request = new ShopGetRequest();
        request.Fields = "sid,cid,title,nick,desc,bulletin,pic_path,created,modified";
        request.Nick = nick;
        Shop shop;
        try
        {
            shop = client.ShopGet(request);
        }
        catch
        {
            Response.Write("没有店铺的淘宝会员不可使用该应用，如果您想继续使用，请先去淘宝网开个属于您自己的店铺！<br> <a href='http://www.taobao.com/'>返回</a>");
            Response.End();
            return;
        }

        //获取版本号
        string appkey = "12159997";
        string secret = "614e40bfdb96e9063031d1a9e56fbed5";
        string version = "9";
        IDictionary<string, string> param = new Dictionary<string, string>();
        param.Add("article_code", "service-0-22904");
        param.Add("nick", nick);

        string result = PostJson("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.vas.subscribe.get", top_session, param);
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
        }

        string plus = string.Empty;

        if (version == "1")
        {
            version = "2";
        }

        if (version == "2")
        {
            plus = "freecard";
        }

        if (version == "3")
        {
            plus = "crm|freecard";
        }

        //记录到本地数据库
        string sql = "INSERT INTO TCS_ShopSession (" +
                       "sid, " +
                       "nick, " +
                       "typ, " +
                       "version, " +
                       "plus, " +
                       "token, " +
                       "ip, " +
                       "session" +
                   " ) VALUES ( " +
                       " '" + shop.Sid + "', " +
                       " '" + shop.Nick + "', " +
                       " 'taobao', " +
                       " '" + version + "', " +
                       " '" + plus + "', " +
                       " '" + refreshToken + "', " +
                       " '" + ip + "', " +
                       " '" + top_session + "' " +
                 ") ";

        utils.ExecuteNonQuery(sql);

        //如果是好友推荐来的，记录到推荐数据库
        Common.Cookie cookie = new Common.Cookie();
        string tuijianid = cookie.getCookie("tuijianid");
        if (tuijianid != null && tuijianid != "")
        {
            Rijndael_ encode = new Rijndael_("tetesoft");
            string nickFrom = encode.Decrypt(tuijianid);

            sql = "SELECT COUNT(*) FROM TCS_Tuijian WHERE nickfrom = '" + nickFrom + "' AND nickto = '" + shop.Nick + "'";
            string count = utils.ExecuteString(sql);
            if (count != "0")
            {
                return;
            }

            sql = "INSERT INTO TCS_Tuijian (" +
                           "nickfrom, " +
                           "nickto " +
                       " ) VALUES ( " +
                           " '" + nickFrom + "', " +
                           " '" + shop.Nick + "' " +
                     ") ";

            utils.ExecuteNonQuery(sql);

            string giftMsg = "80";
            //插入充值记录并更新短信条数
            sql = "INSERT INTO TCS_PayLog (" +
                            "typ, " +
                            "enddate, " +
                            "nick, " +
                            "count " +
                        " ) VALUES ( " +
                            " '推荐好友【" + shop.Nick + "】赠送80条短信', " +
                            " GETDATE(), " +
                            " '" + nickFrom + "', " +
                            " '" + giftMsg + "' " +
                      ") ";
            utils.ExecuteNonQuery(sql);

            sql = "UPDATE TCS_ShopConfig SET total = total + " + giftMsg + " WHERE nick = '" + nickFrom + "'";
            utils.ExecuteNonQuery(sql);
        }
    }

    /// <summary>
    /// 判断该TAOBAO会员的店铺是否有记录
    /// </summary>
    /// <param name="nick"></param>
    /// <returns></returns>
    private bool CheckUserExits(string nick)
    {
        string sql = "SELECT * FROM TCS_ShopSession WHERE nick = '" + nick + "'";

        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void CheckUser(string t, string u)
    {
        string appkey = "12159997";
        string secret = "614e40bfdb96e9063031d1a9e56fbed5";

        //判断该店铺是B店还是C店
        IDictionary<string, string> param = new Dictionary<string, string>();
        param.Add("fields", "type");
        param.Add("nick", u);
        string result1 = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.user.get", top_session, param);

        string sql = string.Empty;
        //判断短信购买及充值情况
        param = new Dictionary<string, string>();
        param.Add("nick", u);
        param.Add("article_code", "service-0-22904");
        string resultnew = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.vas.subscribe.get", "", param);
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
                try
                {
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

                    //CRM购买判定
                    if (match[i].Groups[1].ToString() == "service-0-22904-11")
                    {
                        sql = "UPDATE TCS_ShopSession SET plus = 'freecard|crm' WHERE nick = '" + nick + "'";

                        utils.ExecuteNonQuery(sql);
                    }

                    //CRM购买判定
                    if (match[i].Groups[1].ToString() == "service-0-22904-12")
                    {
                        sql = "UPDATE TCS_ShopSession SET plus = plus + '|freecard' WHERE nick = '" + nick + "'";

                        utils.ExecuteNonQuery(sql);
                    }
                }
                catch { }
            }
        }
    }

    #region top api
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

    public static string PostJson(string url, string appkey, string appSecret, string method, string session,
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
    #endregion
}
