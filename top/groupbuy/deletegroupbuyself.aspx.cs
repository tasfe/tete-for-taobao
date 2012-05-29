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

public partial class top_market_deletegroupbuy : System.Web.UI.Page
{
    public string nick = string.Empty;
    public string session = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        nick = this.TextBox1.Text;
        session = this.TextBox2.Text;

        DeleteTaobaAuto(session);
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        nick = this.TextBox1.Text;
        session = this.TextBox2.Text;

        string appkey = "12132145";
        string secret = "1fdd2aadd5e2ac2909db2967cbb71e7f";
        //上传到宝贝描述
        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", appkey, secret);
        for (int j = 1; j <= 500; j++)
        {
            ItemsOnsaleGetRequest request = new ItemsOnsaleGetRequest();
            request.Fields = "num_iid";
            request.PageSize = 200;
            request.PageNo = j;
            PageList<Item> product = client.ItemsOnsaleGet(request, session);

            for (int i = 0; i < product.Content.Count; i++)
            {
                try
                {
                    //获取商品详细
                    ItemGetRequest requestItem = new ItemGetRequest();
                    requestItem.Fields = "desc";
                    requestItem.NumIid = product.Content[i].NumIid;
                    Item item = client.ItemGet(requestItem, session);

                    //判断是否增加过该图片
                    string newcontent = CreateDescDelHaoping(item.Desc);

                    //if (product.Content[i].NumIid.ToString() == "10002247109")
                    //{
                    //    Response.Write(item.Desc);
                    //    Response.Write("**************************************************");
                    //    Response.Write(newcontent);
                    //    return;
                    //}

                    if (newcontent == "")
                    {
                        continue;
                    }

                    //更新宝贝描述
                    IDictionary<string, string> param = new Dictionary<string, string>();
                    param.Add("num_iid", product.Content[i].NumIid.ToString());
                    param.Add("desc", newcontent);
                    string resultpro = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.item.update", session, param);
                }
                catch
                { }
            }

            if (product.Content.Count < 200)
            {
                break;
            }
        }


        Response.Write("<script>alert('清除成功！');</script>");
        Response.End();
    }

    private void DeleteTaobaAuto(string session)
    {
        string appkey = "12287381";
        string secret = "d3486dac8198ef01000e7bd4504601a4";
        //上传到宝贝描述
        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", appkey, secret);
        for (int j = 1; j <= 2; j++)
        {
            ItemsOnsaleGetRequest request = new ItemsOnsaleGetRequest();
            request.Fields = "num_iid";
            request.PageSize = 200;
            request.PageNo = j;
            PageList<Item> product = client.ItemsOnsaleGet(request, session);

            for (int i = 0; i < product.Content.Count; i++)
            {
                try
                {
                    //获取商品详细
                    ItemGetRequest requestItem = new ItemGetRequest();
                    requestItem.Fields = "desc";
                    requestItem.NumIid = product.Content[i].NumIid;
                    Item item = client.ItemGet(requestItem, session);

                    //判断是否增加过该图片
                    string newcontent = CreateDescDel(item.Desc);

                    Response.Write(newcontent);
                    return;

                    if (newcontent == "")
                    {
                        continue;
                    }

                    //更新宝贝描述
                    IDictionary<string, string> param = new Dictionary<string, string>();
                    param.Add("num_iid", product.Content[i].NumIid.ToString());
                    param.Add("desc", newcontent);
                    string resultpro = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.item.update", session, param);
                }
                catch(Exception e)
                {
                    Response.Write(e.Message);
                }
            }
            Response.Write(product.Content.Count.ToString() + "<br>");
            if (product.Content.Count < 200)
            {
                break;
            }
        }

        Response.Write("<script>alert('清除成功！');</script>");
        Response.End();
    }





    /// <summary>
    /// 获取调整过的宝贝描述
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    private string CreateDescDelHaoping(string desc)
    {
        string newdesc = string.Empty;

        if (!Regex.IsMatch(desc, @"<div>[\s]*<a name=""tetehaoping-start"">[\s]*</a>[\s]*</div>[\s]*<div>[\s]*<img src=""([\s\S]*)"">[\s]*</div>[\s]*<div>[\s]*<a name=""tetehaoping-end"">[\s]*</a>[\s]*</div>"))
        {
            newdesc = desc;
            return "";
        }
        else
        {
            newdesc = Regex.Replace(desc, @"<div>[\s]*<a name=""tetehaoping-start"">[\s]*</a>[\s]*</div>[\s]*<div>[\s]*<img src=""([\s\S]*)"">[\s]*</div>[\s]*<div>[\s]*<a name=""tetehaoping-end"">[\s]*</a>[\s]*</div>", @"");
        }

        return newdesc;
    }


    /// <summary>
    /// 获取调整过的宝贝描述
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    private string CreateDescDel(string desc)
    {
        string newdesc = string.Empty;
        if (!Regex.IsMatch(desc, @"<div>[\s]*<a name=""tetesoft-area-start-[0-9]*"">[\s]*</a>[\s]*</div>[\s]*([\s\S]*)<div>[\s]*<a name=""tetesoft-area-end-[0-9]*"">[\s]*</a>[\s]*</div>"))
        {
            newdesc = desc;
            return "";
        }
        else
        {
            newdesc = Regex.Replace(desc, @"<div>[\s]*<a name=""tetesoft-area-start-[0-9]*"">[\s]*</a>[\s]*</div>[\s]*([\s\S]*)<div>[\s]*<a name=""tetesoft-area-end-[0-9]*"">[\s]*</a>[\s]*</div>", @"");
        }

        return newdesc;
    }

    private string CreateDescDelList(string desc)
    {
        string newdesc = string.Empty;
        if (!Regex.IsMatch(desc, @"<div>[\s]*<a name=tetesoft-area-start-[0-9]*>[\s]*</a>[\s]*</div>[\s]*([\s\S]*)<div>[\s]*<a name=tetesoft-area-end-[0-9]*>[\s]*</a>[\s]*</div>"))
        {
            newdesc = desc;
            return "";
        }
        else
        {
            newdesc = Regex.Replace(desc, @"<div>[\s]*<a name=tetesoft-area-start-[0-9]*>[\s]*</a>[\s]*</div>[\s]*([\s\S]*)<div>[\s]*<a name=tetesoft-area-end-[0-9]*>[\s]*</a>[\s]*</div>", @"");
        }

        return newdesc;
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