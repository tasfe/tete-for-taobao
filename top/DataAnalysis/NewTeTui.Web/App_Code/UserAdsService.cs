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
/// 用户广告存取类
/// </summary>
public class UserAdsService
{

    const string SQL_SELECT_ALL_USERADS = "SELECT [Id],[AdsTitle],[AdsUrl],[AdsId],[UserAdsState],[AdsShowStartTime],[AdsShowFinishTime],[AliWang],[SellCateName],AddTime,FeeId,AdsPic FROM BangT_UserAds WHERE Nick=@Nick AND IsSend=0";

    const string SQL_SELECT_ALL_USERADSLIST = "SELECT [Id],[AdsTitle],[AdsUrl],[AdsId],[UserAdsState],[AdsShowStartTime],[AdsShowFinishTime],[AliWang],[SellCateName],AddTime,FeeId,AdsPic,IsSend FROM BangT_UserAds WHERE Nick=@Nick";

    const string SQL_INSERT = "INSERT BangT_UserAds([Id],[AdsTitle],[AdsUrl],[AdsId],[UserAdsState],[AdsShowStartTime],[AdsShowFinishTime],[AliWang],[SellCateName],AddTime,FeeId,Nick,CateIds,AdsPic,Price,IsSend) VALUES(@Id,@AdsTitle,@AdsUrl,@AdsId,@UserAdsState,@AdsShowStartTime,@AdsShowFinishTime,@AliWang,@SellCateName,@AddTime,@FeeId,@Nick,@CateIds,@AdsPic,@Price,@IsSend)";

    const string SQL_SELECT_USEDADS = "SELECT FeeId,Nick FROM BangT_UserAds WHERE UserAdsState=1 AND FeeId IN (SELECT FeeId FROM BangT_Fee WHERE AdsType=5)";

    const string SQL_SELECT_TAOCLASS_ADSINFO = "SELECT [Id],[AdsTitle],[AdsUrl],[AdsId],[AliWang],AdsPic FROM BangT_UserAds WHERE UserAdsState=@UserAdsState AND CHARINDEX(@CateId,CateIds,0)>0";

    const string SQL_DELETE_USERADS = "DELETE FROM BangT_UserAds WHERE Id=@Id";

    const string SQL_UPDATE_ADS_STATE = "UPDATE BangT_UserAds SET UserAdsState=@UserAdsState,AdsShowStartTime=@AdsShowStartTime,AdsShowFinishTime=@AdsShowFinishTime,AddTime=@AddTime,FeeId=@FeeId,AdsId=@AdsId WHERE Id=@Id";

    const string SQL_SELECT_USERADS = "SELECT [AdsTitle],[AdsUrl],[AdsId],[UserAdsState],[AdsShowStartTime],[AdsShowFinishTime],[AliWang],[SellCateName],AddTime,FeeId,AdsPic FROM BangT_UserAds WHERE Id=@Id";

    const string SQL_STOP = "UPDATE BangT_UserAds SET UserAdsState=@UserAdsState WHERE Id=@Id";

    const string SQL_SELECT_USERADS_BY_ADSID = "SELECT [Id],[AdsTitle],[AdsUrl],[AdsShowStartTime],[AdsShowFinishTime],[AliWang],[SellCateName],AddTime,FeeId,AdsPic,Price,Nick FROM BangT_UserAds WHERE AdsId=@AdsId AND UserAdsState=@UserAdsState";

    const string SQL_SELECT_ADSCLICK = "select ua.*,c.ClickCount from (select Id,AdsUrl,FeeId from BangT_UserAds where UserAdsState=1 and AddTime<@time) ua left join BangT_Click c on ua.Id=c.UserAdsId and ClickDate=@date order by c.ClickCount desc";

    const string SQL_SELECT_CLICKIP_USERADSID = "select UserAdsId from BangT_UserAds ua inner join BangT_ClickIP c on ua.Id=c.UserAdsId and VisitDate=@date and VisitIP=@ip";

    const string SQL_UPDATE_ADSSITE = "UPDATE BangT_UserAds SET AdsId=@AdsId WHERE Id=@Id";

    public int UpdateAdsSite(Guid id, Guid adsId)
    {
        SqlParameter[] param = new[]
        {
            new SqlParameter("@Id",id),
            new SqlParameter("@AdsId",adsId)
        };

        return DBHelper.ExecuteNonQuery(SQL_UPDATE_ADSSITE, param);
    }

    public IList<Guid> SelectAllClickIP(DateTime date,string ip)
    {
        SqlParameter[] param = new SqlParameter[]
        {
            new SqlParameter("@date", date.ToString("yyyyMMdd")),
            new SqlParameter("@ip",ip)
        };

        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_CLICKIP_USERADSID, param);
        IList<Guid> list = new List<Guid>();
        foreach (DataRow dr in dt.Rows)
        {
            Guid g = new Guid(dr["UserAdsId"].ToString()); //这个id作为广告的id

            list.Add(g);
        }

