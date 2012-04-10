using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Model;
using System.Collections.Generic;

public partial class CreateKeyWord : BasePage
{
    Search.DAL.KeyWordService kyDal = new Search.DAL.KeyWordService();
    PagedDataSource pds = new PagedDataSource();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);

            Bind(nick);
        }
    }

    private void Bind(string nick)
    {
        int TotalCount = 0;//总记录数
        int TotalPage = 1; //总页数

        int page = 1;
        try
        {
            page = int.Parse(Request.QueryString["Page"]);
            if (ViewState["page"] != null)
            {
                page = int.Parse(ViewState["page"].ToString());
                ViewState["page"] = null;
            }
        }
        catch { }

        IList<KeyWordInfo> kyList = kyDal.GetKeyWords(nick);

        TotalCount = kyList.Count;
        pds.DataSource = kyList;
        pds.AllowPaging = true;
        pds.PageSize = 10;

        if (TotalCount == 0)
            TotalPage = 1;
        else
        {
            if (TotalCount % pds.PageSize == 0)
                TotalPage = TotalCount / pds.PageSize;
            else
                TotalPage = TotalCount / pds.PageSize + 1;
        }

        pds.CurrentPageIndex = page - 1;
        lblCurrentPage.Text = "共" + TotalCount.ToString() + "条记录 当前页：" + page + "/" + TotalPage;

        lnkFrist.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=1";
        if (!pds.IsFirstPage)
            lnkPrev.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + (page - 1);
        if (!pds.IsLastPage)
            lnkNext.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + (page + 1);
        lnkEnd.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + TotalPage;
        Rpt_Keys.DataSource = pds;
        Rpt_Keys.DataBind();

    }

    protected void Btn_Add_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Tb_txt.Text.Trim()))
        {
            return;
        }

        KeyWordInfo info = new KeyWordInfo();
        info.Nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
        info.KeyWord = Tb_txt.Text.Trim();

        if (kyDal.GetKeyWord(info.Nick, info.KeyWord) > 0)
            return;
        info.KeyId = Guid.NewGuid();
        kyDal.Insert(info);

        ViewState["page"] = "1";
        Bind(info.Nick);

    }
}
