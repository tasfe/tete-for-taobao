using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;

public partial class top_blog_success : System.Web.UI.Page
{
    public string blogUrl = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        blogUrl = utils.NewRequest("adr", utils.RequestType.QueryString).Replace("''", "'");

        BindData();
    }

    /// <summary>
    /// 数据绑定
    /// </summary>
    private void BindData()
    {
        string sqlNew = "SELECT TOP 10 * FROM TopBlog WHERE CHARINDEX(blogurl, '" + blogUrl.Replace("'", "''") + "') = 0 AND blogtitle <> '' AND sendStatus = 1 ORDER BY id DESC";
        DataTable dtNew = utils.ExecuteDataTable(sqlNew);

        rptArticle.DataSource = dtNew;
        rptArticle.DataBind();
    }
}