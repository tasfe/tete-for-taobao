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
using System.Text;
using System.Security.Cryptography;

public partial class top_containermicro : System.Web.UI.Page
{
    public string top_session = string.Empty;
    public string nick = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        /*
         * http://www.7fshop.com/top/container.aspx
         * ?top_appkey=12132145
         * &top_parameters=aWZyYW1lPTEmdHM9MTI4NTg3MTY5Nzg0OSZ2aWV3X21vZGU9ZnVsbCZ2aWV3X3dpZHRoPTAmdmlzaXRvcl9pZD0xNzMyMzIwMCZ2aXNpdG9yX25pY2s90ra2+cvmx+W35w==
         * &top_session=23200857b2aa0ca62d3d0d9c78a750df07300
         * &top_sign=52sdJ6lvJPeaBef7rwcoSw==
         * &agreement=true
         * &agreementsign=12132145-21431194-D1A5A27626CB119D69F0D5438423A99A
         * &y=13
         * &x=36*/
        //签名验证
        string top_appkey = "12166431";
        string top_parameters = utils.NewRequest("top_parameters", utils.RequestType.QueryString);
        top_session = utils.NewRequest("top_session", utils.RequestType.QueryString);
        string app_secret = "15904e4c7e32ba5be6b1139fc64eb7a5";
        string top_sign = utils.NewRequest("top_sign", utils.RequestType.QueryString);


        if (!Taobao.Top.Api.Util.TopUtils.VerifyTopResponse(top_parameters, top_session, top_sign, top_appkey, app_secret))
        {
            Response.Write("top签名验证不通过，请不要非法注入");
            Response.End();
            return;
        }
        //Response.Write("!!!通过");
        //Response.End();
        nick = Taobao.Top.Api.Util.TopUtils.DecodeTopParams(top_parameters)["visitor_nick"];
        if (nick == null || nick == "")
        {
            Response.Write("top签名验证不通过，请不要非法注入");
            Response.End();
            return;
        }

        //判断跳转
        GetData(nick);
    }

    private void GetData(string nick)
    {
        string session = top_session; 
        //string session = "23200d282b335fc82ee9466c363c14f7e1b03";

        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", "12166431", "15904e4c7e32ba5be6b1139fc64eb7a5");

        //通过获取当前会话客户的在销商品来获取用户NICK
        //ItemsOnsaleGetRequest request1 = new ItemsOnsaleGetRequest();
        //request1.PageSize = 1;
        //request1.Fields = "nick";
        //PageList<Item> item = client.ItemsOnsaleGet(request1, session);
        //if (item.Content.Count == 0)
        //{
        //    Response.Write("请您先在店铺里添加商品 <a href='http://i.taobao.com/my_taobao.htm'>我的淘宝</a>");
        //    Response.End();
        //    return;
        //}
        //else
        //{
        //    nick = item.Content[0].Nick;
        //}
        
        //获取店铺基本信息
        UserGetRequest request = new UserGetRequest();
        request.Fields = "user_id,nick,seller_credit";
        request.Nick = nick;
        User user = client.UserGet(request, session);

        if(CheckUserExits(nick))
        {
            //更新该会员的店铺信息
            //记录2次登录日志
            string sql = "INSERT INTO TopLoginLog (" +
                           "nick " +
                       " ) VALUES ( " +
                           " '" + nick + "'" +
                     ") ";
            utils.ExecuteNonQuery(sql);

            //更新登录次数和最近登陆时间
            sql = "UPDATE toptaobaoshop SET logintimes = logintimes + 1,lastlogin = GETDATE(),session='" + top_session + "' WHERE nick = '" + nick + "'";
            utils.ExecuteNonQuery(sql);
        }
        else
        {
            //记录该会员的店铺信息
            InsertUserInfo(nick);
        }
        
        //加密NICK
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Encrypt(nick);

        Cookie cookie = new Cookie();
        cookie.setCookie("top_sessionmicro", top_session, 999999);
        cookie.setCookie("nick", nick, 999999);

        Response.Redirect("indexmicro.html");
    }


    /// <summary>
    /// 记录该会员的店铺信息
    /// </summary>
    private void InsertUserInfo(string nick)
    {
        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", "12166431", "15904e4c7e32ba5be6b1139fc64eb7a5");
        //记录店铺基本信息
        ShopGetRequest request = new ShopGetRequest();
        request.Fields = "sid,cid,title,nick,desc,bulletin,pic_path,created,modified";
        request.Nick = nick;
        Shop shop;
        try
        {
            shop = client.ShopGet(request);
        }
        catch
        {
            Response.Write("没有店铺的淘宝会员不可使用该应用，如果您想继续使用，请先去淘宝网开个属于您自己的店铺！<br> <a href='http://www.taobao.com/'>返回</a>");
            Response.End();
            return;
        }
        //记录到本地数据库
        string sql = "INSERT INTO TopTaobaoShop (" +
                        "sid, " +
                        "cid, " +
                        "title, " +
                        "nick, " +
                        "[desc], " +
                        "bulletin, " +
                        "pic_path, " +
                        "created, " +
                        "modified, " +
                        "shop_score, " +
                        "remain_count " +
                    " ) VALUES ( " +
                        " '" + shop.Sid + "', " +
                        " '" + shop.Cid + "', " +
                        " '" + shop.Title + "', " +
                        " '" + shop.Nick + "', " +
                        " '" + shop.Desc + "', " +
                        " '" + shop.Bulletin + "', " +
                        " '" + shop.PicPath + "', " +
                        " '" + shop.Created + "', " +
                        " '" + shop.Modified + "', " +
                        " '" + shop.ShopScore + "', " +
                        " '" + shop.RemainCount + "' " +
                  ") ";

        utils.ExecuteNonQuery(sql);
        //记录店铺分类信息
        SellercatsListGetRequest request1 = new SellercatsListGetRequest();
        request1.Fields = "cid,parent_cid,name,is_parent";
        request1.Nick = nick;
        PageList<SellerCat> cat = client.SellercatsListGet(request1);

        for (int i = 0; i < cat.Content.Count; i++)
        {
            sql = "INSERT INTO TopTaobaoShopCat (" +
                            "cid, " +
                            "parent_cid, " +
                            "name, " +
                            "pic_url, " +
                            "sort_order, " +
                            "created, " +
                            "nick, " +
                            "modified " +
                        " ) VALUES ( " +
                            " '" + cat.Content[i].Cid + "', " +
                            " '" + cat.Content[i].ParentCid + "', " +
                            " '" + cat.Content[i].Name + "', " +
                            " '" + cat.Content[i].PicUrl + "', " +
                            " '" + cat.Content[i].SortOrder + "', " +
                            " '" + cat.Content[i].Created + "', " +
                            " '" + nick + "', " +
                            " '" + cat.Content[i].Modified + "' " +
                      ") ";
            utils.ExecuteNonQuery(sql);
        }

        //记录店铺所有商品信息-暂不记录
    }

    /// <summary>
    /// 判断该TAOBAO会员的店铺是否有记录
    /// </summary>
    /// <param name="nick"></param>
    /// <returns></returns>
    private bool CheckUserExits(string nick)
    {
        string sql = "SELECT * FROM TopTaobaoShop WHERE nick = '" + nick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count == 0)
        {
            return false;
        }
        else
        { 
            return true;
        }
    }
}
