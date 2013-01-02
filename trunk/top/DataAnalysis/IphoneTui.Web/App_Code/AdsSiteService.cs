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
using System.Collections.Generic;
using System.Data.SqlClient;

/// <summary>
/// 广告网站存取类
/// </summary>
public class AdsSiteService
{
    #region 存取sql

    const string SQL_ADD_SITE = "INSERT BangT_Sites(SiteId,SiteName,SiteUrl) VALUES(@SiteId,@SiteName,@SiteUrl)";

    const string SQL_ADD_ADS = "INSERT BangT_Ads(AdsId,SiteId,AdsName,AdsSize,AdsType,AdsPic) VALUES(@AdsId,@SiteId,@AdsName,@AdsSize,@AdsType,@AdsPic)";

    const string SQL_UPDATE_SITE = "UPDATE BangT_Sites SET SiteName=@SiteName,SiteUrl=@SiteUrl WHERE SiteId=@SiteId";

    const string SQL_UPDATE_ADS = "UPDATE BangT_Ads SET AdsName=@AdsName,AdsSize=@AdsSize WHERE AdsId=@AdsId";

    const string SQL_DELETE_SITE = "DELETE FROM BangT_Sites WHERE SiteId=@SiteId";

    const string SQL_DELETE_ADS = "DELETE FROM BangT_SiteAds WHERE Id=@Id";

    const string SQL_SELECT_SITE_ID = "SELECT SiteId,SiteName,SiteUrl FROM BangT_Sites WHERE SiteId=@SiteId";

    const string SQL_SELECT_SITE_ALL = "SELECT SiteId,SiteName,SiteUrl FROM BangT_Sites";

    const string SQL_SELECT_ADS_BY_ID = "SELECT AdsId,SiteId,AdsName,AdsSize,AdsType,AdsPic FROM BangT_Ads WHERE AdsId=@AdsId";

    const string SQL_SELECT_ADS_BY_SITEID = "SELECT id,SiteId,AdsId,AdsPosition,AdsPic,AdsCount,AdsCalCount,PositionCode FROM BangT_SiteAds WHERE SiteId=@SiteId";

    const string SQL_SELECT_ADS = "SELECT AdsId,AdsName,AdsSize,AdsType FROM BangT_Ads";

    #endregion

    #region 存取方法

    /// <summary>
    /// 插入一个网站
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public int AddSite(SiteInfo info)
    {
        return DBHelper.ExecuteNonQuery(SQL_ADD_SITE, CreateSite(info));
    }

    /// <summary>
    /// 更新一个网站
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public int UpdateSite(SiteInfo info)
    {
        return DBHelper.ExecuteNonQuery(SQL_UPDATE_SITE, CreateSite(info));
    }

    /// <summary>
    /// 插入一个广告位
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public int AddAds(AdsInfo info)
    {
        return DBHelper.ExecuteNonQuery(SQL_ADD_ADS, CreateAds(info));
    }

    /// <summary>
    /// 更新一个广告位
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public int UpdateAds(AdsInfo info)
    {
        return DBHelper.ExecuteNonQuery(SQL_UPDATE_ADS, CreateAds(info));
    }

    /// <summary>
    /// 查找一个网站
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public SiteInfo SelectSite(Guid id)
    {
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_SITE_ID, new SqlParameter("@SiteId", id));
        SiteInfo info = null;
        foreach (DataRow dr in dt.Rows)
        {
            info = new SiteInfo();
            info.SiteId = new Guid(dr["SiteId"].ToString());
            info.SiteName = dr["SiteName"].ToString();
            info.SiteUrl = dr["SiteUrl"].ToString();
        }

