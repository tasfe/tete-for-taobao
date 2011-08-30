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
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;
using System.Collections.Generic;
using Common;
using Microsoft.Practices.EnterpriseLibrary.Caching;

public partial class show_plist : System.Web.UI.Page
{
    public string title = string.Empty;
    public string id = string.Empty;

    public string width = string.Empty;
    public string height = string.Empty;
    public string num = string.Empty;
    public string url = string.Empty;
    public string nickid = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        //TopXmlRestClient client = new TopXmlRestClient("http://gw.api.tbsandbox.com/router/rest", "test", "test");
        id = utils.NewRequest("id", utils.RequestType.QueryString);

        string itemid = utils.NewRequest("itemid", utils.RequestType.QueryString);
        string ideaid = utils.NewRequest("idea", utils.RequestType.QueryString);
        string act = utils.NewRequest("act", utils.RequestType.QueryString);
        url = utils.NewRequest("url", utils.RequestType.QueryString);

        width = utils.NewRequest("width", utils.RequestType.QueryString);
        height = utils.NewRequest("height", utils.RequestType.QueryString);
        num = utils.NewRequest("num", utils.RequestType.QueryString);
        string size = string.Empty;

        if (id != "" && !utils.IsInt32(id))
        {
            Response.Write("非法参数1");
            Response.End();
            return;
        }

        if (num != "" && !utils.IsInt32(id))
        {
            Response.Write("非法参数0");
            Response.End();
            return;
        }

        if (itemid != "" && !IsLong(itemid))
        {
            Response.Write("非法参数2");
            Response.End();
            return;
        }

        if (ideaid != "" && !utils.IsInt32(ideaid))
        {
            Response.Write("非法参数3");
            Response.End();
            return;
        }

        string sql = string.Empty;

        //只有正式模式下才统计相关信息
        if (id != "0")
        {
            if (act == "hit")
            {
                return;
                //记录广告点击
                sql = "UPDATE TopIdea SET hitcount = hitcount + 1 WHERE id = " + ideaid;
                utils.ExecuteNonQuery(sql);
                //记录点击日志
                sql = "INSERT INTO TopIdeaHitLog (ideaid, itemid, url) VALUES ('" + ideaid + "', '" + itemid + "', '" + url + "')";
                utils.ExecuteNonQuery(sql);
                Response.End();
                return;
            }
            else // if (act == "view")
            {
                //记录广告浏览次数
                sql = "UPDATE TopIdea SET viewcount = viewcount + 1 WHERE id = " + id;
                utils.ExecuteNonQuery(sql);
                //记录浏览日志
                sql = "INSERT INTO TopIdeaLog (ideaid, url) VALUES ('" + id + "', '" + url + "')";
                utils.ExecuteNonQuery(sql);
                //Response.End();
                //return;
            }
        }
        //开启缓存
        CacheManager testcaching1 = CacheFactory.GetCacheManager();
        if (testcaching1.Contains("cache_1_" + id) && id != "3972")
        {
            Response.Write(testcaching1.GetData("cache_1_" + id));
            Response.End();
            return;
        }


        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", "12132145", "1fdd2aadd5e2ac2909db2967cbb71e7f");

