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

        string sql = "SELECT a.name,a.areaisfree,a.arealist,f.startdate,f.carddate,f.usecount,f.usecountlimit,f.price FROM TCS_FreeCard f INNER JOIN TCS_FreeCardAction a ON a.guid = f.cardid WHERE f.nick = '" + nick + "' AND f.buynick = '" + buynick + "'";

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
            con += "<div style='margin:0 0 0 16px; padding:3px;'>★ 动态评分全5分才送</div>";
            if (dt.Rows[0]["iscancelauto"].ToString() == "1")
            {
                con += "<div style='margin:0 0 0 16px; padding:3px;'>★ 默认好评不赠送</div>";
            }
            if (dt.Rows[0]["wordcount"].ToString() != "0")
            {
                con += "<div style='margin:0 0 0 16px; padding:3px;'>★ 评价字数必须大于" + dt.Rows[0]["wordcount"].ToString() + "个字</div>";
            }
            if (dt.Rows[0]["iskeyword"].ToString() == "1")
            {
                if (dt.Rows[0]["keywordisbad"].ToString() == "0")
                {
                    con += "<div style='margin:0 0 0 16px; padding:3px;'>★ 评价内包含以下关键字则赠送：";
                }
                else {
                    con += "<div style='margin:0 0 0 16px; padding:3px;'>★ 评价内包含以下关键字则不赠送：";
                }
                con += "【" + dt.Rows[0]["keyword"].ToString() + "】</div>";
            }

            con += "<div style='margin:0 0 0 16px; padding:3px;'>★ 物流签收后" + dt.Rows[0]["mindate"].ToString() + "天内评价确认则赠送</div>";

            //优惠券
            if (dt.Rows[0]["iscoupon"].ToString() == "1")
            {
                gift += "<div style='margin:0 0 0 16px; padding:3px;'>★ 赠送满100减50优惠券（每人限领3张）</div>";
            }

            if (dt.Rows[0]["isfree"].ToString() == "1")
            {
                gift += "<div style='margin:0 0 0 16px; padding:3px;'>★ 赠送无限制包邮卡1张，可用3次</div>";
            }

            if (dt.Rows[0]["isalipay"].ToString() == "1")
            {
                gift += "<div style='margin:0 0 0 16px; padding:3px;'>★ 赠送价值10元支付宝红包1个</div>";
            }
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