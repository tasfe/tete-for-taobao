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

public partial class top_crm_missionadd : System.Web.UI.Page
{
    public string session = string.Empty;
    public string nick = string.Empty;
    public string id = string.Empty;
    public string now = string.Empty;

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
            string msg = "尊敬的" + nick + "，非常抱歉的告诉您，您尚未订购该功能，如需继续使用请购买该服务:<br><br>19元/月 【赠送短信50条】 <a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22904-11:1;' target='_blank'>立即购买</a><br><br>54元/季 【赠送短信150条】 <a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22904-11:3;' target='_blank'>立即购买</a><br><br>99元/半年 【赠送短信300条】<a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22904-11:6;' target='_blank'>立即购买</a><br><br>188元/年 【赠送短信600条】<a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22904-11:12;' target='_blank'>立即购买</a><br>";
            Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
            Response.End();
            return;
        }


        if (!IsPostBack)
        {
            now = DateTime.Now.AddHours(1).ToString();
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string guid = Guid.NewGuid().ToString();
        string sql = string.Empty;
        string typ = utils.NewRequest("typ", utils.RequestType.Form);
        string group = utils.NewRequest("group", utils.RequestType.Form);
        string cuicontent = utils.NewRequest("cuicontent", utils.RequestType.Form);
        string cuidate = utils.NewRequest("cuidate", utils.RequestType.Form);
        string birthdaycontent = utils.NewRequest("birthdaycontent", utils.RequestType.Form);
        string backdate = utils.NewRequest("backdate", utils.RequestType.Form);
        string backcontent = utils.NewRequest("backcontent", utils.RequestType.Form);
        string actdate = utils.NewRequest("actdate", utils.RequestType.Form);
        string actcontent = utils.NewRequest("actcontent", utils.RequestType.Form);

        switch (typ)
        {
            case "unpay":
                sql = "INSERT INTO TCS_Mission (guid, nick, typ, content, group, timecount) VALUES ('" + guid + "','" + nick + "','" + typ + "','" + cuicontent + "','" + group + "','" + cuidate + "')";
                break;
            case "birthday":
                sql = "INSERT INTO TCS_Mission (guid, nick, typ, content, group) VALUES ('" + guid + "','" + nick + "','" + typ + "','" + birthdaycontent + "','" + group + "')";
                break;
            case "back":
                sql = "INSERT INTO TCS_Mission (guid, nick, typ, content, group, timecount) VALUES ('" + guid + "','" + nick + "','" + typ + "','" + backcontent + "','" + group + "','" + backdate + "')";
                break;
            case "act":
                sql = "INSERT INTO TCS_Mission (guid, nick, typ, content, group, senddate) VALUES ('" + guid + "','" + nick + "','" + typ + "','" + actcontent + "','" + group + "','" + actdate + "')";
                break;
        }

        utils.ExecuteNonQuery(sql);

        Response.Write(sql);
        //Response.Redirect("missionlist.aspx");
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
}