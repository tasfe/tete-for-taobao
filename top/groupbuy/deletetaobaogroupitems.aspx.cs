﻿using System;
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

public partial class top_groupbuy_deletetaobaogroupitems : System.Web.UI.Page
{
    public string id = string.Empty;
    public string ty = string.Empty;

    public string logUrl = "D:/groupbuy.7fshop.com/wwwroot/top/groupbuy/ErrLog";
    protected void Page_Load(object sender, EventArgs e)
    {
        id = utils.NewRequest("id", Common.utils.RequestType.QueryString);
        ty = utils.NewRequest("act", Common.utils.RequestType.QueryString);
    
        if (id != "" && !utils.IsInt32(id))
        {
            Response.Write("非法参数1");
            Response.End();
            return;
        }
        //老版清除团购

        if (ty == "del")
        {
            DeleteTaobaolist(id);

            Common.Cookie cookie = new Common.Cookie();
            string taobaoNick = cookie.getCookie("nick");
            string session = cookie.getCookie("top_sessiongroupbuy");

            //COOKIE过期判断
            if (taobaoNick == "")
            {
                //SESSION超期 跳转到登录页
                Response.Write("<a href='http://container.open.taobao.com/container?appkey=12287381'> 请重新授权</a></script>");
                Response.End();
            }

            Rijndael_ encode = new Rijndael_("tetesoft");
            taobaoNick = encode.Decrypt(taobaoNick);

            string sql = "SELECT COUNT(*) FROM TopMission WHERE groupbuyid = " + id + " AND typ='delete' AND isok = 0 AND nick<>''";
            string count = utils.ExecuteString(sql);

            if (count != "0")
            {
                //删除宝贝描述
                DeleteTaobao(taobaoNick);
                Response.Write("true");
                Response.End();
            }


            if (taobaoNick.Trim() == "")
            {
                //SESSION超期 跳转到登录页
                Response.Write("<a href='http://container.open.taobao.com/container?appkey=12287381'> 请重新授权</a></script>");
                Response.End();
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
            //删除宝贝描述
            DeleteTaobao(taobaoNick);
            Response.Write("true");
            Response.End();
        }

        if (ty == "delitem")
        {

            Common.Cookie cookie = new Common.Cookie();
            string taobaoNick = cookie.getCookie("nick");
            string session = cookie.getCookie("top_sessiongroupbuy");

            //COOKIE过期判断
            if (taobaoNick == "")
            {
                //SESSION超期 跳转到登录页
                Response.Write("<a href='http://container.open.taobao.com/container?appkey=12287381'> 请重新授权</a></script>");
                Response.End();
            }

            Rijndael_ encode = new Rijndael_("tetesoft");
            taobaoNick = encode.Decrypt(taobaoNick);

            string sql = "SELECT COUNT(*) FROM TopMission WHERE groupbuyid = " + id + " AND typ='delete' AND isok = 0 AND nick<>''";
            string count = utils.ExecuteString(sql);

            if (count != "0")
            {
                //删除宝贝描述
                DeleteTaobao(taobaoNick);
                Response.Write("true");
                Response.End();
            }


            if (taobaoNick.Trim() == "")
            {
                //SESSION超期 跳转到登录页
                Response.Write("<a href='http://container.open.taobao.com/container?appkey=12287381'> 请重新授权</a></script>");
                Response.End();
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
            //删除宝贝描述
            DeleteTaobao(taobaoNick);
            Response.Write("true");
            Response.End();
        }
    }

    /// <summary>
    /// 删除团购任务列表
    /// </summary>
    /// <param name="gid">团购ID</param>
    private void DeleteTaobaolist(string gid)
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

            session = dt.Rows[i]["sessiongroupbuy"].ToString();
            missionid = dt.Rows[i]["id"].ToString();
            html = "";

            sql = "delete from TopWriteContent where groupbuyid =  '" + dt.Rows[i]["groupbuyid"].ToString() + "'";
            utils.ExecuteNonQuery(sql);
            if (dt.Rows[i]["itemid"] != null && dt.Rows[i]["itemid"].ToString() != "" && dt.Rows[i]["itemid"].ToString() != "NULL")
            {
                RecordMissionDetail(id, missionid, dt.Rows[i]["itemid"].ToString(), "");
            }
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

    private void DeleteTaobao(string nick2)
    {
        //try
        //{
        //获取正在进行中的宝贝同步任务        
        string appkey = "12287381";
        string secret = "d3486dac8198ef01000e7bd4504601a4";
        string session = string.Empty;
        string id = string.Empty;
        string missionid = string.Empty;
        string html = string.Empty;
        string shopcat = "0";
        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", appkey, secret);
         
        string sql = "SELECT  top 300  t.*, s.sessiongroupbuy,s.sid FROM TopMission t INNER JOIN TopTaobaoShop s ON s.nick = t.nick WHERE t.isok = 0 AND t.typ = 'delete' and  s.nick='" + nick2 + "' ORDER BY t.id ASC";

        DataTable dt = utils.ExecuteDataTable(sql);
        DataTable dtWrite = null;
        for (int i = 0; i < dt.Rows.Count; i++)
        {

            id = dt.Rows[i]["groupbuyid"].ToString();
            session = dt.Rows[i]["sessiongroupbuy"].ToString();
            missionid = dt.Rows[i]["id"].ToString();
            html = "";

            sql = "SELECT DISTINCT itemid FROM TopWriteContent WHERE groupbuyid = '" + dt.Rows[i]["groupbuyid"].ToString() + "' AND isok = 1";
            dtWrite = utils.ExecuteDataTable(sql);
            if (dtWrite == null || dtWrite.Rows.Count < 1)
            {
                for (int j = 1; j <= 500; j++)
                {
                    ItemsOnsaleGetRequest request = new ItemsOnsaleGetRequest();
                    request.Fields = "num_iid,title,price,pic_url";
                    request.PageSize = 200;
                    request.PageNo = j;
                     
                    string taobaoNick = dt.Rows[i]["nick"].ToString();
                    try
                    {
                        PageList<Item> product = client.ItemsOnsaleGet(request, session);

                        WriteLog(taobaoNick + "INGCount：" + product.Content.Count.ToString(), "1", nick2, "");
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
                        WriteLog(e.StackTrace, "1", nick2, "");
                        WriteLog(e.Message, "1", nick2, "");
                        sql = "UPDATE TopMission SET fail = fail + 1,isok = -1 WHERE id = " + dt.Rows[i]["id"].ToString();
                        utils.ExecuteNonQuery(sql);
                        break;
                    }
                }

                sql = "SELECT DISTINCT itemid FROM TopWriteContent WHERE groupbuyid = '" + dt.Rows[i]["groupbuyid"].ToString() + "' AND isok = 1";
                dtWrite = utils.ExecuteDataTable(sql);
            }
            WriteLog("ING：" + dtWrite.Rows.Count.ToString(), "1", nick2, "");
            for (int j = 0; j < dtWrite.Rows.Count; j++)
            {
                try
                {
                    //获取原宝贝描述
                    ItemGetRequest requestItem = new ItemGetRequest();
                    requestItem.Fields = "desc";
                    requestItem.NumIid = long.Parse(dtWrite.Rows[j]["itemid"].ToString());
                    WriteLog("更新删除准备中", "1", nick2, "");
                    Item product = client.ItemGet(requestItem, session);
                    WriteLog("更新删除准备中。。。。", "1", nick2, "");
                    string newContent2 = string.Empty;
                    string groupid = dt.Rows[i]["groupbuyid"].ToString();
                    string tetegroupbuyGuid = groupid;
                    string sqltemp = "SELECT * FROM TopGroupBuy WHERE id = '" + groupid + "'";
                    DataTable dttemp = utils.ExecuteDataTable(sqltemp);
                    if (dttemp == null)
                    {
                        continue;

                    }
                    //判断团购是多模板
                    if (dttemp.Rows[0]["groupbuyGuid"].ToString() != "")
                    {
                        tetegroupbuyGuid = dttemp.Rows[0]["groupbuyGuid"].ToString();
                    }

                    if (!Regex.IsMatch(product.Desc, @"<div>[\s]*<a name=""tetesoft-area-start-" + tetegroupbuyGuid + @""">[\s]*</a>[\s]*</div>[\s]*([\s\S]*)<div>[\s]*<a name=""tetesoft-area-end-" + tetegroupbuyGuid + @""">[\s]*</a>[\s]*</div>"))
                    {
                        //更新状态

                        WriteLog(DateTime.Now.ToString() + "http://item.taobao.com/item.htm?id=" + dtWrite.Rows[j]["itemid"].ToString() + " 不含需要清除的代码", "", nick2, "");
                        sql = "UPDATE TopMission SET success = success + 1,isok = 1 WHERE id = " + dt.Rows[i]["id"].ToString();
                        utils.ExecuteNonQuery(sql);
                        continue;
                    }
                    else
                    {

                        newContent2 = Regex.Replace(product.Desc, @"<div>[\s]*<a name=""tetesoft-area-start-" + tetegroupbuyGuid + @""">[\s]*</a>[\s]*</div>[\s]*([\s\S]*)<div>[\s]*<a name=""tetesoft-area-end-" + tetegroupbuyGuid + @""">[\s]*</a>[\s]*</div>", @"");

                    }

                    //更新宝贝描述
                    IDictionary<string, string> param = new Dictionary<string, string>();
                    param.Add("num_iid", dtWrite.Rows[j]["itemid"].ToString());
                    param.Add("desc", newContent2);
                    string resultpro = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.item.update", session, param);


                    //插入宝贝错误日志
                    if (resultpro.ToLower().IndexOf("ITEM_PROPERTIES_ERROR") != -1)
                    {

                        WriteLog("删除活动更新宝贝描述：宝贝ID：" + dtWrite.Rows[j]["itemid"].ToString() + "返回的错误信息" + resultpro.Replace("<?xml version=\"1.0\" encoding=\"utf-8\" ?>", ""), "1", nick2, "");
                    }
                    else
                    {
                        WriteLog("删除活动更新宝贝描述：宝贝ID：" + dtWrite.Rows[j]["itemid"].ToString() + "返回的成功信息" + resultpro.Replace("<?xml version=\"1.0\" encoding=\"utf-8\" ?>", ""), "", nick2, "");
                    }

                    if (resultpro.IndexOf("ITEM_PROPERTIES_ERROR") != -1)
                    {
                        WriteLog("删除宝贝更新宝贝描述结果：宝贝ID：" + dtWrite.Rows[j]["itemid"].ToString() + "返回的错误信息" + resultpro, "", nick2, "");

                        //更新宝贝错误数
                        sql = "UPDATE TopMission SET fail = fail + 1,isok = -1  WHERE id = " + dt.Rows[i]["id"].ToString();
                        utils.ExecuteNonQuery(sql);
                    }
                    else
                    {

                        //更新状态
                        sql = "UPDATE TopWriteContent SET isok = 1 WHERE id = " + dtWrite.Rows[j]["itemid"].ToString();
                        utils.ExecuteNonQuery(sql);


                        //更新状态
                        sql = "UPDATE TopMission SET success = success + 1,isok = 1 WHERE id = " + dt.Rows[i]["id"].ToString();
                        utils.ExecuteNonQuery(sql);
                    }
                }
                catch (Exception e)
                {
                    WriteLog("删除宝贝" + e.StackTrace, "1",nick2,"");
                    WriteLog("删除宝贝" + e.Message, "1", nick2, "");
                    sql = "UPDATE TopMission SET fail = fail + 1,isok = -1 WHERE id = " + dt.Rows[i]["id"].ToString();
                    utils.ExecuteNonQuery(sql);
                    continue;
                }
            }
            dtWrite.Dispose();


        }
        dt.Dispose();


        WriteLog("**********************************************************", "",nick2,"");
 
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
    /// 写日志
    /// </summary>
    /// <param name="value">日志内容</param>
    /// <param name="type">类型 0(成功日志),1(错误日志) 可传空文本默认为0</param>
    /// <returns></returns>
    public void WriteLog(string message, string type, string nick, string mid)
    {

        string tempStr = logUrl + "/groupbuy" + nick + DateTime.Now.ToString("yyyyMMdd");//文件夹路径
        string tempFile = tempStr + "/delgroupbuypromotion" + mid + DateTime.Now.ToString("yyyyMMdd") + ".txt";
        //if (type == "1")
        //{
        //    tempFile = tempStr + "/activitypromotionErr" + nick + DateTime.Now.ToString("yyyyMMdd") + ".txt";
        //}
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