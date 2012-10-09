using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;
using System.IO;
using System.Text;

public partial class top_review_couponsend : System.Web.UI.Page
{
    public string session = string.Empty;
    public string nick = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string id = utils.NewRequest("id", utils.RequestType.QueryString);
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

        if (!IsPostBack)
        {
            string buynick = utils.NewRequest("buynick", utils.RequestType.QueryString);
            if (buynick.Length == 0)
            {
                BindData();
            }
            else
            {
                ShowSearchData(buynick);
            }
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (search.Text.Length == 0)
        {
            Response.Redirect("couponsend.aspx");
            return;
        }

        ShowSearchData(search.Text.Replace("'", "''"));
    }


    protected void Button1_Click(object sender, EventArgs e)
    {

        StringBuilder builder = new StringBuilder();
        string sql = "SELECT s.*,c.num,c.condition,t.totalprice,c.name FROM TCS_CouponSend s LEFT JOIN TCS_Coupon c ON c.guid = s.guid LEFT JOIN TCS_Trade t ON t.orderid = s.orderid WHERE nick = '" + nick + "' ORDER BY s.taobaonumber DESC";
        DataTable dt = utils.ExecuteDataTable(sql);
        builder.Append("名称,优惠券编号,买家,优惠金额,订单号,订单金额,赠送日期");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            builder.Append("\r\n");
            builder.Append(dt.Rows[i]["name"].ToString());
            builder.Append(",");
            builder.Append(dt.Rows[i]["taobaonumber"].ToString());
            builder.Append(",");
            builder.Append(dt.Rows[i]["buynick"].ToString());
            builder.Append(",");
            builder.Append("满" + dt.Rows[i]["condition"].ToString() + "元减" + dt.Rows[i]["num"].ToString() + "元");
            builder.Append(",");
            builder.Append(dt.Rows[i]["orderid"].ToString());
            builder.Append(",");
            builder.Append(dt.Rows[i]["totalprice"].ToString());
            builder.Append(",");
            builder.Append(dt.Rows[i]["senddate"].ToString());
        }
        //生成excel文件
        string fileName = "tmp/" + nick + DateTime.Now.Ticks.ToString() + ".csv";
        File.WriteAllText(Server.MapPath(fileName), builder.ToString(), Encoding.Default);

        Response.Redirect(fileName);
    }

    private void ShowSearchData(string buynick)
    {
        string sqlNew = "SELECT s.*,c.num,c.condition,t.totalprice,c.name FROM TCS_CouponSend s LEFT JOIN TCS_Coupon c ON c.guid = s.guid LEFT JOIN TCS_Trade t ON t.orderid = s.orderid WHERE s.nick = '" + nick + "' AND s.buynick = '" + buynick + "' ORDER BY s.taobaonumber DESC";
        DataTable dt = utils.ExecuteDataTable(sqlNew);

        rptArticle.DataSource = dt;
        rptArticle.DataBind();

        lbPage.Text = "";
    }

    private void BindData()
    {
        string page = utils.NewRequest("page", utils.RequestType.QueryString);
        int pageNow = 1;
        if (page == "")
        {
            pageNow = 1;
        }
        else
        {
            pageNow = int.Parse(page);
        }
        int pageCount = 12;
        int dataCount = (pageNow - 1) * pageCount;

        //string sqlCoupon = "SELECT * FROM TopCoupon WHERE coupon_id = " + ; 

        string sqlNew = "SELECT TOP " + pageCount.ToString() + " * FROM (SELECT s.*,c.num,c.condition,t.totalprice,c.name,ROW_NUMBER() OVER (ORDER BY s.taobaonumber DESC) AS rownumber FROM TCS_CouponSend s LEFT JOIN TCS_Coupon c ON c.guid = s.guid LEFT JOIN TCS_Trade t ON t.orderid = s.orderid WHERE s.nick = '" + nick + "') AS a WHERE a.rownumber > " + dataCount.ToString() + " ORDER BY taobaonumber DESC";
        DataTable dt = utils.ExecuteDataTable(sqlNew);
        //Response.Write(sqlNew);
        rptArticle.DataSource = dt;
        rptArticle.DataBind();

        //分页数据初始化
        sqlNew = "SELECT COUNT(*) FROM TCS_CouponSend WHERE nick = '" + nick + "'";
        int totalCount = int.Parse(utils.ExecuteString(sqlNew));

        lbPage.Text = InitPageStr(totalCount, "couponsend.aspx");

        ////string sql = "SELECT s.*,c.coupon_name FROM TopCouponSend s INNER JOIN TopCoupon c ON c.coupon_id = s.couponid WHERE s.nick = '" + nick + "' ORDER BY s.id DESC";
        ////DataTable dt = utils.ExecuteDataTable(sql);

        //rptArticle.DataSource = dt;
        //rptArticle.DataBind();
    }


    private string InitPageStr(int total, string url)
    {
        //分页数据初始化
        string str = string.Empty;
        int pageCount = 12;
        int pageSize = 0;
        int pageNow = 1;
        string page = utils.NewRequest("page", utils.RequestType.QueryString);
        if (page == "")
        {
            pageNow = 1;
        }
        else
        {
            pageNow = int.Parse(page);
        }

        //取总分页数
        if (total % pageCount == 0)
        {
            pageSize = total / pageCount;
        }
        else
        {
            pageSize = total / pageCount + 1;
        }

        //如果总页面大于20，则最大页面差不超过20
        int start = 1;
        int end = 20;

        if (pageSize < end)
        {
            end = pageSize;
        }
        else
        {
            if (pageNow > 15)
            {
                start = pageNow - 10;

                if (pageNow < (total - 10))
                {
                    end = pageNow + 10;
                }
                else
                {
                    end = total;
                }
            }
        }

        for (int i = start; i <= end; i++)
        {
            if (i.ToString() == pageNow.ToString())
            {
                str += i.ToString() + " ";
            }
            else
            {
                str += "<a href='" + url + "?page=" + i.ToString() + "'>[" + i.ToString() + "]</a> ";
            }
        }

        return str;
    }
}