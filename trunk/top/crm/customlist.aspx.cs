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

        string sql = "SELECT * FROM TCS_Customer WITH (NOLOCK) WHERE nick = '" + nick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);

        rptArticle.DataSource = dt;
        rptArticle.DataBind();
    }

    public static string getgrade(string grade)
    {
        string str = string.Empty;

        switch (grade)
        {
            case "0":
                str = "<span style='#eeeeee'>【尚未购买】</span>";
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
}