using System;
using System.Data;
using System.Collections;
using Common;
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;

public partial class containersale : System.Web.UI.Page
{
    public string top_session = string.Empty;
    public string nick = string.Empty;
    public string versionNo = string.Empty;
    public string refreshToken = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.AddHeader("P3P", "CP=CURa ADMa DEVa PSAo PSDo OUR BUS UNI PUR INT DEM STA PRE COM NAV OTC NOI DSP COR");
        string top_appkey = "12450498";

        string top_parameters = utils.NewRequest("top_parameters", utils.RequestType.QueryString).Replace(" ", "+");
        top_session = utils.NewRequest("top_session", utils.RequestType.QueryString).Replace(" ", "+");
        string app_secret = "38c892fcaa5a971aec7a9effd105c7ba";
        string top_sign = utils.NewRequest("top_sign", utils.RequestType.QueryString).Replace(" ", "+");
        string sign = utils.NewRequest("sign", utils.RequestType.QueryString).Replace(" ", "+");

        versionNo = utils.NewRequest("versionNo", utils.RequestType.QueryString);
        string leaseId = utils.NewRequest("leaseId", utils.RequestType.QueryString).Replace(" ", "+"); ;//可以从 QueryString 来获取,也可以固定 
        string timestamp = utils.NewRequest("timestamp", utils.RequestType.QueryString).Replace(" ", "+"); ;//可以从 QueryString 来获取 

        refreshToken = Taobao.Top.Api.Util.TopUtils.DecodeTopParams(top_parameters)["refresh_token"];

