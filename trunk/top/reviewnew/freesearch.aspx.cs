using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;
using System.Text;

public partial class top_reviewnew_freesearch : System.Web.UI.Page
{
    public string buynick = string.Empty;
    public string con = string.Empty;
    public string gift = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string nick = utils.NewRequest("nick", utils.RequestType.QueryString);
        buynick = utils.NewRequest("buynick", utils.RequestType.QueryString);
        //StringBuilder builder = new StringBuilder();

        string sql = "SELECT a.name,a.areaisfree,a.arealist,f.startdate,f.carddate,f.usecount,f.usecountlimit,f.price FROM TCS_FreeCard f INNER JOIN TCS_FreeCardAction a ON a.guid = f.cardid WHERE f.nick = '" + nick + "' AND f.buynick = '" + buynick + "' AND f.isdel=0";

        DataTable dt = utils.ExecuteDataTable(sql);

        rptArticle.DataSource = dt;
        rptArticle.DataBind();

        if (dt.Rows.Count == 0)
        {
            Panel1.Visible = false;
        }

        sql = "SELECT * FROM TCS_ShopConfig WHERE nick = '"+nick+"'";
        dt = utils.ExecuteDataTable(sql);

        if (dt.Rows.Count != 0)
        {
            con += "<div style='margin:0 0 0 16px; padding:3px; font-size:18px;'>★ 三项动态评分全5分</div>";
            if (dt.Rows[0]["iscancelauto"].ToString() == "1")
            {
                con += "<div style='margin:0 0 0 16px; padding:3px; font-size:18px;'>★ 默认好评不赠送</div>";
            }
            if (dt.Rows[0]["iskeyword"].ToString() == "1")
            {
                if (dt.Rows[0]["wordcount"].ToString() != "0")
                {
                    con += "<div style='margin:0 0 0 16px; padding:3px; font-size:18px;'>★ 评价字数≥" + dt.Rows[0]["wordcount"].ToString() + "个字</div>";
                }

                if (dt.Rows[0]["keywordisbad"].ToString() == "0")
                {
                    //con += "<div style='margin:0 0 0 16px; padding:3px; font-size:18px;'>★ 评价内包含以下关键字则赠送：";
                }
                else {
                   // con += "<div style='margin:0 0 0 16px; padding:3px; font-size:18px;'>★ 评价内包含以下关键字则不赠送：";
                }
                //con += "【" + dt.Rows[0]["keyword"].ToString() + "】</div>";
            }

            con += "<div style='margin:0 0 0 16px; padding:3px; font-size:18px;'>★ 物流签收后" + dt.Rows[0]["mindate"].ToString() + "天内评价</div>";

            //优惠券
            if (dt.Rows[0]["iscoupon"].ToString() == "1")
            {
                sql = "SELECT * FROM TCS_Coupon WHERE guid = '" + dt.Rows[0]["couponid"].ToString() + "'";
                DataTable dt1 = utils.ExecuteDataTable(sql);
                if (dt1.Rows.Count != 0)
                {
                    gift += "<div style='float:left; width:290px; padding:0px 5px; font-size:14px;'><img src='images/gift2.jpg'><br>★ 赠送优惠券【" + dt1.Rows[0]["name"].ToString() + "】满" + dt1.Rows[0]["condition"].ToString() + "元减" + dt1.Rows[0]["num"].ToString() + "元 <br>（每人限领" + dt1.Rows[0]["per"].ToString() + "张）<br> 有效期：</div>";
                }
            }

            if (dt.Rows[0]["isfree"].ToString() == "1")
            {
                sql = "SELECT * FROM TCS_FreecardAction WHERE guid = '" + dt.Rows[0]["freeid"].ToString() + "'";
                DataTable dt1 = utils.ExecuteDataTable(sql);
                if (dt1.Rows.Count != 0)
                {
                    gift += "<div style='float:left; width:290px; padding:0px 5px; font-size:14px;'><img src='images/gift1.jpg'><br>★ 赠送包邮卡1张" + show(dt1.Rows[0]["areaisfree"].ToString(), dt1.Rows[0]["arealist"].ToString()) + "，满" + dt1.Rows[0]["price"].ToString() + "元可用，可用次数" + show1(dt1.Rows[0]["usecount"].ToString()) + "</div>";
                }
            }

            if (dt.Rows[0]["isalipay"].ToString() == "1")
            {
                sql = "SELECT * FROM TCS_Alipay WHERE guid = '" + dt.Rows[0]["alipayid"].ToString() + "'";
                DataTable dt1 = utils.ExecuteDataTable(sql);
                if (dt1.Rows.Count != 0)
                {
                    gift += "<div style='float:left; width:290px; padding:0px 5px; font-size:14px;'><img src='images/gift3.jpg'><br>★ 赠送价值" + dt1.Rows[0]["num"].ToString() + "元支付宝红包1张 <br> （每人限领" + dt1.Rows[0]["per"].ToString() + "张）</div>";
                }
            }
        }
    }

    private string show1(string p)
    {
        if (p == "0")
        {
            return "【不限次数】";
        }
        else
        {
            return "【"+p+"次】";
        }
    }


    public static string show(string isfree, string arealist)
    {
        if (arealist.Length == 0)
        {
            return "【国内全部包邮】";
        }

        if (isfree == "1")
        {
            return "【只有以下地区包邮】（" + arealist + "）";
        }
        else
        {
            return "【以下地区不包邮】（" + arealist + "）";
        }
    }
}