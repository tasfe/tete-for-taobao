using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using PaiPai.Model;
using PaiPai.DAL;

public partial class qqcontainer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string selluin = Request.QueryString["useruin"];
            string accesstoken = Request.QueryString["access_token"];
            if (string.IsNullOrEmpty(selluin) || string.IsNullOrEmpty(accesstoken))
            {
                Response.Write("数据不合法");
                return;
            }

            PaiPaiShopInfo info = PaiPaiTopAPI.GetShopInfo(selluin, accesstoken);
            if (info == null)
            {
                Response.Write("未找到店铺信息");
                return;
            }
            PaiPaiShopService ppsDal = new PaiPaiShopService();
            info.logintimes = 1;
            info.lastlogin = DateTime.Now;
            info.BanBen = 0;
            info.MessgeCount = 10;  //暂时购买就送10条
            info.HadPost = 0;
            info.ExpiredDays = 8; //默认过期未评的时间为8天
            info.NotPayExpiredMinutes = 60; //默认多久未付款发短信

            //发送短信模板
            info.NotPay = true;
            info.NotPayPostModel = "亲，您在【{ShopName}】拍下的宝贝还没付款哦，宝贝我们一直为您留着，快来把宝贝领回家吧。";
            info.PostGoods = true;
            info.PostGoodsPostModel = "【{ShopName}】:亲,您购买的宝贝已发出,{ExpressName}+{ExpressNo},请注意查收！如有问题请及时联系我们。";
            info.NotPing = true;
            info.NotPingPostModel = "【{ShopName}】:亲，您的宝贝已经显示签收，如果对商品满意请帮忙评价下，非常感谢。";

            info.AcessToken = accesstoken;
            if (ppsDal.SelectShopBySellerUin(info.sellerUin))
                ppsDal.UpdateShopInfo(info);
            else
                ppsDal.InsertShop(info);

            //添加购买信息
            string sql = "SELECT * FROM [BangT_Buys] WHERE nick = '" + selluin + "'";
            DataTable dingdt =  DataHelp.DBHelper.ExecuteDataTable(sql);

            if (dingdt.Rows.Count > 0)
            {

            }
            else
            {
                //插入
                sql = "INSERT INTO BangT_Buys ([Nick],[FeeId],[BuyTime],[IsExpied],ExpiedTime) VALUES ('" + selluin + "','21D5A8DB-F363-4679-BB64-0EA58D5B9F3D',GETDATE(),0,'" + DateTime.Now.AddMonths(1).ToShortDateString() + "')";
                DataHelp.DBHelper.ExecuteNonQuery(sql);
            }

            AddCookie(selluin, accesstoken);

            Response.Redirect("index.html");
        }
    }

    private void AddCookie(string sellerUin, string accesstoken)
    {
        HttpCookie cookie = new HttpCookie("nick", HttpUtility.UrlEncode(sellerUin));
        HttpCookie cooksession = new HttpCookie("top_session", accesstoken);
        cookie.Expires = DateTime.Now.AddDays(1);
        cooksession.Expires = DateTime.Now.AddDays(1);

        Response.Cookies.Add(cookie);
        Response.Cookies.Add(cooksession);

        Session["snick"] = sellerUin;
        Session["top_session"] = accesstoken;
    }
}
