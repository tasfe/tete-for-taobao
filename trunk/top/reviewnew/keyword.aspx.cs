using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;
using System.Net;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Data;

public partial class top_review_keyword : System.Web.UI.Page
{
    public string session = string.Empty;
    public string nick = string.Empty;
    public string wordcount = string.Empty;
    public string keyword = string.Empty;
    public string badkeyword = string.Empty;
    public string keywordisbad = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        session = cookie.getCookie("top_sessiongroupbuy");
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Decrypt(taobaoNick);

        if (!IsPostBack)
        {
            string sql = "SELECT * FROM TCS_ShopConfig WHERE nick = '" + nick + "'";
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                wordcount = dt.Rows[0]["wordcount"].ToString();
                keywordisbad = dt.Rows[0]["keywordisbad"].ToString();
                keyword = dt.Rows[0]["keyword"].ToString().Replace("|", "\r\n");
                badkeyword = dt.Rows[0]["badkeyword"].ToString().Replace("|", "\r\n");
            }
            else
            {
                Response.Write("请先到基本设置里面进行信息设置，<a href='setting.aspx'>点此进入</a>");
                Response.End();
            }
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string sql = "UPDATE TCS_ShopConfig SET " +
                        "wordcount = '" + utils.NewRequest("wordcount", utils.RequestType.Form) + "', " +
                        "keywordisbad = '" + utils.NewRequest("keywordisbad", utils.RequestType.Form) + "', " +
                        "keyword = '" + utils.NewRequest("keyword", utils.RequestType.Form).Replace("\r\n", "|") + "' " +
                        "badkeyword = '" + utils.NewRequest("badkeyword", utils.RequestType.Form).Replace("\r\n", "|") + "' " +
                    "WHERE nick = '" + nick + "'";

        utils.ExecuteNonQuery(sql);

        Response.Write("<script>alert('保存成功！');window.location.href='keyword.aspx';</script>");
        Response.End();
        return;
    }
}