        if (!Taobao.Top.Api.Util.TopUtils.VerifyTopResponse(top_parameters, top_session, top_sign, top_appkey, app_secret))
        {
            Response.Write("top签名验证不通过，请不要非法注入");
            Response.End();
            return;
        }

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
            versionNo = "1";
        }

        nick = Taobao.Top.Api.Util.TopUtils.DecodeTopParams(top_parameters)["visitor_nick"];
        if (nick == null || nick == "")
        {
            Response.Write("top签名验证不通过，请不要非法注入");
            Response.End();
            return;
        }
        //判断跳转
        GetVersion(nick);
        GetData(nick);
    }


    /// <summary>
    /// 当没有版本号传入的时候获取客户版本号
    /// </summary>
    /// <returns></returns>
    private string GetVersion(string u)
    {
        string appkey = "12450498";
        string secret = "38c892fcaa5a971aec7a9effd105c7ba";

        //判断该店铺是B店还是C店
        IDictionary<string, string> param = new Dictionary<string, string>();
        string sql = string.Empty;
        //判断短信购买及充值情况
        param.Add("nick", u);
        param.Add("article_code", "ts-22635");
        string resultnew = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.vas.subscribe.get", "", param);
        if (resultnew.IndexOf("invali") != -1)
        {
            //到期了
            return "-1";
        }
        else
        {
            Regex reg = new Regex(@"<item_code>([^<]*)</item_code><deadline>([^<]*)</deadline>", RegexOptions.IgnoreCase);
            string guid = "";
            string deadline = "";
            //更新日期
            MatchCollection match = reg.Matches(resultnew);

            for (int i = 0; i < match.Count; i++)
            {

                try
                {
                    //10元
                    if (match[i].Groups[1].ToString() == "ts-22635-1")
                    {
                        guid = "20FE4A6B-E05D-448A-A514-384B10D1DBF9";
                        deadline = match[i].Groups[2].ToString();
                    }
                    //10元
                    if (match[i].Groups[1].ToString() == "ts-22635-2")
                    {
                        guid = "42D45910-C56C-4B1A-AB7F-107945178787";
                        deadline = match[i].Groups[2].ToString();
                    }
                    //10元
                    if (match[i].Groups[1].ToString() == "ts-22635-3")
                    {
                        guid = "04B04022-D2D1-4C1F-BB7C-0DF238EC009E";
                        deadline = match[i].Groups[2].ToString();
                    }
                    //10元
                    if (match[i].Groups[1].ToString() == "ts-22635-4")
                    {
                        guid = "3D8CF413-7778-4421-A748-6294B18C8685";
                        deadline = match[i].Groups[2].ToString();
                    }

                    if (match[i].Groups[1].ToString() == "ts-22635-5")
                    {
                        guid = "6970A0F4-AF47-495A-8E88-AFB5F6AB906E";
                        deadline = match[i].Groups[2].ToString();
                    }
                    if (match[i].Groups[1].ToString() == "ts-22635-6")
                    {
                        guid = "216C8ADF-79DF-4602-9683-CC3157DE3F7A";
                        deadline = match[i].Groups[2].ToString();
                    }
                    //10元
                    if (match[i].Groups[1].ToString() == "ts-22635-7")
                    {
                        guid = "C89032C1-B56E-4CC2-9DCF-449001C26FCD";
                        deadline = match[i].Groups[2].ToString();
                    }

                    if (match[i].Groups[1].ToString() == "ts-22635-8")
                    {
                        guid = "D747589D-EE31-4F92-A93A-F4CC4A4F8CD6";
                        deadline = match[i].Groups[2].ToString();
                    }

                }
                catch { }
            }

            if (guid.Length != 0)
            {
                sql = "SELECT * FROM [BangT_Buys] WHERE nick = '" + u + "'";
                DataTable dingdt = utils.ExecuteDataTable(sql);

                //UPDATE BangT_UsedInfo SET UsedTimes=0 WHERE nick=@nick

                if (dingdt.Rows.Count >0)
                {
                    if (new Guid(dingdt.Rows[0]["FeeId"].ToString()) != new Guid(guid))
                    {
                        sql = "UPDATE BangT_UserAds SET feeid='" + guid + "' WHERE nick='" + u + "' AND   UserAdsState<>0";
                        utils.ExecuteNonQuery(sql);
                        sql = "UPDATE BangT_Buys SET isexpied=0,buytime=GETDATE(),ExpiedTime = '" + deadline + "',FeeId='" + guid + "' WHERE nick='" + u + "'";
                        utils.ExecuteNonQuery(sql);

                        CacheCollection.RemoveCacheByKey(CacheCollection.KEY_ALLBUYNINFO);
                    }

                    //update xufei
                    sql = "SELECT * FROM BangT_Buys WHERE nick = '" + u + "' AND isexpied = 1";
                    //string count1 = utils.ExecuteString(sql);
                    DataTable dt = utils.ExecuteDataTable(sql);
                    if (dt.Rows.Count != 0)
                    {
                        if (new Guid(dt.Rows[0]["FeeId"].ToString()) != new Guid(guid))
                        {
                            sql = "UPDATE BangT_UserAds SET feeid='" + guid + "' WHERE nick='" + u + "' AND   UserAdsState<>0";
                            utils.ExecuteNonQuery(sql);
                            sql = "UPDATE BangT_Buys SET isexpied=0,buytime=GETDATE(),ExpiedTime = '" + deadline + "',FeeId='" + guid + "' WHERE nick='" + u + "'";
                            utils.ExecuteNonQuery(sql);

                            CacheCollection.RemoveCacheByKey(CacheCollection.KEY_ALLBUYNINFO);
                        }

                        //判断淘宝获取的到期日期跟数据库里日期是否一致
                        if (dt.Rows[0]["ExpiedTime"] != DBNull.Value)
                        {
                            if (deadline != DateTime.Parse(dt.Rows[0]["ExpiedTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))
                            {
                                sql = "UPDATE BangT_UsedInfo SET UsedTimes=0 WHERE nick='" + u + "'";
                                utils.ExecuteNonQuery(sql);

                                sql = "UPDATE BangT_Buys SET isexpied=0,buytime=GETDATE(),ExpiedTime = '" + deadline + "' WHERE nick='" + u + "'";
                                utils.ExecuteNonQuery(sql);

                                CacheCollection.RemoveCacheByKey(CacheCollection.KEY_ALLBUYNINFO);
                            }
                        }
                    }
                }
                else
                {
                    //插入
                    sql = "INSERT INTO BangT_Buys ([Nick],[FeeId],[BuyTime],[IsExpied],ExpiedTime) VALUES ('" + u + "','" + guid + "',GETDATE(),0,'" + deadline + "')";
                    utils.ExecuteNonQuery(sql);

                    CacheCollection.RemoveCacheByKey(CacheCollection.KEY_ALLBUYNINFO);
                }
            }
        }

        File.WriteAllText(Server.MapPath("customer/" + u + ".txt"), Request.Url + "?" + Request.QueryString + sql + resultnew);

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
        //string session = "23200d282b335fc82ee9466c363c14f7e1b03";

        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", "12450498", "38c892fcaa5a971aec7a9effd105c7ba");

        //通过获取当前会话客户的在销商品来获取用户NICK
        //ItemsOnsaleGetRequest request1 = new ItemsOnsaleGetRequest();
        //request1.PageSize = 1;
        //request1.Fields = "nick";
        //PageList<Item> item = client.ItemsOnsaleGet(request1, session);
        //if (item.Content.Count == 0)
        //{
        //    Response.Write("请您先在店铺里添加商品 <a href='http://i.taobao.com/my_taobao.htm'>我的淘宝</a>");
        //    Response.End();
        //    return;
        //}
        //else
        //{
        //    nick = item.Content[0].Nick;
        //}

        //获取店铺基本信息
        UserGetRequest request = new UserGetRequest();
        request.Fields = "user_id,nick,seller_credit";
        request.Nick = nick;
        //User user = client.UserGet(request, session);

        //加入推荐好友判断
        //Tuijian(nick);

        if (CheckUserExits(nick))
        {
            //更新该会员的店铺信息
            string ip = Request.UserHostAddress;
            //记录2次登录日志
            string sql = "INSERT INTO TopLoginLog (" +
                           "nick " +
                       " ) VALUES ( " +
                           " '" + nick + "'" +
                     ") ";
            utils.ExecuteNonQuery(sql);

            //更新登录次数和最近登陆时间
            sql = "UPDATE toptaobaoshop SET logintimes = logintimes + 1,lastlogin = GETDATE(),session='" + top_session + "',sessionmarket='" + top_session + "',ip='" + ip + "',refreshToken='" + refreshToken + "' WHERE nick = '" + nick + "'";
            utils.ExecuteNonQuery(sql);
            ReflashSession();
        }
        else
        {
            //记录该会员的店铺信息
            InsertUserInfo(nick);
        }

        //string oldNick = nick;

        ////加密NICK
        //Rijndael_ encode = new Rijndael_("tetesoft");
        //nick = encode.Encrypt(nick);

        //Common.Cookie cookie = new Common.Cookie();
        //cookie.setCookie("top_session", top_session, 999999);
        //cookie.setCookie("nick", nick, 999999);

        //这里做获取用户商品的操作
        AddCookie(top_session);

        //CacheCollection.RemoveCacheByKey(CacheCollection.KEY_ALLBUYNINFO);
        Response.Redirect("~/index.html");
        //Response.Redirect("http://bang.7fshop.com/default.aspx?istongji=1&session=" + top_session + "&nick=" + HttpUtility.UrlEncode(oldNick));
    }

    private void ReflashSession()
    {
        string appkey = "12450498";
        string secret = "38c892fcaa5a971aec7a9effd105c7ba";

        IDictionary<string, string> param = new Dictionary<string, string>();
        string url = string.Empty;
        string result = string.Empty;
        string str = string.Empty;
        string sign = string.Empty;
        sign = CreateNewSign(appkey, secret, refreshToken, top_session);

        url = "http://container.open.taobao.com/container/refresh?appkey=12450498&refresh_token=" + refreshToken
+ "&sessionkey=" + top_session + "&sign=" + sign;
        result = GetWebSiteContent(url, "get", "", "gbk");
    }

    private static string GetWebSiteContent(string url, string requestMethod, string requestBody, string encode)
    {
        string strReturn = "";

        WebRequest wRequestUTF =
            WebRequest.Create(url +
                              (requestMethod.ToUpper() == "GET" && !string.IsNullOrEmpty(requestBody)
                                   ? "?" + requestBody
                                   : ""));
        wRequestUTF.Credentials = CredentialCache.DefaultCredentials;
        wRequestUTF.Timeout = 10000; //10秒改为5秒超时
        wRequestUTF.Method = requestMethod.ToUpper();
        //wRequestUTF.ContentType = "application/X-www-form-urlencoded;charset=utf-8";

        wRequestUTF.ContentType = "application/X-www-form-urlencoded;charset=" + encode;
        wRequestUTF.Headers.Set("Pragma", "no-cache");
        //wRequestUTF.Headers.Set("Referer", " http://www.aidai.com/Frames.html");
        if (wRequestUTF.Method == "POST")
        {
            if (requestBody != null)
            {
                byte[] bs = Encoding.UTF8.GetBytes(requestBody);

                wRequestUTF.ContentLength = bs.Length;

                using (Stream reqStream = wRequestUTF.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                }
            }
        }

        try
        {
            WebResponse wResponseUTF = wRequestUTF.GetResponse();
            Stream streamUTF = wResponseUTF.GetResponseStream();
            //StreamReader sReaderUTF = new StreamReader(streamUTF, Encoding.UTF8);
            StreamReader sReaderUTF = new StreamReader(streamUTF, Encoding.GetEncoding(encode));
            strReturn = sReaderUTF.ReadToEnd();
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(url + ex.Message);
        }

        return strReturn;
    }

    protected static string CreateNewSign(string appkey, string app_secret, string token, string session)
    {
        StringBuilder result = new StringBuilder();
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        result.Append("appkey").Append(appkey).Append("refresh_token").Append(token).Append("sessionkey").Append(session).Append(app_secret);
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

        return result.ToString();
    }


    private void AddCookie(string session)
    {
        if (Request.Cookies["nick"] == null)
        {
            if (new GoodsService().SelectGoodsCountByNick(nick) == 0)
            {
                IList<TaoBaoGoodsClassInfo> classList = TopAPI.GetGoodsClassInfoList(nick, session);

                if (classList != null)
                {
                    TaoBaoGoodsClassService tbgcDal = new TaoBaoGoodsClassService();

                    foreach (TaoBaoGoodsClassInfo cinfo in classList)
                    {
                        tbgcDal.InsertGoodsClass(cinfo, nick);
                    }
                }

                GoodsService goodsDal = new GoodsService();
                IList<TaoBaoGoodsInfo> list = TopAPI.GetGoodsInfoListByNick(nick, session);

                foreach (TaoBaoGoodsInfo info in list)
                {
                    goodsDal.InsertGoodsInfo(info, nick);
                }
            }
        }

        HttpCookie cookie = new HttpCookie("nick", HttpUtility.UrlEncode(nick));
        HttpCookie cooksession = new HttpCookie("nicksession", session);
        cookie.Expires = DateTime.Now.AddDays(1);
        cooksession.Expires = DateTime.Now.AddDays(1);

        Response.Cookies.Add(cookie);
        //LogInfo.Add("添加了cookie", nick);

        Session["snick"] = nick;
        Session["ssession"] = session;

        Response.Cookies.Add(cooksession);
    }

    private void Tuijian(string nick)
    {
        Common.Cookie cookie = new Common.Cookie();
        string tuijianid = cookie.getCookie("tuijianid");

        string sql = "SELECT * FROM TopTaobaoShop WHERE sid = '" + tuijianid + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            //判断是否为老用户
            sql = "SELECT COUNT(*) FROM TopTaobaoShop WHERE nick = '" + nick + "'";
            string count = utils.ExecuteString(sql);
            if (count != "0")
            {
                return;
            }

            //判断是否推荐过
            sql = "SELECT COUNT(*) FROM TopTuijian WHERE nickto = '" + nick + "'";
            count = utils.ExecuteString(sql);
            if (count != "0")
            {
                return;
            }

            sql = "INSERT INTO TopTuijian (" +
                        "nickfrom, " +
                        "isok, " +
                        "okdate, " +
                        "nickto " +
                    " ) VALUES ( " +
                        " '" + dt.Rows[0]["nick"].ToString() + "', " +
                        " '1', " +
                        " GETDATE(), " +
                        " '" + nick + "' " +
                  ") ";
            utils.ExecuteNonQuery(sql);
        }
    }


    /// <summary>
    /// 记录该会员的店铺信息
    /// </summary>
    private void InsertUserInfo(string nick)
    {
        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", "12450498", "38c892fcaa5a971aec7a9effd105c7ba");
        //记录店铺基本信息
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
        string ip = Request.UserHostAddress;
        //记录到本地数据库
        string sql = "INSERT INTO TopTaobaoShop (" +
                        "sid, " +
                        "cid, " +
                        "title, " +
                        "nick, " +
                        "[desc], " +
                        "bulletin, " +
                        "pic_path, " +
                        "created, " +
                        "modified, " +
                        "shop_score, " +
                        "ip, " +
                        "session, " +
                        "remain_count,refreshToken" +
                    " ) VALUES ( " +
                        " '" + shop.Sid + "', " +
                        " '" + shop.Cid + "', " +
                        " '" + shop.Title + "', " +
                        " '" + shop.Nick + "', " +
                        " '" + shop.Desc + "', " +
                        " '" + shop.Bulletin + "', " +
                        " '" + shop.PicPath + "', " +
                        " '" + shop.Created + "', " +
                        " '" + shop.Modified + "', " +
                        " '" + shop.ShopScore + "', " +
                        " '" + ip + "', " +
                        " '" + top_session + "', " +
                        " '" + shop.RemainCount + "','" +refreshToken+"'"+
                  ") ";

        utils.ExecuteNonQuery(sql);
    }

    /// <summary>
    /// 判断该TAOBAO会员的店铺是否有记录
    /// </summary>
    /// <param name="nick"></param>
    /// <returns></returns>
    private bool CheckUserExits(string nick)
    {
        string sql = "SELECT * FROM TopTaobaoShop WHERE nick = '" + nick + "'";
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
