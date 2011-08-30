using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Common;

public partial class top_blog_autosend : System.Web.UI.Page
{
    public string isautohtml = string.Empty;
    public string open = string.Empty;
    public string close = string.Empty;
    public string searchkeyhtml = string.Empty;
    public string accounthtml = string.Empty;
    public string linkhtml = string.Empty;
    public string html = string.Empty;
    public string htmlencode = string.Empty;
    public string adsid = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            adsid = "0";

            BindData();
        }
    }

    private void BindData()
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");

        //过期判断
        if (string.IsNullOrEmpty(taobaoNick))
        {
            Response.Write("window.location.href='http://container.open.taobao.com/container?appkey=12159997'");
            Response.End();
            return;
        }

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        //判断状态
        string sql = "SELECT * FROM TopBlogAuto WHERE nick = '" + taobaoNick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            isautohtml = dt.Rows[0]["isauto"].ToString();
            searchkeyhtml = dt.Rows[0]["keywords"].ToString();
            accounthtml = dt.Rows[0]["uids"].ToString();
            linkhtml = dt.Rows[0]["links"].ToString();
            html = dt.Rows[0]["html"].ToString();
            htmlencode = HttpUtility.HtmlEncode(html);
            adsid = dt.Rows[0]["adsid"].ToString();
        }
        else
        {
            isautohtml = "0";
        }

        if (isautohtml == "0" || isautohtml == "2")
        {
            close = "checked";
        }
        else
        {
            open = "checked";
        }

        //错误信息提示
        if (isautohtml == "2")
        {
            errArea.InnerHtml = "您的自动发送已经被取消，取消原因：" + dt.Rows[0]["errcontent"].ToString();
        }

        //搜索关键字绑定
        string sqlNew = "SELECT * FROM TopBlogSearchKey WHERE nick = '" + taobaoNick + "' ORDER BY count DESC";
        DataTable dtNew = utils.ExecuteDataTable(sqlNew);

        rptSearch.DataSource = ChangeTable(dtNew, searchkeyhtml, "searchkey");
        rptSearch.DataBind();

        //发送帐号绑定
        sqlNew = "SELECT * FROM TopBlogAccountNew WHERE nick = '" + taobaoNick + "' ORDER BY id DESC";
        dtNew = utils.ExecuteDataTable(sqlNew);

        rptAccount.DataSource = ChangeTable(dtNew, accounthtml, "uid");
        rptAccount.DataBind();

        //替换关键字
        sqlNew = "SELECT * FROM TopBlogLink WHERE nick = '" + taobaoNick + "' ORDER BY id DESC";
        dtNew = utils.ExecuteDataTable(sqlNew);

        rptLink.DataSource = ChangeTable(dtNew, linkhtml, "keyword");
        rptLink.DataBind();

        //数据绑定
        sqlNew = "SELECT TOP 10 * FROM TopIdea WHERE nick = '" + taobaoNick + "' ORDER BY id DESC";
        dtNew = utils.ExecuteDataTable(sqlNew);

        rptAds.DataSource = dtNew;
        rptAds.DataBind();
    }

    private DataTable ChangeTable(DataTable dtNew, string searchkeyhtml, string field)
    {
        for (int i = 0; i < dtNew.Rows.Count; i++)
        {
            string list = "," + searchkeyhtml + ",";
            if (list.IndexOf("," + dtNew.Rows[i][field].ToString() + ",") != -1)
            {
                dtNew.Rows[i]["nick"] = "checked";
            }
        }

        return dtNew;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string searchkey = utils.NewRequest("searchkey", utils.RequestType.Form);
        string account = utils.NewRequest("account", utils.RequestType.Form);
        string link = utils.NewRequest("link", utils.RequestType.Form);
        string isauto = utils.NewRequest("isauto", utils.RequestType.Form);
        string ads = utils.NewRequest("ads", utils.RequestType.Form);
        string html1 = utils.NewRequest("html", utils.RequestType.Form);

        //判断是否有设置关键字
        if (searchkey == "")
        {
            Response.Write("<script>alert('请设置您自动发送的搜索关键字！');history.go(-1);</script>");
            return;
        }
        if (account == "")
        {
            Response.Write("<script>alert('请设置您自动发送的关联帐号！');history.go(-1);</script>");
            return;
        }

        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");

        //过期判断
        if (string.IsNullOrEmpty(taobaoNick))
        {
            Response.Write("window.location.href='http://container.open.taobao.com/container?appkey=12159997'");
            Response.End();
            return;
        }

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        //先判断数据库是否有记录
        string sql = "SELECT COUNT(*) FROM TopBlogAuto WHERE nick = '" + taobaoNick + "'";
        string count = utils.ExecuteString(sql);
        if (count == "0")
        {
            //如果是开启状态
            if (isauto == "1")
            {
                //插入数据库
                sql = "INSERT INTO TopBlogAuto (" +
                                "keywords, " +
                                "uids, " +
                                "links, " +
                                "adsid, " +
                                "html, " +
                                "nick " +
                            " ) VALUES ( " +
                                " '" + searchkey + "', " +
                                " '" + account + "', " +
                                " '" + link + "', " +
                                " '" + ads + "', " +
                                " '" + html1 + "', " +
                                " '" + taobaoNick + "' " +
                          ") ";

                utils.ExecuteNonQuery(sql);
            }
        }
        else
        {
            sql = "UPDATE TopBlogAuto SET " +
                        "keywords = '" + searchkey + "', " +
                        "uids = '" + account + "', " +
                        "links = '" + link + "', " +
                        "adsid = '" + ads + "', " +
                        "html = '" + html1 + "', " +
                        "isauto = '" + isauto + "' " +
                    " WHERE nick = '" + taobaoNick + "'";

            utils.ExecuteNonQuery(sql);
        }


        Response.Write("<script>alert('保存成功，程序会按照您的设置，从每天的0点开始自动进行博客推广，您可以在左侧菜单的“推广管理”里面查看每天的推广结果！为了防止您的博客被封，每天每个帐号最多自动发送5篇文章，如果您需要多发请增加多个帐号，QQ空间和网易博客暂不支持自动发送，新浪博客由于自动发送会被封号暂停自动发送！');window.location.href='autosend.aspx'</script>");
    }
}