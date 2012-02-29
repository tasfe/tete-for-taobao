﻿using System;
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
            IList<HourTotalInfo> list = visitDal.GetHourPVTotal("246bcca56c050c665b67708d33127e46");
            IList<HourTotalInfo> ipList = visitDal.GetHourIPTotal("246bcca56c050c665b67708d33127e46");
             
            SeriseText ="[{name:'PV量', data:[";
            string iptotal = ",{name:'IP量',data:[";
            string avg = ",{name:'人均浏览次数',data:[";
            DateText = "[";
            for (int h = 0; h <= DateTime.Now.Hour; h++)
            {
                DateText += "'" + h + "',";
                IList<HourTotalInfo> thisInfo = list.Where(o => o.Hour == h).ToList();
                IList<HourTotalInfo> thisIpInfo = ipList.Where(o => o.Hour == h).ToList();

                if (thisIpInfo.Count == 0)
                    iptotal += "0,";
                else
                    iptotal += thisIpInfo[0].PVCount + ",";

                if (thisInfo.Count == 0)
                    SeriseText += "0,";
                else
                    SeriseText += thisInfo[0].PVCount + ",";

                if (thisIpInfo.Count == 0)
                    avg += "0,";
                else
                    avg +=((double)thisInfo[0].PVCount / thisIpInfo[0].PVCount).ToString(".00") + ",";

            }
            SeriseText = SeriseText.Substring(0, SeriseText.Length - 1);
            SeriseText += "]}";
            iptotal = iptotal.Substring(0, iptotal.Length - 1);
            iptotal += "]}";
            avg = iptotal + avg.Substring(0, avg.Length - 1);
            SeriseText += avg + "]}]";

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
