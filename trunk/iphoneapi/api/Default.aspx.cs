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

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            act = utils.NewRequest("act", utils.RequestType.QueryString);
            uid = utils.NewRequest("uid", utils.RequestType.QueryString);
            cid = utils.NewRequest("cid", utils.RequestType.QueryString);
            itemid = utils.NewRequest("itemid", utils.RequestType.QueryString);

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
                case "detail":
                    ShowDetailInfo();
                    break;
            }
        }
        catch
        {
            string str = "{\"error_response\":\"service_error\"}";
            Response.Write(str);
        }
    }

    private void ShowDetailInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;

        sql = "SELECT * FROM TeteShopItem WHERE nick = '" + uid + "' AND itemid = " + itemid + "";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str = "{";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i != 0)
                {
                    str += ",";
                }

                str += "\"item\":{\"itemid\":\"" + dt.Rows[i]["itemid"].ToString() + "\",\"pic_url\":\"" + dt.Rows[i]["picurl"].ToString() + "\",\"name\":\"" + dt.Rows[i]["itemname"].ToString() + "\",\"detail_url\":\"" + dt.Rows[i]["linkurl"].ToString() + "\"}";
            }
            str += "}";
        }
        else
        {
            str = "{\"\"}";
        }

        Response.Write(str);
    }

    private void ShowListInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;

        sql = "SELECT * FROM TeteShopItem WHERE nick = '" + uid + "' AND CHARINDEX('" + cid + "', cateid) > 0";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str = "{";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i != 0)
                {
                    str += ",";
                }

                str += "\"item\":{\"itemid\":\"" + dt.Rows[i]["itemid"].ToString() + "\",\"pic_url\":\"" + dt.Rows[i]["picurl"].ToString() + "\",\"name\":\"" + dt.Rows[i]["itemname"].ToString() + "\"}";
            }
            str += "}";
        }
        else
        {
            str = "{\"\"}";
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
            str = "{";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i != 0)
                {
                    str += ",";
                }

                str += "\"cate\":{\"cid\":\"" + dt.Rows[i]["cateid"].ToString() + "\",\"parent_cid\":\"" + dt.Rows[i]["parentid"].ToString() + "\",\"name\":\"" + dt.Rows[i]["catename"].ToString() + "\",\"count\":\"" + dt.Rows[i]["catecount"].ToString() + "\"}";
            }
            str += "}";
        }
        else
        {
            str = "{\"\"}";
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

        sql = "SELECT ads FROM TeteShop WHERE nick = '" + uid + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str = "{\"adsurl\":\"" + dt.Rows[0][0].ToString() + "\"}";
        }
        else
        {
            str = "{\"adsurl\":\"\"}";
        }

        Response.Write(str);
    }
}