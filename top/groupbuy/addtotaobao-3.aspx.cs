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
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using System.IO;
using System.Text.RegularExpressions;
using System.Text; 
using System.Security.Cryptography;

public partial class top_addtotaobao_3 : System.Web.UI.Page
{
    public static string logUrl = "D:/svngroupbuy/website/ErrLog";
    public string style = string.Empty;
    public string size = string.Empty;
    public string type = string.Empty;
    public string orderby = string.Empty;
    public string query = string.Empty;
    public string shopcat = string.Empty;
    public string name = string.Empty;
    public string items = string.Empty;

    public string nickid = string.Empty;
    public string nickidEncode = string.Empty;
    public string md5nick = string.Empty;
    public string tabletitle = string.Empty;

    public string width = string.Empty;
    public string height = string.Empty;

    public string id = string.Empty;
    public string ads = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        //try
        //{
            id = utils.NewRequest("id", Common.utils.RequestType.QueryString);

            if (id != "" && !utils.IsInt32(id))
            {
                Response.Write("非法参数1");
                Response.End();
                return;
            }

            style = utils.NewRequest("style", Common.utils.RequestType.Form);
            size = utils.NewRequest("size", Common.utils.RequestType.Form);
            type = utils.NewRequest("type", Common.utils.RequestType.Form);
            orderby = utils.NewRequest("orderby", Common.utils.RequestType.Form);
            query = utils.NewRequest("query", Common.utils.RequestType.Form);
            shopcat = utils.NewRequest("shopcat", Common.utils.RequestType.Form);
            name = utils.NewRequest("name", Common.utils.RequestType.Form);
            items = utils.NewRequest("items", Common.utils.RequestType.Form);
            ads = utils.NewRequest("ads", Common.utils.RequestType.Form);

            string missionid = string.Empty;
            string html = CreateGroupbuyHtml(id);
            if (html.Length == 0) 
            {
                return;
            }

            int itemcount = 0;

            string act = utils.NewRequest("act", Common.utils.RequestType.Form);
            if (act == "save" && NoRepeat(id, type))
            {
                //记录该任务
                missionid = RecordMission();

                TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", "12287381", "d3486dac8198ef01000e7bd4504601a4");
                //提交更新到淘宝商品上去
                if (type != "1")
                {
                    for (int j = 1; j <= 500; j++)
                    {
                        ItemsOnsaleGetRequest request = new ItemsOnsaleGetRequest();
                        request.Fields = "num_iid,title,price,pic_url";
                        request.PageSize = 200;
                        request.PageNo = j;

                        if (orderby == "new")
                        {
                            request.OrderBy = "list_time:desc";
                        }
                        else if (orderby == "sale")
                        {
                            request.OrderBy = "volume:desc";
                        }

                        if (shopcat != "0")
                        {
                            request.SellerCids = shopcat;
                        }

                        if (query != "0")
                        {
                            request.Q = query;
                        }

                        Cookie cookie = new Cookie();
                        string taobaoNick = cookie.getCookie("nick");
                        string session = cookie.getCookie("top_sessiongroupbuy");
                        PageList<Item> product = client.ItemsOnsaleGet(request, session);

                        for (int i = 0; i < product.Content.Count; i++)
                        {
                            RecordMissionDetail(id, missionid, product.Content[i].NumIid.ToString(), html);
                            itemcount++;
                        }

                        if (product.Content.Count < 200)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    string[] itemId = items.Split(',');

                    for (int i = 0; i < itemId.Length; i++)
                    {
                        RecordMissionDetail(id, missionid, itemId[i], html);
                        itemcount++;
                    }
                }


            }
        //}
        //catch { }


            //更新总数量
            string sql = "UPDATE TopMission SET total = '" + itemcount + "' WHERE id = " + missionid;
            utils.ExecuteNonQuery(sql);

            missID.Value = missionid;
 
            //更新任务
            DoMyJob(missionid);
            //Response.Redirect("missionlist.aspx");
    }

    //判断是否有同类型任务进行中
    private bool NoRepeat(string id, string type)
    {
        string sql = "SELECT COUNT(*) FROM TopMission WHERE groupbuyid = " + id + " AND typ='write' AND isok = 0";
        string count = utils.ExecuteString(sql);

        if (count != "0")
        {
            Response.Write("<script>alert('创建任务失败，有同类型的任务正在执行中，请等待其完成后再创建新的任务！');window.location.href='missionlist.aspx';</script>");
            Response.End();
            return false;
        }

        return true;
    }

