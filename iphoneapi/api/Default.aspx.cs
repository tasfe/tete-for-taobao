using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;

public partial class api_Default : System.Web.UI.Page
{
    private string act = string.Empty;
    private string uid = string.Empty;
    private string cid = string.Empty;
    private string itemid = string.Empty;
    private string page = string.Empty;
    private string pagesize = string.Empty;
    private string direct = string.Empty;
    private string token = string.Empty;
    private string err = string.Empty;
    private string typ = string.Empty;
    private string msgid = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            act = utils.NewRequest("act", utils.RequestType.QueryString);
            uid = utils.NewRequest("uid", utils.RequestType.QueryString);   
            cid = utils.NewRequest("cid", utils.RequestType.QueryString);
            itemid = utils.NewRequest("itemid", utils.RequestType.QueryString);
            page = utils.NewRequest("page", utils.RequestType.QueryString);
            pagesize = utils.NewRequest("pagesize", utils.RequestType.QueryString);
            direct = utils.NewRequest("direct", utils.RequestType.QueryString);
            token = utils.NewRequest("token", utils.RequestType.QueryString);
            typ = utils.NewRequest("typ", utils.RequestType.QueryString);
            msgid = utils.NewRequest("msgid", utils.RequestType.QueryString);

            err = utils.NewRequest("err", utils.RequestType.Form);

