﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;

public partial class top_review_couponsend : System.Web.UI.Page
{
    public string session = string.Empty;
    public string nick = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string id = utils.NewRequest("id", utils.RequestType.QueryString);
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        session = cookie.getCookie("top_sessiongroupbuy");
        string iscrm = cookie.getCookie("iscrm");
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Decrypt(taobaoNick);


        //过期判断
        if (string.IsNullOrEmpty(taobaoNick))
        {
            string msg = "尊敬的淘宝卖家，非常抱歉的告诉您，您还没有购买此服务或者登录信息已失效，如需继续使用请<a href='http://fuwu.taobao.com/serv/detail.htm?service_id=4545' target='_blank'>进入该服务</a>，谢谢！";
            Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
            Response.End();
            return;
        }

        ////过期判断
        //if (iscrm != "1")
        //{
        //    string msg = "尊敬的" + nick + "，非常抱歉的告诉您，您尚未订购该功能，如需继续使用请购买该服务:<br><br>19元/月 【赠送短信50条】 <a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-4:1;' target='_blank'>立即购买</a><br><br>54元/季 【赠送短信150条】 <a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-4:3;' target='_blank'>立即购买</a><br><br>99元/半年 【赠送短信300条】<a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-4:6;' target='_blank'>立即购买</a><br><br>188元/年 【赠送短信600条】<a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-4:12;' target='_blank'>立即购买</a><br>";
        //    Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
        //    Response.End();
        //    return;
        //}

        if (!IsPostBack)
        {
            BindData();
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (search.Text.Trim() == "")
        {
            Response.Redirect("couponsend.aspx");
            return;
        }

        string sqlNew = "SELECT * FROM TCS_CouponSendCrm WHERE nick = '" + nick + "' AND buynick = '" + search.Text.Trim().Replace("'", "''") + "' ORDER BY taobaonumber DESC";
        DataTable dt = utils.ExecuteDataTable(sqlNew);

        rptArticle.DataSource = dt;
        rptArticle.DataBind();

        lbPage.Text = "";
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
        int pageCount = 12;
        int dataCount = (pageNow - 1) * pageCount;

        //string sqlCoupon = "SELECT * FROM TopCoupon WHERE coupon_id = " + ; 

        string sqlNew = "SELECT TOP " + pageCount.ToString() + " * FROM (SELECT s.*,ROW_NUMBER() OVER (ORDER BY s.taobaonumber DESC) AS rownumber FROM TCS_CouponSend s WHERE s.nick = '" + nick + "') AS a WHERE a.rownumber > " + dataCount.ToString() + " ORDER BY taobaonumber DESC";
        DataTable dt = utils.ExecuteDataTable(sqlNew);
        //Response.Write(sqlNew);
        rptArticle.DataSource = dt;
        rptArticle.DataBind();

        //分页数据初始化
        sqlNew = "SELECT COUNT(*) FROM TCS_CouponSend WHERE nick = '" + nick + "'";
        int totalCount = int.Parse(utils.ExecuteString(sqlNew));

        lbPage.Text = InitPageStr(totalCount, "couponsend.aspx");

        ////string sql = "SELECT s.*,c.coupon_name FROM TopCouponSend s INNER JOIN TopCoupon c ON c.coupon_id = s.couponid WHERE s.nick = '" + nick + "' ORDER BY s.id DESC";
        ////DataTable dt = utils.ExecuteDataTable(sql);

        //rptArticle.DataSource = dt;
        //rptArticle.DataBind();
    }


    private string InitPageStr(int total, string url)
    {
        //分页数据初始化
        string str = string.Empty;
        int pageCount = 12;
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