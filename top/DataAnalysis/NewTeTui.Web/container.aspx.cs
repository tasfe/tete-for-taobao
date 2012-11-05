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

public partial class container : System.Web.UI.Page
{
    public string top_session = string.Empty;
    public string nick = string.Empty;
    public string versionNo = string.Empty;
    public string refreshToken = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Request.QueryString["code"]))
        {
            Response.Write("请从淘宝授权登录");
            Response.End();
            return;
        }

        IDictionary<string, string> param = new Dictionary<string, string>();
        param.Add("client_id", "21093339");
        param.Add("client_secret", "c1c22ba85fb91bd20279213ef7b9ee80");
        param.Add("grant_type", "authorization_code");
        param.Add("code", Request.QueryString["code"]);
        param.Add("redirect_uri", "http://www.fensehenghuo.com/containerNew.aspx");
        string result = "";
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://oauth.taobao.com/token");
        req.Method = "POST";
        req.KeepAlive = true;
        req.Timeout = 300000;
        req.ContentType = "application/x-www-form-urlencoded;charset=gb2312";
        byte[] postData = Encoding.UTF8.GetBytes(PostData(param));
        Stream reqStream = req.GetRequestStream();
        reqStream.Write(postData, 0, postData.Length);
        reqStream.Close();
        try
        {
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
        }
        catch (Exception ex)
        {
            LogInfo.Add("淘宝进入异常", ex.Message);
        }

        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
        ValiInfo info = js.Deserialize<ValiInfo>(result);

        top_session = info.access_token;
        refreshToken = info.refresh_token;
        nick = info.taobao_user_nick;
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
        LogInfo.Add("c", u);
        string appkey = "21093339";
        string secret = "c1c22ba85fb91bd20279213ef7b9ee80";

        //判断该店铺是B店还是C店
        IDictionary<string, string> param = new Dictionary<string, string>();
        string sql = string.Empty;
        //判断短信购买及充值情况
        param.Add("nick", u);
        param.Add("article_code", "ts-1800709");
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
                    if (match[i].Groups[1].ToString() == "ts-1800709-1")
                    {
                        guid = "28F46E17-3117-44E7-847F-79D0BB0BEF69";
                        deadline = match[i].Groups[2].ToString();
                    }
                    //10元
                    if (match[i].Groups[1].ToString() == "ts-1800709-2")
                    {
                        guid = "C7FCB728-C736-4AF5-935B-14AB24AE37AA";
                        deadline = match[i].Groups[2].ToString();
                    }
                    //10元
                    if (match[i].Groups[1].ToString() == "ts-1800709-3")
                    {
                        guid = "DB6964D2-A4CB-47C8-B97C-1EB4EC056B56";
                        deadline = match[i].Groups[2].ToString();
                    }
                    //10元
                    if (match[i].Groups[1].ToString() == "ts-1800709-4")
                    {
                        guid = "F021E717-4ED7-49D7-9289-2B62D2F6119D";
                        deadline = match[i].Groups[2].ToString();
                    }

                    if (match[i].Groups[1].ToString() == "ts-1800709-5")
                    {
                        guid = "8A76231A-3874-4CE0-912F-936BCBC1907A";
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

    private void GetData(string nick)
    {
        string session = top_session;
        //string session = "23200d282b335fc82ee9466c363c14f7e1b03";

        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", "21093339", "c1c22ba85fb91bd20279213ef7b9ee80");

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

        ReflashSession();
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
        Response.Redirect("Default.aspx");
        //Response.Redirect("http://bang.7fshop.com/default.aspx?istongji=1&session=" + top_session + "&nick=" + HttpUtility.UrlEncode(oldNick));
    }

    private void ReflashSession()
    {
        string appkey = "21093339";
        string secret = "c1c22ba85fb91bd20279213ef7b9ee80";

        IDictionary<string, string> param = new Dictionary<string, string>();
        string url = string.Empty;
        string result = string.Empty;
        string str = string.Empty;
        string sign = string.Empty;
        sign = CreateNewSign(appkey, secret, refreshToken, top_session);

        url = "http://container.open.taobao.com/container/refresh?appkey=21093339&refresh_token=" + refreshToken
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
        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", "21093339", "c1c22ba85fb91bd20279213ef7b9ee80");
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
