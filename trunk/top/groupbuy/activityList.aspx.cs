using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;

public partial class top_groupbuy_activityList : System.Web.UI.Page
{
    public string nickencode = string.Empty;
    public string statusStr = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");

        //过期判断
        if (string.IsNullOrEmpty(taobaoNick))
        {
            string msg = "尊敬的淘宝卖家，非常抱歉的告诉您，您还没有购买此服务或者登录信息已失效，如需继续使用请<a href='http://fuwu.taobao.com/serv/detail.htm?service_id=11807' target='_blank'>进入该服务</a>，谢谢！";
           // Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
          //  Response.End();
           // return;
        }
        statusStr = "";
        if (Request.QueryString["actionType"] != null)
        {
            if (Request.QueryString["actionType"].ToString() == "all")
            {
                liID.Value = "li1";
                statusStr = "";

            }
            if (Request.QueryString["actionType"].ToString() == "before")
            {
                liID.Value = "li2";
                statusStr = " and [Status]=0 ";
            }
            if (Request.QueryString["actionType"].ToString() == "start")
            {
                liID.Value = "li3";
                statusStr = " and [Status]=1 ";
            }
            if (Request.QueryString["actionType"].ToString() == "pause")
            {
                liID.Value = "li4";
                statusStr = " and [Status]=3 ";
            }
            if (Request.QueryString["actionType"].ToString() == "stop")
            {
                liID.Value = "li5";
                statusStr = " and [Status]=2 ";
            }
        }
        //获取买家的促销信息清单
        BindData();

    }

    /// <summary>
    /// 输出HTML
    /// </summary>
    /// <param name="status">0:未开始，1:进行中，2：已结束，3已暂停</param>
    /// <param name="actionID">活动ID</param>
    /// <returns></returns>
    public string outShowHtml(string status,string actionID)
    {
        string html = "";
        if (status == "0")
        {
            html = "<div> <a href=\"activityView.aspx?activityID=" + actionID + "&tp=update\" >修改此活动<div ><a href=\"activityView.aspx?activityID=" + actionID + "&tp=del\" title=''>删除此活动</a></div>";
        }
        else if (status == "1")
        {
            html = " <div > <a href=\"activityView.aspx?activityID=" + actionID + "&tp=update\" >  修改此活动 </a>  </div> <div style=\"height: 10px\"></div>  <div ><a href=\"activityView.aspx?activityID=" + actionID + "&tp=pause\" title=''>暂停此活动</a></div>  <div style=\"height: 10px\"></div> <div ><a href=\"activityView.aspx?activityID=" + actionID + "&tp=del\" title=''>删除此活动</a></div>";
        }
        else if (status == "2")
        {
            html = "<div ><a href=\"activityView.aspx?activityID=" + actionID + "&tp=yc\" >延长活动时间 </a> </div><div style=\"height: 10px\"></div> <div ><a href=\"activityView.aspx?activityID=" + actionID + "&tp=del\" title=''>删除此活动</a></div>";
        }
        else if (status == "3")
        {
            html = " <div>  <a href=\"activityView.aspx?activityID=" + actionID + "&tp=hf\" title=''>恢复此活动</a> </div> <div style=\"height: 10px\"></div> <div><a href=\"activityView.aspx?activityID=" + actionID + "&tp=del\" title=''>删除此活动</a></div>";
        }
        return html;
    }

    private void BindData()
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        nickencode = HttpUtility.UrlEncode(taobaoNick);

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

        string sqlNew = "SELECT TOP " + pageCount.ToString() + " * FROM (SELECT *,ROW_NUMBER() OVER (ORDER BY b.id DESC) AS rownumber FROM tete_activity b WHERE b.nick = '" + taobaoNick + "' " + statusStr + " and Status<>4 ) AS a WHERE a.rownumber > " + dataCount.ToString() + " ORDER BY id DESC";

         
       // DataTable dtNew = utils.ExecuteDataTable(sqlNew);
      //  rptArticle.DataSource = dtNew;
        rptArticle.DataBind();
//
        //分页数据初始化
        sqlNew = "SELECT COUNT(*) FROM tete_activity WHERE nick = '" + taobaoNick + "' " + statusStr + "  and Status<>4 ";
        int totalCount = 0;// int.Parse(utils.ExecuteString(sqlNew));

        lbPage.Text = InitPageStr(totalCount, "activityList.aspx");
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
                str += "<a href='" + url + "?page=" + i.ToString() + "'>[" + i.ToString() + "]</a> ";
            }
        }

        return str;
    }
}