using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;

public partial class top_groupbuy_deletetaobao : System.Web.UI.Page
{
    public string id = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        id = utils.NewRequest("id", Common.utils.RequestType.QueryString);

        if (id != "" && !utils.IsInt32(id))
        {
            Response.Write("非法参数1");
            Response.End();
            return;
        }

        Cookie cookie = new Cookie();
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

        string sql = "SELECT COUNT(*) FROM TopMission WHERE groupbuyid = " + id + " AND typ='delete' AND isok = 0";
        string count = utils.ExecuteString(sql);

        if (count != "0")
        {
            Response.Write("<script>alert('创建任务失败，有同类型的任务正在执行中，请等待其完成后再创建新的任务！');window.location.href='missionlist.aspx';</script>");
            Response.End();
            return;
        }

        sql = "INSERT INTO TopMission (typ, nick, groupbuyid) VALUES ('delete', '" + taobaoNick + "', '" + id + "')";
        utils.ExecuteNonQuery(sql);

        sql = "SELECT TOP 1 ID FROM TopMission ORDER BY ID DESC";
        string missionid = utils.ExecuteString(sql);

        //获取团购信息并更新
        sql = "SELECT name,productimg,productid FROM TopGroupBuy WHERE id = '" + id + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            sql = "UPDATE TopMission SET groupbuyname = '" + dt.Rows[0]["name"].ToString() + "',groupbuypic = '" + dt.Rows[0]["productimg"].ToString() + "',itemid = '" + dt.Rows[0]["productid"].ToString() + "' WHERE id = " + missionid;
            utils.ExecuteNonQuery(sql);
        }

        //更新任务总数
        sql = "SELECT COUNT(*) FROM TopWriteContent WHERE groupbuyid = '" + id + "' AND isok = 1";
        count = utils.ExecuteString(sql);
        sql = "UPDATE TopMission SET total = '" + count + "' WHERE id = " + missionid;
        utils.ExecuteNonQuery(sql);

        Response.Redirect("missionlist.aspx");
    }
}