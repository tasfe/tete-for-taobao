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

public partial class top_crm_missionmodify : System.Web.UI.Page
{
    public string session = string.Empty;
    public string nick = string.Empty;
    public string id = string.Empty;
    public string now = string.Empty;

    public string typ = string.Empty;
    public string group = string.Empty;
    public string content = string.Empty;
    public string grade = string.Empty;
    public string isstop = string.Empty;
    public string timecount = string.Empty;
    public string senddate = string.Empty;
    public string index = string.Empty;

    public string startsend = string.Empty;
    public string endsend = string.Empty;
    public string price = string.Empty;
    public string ispayone = string.Empty;
    public string isclose = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
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

            string msg = "尊敬的" + nick + "，非常抱歉的告诉您，只有VIP版本才能使用【客户关系营销】功能，如需继续使用请<a href='http://fuwu.taobao.com/ser/detail.htm?service_code=service-0-22904&frm=haoping' target='_blank'>购买高级会员服务</a>，谢谢！";
            Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
            Response.End();
            return;
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
        string id = utils.NewRequest("id", utils.RequestType.QueryString);

        string sql = "SELECT * FROM TCS_Mission WHERE guid = '" + id + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            typ = dt.Rows[0]["typ"].ToString();
            content = dt.Rows[0]["content"].ToString();
            grade = dt.Rows[0]["grade"].ToString();
            isstop = dt.Rows[0]["isstop"].ToString();
            timecount = dt.Rows[0]["timecount"].ToString();
            senddate = dt.Rows[0]["senddate"].ToString();
            index = gettyp(dt.Rows[0]["typ"].ToString());

            startsend = dt.Rows[0]["startsend"].ToString();
            endsend = dt.Rows[0]["endsend"].ToString();
            price = dt.Rows[0]["price"].ToString();
            ispayone = dt.Rows[0]["ispayone"].ToString();
            isclose = dt.Rows[0]["isclose"].ToString();
        }
    }

    public static string getcheck(string str)
    {
        if (str == "1")
            return "checked";
        else
            return "";
    }

    public static string gettypinfo(string grade)
    {
        string str = string.Empty;

        switch (grade)
        {
            case "unpay":
                str = "<span style='#000'>未付款订单催单</span>";
                break;
            case "birthday":
                str = "<span style='color:blue'>客户生日关怀</span>";
                break;
            case "back":
                str = "<span style='color:green'>买家定期回访</span>";
                break;
            case "act":
                str = "<span style='color:red'>新品活动营销</span>";
                break;
        }

        return str;
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string id = utils.NewRequest("id", utils.RequestType.QueryString);
        string typ = utils.NewRequest("typ", utils.RequestType.Form);
        string group = utils.NewRequest("group", utils.RequestType.Form);
        string cuicontent = utils.NewRequest("cuicontent", utils.RequestType.Form);
        string cuidate = utils.NewRequest("cuidate", utils.RequestType.Form);
        string birthdaycontent = utils.NewRequest("birthdaycontent", utils.RequestType.Form);
        string backdate = utils.NewRequest("backdate", utils.RequestType.Form);
        string backcontent = utils.NewRequest("backcontent", utils.RequestType.Form);
        string actdate = utils.NewRequest("actdate", utils.RequestType.Form);
        string actcontent = utils.NewRequest("actcontent", utils.RequestType.Form);
        string isstop = utils.NewRequest("isstop", utils.RequestType.Form);
        string sql = string.Empty;

        string startsend = utils.NewRequest("startsend", utils.RequestType.Form);
        string endsend = utils.NewRequest("endsend", utils.RequestType.Form);
        string price = utils.NewRequest("price", utils.RequestType.Form);
        string ispayone = utils.NewRequest("ispayone", utils.RequestType.Form);
        string isclose = utils.NewRequest("isclose", utils.RequestType.Form);

        switch (typ)
        {
            case "unpay":
                sql = "UPDATE TCS_Mission SET typ = '" + typ + "',content = '" + cuicontent + "',timecount = '" + cuidate + "',isstop = '" + isstop + "',startsend = '" + startsend + "',endsend = '" + endsend + "',price = '" + price + "',ispayone = '" + ispayone + "',isclose = '" + isclose + "' WHERE nick = '" + nick + "' AND guid = '" + id + "'";
                break;
            case "birthday":
                sql = "UPDATE TCS_Mission SET typ = '" + typ + "',content = '" + birthdaycontent + "',isstop = '" + isstop + "' WHERE nick = '" + nick + "' AND guid = '" + id + "'";
                break;
            case "back":
                sql = "UPDATE TCS_Mission SET typ = '" + typ + "',content = '" + backcontent + "',timecount = '" + backdate + "',isstop = '" + isstop + "' WHERE nick = '" + nick + "' AND guid = '" + id + "'";
                break;
            case "act":
                sql = "UPDATE TCS_Mission SET typ = '" + typ + "',content = '" + actcontent + "',senddate = '" + actdate + "',isstop = '" + isstop + "' WHERE nick = '" + nick + "' AND guid = '" + id + "'";
                break;
        }

        utils.ExecuteNonQuery(sql);

        Response.Redirect("missionlist.aspx");
    }

    public static string gettyp(string grade)
    {
        string str = string.Empty;

        switch (grade)
        {
            case "unpay":
                str = "1";
                break;
            case "birthday":
                str = "2";
                break;
            case "back":
                str = "3";
                break;
            case "act":
                str = "4";
                break;
        }

        return str;
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