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

public partial class top_freecard_freecardadd : System.Web.UI.Page
{
    public string session = string.Empty;
    public string nick = string.Empty;
    public string typ = string.Empty;

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

        //过期判断
        if (!IsBuy(nick))
        {
            string msg = "尊敬的" + nick + "，非常抱歉的告诉您，您尚未订购该功能，如需继续使用请购买该服务:<br><br>10元/月 【赠送短信20条】 <a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22904-12:1;' target='_blank'>立即购买</a><br><br>28元/季 【赠送短信60条】 <a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22904-12:3;' target='_blank'>立即购买</a><br><br>54元/半年 【赠送短信120条】<a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22904-12:6;' target='_blank'>立即购买</a><br><br>99元/年 【赠送短信240条】<a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22904-12:12;' target='_blank'>立即购买</a><br>";
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

    private void BindData()
    {
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {
        string name = utils.NewRequest("name", utils.RequestType.Form);
        string carddate = utils.NewRequest("carddate", utils.RequestType.Form);
        string usecount = utils.NewRequest("usecount", utils.RequestType.Form);

        string sql = "INSERT INTO TCS_FreeCardAction (name, carddate, usecount, nick) VALUES ('" + name + "','" + carddate + "','" + usecount + "', '" + nick + "')";
        utils.ExecuteNonQuery(sql);

        Response.Redirect("freecardlist.aspx");
    }
}