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

public partial class top_groupbuy_groupbuyadd : System.Web.UI.Page
{
    public string startdate = string.Empty;
    public string todate = string.Empty;
    public string enddate = string.Empty;
    public string nowstr = string.Empty;
    public string nick = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Common.Cookie cookie1 = new Common.Cookie();
        string taobaoNick = cookie1.getCookie("nick");
        string isAct = cookie1.getCookie("act");
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Decrypt(taobaoNick);

        //过期判断
        if (string.IsNullOrEmpty(taobaoNick))
        {
            string msg = "尊敬的淘宝卖家，非常抱歉的告诉您，您还没有购买此服务或者登录信息已失效，如需继续使用请<a href='http://fuwu.taobao.com/serv/detail.htm?service_id=764' target='_blank'>进入该服务</a>，谢谢！";
            Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
            Response.End();
            return;
        }

        //过期判断
        if (isAct != "1")
        {
            string msg = "尊敬的" + nick + "，非常抱歉的告诉您，您尚未订购该功能，如需继续使用请<a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-10:1;' target='_blank'>购买该服务</a>，谢谢！";
            Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
            Response.End();
            return;
        }

        FormatData();
    }

    /// <summary>
    /// 根据当前时间初始化页面控件值
    /// </summary>
    private void FormatData()
    {
        nowstr = DateTime.Now.ToString();
        //获取服务器当前日期
        DateTime now = DateTime.Now;
        DateTime to = DateTime.Now.AddDays(1);
        DateTime end = DateTime.Now.AddDays(13);

        startdate = now.Month.ToString() + "/" + now.Day.ToString() + "/" + now.Year.ToString();
        todate = to.Month.ToString() + "/" + to.Day.ToString() + "/" + to.Year.ToString();
        enddate = end.Month.ToString() + "/" + end.Day.ToString() + "/" + end.Year.ToString();

        //选择当前时间节点
        int hour = DateTime.Now.Hour + 1;

        //如果是11点则选择0
        if (hour == 24)
        {
            hour = 0;
        }

        startSelect.SelectedIndex = hour;
        endSelect.SelectedIndex = hour;
    }

    /// <summary>
    /// 完成创建
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        string session = cookie.getCookie("top_session");
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Decrypt(taobaoNick);

        //判断人群数不得超过4个
        if (ActCountJudge())
        {
            ShowErr("您最多只能创建4个活动");
            return;
        }

        //后端判断数据但是否合法
        if (DataJudge())
        {
            ShowErr("录入数据不合法");
            return;
        }

        //插入数据库
        string groupbuyname = utils.NewRequest("groupbuyname", utils.RequestType.Form).Replace("'", "''");
        string starttime = utils.NewRequest("starttime", utils.RequestType.Form) + " " + utils.NewRequest("startSelect", utils.RequestType.Form).Replace("'", "''") + ":00";
        string endtime = utils.NewRequest("endtime", utils.RequestType.Form) + " " + utils.NewRequest("endSelect", utils.RequestType.Form).Replace("'", "''") + ":00";
        string groupbuyprice = utils.NewRequest("groupbuyprice", utils.RequestType.Form);
        string productid = utils.NewRequest("productid", utils.RequestType.Form);
        string maxcount = utils.NewRequest("maxcount", utils.RequestType.Form);
        string zhekou = utils.NewRequest("zhekou", utils.RequestType.Form);
        string mintime = utils.NewRequest("mintime", utils.RequestType.Form);


        //通过借口获取淘宝相关数据
        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", "12132145", "1fdd2aadd5e2ac2909db2967cbb71e7f");
        ItemGetRequest request = new ItemGetRequest();
        request.Fields = "num_iid,title,price,pic_url";
        request.NumIid = long.Parse(productid);

        Item product = client.ItemGet(request, session);


        string newprice = Math.Round(decimal.Parse(product.Price) - decimal.Parse(zhekou), 2).ToString();

        //创建活动及相关人群
        string appkey = "12132145";
        string secret = "1fdd2aadd5e2ac2909db2967cbb71e7f";

        ////创建活动相关人群
        //string guid = Guid.NewGuid().ToString().Substring(0, 4);
        IDictionary<string, string> param = new Dictionary<string, string>();
        //param.Add("tag_name", nick + "_活动人群_" + guid);
        //param.Add("description", nick + "_活动人群描述_" + guid);
        //string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.marketing.tag.add", session, param);
        //string tagid = new Regex(@"<tag_id>([^<]*)</tag_id>", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();

        //Response.Write(result + "<br><br>");

        //目前针对全部商城会员
        string tagid = "1";

        //创建活动
        param = new Dictionary<string, string>();
        param.Add("num_iids", productid);
        param.Add("discount_type", "PRICE");
        param.Add("discount_value", newprice);
        param.Add("start_date", starttime);
        param.Add("end_date", endtime);
        param.Add("promotion_title", groupbuyname);
        param.Add("tag_id", tagid);
        string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.marketing.promotion.add", session, param);

        if (result.IndexOf("Insufficient session permissions") != -1)
        {
            Response.Write("<b>优惠券创建失败，错误原因：</b><br><font color='red'>您的session已经失效，需要重新授权</font><br><a href='http://container.api.taobao.com/container?appkey=12132145&scope=promotion' target='_parent'>重新授权</a>");
            Response.End();
            return;
        }

        if (result.IndexOf("error_response") != -1)
        {
            string err = new Regex(@"<sub_msg>([^<]*)</sub_msg>", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
            Response.Write("<b>活动创建失败，错误原因：</b><br><font color='red'>" + err + "</font><br><a href='groupbuyadd.aspx'>重新添加</a>");
            Response.End();
            return;
        }


        string promotionid = new Regex(@"<promotion_id>([^<]*)</promotion_id>", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();

        //Response.Write(result + "<br><br>");


        string productname = product.Title;
        string productprice = product.Price;
        string productimg = product.PicUrl;
        string producturl = "http://item.taobao.com/item.htm?id=" + productid;


        string sql = "INSERT INTO TopGroupBuy (" +
                       "name," +
                       "starttime," +
                       "endtime," +
                       "productname," +
                       "productprice," +
                       "productimg," +
                       "producturl," +
                       "productid," +
                       "nick," +
                       "maxcount," +
                       "buycount," +
                       "tagid," +
                       "promotionid," +
                       "mintime," +
                       "zhekou," +
                       "groupbuyprice" +
                   " ) VALUES ( " +
                       " '" + groupbuyname + "'," +
                       " '" + starttime + "'," +
                       " '" + endtime + "'," +
                       " '" + productname + "'," +
                       " '" + productprice + "'," +
                       " '" + productimg + "'," +
                       " '" + producturl + "'," +
                       " '" + productid + "'," +
                       " '" + nick + "'," +
                       " '" + maxcount + "'," +
                       " '0'," +
                       " '" + tagid + "'," +
                       " '" + promotionid + "'," +
                       " '" + mintime + "'," +
                       " '" + newprice + "'," +
                       " '" + zhekou + "'" +
                 ") ";

        //Response.Write(sql);
        //Response.End();
        //return;
        utils.ExecuteNonQuery(sql);

        

        //将测试用户加入该活动关联人群组
        //IDictionary<string, string> param = new Dictionary<string, string>();
        //param.Add("tag_id", tagid);
        //param.Add("nick", "美杜莎之心");

        //string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.marketing.taguser.add", session, param);

        //Response.Write(result);

        //查询活动详细
        //IDictionary<string, string> param = new Dictionary<string, string>();
        //param.Add("fields", "num_iids,discount_type,discount_value,start_date,end_date,promotion_title,tag_id");
        //param.Add("num_iid", "7591980225");

        //string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.marketing.promotions.get", session, param);

        //Response.Write(result);

        //更新商品描述关联的广告图片
        CreateGroupbuyImg();
        Response.Redirect("grouplist.aspx");
    }

    /// <summary>
    /// 创建活动关联的广告图片
    /// </summary>
    private void CreateGroupbuyImg()
    {
        return;
    }

    private bool DataJudge()
    {
        return false;
    }

    private bool ActCountJudge()
    {
        return false;
    }

    private void ShowErr(string p)
    {
        return;
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