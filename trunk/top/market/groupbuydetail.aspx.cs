using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Common;

public partial class top_groupbuy_groupbuydetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //根据团购ID获取该活动的购买详情
        BindData();
    }

    private void BindData()
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);


        string id = utils.NewRequest("id", utils.RequestType.QueryString);

        string sql = "SELECT * FROM TopGroupBuyDetail WHERE groupbuyid =" + id;
        DataTable dtNew = utils.ExecuteDataTable(sql);
        rptArticle.DataSource = dtNew;
        rptArticle.DataBind();


        sql = "SELECT * FROM TopGroupBuy WHERE nick = '" + taobaoNick + "' AND id = " + id;
        dtNew = utils.ExecuteDataTable(sql);
        Repeater1.DataSource = dtNew;
        Repeater1.DataBind();
    }


    public static string splitstr(string str)
    {
        if (str.IndexOf("|") == -1)
        {
            return str;
        }

        string[] arr = str.Split('|');
        string newstr = string.Empty;

        for (int i = 1; i < arr.Length; i++)
        {
            if (i != 1)
                newstr += "<br>";
            newstr += "<a href='#' target='_blank'>" + arr[i] + "</a>";
        }
        return newstr;
    }


    public static string splitstrnew(string str)
    {
        if (str.IndexOf("|") == -1)
        {
            return str;
        }

        string[] arr = str.Split('|');
        string newstr = string.Empty;

        for (int i = 1; i < arr.Length; i++)
        {
            if (i != 1)
                newstr += "<br>";
            newstr += arr[i];
        }
        return newstr;
    }
}