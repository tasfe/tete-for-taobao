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
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Security.Cryptography;

public partial class top_groupbuy_LoadAjax2 : System.Web.UI.Page
{
    public static string logUrl = "D:/svngroupbuy/website/ErrLog";
    protected void Page_Load(object sender, EventArgs e)
    {
 
        if (Request.QueryString["sqltype"] != null)
        {
            DoMyJob(Request.QueryString["sqltype"].ToString());
            Response.Write("ok");
            Response.End();
        }
    }


    private void DoMyJob(string topMissionID)
    {

        //获取正在进行中的宝贝同步任务        
        string appkey = "12287381";
        string secret = "d3486dac8198ef01000e7bd4504601a4";
        string session = string.Empty;
        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", appkey, secret);

        string sql = "SELECT t.*, s.sessiongroupbuy FROM TopMission t INNER JOIN TopTaobaoShop s ON s.nick = t.nick WHERE t.isok = 0 AND t.typ = 'write' AND t.id=" + topMissionID + "  ORDER BY t.id ASC";

        DataTable dt = utils.ExecuteDataTable(sql);
        DataTable dtWrite = null;
        string styleHtml = string.Empty;
        if (dt != null)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                session = dt.Rows[i]["sessiongroupbuy"].ToString();
                sql = "SELECT * FROM TopWriteContent WHERE missionid = '" + dt.Rows[i]["id"].ToString() + "' AND isok = 0";

                WriteLog("sql1:" + sql, "");
                dtWrite = utils.ExecuteDataTable(sql);
                if (dtWrite != null)
                {
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

                            WriteLog("html:" + styleHtml.Length.ToString(), "");
                            if (!Regex.IsMatch(product.Desc, @"<div><a name=""tetesoft-area-start-" + groupid + @"""></a></div>([\s\S]*)<div><a name=""tetesoft-area-end-" + groupid + @"""></a></div>"))
                            {
                                newContent = @"<div><a name=""tetesoft-area-start-" + groupid + @"""></a></div>" + styleHtml + @"<div><a name=""tetesoft-area-end-" + groupid + @"""></a></div>" + product.Desc;
                            }
                            else
                            {
                                newContent = Regex.Replace(product.Desc, @"<div><a name=""tetesoft-area-start-" + groupid + @"""></a></div>([\s\S]*)<div><a name=""tetesoft-area-end-" + groupid + @"""></a></div>", @"<div><a name=""tetesoft-area-start-" + groupid + @"""></a></div>" + styleHtml + @"<div><a name=""tetesoft-area-end-" + groupid + @"""></a></div>");
                            }
                            WriteLog("html2:" + newContent.Length.ToString(), "");


                            //更新宝贝描述
                            IDictionary<string, string> param = new Dictionary<string, string>();
                            param.Add("num_iid", dtWrite.Rows[j]["itemid"].ToString());
                            param.Add("desc", newContent);
                            string resultpro = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.item.update ", session, param);

                            if (resultpro.IndexOf("ITEM_PROPERTIES_ERROR") != -1)
                            {
                                //插入宝贝错误日志
                                //sql = "insert TopMissionErrDetail (TopMissionID,itemid,nick,ErrDetail) values('" + dt.Rows[i]["id"].ToString() + "','" + dtWrite.Rows[j]["itemid"].ToString() + "','" + dt.Rows[i]["nick"].ToString() + "','" + resultpro + "')";
                                utils.ExecuteNonQuery(sql);
                                //更新宝贝错误数
                                sql = "UPDATE TopMission SET fail = fail + 1,isok = -1  WHERE id = " + dt.Rows[i]["id"].ToString();
                                utils.ExecuteNonQuery(sql);
                            }
                            else
                            {

                                //更新状态
                                sql = "UPDATE TopWriteContent SET isok = 1 WHERE id = " + dtWrite.Rows[j]["id"].ToString();
                                utils.ExecuteNonQuery(sql);

                                //更新状态
                                sql = "UPDATE TopMission SET success = success + 1 WHERE id = " + dt.Rows[i]["id"].ToString();
                                utils.ExecuteNonQuery(sql);
                            }
                        }
                        catch (Exception e)
                        {
                            WriteLog(e.Message, "1");
                            WriteLog(e.StackTrace, "1");
                            sql = "UPDATE TopMission SET fail = fail + 1,isok = -1  WHERE id = " + dt.Rows[i]["id"].ToString();
                            utils.ExecuteNonQuery(sql);
                            continue;
                        }
                    }
                }

                dtWrite.Dispose();

                sql = "UPDATE TopMission SET isok = 1 WHERE id = " + dt.Rows[i]["id"].ToString();
                utils.ExecuteNonQuery(sql);
            }

            dt.Dispose();
        }

    }








    /// <summary>
    /// 写日志
    /// </summary>
    /// <param name="value">日志内容</param>
    /// <param name="type">类型 0(成功日志),1(错误日志) 可传空文本默认为0</param>
    /// <returns></returns>
    public static void WriteLog(string message, string type)
    {
        string tempStr = logUrl + "/Groupby" + DateTime.Now.ToString("yyyyMMdd");//文件夹路径
        string tempFile = tempStr + "/Groupbypromotion" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
        if (type == "1")
        {
            tempFile = tempStr + "/GroupbypromotionErr" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
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
                //postData.Append(Uri.EscapeDataString(value));
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
}