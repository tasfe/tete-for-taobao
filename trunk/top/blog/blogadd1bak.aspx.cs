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
using System.IO;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;

public partial class top_blog_blogadd1bak : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string url = utils.NewRequest("title", utils.RequestType.Form);
        string title = utils.NewRequest("t", utils.RequestType.Form);
        //<div id="sina_keyword_ad_area2" class="articalContent">  <div class="shareUp">


        string strHtml = "";//getUrl(url, "utf-8");
        //<div id="sina_keyword_ad_area2" class="articalContent">  <div class="shareUp">

        Regex reg = new Regex(@"<div id=""sina_keyword_ad_area2"" class=""[^""]*"">([\s\S]*?)</div>[\s]*<!-- 正文结束 -->", RegexOptions.IgnoreCase);
        MatchCollection match = reg.Matches(strHtml);

        //替换博客里面的图片
        string str = "";

        //替换掉博客内容里面的连接
        Regex regLink = new Regex(@"<a[^>]*>([\s\S]*?)</a>", RegexOptions.IgnoreCase);
        str = regLink.Replace(str, "$1");

        this.tbTitle.Text = title;
        this.FCKeditor1.Value = str;

        BindData();
    }

    private void BindData()
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        string session = cookie.getCookie("top_sessionblog");

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        //获取用户店铺商品列表
        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", "12159997", "614e40bfdb96e9063031d1a9e56fbed5");
        ItemsOnsaleGetRequest request = new ItemsOnsaleGetRequest();
        request.Fields = "num_iid,title,price,pic_url";
        request.Q = tbKey.Text;
        request.PageSize = 5;
        request.OrderBy = "volume:desc";

        PageList<Item> product = new PageList<Item>();

        try
        {
            product = client.ItemsOnsaleGet(request, session);
        }
        catch (Exception e)
        {
            if (e.Message == "27:Invalid session:Session not exist")
            {
                //SESSION超期 跳转到登录页
                Response.Write("<script>parent.location.href='http://container.open.taobao.com/container?appkey=12159997'</script>");
                Response.End();
            }
            return;
        }
        rptProduct.DataSource = product.Content;
        rptProduct.DataBind();

        //数据绑定
        string sqlNew = "SELECT TOP 10 * FROM TopIdea WHERE nick = '" + taobaoNick + "' ORDER BY id DESC";
        DataTable dtNew = utils.ExecuteDataTable(sqlNew);

        rptAds.DataSource = dtNew;
        rptAds.DataBind();

        //数据绑定
        sqlNew = "SELECT * FROM TopBlogLink WHERE nick = '" + taobaoNick + "' ORDER BY id DESC";
        dtNew = utils.ExecuteDataTable(sqlNew);

        rptLink.DataSource = dtNew;
        rptLink.DataBind();
    }

    private string getUrl(string url, string codeStr)
    {
        //准备生成
        string strHtml = string.Empty;
        StreamReader sr = null; //用来读取流
        Encoding code = Encoding.GetEncoding(codeStr); //定义编码 
        ASCIIEncoding encoding = new ASCIIEncoding();
        //构造web请求，发送请求，获取响应
        WebRequest HttpWebRequest = null;
        HttpWebRequest = WebRequest.Create(url);
        //发送
        WebResponse HttpWebResponse = null;
        HttpWebResponse = HttpWebRequest.GetResponse();
        //获得流
        sr = new StreamReader(HttpWebResponse.GetResponseStream(), code);
        strHtml = sr.ReadToEnd();

        return strHtml;
    }
}