        return list;
    }

    public IList<UserAdsInfo> SelectAllUserAdsClick(DateTime date)
    {

        SqlParameter[] param = new[]
        {
            new SqlParameter("@time",date.AddMinutes(-30)),
            new SqlParameter("@date", date.ToString("yyyyMMdd"))
        };

        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_ADSCLICK, param);
        IList<UserAdsInfo> list = new List<UserAdsInfo>();

        foreach (DataRow dr in dt.Rows)
        {
            UserAdsInfo info = new UserAdsInfo();
            info.Id = new Guid(dr["Id"].ToString());
            info.AdsUrl = dr["AdsUrl"].ToString();
            info.IsSend = dr["ClickCount"] == DBNull.Value ? 0 : int.Parse(dr["ClickCount"].ToString()); //这个是否赠送作为点击次数
            info.FeeId = new Guid(dr["FeeId"].ToString());

            list.Add(info);
        }

        return list;
    }

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

    public IList<UserAdsInfo> SelectAllUserAdsList(string nick)
    {
        IList<UserAdsInfo> list = new List<UserAdsInfo>();

        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_ALL_USERADSLIST, new SqlParameter("@Nick", nick));
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
            info.IsSend = dr["IsSend"] == DBNull.Value ? 0 : int.Parse(dr["IsSend"].ToString());

            list.Add(info);
        }

        return list;
    }

    public IList<UserAdsInfo> SelectAllUserAdsByAdsId(Guid adsId, int state)
    {
        IList<UserAdsInfo> list = new List<UserAdsInfo>();
        SqlParameter[] param = new[]
            {
                new SqlParameter("@AdsId", adsId),
                new SqlParameter("@UserAdsState",state)
            };
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_USERADS_BY_ADSID, param);
        foreach (DataRow dr in dt.Rows)
        {
            UserAdsInfo info = new UserAdsInfo();
            info.Id = new Guid(dr["Id"].ToString());
            info.AdsTitle = dr["AdsTitle"].ToString();
            info.AdsUrl = dr["AdsUrl"].ToString();
            info.AdsShowStartTime = DateTime.Parse(dr["AdsShowStartTime"].ToString());
            info.AdsShowFinishTime = DateTime.Parse(dr["AdsShowFinishTime"].ToString());
            info.AliWang = dr["AliWang"].ToString();
            info.SellCateName = dr["SellCateName"].ToString();
            info.AddTime = DateTime.Parse(dr["AddTime"].ToString());
            info.FeeId = new Guid(dr["FeeId"].ToString());
            info.AdsPic = dr["AdsPic"].ToString();
            info.Nick = dr["Nick"].ToString();
            info.Price = dr["Price"] == DBNull.Value ? 0 : decimal.Parse(dr["Price"].ToString());

            list.Add(info);
        }

        return list;
    }

    public UserAdsInfo SelectUserAdsById(Guid id)
    {
        UserAdsInfo info = null;
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_USERADS, new SqlParameter("@Id", id));
        foreach (DataRow dr in dt.Rows)
        {
            info = new UserAdsInfo();
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
        }

        return info;
    }

    public IList<UserAdsInfo> SelectAllTouUserAds(string cid,int state)
    {
        IList<UserAdsInfo> list = new List<UserAdsInfo>();

        SqlParameter[] param = new SqlParameter[]
        {
            new SqlParameter("@UserAdsState", state),
            new SqlParameter("@CateId",cid)
        };

        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_TAOCLASS_ADSINFO, param);
        foreach (DataRow dr in dt.Rows)
        {
            UserAdsInfo info = new UserAdsInfo();
            info.Id = new Guid(dr["Id"].ToString());
            info.AdsTitle = dr["AdsTitle"].ToString();
            info.AdsUrl = dr["AdsUrl"].ToString();
            info.AdsId = new Guid(dr["AdsId"].ToString());
            info.AliWang = dr["AliWang"].ToString();
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

    public int DelteUserAds(Guid id)
    {
        return DBHelper.ExecuteNonQuery(SQL_DELETE_USERADS, new SqlParameter("@Id", id));
    }

    public int StopUserAds(int state, Guid id)
    {
        SqlParameter[] param = new[]
        {
            new SqlParameter("@Id",id),
            new SqlParameter("@UserAdsState",state)
        };

        return DBHelper.ExecuteNonQuery(SQL_STOP, param);
    }

    public int UpdateUserAdsState(UserAdsInfo info)
    {
        SqlParameter[] param = new[]
        {
            new SqlParameter("@Id",info.Id),
            new SqlParameter("@UserAdsState",info.UserAdsState),
            new SqlParameter("@AdsShowStartTime",info.AdsShowStartTime==DateTime.MinValue?DateTime.Parse("1990-1-1"):info.AdsShowStartTime),
            new SqlParameter("@AdsShowFinishTime",info.AdsShowFinishTime==DateTime.MinValue?DateTime.Parse("1990-1-1"):info.AdsShowFinishTime),
            new SqlParameter("@AddTime",info.AddTime==DateTime.MinValue?DateTime.Parse("1990-1-1"):info.AddTime),
            new SqlParameter("@FeeId",info.FeeId),
            new SqlParameter("@AdsId",info.AdsId)
        };

        return DBHelper.ExecuteNonQuery(SQL_UPDATE_ADS_STATE, param);
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
            new SqlParameter("@AdsPic",info.AdsPic),
            new SqlParameter("@Price",info.Price),
            new SqlParameter("@IsSend",info.IsSend)
        };

        return param;
    }
}
