using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;
using System.Net;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Data;

public partial class top_crm_freecardsend : System.Web.UI.Page
{
    public string couponstr = string.Empty;
    public string session = string.Empty;
    public string nick = string.Empty;
    public string itemid = string.Empty;
    public string mindate = string.Empty;
    public string maxdate = string.Empty;
    public string itemstr = string.Empty;
    public string isfree = string.Empty;
    public string iscoupon = string.Empty;
    public string issendmsg = string.Empty;
    public string iskefu = string.Empty;

    public string typ = string.Empty;

    public string count1 = string.Empty;
    public string count2 = string.Empty;
    public string count3 = string.Empty;
    public string count4 = string.Empty;
    public string count5 = string.Empty;
    public string count6 = string.Empty;
    public string count7 = string.Empty;
    public string count8 = string.Empty;
    public string count9 = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        session = cookie.getCookie("top_sessiongroupbuy");
        string iscrm = cookie.getCookie("iscrm");
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Decrypt(taobaoNick);
        typ = utils.NewRequest("typ", utils.RequestType.QueryString);


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

            string msg = "尊敬的" + nick + "，非常抱歉的告诉您，只有VIP版本才能使用【客户关系营销】功能，如需继续使用请<a href='http://fuwu.taobao.com/ser/detail.htm?service_code=service-0-22904&frm=haoping' target='_blank'>购买高级会员服务</a>，谢谢！";
            Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
            Response.End();
            return;
        }

        BindData();
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
            if (plus.IndexOf("crm") != -1)
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

    private void BindData()
    {
        //数据绑定
        DataTable dtCoupon = utils.ExecuteDataTable("SELECT * FROM TCS_FreeCardAction WHERE nick = '" + nick + "' AND isdel = 0 ORDER BY startdate DESC");

        if (dtCoupon.Rows.Count <= 0)
        {
            Response.Write("<script>alert('请先创建1张包邮卡才能可以给买家赠送！');window.location.href='../freecard/freecardadd.aspx'</script>");
            Response.End();
        }

        couponstr = "<select name='freecardid'>";
        for (int i = 0; i < dtCoupon.Rows.Count; i++)
        {
            couponstr += "<option value='" + dtCoupon.Rows[i]["guid"].ToString() + "'>" + dtCoupon.Rows[i]["name"].ToString() + " - 免费" + dtCoupon.Rows[i]["carddate"].ToString() + "月 限制" + dtCoupon.Rows[i]["usecount"].ToString() + "次</option>";
        }
        couponstr += "</select>";

        string sql = string.Empty;
        //sql = "SELECT COUNT(*) FROM TCS_Customer WHERE nick = '" + nick + "'";
        //count1 = utils.ExecuteString(sql);

        //sql = "SELECT COUNT(*) FROM TCS_Customer WHERE nick = '" + nick + "' AND tradecount = 0";
        //count2 = utils.ExecuteString(sql);

        //sql = "SELECT COUNT(*) FROM TCS_Customer WHERE nick = '" + nick + "' AND tradecount = 1";
        //count3 = utils.ExecuteString(sql);

        //sql = "SELECT COUNT(*) FROM TCS_Customer WHERE nick = '" + nick + "' AND tradecount = 2";
        //count4 = utils.ExecuteString(sql);

        //sql = "SELECT COUNT(*) FROM TCS_Customer WHERE nick = '" + nick + "' AND grade = 0";
        //count5 = utils.ExecuteString(sql);

        //sql = "SELECT COUNT(*) FROM TCS_Customer WHERE nick = '" + nick + "' AND grade = 1";
        //count6 = utils.ExecuteString(sql);

        //sql = "SELECT COUNT(*) FROM TCS_Customer WHERE nick = '" + nick + "' AND grade = 2";
        //count7 = utils.ExecuteString(sql);

        //sql = "SELECT COUNT(*) FROM TCS_Customer WHERE nick = '" + nick + "' AND grade = 3";
        //count8 = utils.ExecuteString(sql);

        //sql = "SELECT COUNT(*) FROM TCS_Customer WHERE nick = '" + nick + "' AND grade = 4";
        //count9 = utils.ExecuteString(sql);

        sql = "SELECT * FROM TCS_Group WHERE nick = '" + nick + "' AND isdel = 0";
        DataTable dt = utils.ExecuteDataTable(sql);

        rptGroup.DataSource = dt;
        rptGroup.DataBind();
    }

    /// <summary>
    /// 保存设置
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {
        //执行优惠券赠送行为
        string sql = string.Empty;

        string buynick = "";// this.txtBuyerNick.Text;

        //获取淘宝优惠券ID
        string cardid = utils.NewRequest("freecardid", utils.RequestType.Form);
        sql = "SELECT * FROM TCS_FreeCardAction WHERE guid = '" + cardid + "'";

        string typ = utils.NewRequest("typ", utils.RequestType.Form);
        string condition = string.Empty;

        switch (typ)
        {
            case "0":
                condition = " AND b.tradecount = 0";
                break;
            case "1":
                condition = " AND b.tradecount = 1";
                break;
            case "2":
                condition = " AND b.tradecount > 1";
                break;
            case "a":
                condition = " AND b.grade = 0";
                break;
            case "b":
                condition = " AND b.grade = 1";
                break;
            case "c":
                condition = " AND b.grade = 2";
                break;
            case "d":
                condition = " AND b.grade = 3";
                break;
            case "e":
                condition = " AND b.grade = 4";
                break;
        }

        if (typ.Length > 10)
        {
            condition = " AND b.groupguid = '" + typ + "'";
        }

        int index = 0;
        int err = 0;
        string errtext = string.Empty;
        string usecountlimit = string.Empty;
        string carddate = string.Empty;

        sql = "SELECT * FROM TCS_FreeCardAction WHERE guid = '" + cardid + "'";
        DataTable dtFreecard = utils.ExecuteDataTable(sql);
        if (dtFreecard.Rows.Count != 0)
        {
            usecountlimit = dtFreecard.Rows[0]["usecount"].ToString();
            carddate = dtFreecard.Rows[0]["carddate"].ToString();
        }

        sql = "SELECT * FROM TCS_Customer b WHERE b.nick = '" + nick + "' " + condition + "";
        DataTable dt = utils.ExecuteDataTable(sql);
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            buynick = dt.Rows[i]["buynick"].ToString();

            sql = "SELECT COUNT(*) FROM TCS_FreeCard WHERE nick = '" + nick + "' AND buynick = '" + buynick + "'";
            string count = utils.ExecuteString(sql);
            if (count != "0")
            {
                err++;
                //Response.Write("<script>alert('不可对买家重复赠送包邮卡，如果需要修改买家包邮卡使用日期和限制请在赠送记录里面直接编辑买家的包邮卡');history.go(-1);</script>");
                //Response.End();
                //return;
                continue;
            }

            string startdate = DateTime.Now.ToString();
            string enddate = DateTime.Now.ToString();
            
            if (carddate == "0")
            {
                enddate = DateTime.Now.AddMonths(999).ToString();
            }
            else
            {
                enddate = DateTime.Now.AddMonths(int.Parse(carddate)).ToString();
            }

            sql = "INSERT INTO TCS_FreeCard (nick,buynick,cardid,startdate,enddate,carddate,usecountlimit) VALUES ('" + nick + "', '" + buynick + "','" + cardid + "','" + startdate + "','" + enddate + "','" + carddate + "','" + usecountlimit + "')";
            utils.ExecuteNonQuery(sql);

            //增加免邮卡领取次数
            sql = "UPDATE TCS_FreeCardAction SET sendcount = sendcount + 1 WHERE guid = '" + cardid + "'";
            utils.ExecuteNonQuery(sql);

            index++;
            //}
            //else
            //{
            //    err++;
            //}
        }
        if (err == 0)
        {
            Response.Write("<script>alert('赠送完毕，成功赠送" + index.ToString() + "张，失败" + err.ToString() + "张！');window.location.href='../freecard/freecardcustomer.aspx';</script>");
        }
        else
        {
            Response.Write("<script>alert('赠送完毕，成功赠送" + index.ToString() + "张，失败" + err.ToString() + "张，失败原因" + errtext + "！');window.location.href='../freecard/freecardcustomer.aspx';</script>");
        }
        Response.End();
    }
}