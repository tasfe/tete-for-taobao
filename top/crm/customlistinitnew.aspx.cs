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

public partial class top_crm_customlist : System.Web.UI.Page
{
    public string session = string.Empty;
    public string nick = string.Empty;
    public string typ = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string id = utils.NewRequest("id", utils.RequestType.QueryString);
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        session = cookie.getCookie("top_sessiongroupbuy");
        string iscrm = cookie.getCookie("iscrm");
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

        //如果是第一次进入则根据好评数据库获取会员信息
        //if (nick == "珍爱一生311913")
        //{
           InitHaopingOldData();
        //}

        //过期判断
        if (!IsBuy(nick))
        {
            string msg = "尊敬的" + nick + "，非常抱歉的告诉您，只有VIP版本才能使用【客户关系营销】功能，如需继续使用请<a href='http://fuwu.taobao.com/ser/detail.htm?service_code=service-0-22904&frm=haoping' target='_blank'>购买高级会员服务</a>，谢谢！";
            Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
            Response.End();
            return;
        }

        BindData();
    }


    protected void Button1_Click(object sender, EventArgs e)
    {
        StringBuilder builder = new StringBuilder();
        string sql = "SELECT * FROM TCS_Customer WHERE nick = '" + nick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        builder.Append("买家昵称,省,市,区,手机,性别,等级,交易量,交易额,最后交易,评论次数,优惠券赠送次数,支付红包赠送次数,优惠券使用次数");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            builder.Append("\r\n");
            builder.Append(dt.Rows[i]["buynick"].ToString());
            builder.Append(",");
            builder.Append(dt.Rows[i]["sheng"].ToString());
            builder.Append(",");
            builder.Append(dt.Rows[i]["shi"].ToString());
            builder.Append(",");
            builder.Append(dt.Rows[i]["qu"].ToString());
            builder.Append(",");
            builder.Append(dt.Rows[i]["mobile"].ToString());
            builder.Append(",");
            builder.Append(getsex(dt.Rows[i]["sex"].ToString()));
            builder.Append(",");
            builder.Append(getgrade(dt.Rows[i]["grade"].ToString()));
            builder.Append(",");
            builder.Append(dt.Rows[i]["tradecount"].ToString());
            builder.Append(",");
            builder.Append(dt.Rows[i]["tradeamount"].ToString());
            builder.Append(",");
            builder.Append(dt.Rows[i]["lastorderdate"].ToString());
            builder.Append(",");
            builder.Append(dt.Rows[i]["reviewcount"].ToString());
            builder.Append(",");
            builder.Append(dt.Rows[i]["giftcount"].ToString());
            builder.Append(",");
            builder.Append(dt.Rows[i]["alipaycount"].ToString());
            builder.Append(",");
            builder.Append(dt.Rows[i]["couponcount"].ToString());
        }
        //生成excel文件
        string fileName = "tmp/" + nick + DateTime.Now.Ticks.ToString() + ".csv";
        File.WriteAllText(Server.MapPath(fileName), builder.ToString(), Encoding.Default);
        
        Response.Redirect(fileName);
    }

    /// <summary>
    /// 判断该用户是否订购了该服务
    /// </summary>
    /// <param name="nick"></param>
    /// <returns></returns>
    private bool IsBuy(string nick)
    {
        string sql = "SELECT plus FROM TCS_ShopSession WHERE nick = '" + nick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            string plus = dt.Rows[0][0].ToString();
            if (plus.IndexOf("crm") != -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 获取好评会员数据库信息
    /// </summary>
    private void InitHaopingOldData()
    {
        string appkey = "12159997";
        string secret = "614e40bfdb96e9063031d1a9e56fbed5";

        string sql = "SELECT * FROM TCS_Trade WHERE nick = '" + nick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (!IsHaveThisCustomer(dt.Rows[i]["nick"].ToString(), dt.Rows[i]["buynick"].ToString()))
            {
                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("fields", "sex,buyer_credit.level,created,last_visit,birthday,email");
                param.Add("nick", dt.Rows[i]["buynick"].ToString());

                string nickresult = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.user.get", session, param);

                string sex = GetValueByProperty(nickresult, "sex");
                string level = GetValueByProperty(nickresult, "level");
                string created = GetValueByProperty(nickresult, "created");
                string last_visit = GetValueByProperty(nickresult, "last_visit");
                string birthday = GetValueByProperty(nickresult, "birthday");
                string email = GetValueByProperty(nickresult, "email");

                param = new Dictionary<string, string>();
                param.Add("buyer_nick", dt.Rows[i]["buynick"].ToString());
                param.Add("current_page", "1");

                string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.crm.members.search", session, param);

                string buyer_id = GetValueByProperty(result, "buyer_id");
                string buyer_nick = GetValueByProperty(result, "buyer_nick");
                string group_ids = GetValueByProperty(result, "group_ids");
                string item_num = GetValueByProperty(result, "item_num");
                string grade = GetValueByProperty(result, "grade");
                string relation_source = GetValueByProperty(result, "relation_source");
                string last_trade_time = GetValueByProperty(result, "last_trade_time");
                string status = GetValueByProperty(result, "status");
                string trade_amount = GetValueByProperty(result, "trade_amount");
                string trade_count = GetValueByProperty(result, "trade_count");
                string province = GetValueByProperty(result, "province");
                string city = GetValueByProperty(result, "city");
                string avg_price = GetValueByProperty(result, "avg_price");

                //如果有则通过会员接口插入会员基础数据
                InsertCustomerData(dt.Rows[i]["nick"].ToString(), dt.Rows[i]["buynick"].ToString(), dt.Rows[i]["mobile"].ToString(), dt.Rows[i]["receiver_name"].ToString(), dt.Rows[i]["receiver_address"].ToString(), dt.Rows[i]["receiver_state"].ToString(), dt.Rows[i]["receiver_city"].ToString(), dt.Rows[i]["receiver_district"].ToString(), sex, level, created, last_visit, birthday, email,buyer_id, buyer_nick, group_ids, item_num, grade, relation_source, last_trade_time, status, trade_amount, trade_count, province, city, avg_price);
            }
        }
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

    /// <summary>
    /// 插入CRM会员记录
    /// </summary>
    /// <param name="trade"></param>
    /// <param name="customer"></param>
    public void InsertCustomerData(string nick, string buynick, string mobile, string truename, string address, string sheng, string shi, string qu, string sex, string level, string created, string last_visit, string birthday, string email,string buyer_id,string buyer_nick,string group_ids,string item_num,string grade,string relation_source,string last_trade_time,string status,string trade_amount,string trade_count,string province,string city,string avg_price)
    {
        string sql = "INSERT INTO TCS_Customer (" +
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
                                    "mobile, " +
                                    "truename, " +
                                    "address, " +
                                    "sheng, " +
                                    "shi, " +
                                    "sex, " +
                                    "buyerlevel, " +
                                    "created, " +
                                    "lastlogin, " +
                                    "email, " +
                                    "birthday, " +
                                    "qu " +
                                " ) VALUES ( " +
                                    " '" + nick + "', " +
                                    " '" + buynick + "', " +
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
                                    " '" + mobile + "', " +
                                    " '" + truename + "', " +
                                    " '" + address + "', " +
                                    " '" + sheng + "', " +
                                    " '" + shi + "', " +
                                    " '" + sex + "', " +
                                    " '" + level + "', " +
                                    " '" + created + "', " +
                                    " '" + last_visit + "', " +
                                    " '" + email + "', " +
                                    " '" + birthday + "'," +
                                    " '" + qu + "'" +
                                ") ";

        utils.ExecuteNonQuery(sql);
    }


    /// <summary>
    /// 看看数据库里面是否有此会员数据
    /// </summary>
    /// <param name="trade"></param>
    /// <param name="customer"></param>
    public bool IsHaveThisCustomer(string nick, string buynick)
    {
        string sql = "SELECT COUNT(*) FROM TCS_Customer WHERE nick = '" + nick + "' AND buynick = '" + buynick + "'";
        string count = utils.ExecuteString(sql);

        if (count == "0")
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void BindData()
    {
        string count = utils.NewRequest("count", utils.RequestType.QueryString);
        typ = count;
        string condition = string.Empty;
        string pageUrl = "customlist.aspx?1=1";

        switch (count)
        {
            case "0":
                pageUrl = "customlist.aspx?count=0";
                condition = " AND b.tradecount = 0";
                break;
            case "1":
                pageUrl = "customlist.aspx?count=1";
                condition = " AND b.tradecount = 1";
                break;
            case "2":
                pageUrl = "customlist.aspx?count=2";
                condition = " AND b.tradecount > 1";
                break;
            case "a":
                pageUrl = "customlist.aspx?count=a";
                condition = " AND b.grade = 0";
                break;
            case "b":
                pageUrl = "customlist.aspx?count=b";
                condition = " AND b.grade = 1";
                break;
            case "c":
                pageUrl = "customlist.aspx?count=c";
                condition = " AND b.grade = 2";
                break;
            case "d":
                pageUrl = "customlist.aspx?count=d";
                condition = " AND b.grade = 3";
                break;
            case "e":
                pageUrl = "customlist.aspx?count=e";
                condition = " AND b.grade = 4";
                break;
        }

        string page = utils.NewRequest("page", utils.RequestType.QueryString);
        int pageNow = 1;
        if (page == "")
        {
            pageNow = 1;
        }
        else
        {
            pageNow = int.Parse(page);
        }
        int pageCount = 12;
        int dataCount = (pageNow - 1) * pageCount;

        string sql = "SELECT TOP " + pageCount.ToString() + " * FROM (SELECT *,ROW_NUMBER() OVER (ORDER BY b.lastorderdate DESC) AS rownumber FROM TCS_Customer b WITH (NOLOCK) WHERE b.nick = '" + nick + "' " + condition + ") AS a WHERE a.rownumber > " + dataCount.ToString() + " ORDER BY lastorderdate DESC";
        DataTable dt = utils.ExecuteDataTable(sql);

        rptArticle.DataSource = dt;
        rptArticle.DataBind();

        //分页数据初始化
        sql = "SELECT COUNT(*) FROM TCS_Customer b WHERE b.nick = '" + nick + "' " + condition + "";
        int totalCount = int.Parse(utils.ExecuteString(sql));

        lbPage.Text = InitPageStr(totalCount, pageUrl);
    }

    public static string getgrade(string grade)
    {
        string str = string.Empty;

        switch (grade)
        {
            case "0":
                str = "<span style='#eeeeee'>【未购买】</span>";
                break;
            case "1":
                str = "普通会员";
                break;
            case "2":
                str = "<span style='color:blue'>高级会员</span>";
                break;
            case "3":
                str = "<span style='color:green'>VIP会员</span>";
                break;
            case "4":
                str = "<span style='color:red'>至尊VIP会员</span>";
                break;
        }

        return str;
    }

    public static string getsex(string sex)
    {
        string str = string.Empty;

        switch (sex)
        {
            case "m":
                str = "男";
                break;
            case "f":
                str = "女";
                break;
            case "":
                str = "--";
                break;
        }

        return str;
    }



    private string InitPageStr(int total, string url)
    {
        //分页数据初始化
        string str = string.Empty;
        int pageCount = 12;
        int pageSize = 0;
        int pageNow = 1;
        string page = utils.NewRequest("page", utils.RequestType.QueryString);
        if (page == "")
        {
            pageNow = 1;
        }
        else
        {
            pageNow = int.Parse(page);
        }

        //取总分页数
        if (total % pageCount == 0)
        {
            pageSize = total / pageCount;
        }
        else
        {
            pageSize = total / pageCount + 1;
        }

        //如果总页面大于20，则最大页面差不超过20
        int start = 1;
        int end = 20;

        if (pageSize < end)
        {
            end = pageSize;
        }
        else
        {
            if (pageNow > 15)
            {
                start = pageNow - 10;

                if (pageNow < (pageSize - 10))
                {
                    end = pageNow + 10;
                }
                else
                {
                    end = pageSize;
                }
            }
        }

        for (int i = start; i <= end; i++)
        {
            if (i.ToString() == pageNow.ToString())
            {
                str += i.ToString() + " ";
            }
            else
            {
                str += "<a href='" + url + "&page=" + i.ToString() + "'>[" + i.ToString() + "]</a> ";
            }
        }

        return str;
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