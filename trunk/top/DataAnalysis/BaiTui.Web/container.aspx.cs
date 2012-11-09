using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Common;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;
using System.Net;
using System.IO;

public partial class container : System.Web.UI.Page
{
    public string top_session = string.Empty;
    public string nick = string.Empty;
    public string versionNo = string.Empty;
    public string refreshToken = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            Response.AddHeader("P3P", "CP=CURa ADMa DEVa PSAo PSDo OUR BUS UNI PUR INT DEM STA PRE COM NAV OTC NOI DSP COR");
            //File.WriteAllText(Server.MapPath("aaaaa.txt"), Request.Url.ToString());
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
            string top_appkey = "12579340";
            string top_parameters = utils.NewRequest("top_parameters", utils.RequestType.QueryString).Replace(" ", "+");
            top_session = utils.NewRequest("top_session", utils.RequestType.QueryString).Replace(" ", "+");
            string app_secret = "5b384ce1102e72ee0643c5b303e2a96a";
            string top_sign = utils.NewRequest("top_sign", utils.RequestType.QueryString).Replace(" ", "+");
            string sign = utils.NewRequest("sign", utils.RequestType.QueryString).Replace(" ", "+");
            refreshToken = Taobao.Top.Api.Util.TopUtils.DecodeTopParams(top_parameters)["refresh_token"];
            versionNo = utils.NewRequest("versionNo", utils.RequestType.QueryString);
            string leaseId = utils.NewRequest("leaseId", utils.RequestType.QueryString).Replace(" ", "+"); ;//可以从 QueryString 来获取,也可以固定 
            string timestamp = utils.NewRequest("timestamp", utils.RequestType.QueryString).Replace(" ", "+"); ;//可以从 QueryString 来获取 



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

            //判断用户是否存在
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
            ReflashSession();
            //添加cookie
            AddCookie(top_session);
            Response.Redirect("/Index.aspx");
        }
    }

    private void ReflashSession()
    {
        string appkey = "12579340";
        string secret = "5b384ce1102e72ee0643c5b303e2a96a";

        IDictionary<string, string> param = new Dictionary<string, string>();
        string url = string.Empty;
        string result = string.Empty;
        string str = string.Empty;
        string sign = string.Empty;
        sign = CreateNewSign(appkey, secret, refreshToken, top_session);

        url = "http://container.open.taobao.com/container/refresh?appkey=12579340&refresh_token=" + refreshToken
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
    /// 记录该会员的店铺信息
    /// </summary>
    private void InsertUserInfo(string nick)
    {
        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", "12579340", "5b384ce1102e72ee0643c5b303e2a96a");
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
                        " '" + shop.RemainCount + "','" + refreshToken + "'" +
                  ") ";

        utils.ExecuteNonQuery(sql);
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
}