            switch (act)
            {
                case "ads":
                    ShowAdsInfo();
                    break;
                case "cate":
                    ShowCateInfo();
                    break;
                case "list":
                    ShowListInfo();
                    break;
                case "special":
                    ShowSpecialListInfo();
                    break;
                case "detail":
                    ShowDetailInfo();
                    break;
                case "near":
                    ShowNearInfo();
                    break;
                case "token":
                    RecordTokenInfo();
                    break;
                case "err":
                    RecordErrInfo();
                    break;
                case "msgcount":
                    ShowMsgCountInfo();
                    break;
                case "msglist":
                    ShowMsgListInfo();
                    break;
                case "msgdetail":
                    ShowMsgDetailInfo();
                    break;
            }
        }
        catch
        {
            string str = "{\"error_response\":\"service_error\"}";
            Response.Write(str);
        }
    }

    private void ShowMsgDetailInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;

        sql = "SELECT * FROM TeteUserMsg WHERE nick = '" + uid + "' AND id = " + msgid + "";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str += "{\"id\":\"" + dt.Rows[0]["id"].ToString() + "\",\"html\":\"" + dt.Rows[0]["html"].ToString() + "\"}";
            
            //更新标记为已读
            sql = "UPDATE TeteUserMsg SET isread = 1 WHERE nick = '" + uid + "' AND id = " + msgid + "";
            utils.ExecuteNonQuery(sql);
        }
        else
        {
            str = "{\"count\":\"0\"}";
        }

        Response.Write(str);
    }

    private void ShowMsgListInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;

        sql = "SELECT * FROM TeteUserMsg WHERE token = '" + token + "' AND nick = '" + uid + "' ORDER BY id DESC";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str = "{\"msg\":[";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i != 0)
                {
                    str += ",";
                }

                str += "{\"id\":\"" + dt.Rows[i]["id"].ToString() + "\",\"title\":\"" + dt.Rows[i]["title"].ToString() + "\",\"date\":\"" + dt.Rows[i]["adddate"].ToString() + "\",\"isread\":\"" + dt.Rows[i]["isread"].ToString() + "\"}";
            }
            str += "]}";
        }
        else
        {
            str = "{\"count\":\"0\"}";
        }

        Response.Write(str);
    }

    private void ShowMsgCountInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;

        sql = "SELECT COUNT(*) FROM TeteUserMsg WHERE token = '" + token + "' AND nick = '" + uid + "' AND isread = 0";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str = "{\"count\":\"" + dt.Rows[0][0].ToString() + "\"}";
        }
        else
        {
            str = "{\"count\":\"\",\"0\":\"\"}";
        }

        Response.Write(str);
    }


    private void RecordErrInfo()
    {
        string sql = string.Empty;


        sql = "INSERT INTO TeteUserErr (err, token, nick) VALUES ('" + err + "', '" + token + "', '" + uid + "')";
        utils.ExecuteNonQuery(sql);
        

        string str = "{\"result\":\"ok\"}";
        Response.Write(str);
    }

    private void RecordTokenInfo()
    {
        string sql = string.Empty;

        sql = "SELECT COUNT(*) FROM TeteUserToken WHERE nick = '" + uid + "' AND token = '" + token + "'";
        string count = utils.ExecuteString(sql);

        if (count == "0")
        {
            sql = "INSERT INTO TeteUserToken (nick, token) VALUES ('" + uid + "', '" + token + "')";
            utils.ExecuteNonQuery(sql);
        }

        string str = "{\"result\":\"ok\"}";
        Response.Write(str);
    }

    private void ShowNearInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;

        if (direct == "left")
        {
            sql = "SELECT TOP 1 * FROM TeteShopItem WHERE nick = '" + uid + "' AND id < (SELECT id FROM TeteShopItem WHERE itemid = " + itemid + ") AND CHARINDEX('" + cid + "', cateid) > 0 ORDER BY id DESC";
        }
        else
        {
            sql = "SELECT TOP 1 * FROM TeteShopItem WHERE nick = '" + uid + "' AND id > (SELECT id FROM TeteShopItem WHERE itemid = " + itemid + ") AND CHARINDEX('" + cid + "', cateid) > 0 ORDER BY id ASC";
        }

        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str = "{\"item\":[";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i != 0)
                {
                    str += ",";
                }

                str += "{\"itemid\":\"" + dt.Rows[i]["itemid"].ToString() + "\",\"pic_url\":\"" + dt.Rows[i]["picurl"].ToString() + "\",\"name\":\"" + dt.Rows[i]["itemname"].ToString() + "\",\"detail_url\":\"" + dt.Rows[i]["linkurl"].ToString() + "\"}";
            }
            str += "]}";
        }
        else
        {
            str = "{\"count\":\"0\"}";
        }

        Response.Write(str);
    }

    private void ShowDetailInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;

        sql = "SELECT * FROM TeteShopItem WHERE nick = '" + uid + "' AND itemid = " + itemid + "";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str = "{\"item\":[";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i != 0)
                {
                    str += ",";
                }

                str += "{\"itemid\":\"" + dt.Rows[i]["itemid"].ToString() + "\",\"pic_url\":\"" + dt.Rows[i]["picurl"].ToString() + "\",\"name\":\"" + dt.Rows[i]["itemname"].ToString() + "\",\"detail_url\":\"" + dt.Rows[i]["linkurl"].ToString() + "\"}";
            }
            str += "]}";
        }
        else
        {
            str = "{\"count\":\"0\"}";
        }

        Response.Write(str);
    }


    private void ShowSpecialListInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;

        int pageNow = 1;
        if (page == "")
        {
            pageNow = 1;
        }
        else
        {
            pageNow = int.Parse(page);
        }

        int pageSizeNow = 20;
        if (pagesize == "")
        {
            pageSizeNow = 20;
        }
        else
        {
            pageSizeNow = int.Parse(pagesize);
        }
        int pageCount = pageSizeNow;
        int dataCount = (pageNow - 1) * pageCount;


        sql = "SELECT COUNT(*) FROM TeteShopItem WHERE nick = '" + uid + "'";
        int totalCount = int.Parse(utils.ExecuteString(sql));
        int totalPageCount = 1;

        if (totalCount % pageCount == 0)
        {
            totalPageCount = totalCount / pageCount;
        }
        else
        {
            totalPageCount = totalCount / pageCount + 1;
        }

        sql = "SELECT TOP " + pageCount.ToString() + " * FROM (SELECT *,ROW_NUMBER() OVER (ORDER BY id DESC) AS rownumber FROM TeteShopItem WHERE nick = '" + uid + "') AS a WHERE a.rownumber > " + dataCount.ToString() + " ORDER BY id DESC";
        //Response.Write(sql);
        //sql = "SELECT * FROM TeteShopItem WHERE nick = '" + uid + "' AND CHARINDEX('" + cid + "', cateid) > 0";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str = "{\"item\":[";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i != 0)
                {
                    str += ",";
                }

                str += "{\"itemid\":\"" + dt.Rows[i]["itemid"].ToString() + "\",\"pic_url\":\"" + dt.Rows[i]["picurl"].ToString() + "\",\"name\":\"" + dt.Rows[i]["itemname"].ToString() + "\",\"detail_url\":\"" + dt.Rows[i]["linkurl"].ToString() + "\"}";
            }
            str += "],\"pagenow\":" + page + ",\"total\":" + totalPageCount + "}";
        }
        else
        {
            str = "{\"count\":\"0\"}";
        }

        Response.Write(str);
    }

    private void ShowListInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;

        int pageNow = 1;
        if (page == "")
        {
            pageNow = 1;
        }
        else
        {
            pageNow = int.Parse(page);
        }

        int pageSizeNow = 20;
        if (pagesize == "")
        {
            pageSizeNow = 20;
        }
        else
        {
            pageSizeNow = int.Parse(pagesize);
        }
        int pageCount = pageSizeNow;
        int dataCount = (pageNow - 1) * pageCount;


        sql = "SELECT COUNT(*) FROM TeteShopItem WHERE nick = '" + uid + "' AND CHARINDEX('" + cid + "', cateid) > 0";
        int totalCount = int.Parse(utils.ExecuteString(sql));
        int totalPageCount = 1;

        if (totalCount % pageCount == 0)
        {
            totalPageCount = totalCount / pageCount;
        }
        else
        {
            totalPageCount = totalCount / pageCount + 1;
        }

        sql = "SELECT TOP " + pageCount.ToString() + " * FROM (SELECT *,ROW_NUMBER() OVER (ORDER BY id DESC) AS rownumber FROM TeteShopItem WHERE nick = '" + uid + "' AND CHARINDEX('" + cid + "', cateid) > 0) AS a WHERE a.rownumber > " + dataCount.ToString() + " ORDER BY id DESC";
        //Response.Write(sql);
        //sql = "SELECT * FROM TeteShopItem WHERE nick = '" + uid + "' AND CHARINDEX('" + cid + "', cateid) > 0";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str = "{\"item\":[";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i != 0)
                {
                    str += ",";
                }

                str += "{\"itemid\":\"" + dt.Rows[i]["itemid"].ToString() + "\",\"pic_url\":\"" + dt.Rows[i]["picurl"].ToString() + "\",\"name\":\"" + dt.Rows[i]["itemname"].ToString() + "\",\"detail_url\":\"" + dt.Rows[i]["linkurl"].ToString() + "\"}";
            }
            str += "],\"pagenow\":" + page + ",\"total\":" + totalPageCount + "}";
        }
        else
        {
            str = "{\"count\":\"0\"}";
        }

        Response.Write(str);
    }

    private void ShowCateInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;

        sql = "SELECT * FROM TeteShopCategory WHERE nick = '" + uid + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str = "{\"cate\":[";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i != 0)
                {
                    str += ",";
                }

                str += "{\"cid\":\"" + dt.Rows[i]["cateid"].ToString() + "\",\"parent_cid\":\"" + dt.Rows[i]["parentid"].ToString() + "\",\"name\":\"" + dt.Rows[i]["catename"].ToString() + "\",\"count\":\"" + dt.Rows[i]["catecount"].ToString() + "\",\"catepicurl\":\"" + dt.Rows[i]["catepicurl"].ToString() + "\"}";
            }
            str += "]}";
        }
        else
        {
            str = "{\"count\":\"0\"}";
        }

        Response.Write(str);
    }

    /// <summary>
    /// 将客户设置的广告图片地址发送给客户端
    /// </summary>
    private void ShowAdsInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;

        sql = "SELECT logo,ads FROM TeteShop WHERE nick = '" + uid + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str = "{\"logo\":\"" + dt.Rows[0][0].ToString() + "\",\"adsurl\":\"" + dt.Rows[0][1].ToString() + "\"}";
        }
        else
        {
            str = "{\"logo\":\"\",\"adsurl\":\"\"}";
        }

        Response.Write(str);
    }
}