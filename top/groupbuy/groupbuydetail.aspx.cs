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

        dtNew = ConvertTable(dtNew);

        rptArticle.DataSource = dtNew;
        rptArticle.DataBind();


        sql = "SELECT * FROM TopGroupBuy WHERE nick = '" + taobaoNick + "' AND id = " + id;
        dtNew = utils.ExecuteDataTable(sql);
        Repeater1.DataSource = dtNew;
        Repeater1.DataBind();
    }

    /// <summary>
    /// 替换客户的下单情况
    /// </summary>
    /// <param name="dtNew"></param>
    /// <returns></returns>
    private DataTable ConvertTable(DataTable dtNew)
    {
        string sql = string.Empty;
        DataTable dt;

        for (int i = 0; i < dtNew.Rows.Count; i++)
        {
            sql = "SELECT * FROM TopGroupBuyDetailOrder WHERE detailid = " + dtNew.Rows[i]["id"].ToString();
            dt = utils.ExecuteDataTable(sql);

            //重新赋值
            dtNew.Rows[i]["ordernumber"] = "";
            dtNew.Rows[i]["payStatus"] = "";

            for (int j = 0; j < dt.Rows.Count; j++) 
            {
                if (j == 0)
                {
                    dtNew.Rows[i]["ordernumber"] = dt.Rows[j]["orderid"].ToString();
                    dtNew.Rows[i]["payStatus"] = dt.Rows[j]["status"].ToString();
                }
                else
                {
                    dtNew.Rows[i]["ordernumber"] += "|" + dt.Rows[j]["orderid"].ToString();
                    dtNew.Rows[i]["payStatus"] += "|" + dt.Rows[j]["status"].ToString();
                }
            }

            //如果为空则判断
            if (dtNew.Rows[i]["ordernumber"].ToString() == "")
            {
                dtNew.Rows[i]["ordernumber"] = "--";
            }
            if (dtNew.Rows[i]["payStatus"].ToString() == "")
            {
                dtNew.Rows[i]["payStatus"] = "--";
            }
        }

        return dtNew;
    }


    public static string splitstr(string str)
    {
        if (str.IndexOf("|") == -1)
        {
            return str;
        }

        string[] arr = str.Split('|');
        string newstr = string.Empty;

        for (int i = 0; i < arr.Length; i++)
        {
            if (i != 0)
                newstr += "<br>";
            newstr += arr[i];
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

        for (int i = 0; i < arr.Length; i++)
        {
            if (i != 0)
            {
                newstr += "<br>";
            }
            
            if (arr[i] == "WAIT_BUYER_PAY")
            {
                newstr += "等待买家付款";
            }
            else
            {
                newstr += "买家已付款";
            }
        }
        return newstr;
    }
}