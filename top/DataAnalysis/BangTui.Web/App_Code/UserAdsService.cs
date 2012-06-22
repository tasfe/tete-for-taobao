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
using System.Collections.Generic;
using System.Data.SqlClient;

/// <summary>
/// 用户广告存取类
/// </summary>
public class UserAdsService
{

    const string SQL_SELECT_ALL_USERADS = "SELECT [Id],[AdsTitle],[AdsUrl],[AdsId],[UserAdsState],[AdsShowStartTime],[AdsShowFinishTime],[AliWang],[SellCateName],AddTime,FeeId,AdsPic FROM BangT_UserAds WHERE Nick=@Nick";

    const string SQL_INSERT = "INSERT BangT_UserAds([Id],[AdsTitle],[AdsUrl],[AdsId],[UserAdsState],[AdsShowStartTime],[AdsShowFinishTime],[AliWang],[SellCateName],AddTime,FeeId,Nick,CateIds,AdsPic) VALUES(@Id,@AdsTitle,@AdsUrl,@AdsId,@UserAdsState,@AdsShowStartTime,@AdsShowFinishTime,@AliWang,@SellCateName,@AddTime,@FeeId,@Nick,@CateIds,@AdsPic)";

    const string SQL_SELECT_USEDADS = "SELECT FeeId,Nick FROM BangT_UserAds WHERE UserAdsState=1 AND FeeId IN (SELECT FeeId FROM BangT_Fee WHERE AdsType=5)";

    public IList<UserAdsInfo> SelectAllUserAds(string nick)
    {
        IList<UserAdsInfo> list = new List<UserAdsInfo>();

        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_ALL_USERADS, new SqlParameter("@Nick", nick));
        foreach (DataRow dr in dt.Rows)
        {
            UserAdsInfo info = new UserAdsInfo();
            info.Id = new Guid(dr["Id"].ToString());
            info.AdsTitle = dr["AdsTitle"].ToString();
            info.AdsUrl = dr["AdsUrl"].ToString();
            info.AdsId = new Guid(dr["AdsId"].ToString());
            info.UserAdsState = int.Parse(dr["UserAdsState"].ToString());
            info.AdsShowStartTime = DateTime.Parse(dr["AdsShowStartTime"].ToString());
            info.AdsShowFinishTime = DateTime.Parse(dr["AdsShowFinishTime"].ToString());
            info.AliWang = dr["AliWang"].ToString();
            info.SellCateName = dr["SellCateName"].ToString();
            info.AddTime = DateTime.Parse(dr["AddTime"].ToString());
            info.FeeId = new Guid(dr["FeeId"].ToString());
            info.AdsPic = dr["AdsPic"].ToString();

            list.Add(info);
        }

        return list;
    }

    /// <summary>
    /// 查找所有已经使用了的类型为5的广告位
    /// </summary>
    /// <returns></returns>
    public IList<UserAdsInfo> SelectAllUsedAds()
    {
        IList<UserAdsInfo> list = new List<UserAdsInfo>();

        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_USEDADS);
        foreach (DataRow dr in dt.Rows)
        {
            UserAdsInfo info = new UserAdsInfo();
            info.Nick = dr["Nick"].ToString();
            info.FeeId = new Guid(dr["FeeId"].ToString());

            list.Add(info);
        }

        return list;
    }


    public int InsertUserAds(UserAdsInfo info)
    {
        return DBHelper.ExecuteNonQuery(SQL_INSERT, CreateParameter(info));
    }

    private static SqlParameter[] CreateParameter(UserAdsInfo info)
    {
        SqlParameter[] param = new[]
        {
            new SqlParameter("@Id",info.Id),
            new SqlParameter("@AdsTitle",info.AdsTitle),
            new SqlParameter("@AdsUrl",info.AdsUrl),
            new SqlParameter("@AdsId",string.IsNullOrEmpty(info.AdsId.ToString())?Guid.Empty:info.AdsId),
            new SqlParameter("@UserAdsState",info.UserAdsState),
            new SqlParameter("@AdsShowStartTime",info.AdsShowStartTime==DateTime.MinValue?DateTime.Parse("1990-1-1"):info.AdsShowStartTime),
            new SqlParameter("@AdsShowFinishTime",info.AdsShowFinishTime==DateTime.MinValue?DateTime.Parse("1990-1-1"):info.AdsShowFinishTime),
            new SqlParameter("@AliWang",info.AliWang),
            new SqlParameter("@SellCateName",info.SellCateName),
            new SqlParameter("@AddTime",info.AddTime==DateTime.MinValue?DateTime.Parse("1990-1-1"):info.AddTime),
            new SqlParameter("@FeeId",string.IsNullOrEmpty(info.FeeId.ToString())?Guid.Empty:info.FeeId),
            new SqlParameter("@Nick",info.Nick),
            new SqlParameter("@CateIds",info.CateIds),
            new SqlParameter("@AdsPic",info.AdsPic)
        };

        return param;
    }
}
