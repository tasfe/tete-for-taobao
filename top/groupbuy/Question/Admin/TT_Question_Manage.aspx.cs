using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using System.Data;

public partial class Admin_TT_Question_Manage : System.Web.UI.Page
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["colose"] != null)
        {
            if (Request.QueryString["colose"].ToString() == "1" && Request.QueryString["id"] != null)
            {
                utils.ExecuteNonQuery("UPDATE TT_Question SET state=1 WHERE ID=" + Request.QueryString["id"]);
            }
        }
        if (!IsPostBack)
        {
            DataTable dt = utils.ExecuteDataTable("SELECT * FROM TT_Question WHERE LV=1 ORDER BY id DESC ");
            dt.Columns.Add("count");
            dt= getCount(dt);

            PagedDataSource objPds = new PagedDataSource();
            //设置分页控件数据
            objPds.DataSource = dt.DefaultView;
            objPds.AllowPaging = true;
            objPds.PageSize = 8;

            if (dt != null && dt.Rows.Count > 0)
            {
                lbcountPage.Text = objPds.PageCount.ToString();
            }
            int CurPage;
            if (Request.QueryString["Page"] != null)
                CurPage = Convert.ToInt32(Request.QueryString["Page"]);
            else
                CurPage = 1;

 
            //设置分页码
            objPds.CurrentPageIndex = CurPage - 1;
            lblCurrentPage.Text = "当前页：" + CurPage.ToString();

            //设置下一页参数
            if (!objPds.IsFirstPage)
                lnkPrev.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + Convert.ToString(CurPage - 1);
            //设置上一页参数
            if (!objPds.IsLastPage)
                lnkNext.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + Convert.ToString(CurPage + 1) ;




            Repeater1.DataSource = objPds;
            Repeater1.DataBind();
        }
    }

    /// <summary>
    /// 返回多少个用户排队中
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public DataTable getCount(DataTable dt)
    {
        string id = string.Empty;
        string sql = string.Empty;
        DataTable dtcount = new DataTable();
        string count = "0";
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (dt.Rows[i]["state"].ToString() != "0")
            {
                continue;
            }
            id = dt.Rows[i]["id"].ToString();
            
            sql = "SELECT COUNT(*) AS count FROM TT_Question WHERE state=0 AND id<"+id;
            dtcount = utils.ExecuteDataTable(sql);
            if (dtcount != null && dtcount.Rows.Count > 0)
            {
                count = dtcount.Rows[0]["count"].ToString();
            }
            dt.Rows[i]["count"] = " (<font size=2 color=red>" + count + "</font><font size=2 >个用户排队中</font>)";
        }
        return dt;
    }

    public static string decode(string uid)
    {
        Rijndael_ encode = new Rijndael_("tetesoft");
        uid = encode.Decrypt(uid);

        return uid;
    }
}