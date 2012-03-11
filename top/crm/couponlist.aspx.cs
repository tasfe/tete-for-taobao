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
using ThoughtWorks.QRCode.Codec;

public partial class top_review_couponlist : System.Web.UI.Page
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
            string msg = "尊敬的" + nick + "，非常抱歉的告诉您，您尚未订购该功能，如需继续使用请购买该服务:<br><br>19元/月 【赠送短信50条】 <a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-4:1;' target='_blank'>立即购买</a><br><br>54元/季 【赠送短信150条】 <a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-4:3;' target='_blank'>立即购买</a><br><br>99元/半年 【赠送短信300条】<a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-4:6;' target='_blank'>立即购买</a><br><br>188元/年 【赠送短信600条】<a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-4:12;' target='_blank'>立即购买</a><br>";
            Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
            Response.End();
            return;
        }

        string act = utils.NewRequest("act", utils.RequestType.QueryString);
        if (act == "del")
        {
            DeleteGroup();
            return;
        }

        if (act == "create")
        {
            CreateCode(id);
            return;
        }

        BindData();
    }

    /// <summary>
    /// 生成二维码图片
    /// </summary>
    private void CreateCode(string couponid)
    {
        QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
        String encoding = "Byte";
        if (encoding == "Byte")
        {
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
        }
        else if (encoding == "AlphaNumeric")
        {
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC;
        }
        else if (encoding == "Numeric")
        {
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.NUMERIC;
        }
        try
        {
            int scale = Convert.ToInt16("4");
            qrCodeEncoder.QRCodeScale = scale;
        }
        catch (Exception ex)
        {
            //MessageBox.Show("Invalid size!");
            return;
        }
        try
        {
            int version = Convert.ToInt16("3");
            qrCodeEncoder.QRCodeVersion = version;
        }
        catch (Exception ex)
        {
            //MessageBox.Show("Invalid version !");
        }

        string errorCorrect = "L";
        if (errorCorrect == "L")
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
        else if (errorCorrect == "M")
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
        else if (errorCorrect == "Q")
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
        else if (errorCorrect == "H")
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;

        System.Drawing.Image image;
        string data = "http://iphone.tetesoft.com/down/wbl/mshop.apk";
        image = qrCodeEncoder.Encode(data);
        data = "qrcode/test";

        image.Save(Server.MapPath(data + ".jpg"));

        Response.Redirect(data + ".jpg");
    }

    /// <summary>
    /// 删除优惠券信息
    /// </summary>
    private void DeleteGroup()
    {
        //判断如果活动未开始或进行中则可以关闭活动
        string id = utils.NewRequest("id", utils.RequestType.QueryString);
        //if (!utils.IsInt32(id))
        //{
        //    return;
        //}

        //删除活动
        string appkey = "12159997";
        string secret = "614e40bfdb96e9063031d1a9e56fbed5";
        IDictionary<string, string> param = new Dictionary<string, string>();
        Common.Cookie cookie = new Common.Cookie();
        string session = cookie.getCookie("top_sessiongroupbuy");

        //通过数据库查询获取活动ID
        string sql = "UPDATE TCS_Coupon SET isdel = 1 WHERE guid = '" + id + "'";
        utils.ExecuteNonQuery(sql);

        Response.Write("<script>alert('取消成功！');window.location.href='couponlist.aspx';</script>");
    }

    private void BindData()
    {
        string sql = "SELECT * FROM TCS_Coupon WHERE nick = '" + nick + "' AND isdel = 0";
        DataTable dt = utils.ExecuteDataTable(sql);

        rptArticle.DataSource = dt;
        rptArticle.DataBind();
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