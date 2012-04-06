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
using Model;
using System.Data.SqlClient;
using System.Collections.Generic;

/// <summary>
/// Summary description for TijianParamValueService
/// </summary>
public class TijianParamValueService
{

    const string SQL_SELECT_INFO = "SELECT ParamName,ParamValue FROM TopTijianParamValue WHERE Nick=@nick";

    const string SQL_INSERT = "INSERT TopTijianParamValue(Nick,ParamName,ParamValue) VALUES(@Nick,@ParamName,@ParamValue)";

    const string SQL_UPDATE = "UPDATE TopTijianParamValue SET ParamValue=@ParamValue WHERE Nick=@Nick AND ParamName=@ParamName";

    public IList<TijianParamInfo> GetParamInfo(string nick)
    {
        IList<TijianParamInfo> list = new List<TijianParamInfo>();

        SqlParameter param = new SqlParameter("@nick", nick);
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_INFO, param);
        foreach (DataRow dr in dt.Rows)
        {
            TijianParamInfo info = new TijianParamInfo();
            info = new TijianParamInfo();
            info.Nick = nick;
            info.ParamName = dr["ParamName"].ToString();
            info.ParamValue = decimal.Parse(dr["ParamValue"].ToString());

            list.Add(info);
        }

        return list;
    }

    public int InsertTijianInfo(IList<TijianParamInfo> list)
    {
        if (list.Count == 0) return 0;

        int count = 0;
        if (GetParamInfo(list[0].Nick).Count > 0)
        {
            return UpdateTijianInfo(list);
        }
        else
        {
            foreach (TijianParamInfo info in list)
            {
                SqlParameter[] param = CreateParam(info);
                count += DBHelper.ExecuteNonQuery(SQL_INSERT, param);
            }
        }
        return count;
    }

    private static int UpdateTijianInfo(IList<TijianParamInfo> list)
    {
        int count = 0;
        foreach (TijianParamInfo info in list)
        {
            SqlParameter[] param = CreateParam(info);
            count += DBHelper.ExecuteNonQuery(SQL_UPDATE, param);
        }

        return count;
    }

    private static SqlParameter[] CreateParam(TijianParamInfo info)
    {
        SqlParameter[] param = new[]
        {
            new SqlParameter("@Nick",info.Nick),
             new SqlParameter("@ParamName",info.ParamName),
              new SqlParameter("@ParamValue",info.ParamValue)
        };
        return param;
    }
}
