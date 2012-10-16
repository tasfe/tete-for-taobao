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

public partial class top_crm_custommodify : System.Web.UI.Page
{
    public string session = string.Empty;
    public string nick = string.Empty;
    public string buynick = string.Empty;
    public string birthday = string.Empty;
    public string id = string.Empty;
    public string now = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        id = utils.NewRequest("id", utils.RequestType.QueryString);
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
        string sql = "SELECT * FROM TCS_Customer WHERE nick = '" + nick + "' AND guid = '" + id + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            buynick = dt.Rows[0]["buynick"].ToString();
            birthday = dt.Rows[0]["birthday"].ToString().Replace(" 0:00:00", "");
            if (birthday == "1900-1-1")
            {
                birthday = "";
            }
            lbAddress.Text = dt.Rows[0]["address"].ToString();

            Label1.Text = dt.Rows[0]["sheng"].ToString();
            Label2.Text = dt.Rows[0]["shi"].ToString();
            Label3.Text = dt.Rows[0]["qu"].ToString();
            Label4.Text = dt.Rows[0]["truename"].ToString();
            Label5.Text = dt.Rows[0]["mobile"].ToString();
        }
    }


    protected void Button1_Click(object sender, EventArgs e)
    {
        string sql = string.Empty;
        string birthday = utils.NewRequest("birthday", utils.RequestType.Form);

        sql = "UPDATE TCS_Customer SET birthday = '" + birthday + "' WHERE nick = '" + nick + "' AND guid = '" + id + "'";
        utils.ExecuteNonQuery(sql);

        Response.Redirect("custommodify.aspx?id=" + id);
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