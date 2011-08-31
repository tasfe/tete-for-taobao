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

public partial class top_review_setting : System.Web.UI.Page
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

        //判断VIP版本，只有VIP才能使用此功能
        string sql = "SELECT * FROM TopTaobaoShop WHERE nick = '" + nick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            string flag = dt.Rows[0]["versionNoBlog"].ToString();
            if (flag == "0")
            {
                Response.Redirect("xufei.aspx");
                Response.End();
                return;
            }
        }

        BindData();
    }

    /// <summary>
    /// 数据绑定
    /// </summary>
    private void BindData()
    {
        //数据绑定
        DataTable dtCoupon = utils.ExecuteDataTable("SELECT * FROM TopCoupon WHERE nick = '" + nick + "' AND isdel = 0 ORDER BY id DESC");
        couponstr = "<select name='couponid'>";
        for (int i = 0; i < dtCoupon.Rows.Count; i++)
        {
            couponstr += "<option value='" + dtCoupon.Rows[i]["coupon_id"].ToString() + "'>" + dtCoupon.Rows[i]["coupon_name"].ToString() + " - " + dtCoupon.Rows[i]["denominations"].ToString() + "元</option>";
        }
        couponstr += "</select>";

        string sql = "SELECT * FROM TopAutoReview WHERE nick = '" + nick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            itemid = dt.Rows[0]["itemid"].ToString();
            mindate = dt.Rows[0]["mindate"].ToString();
            maxdate = dt.Rows[0]["maxdate"].ToString();
            isfree = dt.Rows[0]["isfree"].ToString();
            iscoupon = dt.Rows[0]["iscoupon"].ToString();
            issendmsg = dt.Rows[0]["issendmsg"].ToString();
            iskefu = dt.Rows[0]["iskefu"].ToString();
        }
        else
        { 
            //设置默认值
            mindate = "3";
            maxdate = "6";
            isfree = "0";
            iscoupon = "0";
            issendmsg = "0";
            
            //默认B店开启审核
            string typ = utils.ExecuteString("SELECT typ FROM TopTaobaoShop WHERE nick = '" + nick + "'");
            if (typ == "C")
            {
                iskefu = "0";
            }
            else
            {
                iskefu = "1";
            }

            //如果订购的不是VIP版，则默认将审核关闭
            sql = "SELECT * FROM TopTaobaoShop WHERE nick = '" + nick + "'";
            DataTable dtTest = utils.ExecuteDataTable(sql);
            if (dtTest.Rows.Count != 0)
            {
                string flag = dtTest.Rows[0]["versionNoBlog"].ToString();
                if (flag != "3")
                {
                    iskefu = "1";
                }
            }
        }
    }


    //protected void Button2_Click(object sender, EventArgs e)
    //{
    //    InitOldData();
    //}

    private void InitOldData()
    {
        string appkey = "12159997";
        string secret = "614e40bfdb96e9063031d1a9e56fbed5";
        string sql = string.Empty;

        //获取历史已发货但未确认订单
        IDictionary<string, string> param = new Dictionary<string, string>();
        param.Add("fields", "buyer_nick,tid,status,created,receiver_mobile,orders.oid,orders.num_iid");
        param.Add("status", "WAIT_BUYER_CONFIRM_GOODS");
        string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.trades.sold.get", session, param);

        Regex reg = new Regex(@"<buyer_nick>([^<]*)</buyer_nick><created>([^<]*)</created><orders list=""true"">([\s\S]*?)</orders><receiver_mobile>([^<]*)</receiver_mobile><status>([^<]*)</status><tid>([^<]*)</tid>", RegexOptions.IgnoreCase);
        MatchCollection match = reg.Matches(result);

        for (int j = 0; j < match.Count; j++)
        {
            string tid = match[j].Groups[6].ToString();
            string orderstatus = match[j].Groups[5].ToString();
            string created = match[j].Groups[2].ToString();
            string receiver_mobile = match[j].Groups[4].ToString();
            string buynick = match[j].Groups[1].ToString();

            //判断该订单是否存在
            sql = "SELECT COUNT(*) FROM TopOrder WHERE orderid = '" + tid + "'";
            string orderCount = utils.ExecuteString(sql);
            if (orderCount == "0")
            {
                //记录用户产生的订单
                sql = "INSERT INTO TopOrder (" +
                            "nick, " +
                            "orderid, " +
                            "orderstatus, " +
                            "addtime, " +
                            "buynick, " +
                            "receiver_mobile " +
                        " ) VALUES ( " +
                            " '" + nick + "', " +
                            " '" + tid + "', " +
                            " '" + orderstatus + "', " +
                            " '" + created + "', " +
                            " '" + buynick + "', " +
                            " '" + receiver_mobile + "' " +
                        ") ";
                utils.ExecuteNonQuery(sql);

                //记录订单关联的交易子订单
                Regex regChild = new Regex(@"<order><num_iid>([^<]*)</num_iid><oid>([^<]*)</oid></order>", RegexOptions.IgnoreCase);
                //textBox1.AppendText("\r\n" + match[j].Groups[3].ToString());
                MatchCollection matchChild = regChild.Matches(match[j].Groups[3].ToString());
                for (int k = 0; k < matchChild.Count; k++)
                {
                    string oid = matchChild[k].Groups[2].ToString();
                    string num_iid = matchChild[k].Groups[1].ToString();

                    sql = "INSERT INTO TopOrderList (" +
                            "nick, " +
                            "tid, " +
                            "oid, " +
                            "itemid " +
                        " ) VALUES ( " +
                            " '" + nick + "', " +
                            " '" + tid + "', " +
                            " '" + oid + "', " +
                            " '" + num_iid + "'" +
                        ") ";
                    utils.ExecuteNonQuery(sql);
                }
            }
        }

        return;
    }

    public static string check(string str, string val)
    {
        if (str == val)
        {
            return "checked";
        }
        return "";
    }

    /// <summary>
    /// 保存设置
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {
        string appkey = "12159997";
        string secret = "614e40bfdb96e9063031d1a9e56fbed5";

        //如果订购的不是VIP版，则默认将审核关闭
        string sqltest1 = "SELECT * FROM TopTaobaoShop WHERE nick = '" + nick + "'";
        DataTable dtTest = utils.ExecuteDataTable(sqltest1);
        if (dtTest.Rows.Count != 0)
        {
            string flag = dtTest.Rows[0]["versionNoBlog"].ToString();
            if (flag != "3")
            {
                if (utils.NewRequest("iskefu", utils.RequestType.Form) == "1")
                {
                    Response.Write("<script>alert('尊敬的" + nick + "，非常抱歉的告诉您，只有VIP版本才能使用【评价手动审核】功能！');window.location.href='setting.aspx';</script>");
                    Response.End();
                    return;
                }
            }
        }

        //先看看是否需要清理之前的活动
        string needUpdate = "1";
        string promotionid = string.Empty;
        string tagid = string.Empty;
        string sqltest = "SELECT * FROM TopAutoReview WHERE nick = '" + nick + "'";
        DataTable dt = utils.ExecuteDataTable(sqltest);
        if (dt.Rows.Count != 0)
        {
            promotionid = dt.Rows[0]["promotionid"].ToString();
            tagid = dt.Rows[0]["tagid"].ToString();

            //如果是同一个商品则不需要更改促销活动
            if (utils.NewRequest("productid", utils.RequestType.Form) == dt.Rows[0]["itemid"].ToString())
            {
                needUpdate = "0";
            }
            else
            {
                if (dt.Rows[0]["itemid"].ToString() != "")
                {
                    //清理之前的活动
                    IDictionary<string, string> param = new Dictionary<string, string>();
                    param.Add("promotion_id", promotionid);
                    string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.marketing.promotion.delete", session, param);
                    //Response.Write(result + "<br><br><br><br><br><br>");

                    //删除活动相关人群
                    param = new Dictionary<string, string>();
                    param.Add("tag_id", tagid);
                    result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.marketing.tag.delete", session, param);
                    //Response.Write(result + "<br><br><br><br><br><br>");
                }
            }
        }

        if (needUpdate == "1" && utils.NewRequest("isfree", utils.RequestType.Form) == "1")
        {
            //创建活动相关人群
            string guid = Guid.NewGuid().ToString().Substring(0, 4);
            IDictionary<string, string> param = new Dictionary<string, string>();
            param.Add("tag_name", left(nick + "_好评人群_" + guid, 20));
            param.Add("description", left(nick + "_好评描述_" + guid, 30));
            string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.marketing.tag.add", session, param);
            tagid = new Regex(@"<tag_id>([^<]*)</tag_id>", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
            //Response.Write(result + "<br><br><br><br><br><br>");

            if (result.IndexOf("error_response") != -1)
            {
                //清除多建的人群<user_tag><tag_id>896136</tag_id></user_tag>
                param = new Dictionary<string, string>();
                param.Add("fields", "tag_id");
                result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.marketing.tags.get", session, param);
                MatchCollection matchtag = new Regex(@"<user_tag><tag_id>([^<]*)</tag_id></user_tag>", RegexOptions.IgnoreCase).Matches(result);
                for (int x = 0; x < matchtag.Count; x++)
                {
                    //删除活动相关人群
                    param = new Dictionary<string, string>();
                    param.Add("tag_id", matchtag[x].Groups[1].ToString());
                    result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.marketing.tag.delete", session, param);
                    //Response.Write(result + "<br><br><br><br><br><br>");
                }
                Response.Redirect("setting.aspx");
                return;
            }

            //创建活动
            param = new Dictionary<string, string>();
            param.Add("num_iids", utils.NewRequest("productid", utils.RequestType.Form));
            param.Add("discount_type", "PRICE");
            param.Add("discount_value", (double.Parse(utils.NewRequest("price", utils.RequestType.Form)) - 0.01).ToString());
            param.Add("start_date", "2010-07-27 00:00:00");
            param.Add("end_date", "2020-01-01 00:00:00");
            param.Add("promotion_title", "好评有礼");
            param.Add("tag_id", tagid);
            result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.marketing.promotion.add", session, param);

            //Response.Write(result + "<br><br><br><br><br><br>");
            if (result.IndexOf("error_response") != -1)
            {
                string err = new Regex(@"<sub_msg>([^<]*)</sub_msg>", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
                Response.Write("<b>赠送礼品关联活动创建失败，错误原因：</b><br><font color='red'>" + result + "</font><br><a href='setting.aspx'>返回</a>");
                Response.End();
                return;
            }

            promotionid = new Regex(@"<promotion_id>([^<]*)</promotion_id>", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
        }

        //先判断是否有记录
        string sql = "SELECT COUNT(*) FROM TopAutoReview WHERE nick = '" + nick + "'";
        string count = utils.ExecuteString(sql);
        if (count == "0")
        {
            sql = "INSERT INTO TopAutoReview (" +
                        "nick, " +
                        "iscoupon, " +
                        "couponid, " +
                        "promotionid, " +
                        "tagid, " +
                        "isfree, " +
                        "itemid, " +
                        "iskefu, " +
                        "mindate, " +
                        "maxdate, " +
                        "issendmsg " +
                    " ) VALUES ( " +
                        " '" + nick + "', " +
                        " '" + utils.NewRequest("iscoupon", utils.RequestType.Form) + "', " +
                        " '" + utils.NewRequest("couponid", utils.RequestType.Form) + "', " +
                        " '" + promotionid + "', " +
                        " '" + tagid + "', " +
                        " '" + utils.NewRequest("isfree", utils.RequestType.Form) + "', " +
                        " '" + utils.NewRequest("productid", utils.RequestType.Form) + "', " +
                        " '" + utils.NewRequest("iskefu", utils.RequestType.Form) + "', " +
                        " '" + utils.NewRequest("mindate", utils.RequestType.Form) + "', " +
                        " '" + utils.NewRequest("maxdate", utils.RequestType.Form) + "', " +
                        " '" + utils.NewRequest("issendmsg", utils.RequestType.Form) + "' " +
                    ") ";
            utils.ExecuteNonQuery(sql);


            //如果改客户是第一次进入，则开始初始化数据
            InitOldData();
        }
        else
        {
            sql = "UPDATE TopAutoReview SET " +
                        "iscoupon = '" + utils.NewRequest("iscoupon", utils.RequestType.Form) + "', " +
                        "couponid = '" + utils.NewRequest("couponid", utils.RequestType.Form) + "', " +
                        "promotionid = '" + promotionid + "', " +
                        "tagid = '" + tagid + "', " +
                        "isfree = '" + utils.NewRequest("isfree", utils.RequestType.Form) + "', " +
                        "iskefu = '" + utils.NewRequest("iskefu", utils.RequestType.Form) + "', " +
                        "itemid = '" + utils.NewRequest("productid", utils.RequestType.Form) + "', " +
                        "mindate = '" + utils.NewRequest("mindate", utils.RequestType.Form) + "', " +
                        "maxdate = '" + utils.NewRequest("maxdate", utils.RequestType.Form) + "', " +
                        "updatedate = GETDATE(), " +
                        "issendmsg = '" + utils.NewRequest("issendmsg", utils.RequestType.Form) + "' " +
                    "WHERE nick = '" + nick + "'";
            //Response.Write(sql);
            utils.ExecuteNonQuery(sql);
        }

        Response.Write("<script>alert('保存成功！');window.location.href='setting.aspx';</script>");
        Response.End();
        return;
    }


    private string left(string str, int len)
    {
        if (str.Length > len)
        {
            str = str.Substring(0, len);
        }

        return str;
    }


    //删除活动
    //IDictionary<string, string> param = new Dictionary<string, string>();
    //param.Add("fields", "promotion_id, promotion_title, item_id, status, tag_id");
    //param.Add("num_iid", "9362115391");
    //string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.marketing.promotions.get", session, param);
    //Response.Write(result + "<br><br><br><br><br><br>");
    //return;

    //IDictionary<string, string> param = new Dictionary<string, string>();
    //param.Add("promotion_id", "63868317");
    //string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.marketing.promotion.delete", session, param);
    //Response.Write(result + "<br><br><br><br><br><br>");

    ////删除活动相关人群
    //param = new Dictionary<string, string>();
    //param.Add("tag_id", "901040");
    //result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.marketing.tag.delete", session, param);
    //Response.Write(result + "<br><br><br><br><br><br>");
    //return;





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