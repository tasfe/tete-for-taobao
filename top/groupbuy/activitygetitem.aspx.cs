using System;
using System.Collections.Generic;
using System.Web;
using Common;
using System.Data;
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;

public partial class top_groupbuy_activitygetitem : System.Web.UI.Page
{
    public string teteendDate = string.Empty;
    public string nick = string.Empty;
    public string name = "";
    public string memo = "";
    public string startDate = "";
    public string endDate = "";
    public string itemType = "";
    public string discountType = "";
    public string zhe = "";
    public string yuan = "";
    public string decreaseNum = "";
    public string rcount = "";
    public string tagId = "";
    public string status = "";
    public string itemTypeStr = "";
    public string discountTypeStr = "";
    public string decreaseNumStr = "";
    public string discountValue = "";
    public string activityenddatestr = string.Empty;
    public string activitystatrdatestr = string.Empty;
    public string activitystatusstr = string.Empty;
    public string activityitemTypestr = string.Empty;
    public string activitydiscountTypestr = string.Empty;
    public string activitydiscountValuestr = string.Empty;
    public string activitydecreaseNumstr = string.Empty;
    public string addactivity = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["tp"] != null)
        {
            //删除活动
            string sql2 = "delete from tete_activitylist where  id=" + Request.QueryString["ID"].ToString();
            utils.ExecuteNonQuery(sql2);
            Response.Redirect("activitygetitem.aspx");
            return;
        }

        Cookie cookie = new Cookie();
        string taobaoNick = cookie.getCookie("nick");
        string session = cookie.getCookie("top_sessiongroupbuy");

        if (Request.QueryString["activityID"] == null)
        {
            Response.Redirect("activityList.aspx");
            return;
        }
      
        string activityID = Request.QueryString["activityID"].ToString();
        addactivity = "<a href=\"activitytaobaoItem.aspx?activityID=" + activityID + "\">添加促销宝贝</a>   <a href=\"activityList.aspx\">返回活动列表</a>";
        activityIDstr.Value = activityID;
        string sql = "SELECT *  FROM  [tete_activity] where ID=" + activityID;
        DataTable dt3 = utils.ExecuteDataTable(sql);
        if (dt3 != null && dt3.Rows.Count > 0)
        {
            activityenddatestr = dt3.Rows[0]["endDate"].ToString();
            activitystatrdatestr = dt3.Rows[0]["startDate"].ToString();
            activitystatusstr = dt3.Rows[0]["Status"].ToString();
            if (activitystatusstr == "0")
            {
                activitystatusstr = "活动未开始";
            }
            else if (activitystatusstr == "1")
            {
                activitystatusstr = "活动进行中";
            }
            else if (activitystatusstr == "2")
            {
                activitystatusstr = "活动已结束";
            }
            else if (activitystatusstr == "3")
            {
                activitystatusstr = "活动已暂停";
            }
            else
            {
                activitystatusstr = "活动已删除";
            }
            activityitemTypestr = dt3.Rows[0]["itemType"].ToString();
            if (activityitemTypestr == "same")
            {
                activitydiscountTypestr = dt3.Rows[0]["discountType"].ToString();
                activitydiscountValuestr = dt3.Rows[0]["discountValue"].ToString();
                activitydecreaseNumstr = dt3.Rows[0]["decreaseNum"].ToString();

                if (activitydiscountTypestr.Trim() == "DISCOUNT")
                {
                    activitydiscountTypestr = "打折";
                    activitydiscountValuestr += "折";
                    activitydecreaseNumstr = "买家拍下的多件商品均享受优惠";
                }
                else
                {
                    activitydiscountTypestr = "减价";
                    activitydiscountValuestr += "元";
                    if (activitydecreaseNumstr == "0")
                    {
                        activitydecreaseNumstr = "买家拍下的多件商品均享受优惠";
                    }
                    else
                    {
                        activitydecreaseNumstr = "只对买家拍下的第一件商品优惠";
                    }
                }

            }
            else
            {

                activitydiscountTypestr = "每个参加活动的宝贝设置不同促销力度 ";
                activitydiscountValuestr = "每个参加活动的宝贝设置不同促销力度 ";
                activitydecreaseNumstr = "每个参加活动的宝贝设置不同促销力度 ";
            }

        }
        //Status 活动状态(0:未开始，1:进行中，2：已结束，3已暂停，4已删除)

        //COOKIE过期判断
        if (taobaoNick == "")
        {
            //SESSION超期 跳转到登录页
            Response.Write("<script>parent.location.href='http://container.open.taobao.com/container?appkey=12159997'</script>");
            Response.End();
        }

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        if (!IsPostBack)
        {
            BindData();
        }

    }

    /// <summary>
    /// 输出HTML
    /// </summary> 
    /// <param name="actionID">活动ID</param>
    /// <returns></returns>
    public string outShowHtml(string ID, string actionID)
    {
        string html = "<div> <a href=\"activitylistView.aspx?activityID=" + actionID + "&ID=" + ID + "\" >修改此活动<div ><a href=\"activitygetitem.aspx?activityID=" + actionID + "&ID=" + ID + "&tp=del\" title=''>删除此活动</a></div>";
        
        return html;
    }

    private void BindData()
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        taobaoNick = HttpUtility.UrlEncode(taobaoNick);

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
        int pageCount = 5;
        int dataCount = (pageNow - 1) * pageCount;

        string sqlNew = "SELECT TOP " + pageCount.ToString() + " * FROM (SELECT *,ROW_NUMBER() OVER (ORDER BY b.id DESC) AS rownumber FROM Tete_activitylist b WHERE b.nick = '" + taobaoNick + "' and  ActivityID=" + Request.QueryString["activityID"].ToString() + " and Status<>4 ) AS a WHERE a.rownumber > " + dataCount.ToString() + " ORDER BY id DESC";


        DataTable dtNew = utils.ExecuteDataTable(sqlNew);
        rptItems.DataSource = dtNew;
        rptItems.DataBind();
        //
        //分页数据初始化
        sqlNew = "SELECT COUNT(*) FROM Tete_activitylist WHERE nick = '" + taobaoNick + "' and  ActivityID=" + Request.QueryString["activityID"].ToString() + "  and Status<>4 ";
        int totalCount = int.Parse(utils.ExecuteString(sqlNew));

        lbPage.Text = InitPageStr(totalCount, "activitygetitem.aspx");
    }

    private string InitPageStr(int total, string url)
    {
        //分页数据初始化
        string str = string.Empty;
        int pageCount = 5;
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

        for (int i = 1; i <= pageSize; i++)
        {
            if (i.ToString() == pageNow.ToString())
            {
                str += i.ToString() + " ";
            }
            else
            {
                str += "<a href='" + url + "?activityID=" + Request.QueryString["activityID"].ToString() + "&page=" + i.ToString() + "'>[" + i.ToString() + "]</a> ";
            }
        }

        return str;
    }
}