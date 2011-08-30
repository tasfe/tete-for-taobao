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
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using System.IO;
using System.Text.RegularExpressions;

public partial class top_addtotaobao_3 : System.Web.UI.Page
{
    public string style = string.Empty;
    public string size = string.Empty;
    public string type = string.Empty;
    public string orderby = string.Empty;
    public string query = string.Empty;
    public string shopcat = string.Empty;
    public string name = string.Empty;
    public string items = string.Empty;

    public string nickid = string.Empty;
    public string nickidEncode = string.Empty;
    public string md5nick = string.Empty;
    public string tabletitle = string.Empty;

    public string width = string.Empty;
    public string height = string.Empty;

    public string id = string.Empty;
    public string ads = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        //try
        //{
            id = utils.NewRequest("id", Common.utils.RequestType.QueryString);

            if (id != "" && !utils.IsInt32(id))
            {
                Response.Write("非法参数1");
                Response.End();
                return;
            }

            style = utils.NewRequest("style", Common.utils.RequestType.Form);
            size = utils.NewRequest("size", Common.utils.RequestType.Form);
            type = utils.NewRequest("type", Common.utils.RequestType.Form);
            orderby = utils.NewRequest("orderby", Common.utils.RequestType.Form);
            query = utils.NewRequest("query", Common.utils.RequestType.Form);
            shopcat = utils.NewRequest("shopcat", Common.utils.RequestType.Form);
            name = utils.NewRequest("name", Common.utils.RequestType.Form);
            items = utils.NewRequest("items", Common.utils.RequestType.Form);
            ads = utils.NewRequest("ads", Common.utils.RequestType.Form);

            string missionid = string.Empty;
            string html = CreateGroupbuyHtml(id);
            if (html.Length == 0) 
            {
                return;
            }

            int itemcount = 0;

            string act = utils.NewRequest("act", Common.utils.RequestType.Form);
            if (act == "save" && NoRepeat(id, type))
            {
                //记录该任务
                missionid = RecordMission();

                TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", "12287381", "d3486dac8198ef01000e7bd4504601a4");
                //提交更新到淘宝商品上去
                if (type != "1")
                {
                    for (int j = 1; j <= 500; j++)
                    {
                        ItemsOnsaleGetRequest request = new ItemsOnsaleGetRequest();
                        request.Fields = "num_iid,title,price,pic_url";
                        request.PageSize = 200;
                        request.PageNo = j;

                        if (orderby == "new")
                        {
                            request.OrderBy = "list_time:desc";
                        }
                        else if (orderby == "sale")
                        {
                            request.OrderBy = "volume:desc";
                        }

                        if (shopcat != "0")
                        {
                            request.SellerCids = shopcat;
                        }

                        if (query != "0")
                        {
                            request.Q = query;
                        }

                        Cookie cookie = new Cookie();
                        string taobaoNick = cookie.getCookie("nick");
                        string session = cookie.getCookie("top_sessiongroupbuy");
                        PageList<Item> product = client.ItemsOnsaleGet(request, session);

                        for (int i = 0; i < product.Content.Count; i++)
                        {
                            RecordMissionDetail(id, missionid, product.Content[i].NumIid.ToString(), html);
                            itemcount++;
                        }

                        if (product.Content.Count < 200)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    string[] itemId = items.Split(',');

                    for (int i = 0; i < itemId.Length; i++)
                    {
                        RecordMissionDetail(id, missionid, itemId[i], html);
                        itemcount++;
                    }
                }


            }
        //}
        //catch { }


            //更新总数量
            string sql = "UPDATE TopMission SET total = '" + itemcount + "' WHERE id = " + missionid;
            utils.ExecuteNonQuery(sql);

            Response.Redirect("missionlist.aspx");
    }

    //判断是否有同类型任务进行中
    private bool NoRepeat(string id, string type)
    {
        string sql = "SELECT COUNT(*) FROM TopMission WHERE groupbuyid = " + id + " AND typ='write' AND isok = 0";
        string count = utils.ExecuteString(sql);

        if (count != "0")
        {
            Response.Write("<script>alert('创建任务失败，有同类型的任务正在执行中，请等待其完成后再创建新的任务！');window.location.href='missionlist.aspx';</script>");
            Response.End();
            return false;
        }

        return true;
    }

