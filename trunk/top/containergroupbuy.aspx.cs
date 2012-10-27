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

public partial class top_containergroupbuy : System.Web.UI.Page
{
    public static string logUrl = "D:/svngroupbuy/website/ErrLog";
    public string top_session = string.Empty;
    public string nick = string.Empty;
    public string versionNo = string.Empty;
    public string refreshToken = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.AddHeader("P3P", "CP=CURa ADMa DEVa PSAo PSDo OUR BUS UNI PUR INT DEM STA PRE COM NAV OTC NOI DSP COR");
        /*
         * http://www.7fshop.com/top/container.aspx
         * ?top_appkey=12132145
         * &top_parameters=aWZyYW1lPTEmdHM9MTI4NTg3MTY5Nzg0OSZ2aWV3X21vZGU9ZnVsbCZ2aWV3X3dpZHRoPTAmdmlzaXRvcl9pZD0xNzMyMzIwMCZ2aXNpdG9yX25pY2s90ra2+cvmx+W35w==
         * &top_session=23200857b2aa0ca62d3d0d9c78a750df07300
         * &top_sign=52sdJ6lvJPeaBef7rwcoSw==
         * &agreement=true
         * &agreementsign=12132145-21431194-D1A5A27626CB119D69F0D5438423A99A
         * &y=13
         * &x=36*/
        //签名验证
        string top_appkey = "12287381";
        string top_parameters = utils.NewRequest("top_parameters", utils.RequestType.QueryString);
        top_session = utils.NewRequest("top_session", utils.RequestType.QueryString);
        string app_secret = "d3486dac8198ef01000e7bd4504601a4";
        string top_sign = utils.NewRequest("top_sign", utils.RequestType.QueryString);
        string sign = utils.NewRequest("sign", utils.RequestType.QueryString);

        versionNo = utils.NewRequest("versionNo", utils.RequestType.QueryString);
        string leaseId = utils.NewRequest("leaseId", utils.RequestType.QueryString); ;//可以从 QueryString 来获取,也可以固定 
        string timestamp = utils.NewRequest("timestamp", utils.RequestType.QueryString); ;//可以从 QueryString 来获取 
        string agreementsign = utils.NewRequest("agreementsign", utils.RequestType.QueryString);


        if (agreementsign == "")
        {
            Response.Redirect("http://container.api.taobao.com/container?appkey=12287381&scope=promotion");
            return;
        }


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


        refreshToken = Taobao.Top.Api.Util.TopUtils.DecodeTopParams(top_parameters)["refresh_token"];

        //Response.Write("!!!通过");
        //Response.End();
        nick = Taobao.Top.Api.Util.TopUtils.DecodeTopParams(top_parameters)["visitor_nick"];

        //刷新session
        string signStr = CreateSign("12287381", "d3486dac8198ef01000e7bd4504601a4", refreshToken, top_session);

        string url = "http://container.open.taobao.com/container/refresh?appkey=12159997&refresh_token=" + refreshToken + "&sessionkey=" + top_session + "&sign=" + signStr;
        string result = Post(url);

