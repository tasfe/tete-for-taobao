using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;
using System.Net;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Data;
using Taobao.Top.Api.Util;

public partial class top_review_html : System.Web.UI.Page
{
    public string session = string.Empty;
    public string nick = string.Empty;
    public string nickencode = string.Empty;
    public string url = string.Empty;
    public string leftimgurl = string.Empty;
    public string leftimgistop = string.Empty;
    public string leftimgname = string.Empty;
    public string detailimgurl = string.Empty;
    public string detailimgistop = string.Empty;
    public string detailimgname = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        session = cookie.getCookie("top_sessiongroupbuy");
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Decrypt(taobaoNick);

        nickencode = HttpUtility.UrlEncode(nick);
        url = HttpUtility.UrlEncode("http://container.api.taobao.com/container?action=freecard&appkey=12690739&newnick=");

        //过期判断
        if (string.IsNullOrEmpty(taobaoNick))
        {
            string msg = "尊敬的淘宝卖家，非常抱歉的告诉您，您还没有购买此服务或者登录信息已失效，如需继续使用请<a href='http://fuwu.taobao.com/serv/detail.htm?service_id=4545' target='_blank'>进入该服务</a>，谢谢！";
            Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
            Response.End();
            return;
        }

        //判断VIP版本，只有VIP才能使用此功能
        string sql = "SELECT * FROM TCS_ShopSession WHERE nick = '" + nick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            string flag = dt.Rows[0]["version"].ToString();
            if (flag == "0")
            {
                Response.Redirect("xufei.aspx");
                Response.End();
                return;
            }
        }

        BindData();
    }

    private void BindData()
    {
        string sql = "SELECT * FROM TCS_ShopConfig WHERE nick = '" + nick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            leftimgurl = dt.Rows[0]["leftimgurl"].ToString();
            leftimgistop = dt.Rows[0]["leftimgistop"].ToString();
            leftimgname = dt.Rows[0]["leftimgname"].ToString();
            detailimgurl = dt.Rows[0]["detailimgurl"].ToString();
            detailimgistop = dt.Rows[0]["detailimgistop"].ToString();
            detailimgname = dt.Rows[0]["detailimgname"].ToString();

            if (leftimgurl == "")
            {
                try
                {
                    UploadImg();
                }
                catch
                {
                    Response.Write("装修图片需要上传到您的淘宝图片空间，您的空间剩余大小不够，无法上传！");
                    Response.End();
                }
            }
        }
        else
        {
            Response.Write("请先到基本设置里面进行信息设置，<a href='setting.aspx'>点此进入</a>");
            Response.End();
        }
    }

    /// <summary>
    /// 上传图片到客户自己空间幷保存返回地址
    /// </summary>
    private void UploadImg()
    {
        string pic1 = TaobaoUpload("left2.jpg", "特特好评有礼左侧菜单图片1");
        string pic2 = TaobaoUpload("left3.jpg", "特特好评有礼左侧菜单图片2");
        string pic3 = TaobaoUpload("left4.jpg", "特特好评有礼左侧菜单图片3");
        string pic4 = TaobaoUpload("left5.jpg", "特特好评有礼左侧菜单图片4");

        if (pic1 == "" || pic2 == "" || pic3 == "" || pic4 == "")
        {
            Response.Write("您的淘宝图片空间已满，请清理后再使用此功能！");
            Response.End();
            return;
        }
        string leftimgurl = pic1 + "|" + pic2 + "|" + pic3 + "|" + pic4;


        string pic5 = TaobaoUpload("750a.jpg", "特特好评有礼宝贝描述图片1");
        string pic6 = TaobaoUpload("750b.jpg", "特特好评有礼宝贝描述图片2");
        string pic7 = TaobaoUpload("750c.jpg", "特特好评有礼宝贝描述图片3");

        if (pic5 == "" || pic6 == "" || pic7 == "")
        {
            Response.Write("您的淘宝图片空间已满，请清理后再使用此功能！");
            Response.End();
            return;
        }
        string detailimgurl = pic5 + "|" + pic6 + "|" + pic7;

        //更新到个人信息里面
        string sql = "UPDATE TCS_ShopConfig SET " +
               "leftimgurl = '" + leftimgurl + "', " +
               "detailimgurl = '" + detailimgurl + "' " +
           "WHERE nick = '" + nick + "'";
        utils.ExecuteNonQuery(sql);
    }

    public string TaobaoUpload(string picurl, string picname)
    {
        string appkey = "12159997";
        string secret = "614e40bfdb96e9063031d1a9e56fbed5";

        TopXmlRestClient clientaa = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", appkey, secret);
        PictureUploadRequest request = new PictureUploadRequest();

        string filepath = Server.MapPath("images/" + picurl);

        request.Img = new FileItem(filepath, File.ReadAllBytes(filepath));
        request.ImageInputTitle = picurl;
        request.PictureCategoryId = 0;
        request.Title = picname;

        clientaa.PictureUpload(request, session);


        PictureGetRequest request1 = new PictureGetRequest();
        request1.Title = picname;
        string path = string.Empty;
        path = clientaa.PictureGet(request1, session).Content[0].PicturePath;

        return path;
    }


    protected void Button13_Click(object sender, EventArgs e)
    {
        UploadImg();


        Response.Write("<script>alert('重新上传图片成功！');window.location.href='html.aspx';</script>");
        Response.End();
        return;
    }

    /// <summary>
    /// 增加活动图片到店铺分类
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {
        string appkey = "12159997";
        string secret = "614e40bfdb96e9063031d1a9e56fbed5";
        string name = utils.NewRequest("left", utils.RequestType.Form);
        string istop = utils.NewRequest("leftimgistop", utils.RequestType.Form);
        int order = 0;


        //保存到数据库
        string sql = "UPDATE TCS_ShopConfig SET leftimgname = '" + name + "', leftimgistop = '" + istop + "' WHERE nick = '" + nick + "'";

        utils.ExecuteNonQuery(sql);

        //通过接口同步到淘宝
        TopXmlRestClient clientaa = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", appkey, secret);

        //左侧分类的图片位置默认在最下面,获取当前序列号最大的
        SellercatsListGetRequest request1 = new SellercatsListGetRequest();
        request1.Nick = nick;
        request1.Fields = "name,sort_order,parent_cid";
        PageList<SellerCat> cat = clientaa.SellercatsListGet(request1, session);
        if (cat.Content.Count == 0)
        {
            order = 1;
        }
        else
        {
            int max = 0;
            for (int i = 0; i < cat.Content.Count; i++)
            {
                if (cat.Content[i].ParentCid == 0 && cat.Content[i].SortOrder > max)
                {
                    max = cat.Content[i].SortOrder;
                }
            }
            order = max + 1;
        }

        //判断该店铺是否增加过该分类
        string isok = "0";
        string catid = string.Empty;
        for (int i = 0; i < cat.Content.Count; i++)
        {
            //Response.Write(cat.Content[i].Name + "<br>");

            if (cat.Content[i].Name == "好评有礼_特特营销")
            {
                isok = "1";
                catid = cat.Content[i].Cid.ToString();
                break;
            }
        }
        //Response.End();

        if (isok == "0")
        {
            IDictionary<string, string> param = new Dictionary<string, string>();
            param.Add("name", "好评有礼_特特营销");
            param.Add("pict_url", GetTaobaoImg(name, "left"));
            param.Add("sort_order", order.ToString());

            string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.sellercats.list.add", session, param);

            //Response.Write(result);
            //Response.End();

            ////添加到左侧分类
            //SellercatsListAddRequest request = new SellercatsListAddRequest();
            //request.Name = "好评有礼_特特营销";
            //request.PictUrl = GetTaobaoImg(name, "left");
            //request.SortOrder = order;
            //clientaa.SellercatsListAdd(request, session);
        }
        else
        {
            IDictionary<string, string> param = new Dictionary<string, string>();
            param.Add("cid", catid);
            param.Add("pict_url", GetTaobaoImg(name, "left"));

            string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.sellercats.list.update", session, param);

            ////更新分类图片
            //SellercatsListUpdateRequest request = new SellercatsListUpdateRequest();
            //request.Cid = int.Parse(catid);
            //request.PictUrl = GetTaobaoImg(name, "left");
            //clientaa.SellercatsListUpdate(request, session);
        }


        Response.Write("<script>alert('同步成功，如果是第一次同步左侧分类可能需要多等待一会才能看到，或者您可以在店铺装修里面发布一下即可！');window.location.href='html.aspx';</script>");
        Response.End();
        return;
    }

    private string GetTaobaoImg(string name, string position)
    {
        string imgurl = string.Empty;
        string taobaourl = string.Empty;
        string sql = "SELECT * FROM TCS_ShopConfig WHERE nick = '" + nick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            if (position == "left")
            {
                imgurl = dt.Rows[0]["leftimgurl"].ToString();
            }
            else
            {
                imgurl = dt.Rows[0]["detailimgurl"].ToString();
            }
            string[] imgArray = imgurl.Split('|');

            //根据位置获取对应图片
            switch (name)
            {
                //左侧图片
                case "left2":
                    taobaourl = imgArray[0];
                    break;
                case "left3":
                    taobaourl = imgArray[1];
                    break;
                case "left4":
                    taobaourl = imgArray[2];
                    break;
                case "left5":
                    taobaourl = imgArray[3];
                    break;
                //宝贝描述图片
                case "750a":
                    taobaourl = imgArray[0];
                    break;
                case "750b":
                    taobaourl = imgArray[1];
                    break;
                case "750c":
                    taobaourl = imgArray[2];
                    break;
            }

            return taobaourl;
        }
        else
        {
            Response.Write("请先到基本设置里面进行信息设置，<a href='setting.aspx'>点此进入</a>");
            Response.End();
            return "";
        }
    }

    ///// <summary>
    ///// 清除店铺活动图片
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    //protected void Button2_Click(object sender, EventArgs e)
    //{

    //}

    /// <summary>
    /// 增加活动图片到宝贝描述
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button3_Click(object sender, EventArgs e)
    {
        string appkey = "12159997";
        string secret = "614e40bfdb96e9063031d1a9e56fbed5";

        //保存到数据库
        string name = utils.NewRequest("detail", utils.RequestType.Form);
        string istop = utils.NewRequest("detailimgistop", utils.RequestType.Form);
        string sql = "UPDATE TCS_ShopConfig SET detailimgname = '" + name + "', detailimgistop = '" + istop + "' WHERE nick = '" + nick + "'";
        StringBuilder builder = new StringBuilder();

        utils.ExecuteNonQuery(sql);

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
                    string newcontent = CreateDesc(item.Desc, istop);

                    //更新宝贝描述
                    IDictionary<string, string> param = new Dictionary<string, string>();
                    param.Add("num_iid", product.Content[i].NumIid.ToString());
                    param.Add("desc", newcontent);
                    string resultpro = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.item.update", session, param);
                    builder.Append("宝贝ID：" + product.Content[i].NumIid.ToString() + "\r\n" + resultpro + "\r\n");
                }
                catch
                { }
            }

            if (product.Content.Count < 200)
            {
                break;
            }
        }

        File.WriteAllText(Server.MapPath("htmlLog/" + DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + nick + ".txt"), builder.ToString());

        Response.Write("<script>alert('同步成功！');window.location.href='html.aspx';</script>");
        Response.End();
        return;
    }

    /// <summary>
    /// 获取调整过的宝贝描述
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    private string CreateDesc(string desc, string istop)
    {
        string newdesc = string.Empty;
        string sql = "SELECT * FROM TCS_ShopConfig WHERE nick = '" + nick + "'";

        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            string taobaourl = GetTaobaoImg(dt.Rows[0]["detailimgname"].ToString(), "detail");

            if (!Regex.IsMatch(desc, @"<div>[\s]*<a name=""tetehaoping-start"">[\s]*</a>[\s]*</div>[\s]*<div>[\s]*<img src=""([\s\S]*)"">[\s]*</div>[\s]*<div>[\s]*<a name=""tetehaoping-end"">[\s]*</a>[\s]*</div>"))
            {
                if (istop == "1")
                {
                    newdesc = @"<div><a name=""tetehaoping-start""></a></div><div><img src=""" + taobaourl + @"""></div><div><a name=""tetehaoping-end""></a></div>" + desc;
                }
                else
                {
                    newdesc = desc + @"<div><a name=""tetehaoping-start""></a></div><div><img src=""" + taobaourl + @"""></div><div><a name=""tetehaoping-end""></a></div>";
                }
            }
            else
            {
                string blankdesc = string.Empty;
                blankdesc = CreateDescDel(desc);
                if (blankdesc != "")
                {
                    //如果已经添加过，则自动先清除后再添加
                    if (istop == "1")
                    {
                        newdesc = @"<div><a name=""tetehaoping-start""></a></div><div><img src=""" + taobaourl + @"""></div><div><a name=""tetehaoping-end""></a></div>" + blankdesc;
                    }
                    else
                    {
                        newdesc = blankdesc + @"<div><a name=""tetehaoping-start""></a></div><div><img src=""" + taobaourl + @"""></div><div><a name=""tetehaoping-end""></a></div>";
                    }
                }
            }

            return newdesc;
        }
        else
        {
            Response.Write("请先到基本设置里面进行信息设置，<a href='setting.aspx'>点此进入</a>");
            Response.End();
            return "";
        }
    }

    /// <summary>
    /// 清除宝贝描述里面的活动页面
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button4_Click(object sender, EventArgs e)
    {
        string appkey = "12159997";
        string secret = "614e40bfdb96e9063031d1a9e56fbed5";
        StringBuilder builder = new StringBuilder();
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
                    string newcontent = CreateDescDel(item.Desc);

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
                    builder.Append("宝贝ID：" + product.Content[i].NumIid.ToString() + "\r\n" + resultpro + "\r\n");
                }
                catch
                { }
            }

            if (product.Content.Count < 200)
            {
                break;
            }
        }

        File.WriteAllText(Server.MapPath("htmlLog/" + DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + nick + ".txt"), builder.ToString());

        Response.Write("<script>alert('清除成功！');window.location.href='html.aspx';</script>");
        Response.End();
    }


    /// <summary>
    /// 获取调整过的宝贝描述
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    private string CreateDescDel(string desc)
    {
        string newdesc = string.Empty;
        string sql = "SELECT * FROM TCS_ShopConfig WHERE nick = '" + nick + "'";

        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            string taobaourl = GetTaobaoImg(dt.Rows[0]["detailimgname"].ToString(), "detail");

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
        else
        {
            Response.Write("请先到基本设置里面进行信息设置，<a href='setting.aspx'>点此进入</a>");
            Response.End();
            return "";
        }
    }



    #region TOP API
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
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        req.Method = "POST";
        req.KeepAlive = true;
        req.Timeout = 300000;
        req.ContentType = "application/x-www-form-urlencoded;charset=gb2312";
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
    #endregion
}