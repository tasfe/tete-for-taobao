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

public partial class top_groupbuy_activitytempmanage : System.Web.UI.Page
{

    public string logUrl = "D:/groupbuy.7fshop.com/wwwroot/top/groupbuy/ErrLog";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["act"] != null&&Request.QueryString["act"].ToString()=="del")
        {
            if (Request.QueryString["id"] != null)
            {
                string id = Request.QueryString["id"].ToString();
                string aid = Request.QueryString["aid"].ToString();
                //删除模板


                utils.ExecuteNonQuery("update tete_shoptemplet set Isdelete=1 where id=" + id);
                //删除模板列表,10天后再删除
                //utils.ExecuteNonQuery("delete from tete_shoptempletlist where shoptempletID=" + id);
                //清除宝贝描述
                ClearitemsDesc(id, aid);
                Response.Redirect("activitytempmanage.aspx");
            }
        }
        if (Request.QueryString["act"] != null && Request.QueryString["act"].ToString() == "delitem")
        {
            if (Request.QueryString["id"] != null)
            {
                string id = Request.QueryString["id"].ToString();
                string aid = Request.QueryString["aid"].ToString();
                //清除宝贝描述
                //utils.ExecuteNonQuery("delete from tete_shoptempletlist where shoptempletID=" + id);
                //清除宝贝描述
                ClearitemsDesc(id, aid);
                
            }
        }
        if (!IsPostBack)
        {
            BindData();
        }
    }

    /// <summary>
    /// 清除宝贝描述
    /// </summary>
    /// <param name="id">模板ID</param>
    /// <param name="aid">活动ID</param>
    public void ClearitemsDesc(string id,string aid)
    {
        if (id != "" && !utils.IsInt32(id))
        {
            Response.Write("非法参数1");
            Response.End();
            return;
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

        string sql = "SELECT COUNT(*) FROM Tete_ActivityMission WHERE shoptempletID = " + id + " AND typ='delete' AND isok = 0 AND nick<>''";
        string count = utils.ExecuteString(sql);

        if (count != "0")
        {
            Response.Write("<script>alert('创建任务失败，有同类型的任务正在执行中，请等待其完成后再创建新的任务！');window.location.href='activitymissionlist.aspx';</script>");
            Response.End();
            return;
        }


        if (taobaoNick.Trim() == "")
        {
            //SESSION超期 跳转到登录页
            Response.Write("<script>parent.location.href='http://container.open.taobao.com/container?appkey=12287381'</script>");
            Response.End();
        } 
        sql = "INSERT INTO Tete_ActivityMission (typ, nick, ActivityID,IsOK,shoptempletID,Success,Fail,Startdate) VALUES ('delete','" + taobaoNick + "','" + aid + "',0,'" + id + "',0,0,'" + DateTime.Now.ToString() + "')";
        utils.ExecuteNonQuery(sql);


        sql = "SELECT TOP 1 ID FROM Tete_ActivityMission ORDER BY ID DESC";
        string missionid = utils.ExecuteString(sql);

        

        //更新任务总数
        sql = "SELECT COUNT(*) FROM Tete_ActivityWriteContent WHERE shoptempletID = '" + id + "' and ActivityID=" + aid + " AND isok = 1";
        count = utils.ExecuteString(sql);
        sql = "UPDATE tete_ActivityMission SET total = '" + count + "' WHERE id = " + missionid;
        utils.ExecuteNonQuery(sql);
        // 清除宝贝描述
        DeleteTaobaoitmes(taobaoNick);
        Response.Redirect("activitymissionlist.aspx");
    }


    /// <summary>
    /// 清除宝贝描述
    /// </summary>
    private void DeleteTaobaoitmes(string nick2)
    {

        WriteLog("活动清除进行", "1", "err", "");
        //获取正在进行中的宝贝同步任务        
        string appkey = "12287381";
        string secret = "d3486dac8198ef01000e7bd4504601a4";
        string session = string.Empty;
        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", appkey, secret);

        string sql = "SELECT top 100 t.*, s.sessiongroupbuy FROM Tete_ActivityMission t INNER JOIN TopTaobaoShop s ON s.nick = t.nick WHERE t.isok = 0 AND t.typ = 'delete' and s.nick='" + nick2 + "'  ORDER BY t.id ASC";

        DataTable dt = utils.ExecuteDataTable(sql);
        DataTable dtWrite = null;

        if (dt != null)
        {
            WriteLog("活动清除进行中", "1", "err", "");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                session = dt.Rows[i]["sessiongroupbuy"].ToString();
                sql = "SELECT * FROM Tete_ActivityWriteContent WHERE shoptempletID= '" + dt.Rows[i]["shoptempletID"].ToString() + "' and  ActivityID = '" + dt.Rows[i]["ActivityID"].ToString() + "'";
                WriteLog(sql, "1", "err", "");
                dtWrite = utils.ExecuteDataTable(sql);
                if (dtWrite != null)
                {
                    WriteLog("商品总数" + dtWrite.Rows.Count.ToString(), "1", "err", "");
                    for (int j = 0; j < dtWrite.Rows.Count; j++)
                    {

                        try
                        {
                            //获取原宝贝描述
                            ItemGetRequest requestItem = new ItemGetRequest();
                            requestItem.Fields = "desc";
                            requestItem.NumIid = long.Parse(dtWrite.Rows[j]["itemid"].ToString());
                            Item product = client.ItemGet(requestItem, session);
                            string newContent = string.Empty;
                            string ActivityMissionID = dtWrite.Rows[j]["ActivityMissionID"].ToString();
                            string tetegroupbuyGuid = ActivityMissionID;
                            string sqltemp = "SELECT * FROM Tete_ActivityMission WHERE id = '" + ActivityMissionID + "'";
                            DataTable dttemp = utils.ExecuteDataTable(sqltemp);
                            if (dttemp == null)
                            {
                                return;
                            }
                            newContent = product.Desc;
                            if (!Regex.IsMatch(product.Desc, @"<div>[\s]*<a name=""tetesoft-area-start-" + tetegroupbuyGuid + @""">[\s]*</a>[\s]*</div>[\s]*([\s\S]*)<div>[\s]*<a name=""tetesoft-area-end-" + tetegroupbuyGuid + @""">[\s]*</a>[\s]*</div>"))
                            {
                                //如果没有代码
                                //更新状态
                                // WriteDeleteLog2("http://item.taobao.com/item.htm?id=" + dtWrite.Rows[j]["itemid"].ToString() + " 不含需要清除的代码", "");
                                //更新状态
                                WriteLog("http://item.taobao.com/item.htm?id=" + dtWrite.Rows[j]["itemid"].ToString() + " 不含需要清除的代码" + dtWrite.Rows.Count.ToString(), "1", "err", "");
                                sql = "UPDATE Tete_ActivityWriteContent SET isok = 1 WHERE id = " + dtWrite.Rows[j]["id"].ToString();
                                utils.ExecuteNonQuery(sql);

                                //更新状态
                                sql = "UPDATE Tete_ActivityMission SET success = success + 1 WHERE id = " + dt.Rows[i]["id"].ToString();
                                utils.ExecuteNonQuery(sql);
                                continue;
                            }
                            else
                            {
                                newContent = Regex.Replace(product.Desc, @"<div>[\s]*<a name=""tetesoft-area-start-" + tetegroupbuyGuid + @""">[\s]*</a>[\s]*</div>[\s]*([\s\S]*)<div>[\s]*<a name=""tetesoft-area-end-" + tetegroupbuyGuid + @""">[\s]*</a>[\s]*</div>", @"");



                                //更新宝贝描述
                                IDictionary<string, string> param = new Dictionary<string, string>();
                                param.Add("num_iid", dtWrite.Rows[j]["itemid"].ToString());
                                param.Add("desc", newContent);

                                string resultpro = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.item.update ", session, param);
                                //插入宝贝错误日志
                                if (resultpro.IndexOf("ITEM_PROPERTIES_ERROR") != -1)
                                {

                                    WriteLog("errID：" + dtWrite.Rows[j]["itemid"].ToString() + "errinfo" + resultpro, "", dt.Rows[i]["nick"].ToString(), dtWrite.Rows[j]["ActivityMissionID"].ToString());
                                    //更新宝贝错误数
                                    sql = "UPDATE Tete_ActivityMission SET fail = fail + 1,isok = -1  WHERE id = " + dt.Rows[i]["id"].ToString();
                                    utils.ExecuteNonQuery(sql);
                                }
                                else
                                {
                                    WriteLog("删除itemid:" + dtWrite.Rows[j]["itemid"].ToString() + resultpro, "", dt.Rows[i]["nick"].ToString(), dtWrite.Rows[j]["ActivityMissionID"].ToString());
                                    //更新状态
                                    sql = "UPDATE Tete_ActivityWriteContent SET isok = 1 WHERE id = " + dtWrite.Rows[j]["id"].ToString();
                                    utils.ExecuteNonQuery(sql);

                                    //更新状态
                                    sql = "UPDATE Tete_ActivityMission SET success = success + 1 WHERE id = " + dt.Rows[i]["id"].ToString();
                                    utils.ExecuteNonQuery(sql);
                                }
                            }

                        }
                        catch (Exception e)
                        {
                            WriteLog("失败" + e.Message, "1", dt.Rows[i]["nick"].ToString(), dtWrite.Rows[j]["ActivityMissionID"].ToString());
                            WriteLog("失败" + e.StackTrace, "1", dt.Rows[i]["nick"].ToString(), dtWrite.Rows[j]["ActivityMissionID"].ToString());
                            sql = "UPDATE Tete_ActivityMission SET fail = fail + 1,isok = -1  WHERE id = " + dt.Rows[i]["id"].ToString();
                            utils.ExecuteNonQuery(sql);
                            continue;
                        }
                    }

                    dtWrite.Dispose();
                }
                sql = "UPDATE Tete_ActivityMission SET isok = 1 WHERE id = " + dt.Rows[i]["id"].ToString();
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
    public void WriteLog(string message, string type, string nick, string mid)
    {

        string tempStr = logUrl + "/activity" + nick + DateTime.Now.ToString("yyyyMMdd");//文件夹路径
        string tempFile = tempStr + "/delactivitypromotion" + mid + DateTime.Now.ToString("yyyyMMdd") + ".txt";
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
    /// 
    /// </summary>
    /// <param name="ID"></param>
    /// <param name="actionID"></param>
    /// <param name="pid"></param>
    /// <returns></returns>
    public string outShowHtml(string ID,string aid)
    {
        //string html = "<div>  <a  target=_blank href=\"activitytempView.aspx?tid=" + ID + "\")\">获取代码</a>   <a  target=_blank  href=\"activitytempView.aspx?ytid=" + ID + "\")\">预览</a>      <a href=\"activitytempmanage.aspx?act=del&aid=" + aid + "&id=" + ID + "\")\">删除</a>   <a href=\"activitytempmanage.aspx?act=delitem&aid=" + aid + "&id=" + ID + "\")\" ' onclick=\"return confirm('您确认要清除关联描述，该操作不可恢复？')\">清除宝贝描述</a><div id=\"del" + ID + "\"></div></div>";//javascript:addItemAction(12305275000)

        string html = "<div>  <a  target=_blank href=\"activitytempView.aspx?tid=" + ID + "\")\">获取代码</a>   <a  target=_blank  href=\"activitytempView.aspx?ytid=" + ID + "\")\">预览</a>      <a href=\"activitytempmanage.aspx?act=del&aid=" + aid + "&id=" + ID + "\")\">删除</a>   <a href=\"javascript:delItemtemp('delitem','" + aid + "','" + ID + "')\"  onclick=\"return confirm('您确认要清除关联描述，该操作不可恢复？')\">清除宝贝描述</a><div id=\"del" + ID + "\"></div></div>";

        return html;
    }

    private void BindData()
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        //taobaoNick = HttpUtility.UrlEncode(taobaoNick);

        string page = utils.NewRequest("page", utils.RequestType.QueryString);
        int pageNow = 1;
        if (page == "")
        {
            pageNow = 1;
        }
        else
        {
            pageNow = int.Parse(page);
        }
        int pageCount = 10;
        int dataCount = (pageNow - 1) * pageCount;
        //select [tete_shoptemplet].*, [tete_templet].name from [tete_shoptemplet] 
        //left join [tete_templet] on [tete_shoptemplet].[templetID]=[tete_templet].id
        string sqlNew = "SELECT TOP " + pageCount.ToString() + " * FROM (SELECT b.*,name,ROW_NUMBER() OVER (ORDER BY b.id DESC) AS rownumber FROM tete_shoptemplet b left join [tete_templet] on b.[templetID]=[tete_templet].id WHERE b.Isdelete=0 and b.nick = '" + taobaoNick + "' ) AS a WHERE a.rownumber > " + dataCount.ToString() + " ORDER BY id DESC";

 //Response.Write(sqlNew);
        DataTable dtNew = utils.ExecuteDataTable(sqlNew);
        rptItems.DataSource = dtNew;
        rptItems.DataBind();
        //
        //分页数据初始化
        sqlNew = "SELECT COUNT(*) FROM tete_shoptemplet WHERE Isdelete=0 and nick = '" + taobaoNick + "' ";
        int totalCount = int.Parse(utils.ExecuteString(sqlNew));

        lbPage.Text = InitPageStr(totalCount, "activitytempmanage.aspx");
    }

    private string InitPageStr(int total, string url)
    {
        //分页数据初始化
        string str = string.Empty;
        int pageCount = 10;
        int pageSize = 0;
        int pageNow = 1;
        string page = utils.NewRequest("page", utils.RequestType.QueryString);
        if (page == "")
        {
            pageNow = 1;
        }
        else
        {
            pageNow = int.Parse(page);
        }

        //取总分页数
        if (total % pageCount == 0)
        {
            pageSize = total / pageCount;
        }
        else
        {
            pageSize = total / pageCount + 1;
        }

        for (int i = 1; i <= pageSize; i++)
        {
            if (i.ToString() == pageNow.ToString())
            {
                str += i.ToString() + " ";
            }
            else
            {
                str += "<a href='" + url + "?page=" + i.ToString() + "'>[" + i.ToString() + "]</a> ";
            }
        }

        return str;
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