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
using Microsoft.Practices.EnterpriseLibrary.Data;
using Common;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using System.IO;
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;

public partial class top_market_idealist : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BindData();
    }

    private void BindData()
    {
        //获取用户信息
        Cookie cookie = new Cookie();
        string taobaoNick = cookie.getCookie("nick");
        string session = cookie.getCookie("top_session");

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        //判断是否删除
        string act = utils.NewRequest("act", utils.RequestType.QueryString);
        string id = utils.NewRequest("id", utils.RequestType.QueryString);

        if(id != "" && !utils.IsInt32(id))
        {
            Response.Write("非法参数");
            Response.End();
            return;
        }

        if (act == "del")
        {
            string sql = "DELETE FROM TopIdea WHERE nick = '" + taobaoNick + "' AND id = " + id;
            utils.ExecuteNonQuery(sql);

            Response.Redirect("idealist.aspx");
            return;
        }
        else if(act == "update")
        {
            //如果是自动更新模式，需要更新店铺商品数据
            string sql = "SELECT shopcategoryid,query,nick,showtype FROM TopIdea WHERE nick = '" + taobaoNick + "' AND id = " + id;
            DataTable dt1 = utils.ExecuteDataTable(sql);
            string shopcat = string.Empty;
            string query = string.Empty;
            if(dt1.Rows[0]["showtype"].ToString() == "0")
            {
                //更新下架商品数据
                shopcat = dt1.Rows[0][0].ToString();
                query = dt1.Rows[0][1].ToString();
                taobaoNick = dt1.Rows[0][2].ToString();

                //获取新商品列表
                TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", "12132145", "1fdd2aadd5e2ac2909db2967cbb71e7f");
                ItemsOnsaleGetRequest request = new ItemsOnsaleGetRequest();
                request.Fields = "num_iid,title,price,pic_url";
                request.PageSize = 12;
                request.OrderBy = "list_time:desc";
                request.OrderBy = "volume:desc";
                if (shopcat != "0")
                {
                    request.SellerCids = shopcat;
                }
                request.Q = query;

                //清理关联商品
                sql = "DELETE FROM TopIdeaProduct WHERE ideaid = " + id;
                utils.ExecuteNonQuery(sql);

                //未登录用户不能获取小二下架或删除的商品-错误过滤，原因未知
                try
                {
                    PageList<Item> product = client.ItemsOnsaleGet(request, session);

                    for (int i = 0; i < product.Content.Count; i++)
                    {
                        sql = "INSERT INTO TopIdeaProduct (" +
                                    "itemid, " +
                                    "itemname, " +
                                    "itemprice, " +
                                    "itempicurl, " +
                                    "ideaid " +
                                " ) VALUES ( " +
                                    " '" + product.Content[i].NumIid + "', " +
                                    " '" + product.Content[i].Title + "', " +
                                    " '" + product.Content[i].Price + "', " +
                                    " '" + product.Content[i].PicUrl + "', " +
                                    " '" + id + "' " +
                              ") ";
                        utils.ExecuteNonQuery(sql);
                    }
                }
                catch { }
            }

            //清理广告缓存
            CacheManager testcaching1 = CacheFactory.GetCacheManager();
            if (testcaching1.Contains("cache_1_" + id))
            {
                testcaching1.Remove("cache_1_" + id);
            }

            //更新广告图片
            string folderPath = Server.MapPath("\\show\\folder\\" + MD5(taobaoNick) + "\\result_" + id + ".jpg");
            if (File.Exists(folderPath))
            {
                File.Delete(folderPath);
            }
            
            //Response.Write("推广更新成功！！<br> <a href='../../show/plist.aspx?id=" + id + "' target='_blank'>查看更新过的广告</a> <br> <a href='idealist.aspx'>返回</a>");
            Response.Redirect("success.aspx?id=" + id);
            return;
        }

        DataTable dt = utils.ExecuteDataTable("SELECT * FROM TopIdea WHERE nick = '" + taobaoNick + "' ORDER BY id DESC");

        rptIdeaList.DataSource = dt;
        rptIdeaList.DataBind();
    }

    public static string MD5(string str)
    {
        return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").Substring(8, 16);
    }
}
