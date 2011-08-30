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
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;
using System.Collections.Generic;

public partial class top_market_showCode : System.Web.UI.Page
{
    public string style = string.Empty;
    public string size = string.Empty;
    public string type = string.Empty;
    public string orderby = string.Empty;
    public string query = string.Empty;
    public string shopcat = string.Empty;
    public string name = string.Empty;
    public string items = string.Empty;

    public string nickid = string.Empty;
    public string md5nick = string.Empty;
    public string tabletitle = string.Empty;

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

        //获取NICK的淘宝ID
        string taobaoNickNew = string.Empty;
        string sql = "SELECT * FROM TopIdea WHERE id = " + id + "";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            taobaoNickNew = dt.Rows[0]["nick"].ToString();
            tabletitle = dt.Rows[0]["name"].ToString();
        }
        else
        {
            Response.End();
            return;
        }

        md5nick = MD5(taobaoNickNew);

        string sqlNew = "SELECT sid FROM TopTaobaoShop WHERE nick = '" + taobaoNickNew + "'";
        DataTable dtNew = utils.ExecuteDataTable(sqlNew);
        if (dtNew.Rows.Count != 0)
        {
            nickid = "http://shop" + dtNew.Rows[0]["sid"].ToString() + ".taobao.com/";
        }
        else
        {
            nickid = "http://www.taobao.com/";
        }

        //绑定商品列表
        string sql11 = "SELECT TOP 5 * FROM TopIdeaProduct WHERE ideaid = " + id;
        DataTable newdt = utils.ExecuteDataTable(sql11);
        rptProduct.DataSource = newdt;
        rptProduct.DataBind();
    }

    /// <summary> 
    /// MD5 加密函数 
    /// </summary> 
    /// <param name="str"></param> 
    /// <param name="code"></param> 
    /// <returns></returns> 
    public static string MD5(string str)
    {
        return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").Substring(8, 16);
    }
}
