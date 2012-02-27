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
using System.Data.SqlClient;

/// <summary>
/// Summary description for VisitService
/// </summary>
public class VisitService
{
    const string SQL_INSERT = "INSERT TopVisitInfo(VisitID,VisitIP,VisitUrl,VisitTime,VisitUserAgent,VisitBrower,VisitOSLanguage) VALUES(@VisitID,@VisitIP,@VisitUrl,@VisitTime,@VisitUserAgent,@VisitBrower,@VisitOSLanguage)";

    public void InsertVisitInfo(TopVisitInfo info)
    {
        DBHelper.ExecuteNonQuery(SQL_INSERT,CreateParameter(info));
    }

    private static SqlParameter[] CreateParameter(TopVisitInfo info)
    {
        SqlParameter[] param = new[]
        {
            new SqlParameter("@VisitID", info.VisitID),
            new SqlParameter("@VisitIP",info.VisitIP),
            new SqlParameter("@VisitUrl",info.VisitUrl),
            new  SqlParameter("@VisitTime",info.VisitTime),
            new SqlParameter("@VisitUserAgent", info.VisitUserAgent),
            new SqlParameter("@VisitBrower", info.VisitBrower),
            new SqlParameter("@VisitOSLanguage",info.VisitOSLanguage)
        };

        return param;
    }
}
