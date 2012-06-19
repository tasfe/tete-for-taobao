using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;


public partial class top_reviewnew_reviewindex : System.Web.UI.Page
{
    public string session = string.Empty;
    public string nick = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string id = utils.NewRequest("id", utils.RequestType.QueryString);
        string act = utils.NewRequest("act", utils.RequestType.QueryString);
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
        }

        if (act == "add")
        {
            InitTradeRateData(id);
            return;
        }

        if (act == "del")
        {
            DelTradeRateData(id);
            return;
        }

        BindData();
    }

    /// <summary>
    /// 删除首页展示的评价信息
    /// </summary>
    /// <param name="id"></param>
    private void DelTradeRateData(string id)
    {
        string sql = "UPDATE TCS_TradeRate SET isshow = 0 WHERE orderid = '" + id + "' AND nick = '" + nick + "'";
        utils.ExecuteNonQuery(sql);

        Response.Redirect("reviewindex.aspx");
    }

    /// <summary>
    /// 加入首页展示的评价信息
    /// </summary>
    /// <param name="id"></param>
    private void InitTradeRateData(string id)
    {

        Response.Redirect("reviewindex.aspx");
    }

    public static string left(string str)
    {
        string newstr = string.Empty;
        if (str.Length < 25)
        {
            newstr = str;
        }
        else
        {
            newstr = "<span title='" + str + "'>" + str.Substring(0, 25) + "..</span>";
        }
        return newstr;
    }

    private void BindData()
    {
        string sql = "SELECT TOP 20 * FROM TCS_TradeRate WHERE nick = '" + nick + "' AND isshow = 1 ORDER BY showindex,reviewdate DESC";
        DataTable dt = utils.ExecuteDataTable(sql);

        rptArticle.DataSource = dt;
        rptArticle.DataBind();
    }
}