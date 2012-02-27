using System;

/// <summary>
///来访者信息
/// </summary>
public class TopVisitInfo
{
	public TopVisitInfo()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    /// <summary>
    /// 唯一ID
    /// </summary>
    public Guid VisitID { set; get; }

    /// <summary>
    /// 来访者IP
    /// </summary>
    public string VisitIP { set; get; }

    /// <summary>
    /// 来访者地址
    /// </summary>
    public string VisitUrl { set; get; }

    /// <summary>
    /// 访问时间
    /// </summary>
    public DateTime VisitTime { set; get; }

    /// <summary>
    /// 来访者操作系统信息
    /// </summary>
    public string VisitUserAgent { set; get; }

    /// <summary>
    /// 来访者浏览器信息
    /// </summary>
    public string VisitBrower { set; get; }

    /// <summary>
    /// 来访者操作系统语言
    /// </summary>
    public string VisitOSLanguage { set; get; }
}
