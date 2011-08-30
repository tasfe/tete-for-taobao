using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.IO;
using System.Data;

public partial class top_groupbuy_getcode : System.Web.UI.Page
{
    public string html1 = string.Empty;//淘帮派
    public string html2 = string.Empty;//站外HTML代码
    public string html3 = string.Empty;
    public string html4 = string.Empty;
    public string html5 = string.Empty;
    public string html6 = string.Empty;
    public string id = string.Empty;
    //public string groupbyin = string.Empty;
    //public string groupbying = string.Empty;
    //public string groupbyend = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            id = utils.NewRequest("id", Common.utils.RequestType.QueryString);

            if (id != "" && !utils.IsInt32(id))
            {
                Response.Write("非法参数1");
                Response.End();
                return;
            }

            html1 = CreateGroupbuyHtml(id, "");
            html2 = CreateGroupbuyHtml(id, "webSite");
        }
    }

    /// <summary>
    /// 生成团购HTML
    /// </summary>
    /// <param name="id"></param>
    /// <param name="type"> 类型  webSite：站外HTML 可传空 默认为：淘帮派 </param>
    /// <returns></returns>
    private string CreateGroupbuyHtml(string id,string type)
    {
        string str = string.Empty;
        string html = File.ReadAllText(Server.MapPath("tpl/style2.html"));
        if (type == "webSite")
        {
            html = File.ReadAllText(Server.MapPath("tpl/outWebsite/style/style2.html"));
        }
        string sql = "SELECT * FROM TopGroupBuy WHERE id = '" + id + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        { 
            //groupbyin = dt.Rows[0]["groupbuyImg"].ToString();
            //groupbying = dt.Rows[0]["groupbuyingimg"].ToString();
            //groupbyend = dt.Rows[0]["groupbuyendimg"].ToString();
            str = html;
            str = str.Replace("{name}", dt.Rows[0]["name"].ToString());
            str = str.Replace("{oldprice}", dt.Rows[0]["productprice"].ToString());
            str = str.Replace("{zhekou}", Math.Round((decimal.Parse(dt.Rows[0]["productprice"].ToString()) - decimal.Parse(dt.Rows[0]["zhekou"].ToString())) / decimal.Parse(dt.Rows[0]["productprice"].ToString()) * 10, 1).ToString());
            str = str.Replace("{leftprice}", (decimal.Parse(dt.Rows[0]["productprice"].ToString()) - decimal.Parse(dt.Rows[0]["zhekou"].ToString())).ToString().Split('.')[0]);
            str = str.Replace("{rightprice}", (decimal.Parse(dt.Rows[0]["productprice"].ToString()) - decimal.Parse(dt.Rows[0]["zhekou"].ToString())).ToString().Split('.')[1]);
            str = str.Replace("{newprice}", dt.Rows[0]["zhekou"].ToString());
            str = str.Replace("{buycount}", dt.Rows[0]["buycount"].ToString());
            str = str.Replace("{producturl}", dt.Rows[0]["producturl"].ToString());
            str = str.Replace("{productimg}", dt.Rows[0]["productimg"].ToString());
            str = str.Replace("{id}", id);

            DateTime date = DateTime.Now;
            DateTime endDate = DateTime.Parse(dt.Rows[0]["endtime"].ToString());
            DateTime startDate = date;
            TimeSpan tdays = endDate - startDate;  //得到时间差 当前时间与团购结束时间
            string d = tdays.Days.ToString();//剩余时间:小时
            string h = tdays.Hours.ToString();//剩余时间:分钟
            string m = tdays.Minutes.ToString();//剩余时间:秒

            str = str.Replace("{d}", OutPutImage(d));
            str = str.Replace("{h}", OutPutImage(h));
            str = str.Replace("{m}", OutPutImage(m));
            
            str = str.Replace("'", "''");
        }
        return str;
    }

    private string OutPutImage(string imageName)
    {
        //淘宝图片地址
        string url = string.Empty;
        string sql = "SELECT imageName,taobaoImageUrl,Type FROM taobaoImageUrl WHERE Type = 1 AND imageName='" + imageName + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt != null && dt.Rows.Count > 0)
        {
            //输出淘宝图片地址
            url = dt.Rows[0]["taobaoImageUrl"].ToString();
        }

        return url;
    }

    
}