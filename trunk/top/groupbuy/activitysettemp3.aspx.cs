using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.IO;
using System.Data;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Collections;
using System.Web.Security;
using System.Data;
using System.Threading;
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Util;

public partial class top_groupbuy_activitysettemp1 : System.Web.UI.Page
{
    public string name = string.Empty;
    public string templetid = string.Empty;
    public string bt = string.Empty;
    public string mall = string.Empty;
    public string liang = string.Empty;
    public string baoy = string.Empty;
    public string hdID = string.Empty;
    public string productid = string.Empty;
    public string price = string.Empty;
    public string zhekou = string.Empty;
    public string rcount = string.Empty;
    public string sort = string.Empty;
    public string sql = string.Empty;
    public string pid = string.Empty;
    public string pimg = string.Empty;
    public string purl = string.Empty;
    public string pname = string.Empty;
    string appkey = "12287381";
    string secret = "d3486dac8198ef01000e7bd4504601a4";
 

    protected void Page_Load(object sender, EventArgs e)
    {
        name = Request.Form["name"].ToString();
        templetid = Request.Form["templetid"].ToString();//模板ID
        bt = Request.Form["bt"].ToString();
        mall = Request.Form["mall"].ToString();
        liang = Request.Form["liang"].ToString();
        hdID = Request.Form["hdID"].ToString();//活动ID
        baoy = Request.Form["baoy"].ToString();

 

        productid = Request.Form["productid"].ToString();
        price = Request.Form["price"].ToString();
        zhekou = Request.Form["zhekou"].ToString();
        rcount = Request.Form["rcount"].ToString();
        sort = Request.Form["sort"].ToString();
        pimg = Request.Form["pimg"].ToString(); 
        pname=  Request.Form["pname"].ToString();
        Cookie cookie = new Cookie();
         string taobaoNick = cookie.getCookie("nick");
         string session = cookie.getCookie("top_sessiongroupbuy");
        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);
        //判断模板图片是否已经上传（查询本地数据库tete_shoptempletimg）,生成HTML时图片需要替换图片地址
        sql = "select * from  tete_shoptempletimg where nick='"+taobaoNick+"'";
        DataTable dts3 = utils.ExecuteDataTable(sql);

