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
using System.Data.SqlClient;
using System.Collections.Generic;

/// <summary>
/// 广告点击数据存取
/// </summary>
public class ClickService
{
    const string SQL_INSERT = "INSERT BangT_Click(UserAdsId,ClickDate,ClickCount,ClickType) VALUES(@UserAdsId,@ClickDate,@ClickCount,@ClickType)";

    const string SQL_UPDATE = "UPDATE BangT_Click SET ClickCount=ClickCount+1 WHERE UserAdsId=@UserAdsId AND ClickDate=@ClickDate AND ClickType=@ClickType";

    const string SQL_SELECT_HASCOUNT = "SELECT COUNT(*) AS HASCOUNT FROM BangT_Click WHERE UserAdsId=@UserAdsId AND ClickDate=@ClickDate AND ClickType=@ClickType";

    const string SQL_SELCT_CLICK_BY_DATE = "SELECT ClickCount,ClickDate FROM BangT_Click WHERE UserAdsId=@UserAdsId AND ClickDate BETWEEN @start AND @end";

    public static int InsertClickInfo(ClickInfo info)
    {
        return DBHelper.ExecuteNonQuery(SQL_INSERT, CreateParameter(info));
    }

    public static int UpdateClickInfo(ClickInfo info)
    {
        return DBHelper.ExecuteNonQuery(SQL_UPDATE, CreateParameter(info));
    }

    public IList<ClickInfo> SelectAllClickCount(Guid adsId, string start, string end)
    {
        SqlParameter[] param = new[]
        {
            new SqlParameter("@UserAdsId",adsId),
            new SqlParameter("@start",start),
            new SqlParameter("@end",end)
        };

        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELCT_CLICK_BY_DATE, param);
        IList<ClickInfo> list = new List<ClickInfo>();
        foreach (DataRow dr in dt.Rows)
        {
            ClickInfo info = new ClickInfo();
            info.ClickDate = dr["ClickDate"].ToString();
            info.ClickCount = int.Parse(dr["ClickCount"].ToString());

            list.Add(info);
        }

        return list;
    }

    /// <summary>
    /// 查找是否插入过
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public static bool SelectHasCount(ClickInfo info)
    {
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_HASCOUNT, CreateParameter(info));

        foreach (DataRow dr in dt.Rows)
        {
            return int.Parse(dr[0].ToString()) == 0 ? false : true;
        }

        return false;
    }

    private static SqlParameter[] CreateParameter(ClickInfo info)
    {
        SqlParameter[] param = new[]
        {
            new SqlParameter("@UserAdsId",info.UserAdsId),
            new SqlParameter("@ClickCount",info.ClickCount),
            new SqlParameter("@ClickDate",info.ClickDate),
            new SqlParameter("@ClickType",info.ClickType)
        };

        return param;
    }
}
