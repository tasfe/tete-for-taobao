using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Common;

public partial class top_market_addidea : System.Web.UI.Page
{
    public string url = string.Empty;
    public string id = string.Empty;

    public string style = string.Empty;
    public string size = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
	Response.AddHeader("P3P", "CP=CURa ADMa DEVa PSAo PSDo OUR BUS UNI PUR INT DEM STA PRE COM NAV OTC NOI DSP COR");
        Cookie cookie = new Cookie();
        string taobaoNick = cookie.getCookie("nick");

        //过期判断
        if (string.IsNullOrEmpty(taobaoNick))
        {
            string msg = "尊敬的淘宝卖家，非常抱歉的告诉您，您还没有购买此服务或者登录信息已失效，如需继续使用请<a href='http://fuwu.taobao.com/serv/detail.htm?service_id=764' target='_blank'>进入该服务</a>，谢谢！";
            Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
            Response.End();
            return;
        }

        //过期判断
        if (string.IsNullOrEmpty(taobaoNick))
        {
            Response.Write("<script>window.location.href='http://container.open.taobao.com/container?appkey=12132145'</script>");
            Response.End();
            return;
        }

        url = "addidea-1.aspx";
        id = utils.NewRequest("id", Common.utils.RequestType.QueryString);

        if (id != "" && !utils.IsInt32(id))
        {
            Response.Write("非法参数1");
            Response.End();
            return;
        }

        if (id != "" && id != "0")
        { 
            //数据绑定
            string sql = "SELECT * FROM TopIdea WHERE id = " + id;
            DataTable dt = utils.ExecuteDataTable(sql);
            if(dt.Rows.Count != 0)
            {
                ideaName.Value = dt.Rows[0]["name"].ToString();
            }

            style = dt.Rows[0]["style"].ToString();
            size = dt.Rows[0]["size"].ToString();

            url = "addidea-1.aspx?id=" + id;
        }
    }
}