        if (dts3 != null && dts3.Rows.Count > 0)
        {
            //如果没有上传，断模板分类是否创建,
            sql = "select * from  tete_shoptempletimg where nick='" + taobaoNick + "' and templetID=" + templetid;
            dts3 = utils.ExecuteDataTable(sql);
            //如果没有上传图片
            if (dts3 == null || dts3.Rows.Count < 1)
            {
                //获取分类ID，上传图片，返回图片地址，创建本地店铺模板图片地址
                IDictionary<string, string> param = new Dictionary<string, string>();
                //创建活动
                param = new Dictionary<string, string>();
                param.Add("picture_category_name", "特特团购图片");

                string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.picture.category.get", session, param);

                Response.Write(result);
                Response.End();
            }

        }
        else {
            //添加分类，获取分类ID，上传图片，返回图片地址，创建本地店铺模板图片地址
            IDictionary<string, string> param = new Dictionary<string, string>();
            //创建特特图片分类
            param = new Dictionary<string, string>();
            param.Add("picture_category_name", "特特团购模板图片勿删");

            string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.picture.category.add", session, param);
            if (result.IndexOf("error_response") != -1)
            {
                string err = new Regex(@"<sub_msg>([^<]*)</sub_msg>", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
                if (err == "")
                {
                    Response.Write("<b>模板创建失败，错误原因：</b><br><font color='red'>您的session已经失效，需要重新授权</font><br><a href='http://container.api.taobao.com/container?appkey=12287381&scope=promotion' target='_parent'>重新授权</a>");
                    Response.End();
                }

                Response.Write("<b>模板创建失败，错误原因：</b><br><font color='red'>" + err + "</font><br><a href='groupbuyadd.aspx'>重新添加</a>");
                Response.End();
                return;
            }
            string categoryid = new Regex(@"<picture_category_id>([^<]*)</picture_category_id>", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
            sql = "select * from  tete_templetimg where templetID=" + templetid;
            dts3 = utils.ExecuteDataTable(sql);
            if (dts3 != null && dts3.Rows.Count > 0)
            {
                //如上传图片，返回图片地址，创建本地店铺模板图片地址
                for (int j = 0; j < dts3.Rows.Count; j++)
                {
                    //上传图片
                    string newurl = TaobaoUpload(dts3.Rows[j]["url"].ToString(), "templetid" + j.ToString(), long.Parse(categoryid));
                    //创建本地店铺模板图片地址
                    sql = "insert into tete_shoptempletimg ([templetID],[url] ,[taobaourl] ,[nick]) VALUES (" + templetid + ",'" + dts3.Rows[j]["url"].ToString() + "','" + newurl + "','" + taobaoNick + "')";
                    utils.ExecuteNonQuery(sql);
                }
            }
            Response.Write(result +"cid="+ categoryid);
            Response.End();
        }
 
        //添加店铺模板
        sql = "INSERT INTO  [tete_shoptemplet] ([templetID] ,[buttonValue] ,[scbzvalue] ,[lpbzvalue] ,[byvalue] ,[nick] ,[title] ,careteDate)   VALUES   ("+templetid+",'"+bt+"','"+mall+"','"+liang+"','"+baoy+"' ,'"+taobaoNick+"' ,'"+name+"' ,'"+ DateTime.Now.ToString()+"')";
 
         utils.ExecuteNonQuery(sql);
         DataTable dt2 = utils.ExecuteDataTable("select top 1 * from tete_shoptemplet where nick='" + taobaoNick + "' order by id desc");
         string shoptempid = "";
         if (dt2 != null && dt2.Rows.Count > 0)
         {
             shoptempid = dt2.Rows[0]["id"].ToString();
         }
 
         if (shoptempid != "")
         {
 
             //添加店铺模板列表
             for (int i = 0; i < price.Split(',').Length; i++)
             {
                 sql = "INSERT INTO  [tete_shoptempletlist] ([shoptempletID],[name],[price],[proprice],[Sort],[nick],[rcount],[ProductImg],[ProductUrl],[ProductID])  VALUES (" + shoptempid + ",'" + pname.Split(',')[i].ToString() + "','" + price.Split(',')[i].ToString() + "','" + zhekou.Split(',')[i].ToString() + "'," + sort.Split(',')[i].ToString() + ",'" + taobaoNick + "'," + rcount.Split(',')[i].ToString() + ",'" + pimg.Split(',')[i].ToString() + "','http:///item.taobao.com/item.htm?id=" + productid.Split(',')[i].ToString() + "','" + productid.Split(',')[i].ToString() + "')";
                  
                 utils.ExecuteNonQuery(sql);
   
             }
            //替换图片
             TextBox1.Text  = CreateGroupbuyHtml(shoptempid);

         }
         else
         {
             TextBox1.Text = "模板创建失败！";
         }
    }

    /// <summary>
    /// 图片上传
    /// </summary>
    /// <param name="picurl"></param>
    /// <param name="picname"></param>
    /// <param name="CategoryId"></param>
    /// <returns></returns>
    public string TaobaoUpload(string picurl, string picname, long CategoryId)
    {
        Cookie cookie = new Cookie(); 
        string session = cookie.getCookie("top_sessiongroupbuy");
        TopXmlRestClient clientaa = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", appkey, secret);
        PictureUploadRequest request = new PictureUploadRequest();

        string filepath = Server.MapPath("images/" + picurl);

        request.Img = new FileItem(filepath, File.ReadAllBytes(filepath));
        request.ImageInputTitle = picurl;
        request.PictureCategoryId = CategoryId;
        request.Title = picname;

        clientaa.PictureUpload(request, session);


        PictureGetRequest request1 = new PictureGetRequest();
        request1.Title = picname;
        string path = string.Empty;
        path = clientaa.PictureGet(request1, session).Content[0].PicturePath;

        return path;
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="id">店铺模板ID</param>
    /// <returns></returns>
    private string CreateGroupbuyHtml(string id)
    {
        if (id == "")
        {
            return "";
        }
        string str = string.Empty;
        string sql = "select tete_shoptempletlist.*,templetID,[buttonValue],[scbzvalue],[lpbzvalue],[byvalue],[title],[careteDate],[Sort] from tete_shoptempletlist left join   tete_shoptemplet on tete_shoptempletlist.shoptempletID=tete_shoptemplet.ID  WHERE tete_shoptemplet.templetID = '" + id + "'";
       
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt == null)
        {
            return "";
        }
        string templatehtmlUrl = "tpl/style1.html";//默认模板
        string template2htmlUrl = "tpl/stylenew2-1.html";//第二套模板（一大三小） 小模板  (团购模板第三套和第二套)
 
        //模板生成需要在好好考虑一下。。。。。。。。。。。。。。。。。。。。。。。。。。。。
            //是多商品团购模板
            if (dt.Rows[0]["templetID"].ToString() == "2")
            {
                //第二套模板（一大三小）
                templatehtmlUrl = "tpl/stylenew2.html";
            }
            //是多商品团购模板
            if (dt.Rows[0]["templetID"].ToString() == "3")
            {
                //第三套模板（一排三列）
                templatehtmlUrl = "tpl/style3.html";
            }

 
        string html = File.ReadAllText(Server.MapPath(templatehtmlUrl));
        string smailtempStr = string.Empty;//小模板
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string purl =  dt.Rows[i]["ProductUrl"].ToString();
            if (i == 0)
            {
                str = html;
                str = str.Replace("{name}", dt.Rows[i]["name"].ToString());
                str = str.Replace("{oldprice}", dt.Rows[i]["price"].ToString());
                str = str.Replace("{zhekou}", Math.Round((decimal.Parse(dt.Rows[i]["price"].ToString()) - decimal.Parse(dt.Rows[i]["proprice"].ToString())) / decimal.Parse(dt.Rows[i]["price"].ToString()) * 10, 1).ToString());
                str = str.Replace("{leftprice}", (decimal.Parse(dt.Rows[i]["price"].ToString()) - decimal.Parse(dt.Rows[i]["proprice"].ToString())).ToString().Split('.')[0]);
                str = str.Replace("{rightprice}", (decimal.Parse(dt.Rows[i]["price"].ToString()) - decimal.Parse(dt.Rows[i]["proprice"].ToString())).ToString().Split('.')[1]);
                str = str.Replace("{newprice}", (decimal.Parse(dt.Rows[i]["price"].ToString()) - decimal.Parse(dt.Rows[i]["proprice"].ToString())).ToString());
                str = str.Replace("{buycount}", dt.Rows[i]["rcount"].ToString());
                str = str.Replace("{producturl}", dt.Rows[i]["producturl"].ToString());
                str = str.Replace("{productimg}", dt.Rows[i]["productimg"].ToString());
                str = str.Replace("{id}", id);
                str = str.Replace("'", "''");
                //如果模板是第三套模板，追加第一个活动HTML
                if (templatehtmlUrl == "tpl/style3.html")
                {
                    html = File.ReadAllText(Server.MapPath(template2htmlUrl));
                    smailtempStr += html;
                    smailtempStr = smailtempStr.Replace("{name}", dt.Rows[i]["name"].ToString());
                    smailtempStr = smailtempStr.Replace("{oldprice}", dt.Rows[i]["productprice"].ToString());
                    smailtempStr = smailtempStr.Replace("{zhekou}", Math.Round((decimal.Parse(dt.Rows[i]["productprice"].ToString()) - decimal.Parse(dt.Rows[i]["zhekou"].ToString())) / decimal.Parse(dt.Rows[i]["productprice"].ToString()) * 10, 1).ToString());
                    smailtempStr = smailtempStr.Replace("{leftprice}", (decimal.Parse(dt.Rows[i]["productprice"].ToString()) - decimal.Parse(dt.Rows[i]["zhekou"].ToString())).ToString().Split('.')[0]);
                    smailtempStr = smailtempStr.Replace("{rightprice}", (decimal.Parse(dt.Rows[i]["productprice"].ToString()) - decimal.Parse(dt.Rows[i]["zhekou"].ToString())).ToString().Split('.')[1]);
                    smailtempStr = smailtempStr.Replace("{newprice}", dt.Rows[i]["zhekou"].ToString());
                    smailtempStr = smailtempStr.Replace("{buycount}", dt.Rows[i]["buycount"].ToString());
                    smailtempStr = smailtempStr.Replace("{producturl}", dt.Rows[i]["producturl"].ToString());
                    smailtempStr = smailtempStr.Replace("{productimg}", dt.Rows[i]["productimg"].ToString());
                    smailtempStr = smailtempStr.Replace("{id}", id);
                    smailtempStr = smailtempStr.Replace("'", "''");
                }
            }
            else
            {
                //是多商品团购模板
                if (dt.Rows[i]["templetID"].ToString() == "2" || dt.Rows[i]["templetID"].ToString() == "3")
                {
                    html = File.ReadAllText(Server.MapPath(template2htmlUrl));
                    smailtempStr += html;
                    smailtempStr = smailtempStr.Replace("{name}", dt.Rows[i]["name"].ToString());
                    smailtempStr = smailtempStr.Replace("{oldprice}", dt.Rows[i]["productprice"].ToString());
                    smailtempStr = smailtempStr.Replace("{zhekou}", Math.Round((decimal.Parse(dt.Rows[i]["productprice"].ToString()) - decimal.Parse(dt.Rows[i]["zhekou"].ToString())) / decimal.Parse(dt.Rows[i]["productprice"].ToString()) * 10, 1).ToString());
                    smailtempStr = smailtempStr.Replace("{leftprice}", (decimal.Parse(dt.Rows[i]["productprice"].ToString()) - decimal.Parse(dt.Rows[i]["zhekou"].ToString())).ToString().Split('.')[0]);
                    smailtempStr = smailtempStr.Replace("{rightprice}", (decimal.Parse(dt.Rows[i]["productprice"].ToString()) - decimal.Parse(dt.Rows[i]["zhekou"].ToString())).ToString().Split('.')[1]);
                    smailtempStr = smailtempStr.Replace("{newprice}", dt.Rows[i]["zhekou"].ToString());
                    smailtempStr = smailtempStr.Replace("{buycount}", dt.Rows[i]["buycount"].ToString());
                    smailtempStr = smailtempStr.Replace("{producturl}", dt.Rows[i]["producturl"].ToString());
                    smailtempStr = smailtempStr.Replace("{productimg}", dt.Rows[i]["productimg"].ToString());
                    smailtempStr = smailtempStr.Replace("{id}", id);
                    smailtempStr = smailtempStr.Replace("'", "''");
                }

            }
        }
        str = str.Replace("{productlist}", smailtempStr);//一大三小模板，商品列表替换
        return str;
    }

    #region  TOP API POST 请求

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

    #endregion
}