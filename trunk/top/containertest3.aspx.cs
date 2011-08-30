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

public partial class top_containertest2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //GetData();
    }

    private void GetData()
    {
        //string session = utils.NewRequest("top_session", utils.RequestType.QueryString);
        ////string session = "23200d282b335fc82ee9466c363c14f7e1b03";
        //string nick = string.Empty;

        //TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", "12132145", "1fdd2aadd5e2ac2909db2967cbb71e7f");

        ////通过获取当前会话客户的在销商品来获取用户NICK
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
        
        ////获取店铺基本信息
        //UserGetRequest request = new UserGetRequest();
        //request.Fields = "user_id,nick,seller_credit";
        //request.Nick = nick;
        //User user = client.UserGet(request, session);

        //if(CheckUserExits(nick))
        //{
        //    //更新该会员的店铺信息
            
        //}
        //else
        //{
        //    //记录该会员的店铺信息
        //    InsertUserInfo(nick);
        //}

        string nick = string.Empty;
        string session = string.Empty;

        nick = Request.QueryString["nick"]; //utils.NewRequest("nick", utils.RequestType.QueryString);

        Response.Write(nick);
        Response.End();

        string sql = "SELECT * FROM TopTaobaoShop WHERE nick = '" + nick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            session = dt.Rows[0]["sessiongroupbuy"].ToString();   
        }
        
        //加密NICK
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Encrypt(nick);

        Cookie cookie = new Cookie();
        cookie.setCookie("top_sessiongroupbuy", session, 999999);
        cookie.setCookie("nick", nick, 999999);

        Response.Redirect("indexgroup.html");
    }

    /// <summary>
    /// 记录该会员的店铺信息
    /// </summary>
    private void InsertUserInfo(string nick)
    {
        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", "12287381", "d3486dac8198ef01000e7bd4504601a4");
        //记录店铺基本信息
        ShopGetRequest request = new ShopGetRequest();
        request.Fields = "sid,cid,title,nick,desc,bulletin,pic_path,created,modified";
        request.Nick = nick;
        Shop shop = client.ShopGet(request);
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

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (this.TextBox2.Text != "terry0077")
        {
            return;
        }

        string nick = string.Empty;
        string session = string.Empty;

        nick = this.TextBox1.Text; //utils.NewRequest("nick", utils.RequestType.QueryString);

        string sql = "SELECT * FROM TopTaobaoShop WHERE nick = '" + nick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            session = dt.Rows[0]["sessiongroupbuy"].ToString();
        }

        //加密NICK
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Encrypt(nick);

        Cookie cookie = new Cookie();
        cookie.setCookie("top_sessiongroupbuy", session, 999999);
        cookie.setCookie("nick", nick, 999999);

        Response.Redirect("indexreview.html");
    }
}