        return info;
    }

    /// <summary>
    /// 查找所有网站
    /// </summary>
    /// <returns></returns>
    public IList<SiteInfo> SelectAllSite()
    {
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_SITE_ALL);
        IList<SiteInfo> list = new List<SiteInfo>();
        foreach (DataRow dr in dt.Rows)
        {
            SiteInfo info = new SiteInfo();
            info = new SiteInfo();
            info.SiteId = new Guid(dr["SiteId"].ToString());
            info.SiteName = dr["SiteName"].ToString();
            info.SiteUrl = dr["SiteUrl"].ToString();

            list.Add(info);
        }

        return list;
    }

    /// <summary>
    /// 指定网站查找所有广告
    /// </summary>
    /// <param name="siteId"></param>
    /// <returns></returns>
    public IList<AdsInfo> SelectAllAdsBySiteId(Guid siteId)
    {
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_ADS_BY_SITEID, new SqlParameter("@SiteId", siteId));
        IList<AdsInfo> list = new List<AdsInfo>();
        foreach (DataRow dr in dt.Rows)
        {
            AdsInfo info = new AdsInfo();
            info = new AdsInfo();
            info.AdsSize = dr["AdsSize"].ToString();
            info.AdsName = dr["AdsName"].ToString();
            info.AdsId = new Guid(dr["AdsId"].ToString());
            info.AdsType = int.Parse(dr["AdsType"].ToString());

            list.Add(info);
        }

        return list;
    }

    /// <summary>
    /// 指定网站查找所有广告
    /// </summary>
    /// <returns></returns>
    public IList<AdsInfo> SelectAllAds()
    {
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_ADS);
        IList<AdsInfo> list = new List<AdsInfo>();
        foreach (DataRow dr in dt.Rows)
        {
            AdsInfo info = new AdsInfo();
            info = new AdsInfo();
            info.AdsSize = dr["AdsSize"].ToString();
            info.AdsName = dr["AdsName"].ToString();
            info.AdsId = new Guid(dr["AdsId"].ToString());
            info.AdsType = int.Parse(dr["AdsType"].ToString());
            list.Add(info);
        }

        return list;
    }

    /// <summary>
    /// 查找单个广告
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public AdsInfo SelectAdsInfo(Guid id)
    {
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_ADS_BY_ID, new SqlParameter("@AdsId", id));
        AdsInfo info = null;
        foreach (DataRow dr in dt.Rows)
        {
            info = new AdsInfo();
            info.AdsSize = dr["AdsSize"].ToString();
            info.AdsName = dr["AdsName"].ToString();
            info.AdsId = new Guid(dr["AdsId"].ToString());
            info.AdsType = int.Parse(dr["AdsType"].ToString());
        }

        return info;
    }

    /// <summary>
    /// 删除网站
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public int DeleteSite(Guid id)
    {
        //先删除该网站下的所有广告
        IList<AdsInfo> list = SelectAllAdsBySiteId(id);

        foreach (AdsInfo info in list)
            DeleteAds(info.AdsId);


        return DBHelper.ExecuteNonQuery(SQL_DELETE_SITE, new SqlParameter("@SiteId", id));
    }

    /// <summary>
    /// 删除指定广告
    /// </summary>
    /// <param name="adsId"></param>
    /// <returns></returns>
    public int DeleteAds(Guid Id)
    {
        return DBHelper.ExecuteNonQuery(SQL_DELETE_ADS, new SqlParameter("@Id", Id));
    }

    #endregion

    private static SqlParameter[] CreateSite(SiteInfo info)
    {
        return new SqlParameter[]
        {
            new SqlParameter("@SiteId",info.SiteId),
            new SqlParameter("@SiteName",info.SiteName),
            new SqlParameter("@SiteUrl",info.SiteUrl)
        };
    }

    private static SqlParameter[] CreateAds(AdsInfo info)
    {
        return new SqlParameter[]
        {
            new SqlParameter("@AdsId",info.AdsId),
            new SqlParameter("@AdsName",info.AdsName),
            new SqlParameter("@AdsSize",info.AdsSize),
            new SqlParameter("@AdsType",info.AdsType)
        };
    }
}
