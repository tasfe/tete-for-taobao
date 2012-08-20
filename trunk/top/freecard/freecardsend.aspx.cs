using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Common;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;
using System.Net;
using System.Security.Cryptography;

public partial class top_freecard_freecardsend : System.Web.UI.Page
{
    public string session = string.Empty;
    public string nick = string.Empty;
    public string id = string.Empty;
    public string couponstr = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        id = utils.NewRequest("id", utils.RequestType.QueryString);
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        session = cookie.getCookie("top_sessiongroupbuy");
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

        //过期判断
        if (!IsBuy(nick))
        {
            string msg = "尊敬的" + nick + "，非常抱歉的告诉您，您尚未订购该功能，如需继续使用请购买该服务:<br><br>29元/月 【赠送短信100条】 <a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22904-11:1;' target='_blank'>立即购买</a><br><br>78元/季 【赠送短信300条】 <a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22904-11:3;' target='_blank'>立即购买</a><br><br>148元/半年 【赠送短信600条】<a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22904-11:6;' target='_blank'>立即购买</a><br><br>288元/年 【赠送短信1200条】<a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22904-11:12;' target='_blank'>立即购买</a><br>";
            Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
            Response.End();
            return;
        }

        BindData();
    }

    private void BindData()
    {
        //数据绑定
        DataTable dtCoupon = utils.ExecuteDataTable("SELECT * FROM TCS_FreeCardAction WHERE nick = '" + nick + "' AND isdel = 0 ORDER BY adddate DESC");
        couponstr = "<select name='freecardid'>";
        for (int i = 0; i < dtCoupon.Rows.Count; i++)
        {
            couponstr += "<option value='" + dtCoupon.Rows[i]["guid"].ToString() + "'>" + dtCoupon.Rows[i]["name"].ToString() + " - 免费" + dtCoupon.Rows[i]["carddate"].ToString() + "月 限制" + dtCoupon.Rows[i]["usecount"].ToString() + "次</option>";
        }
        couponstr += "</select>";
    }

    /// <summary>
    /// 判断该用户是否订购了该服务
    /// </summary>
    /// <param name="nick"></param>
    /// <returns></returns>
    private bool IsBuy(string nick)
    {
        string sql = "SELECT plus FROM TCS_ShopSession WHERE nick = '" + nick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            string plus = dt.Rows[0][0].ToString();
            if (plus.IndexOf("freecard") != -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (this.txtBuyerNick.Text.Length == 0)
        {
            Response.Write("<script>alert('买家昵称不能为空');history.go(-1);</script>");
            Response.End();
            return;
        }

        string cardid = utils.NewRequest("freecardid", utils.RequestType.Form);
        string sql = "SELECT * FROM TCS_FreeCardAction WHERE guid = '" + cardid + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            //sql = "SELECT COUNT(*) FROM TCS_FreeCard WHERE nick = '" + nick + "' AND buynick = '" + txtBuyerNick.Text + "'";
            //string count = utils.ExecuteString(sql);
            //if (count != "0")
            //{
            //    Response.Write("<script>alert('不可对买家重复赠送包邮卡，如果需要修改买家包邮卡使用日期和限制请在赠送记录里面直接编辑买家的包邮卡');history.go(-1);</script>");
            //    Response.End();
            //    return;
            //}

            string startdate = DateTime.Now.ToString();
            string enddate = DateTime.Now.ToString();
            string usecountlimit = dt.Rows[0]["usecount"].ToString();
            string carddate = dt.Rows[0]["carddate"].ToString();
            if (carddate == "0")
            {
                enddate = DateTime.Now.AddMonths(999).ToString();
            }
            else
            {
                enddate = DateTime.Now.AddMonths(int.Parse(carddate)).ToString();
            }

            sql = "INSERT INTO TCS_FreeCard (nick,buynick,cardid,startdate,enddate,carddate,usecountlimit) VALUES ('" + nick + "', '" + this.txtBuyerNick.Text + "','" + cardid + "','" + startdate + "','" + enddate + "','" + carddate + "','" + usecountlimit + "')";
            utils.ExecuteNonQuery(sql);

            //增加免邮卡领取次数
            sql = "UPDATE TCS_FreeCardAction SET sendcount = sendcount + 1 WHERE guid = '" + cardid + "'";
            utils.ExecuteNonQuery(sql);
        }

        Response.Redirect("freecardcustomer.aspx");
    }
}