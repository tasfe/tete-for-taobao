using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Common;
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;
using System.Net;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Xml;

public partial class top_review_msg : System.Web.UI.Page
{
    public string session = string.Empty;
    public string nick = string.Empty;
    public string used = string.Empty;
    public string total = string.Empty;

    public string giftflag = string.Empty;
    public string giftcontent = string.Empty;
    public string shippingflag = string.Empty;
    public string shippingcontent = string.Empty;
    public string reviewflag = string.Empty;
    public string reviewcontent = string.Empty;
    public string fahuoflag = string.Empty;
    public string fahuocontent = string.Empty;
    public string reviewtime = string.Empty;
    public string oldshopname = string.Empty;
    public string shopname = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string id = utils.NewRequest("id", utils.RequestType.QueryString);
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        session = cookie.getCookie("top_sessiongroupbuy");
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Decrypt(taobaoNick);

        //判断VIP版本，只有VIP才能使用此功能
        string sql = "SELECT * FROM TopTaobaoShop WHERE nick = '" + nick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            string flag = dt.Rows[0]["versionNoBlog"].ToString();
            if (flag == "1")
            {
                string msg = "尊敬的" + nick + "，非常抱歉的告诉您，只有专业版或者以上版本才能使用【短信自动提醒】功能，如需继续使用请<a href='http://pay.taobao.com/mysub/subarticle/upgrade_order_sub_article.htm?market_type=6&article_id=181' target='_blank'>购买高级会员服务</a>，谢谢！";
                Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
                Response.End();
                return;
            }

            oldshopname = dt.Rows[0]["title"].ToString();
        }

        if (!IsPostBack)
        {
            BindData();
        }
    }

    /// <summary>
    /// 数据绑定
    /// </summary>
    private void BindData()
    {

        //获取相关短信设置
        string sql = "SELECT * FROM TopAutoReview WHERE nick = '" + nick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            used = dt.Rows[0]["used"].ToString();
            total = dt.Rows[0]["total"].ToString();
            giftflag = dt.Rows[0]["giftflag"].ToString();
            giftcontent = dt.Rows[0]["giftcontent"].ToString();
            shippingflag = dt.Rows[0]["shippingflag"].ToString();
            shippingcontent = dt.Rows[0]["shippingcontent"].ToString();
            reviewflag = dt.Rows[0]["reviewflag"].ToString();
            reviewcontent = dt.Rows[0]["reviewcontent"].ToString();
            fahuoflag = dt.Rows[0]["fahuoflag"].ToString();
            fahuocontent = dt.Rows[0]["fahuocontent"].ToString();
            shopname = dt.Rows[0]["shopname"].ToString();
            reviewtime = dt.Rows[0]["reviewtime"].ToString();

            //增加默认值
            if (giftcontent.Length == 0)
            {
                giftcontent = "+[shopname]+:亲爱的[buynick],恭喜您获得我店[gift],请注意查收,感谢您的及时评价!";
            }
            if (shippingcontent.Length == 0)
            {
                shippingcontent = "+[shopname]+:亲爱的[buynick],您的宝贝已到达,请您尽快确认好评,满分好评+优质评价,即可获赠[gift],赶快行动!哦!";
            }
            if (reviewcontent.Length == 0)
            {
                reviewcontent = "+[shopname]+:亲爱的[buynick],您购买的货物已经收到很多天了,请尽快确认好评,满分好评+优质评价,即可获赠[gift],赶快行动!哦!";
            }
            if (fahuocontent.Length == 0)
            {
                fahuocontent = "+[shopname]+:亲爱的[buynick],您购买的货物已经发出,请您在收到货物后尽快确认好评,满分好评+优质评价,即可获赠[gift],赶快行动!哦!";
            }
            if (shopname.Length == 0)
            {
                shopname = oldshopname;
            }
        }
    }

    
    /// <summary>
    /// 保存设置
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {
        string giftflag = utils.NewRequest("giftflag", utils.RequestType.Form) == "1" ? "1" : "0";
        string shippingflag = utils.NewRequest("shippingflag", utils.RequestType.Form) == "1" ? "1" : "0";
        string reviewflag = utils.NewRequest("reviewflag", utils.RequestType.Form) == "1" ? "1" : "0";
        string fahuoflag = utils.NewRequest("fahuoflag", utils.RequestType.Form) == "1" ? "1" : "0";
        string reviewtime = utils.NewRequest("reviewtime", utils.RequestType.Form);

        string sql = "UPDATE TopAutoReview SET " +
            "giftflag = '" + giftflag + "', " +
            "giftcontent = '" + utils.NewRequest("giftcontent", utils.RequestType.Form) + "', " +
            "shippingflag = '" + shippingflag + "', " +
            "shippingcontent = '" + utils.NewRequest("shippingcontent", utils.RequestType.Form) + "', " +
            "reviewflag = '" + reviewflag + "', " +
            "shopname = '" + utils.NewRequest("shopname", utils.RequestType.Form) + "', " +
            "fahuoflag = '" + fahuoflag + "', " +
            "fahuocontent = '" + utils.NewRequest("fahuocontent", utils.RequestType.Form) + "', " +
            "reviewtime = '" + utils.NewRequest("reviewtime", utils.RequestType.Form) + "', " +
            "reviewcontent = '" + utils.NewRequest("reviewcontent", utils.RequestType.Form) + "' " +
        "WHERE nick = '" + nick + "'";

        //Response.Write(sql);
        utils.ExecuteNonQuery(sql);

        Response.Write("<script>alert('保存成功！');window.location.href='msg.aspx';</script>");
        Response.End();
        return;
    }
}