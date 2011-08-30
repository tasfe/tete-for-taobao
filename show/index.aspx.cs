using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Common;

public partial class show_index : System.Web.UI.Page
{
    public string id = string.Empty;
    public string style = string.Empty;
    public string size = string.Empty;
    public string type = string.Empty;
    public string orderby = string.Empty;
    public string query = string.Empty;
    public string shopcat = string.Empty;
    public string items = string.Empty;
    public string title = string.Empty;

    public string width = string.Empty;
    public string height = string.Empty;
    public string num = string.Empty;
    public string url = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        id = utils.NewRequest("id", utils.RequestType.QueryString);
        style = utils.NewRequest("style", utils.RequestType.QueryString);
        size = utils.NewRequest("size", utils.RequestType.QueryString);
        type = utils.NewRequest("type", utils.RequestType.QueryString);
        orderby = utils.NewRequest("orderby", utils.RequestType.QueryString);
        query = utils.NewRequest("query", utils.RequestType.QueryString);
        shopcat = utils.NewRequest("shopcat", utils.RequestType.QueryString);
        items = utils.NewRequest("items", utils.RequestType.QueryString);
        title = utils.NewRequest("title", utils.RequestType.QueryString);
        url = Request.UrlReferrer == null ? "" : Request.UrlReferrer.ToString();

        title = HttpUtility.UrlEncode(title);
        query = HttpUtility.UrlEncode(query);

        if (id != "" && !utils.IsInt32(id))
        {
            Response.Write("非法参数");
            Response.End();
            return;
        }

        if (id != "0")
        {
            string sql = "SELECT name,size FROM TopIdea WHERE id = " + id;
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                size = dt.Rows[0]["size"].ToString();
            }
        }


        if (size.IndexOf('*') != -1)
        {
            string[] arr = size.Split('*');

            width = arr[0];
            height = arr[1];

            if (width == "714")
            {
                width = "726";
            }

            if (width == "114")
            {
                width = "126";
            }

            if (height == "308")
            {
                height = "328";
            }

            switch (size)
            { 
                case "514*160":
                    num = "5";
                    break;
                case "514*288":
                    num = "10";
                    break;
                case "664*160":
                    num = "6";
                    break;
                case "218*286":
                    num = "4";
                    break;
                case "312*288":
                    num = "6";
                    break;
                case "336*280":
                    num = "4";
                    break;
                case "714*160":
                    num = "7";
                    break;
                case "114*418":
                    num = "3";
                    break;
                case "743*308":
                    num = "4";
                    break;
                default:
                    num = "4";
                    break;
            }
        }
        else
        {
            width = "560";
            height = "300";
            num = "10";
        }
    }
}
