using System;
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
using System.Reflection;

/// <summary>
/// Summary description for PageVisitInfoTotal
/// </summary>
public class PageVisitInfoTotal
{
	public PageVisitInfoTotal()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public string VisitURL { set; get; }

    public int VisitCount { set; get; }

    public int IPCount { set; get; }

    public string VisitAvg { get { return ((double)VisitCount / IPCount).ToString(".00"); } }

    public override bool Equals(object obj)
    {
        if (obj is PageVisitInfoTotal)
        {
            if ((obj as PageVisitInfoTotal).VisitURL == VisitURL)
                return true;
        }

        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        if (VisitURL == string.Empty) return base.GetHashCode();
        string stringRepresentation = MethodBase.GetCurrentMethod().DeclaringType.FullName + "#" + VisitURL;
        return stringRepresentation.GetHashCode();
    }
}
