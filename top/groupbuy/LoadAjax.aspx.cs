using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Common;
using System.Data;
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

public partial class top_groupbuy_LoadAjax : System.Web.UI.Page
{
    string sql = "";
    string actionId = "";
    string iid = "";
    string discountType = "";
    string discountValue = "";
    string decreaseNum = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["actionType"] != null)
        {
            //LoadAjax.aspx?actionType=add&actionId=' + actionID + '&iid=' + iid + '&discountType=' + discountType + '&discountValue=' + discountValue + '&decreaseNum=' + decreaseNum

            actionId = Request.QueryString["actionId"].ToString();//活动ID
            iid = Request.QueryString["iid"].ToString();//商品ID
            discountType = Request.QueryString["discountType"].ToString();//DISCOUNT 或PRICE
            discountValue = Request.QueryString["discountValue"].ToString();//促销力度
            decreaseNum = Request.QueryString["decreaseNum"].ToString();//是否优惠限制

            if (Request.QueryString["actionType"].ToString() == "add")
            {
                addactivity();
            }
            if (Request.QueryString["actionType"].ToString() == "del")
            {
                delactivity();
            }
            Response.Write("");
            Response.End();
        }

        int mid = Request.QueryString["mid"] == null ? 0 : Request.QueryString["mid"] == "" ? 0 : int.Parse(Request.QueryString["mid"].ToString());
        int d = 10;
        sql = "select * from  TopMission  WHERE id = " + mid.ToString();
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt != null)
        {
            int tol = int.Parse(dt.Rows[0]["total"].ToString());//总数
            decimal su = int.Parse(dt.Rows[0]["success"].ToString()) + int.Parse(dt.Rows[0]["fail"].ToString());//已完成

            decimal num = 100;
            if (su < 1)
            {
                su = 1;
            }
            if (tol > 0)
            {
                num = (su / tol) * 100;
            }
            num = System.Math.Round(num, 2);
            Response.Write(num.ToString());
            Response.End();
        }
        else
        {
            Response.Write(d.ToString());
            Response.End();
        }

    }

    /// <summary>
    /// 添加活动
    /// </summary>
    public void addactivity()
    {
        Cookie cookie = new Cookie();
        string taobaoNick = cookie.getCookie("nick");
        string session = cookie.getCookie("top_sessiongroupbuy");
        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);
        //获取原宝贝
        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", "12287381", "d3486dac8198ef01000e7bd4504601a4");

        ItemGetRequest requestItem = new ItemGetRequest();
        requestItem.Fields = "desc,num_iid,title,price,pic_url ";
        requestItem.NumIid = long.Parse(iid);
        Item product = client.ItemGet(requestItem, session);

        sql = "select * from tete_activity where id=" + actionId;
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt != null && dt.Rows.Count > 0)
        {
            discountType = Request.QueryString["discountType"].ToString();//DISCOUNT 或PRICE
            discountValue = Request.QueryString["discountValue"].ToString();//促销力度
            decreaseNum = Request.QueryString["decreaseNum"].ToString();//是否优惠限制
            string rcounts = Request.QueryString["rcounts"].ToString();//团购人数
            sql = "INSERT    [tete_activitylist] ([ActivityID] ,[Productname] ,[Productprice] ,[ProductImg] ,[ProductUrl] ,[ProductID] ,[promotionID] ,[Name] ,[Description] ,[Remark] ,[startDate] ,[endDate] ,[itemType] ,[discountType] ,[discountValue] ,[tagId] ,[Status] ,[Rcount] ,[Nick] ,[decreaseNum] ,[isOK])     VALUES(" + actionId + ",'" + product.Title + "','" + product.Price + "','" + product.PicUrl + "','http://item.taobao.com/item.htm?id=" + product.NumIid.ToString() + "','" + iid + "',0,'" + dt.Rows[0]["Name"].ToString() + "','" + dt.Rows[0]["Description"].ToString() + "','" + dt.Rows[0]["Remark"].ToString() + "','" + dt.Rows[0]["startDate"].ToString() + "','" + dt.Rows[0]["endDate"].ToString() + "','" + dt.Rows[0]["itemType"].ToString() + "','" + discountType + "','" + discountValue + "','" + dt.Rows[0]["tagId"].ToString() + "',0," + rcounts + ",'" + taobaoNick + "'," + decreaseNum + ",0)";
           
            utils.ExecuteNonQuery(sql);

 

            //创建活动及相关人群
            string appkey = "12287381";
            string secret = "d3486dac8198ef01000e7bd4504601a4";



            //创建活动相关人群
            string guid = Guid.NewGuid().ToString().Substring(0, 4);
            IDictionary<string, string> param = new Dictionary<string, string>();

            string tagid = "1"; //new Regex(@"<tag_id>([^<]*)</tag_id>", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();

            //创建活动
            param = new Dictionary<string, string>();
            param.Add("num_iids", iid);
            param.Add("discount_type", discountType);
            param.Add("discount_value", discountValue);
            param.Add("start_date", DateTime.Parse(dt.Rows[0]["startDate"].ToString()).ToString("yyyy-MM-dd hh:mm:ss"));
            param.Add("end_date", DateTime.Parse(dt.Rows[0]["endDate"].ToString()).ToString("yyyy-MM-dd hh:mm:ss"));
            param.Add("promotion_title", dt.Rows[0]["Name"].ToString());
            param.Add("decrease_num", decreaseNum);


            param.Add("tag_id", tagid);
            string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.marketing.promotion.add", session, param);


            if (result.IndexOf("error_response") != -1)
            {
                sql = "delete from    [tete_activitylist]    WHERE ActivityID = " + actionId + " and  ProductID=" + iid;

                utils.ExecuteNonQuery(sql);

                string err = new Regex(@"<sub_msg>([^<]*)</sub_msg>", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
                if (err == "")
                {
                    Response.Write("<b>活动创建失败，错误原因：</b><br><font color='red'>您的session已经失效，需要重新授权</font><br><a href='http://container.api.taobao.com/container?appkey=12287381&scope=promotion' target='_parent'>重新授权</a>");
                    Response.End();
                }

                Response.Write("<b>活动创建失败，错误原因：</b><br><font color='red'>" + err + "</font><br><a href='groupbuyadd.aspx'>重新添加</a>");
                Response.End();
                return;
            }

            string promotionid = new Regex(@"<promotion_id>([^<]*)</promotion_id>", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
 
            //更新活动
            sql = "update  tete_activitylist set Status=1 ,isok=1,promotionID=" + promotionid + "  WHERE ActivityID = " + actionId + " and  ProductID=" + iid;
            utils.ExecuteNonQuery(sql);


            Response.Write("true");
            Response.End();
        }
        else
        {
            Response.Write("null");
            Response.End();
        }
    }

    /// <summary>
    /// 删除活动
    /// </summary>
    public void delactivity()
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

        //删除活动
        sql = "update  tete_activitylist set Status=4 ,isok=1  WHERE ActivityID = " + actionId + " and  ProductID=" + iid;
        utils.ExecuteNonQuery(sql);

        Response.Write("true");
        Response.End();

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
        MD5 md5 = System.Security.Cryptography.MD5.Create();

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
                // postData.Append(Uri.EscapeDataString(value));
                postData.Append(GetUriFormate(value));
                hasParam = true;
            }
        }
        return postData.ToString();
    }

    /// <summary>
    /// 将参数转换成 uri 格式
    /// </summary>
    /// <param name="inputString">string类型的字符串</param>
    /// <returns>编码后的string</returns>
    private static string GetUriFormate(string inputString)
    {
        StringBuilder strBuilder = new StringBuilder();
        string sourceStr = inputString;
        int len = sourceStr.Length;
        do
        {
            if (len - 21766 <= 0)
            {
                strBuilder.Append(Uri.EscapeDataString(sourceStr));
            }
            else
            {
                strBuilder.Append(Uri.EscapeDataString(sourceStr.Substring(0, 21766)));

                sourceStr = sourceStr.Substring(21766);
                len = sourceStr.Length;
                if (len - 21766 < 0)
                {
                    strBuilder.Append(Uri.EscapeDataString(sourceStr));
                }
            }
        }
        while (len - 21766 > 0);

        return strBuilder.ToString();
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
        System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
        req.Method = "POST";
        req.KeepAlive = true;
        req.Timeout = 300000;
        req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
        byte[] postData = Encoding.UTF8.GetBytes(PostData(param));
        Stream reqStream = req.GetRequestStream();
        reqStream.Write(postData, 0, postData.Length);
        reqStream.Close();
        System.Net.HttpWebResponse rsp = (System.Net.HttpWebResponse)req.GetResponse();
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
