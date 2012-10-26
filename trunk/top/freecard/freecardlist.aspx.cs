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

public partial class top_freecard_freecardlist : System.Web.UI.Page
{
    public string session = string.Empty;
    public string nick = string.Empty;
    public string id = string.Empty;
    public string nickencode = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        id = utils.NewRequest("id", utils.RequestType.QueryString);
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        session = cookie.getCookie("top_sessiongroupbuy");
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Decrypt(taobaoNick);

        nickencode = HttpUtility.UrlEncode(nick);

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

        string act = utils.NewRequest("act", utils.RequestType.QueryString);
        if (act == "del")
        {
            DeleteGroup();
            return;
        }

        BindData();
    }

    public static string checkArea(string str)
    {
        if (str == "1")
        {
            return "【只有以下地区包邮】";
        }
        else
        {
            return "【以下地区不包邮】";
        }
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

    private void DeleteGroup()
    {
        string sql = "SELECT isfree FROM TCS_ShopConfig WHERE nick = '" + nick + "'";
        string isfree = utils.ExecuteString(sql);

        if (isfree == "1")
        {
            sql = "SELECT freeid FROM TCS_ShopConfig WHERE nick = '" + nick + "'";
            string freeid = utils.ExecuteString(sql);

            if (freeid == id)
            {
                Response.Write("<script>alert('默认赠送的包邮卡无法删除，请您到“基本设置”里面更改默认包邮卡或者关闭包邮卡赠送后再删除！');history.go(-1);</script>");
                return;
            }
        }

        //通过数据库查询获取活动ID
         sql = "UPDATE TCS_FreeCardAction SET isdel = 1 WHERE guid = '" + id + "'";
        utils.ExecuteNonQuery(sql);

        Response.Write("<script>alert('取消成功！');window.location.href='freecardlist.aspx';</script>");
    }

    private void BindData()
    {
        string sql = "SELECT * FROM TCS_FreeCardAction WHERE nick = '" + nick + "' AND isdel = 0";
        DataTable dt = utils.ExecuteDataTable(sql);

        rptArticle.DataSource = dt;
        rptArticle.DataBind();
    }
}