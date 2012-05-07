using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text.RegularExpressions;
using Common;

public partial class top_crm_initcustomphone : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string file = Server.MapPath("tmp.txt");
        FileUpload1.SaveAs(file);
        string str = File.ReadAllText(file);

        string[] arr = Regex.Split(str, "\r\n");

        for (int i = 0; i < arr.Length; i++)
        {
            string phone = arr[i];
            if (phone.Length != 0)
            {
                string sql = "INSERT INTO TCS_Customer (nick, buynick, mobile) VALUES ('宝美户外专营店', '" + phone + "', '" + phone + "')";
                utils.ExecuteNonQuery(sql);
            }
        }
    }
}