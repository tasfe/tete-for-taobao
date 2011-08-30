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

public partial class top_market_addidea_2 : System.Web.UI.Page
{
    public string style = string.Empty;
    public string size = string.Empty;
    public string type = string.Empty;
    public string orderby = string.Empty;
    public string query = string.Empty;
    public string shopcat = string.Empty;
    public string name = string.Empty;
    public string items = string.Empty;
    public string id = string.Empty;
    public string url = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        url = "addidea-3.aspx";
        id = utils.NewRequest("id", Common.utils.RequestType.QueryString);

        style = utils.NewRequest("style", Common.utils.RequestType.Form);
        size = utils.NewRequest("size", Common.utils.RequestType.Form);
        type = utils.NewRequest("type", Common.utils.RequestType.Form);
        orderby = utils.NewRequest("orderby", Common.utils.RequestType.Form);
        query = utils.NewRequest("query", Common.utils.RequestType.Form);
        shopcat = utils.NewRequest("shopcat", Common.utils.RequestType.Form);
        name = utils.NewRequest("name", Common.utils.RequestType.Form);
        items = utils.NewRequest("itemsStr", Common.utils.RequestType.Form);

        //过滤items中的0
        if (items.Length > 2) 
        {
            items = items.Substring(1, items.Length - 1);
        }

        BindData();

        //判断是否为编辑状态
        id = utils.NewRequest("id", Common.utils.RequestType.QueryString);

        if (id != "" && !utils.IsInt32(id))
        {
            Response.Write("非法参数1");
            Response.End();
            return;
        }

        if (id != "" && id != "0")
        {
            url = "addidea-3.aspx?id=" + id;
        }
    }

    private void BindData()
    {
        
    }
}
