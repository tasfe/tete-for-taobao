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
    const string SQL_INSERT = "INSERT BangT_SiteAds(Id,SiteId,AdsId,AdsPosition,AdsPic,AdsCount,AdsCalCount,PositionCode) VALUES(@Id,@SiteId,@AdsId,@AdsPosition,@AdsPic,@AdsCount,@AdsCalCount,@PositionCode)";

    const string SQL_SELECT = "SELECT [Id],[SiteId] ,[AdsId],[AdsPosition] ,[AdsPic],[AdsCount],[AdsCalCount] ,[PositionCode],[score] FROM BangT_SiteAds";

    const string SQL_SELECT_ADSCODE = "SELECT AdsCode FROM BangT_SiteAds WHERE Id=@Id";

    const string SQL_DELETE = "DELETE FROM BangT_SiteAds WHERE Id=@Id";

    const string SQL_SELECT_BY_ID = "SELECT [Id],[SiteId] ,[AdsId],[AdsPosition] ,[AdsPic],[AdsCount],[AdsCalCount] ,[PositionCode] FROM BangT_SiteAds WHERE SiteId=@Id";

    const string SQL_UPDATE = "UPDATE BangT_SiteAds SET AdsId=@AdsId,AdsPosition=@AdsPosition,AdsCount=@AdsCount,AdsCalCount=@AdsCalCount,PositionCode=@PositionCode WHERE Id=@Id";

    public int InsertSiteAds(SiteAdsInfo info)
    {
        return DBHelper.ExecuteNonQuery(SQL_INSERT, CreateParameter(info));
    }

    public IList<SiteAdsInfo> SelectAllSiteAds()
    {
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT);
        IList<SiteAdsInfo> list = new List<SiteAdsInfo>();

        foreach (DataRow dr in dt.Rows)
        {
            SiteAdsInfo info = new SiteAdsInfo();
            info.Id = new Guid(dr["Id"].ToString());
            info.AdsCalCount = int.Parse(dr["AdsCalCount"].ToString());
            info.AdsCount = int.Parse(dr["AdsCount"].ToString());
            info.AdsId = new Guid(dr["AdsId"].ToString());
            info.AdsPic = dr["AdsPic"].ToString();
            info.AdsPosition = dr["AdsPosition"].ToString();
            info.PositionCode = dr["PositionCode"] == DBNull.Value ? -1 : int.Parse(dr["PositionCode"].ToString());
            info.SiteId = new Guid(dr["SiteId"].ToString());
            info.Score = int.Parse(dr["Score"].ToString());

            list.Add(info);
        }

        return list;
    }

    public IList<SiteAdsInfo> SelectSiteAdsById(Guid id)
    {
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_BY_ID, new SqlParameter("@Id", id));
        IList<SiteAdsInfo> list = new List<SiteAdsInfo>();

        foreach (DataRow dr in dt.Rows)
        {
            SiteAdsInfo info = new SiteAdsInfo();
            info.Id = new Guid(dr["Id"].ToString());
            info.AdsCalCount = int.Parse(dr["AdsCalCount"].ToString());
            info.AdsCount = int.Parse(dr["AdsCount"].ToString());
            info.AdsId = new Guid(dr["AdsId"].ToString());
            info.AdsPic = dr["AdsPic"].ToString();
            info.AdsPosition = dr["AdsPosition"].ToString();
            info.PositionCode = dr["PositionCode"] == DBNull.Value ? -1 : int.Parse(dr["PositionCode"].ToString()); ;
            info.SiteId = new Guid(dr["SiteId"].ToString());

            list.Add(info);
        }

        return list;
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
        return DBHelper.ExecuteNonQuery(SQL_UPDATE, CreateParameterUp(info));
    }

    public SqlParameter[] CreateParameterUp(SiteAdsInfo info)
    {
        //Id,SiteId,AdsId,AdsPosition,AdsPic,AdsCount,AdsCalCount,PositionCode
        SqlParameter[] param = new[]
        {
            new SqlParameter("@Id",info.Id),
            new SqlParameter("@SiteId",info.SiteId),
            new SqlParameter("@AdsId",info.AdsId),
            new SqlParameter("@AdsPosition",info.AdsPosition),
            new SqlParameter("@AdsCount",info.AdsCount),
            new SqlParameter("@AdsCalCount",info.AdsCalCount),
            new SqlParameter("@PositionCode",info.PositionCode)
        };

        return param;
    }

    public SqlParameter[] CreateParameter(SiteAdsInfo info)
    {
        //Id,SiteId,AdsId,AdsPosition,AdsPic,AdsCount,AdsCalCount,PositionCode
        SqlParameter[] param = new[]
        {
            new SqlParameter("@Id",info.Id),
            new SqlParameter("@SiteId",info.SiteId),
            new SqlParameter("@AdsId",info.AdsId),
            new SqlParameter("@AdsPosition",info.AdsPosition),
            new SqlParameter("@AdsPic",info.AdsPic),
            new SqlParameter("@AdsCount",info.AdsCount),
            new SqlParameter("@AdsCalCount",info.AdsCalCount),
            new SqlParameter("@PositionCode",info.PositionCode)
        };

        return param;
    }
}