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

/// <summary>
/// Summary description for ParameterComparer
/// </summary>
public class ParameterComparer
{
    public int Compare(Parameter x, Parameter y)
    {
        if (x.Name == y.Name)
        {
            return string.Compare(x.Value, y.Value);
        }
        else
        {
            return string.Compare(x.Name, y.Name);
        }
    }
}
