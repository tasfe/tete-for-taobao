using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {

       // DateText = "'11', '12', '3', '4', '5', '6','7', '8', '9', '10', '11', '12'";
        Response.Redirect("~/index.htm");

      // SeriseText  =   "[{name:'XX比率1', data:[{y:74.33,value:194},{y:76.25,value:5045},{y:67.12,value:298}]}, {name:'XX比率2',   data:[{y:66.28,value:173},{y:78.84,value:5216},{y:67.57,value:300}]},  {name:'XX比率3',  data:[{y:88.12,value:230},{y:88.36,value:5846},{y:87.39,value:388}]},  {name:'XX比率4',  data:[{y:38.70,value:101},{y:34.08,value:2255},{y:28.38,value:126}]},  {name:'XX比率5',   data:[{y:16.48,value:43},{y:26.92,value:1781},{y:23.20,value:103}]},  {name:'XX比率6'   ,data:[{y:37.93,value:99},{y:47.73,value:3158},{y:39.64,value:176}]}]";     

    }

    protected string DateText
    {
        get { return ViewState["dateText"].ToString(); }
        set { ViewState["dateText"] = value; }
    }

    protected string SeriseText
    {
        get { return ViewState["seriseText"].ToString(); }
        set { ViewState["seriseText"] = value; }
    }
}
