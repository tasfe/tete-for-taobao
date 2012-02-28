using System;
using System.Collections;
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
using System.Collections.Generic;

public partial class HourPVTotal : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            VisitService visitDal = new VisitService();
            IList<HourPVInfo> list = visitDal.GetHourPVTotal();
            
            SeriseText ="[{name:'PV量', data:[";          

            DateText = "[";
            for (int h = 0; h <= DateTime.Now.Hour; h++)
            {
                DateText += "'" + h + "',";
                IList<HourPVInfo> thisinfo = list.Where(o => o.Hour == h).ToList();

                if (thisinfo.Count == 0)
                    SeriseText += "0,";
                else
                    SeriseText += thisinfo[0].PVCount + ",";

            }
            SeriseText = SeriseText.Substring(0, SeriseText.Length - 1);
            SeriseText += "]}]";
            DateText = DateText.Substring(0, DateText.Length - 1);
            DateText += "]";
        }
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
