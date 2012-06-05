using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

public partial class api_Default : System.Web.UI.Page
{
    private string act = string.Empty;
    private string uid = string.Empty;
    private string cid = string.Empty;
    private string itemid = string.Empty;
    private string page = string.Empty;
    private string pagesize = string.Empty;
    private string direct = string.Empty;
    private string token = string.Empty;
    private string err = string.Empty;
    private string typ = string.Empty;
    private string msgid = string.Empty;
    private string buynick = string.Empty;
    private string couponid = string.Empty;
    private string qrcode = string.Empty;
    private string mobile = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        //try
        //{
            act = utils.NewRequest("act", utils.RequestType.QueryString);
            uid = utils.NewRequest("uid", utils.RequestType.QueryString);   
            cid = utils.NewRequest("cid", utils.RequestType.QueryString);
            itemid = utils.NewRequest("itemid", utils.RequestType.QueryString);
            page = utils.NewRequest("page", utils.RequestType.QueryString);
            pagesize = utils.NewRequest("pagesize", utils.RequestType.QueryString);
            direct = utils.NewRequest("direct", utils.RequestType.QueryString);
            token = utils.NewRequest("token", utils.RequestType.QueryString);
            typ = utils.NewRequest("typ", utils.RequestType.QueryString);
            msgid = utils.NewRequest("msgid", utils.RequestType.QueryString);
            buynick = utils.NewRequest("buynick", utils.RequestType.QueryString);
            couponid = utils.NewRequest("couponid", utils.RequestType.QueryString);
            mobile = utils.NewRequest("mobile", utils.RequestType.QueryString);
            qrcode = utils.NewRequest("qrcode", utils.RequestType.QueryString);

            err = utils.NewRequest("err", utils.RequestType.Form);

