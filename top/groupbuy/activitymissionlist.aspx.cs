using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;


public partial class top_groupbuy_activitymissionlist : System.Web.UI.Page
{
    public string nickencode = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        //获取买家的团购信息清单
        BindData();
    }

    private void BindData()
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        //过期判断
        if (string.IsNullOrEmpty(taobaoNick))
        {
            string msg = "尊敬的淘宝卖家，非常抱歉的告诉您，您还没有购买此服务或者登录信息已失效，如需继续使用请<a href='http://fuwu.taobao.com/serv/detail.htm?service_id=11807' target='_blank'>进入该服务</a>，谢谢！";
            Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
            Response.End();
            return;
        }

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

        string sqlNew = "SELECT TOP " + pageCount.ToString() + " * FROM (SELECT b.*,tete_activity.name,tete_shoptemplet.title,ROW_NUMBER() OVER (ORDER BY b.id DESC) AS rownumber FROM tete_ActivityMission b left join tete_activity on b.ActivityID=tete_activity.id left join tete_shoptemplet on b.shoptempletID=tete_shoptemplet.id WHERE b.nick = '" + taobaoNick + "') AS a WHERE a.rownumber > " + dataCount.ToString() + " ORDER BY id DESC";

        //string sql = "SELECT * FROM TopGroupBuy WHERE nick = '" + taobaoNick + "' ORDER BY id DESC";
        DataTable dtNew = utils.ExecuteDataTable(sqlNew);
        rptArticle.DataSource = dtNew;
        rptArticle.DataBind();

        //分页数据初始化
        sqlNew = "SELECT COUNT(*) FROM tete_ActivityMission WHERE nick = '" + taobaoNick + "'";
        int totalCount = int.Parse(utils.ExecuteString(sqlNew));

        lbPage.Text = InitPageStr(totalCount, "activitymissionlist.aspx");
    }

    public static string result(string msg)
    {
        if (msg == "0")
        {
            return "<b style=\"color:red\">进行中</b>";
        }
        else if (msg == "-1")
        {
            return "<b style=\"color:green\">更新失败 </b>";
        }
        else
        {
            return "<b style=\"color:green\">已完成</b>";
        }
    }

    public static string typ(string msg)
    {
        if (msg == "write")
        {
            return "更新宝贝描述";
        }
        else
        {
            return "清除关联描述";
        }
    }

    public static string typSuccess(string msg)
    {
        if (msg == "write")
        {
            return "Write" + DateTime.Now.ToString("yyyyMMdd") + "/";
        }
        else
        {
            return "Write" + DateTime.Now.ToString("yyyyMMdd") + "/Delete";
        }
    }

    public static string typErr(string msg)
    {
        if (msg == "write")
        {
            return "Write" + DateTime.Now.ToString("yyyyMMdd") + "/Err";
        }
        else
        {
            return "Write" + DateTime.Now.ToString("yyyyMMdd") + "/DeleteErr";
        }
    }

    public static string typSTRfile(string groupbuyID)
    {
        return groupbuyID + DateTime.Now.ToString("yyyyMMdd") + ".txt";

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