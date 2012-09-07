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

public partial class top_reviewnew_blacklistgift : System.Web.UI.Page
{
    public string session = string.Empty;
    public string nick = string.Empty;
    public string used = string.Empty;
    public string total = string.Empty;
    public string blacklist = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string id = utils.NewRequest("id", utils.RequestType.QueryString);
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

        //判断VIP版本，只有VIP才能使用此功能
        string sql = "SELECT * FROM TCS_ShopSession WHERE nick = '" + nick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            string flag = dt.Rows[0]["version"].ToString();

            if (flag == "0")
            {
                Response.Redirect("xufei.aspx");
                Response.End();
                return;
            }

            if (flag == "1")
            {
                string msg = "尊敬的" + nick + "，非常抱歉的告诉您，只有专业版或者以上版本才能使用【短信自动提醒】功能，如需继续使用请<a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22904-2:1;' target='_blank'>购买高级会员服务</a>，谢谢！<br><br> PS：发送的短信需要单独购买，1毛钱1条~";
                Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
                Response.End();
                return;
            }
        }

        if (!IsPostBack)
        {
            BindData();
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

        string sql = string.Empty;
        //delete
        sql = "DELETE FROM TCS_BlackListGift WHERE nick = '" + nick + "'";
        utils.ExecuteNonQuery(sql);
        //insert
        string black = utils.NewRequest("blacklist", utils.RequestType.Form);
        string[] blackAry = Regex.Split(black, "\r\n");
        for (int i = 0; i < blackAry.Length; i++)
        {
            sql = "INSERT INTO TCS_BlackListGift (nick, buynick) VALUES ('" + nick + "','" + blackAry[i] + "')";
            utils.ExecuteNonQuery(sql);
        }

        Response.Write("<script>alert('保存成功！');window.location.href='blacklistgift.aspx';</script>");
        Response.End();
        return;
    }

    /// <summary>
    /// 数据绑定
    /// </summary>
    private void BindData()
    {
        //获取相关短信设置
        string sql = string.Empty;
        //blacklist
        sql = "SELECT * FROM TCS_BlackListGift WHERE nick = '" + nick + "'";
        DataTable dtBlack = utils.ExecuteDataTable(sql);
        for (int i = 0; i < dtBlack.Rows.Count; i++)
        {
            if (i == 0)
            {
                blacklist += dtBlack.Rows[i]["buynick"].ToString();
            }
            else
            {
                blacklist += "\r\n" + dtBlack.Rows[i]["buynick"].ToString();
            }
        }
    }
}