        //判断跳转
        GetData(nick);
    }


    public static string Post(string url)
    {
        string result = string.Empty;
        #region ---- 完成 HTTP POST 请求----
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        req.Method = "GET";
        req.KeepAlive = true;
        req.Timeout = 300000;
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

    protected static string CreateSign(string appkey, string app_secret, string token, string session)
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

        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", "12287381", "d3486dac8198ef01000e7bd4504601a4");

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



        if (CheckUserExits(nick))
        {
            //更新该会员的店铺信息
            //记录2次登录日志
            string sql = "INSERT INTO TopLoginLog (" +
                           "nick " +
                       " ) VALUES ( " +
                           " '" + nick + "'" +
                     ") ";
            utils.ExecuteNonQuery(sql);

            //更新登录次数和最近登陆时间
            sql = "UPDATE toptaobaoshop SET logintimes = logintimes + 1,lastlogin = GETDATE(),session='" + top_session + "',sessiongroupbuy='" + top_session + "',versionNo='" + versionNo + "',token='" + refreshToken + "' WHERE nick = '" + nick + "'";
            utils.ExecuteNonQuery(sql);
        }
        else
        {
            //记录该会员的店铺信息
            InsertUserInfo(nick);
        }

        //更新用户订购信息
        CheckUser("0", nick);

        //加密NICK
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Encrypt(nick);

        Common.Cookie cookie = new Common.Cookie();
        cookie.setCookie("top_sessiongroupbuy", top_session, 999999);
        cookie.setCookie("nick", nick, 999999);

        Response.Redirect("indexgroup2.html");
    }


    /// <summary>
    /// 记录该会员的店铺信息
    /// </summary>
    private void InsertUserInfo(string nick)
    {
        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", "12287381", "d3486dac8198ef01000e7bd4504601a4");
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
                        "versionNo, " +
                        "session, " +
                        "sessiongroupbuy, " +
                        "token, " +
                        "remain_count " +
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
                        " '" + versionNo + "', " +
                        " '" + top_session + "', " +
                        " '" + top_session + "', " +
                        " '" + refreshToken + "', " +
                        " '" + shop.RemainCount + "' " +
                  ") ";
        WriteLog(sql, "0", nick);
        utils.ExecuteNonQuery(sql);




        //记录店铺分类信息
        SellercatsListGetRequest request1 = new SellercatsListGetRequest();
        request1.Fields = "cid,parent_cid,name,is_parent";
        request1.Nick = nick;
        PageList<SellerCat> cat = client.SellercatsListGet(request1);

        for (int i = 0; i < cat.Content.Count; i++)
        {
            sql = "INSERT INTO TopTaobaoShopCat (" +
                            "cid, " +
                            "parent_cid, " +
                            "name, " +
                            "pic_url, " +
                            "sort_order, " +
                            "created, " +
                            "nick, " +
                            "modified " +
                        " ) VALUES ( " +
                            " '" + cat.Content[i].Cid + "', " +
                            " '" + cat.Content[i].ParentCid + "', " +
                            " '" + cat.Content[i].Name + "', " +
                            " '" + cat.Content[i].PicUrl + "', " +
                            " '" + cat.Content[i].SortOrder + "', " +
                            " '" + cat.Content[i].Created + "', " +
                            " '" + nick + "', " +
                            " '" + cat.Content[i].Modified + "' " +
                      ") ";
            
            utils.ExecuteNonQuery(sql);
        }

        //记录店铺所有商品信息-暂不记录
    }

    /// <summary>
    /// 判断该TAOBAO会员的店铺是否有记录
    /// </summary>
    /// <param name="nick"></param>
    /// <returns></returns>
    private bool CheckUserExits(string nick)
    {
        string sql = "SELECT * FROM TopTaobaoShop WHERE nick = '" + nick + "'";
        WriteLog(sql, "0", nick);
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
    /// 写日志
    /// </summary>
    /// <param name="value">日志内容</param>
    /// <param name="type">类型 0(成功日志),1(错误日志) 可传空文本默认为0</param>
    /// <returns></returns>
    public static void WriteLog(string message, string type, string nick)
    {

        string tempStr = logUrl + "/GroupbyShop" + nick + DateTime.Now.ToString("yyyyMMdd");//文件夹路径
        string tempFile = tempStr + "/GroupbyShop" + nick + DateTime.Now.ToString("yyyyMMdd") + ".txt";
        if (type == "1")
        {
            tempFile = tempStr + "/GroupbyShopErr" + nick + DateTime.Now.ToString("yyyyMMdd") + ".txt";
        }
        if (!Directory.Exists(tempStr))
        {
            Directory.CreateDirectory(tempStr);
        }

        if (System.IO.File.Exists(tempFile))
        {
            ///如果日志文件已经存在，则直接写入日志文件
            StreamWriter sr = System.IO.File.AppendText(tempFile);
            sr.WriteLine("\n");
            sr.WriteLine(DateTime.Now + "\n" + message);
            sr.Close();
        }
        else
        {
            ///创建日志文件
            StreamWriter sr = System.IO.File.CreateText(tempFile);
            sr.WriteLine(DateTime.Now + "\n" + message);
            sr.Close();
        }

    }







    /// <summary>
    /// ////////////////////////////////////////////
    /// </summary>
    /// <param name="t"></param>
    /// <param name="u"></param>



    private void CheckUser(string t, string u)
    {
        string appkey = "12287381";
        string secret = "d3486dac8198ef01000e7bd4504601a4";

        IDictionary<string, string> param = new Dictionary<string, string>();
        param.Add("nick", u);
        param.Add("article_code", "ts-11807");

        string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.vas.subscribe.get", "", param);

        string enddate = string.Empty;
        string version = string.Empty;
        string sql = string.Empty;

        if (result.IndexOf("invali") != -1)
        {
            //到期了
        }
        else
        {
            Regex reg = new Regex(@"<article_user_subscribe><item_code>([^<]*)</item_code><deadline>([^<]*)</deadline></article_user_subscribe>", RegexOptions.IgnoreCase);
            //更新日期
            MatchCollection match = reg.Matches(result);
            for (int i = 0; i < match.Count; i++)
            {
                try
                {
                    if (match[i].Groups[1].ToString() == "ts-11807-1")
                    {
                        sql = "UPDATE TopTaobaoShop SET enddate = '" + match[i].Groups[2].ToString() + "' WHERE nick = '" + u + "'";
                        utils.ExecuteNonQuery(sql);
                    }
                    if (match[i].Groups[1].ToString() == "ts-11807-4")
                    {
                        sql = "UPDATE TopTaobaoShop SET pay1 = '" + match[i].Groups[2].ToString() + "' WHERE nick = '" + u + "'";
                        utils.ExecuteNonQuery(sql);
                    }
                    else if (match[i].Groups[1].ToString() == "ts-11807-5")
                    {
                        sql = "UPDATE TopTaobaoShop SET pay2 = '" + match[i].Groups[2].ToString() + "' WHERE nick = '" + u + "'";
                        utils.ExecuteNonQuery(sql);
                    }
                }
                catch { }
            }
        }
        //Response.Write(result + "<br>");
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
