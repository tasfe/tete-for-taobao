using System;
using System.Collections.Generic; 
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;

public partial class top_groupbuy_activitytempmanage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["act"] != null&&Request.QueryString["act"].ToString()=="del")
        {
            if (Request.QueryString["id"] != null)
            {
                string id = Request.QueryString["id"].ToString();
                //删除模板
                utils.ExecuteNonQuery("update tete_shoptemplet set Isdelete=1 where id=" + id);
                //删除模板列表,10天后再删除
                //utils.ExecuteNonQuery("delete from tete_shoptempletlist where shoptempletID=" + id);
                Response.Redirect("activitytempmanage.aspx");
            }
        }
        if (!IsPostBack)
        {
            BindData();
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="ID"></param>
    /// <param name="actionID"></param>
    /// <param name="pid"></param>
    /// <returns></returns>
    public string outShowHtml(string ID)
    {
        string html = "<div>  <a  target=_blank href=\"activitytempView.aspx?tid=" + ID + "\")\">获取代码</a>   <a  target=_blank  href=\"activitytempView.aspx?ytid=" + ID + "\")\">预览</a>      <a href=\"activitytempmanage.aspx?act=del&id=" + ID + "\")\">删除</a></div>";

        return html;
    }

    private void BindData()
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        //taobaoNick = HttpUtility.UrlEncode(taobaoNick);

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
        int pageCount = 10;
        int dataCount = (pageNow - 1) * pageCount;
        //select [tete_shoptemplet].*, [tete_templet].name from [tete_shoptemplet] 
        //left join [tete_templet] on [tete_shoptemplet].[templetID]=[tete_templet].id
        string sqlNew = "SELECT TOP " + pageCount.ToString() + " * FROM (SELECT b.*,name,ROW_NUMBER() OVER (ORDER BY b.id DESC) AS rownumber FROM tete_shoptemplet b left join [tete_templet] on b.[templetID]=[tete_templet].id WHERE b.Isdelete=0 and b.nick = '" + taobaoNick + "' ) AS a WHERE a.rownumber > " + dataCount.ToString() + " ORDER BY id DESC";

 //Response.Write(sqlNew);
        DataTable dtNew = utils.ExecuteDataTable(sqlNew);
        rptItems.DataSource = dtNew;
        rptItems.DataBind();
        //
        //分页数据初始化
        sqlNew = "SELECT COUNT(*) FROM tete_shoptemplet WHERE Isdelete=0 and nick = '" + taobaoNick + "' ";
        int totalCount = int.Parse(utils.ExecuteString(sqlNew));

        lbPage.Text = InitPageStr(totalCount, "activitytempmanage.aspx");
    }

    private string InitPageStr(int total, string url)
    {
        //分页数据初始化
        string str = string.Empty;
        int pageCount = 10;
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