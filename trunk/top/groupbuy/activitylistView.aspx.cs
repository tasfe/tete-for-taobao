using System;
using System.Collections.Generic; 
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Common; 
using Taobao.Top.Api;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;

public partial class top_groupbuy_activitylistView : System.Web.UI.Page
{
    public string teteendDate = string.Empty;
    public string nick = string.Empty;
    public string name = "";
    public string memo = "";
    public string startDate = "";
    public string endDate = "";
    public string itemType = "";
    public string discountType = "";
    public string zhe = "";
    public string yuan = "";
    public string decreaseNum = "";
    public string rcount = "";
    public string tagId = "";
    public string status = "";
    public string itemTypeStr = "";
    public string discountTypeStr = "";
    public string decreaseNumStr = "";
    public string discountValue = "";
    public string imgStr = "";
    public string shopgroupbuyEnddate2 = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        Rijndael_ encode = new Rijndael_("tetesoft");
        string sql23 = "select enddate from TopTaobaoShop where nick='" + nick + "'";
        DataTable dt32 = utils.ExecuteDataTable(sql23);

        if (dt32 != null && dt32.Rows.Count > 0)
        {
            shopgroupbuyEnddate2 = dt32.Rows[0]["enddate"].ToString();
            teteendDate = dt32.Rows[0]["enddate"].ToString();
        }
        nick = encode.Decrypt(taobaoNick);
        if (nick == "")
        {
            //Response.Write("top签名验证不通过，请不要非法注入");
            //Response.End();
            //return;
        }
        if (Request.Form["act"] == "post")
        {
            string activityID = Request.QueryString["activityID"].ToString();
            string ID = Request.QueryString["ID"].ToString();

            startDate =DateTime.Parse(Request.Form["startDate"].ToString()).ToString("yyyy-MM-dd hh:mm:ss");
            endDate = DateTime.Parse(Request.Form["endDate"].ToString()).ToString("yyyy-MM-dd hh:mm:ss");
            discountType = Request.Form["discountType"].ToString();
            zhe = Request.Form["zhe"].ToString();
            yuan = Request.Form["yuan"].ToString();
            decreaseNum = Request.Form["decreaseNum"].ToString();
            rcount = Request.Form["Rcount"].ToString();
            tagId = "1";
            status = "1";//进行中 
            #region  数据格式验证
            if (DateTime.Parse(startDate) > DateTime.Now)
            {
                status = "0";//未开始
            }
            sql23 = "select enddate from TopTaobaoShop where nick='" + nick + "'";
            dt32 = utils.ExecuteDataTable(sql23);

            if (dt32 != null && dt32.Rows.Count > 0)
            {
                shopgroupbuyEnddate2 = dt32.Rows[0]["enddate"].ToString();

            }
            if (DateTime.Parse(shopgroupbuyEnddate2) < DateTime.Now)
            {
                Response.Write("<script>alert('活动结束时间不能大于服务使用结束时间！')</script>");
                return;
            }

            if (DateTime.Parse(endDate) < DateTime.Now)
            {
                Response.Write("<script>alert('活动结束时间不能小于当前时间！')</script>");
                return;
            }
            //每个参加活动的宝贝设置相同促销力度

            //促销方式
            if (Request.Form["discountType"].ToString() == "DISCOUNT")
            {
                if (!isNumber(Request.Form["zhe"].ToString()))
                {
                    Response.Write("<script>alert('折扣格式不正确！')</script>");
                    return;
                }
                discountValue = zhe;
            }
            else
            {
                if (!isNumber(Request.Form["yuan"].ToString()))
                {
                    Response.Write("<script>alert('金额格式不正确！')</script>");
                    return;
                }
                discountValue = yuan;
            }

            if (!isNumber(rcount))
            {
                rcount = "0";
            }

            if (!isDate(Request.Form["startDate"].ToString()))
            {
                Response.Write("<script>alert('团购时间格式不正确！')</script>");
                return;
            }
            if (!isDate(Request.Form["endDate"].ToString()))
            {
                Response.Write("<script>alert('团购时间格式不正确！')</script>");
                return;
            }
            #endregion
            string sql = "update tete_activity set  startDate='" + startDate + "',endDate='" + endDate + "',discountType='" + discountType + "',discountValue='" + discountValue + "',tagId='" + tagId + "',Rcount=" + rcount + ",nick='" + nick + "', decreaseNum='" + decreaseNum + "'  where id=" + activityID; //更新活动

            utils.ExecuteNonQuery(sql);

            sql = "select * from tete_activitylist where  ID=" + ID;

            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                delactivity(dt.Rows[0]["ActivityID"].ToString(), dt.Rows[0]["ProductID"].ToString());
                sql = "update tete_activitylist set startDate='" + startDate + "',endDate='" + endDate + "',discountType='" + discountType + "',discountValue='" + discountValue + "',tagId='" + tagId + "',Rcount=" + rcount + ",Status=1,decreaseNum='" + decreaseNum + "',isok=0 where ID=" + ID;

                utils.ExecuteNonQuery(sql);//修改活动商品  '延长修改活动 Status=1 和 isok=0 '
            }