            switch (act)
            {
                case "ads":
                    ShowAdsInfo();
                    break;
                case "cate":
                    ShowCateInfo();
                    break;
                case "list":
                    ShowListInfo();
                    break;
                case "special":
                    ShowSpecialListInfo();
                    break;
                case "detail":
                    ShowDetailInfo();
                    break;
                case "near":
                    ShowNearInfo();
                    break;
                case "token":
                    RecordTokenInfo();
                    break;
                case "err":
                    RecordErrInfo();
                    break;
                case "msgcount":
                    ShowMsgCountInfo();
                    break;
                case "msglist":
                    ShowMsgListInfo();
                    break;
                case "msgdetail":
                    ShowMsgDetailInfo();
                    break;
                case "getcoupon":
                    GetCouponInfo();
                    break;
                case "couponlist":
                    ShowCouponListInfo();
                    break;
                //case "bindtoken":
                //    BindToken();
                //    break;
            }
        //}
        //catch
        //{
        //    string str = "{\"error_response\":\"service_error\"}";
        //    Response.Write(str);
        //}
    }

    /// <summary>
    /// 
    /// </summary>
    private void ShowCouponListInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;

        sql = "SELECT * FROM TeteUserCoupon WHERE token = '" + token + "' AND nick = '" + uid + "' ORDER BY id DESC";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str = "{\"coupon\":[";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i != 0)
                {
                    str += ",";
                }

                str += "{\"id\":\"" + dt.Rows[i]["id"].ToString() + "\",\"title\":\"" + dt.Rows[i]["title"].ToString() + "\",\"date\":\"" + dt.Rows[i]["adddate"].ToString() + "\",\"isread\":\"" + dt.Rows[i]["isread"].ToString() + "\"}";
            }
            str += "]}";
        }
        else
        {
            str = "{\"count\":\"0\"}";
        }

        Response.Write(str);
    }

    ///// <summary>
    ///// 绑定会员名到TOKEN上
    ///// </summary>
    //private void BindToken()
    //{
    //    string sql = string.Empty;
    //    string str = string.Empty;

    //    //绑定淘宝会员和该手机
    //}

    private void GetCouponInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;

        //判断该2唯码是否有效,如果无效给出提示
        if (1 == 2)
        {
            Response.Write("{\"result\":\"errNumber\"}");
            return;
        }

        //如果有效则判断对应的2唯码是否过期，如果过期给出提示
        if (1 == 2)
        {
            Response.Write("{\"result\":\"expireNumber\"}");
            return;
        }

        //判断该会员是否领取过该优惠券，如果领过给出提示
        if (1 == 2)
        {
            Response.Write("{\"result\":\"alreadyGet\"}");
            return;
        }

        //如果没过期则赠送给该淘宝会员，如果该会员没在此店铺购买过东西给出提示
        if (1 == 2)
        {
            Response.Write("{\"result\":\"notUser\"}");
            return;
        }

        couponid = qrcode.Replace("{\"coupon\":\"", "").Replace("\"}", "");

        //给客户赠送优惠券并给出提示
        string number = "110";
        sql = "SELECT taobaocouponid FROM TeteCoupon WHERE guid = '" + couponid + "'";
        
        string taobaocouponid = utils.ExecuteString(sql);
        //执行优惠券赠送行为
        string appkey = "12159997";
        string secret = "614e40bfdb96e9063031d1a9e56fbed5";
        string session = "610261249120700fcda21f569d424328537a3d82a101d7c204200856";
        //buynick = "叶儿随清风";

        IDictionary<string, string> param = new Dictionary<string, string>();
        param.Add("coupon_id", taobaocouponid);
        param.Add("buyer_nick", buynick);

        string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.promotion.coupon.send", session, param);
        number = new Regex(@"<coupon_number>([^<]*)</coupon_number>", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
        //Response.Write(result);
        //Response.Write("<br>");
        //Response.Write(number);
        Response.Write("{\"url\":\"http://shop57734865.taobao.com/\"}");
    }

    private void ShowMsgDetailInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;

        sql = "SELECT * FROM TeteUserMsg WHERE nick = '" + uid + "' AND id = " + msgid + "";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str += "{\"id\":\"" + dt.Rows[0]["id"].ToString() + "\",\"html\":\"" + dt.Rows[0]["html"].ToString() + "\"}";
            
            //更新标记为已读
            sql = "UPDATE TeteUserMsg SET isread = 1 WHERE nick = '" + uid + "' AND id = " + msgid + "";
            utils.ExecuteNonQuery(sql);
        }
        else
        {
            str = "{\"count\":\"0\"}";
        }

        Response.Write(str);
    }

    private void ShowMsgListInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;

        sql = "SELECT * FROM TeteUserMsg WHERE token = '" + token + "' AND nick = '" + uid + "' ORDER BY id DESC";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str = "{\"msg\":[";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i != 0)
                {
                    str += ",";
                }

                str += "{\"id\":\"" + dt.Rows[i]["id"].ToString() + "\",\"title\":\"" + dt.Rows[i]["title"].ToString() + "\",\"date\":\"" + dt.Rows[i]["adddate"].ToString() + "\",\"isread\":\"" + dt.Rows[i]["isread"].ToString() + "\"}";
            }
            str += "]}";
        }
        else
        {
            str = "{\"count\":\"0\"}";
        }

        Response.Write(str);
    }

    private void ShowMsgCountInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;

        sql = "SELECT COUNT(*) FROM TeteUserMsg WHERE token = '" + token + "' AND nick = '" + uid + "' AND isread = 0";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str = "{\"count\":\"" + dt.Rows[0][0].ToString() + "\"}";
        }
        else
        {
            str = "{\"count\":\"\",\"0\":\"\"}";
        }

        Response.Write(str);
    }


    private void RecordErrInfo()
    {
        string sql = string.Empty;


        sql = "INSERT INTO TeteUserErr (err, token, nick) VALUES ('" + err + "', '" + token + "', '" + uid + "')";
        utils.ExecuteNonQuery(sql);
        

        string str = "{\"result\":\"ok\"}";
        Response.Write(str);
    }

    private void RecordTokenInfo()
    {
        string sql = string.Empty;

        sql = "SELECT COUNT(*) FROM TeteUserToken WHERE nick = '" + uid + "' AND token = '" + token + "'";
        string count = utils.ExecuteString(sql);

        if (count == "0")
        {
            sql = "INSERT INTO TeteUserToken (nick, token, mobile) VALUES ('" + uid + "', '" + token + "', '" + mobile + "')";
            utils.ExecuteNonQuery(sql);
        }
        else
        {
            sql = "UPDATE TeteUserToken SET mobile = '" + mobile + "',updatedate = GETDATE(),logintimes = logintimes + 1 WHERE token = '" + token + "'";
            utils.ExecuteNonQuery(sql);
        }

        string str = "{\"result\":\"ok\"}";
        Response.Write(str);
    }

    private void ShowNearInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;

        if (direct == "left")
        {
            sql = "SELECT TOP 1 * FROM TeteShopItem WHERE nick = '" + uid + "' AND id < (SELECT id FROM TeteShopItem WHERE itemid = " + itemid + ") AND CHARINDEX('" + cid + "', cateid) > 0 ORDER BY id DESC";
        }
        else
        {
            sql = "SELECT TOP 1 * FROM TeteShopItem WHERE nick = '" + uid + "' AND id > (SELECT id FROM TeteShopItem WHERE itemid = " + itemid + ") AND CHARINDEX('" + cid + "', cateid) > 0 ORDER BY id ASC";
        }

        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str = "{\"item\":[";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i != 0)
                {
                    str += ",";
                }

                str += "{\"itemid\":\"" + dt.Rows[i]["itemid"].ToString() + "\",\"pic_url\":\"" + dt.Rows[i]["picurl"].ToString() + "\",\"name\":\"" + dt.Rows[i]["itemname"].ToString() + "\",\"detail_url\":\"" + dt.Rows[i]["linkurl"].ToString() + "\"}";
            }
            str += "]}";
        }
        else
        {
            str = "{\"count\":\"0\"}";
        }

        Response.Write(str);
    }

    private void ShowDetailInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;

        sql = "SELECT * FROM TeteShopItem WHERE nick = '" + uid + "' AND itemid = " + itemid + "";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str = "{\"item\":[";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i != 0)
                {
                    str += ",";
                }

                str += "{\"itemid\":\"" + dt.Rows[i]["itemid"].ToString() + "\",\"pic_url\":\"" + dt.Rows[i]["picurl"].ToString() + "\",\"name\":\"" + dt.Rows[i]["itemname"].ToString() + "\",\"detail_url\":\"" + dt.Rows[i]["linkurl"].ToString() + "\"}";
            }
            str += "]}";
        }
        else
        {
            str = "{\"count\":\"0\"}";
        }

        Response.Write(str);
    }


    private void ShowSpecialListInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;

        int pageNow = 1;
        if (page == "")
        {
            pageNow = 1;
        }
        else
        {
            pageNow = int.Parse(page);
        }

        int pageSizeNow = 20;
        if (pagesize == "")
        {
            pageSizeNow = 20;
        }
        else
        {
            pageSizeNow = int.Parse(pagesize);
        }
        int pageCount = pageSizeNow;
        int dataCount = (pageNow - 1) * pageCount;


        sql = "SELECT COUNT(*) FROM TeteShopItem WHERE nick = '" + uid + "'";
        int totalCount = int.Parse(utils.ExecuteString(sql));
        int totalPageCount = 1;

        if (totalCount % pageCount == 0)
        {
            totalPageCount = totalCount / pageCount;
        }
        else
        {
            totalPageCount = totalCount / pageCount + 1;
        }

        sql = "SELECT TOP " + pageCount.ToString() + " * FROM (SELECT *,ROW_NUMBER() OVER (ORDER BY id DESC) AS rownumber FROM TeteShopItem WHERE nick = '" + uid + "') AS a WHERE a.rownumber > " + dataCount.ToString() + " ORDER BY id DESC";
        Response.Write(sql);
        //sql = "SELECT * FROM TeteShopItem WHERE nick = '" + uid + "' AND CHARINDEX('" + cid + "', cateid) > 0";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str = "{\"item\":[";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i != 0)
                {
                    str += ",";
                }

                str += "{\"itemid\":\"" + dt.Rows[i]["itemid"].ToString() + "\",\"pic_url\":\"" + dt.Rows[i]["picurl"].ToString() + "\",\"name\":\"" + dt.Rows[i]["itemname"].ToString() + "\",\"detail_url\":\"" + dt.Rows[i]["linkurl"].ToString() + "\"}";
            }
            str += "],\"pagenow\":" + page + ",\"total\":" + totalPageCount + "}";
        }
        else
        {
            str = "{\"count\":\"0\"}";
        }

        Response.Write(str);
    }

    private void ShowListInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;

        int pageNow = 1;
        if (page == "")
        {
            pageNow = 1;
        }
        else
        {
            pageNow = int.Parse(page);
        }

        int pageSizeNow = 20;
        if (pagesize == "")
        {
            pageSizeNow = 20;
        }
        else
        {
            pageSizeNow = int.Parse(pagesize);
        }
        int pageCount = pageSizeNow;
        int dataCount = (pageNow - 1) * pageCount;


        sql = "SELECT COUNT(*) FROM TeteShopItem WHERE nick = '" + uid + "' AND CHARINDEX('" + cid + "', cateid) > 0";
        int totalCount = int.Parse(utils.ExecuteString(sql));
        int totalPageCount = 1;

        if (totalCount % pageCount == 0)
        {
            totalPageCount = totalCount / pageCount;
        }
        else
        {
            totalPageCount = totalCount / pageCount + 1;
        }

        sql = "SELECT TOP " + pageCount.ToString() + " * FROM (SELECT *,ROW_NUMBER() OVER (ORDER BY id DESC) AS rownumber FROM TeteShopItem WHERE nick = '" + uid + "' AND CHARINDEX('" + cid + "', cateid) > 0) AS a WHERE a.rownumber > " + dataCount.ToString() + " ORDER BY id DESC";
        //Response.Write(sql);
        //sql = "SELECT * FROM TeteShopItem WHERE nick = '" + uid + "' AND CHARINDEX('" + cid + "', cateid) > 0";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str = "{\"item\":[";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i != 0)
                {
                    str += ",";
                }

                str += "{\"itemid\":\"" + dt.Rows[i]["itemid"].ToString() + "\",\"pic_url\":\"" + dt.Rows[i]["picurl"].ToString() + "\",\"name\":\"" + dt.Rows[i]["itemname"].ToString() + "\",\"detail_url\":\"" + dt.Rows[i]["linkurl"].ToString() + "\"}";
            }
            str += "],\"pagenow\":" + page + ",\"total\":" + totalPageCount + "}";
        }
        else
        {
            str = "{\"count\":\"0\"}";
        }

        Response.Write(str);
    }

    private void ShowCateInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;

        sql = "SELECT * FROM TeteShopCategory WHERE nick = '" + uid + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str = "{\"cate\":[";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i != 0)
                {
                    str += ",";
                }

                str += "{\"cid\":\"" + dt.Rows[i]["cateid"].ToString() + "\",\"parent_cid\":\"" + dt.Rows[i]["parentid"].ToString() + "\",\"name\":\"" + dt.Rows[i]["catename"].ToString() + "\",\"count\":\"" + dt.Rows[i]["catecount"].ToString() + "\",\"catepicurl\":\"" + dt.Rows[i]["catepicurl"].ToString() + "\"}";
            }
            str += "]}";
        }
        else
        {
            str = "{\"count\":\"0\"}";
        }

        Response.Write(str);
    }

    /// <summary>
    /// 将客户设置的广告图片地址发送给客户端
    /// </summary>
    private void ShowAdsInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;

        sql = "SELECT logo,ads FROM TeteShop WHERE nick = '" + uid + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str = "{\"logo\":\"" + dt.Rows[0][0].ToString() + "\",\"adsurl\":\"" + dt.Rows[0][1].ToString() + "\"}";
        }
        else
        {
            str = "{\"logo\":\"\",\"adsurl\":\"\"}";
        }

        Response.Write(str);
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