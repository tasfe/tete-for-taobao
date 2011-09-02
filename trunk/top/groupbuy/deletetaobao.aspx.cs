using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;
using Taobao.Top.Api;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;

public partial class top_groupbuy_deletetaobao : System.Web.UI.Page
{
    public string id = string.Empty;
    public string ty = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        id = utils.NewRequest("id", Common.utils.RequestType.QueryString);
        ty = utils.NewRequest("type", Common.utils.RequestType.QueryString);

        if (id != "" && !utils.IsInt32(id))
        {
            Response.Write("非法参数1");
            Response.End();
            return;
        }
        if (ty == "dle")
        {
            DeleteTaobao(id);
        }


        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        string session = cookie.getCookie("top_sessiongroupbuy");

        //COOKIE过期判断
        if (taobaoNick == "")
        {
            //SESSION超期 跳转到登录页
            Response.Write("<script>parent.location.href='http://container.open.taobao.com/container?appkey=12287381'</script>");
            Response.End();
        }

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        string sql = "SELECT COUNT(*) FROM TopMission WHERE groupbuyid = " + id + " AND typ='delete' AND isok = 0";
        string count = utils.ExecuteString(sql);

        if (count != "0")
        {
            Response.Write("<script>alert('创建任务失败，有同类型的任务正在执行中，请等待其完成后再创建新的任务！');window.location.href='missionlist.aspx';</script>");
            Response.End();
            return;
        }

        sql = "INSERT INTO TopMission (typ, nick, groupbuyid) VALUES ('delete', '" + taobaoNick + "', '" + id + "')";
        utils.ExecuteNonQuery(sql);

        sql = "SELECT TOP 1 ID FROM TopMission ORDER BY ID DESC";
        string missionid = utils.ExecuteString(sql);

        //获取团购信息并更新
        sql = "SELECT name,productimg,productid FROM TopGroupBuy WHERE id = '" + id + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            sql = "UPDATE TopMission SET groupbuyname = '" + dt.Rows[0]["name"].ToString() + "',groupbuypic = '" + dt.Rows[0]["productimg"].ToString() + "',itemid = '" + dt.Rows[0]["productid"].ToString() + "' WHERE id = " + missionid;
            utils.ExecuteNonQuery(sql);
        }

        //更新任务总数
        sql = "SELECT COUNT(*) FROM TopWriteContent WHERE groupbuyid = '" + id + "' AND isok = 1";
        count = utils.ExecuteString(sql);
        sql = "UPDATE TopMission SET total = '" + count + "' WHERE id = " + missionid;
        utils.ExecuteNonQuery(sql);

        Response.Redirect("missionlist.aspx");
    }



    /// <summary>
    /// 清除代码
    /// </summary>
    /// <param name="id"></param>
    public void clearGroupbuy(string id)
    {
        try
        {
            string appkey = "12287381";
            string secret = "d3486dac8198ef01000e7bd4504601a4"; 
            string sql = "SELECT * FROM TopGroupBuy WHERE ID=" + id;
            string session = string.Empty;
            #region 取消活动 删除该活动关联的用户群 将该团购标志为已结束
            //WriteLog(sql, "");
            DataTable enddt = utils.ExecuteDataTable(sql);
            //通过接口将该用户加入人群
            for (int y = 0; y < enddt.Rows.Count; y++)
            {
                sql = "SELECT session FROM TopTaobaoShop WHERE nick = '" + enddt.Rows[y]["nick"].ToString() + "'";

                DataTable dtnick = utils.ExecuteDataTable(sql);
                if (dtnick.Rows.Count != 0)
                {
                    session = utils.ExecuteDataTable(sql).Rows[0][0].ToString();
                }
                //取消该活动
                IDictionary<string, string> paramnew = new Dictionary<string, string>();
                paramnew.Add("promotion_id", enddt.Rows[y]["promotionid"].ToString());
                string resultnew = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.marketing.promotion.delete", session, paramnew);


            }
            #endregion
        }
        catch { }

           
    }

    /// <summary>
    /// 记录该任务的详细关联商品
    /// </summary>
    private void RecordMissionDetail(string groupbuyid, string missionid, string itemid, string html)
    {
        string sql = "INSERT INTO TopWriteContent (groupbuyid, missionid, itemid, html,isok) VALUES ('" + groupbuyid + "', '" + missionid + "', '" + itemid + "', '" + html + "',1)";
        utils.ExecuteNonQuery(sql);
    }


    /// <summary>
    /// 删除团购
    /// </summary>
    /// <param name="gid">团购ID</param>
    private void DeleteTaobao(string gid)
    {
        //获取正在进行中的宝贝同步任务        
        string appkey = "12287381";
        string secret = "d3486dac8198ef01000e7bd4504601a4";
        string session = string.Empty;
        string id = string.Empty;
        string missionid = string.Empty;
        string html = string.Empty;
        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", appkey, secret);

        string sql = "SELECT t.*, s.sessiongroupbuy FROM TopMission t INNER JOIN TopTaobaoShop s ON s.nick = t.nick WHERE groupbuyid=" + gid;

        DataTable dt = utils.ExecuteDataTable(sql); 
        for (int i = 0; i < dt.Rows.Count; i++)
        {

            id = dt.Rows[i]["groupbuyid"].ToString();
            clearGroupbuy(id);
            session = dt.Rows[i]["sessiongroupbuy"].ToString();
            missionid = dt.Rows[i]["id"].ToString();
            html = "";
           
            sql = "delete from TopWriteContent where groupbuyid =  '" + dt.Rows[i]["groupbuyid"].ToString() + "'";
            utils.ExecuteNonQuery(sql);
 
                for (int j = 1; j <= 500; j++)
                {
                    ItemsOnsaleGetRequest request = new ItemsOnsaleGetRequest();
                    request.Fields = "num_iid,title,price,pic_url";
                    request.PageSize = 200;
                    request.PageNo = j;

                    Common.Cookie cookie = new Common.Cookie();
                    string taobaoNick = dt.Rows[i]["nick"].ToString();
                    try
                    {
                        PageList<Item> product = client.ItemsOnsaleGet(request, session);
                        for (int num = 0; num < product.Content.Count; num++)
                        {
                            RecordMissionDetail(id, missionid, product.Content[num].NumIid.ToString(), html);
                        }
                        if (product.Content.Count < 200)
                        {
                            break;
                        }
                    }
                    catch (Exception e)
                    { 
                        sql = "UPDATE TopMission SET fail = fail + 1,isok = -1 WHERE id = " + dt.Rows[i]["id"].ToString();
                        utils.ExecuteNonQuery(sql);
                        break;
                    }
                }
 
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