﻿using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// 多用途类
/// </summary>
public class HourTotalInfo
{
	public HourTotalInfo()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public int Hour { set; get; }

    public int PVCount { set; get; }
}
