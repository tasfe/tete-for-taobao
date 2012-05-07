using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text.RegularExpressions;

public partial class top_crm_initcustomphone : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        FileUpload1.SaveAs("tmp.txt");
        string file = Server.MapPath("tmp.txt");
        string str = File.ReadAllText(file);

        string[] arr = Regex.Split(str, "\r\n");

        for (int i = 0; i < arr.Length; i++)
        {
            Response.Write(arr[i] + "<br>");
        }
    }
}