    private string CreateGroupbuyHtml(string id)
    {
        string str = string.Empty;
        string html = File.ReadAllText(Server.MapPath("tpl/style1.html"));
        string sql = "SELECT * FROM TopGroupBuy WHERE id = '" + id + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str = html;
            str = str.Replace("{name}", dt.Rows[0]["name"].ToString());
            str = str.Replace("{oldprice}", dt.Rows[0]["productprice"].ToString());
            str = str.Replace("{zhekou}", Math.Round((decimal.Parse(dt.Rows[0]["productprice"].ToString()) - decimal.Parse(dt.Rows[0]["zhekou"].ToString())) / decimal.Parse(dt.Rows[0]["productprice"].ToString()) * 10, 1).ToString());
            str = str.Replace("{leftprice}", (decimal.Parse(dt.Rows[0]["productprice"].ToString()) - decimal.Parse(dt.Rows[0]["zhekou"].ToString())).ToString().Split('.')[0]);
            str = str.Replace("{rightprice}", (decimal.Parse(dt.Rows[0]["productprice"].ToString()) - decimal.Parse(dt.Rows[0]["zhekou"].ToString())).ToString().Split('.')[1]);
            str = str.Replace("{newprice}", dt.Rows[0]["zhekou"].ToString());
            str = str.Replace("{buycount}", dt.Rows[0]["buycount"].ToString());
            str = str.Replace("{producturl}", dt.Rows[0]["producturl"].ToString());
            str = str.Replace("{productimg}", dt.Rows[0]["productimg"].ToString());
            str = str.Replace("{id}", id);
            str = str.Replace("'", "''");
        }
        return str;
    }

    /// <summary>
    /// 记录该任务的详细关联商品
    /// </summary>
    private void RecordMissionDetail(string groupbuyid, string missionid, string itemid, string html)
    {
       
        string sql = "INSERT INTO TopWriteContent (groupbuyid, missionid, itemid, html) VALUES ('" + groupbuyid + "', '" + missionid + "', '" + itemid + "', '')";
        utils.ExecuteNonQuery(sql);

        return;
    }

    /// <summary>
    /// 记录该任务并返回任务ID
    /// </summary>
    /// <returns></returns>
    private string RecordMission()
    {
        Cookie cookie = new Cookie();
        string missionid = string.Empty;
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

        string sql = "INSERT INTO TopMission (typ, nick, groupbuyid) VALUES ('write', '" + taobaoNick + "', '" + id + "')";
        utils.ExecuteNonQuery(sql);

        sql = "SELECT TOP 1 ID FROM TopMission ORDER BY ID DESC";
        missionid = utils.ExecuteString(sql);

        //获取团购信息并更新
        sql = "SELECT name,productimg,productid FROM TopGroupBuy WHERE id = '" + id + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            sql = "UPDATE TopMission SET groupbuyname = '" + dt.Rows[0]["name"].ToString() + "',groupbuypic = '" + dt.Rows[0]["productimg"].ToString() + "',itemid = '" + dt.Rows[0]["productid"].ToString() + "' WHERE id = " + missionid;
            utils.ExecuteNonQuery(sql);
        }

        return missionid;
    }

    private string EncodeStr(string[] parmArray)
    {
        string newStr = string.Empty;
        for (int i = 0; i < parmArray.Length; i++)
        {
            if (i == 0)
            {
                newStr = parmArray[i];
            }
            else
            {
                newStr += "|" + parmArray[i];
            }
        }

        Rijndael_ encode = new Rijndael_("tetesoftstr");
        newStr = encode.Encrypt(newStr);
        newStr = HttpUtility.UrlEncode(newStr);
        return newStr;
    }

    /// <summary> 
    /// MD5 加密函数 
    /// </summary> 
    /// <param name="str"></param> 
    /// <param name="code"></param> 
    /// <returns></returns> 
    public static string MD5(string str)
    {
        return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").Substring(8, 16);
    }


    private void DoMyJob(string topMissionID)
    {

        //获取正在进行中的宝贝同步任务        
        string appkey = "12287381";
        string secret = "d3486dac8198ef01000e7bd4504601a4";
        string session = string.Empty;
        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", appkey, secret);

        string sql = "SELECT t.*, s.sessiongroupbuy FROM TopMission t INNER JOIN TopTaobaoShop s ON s.nick = t.nick WHERE t.isok = 0 AND t.typ = 'write' AND t.id="+topMissionID+"  ORDER BY t.id ASC";

        DataTable dt = utils.ExecuteDataTable(sql);
        DataTable dtWrite = null;
        string styleHtml = string.Empty;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            session = dt.Rows[i]["sessiongroupbuy"].ToString();
            sql = "SELECT * FROM TopWriteContent WHERE missionid = '" + dt.Rows[i]["id"].ToString() + "' AND isok = 0";

            //WriteLog("sql1:" + sql, "");
            dtWrite = utils.ExecuteDataTable(sql);
            for (int j = 0; j < dtWrite.Rows.Count; j++)
            {
                styleHtml = CreateGroupbuyHtml(dtWrite.Rows[j]["groupbuyid"].ToString());
                try
                {
                    //获取原宝贝描述
                    ItemGetRequest requestItem = new ItemGetRequest();
                    requestItem.Fields = "desc";
                    requestItem.NumIid = long.Parse(dtWrite.Rows[j]["itemid"].ToString());
                    Item product = client.ItemGet(requestItem, session);
                    string newContent = string.Empty;
                    string groupid = dtWrite.Rows[j]["groupbuyid"].ToString();

                    //WriteLog("html:" + styleHtml.Length.ToString(), "");
                    if (!Regex.IsMatch(product.Desc, @"<div><a name=""tetesoft-area-start-" + groupid + @"""></a></div>([\s\S]*)<div><a name=""tetesoft-area-end-" + groupid + @"""></a></div>"))
                    {
                        newContent = @"<div><a name=""tetesoft-area-start-" + groupid + @"""></a></div>" + styleHtml + @"<div><a name=""tetesoft-area-end-" + groupid + @"""></a></div>" + product.Desc;
                    }
                    else
                    {
                        newContent = Regex.Replace(product.Desc, @"<div><a name=""tetesoft-area-start-" + groupid + @"""></a></div>([\s\S]*)<div><a name=""tetesoft-area-end-" + groupid + @"""></a></div>", @"<div><a name=""tetesoft-area-start-" + groupid + @"""></a></div>" + styleHtml + @"<div><a name=""tetesoft-area-end-" + groupid + @"""></a></div>");
                    }
                    //WriteLog("html2:" + newContent.Length.ToString(), "");


                    //更新宝贝描述
                    IDictionary<string, string> param = new Dictionary<string, string>();
                    param.Add("num_iid", dtWrite.Rows[j]["itemid"].ToString());
                    param.Add("desc", newContent);
                    string resultpro = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.item.update ", session, param);

                    if (resultpro.IndexOf("ITEM_PROPERTIES_ERROR") != -1)
                    {
                        WriteLog("更新宝贝描述：宝贝ID：" + dtWrite.Rows[j]["itemid"].ToString() + "返回的错误信息" + resultpro, "", dt.Rows[i]["nick"].ToString());
                    }
                    else
                    {
                        WriteLog("itemid:" + dtWrite.Rows[j]["itemid"].ToString() + resultpro, "", dt.Rows[i]["nick"].ToString());
                    }
                    //更新状态
                    sql = "UPDATE TopWriteContent SET isok = 1 WHERE id = " + dtWrite.Rows[j]["id"].ToString();
                     utils.ExecuteNonQuery(sql);

                    //更新状态
                    sql = "UPDATE TopMission SET success = success + 1 WHERE id = " + dt.Rows[i]["id"].ToString();
                    utils.ExecuteNonQuery(sql);
                }
                catch (Exception e)
                {
                    WriteLog(e.Message, "1", dt.Rows[i]["nick"].ToString());
                    WriteLog(e.StackTrace, "1", dt.Rows[i]["nick"].ToString());
                    sql = "UPDATE TopMission SET fail = fail + 1,isok = -1  WHERE id = " + dt.Rows[i]["id"].ToString();
                     utils.ExecuteNonQuery(sql);
                    continue;
                }
            }

            dtWrite.Dispose();

            sql = "UPDATE TopMission SET isok = 1 WHERE id = " + dt.Rows[i]["id"].ToString();
             utils.ExecuteNonQuery(sql);
        }

        dt.Dispose();
  

    }








    /// <summary>
    /// 写日志
    /// </summary>
    /// <param name="value">日志内容</param>
    /// <param name="type">类型 0(成功日志),1(错误日志) 可传空文本默认为0</param>
    /// <returns></returns>
    public static void WriteLog(string message, string type,string nick)
    {
 
        string tempStr = logUrl + "/Groupby"+nick + DateTime.Now.ToString("yyyyMMdd");//文件夹路径
        string tempFile = tempStr + "/Groupbypromotion" + nick + DateTime.Now.ToString("yyyyMMdd") + ".txt";
        if (type == "1")
        {
            tempFile = tempStr + "/GroupbypromotionErr" + nick + DateTime.Now.ToString("yyyyMMdd") + ".txt";
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
        MD5 md5 =System.Security.Cryptography.MD5.Create();
        
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
        System.Net.HttpWebRequest req = ( System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
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
