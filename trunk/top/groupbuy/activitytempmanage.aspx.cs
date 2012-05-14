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
        if (Request.QueryString["act"] != null && Request.QueryString["act"].ToString() == "delitem")
        {
            if (Request.QueryString["id"] != null)
            {
                string id = Request.QueryString["id"].ToString();
                string aid = Request.QueryString["aid"].ToString();
                //清除宝贝描述
                //utils.ExecuteNonQuery("delete from tete_shoptempletlist where shoptempletID=" + id);
                //清除宝贝描述
                ClearitemsDesc(id, aid);
                
            }
        }
        if (!IsPostBack)
        {
            BindData();
        }
    }

    /// <summary>
    /// 清除宝贝描述
    /// </summary>
    /// <param name="id">模板ID</param>
    /// <param name="aid">活动ID</param>
    public void ClearitemsDesc(string id,string aid)
    {
        if (id != "" && !utils.IsInt32(id))
        {
            Response.Write("非法参数1");
            Response.End();
            return;
        }

        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        string session = cookie.getCookie("top_sessiongroupbuy");

        //COOKIE过期判断
        if (taobaoNick == "")
        {
            //SESSION超期 跳转到登录页
            Response.Write("<script>parent.location.href='http://container.open.taobao.com/container?appkey=12287381'</script>");
            Response.End();
        }

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        string sql = "SELECT COUNT(*) FROM Tete_ActivityMission WHERE shoptempletID = " + id + " AND typ='delete' AND isok = 0 AND nick<>''";
        string count = utils.ExecuteString(sql);

        if (count != "0")
        {
            Response.Write("<script>alert('创建任务失败，有同类型的任务正在执行中，请等待其完成后再创建新的任务！');window.location.href='activitymissionlist.aspx';</script>");
            Response.End();
            return;
        }


        if (taobaoNick.Trim() == "")
        {
            //SESSION超期 跳转到登录页
            Response.Write("<script>parent.location.href='http://container.open.taobao.com/container?appkey=12287381'</script>");
            Response.End();
        } 
        sql = "INSERT INTO Tete_ActivityMission (typ, nick, ActivityID,IsOK,shoptempletID,Success,Fail,Startdate) VALUES ('delete','" + taobaoNick + "','" + aid + "',0,'" + id + "',0,0,'" + DateTime.Now.ToString() + "')";
        utils.ExecuteNonQuery(sql);


        sql = "SELECT TOP 1 ID FROM Tete_ActivityMission ORDER BY ID DESC";
        string missionid = utils.ExecuteString(sql);

        

        //更新任务总数
        sql = "SELECT COUNT(*) FROM Tete_ActivityWriteContent WHERE shoptempletID = '" + id + "' and ActivityID=" + aid + " AND isok = 1";
        count = utils.ExecuteString(sql);
        sql = "UPDATE tete_ActivityMission SET total = '" + count + "' WHERE id = " + missionid;
        utils.ExecuteNonQuery(sql);

        Response.Redirect("activitymissionlist.aspx");
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="ID"></param>
    /// <param name="actionID"></param>
    /// <param name="pid"></param>
    /// <returns></returns>
    public string outShowHtml(string ID,string aid)
    {
        string html = "<div>  <a  target=_blank href=\"activitytempView.aspx?tid=" + ID + "\")\">获取代码</a>   <a  target=_blank  href=\"activitytempView.aspx?ytid=" + ID + "\")\">预览</a>      <a href=\"activitytempmanage.aspx?act=del&id=" + ID + "\")\">删除</a>   <a href=\"activitytempmanage.aspx?act=delitem&aid="+aid+"&id=" + ID + "\")\" ' onclick=\"return confirm('您确认要清除关联描述，该操作不可恢复？')\">清除宝贝描述</a></div>";

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