        //如果为0则为调试模式，自动按照参数进行调用
        if (id == "0")
        {
            Cookie cookie1 = new Cookie();
            string taobaoNick1 = cookie1.getCookie("nick");

            //COOKIE过期判断
            if (taobaoNick1 == "")
            {
                //SESSION超期 跳转到登录页
                Response.Write("<script>parent.location.href='http://container.open.taobao.com/container?appkey=12132145'</script>");
                Response.End();
            }
            Rijndael_ encode = new Rijndael_("tetesoft");
            taobaoNick1 = encode.Decrypt(taobaoNick1);
            string sqlNew = "SELECT sid FROM TopTaobaoShop WHERE nick = '" + taobaoNick1 + "'";
            DataTable dtNew = utils.ExecuteDataTable(sqlNew);
            if (dtNew.Rows.Count != 0)
            {
                nickid = "http://shop" + dtNew.Rows[0]["sid"].ToString() + ".taobao.com/";
            }
            else
            {
                nickid = "http://www.taobao.com/";
            }



            width = (int.Parse(width) - 10).ToString();

            Cookie cookie = new Cookie();
            string taobaoNick = cookie.getCookie("nick");
            string session = cookie.getCookie("top_session");

            string style = utils.NewRequest("style", utils.RequestType.QueryString);
            size = utils.NewRequest("size", utils.RequestType.QueryString);
            string type = utils.NewRequest("type", utils.RequestType.QueryString);
            string orderby = utils.NewRequest("orderby", utils.RequestType.QueryString);
            string query = utils.NewRequest("query", utils.RequestType.QueryString);
            string shopcat = utils.NewRequest("shopcat", utils.RequestType.QueryString);
            string items = utils.NewRequest("items", utils.RequestType.QueryString);

            title = utils.NewRequest("title", utils.RequestType.QueryString);

            //如果为自动选择模式
            if (type == "0")
            {
                //如果没有选择具体商品
                ItemsOnsaleGetRequest request = new ItemsOnsaleGetRequest();
                request.Fields = "num_iid,title,price,pic_url";
                request.PageSize = int.Parse(num);
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
                try
                {
                    PageList<Item> product = client.ItemsOnsaleGet(request, session);

                    if (size == "743*308")
                    {
                        panel1.Visible = false;
                        panel2.Visible = true;
                        Repeater2.DataSource = product.Content;
                        Repeater2.DataBind();
                    }
                    else
                    {
                        Repeater1.DataSource = product.Content;
                        Repeater1.DataBind();
                    }
                }
                catch
                {
                    Response.Write(session + "- miss session!!");
                    Response.End();
                }
            }
            //如果选择了具体商品
            else
            {
                if(items == "")
                    return;
                string[] arr = items.Split(',');
                List<Item> itemList = new List<Item>();

                //过滤可能发生的错误
                try
                {
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (i >= int.Parse(num))
                            break;

                        ItemGetRequest request = new ItemGetRequest();
                        request.Fields = "num_iid,title,price,pic_url";
                        request.NumIid = long.Parse(arr[i]);

                        Item product = client.ItemGet(request);
                        itemList.Add(product);
                    }
                }
                catch
                { }

                if (size == "743*308")
                {
                    panel1.Visible = false;
                    panel2.Visible = true;
                    Repeater2.DataSource = itemList;
                    Repeater2.DataBind();
                }
                else
                {
                    Repeater1.DataSource = itemList;
                    Repeater1.DataBind();
                }
            }