    private string CreateGroupbuyHtml(string id)
    {
        string str = string.Empty;
        string html = File.ReadAllText(Server.MapPath("tpl/style1.html"));
        string sql = "SELECT * FROM TopGroupBuy WHERE id = '" + id + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str = html;
            str = str.Replace("{name}", dt.Rows[0]["name"].ToString());
            str = str.Replace("{oldprice}", dt.Rows[0]["productprice"].ToString());
            str = str.Replace("{zhekou}", Math.Round((decimal.Parse(dt.Rows[0]["productprice"].ToString()) - decimal.Parse(dt.Rows[0]["zhekou"].ToString())) / decimal.Parse(dt.Rows[0]["productprice"].ToString()) * 10, 1).ToString());
            str = str.Replace("{leftprice}", (decimal.Parse(dt.Rows[0]["productprice"].ToString()) - decimal.Parse(dt.Rows[0]["zhekou"].ToString())).ToString().Split('.')[0]);
            str = str.Replace("{rightprice}", (decimal.Parse(dt.Rows[0]["productprice"].ToString()) - decimal.Parse(dt.Rows[0]["zhekou"].ToString())).ToString().Split('.')[1]);
            str = str.Replace("{newprice}", dt.Rows[0]["zhekou"].ToString());
            str = str.Replace("{buycount}", dt.Rows[0]["buycount"].ToString());
            str = str.Replace("{producturl}", dt.Rows[0]["producturl"].ToString());
            str = str.Replace("{productimg}", dt.Rows[0]["productimg"].ToString());
            str = str.Replace("{id}", id);
            str = str.Replace("'", "''");
        }

        //http://groupbuy.7fshop.com/top/groupbuy/images/pz.png // 商城,良品 

        //http://groupbuy.7fshop.com/top/groupbuy/images/bz.png
        //http://groupbuy.7fshop.com/top/groupbuy/images/bz.png
        return str;
    }

    /// <summary>
    /// 记录该任务的详细关联商品
    /// </summary>
    private void RecordMissionDetail(string groupbuyid, string missionid, string itemid, string html)
    {
        string sql = "INSERT INTO TopWriteContent (groupbuyid, missionid, itemid, html) VALUES ('" + groupbuyid + "', '" + missionid + "', '" + itemid + "', '" + html + "')";
        utils.ExecuteNonQuery(sql);

        return;
    }

    /// <summary>
    /// 记录该任务并返回任务ID
    /// </summary>
    /// <returns></returns>
    private string RecordMission()
    {
        Cookie cookie = new Cookie();
        string missionid = string.Empty;
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

        string sql = "INSERT INTO TopMission (typ, nick, groupbuyid) VALUES ('write', '" + taobaoNick + "', '" + id + "')";
        utils.ExecuteNonQuery(sql);

        sql = "SELECT TOP 1 ID FROM TopMission ORDER BY ID DESC";
        missionid = utils.ExecuteString(sql);

        //获取团购信息并更新
        sql = "SELECT name,productimg,productid FROM TopGroupBuy WHERE id = '" + id + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            sql = "UPDATE TopMission SET groupbuyname = '" + dt.Rows[0]["name"].ToString() + "',groupbuypic = '" + dt.Rows[0]["productimg"].ToString() + "',itemid = '" + dt.Rows[0]["productid"].ToString() + "' WHERE id = " + missionid;
            utils.ExecuteNonQuery(sql);
        }

        return missionid;
    }

    private string EncodeStr(string[] parmArray)
    {
        string newStr = string.Empty;
        for (int i = 0; i < parmArray.Length; i++)
        {
            if (i == 0)
            {
                newStr = parmArray[i];
            }
            else
            {
                newStr += "|" + parmArray[i];
            }
        }

        Rijndael_ encode = new Rijndael_("tetesoftstr");
        newStr = encode.Encrypt(newStr);
        newStr = HttpUtility.UrlEncode(newStr);
        return newStr;
    }

    /// <summary> 
    /// MD5 加密函数 
    /// </summary> 
    /// <param name="str"></param> 
    /// <param name="code"></param> 
    /// <returns></returns> 
    public static string MD5(string str)
    {
        return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").Substring(8, 16);
    }
}
