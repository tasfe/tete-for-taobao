using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;

public partial class top_crm_grouplist : System.Web.UI.Page
{
    public string session = string.Empty;
    public string nick = string.Empty;
    public string id = string.Empty;
    public string now = string.Empty;
    public string totalcustomer = string.Empty;
    public string act = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        id = utils.NewRequest("id", utils.RequestType.QueryString);
        string act = utils.NewRequest("act", utils.RequestType.QueryString);
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
            if (act == "update")
            {
                UpdateData();
            }
            else if (act == "del")
            {
                DeleteData();
            }
            else
            {
                BindData();
            }
        }
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    private void DeleteData()
    {
        string sql = "UPDATE TCS_Group SET isdel = 1 WHERE nick = '" + nick + "' AND guid = '"+id+"'";
        utils.ExecuteNonQuery(sql);

        sql = "UPDATE TCS_Customer SET groupguid = '' WHERE nick = '" + nick + "' AND groupguid = '" + id + "'";
        utils.ExecuteNonQuery(sql);

        Response.Write("<script>alert('删除成功！');window.location.href='grouplist.aspx';</script>");
        Response.End();
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    private void UpdateData()
    {
        string sql = "SELECT * FROM TCS_Group WHERE nick = '" + nick + "' AND isdel = 0 ORDER BY price ASC";
        //Response.Write(sql);
        DataTable dt = utils.ExecuteDataTable(sql);
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            //如果是最后一个
            if (i == dt.Rows.Count - 1)
            {
                //获取符合条件的会员并更新会员分组ID
                sql = "UPDATE TCS_Customer SET groupguid = '" + dt.Rows[i]["guid"].ToString() + "' WHERE nick = '" + nick + "' AND tradeamount <> '' AND tradecount > 0 AND cast(tradeamount as decimal(18,2)) >= " + dt.Rows[i]["price"].ToString() + "";
                //Response.Write(sql);
                utils.ExecuteNonQuery(sql);
            }
            else
            {
                //获取符合条件的会员并更新会员分组ID
                sql = "UPDATE TCS_Customer SET groupguid = '" + dt.Rows[i]["guid"].ToString() + "' WHERE nick = '" + nick + "' AND tradeamount <> '' AND tradecount > 0 AND cast(tradeamount as decimal(18,2)) >= " + dt.Rows[i]["price"].ToString() + " AND cast(tradeamount as decimal(18,2)) < " + dt.Rows[i + 1]["price"].ToString() + "";
                //Response.Write(sql);
                utils.ExecuteNonQuery(sql);
            }

            //获取总数并更新
            sql = "UPDATE TCS_Group SET count = (SELECT COUNT(*) FROM TCS_Customer WHERE groupguid = '" + dt.Rows[i]["guid"].ToString() + "') WHERE guid = '" + dt.Rows[i]["guid"].ToString() + "'";
            //Response.Write(sql);
            utils.ExecuteNonQuery(sql);
        }

        Response.Write("<script>alert('会员组会员更新成功！');window.location.href='grouplist.aspx';</script>");
        Response.End();
    }

    private void BindData()
    {
        string sql = "SELECT * FROM TCS_Group WHERE nick = '" + nick + "' AND isdel = 0 ORDER BY price ASC";
        DataTable dt = utils.ExecuteDataTable(sql);

        rptArticle.DataSource = dt;
        rptArticle.DataBind();
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