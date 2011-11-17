using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using Common;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using System.Data;

public partial class Admin_TT_Question_Operate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string id = "0";
            if (Request.QueryString["id"] != null)
            {
                id = Request.QueryString["id"].ToString();
                DataTable dt = utils.ExecuteDataTable(" SELECT * FROM TT_Question WHERE Family like '%," + id + ",%' ORDER BY id");
                if (dt != null && dt.Rows.Count > 0)
                {
                    Label1.Text = dt.Rows[0]["date"].ToString();
                    Label2.Text = dt.Rows[dt.Rows.Count - 1]["ReturnDate"].ToString();
                    TextBox1.Text = dt.Rows[dt.Rows.Count - 1]["Answer"].ToString();
                    HiddenField1.Value = dt.Rows[dt.Rows.Count - 1]["ID"].ToString();
                }

                Repeater1.DataSource = dt;
                Repeater1.DataBind();
            }
            
        }
    }
    /// <summary>
    /// 提交
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        string id = "0";
        if (Request.QueryString["id"] != null)
        {
            id = HiddenField1.Value;
        }
        string sql = "Update TT_Question SET state=2,Answer='"+TextBox1.Text+"',ReturnDate='" + DateTime.Now.ToString() + "' WHERE id=" + id;

        utils.ExecuteNonQuery(sql);
        Response.Redirect("TT_Question_Operate.aspx?id="+id);
    }

    public static string decode(string uid)
    {
        Rijndael_ encode = new Rijndael_("tetesoft");
        uid = encode.Decrypt(uid);

        return uid;
    }
}
