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

/// <summary>
/// 推广商品存取类
/// </summary>
public class TuiGoodsService
{
    const string SQL_INSERT = "INSERT BaiT_TuiGoods(TuiId,GoodsPic,GoodsId,GoodsName,Keywords,Nick,Type) VALUES(@TuiId,@GoodsPic,@GoodsId,@GoodsName,@Keywords,@Nick,@Type)";

    const string SQL_SELECT_BYID = "SELECT GoodsId FROM BaiT_TuiGoods WHERE SUBSTRING(CAST(TuiId AS VARCHAR(50)),0,9)=@TuiId";

    const string SQL_SELECT_BY_ID = "SELECT GoodsId,GoodsName,Keywords,GoodsPic FROM BaiT_TuiGoods WHERE TuiId=@TuiId";

    const string SQL_SELECT_All_BY_TYPE = "SELECT [TuiId],[GoodsId],[GoodsName],[GoodsPic],[Keywords] FROM BaiT_TuiGoods WHERE [Nick]=@nick AND [Type]=@type";

    public int InsertTuiGoods(TuiGoodsInfo info)
    {
        return DBHelper.ExecuteNonQuery(SQL_INSERT, CreateSqlParameter(info));
    }

    public string GetGoodsId(string tuiId)
    {
        SqlParameter param = new SqlParameter("@TuiId", tuiId);

        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_BYID, param);
        foreach (DataRow dr in dt.Rows)
        {
            return dr[0].ToString();
        }

        return "";
    }

    public TuiGoodsInfo GetTuiGoods(Guid tuiId)
    {
        SqlParameter param = new SqlParameter("@TuiId", tuiId);

        TuiGoodsInfo info = null;

        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_BY_ID, param);
        foreach (DataRow dr in dt.Rows)
        {
            info = new TuiGoodsInfo();
            info.GoodsId = dr["GoodsId"].ToString();
            info.GoodsName = dr["GoodsName"].ToString();
            info.Keywords = dr["Keywords"].ToString();
            info.GoodsPic = dr["GoodsPic"].ToString();
        }

        return info;
    }

    public IList<TuiGoodsInfo> GetAllTuiGoodsByType(string nick,int type)
    {
        SqlParameter[] param = new[]
        {
            new SqlParameter("@nick",nick),
            new SqlParameter("@type",type)
        };
        IList<TuiGoodsInfo> list = new List<TuiGoodsInfo>();
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_All_BY_TYPE, param);
        foreach (DataRow dr in dt.Rows)
        {
            TuiGoodsInfo info = new TuiGoodsInfo();
            info.GoodsId = dr["GoodsId"].ToString();
            info.TuiId = new Guid(dr["TuiId"].ToString());
            info.GoodsName = dr["GoodsName"].ToString();
            info.GoodsPic = dr["GoodsPic"].ToString();
            info.Keywords = dr["Keywords"].ToString();

            list.Add(info);
        }

        return list;
    }

    private static SqlParameter[] CreateSqlParameter(TuiGoodsInfo info)
    {
        SqlParameter[] param = new[]
        {
            new SqlParameter("@TuiId",info.TuiId),
            new SqlParameter("@GoodsPic",info.GoodsPic),
            new SqlParameter("@GoodsId",info.GoodsId),
            new SqlParameter("@GoodsName",info.GoodsName),
            new SqlParameter("@Keywords",info.Keywords),
            new SqlParameter("@Nick",info.Nick),
            new SqlParameter("@Type",info.Type)
        };

        return param;
    }
}
