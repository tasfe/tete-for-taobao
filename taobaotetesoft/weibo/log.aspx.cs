﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using Common;
using QWeiboSDK;
using System.Text;
using System.Data;

public partial class weibo_log : System.Web.UI.Page
{
    private string appKey = string.Empty;
    private string appSecret = string.Empty;
    private string tokenKey = string.Empty;
    private string tokenSecret = string.Empty;
    private string uid = string.Empty;
    public string str = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        appKey = "d3225497956249cbb13a7cb7375d62bd";
        appSecret = "6cf7a3274cb676328e77dff3e203061d";

        Common.Cookie cookie = new Common.Cookie();
        tokenKey = cookie.getCookie("tokenKey");
        tokenSecret = cookie.getCookie("tokenSecret");
        uid = cookie.getCookie("uid");

        BindData();
    }

    private void BindData()
    {
        string page = utils.NewRequest("page", utils.RequestType.QueryString);
        int pageNow = 1;
        if (page == "")
        {
            pageNow = 1;
        }
        else
        {
            pageNow = int.Parse(page);
        }
        int pageCount = 18;
        int dataCount = (pageNow - 1) * pageCount;

        string sqlNew = "SELECT TOP " + pageCount.ToString() + " * FROM (SELECT *,ROW_NUMBER() OVER (ORDER BY id DESC) AS rownumber FROM TopMicroBlogNumLog WHERE uid = '" + uid + "') AS a WHERE a.rownumber > " + dataCount.ToString() + " ORDER BY id DESC";
        DataTable dt = utils.ExecuteDataTable(sqlNew);

        rptArticle.DataSource = dt;
        rptArticle.DataBind();

        //分页数据初始化
        sqlNew = "SELECT COUNT(*) FROM TopMicroBlogNumLog WHERE uid = '" + uid + "'";
        int totalCount = int.Parse(utils.ExecuteString(sqlNew));

        lbPage.Text = InitPageStr(totalCount, "log.aspx");
    }


    /// <summary>
    /// 短信类型
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string message(string str, string detail)
    {
        string newstr = string.Empty;
        if (str == "onekey")
        {
            newstr = "一键收听别人20个微博";
        }
        else if (str == "add")
        {
            newstr = "收听【" + detail + "】的微博";
        }
        else if (str == "deduct")
        {
            newstr = "被【" + detail + "】收听您的微博";
        }
        else if (str == "send")
        {
            newstr = "微博推荐特特互粉软件";
        }
        else if (str == "reg")
        {
            newstr = "注册新用户赠送积分";
        }
        return newstr;
    }


    private string InitPageStr(int total, string url)
    {
        //分页数据初始化
        string str = string.Empty;
        int pageCount = 18;
        int pageSize = 0;
        int pageNow = 1;
        string page = utils.NewRequest("page", utils.RequestType.QueryString);
        if (page == "")
        {
            pageNow = 1;
        }
        else
        {
            pageNow = int.Parse(page);
        }

        //取总分页数
        if (total % pageCount == 0)
        {
            pageSize = total / pageCount;
        }
        else
        {
            pageSize = total / pageCount + 1;
        }

        //如果总页面大于20，则最大页面差不超过20
        int start = 1;
        int end = 20;

        if (pageSize < end)
        {
            end = pageSize;
        }
        else
        {
            if (pageNow > 15)
            {
                start = pageNow - 10;

                if (pageNow < (total - 10))
                {
                    end = pageNow + 10;
                }
                else
                {
                    end = total;
                }
            }
        }

        for (int i = start; i <= end; i++)
        {
            if (i.ToString() == pageNow.ToString())
            {
                str += i.ToString() + " ";
            }
            else
            {
                str += "<a href='" + url + "?page=" + i.ToString() + "'>[" + i.ToString() + "]</a> ";
            }
        }

        return str;
    }

}