            Response.Redirect("activitygetitem.aspx?activityID=" + activityID);
        }
        if (!IsPostBack)
        {
            string sql = "select enddate from TopTaobaoShop where nick='" + nick + "'";
            DataTable dt = utils.ExecuteDataTable(sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                shopgroupbuyEnddate.Value = dt.Rows[0]["enddate"].ToString();//特特结束时间
                teteendDate = dt.Rows[0]["enddate"].ToString();
            }
            sql = "select * from [tete_activitylist] where ID=" + Request.QueryString["ID"].ToString();

            dt = utils.ExecuteDataTable(sql);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    imags.Src = dt.Rows[i]["ProductImg"].ToString();

                    startDate = DateTime.Parse(dt.Rows[i]["startDate"].ToString()).ToString("yyyy-MM-dd hh:mm:ss");
                    endDate = DateTime.Parse(dt.Rows[i]["endDate"].ToString()).ToString("yyyy-MM-dd hh:mm:ss");
                    itemType = dt.Rows[i]["itemType"].ToString();
                    if (itemType != "same")
                    {
                        itemType = "";
                        itemTypeStr = "checked";
                        Detailtype.Value = "2";
                    }
                    else
                    {
                        itemType = "checked";
                        itemTypeStr = "";
                        Detailtype.Value = "1";
                    }
                    discountType = dt.Rows[i]["discountType"].ToString();
                    if (discountType != "DISCOUNT")
                    {
                        discountType = "";
                        discountTypeStr = "checked";
                    }
                    else
                    {
                        discountType = "checked";
                        discountTypeStr = "";
                    }
                    zhe = dt.Rows[i]["discountValue"].ToString();
                    yuan = dt.Rows[i]["discountValue"].ToString();
                    decreaseNum = dt.Rows[i]["decreaseNum"].ToString();
                    if (decreaseNum == "0")
                    {
                        decreaseNum = "selected";
                        decreaseNumStr = "";
                    }
                    else
                    {
                        decreaseNum = "";
                        decreaseNumStr = "selected";
                    }
                    rcount = dt.Rows[i]["Rcount"].ToString();
                }
            }
        }
    }

 
    /// <summary>
    /// 删除活动
    /// </summary>
    /// <param name="actionId">活动ID</param>
    /// <param name="iid">宝贝id</param>
    public void delactivity(string actionId,string iid)
    {
        //删除活动
        string appkey = "12287381";
        string secret = "d3486dac8198ef01000e7bd4504601a4";
        IDictionary<string, string> param = new Dictionary<string, string>();
        Common.Cookie cookie = new Common.Cookie();
        string session = cookie.getCookie("top_sessiongroupbuy");

        //通过数据库查询获取活动ID actionId iid
        string sql = "SELECT promotionID FROM tete_activitylist WHERE ActivityID = " + actionId + " and  ProductID=" + iid;
        string promotion_id = utils.ExecuteString(sql);

        //删除活动
        param = new Dictionary<string, string>();
        param.Add("promotion_id", promotion_id);
        string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.marketing.promotion.delete", session, param);

    }

        
    /// <summary>
    /// 验证是否数字类型
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public bool isNumber(string obj)
    {

        try
        {
            Convert.ToInt32(obj);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 验证是否时间数据
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public bool isDate(string obj)
    {
        try
        {
            Convert.ToDateTime(obj);
            return true;
        }
        catch
        {
            return false;
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