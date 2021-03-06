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
/// 广告信息
/// </summary>
public class AdsInfo
{
	public AdsInfo()
	{
        AdsId = Guid.NewGuid();
	}

    public AdsInfo(Guid siteid, String adsname, String adssize)
    {
        SiteId = siteid;
        AdsName = adsname;
        AdsSize = adssize;
        AdsId = Guid.NewGuid();
    }

    public SiteInfo MySiteInfo { set; get; }

    public Guid AdsId { set; get; }

    public Guid SiteId { set; get; }

    public String AdsName { set; get; }

    /// <summary>
    /// 广告大小格式为宽*高
    /// </summary>
    public String AdsSize { set; get; }

    /// <summary>
    /// 广告类型（1：不限个数，5：只有一个(VIP版本)
    /// </summary>
    public int AdsType { set; get; }

    /// <summary>
    /// 广告位置的图片
    /// </summary>
    public String AdsPic { set; get; }
}
