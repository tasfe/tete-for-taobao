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
/// Summary description for GoodsInfo
/// </summary>
public class GoodsInfo
{
    public GoodsInfo()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public string num_iid { set; get; }

    public string title { set; get; }

    public string nick { set; get; }

    public decimal price { set; get; }

    public int Count { set; get; }

    public override bool Equals(object obj)
    {
        if (obj is GoodsInfo)
        {
            if ((obj as GoodsInfo).num_iid == num_iid)
                return true;
        }

        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        if (num_iid == string.Empty) return base.GetHashCode();
        string stringRepresentation = MethodBase.GetCurrentMethod().DeclaringType.FullName + "#" + num_iid;
        return stringRepresentation.GetHashCode();
    }
}

public class GoodsClassInfo
{
    public string cid { set; get; }

    public string name { set; get; }

    public string pic_url { set; get; }

    public string sort_order { set; get; }

    public string type { set; get; }
}