            return;
        }

        //获取广告标题
        sql = "SELECT name,size FROM TopIdea WHERE id = " + id;
        DataTable dt = utils.ExecuteDataTable(sql);
        if(dt.Rows.Count != 0)
        {
            title = dt.Rows[0]["name"].ToString();
            size = dt.Rows[0]["size"].ToString();

            string[] arr = size.Split('*');

            width = arr[0];
            height = arr[1];

            switch (size)
            {
                case "514*160":
                    num = "5";
                    break;
                case "514*288":
                    num = "10";
                    break;
                case "664*160":
                    num = "6";
                    break;
                case "312*288":
                    num = "6";
                    break;
                case "336*280":
                    num = "4";
                    break;
                case "714*160":
                    num = "7";
                    break;
                case "114*418":
                    num = "3";
                    break;
                case "218*286":
                    num = "4";
                    break;
                case "743*308":
                    num = "4";
                    break;
                default:
                    num = "4";
                    break;
            }
        }

        //获取数据库中保存的商品
        sql = "SELECT TOP " + num + " * FROM TopIdeaProduct WHERE ideaid = " + id + "";// ORDER BY NEWID()";
        dt = utils.ExecuteDataTable(sql);

        test.DataSource = dt;
        test.DataBind();

        //获取店铺地址
        sql = "SELECT name,size,nick FROM TopIdea WHERE id = " + id;
        DataTable dt22 = utils.ExecuteDataTable(sql);
        if (dt22.Rows.Count != 0)
        {
            string taobaoNick = dt22.Rows[0]["nick"].ToString();
            string sqlNew = "SELECT sid FROM TopTaobaoShop WHERE nick = '" + taobaoNick + "'";
            DataTable dtNew = utils.ExecuteDataTable(sqlNew);
            if (dtNew.Rows.Count != 0)
            {
                nickid = "http://shop" + dtNew.Rows[0]["sid"].ToString() + ".taobao.com/";
            }
            else
            {
                nickid = "http://www.taobao.com/";
            }
        }


        //生成HTML缓存
        string cache = string.Empty;
        if (size == "743*308")
        {
            cache += "<table background=\"/top/show1/4.gif\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" height=\"30\" style=\"border-right: #999999 1px solid; border-top: #999999 1px solid;border-left: #999999 1px solid; border-bottom: #999999 1px solid\" width=\"740\"><tr><td>    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"><tr><td align=\"left\"><table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" height=\"30\"><tr><td width=\"10\"></td><td background=\"/top/show1/1.gif\" width=\"24\"></td><td background=\"/top/show1/2.gif\"><font color=\"white\" style=\"font-size: 13px\"><strong>" + title + "</strong></font></td><td><img src=\"/top/show1/3.gif\" /></td></tr></table></td><td align=\"right\"></td></tr></table></td></tr></table>  <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-right: #999999 1px solid;border-top: #999999 1px solid; overflow: hidden; border-left: #999999 1px solid;border-bottom: #999999 1px solid\" width=\"740\"><tr><td valign=\"top\"><TABLE cellSpacing=0 cellPadding=0 width=730 border=0><TBODY><TR><TD align=\"middle\"><TABLE cellSpacing=8 cellPadding=0 align=center border=0><TBODY><TR>";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cache += "<TD vAlign=top align=\"middle\" width=175 bgColor=white><TABLE cellSpacing=0 cellPadding=0 align=center border=0><TBODY><TR><TD vAlign=top align=\"middle\" width=175 bgColor=white><TABLE cellSpacing=0 cellPadding=0 align=center border=0><TBODY><TR><TD align=\"middle\"><DIV style=\"BORDER-RIGHT: #cccccc 1px solid; BORDER-TOP: #cccccc 1px solid; MARGIN-TOP: 4px; BORDER-LEFT: #cccccc 1px solid; WIDTH: 160px; BORDER-BOTTOM: #cccccc 1px solid; HEIGHT: 160px\"><DIV style=\"OVERFLOW: hidden; WIDTH: 160px; HEIGHT: 160px\"><A href=\"/click/?s=" + EncodeStr(new string[] { id, dt.Rows[i]["itemid"].ToString(), "http://item.taobao.com/item.htm?id=" + dt.Rows[i]["itemid"].ToString() }) + "\" onclick=\"javascript:spreadStat('" + id + "','" + dt.Rows[i]["itemid"].ToString() + "');\" target=\"_blank\"><IMG alt=\"" + dt.Rows[i]["itemname"].ToString() + "\" src=\"" + dt.Rows[i]["itempicurl"].ToString() + "_160x160.jpg\" border=0 /></A></DIV></DIV></TD></TR><TR><TD align=\"middle\"><DIV style=\"PADDING-RIGHT: 4px; PADDING-LEFT: 4px; FONT-SIZE: 12px; PADDING-BOTTOM: 4px; PADDING-TOP: 4px\"><A style=\"FONT-SIZE: 12px; COLOR: #3f3f3f; TEXT-DECORATION: none\" href=\"/click/?s=" + EncodeStr(new string[] { id, dt.Rows[i]["itemid"].ToString(), "http://item.taobao.com/item.htm?id=" + dt.Rows[i]["itemid"].ToString() }) + "\" onclick=\"javascript:spreadStat('" + id + "','" + dt.Rows[i]["itemid"].ToString() + "');\" target=\"_blank\" title=\"" + dt.Rows[i]["itemname"].ToString() + "\">" + dt.Rows[i]["itemname"].ToString() + "</A><BR /><FONT style=\"COLOR: #fe596a\"><B>￥&nbsp;" + dt.Rows[i]["itemprice"].ToString() + "元</B></FONT> </DIV><A href=\"/click/?s=" + EncodeStr(new string[] { id, dt.Rows[i]["itemid"].ToString(), "http://item.taobao.com/item.htm?id=" + dt.Rows[i]["itemid"].ToString() }) + "\" onclick=\"javascript:spreadStat('" + id + "','" + dt.Rows[i]["itemid"].ToString() + "');\" target=\"_blank\"><IMG src=\"/top/show1/buy1.gif\" border=0 /></A> <DIV></DIV></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE></TD>";
            }

            cache += "</TR></TBODY></TABLE></td></tr><tr><td align=\"right\" height=\"24\" style=\"border-bottom: #999999 1px solid\" valign=\"center\"><a href=\"/click/?s=" + EncodeStr(new string[] { id, "0", nickid }) + "\" style=\"text-decoration: none\" target=\"_blank\"><font style=\"font-size: 13px; color: #ff6600\"><strong>更多详情请见 " + nickid + "</strong>&nbsp;</font></a></td></tr></table>";
            cache += "<script type=\"text/javascript\" language=\"javascript\" src=\"css/common.js\"></script>";
            cache += "<script type=\"text/javascript\">";
            cache += "var xmlHttp;";
            cache += "var url = '" + url + "';";
            cache += "if(\"0\" != \""+id+"\" && url != \"\"){";
            cache += "createxmlHttpRequest();";
            cache += "var queryString = \"http://www.7fshop.com/show/plist.aspx?act=view&id=" + id + "&url=\"+escape(url)+\"&t=\"+new Date().getTime();";
            cache += "xmlHttp.open(\"GET\",queryString);";
            cache += "xmlHttp.send(null);  }";
            cache += "</script>";
        }
        else
        {
            cache = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">";
            cache += "<html xmlns=\"http://www.w3.org/1999/xhtml\">";
            cache += "<head>";
            cache += "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />";
            cache += "<title>特特推广</title>";
            cache += "<link type=\"text/css\" href=\"css/css.css\" rel=\"stylesheet\"/>";
            cache += "</head>";
            cache += "<body>";
            cache += "<div id=\"container\" style=\"width:" + width + "px; height:" + height + "px; overflow:hidden\">";
            cache += "<div class=\"navigation\">" + title + "</div>";
            cache += "<div class=\"outer\">";
            cache += "<div class=\"inner\">";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cache += "<dl>";
                cache += "<dt><a href='/click/?s=" + EncodeStr(new string[] { id, dt.Rows[i]["itemid"].ToString(), "http://item.taobao.com/item.htm?id=" + dt.Rows[i]["itemid"].ToString() }) + "' title='" + dt.Rows[i]["itemname"].ToString() + "' onclick='javascript:spreadStat('" + id + "','" + dt.Rows[i]["itemid"].ToString() + "');' target='_blank'><img src='" + dt.Rows[i]["itempicurl"].ToString() + "_80x80.jpg' border='0' /></a></dt>";
                cache += "<dd><a href='/click/?s=" + EncodeStr(new string[] { id, dt.Rows[i]["itemid"].ToString(), "http://item.taobao.com/item.htm?id=" + dt.Rows[i]["itemid"].ToString() }) + "' onclick='javascript:spreadStat('" + id + "','" + dt.Rows[i]["itemid"].ToString() + "');' target='_blank'>" + left(dt.Rows[i]["itemname"].ToString(), 16) + "</a></dd>";
                cache += "</dl>";
            }
            cache += "<br class=\"clearfloat\"/>";
            cache += "</div>";
            cache += "</div>";
            cache += "</div>";

            cache += "<script type=\"text/javascript\" language=\"javascript\" src=\"css/common.js\"></script>";
            cache += "<script type=\"text/javascript\">";
            cache += "var xmlHttp;";
            cache += "var url = '" + url + "';";
            cache += "if(\"0\" != \"" + id + "\" && url != \"\"){";
            cache += "createxmlHttpRequest();";
            cache += "var queryString = \"http://www.7fshop.com/show/plist.aspx?act=view&id=" + id + "&url=\"+escape(url)+\"&t=\"+new Date().getTime();";
            cache += "xmlHttp.open(\"GET\",queryString);";
            cache += "xmlHttp.send(null);  }";
            cache += "</script>";
        }

        CacheManager testcaching = CacheFactory.GetCacheManager();
        if (!testcaching.Contains("cache_1_" + id))
        {
            testcaching.Add("cache_1_" + id, cache);
        }

        //如果是743*308，则直接显示
        Response.Write(cache);
        Response.End();
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

    public static bool IsLong(string strText)
    {
        try
        {
            double intTest = Convert.ToDouble(strText);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static string left(string str, int len)
    {
        if (str.Length < len)
        {
            return str;
        }
        else
        {
            return str.Substring(0, len);
        }
    }
}

