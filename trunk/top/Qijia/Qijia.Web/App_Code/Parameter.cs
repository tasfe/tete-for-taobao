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
/// Summary description for Parameter
/// </summary>
public class Parameter
{
    private string name = null;
    private string value = null;

    public Parameter(string name, string value)
    {
        this.name = name;
        this.value = value;
    }

    public string Name
    {
        get { return name; }
    }

    public string Value
    {
        get { return value; }
    }
}
