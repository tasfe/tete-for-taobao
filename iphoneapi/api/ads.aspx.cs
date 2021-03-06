﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;

public partial class iphoneapi_api_cate : System.Web.UI.Page
{
    public string nick = string.Empty;
    public string session = string.Empty;
    public string st = string.Empty;

    public string logo1 = string.Empty;
    public string url1 = string.Empty;
    public string logo2 = string.Empty;
    public string url2 = string.Empty;
    public string logo3 = string.Empty;
    public string url3 = string.Empty;

    public string cate1 = string.Empty;
    public string cate2 = string.Empty;
    public string cate3 = string.Empty;

    public string ary = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        session = cookie.getCookie("top_session");
        st = cookie.getCookie("short");
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Decrypt(taobaoNick);

        string sql = "SELECT TOP 5 * FROM TeteShopCategory WHERE nick = '" + st + "' AND parentid=0 ORDER BY orderid";

        if (!IsPostBack)
        {
            DataTable dt = utils.ExecuteDataTable(sql);
            rpt1.DataSource = dt;
            rpt1.DataBind();

            sql = "SELECT * FROM TeteShopAds WHERE typ = 'index' AND nick = '" + st + "' ORDER BY orderid";
            dt = utils.ExecuteDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                {
                    logo1 = dt.Rows[i]["logo"].ToString();
                    url1 = dt.Rows[i]["url"].ToString();
                    cate1 = dt.Rows[i]["cateid"].ToString();
                }

                if (i == 1)
                {
                    logo2 = dt.Rows[i]["logo"].ToString();
                    url2 = dt.Rows[i]["url"].ToString();
                    cate2 = dt.Rows[i]["cateid"].ToString();
                }

                if (i == 2)
                {
                    logo3 = dt.Rows[i]["logo"].ToString();
                    url3 = dt.Rows[i]["url"].ToString();
                    cate3 = dt.Rows[i]["cateid"].ToString();
                }
            }

            string aryStr = string.Empty;
            sql = "SELECT * FROM TeteShopCategory WHERE nick = '" + st + "' AND catename <> '' ORDER BY orderid";
            dt = utils.ExecuteDataTable(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                {
                    aryStr += "0|链接到指定网址," + dt.Rows[i]["cateid"].ToString() + "|" + dt.Rows[i]["catename"].ToString();
                }
                else
                {
                    aryStr += "," + dt.Rows[i]["cateid"].ToString() + "|" + dt.Rows[i]["catename"].ToString();
                }
            }
            ary = aryStr;
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string sql = string.Empty;
        string name = string.Empty;
        string orderid = string.Empty;
        string cateid = string.Empty;

        string ids = utils.NewRequest("id", utils.RequestType.Form);
        string[] ary = ids.Split(',');
        for (int i = 0; i < ary.Length; i++)
        {
            for (int j = 1; j < 4; j++)
            {
                name = utils.NewRequest("pic_" + ary[i] + "_" + j, utils.RequestType.Form);
                orderid = utils.NewRequest("url_" + ary[i] + "_" + j, utils.RequestType.Form);
                cateid = utils.NewRequest("cate_" + ary[i] + "_" + j, utils.RequestType.Form);

                sql = "UPDATE TeteShopAds SET logo = '" + name + "',url='" + orderid + "',cateid='" + cateid + "' WHERE nick = '" + st + "' AND typ = '" + ary[i] + "' AND orderid = '" + j + "'";
                utils.ExecuteNonQuery(sql);
            }
        }

        string typ = "index";
        for (int j = 1; j < 4; j++)
        {
            name = utils.NewRequest("pic_" + typ + "_" + j, utils.RequestType.Form);
            orderid = utils.NewRequest("url_" + typ + "_" + j, utils.RequestType.Form);
            cateid = utils.NewRequest("cate_" + typ + "_" + j, utils.RequestType.Form);

            sql = "UPDATE TeteShopAds SET logo = '" + name + "',url='" + orderid + "',cateid='" + cateid + "' WHERE nick = '" + st + "' AND typ = '" + typ + "' AND orderid = '" + j + "'";
            utils.ExecuteNonQuery(sql);
        }

        Response.Redirect("ads.aspx");
    }

    public static string getCate(string html, string cateid)
    {
        //return "";
        string[] htmlAry = html.Split(',');
        string str = string.Empty;
        for (int i = 0; i < htmlAry.Length; i++)
        {
            try
            {
                string[] htmlAryChild = htmlAry[i].Split('|');
                if (htmlAryChild[0] == cateid)
                {
                    str += "<option selected value='" + htmlAryChild[0] + "'>" + htmlAryChild[1] + "</option>";
                }
                else
                {
                    str += "<option value='" + htmlAryChild[0] + "'>" + htmlAryChild[1] + "</option>";
                }
            }
            catch { }
        }
        return str;
    }

    protected void rpt1_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Label lb = (Label)e.Item.FindControl("lb1");
        string sql = "SELECT * FROM TeteShopAds WHERE typ = '" + lb.Text + "' AND nick = '"+st+"' ORDER BY orderid";
        DataTable dt = utils.ExecuteDataTable(sql);

        Repeater rpt2 = (Repeater)e.Item.FindControl("rpt2");


        string aryStr = string.Empty;
        sql = "SELECT * FROM TeteShopCategory WHERE nick = '" + st + "' AND catename <> '' ORDER BY orderid";
        DataTable dt1 = utils.ExecuteDataTable(sql);
        for (int i = 0; i < dt1.Rows.Count; i++)
        {
            if (i == 0)
            {
                aryStr += "0|链接到指定网址," + dt1.Rows[i]["cateid"].ToString() + "|" + dt1.Rows[i]["catename"].ToString();
            }
            else
            {
                aryStr += "," + dt1.Rows[i]["cateid"].ToString() + "|" + dt1.Rows[i]["catename"].ToString();
            }
        }


        for (int i = 0; i < dt.Rows.Count; i++)
        {
            dt.Rows[i]["title"] = getCate(aryStr, dt.Rows[i]["cateid"].ToString());
        }

        rpt2.DataSource = dt;
        rpt2.DataBind();
    }
}