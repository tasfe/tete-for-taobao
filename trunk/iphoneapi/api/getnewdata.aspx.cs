using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Common;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;
using Taobao.Top.Api;

public partial class api_getnewdata : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string uid = this.TextBox1.Text;
        string taobaonick = this.TextBox2.Text;
        string sql = "SELECT * FROM TeteShop WHERE nick = '" + uid + "'";

        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        { 
            //同步分类数据
            TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", dt.Rows[0]["appkey"].ToString(), dt.Rows[0]["appsecret"].ToString());
            SellercatsListGetRequest request1 = new SellercatsListGetRequest();
            request1.Fields = "cid,parent_cid,name,is_parent,";
            request1.Nick = taobaonick;
            PageList<SellerCat> cat = client.SellercatsListGet(request1);

            for (int i = 0; i < cat.Content.Count; i++)
            {
                sql = "INSERT INTO TeteShopCategory (" +
                                "cateid, " +
                                "catename, " +
                                "parentid, " +
                                "nick " +
                            " ) VALUES ( " +
                                " '" + cat.Content[i].Cid + "', " +
                                " '" + cat.Content[i].Name + "', " +
                                " '" + cat.Content[i].ParentCid + "', " +
                                " '" + uid + "' " +
                          ") ";
                Response.Write(sql + "<br>");
                utils.ExecuteNonQuery(sql);
            }
        }
    }
}