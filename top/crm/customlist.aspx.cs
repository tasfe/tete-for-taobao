using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;

public partial class top_crm_customlist : System.Web.UI.Page
{
    public string session = string.Empty;
    public string nick = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string id = utils.NewRequest("id", utils.RequestType.QueryString);
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        session = cookie.getCookie("top_sessiongroupbuy");
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Decrypt(taobaoNick);

        BindData();
    }

    private void BindData()
    {
        string count = utils.NewRequest("count", utils.RequestType.QueryString);
        string condition = string.Empty;

        switch (count)
        {
            case "0":
                condition = " AND b.tradecount = 0";
                break;
            case "1":
                condition = " AND b.tradecount = 1";
                break;
            case "2":
                condition = " AND b.tradecount > 1";
                break;
            case "a":
                condition = " AND b.grade = 0";
                break;
            case "b":
                condition = " AND b.grade = 1";
                break;
            case "c":
                condition = " AND b.grade = 2";
                break;
            case "d":
                condition = " AND b.grade = 3";
                break;
            case "e":
                condition = " AND b.grade = 4";
                break;
        }

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
        int pageCount = 20;
        int dataCount = (pageNow - 1) * pageCount;

        string sql = "SELECT TOP " + pageCount.ToString() + " * FROM (SELECT *,ROW_NUMBER() OVER (ORDER BY b.lastorderdate DESC) AS rownumber FROM TCS_Customer b WITH (NOLOCK) WHERE b.nick = '" + nick + "' " + condition + ") AS a WHERE a.rownumber > " + dataCount.ToString() + " ORDER BY lastorderdate DESC";
        DataTable dt = utils.ExecuteDataTable(sql);

        rptArticle.DataSource = dt;
        rptArticle.DataBind();

        //分页数据初始化
        sql = "SELECT COUNT(*) FROM TCS_Customer b WHERE b.nick = '" + nick + "' " + condition + "";
        int totalCount = int.Parse(utils.ExecuteString(sql));

        lbPage.Text = InitPageStr(totalCount, "customlist.aspx");
    }

    public static string getgrade(string grade)
    {
        string str = string.Empty;

        switch (grade)
        {
            case "0":
                str = "<span style='#eeeeee'>【未购买】</span>";
                break;
            case "1":
                str = "普通会员";
                break;
            case "2":
                str = "<span style='color:blue'>高级会员</span>";
                break;
            case "3":
                str = "<span style='color:green'>VIP会员</span>";
                break;
            case "4":
                str = "<span style='color:red'>至尊VIP会员</span>";
                break;
        }

        return str;
    }

    public static string getsex(string sex)
    {
        string str = string.Empty;

        switch (sex)
        {
            case "m":
                str = "男";
                break;
            case "f":
                str = "女";
                break;
            case "":
                str = "--";
                break;
        }

        return str;
    }



    private string InitPageStr(int total, string url)
    {
        //分页数据初始化
        string str = string.Empty;
        int pageCount = 20;
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

        //如果总页面大于20，则最大页面差不超过20
        int start = 1;
        int end = 20;

        if (pageSize < end)
        {
            end = pageSize;
        }
        else
        {
            if (pageNow > 15)
            {
                start = pageNow - 10;

                if (pageNow < (total - 10))
                {
                    end = pageNow + 10;
                }
                else
                {
                    end = total;
                }
            }
        }

        for (int i = start; i <= end; i++)
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