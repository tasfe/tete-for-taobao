using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;

public partial class top_crm_groupadd : System.Web.UI.Page
{
    public string session = string.Empty;
    public string nick = string.Empty;
    public string id = string.Empty;
    public string now = string.Empty;
    public string totalcustomer = string.Empty;

    public string startdatejs = string.Empty;
    public string enddatejs = string.Empty;

    public string startdate = string.Empty;
    public string enddate = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string id = utils.NewRequest("id", utils.RequestType.QueryString);
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        session = cookie.getCookie("top_sessiongroupbuy");
        string iscrm = cookie.getCookie("iscrm");
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Decrypt(taobaoNick);

        startdate = DateTime.Now.AddMonths(-12).ToShortDateString();
        enddate = DateTime.Now.ToShortDateString();

        DateTime start = DateTime.Now.AddMonths(-12);
        DateTime end = DateTime.Now;

        startdatejs = start.Month.ToString() + "/" + start.Day.ToString() + "/" + start.Year.ToString();
        enddatejs = end.Month.ToString() + "/" + end.Day.ToString() + "/" + end.Year.ToString();


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

            string msg = "尊敬的" + nick + "，非常抱歉的告诉您，只有VIP版本才能使用【客户关系营销】功能，如需继续使用请<a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22904-3:1;' target='_blank'>购买高级会员服务</a>，谢谢！";
            Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
            Response.End();
            return;
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string name = utils.NewRequest("name", utils.RequestType.Form);
        string price = utils.NewRequest("price", utils.RequestType.Form);
        string priceend = utils.NewRequest("priceend", utils.RequestType.Form);
        string actdate = utils.NewRequest("actdate", utils.RequestType.Form);
        string actdateend = utils.NewRequest("actdateend", utils.RequestType.Form);
        string area = utils.NewRequest("area", utils.RequestType.Form);
        string id = Guid.NewGuid().ToString();
        string str = string.Empty;

        string left = string.Empty;
        string right = string.Empty;
        string condition = string.Empty;

        string sql = string.Empty;// "SELECT COUNT(*) FROM TCS_Group WHERE nick = '" + nick + "' AND price = '" + price + "' AND isdel = 0";
        //string count = utils.ExecuteString(sql);
        //if (count != "0")
        //{
        //    Response.Write("<script>alert('该价格的会员组已经存在，请修改价格！');history.go(-1);</script>");
        //    Response.End();
        //    return;
        //}

        left = "guid,name,nick";
        right = "'" + id + "','" + name + "','" + nick + "'";
        if (price.Length != 0)
        {
            left += ",price";
            right += ",'" + price + "'";
            condition += " AND tradeamount >= '" + price + "'";
        }
        if (priceend.Length != 0)
        {
            left += ",priceend";
            right += ",'" + priceend + "'";
            condition += " AND tradeamount <= '" + price + "'";
        }

        left += ",arealist";
        right += ",'" + area + "'";
        condition += " AND CHARINDEX(REPLACE(sheng,'省',''),'" + area + "') > 0";

        if (actdate.Length != 0)
        {
            left += ",actdate";
            right += ",'" + actdate + "'";
            condition += " AND lastorderdate >= '" + actdate + "'";
        }
        if (actdateend.Length != 0)
        {
            left += ",actdateend";
            right += ",'" + actdateend + "'";
            condition += " AND lastorderdate <= '" + actdateend + "'";
        }

        sql = "INSERT INTO TCS_Group (" + left + ") VALUES (" + right + ")";
        Response.Write(sql);
        Response.Write("<br>");
        utils.ExecuteNonQuery(sql);

        //获取符合条件的会员并更新会员分组ID
        sql = "UPDATE TCS_Customer SET groupguid = '" + id + "' WHERE nick = '" + nick + "' " + condition;
        Response.Write(sql);
        Response.Write("<br>");
        utils.ExecuteNonQuery(sql);

        //获取总数并更新
        sql = "UPDATE TCS_Group SET count = (SELECT COUNT(*) FROM TCS_Customer WHERE groupguid = '" + id + "') WHERE guid = '" + id + "'";
        Response.Write(sql);
        Response.Write("<br>");
        utils.ExecuteNonQuery(sql);

        Response.Redirect("grouplist.aspx");
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