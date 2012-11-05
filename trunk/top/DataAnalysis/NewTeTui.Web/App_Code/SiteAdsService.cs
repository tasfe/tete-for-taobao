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
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// 网站调用广告数据存取
/// </summary>
public class SiteAdsService
{
    const string SQL_INSERT = "INSERT BangT_SiteAds(Id,SiteUrl,AdsPosition,AdsCode,AdsType) VALUES(@Id,@SiteUrl,@AdsPosition,@AdsCode,@AdsType)";

    const string SQL_SELECT = "SELECT Id,SiteUrl,AdsPosition,AdsCode,AdsType FROM BangT_SiteAds WHERE SiteUrl=@SiteUrl";

    const string SQL_SELECT_ADSCODE = "SELECT AdsCode FROM BangT_SiteAds WHERE Id=@Id";

    const string SQL_DELETE = "DELETE FROM BangT_SiteAds WHERE Id=@Id";

    const string SQL_SELECT_BY_ID = "SELECT Id,SiteUrl,AdsPosition,AdsCode,AdsType FROM BangT_SiteAds WHERE Id=@Id";

    const string SQL_UPDATE = "UPDATE BangT_SiteAds SET SiteUrl=@SiteUrl,AdsPosition=@AdsPosition,AdsCode=@AdsCode,AdsType=@AdsType WHERE Id=@Id";

    public int InsertSiteAds(SiteAdsInfo info)
    {
        return DBHelper.ExecuteNonQuery(SQL_INSERT, CreateParameter(info));
    }

    public IList<SiteAdsInfo> SelectAllSiteAds(string siteUrl)
    {
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT, new SqlParameter("@SiteUrl", siteUrl));
        IList<SiteAdsInfo> list = new List<SiteAdsInfo>();

        foreach (DataRow dr in dt.Rows)
        {
            SiteAdsInfo info = new SiteAdsInfo();
            info.Id = new Guid(dr["Id"].ToString());
            info.SiteUrl = dr["SiteUrl"].ToString();
            info.AdsPosition = dr["AdsPosition"].ToString();
            info.AdsCode = dr["AdsCode"].ToString();
            info.AdsType = (SiteAdsType)dr["AdsType"];

            list.Add(info);
        }

        return list;
    }

    public SiteAdsInfo SelectSiteAdsById(string id)
    {
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_BY_ID, new SqlParameter("@Id", id));
        SiteAdsInfo info = null;

        foreach (DataRow dr in dt.Rows)
        {
            info = new SiteAdsInfo();
            info.Id = new Guid(dr["Id"].ToString());
            info.SiteUrl = dr["SiteUrl"].ToString();
            info.AdsPosition = dr["AdsPosition"].ToString();
            info.AdsCode = dr["AdsCode"].ToString();
            info.AdsType = (SiteAdsType)dr["AdsType"];
        }

        return info;
    }

    public string GetAdsCode(string id)
    {
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_ADSCODE, new SqlParameter("@Id", id));

        foreach (DataRow dr in dt.Rows)
        {
            return dr[0].ToString();
        }

        return "";
    }

    public int DeleteAds(string id)
    {
        return DBHelper.ExecuteNonQuery(SQL_DELETE, new SqlParameter("@Id", id));
    }

    public int UpdateAds(SiteAdsInfo info)
    {
        return DBHelper.ExecuteNonQuery(SQL_UPDATE, CreateParameter(info));
    }

    public SqlParameter[] CreateParameter(SiteAdsInfo info)
    {
        SqlParameter[] param = new[]
        {
            new SqlParameter("@Id",info.Id),
            new SqlParameter("@SiteUrl",info.SiteUrl),
            new SqlParameter("@AdsPosition",info.AdsPosition),
            new SqlParameter("@AdsCode",info.AdsCode),
            new SqlParameter("@AdsType",info.AdsType)
        };

        return param;
    }
}