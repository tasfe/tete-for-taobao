using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;


public partial class top_crm_groupmodify : System.Web.UI.Page
{
    public string session = string.Empty;
    public string nick = string.Empty;
    public string id = string.Empty;
    public string now = string.Empty;
    public string totalcustomer = string.Empty;
    public string act = string.Empty;

    public string name = string.Empty;
    public string price = string.Empty;

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
            else
            {
                BindData();
            }
        }
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    private void UpdateData()
    {
        string name = utils.NewRequest("name", utils.RequestType.Form);
        string price = utils.NewRequest("price", utils.RequestType.Form);

        string sql = "SELECT COUNT(*) FROM TCS_Group WHERE nick = '" + nick + "' AND price = '" + price + "' AND isdel = 0";
        string count = utils.ExecuteString(sql);
        if (count != "0")
        {
            Response.Write("<script>alert('该价格的会员组已经存在，请修改价格！');history.go(-1);</script>");
            Response.End();
            return;
        }

        sql = "UPDATE TCS_Group SET name='" + name + "',price='" + price + "' WHERE nick = '" + nick + "' AND guid = '" + id + "'";
        utils.ExecuteNonQuery(sql);

        Response.Write("<script>alert('更新成功！');window.location.href='grouplist.aspx';</script>");
        Response.End();
    }


    private void BindData()
    {
        string sql = "SELECT * FROM TCS_Group WHERE nick = '" + nick + "' AND guid = '"+id+"'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            name = dt.Rows[0]["name"].ToString();
            price = dt.Rows[0]["price"].ToString();
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
    protected void Button1_Click(object sender, EventArgs e)
    {
        UpdateData();
    }
}