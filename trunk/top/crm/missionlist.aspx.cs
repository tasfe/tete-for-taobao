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

public partial class top_crm_missionlist : System.Web.UI.Page
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

            string msg = "尊敬的" + nick + "，非常抱歉的告诉您，只有VIP版本才能使用【客户关系营销】功能，如需继续使用请<a href='http://fuwu.taobao.com/ser/detail.htm?service_code=service-0-22904&frm=haoping' target='_blank'>购买高级会员服务</a>，谢谢！";
            Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
            Response.End();
            return;
        }
        
        string act = utils.NewRequest("act", utils.RequestType.QueryString);

        if (act == "del")
        {
            string sql = "UPDATE TCS_Mission SET isdel = 1 WHERE nick = '" + nick + "' AND guid = '" + id + "'";
            utils.ExecuteNonQuery(sql);
            Response.Redirect("missionlist.aspx");
            return;
        }

        BindData();
    }


    private void BindData()
    {
        string sql = "SELECT * FROM TCS_Mission WHERE nick = '"+nick+"' AND isdel = 0 ORDER BY adddate DESC";
        DataTable dt = utils.ExecuteDataTable(sql);

        rptArticle.DataSource = dt;
        rptArticle.DataBind();
    }

    public static string checkGrade(string grade, string typ)
    {
        string str = string.Empty;
        string sql = string.Empty;

        if (typ == "birthday" || typ == "back")
        {
            return "全部会员";
        }

        if (typ == "unpay")
        {
            return "未成功购买的会员";
        }

        if (grade.Length > 5)
        {
            sql = "SELECT name,count FROM TCS_Group WHERE guid = '" + grade + "' AND isdel = 0";
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                str = dt.Rows[0][0].ToString() + "(" + dt.Rows[0][1].ToString() + ")";
            }
        }
        else
        {
            if (grade == "0")
            {
                str = "未成功购买的会员";
            }
            else if (grade == "1")
            {
                str = "购买过一次的会员";
            }
            else if (grade == "2")
            {
                str = "购买过多次的会员";
            }
            else if (grade == "a")
            {
                str = "未购买";
            }
            else if (grade == "b")
            {
                str = "普通会员";
            }
            else if (grade == "c")
            {
                str = "高级会员";
            }
            else if (grade == "d")
            {
                str = "VIP会员";
            }
            else if (grade == "e")
            {
                str = "至尊VIP会员";
            }
            else
            {
                str = "全部会员";
            }
        }

        return str;
    }

    public static string gettyp(string grade)
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


    public static string getstop(string grade)
    {
        string str = string.Empty;

        switch (grade)
        {
            case "0":
                str = "<span style='#000'>正常执行</span>";
                break;
            case "1":
                str = "<span style='color:red'>暂停中</span>";
                break;
        }

        return str;
    }


    public static string getsend(string grade)
    {
        string str = string.Empty;

        if (grade.Length == 0)
        {
            str = "<span style='#000'>长期执行</span>";
        }
        else
        {
            str = "<span style='color:red'>" + grade + "</span>";

        }

        return str;
    }

    public static string getsends(string grade, string typ)
    {
        string str = string.Empty;

        if (typ == "act")
        {
            if (grade == "0")
            {
                str = "<span style='#000'>等待发送</span>";
            }
            else if (grade == "2")
            {
                str = "<span style='color:green'>发送中</span>";

            }
            else
            {
                str = "<span style='color:red'>发送成功</span>";

            }
        }
        else
        {
            str = "<span style='color:green'>发送中</span>";
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