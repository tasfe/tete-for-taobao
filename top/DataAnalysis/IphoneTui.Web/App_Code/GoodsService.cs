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
/// 商品存取类
/// </summary>
public class GoodsService
{
    const string SQL_SELECT_ALLGOODS = "SELECT GoodsId,GoodsName,GoodsPrice,GoodsCount,GoodsPic,Modified,CateId,TaoBaoCId FROM [BangT_Goods] WHERE Nick=@Nick";

    const string SQL_INSERT = "INSERT BangT_Goods(GoodsId,GoodsName,GoodsPrice,GoodsCount,GoodsPic,Modified,CateId,TaoBaoCId,Nick) VALUES(@GoodsId,@GoodsName,@GoodsPrice,@GoodsCount,@GoodsPic,@Modified,@CateId,@TaoBaoCId,@Nick)";

    const string SQL_DELETE = "DELETE FROM BangT_Goods WHERE Nick=@Nick";

    const string SQL_SELECT_NICK = "select COUNT(*) from dbo.BangT_Goods where nick=@nick";

    public int DeleteGoodsByNick(string nick)
    {
        return DBHelper.ExecuteNonQuery(SQL_DELETE, new SqlParameter("@Nick", nick));
    }

    public int SelectGoodsCountByNick(string nick)
    {
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_NICK, new SqlParameter("@nick", nick));

        foreach (DataRow dr in dt.Rows)
        {
            if (dr[0] == DBNull.Value)
                return 0;
            return int.Parse(dr[0].ToString());
        }

        return 0;
    }

    public IList<GoodsInfo> SelectAllGoodsByNick(string nick)
    {
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_ALLGOODS, new SqlParameter("@Nick", nick));
        IList<GoodsInfo> list = new List<GoodsInfo>();

        foreach (DataRow dr in dt.Rows)
        {
            GoodsInfo info = new GoodsInfo();
            info.GoodsId = dr["GoodsId"].ToString();
            info.GoodsName = dr["GoodsName"].ToString();
            info.GoodsPrice = decimal.Parse(dr["GoodsPrice"].ToString());
            info.GoodsPic = dr["GoodsPic"].ToString();
            info.GoodsCount = int.Parse(dr["GoodsCount"].ToString());
            info.Modified = DateTime.Parse(dr["Modified"].ToString());
            info.CateId = dr["CateId"].ToString();
            info.TaoBaoCId = dr["TaoBaoCId"].ToString();

            list.Add(info);
        }

        return list;
    }

    public IList<GoodsInfo> SearchGoods(string nick, string goodsname, string cateId, params DateTime[] dates)
    {
        IList<GoodsInfo> list = new List<GoodsInfo>();

        string sql = SQL_SELECT_ALLGOODS;

        if (!string.IsNullOrEmpty(goodsname))
        {
            sql += " AND CHARINDEX('" + goodsname + "',GoodsName,0)>0";
        }

        if (!string.IsNullOrEmpty(cateId))
        {
            sql += " AND CHARINDEX('" + cateId + "',CateId,0)>0";
        }

        if (dates != null && dates.Length == 2)
        {
            sql += " AND Modified BETWEEN '" + dates[0] + "' AND '" + dates[1] + "'";
        }
            
        DataTable dt = DBHelper.ExecuteDataTable(sql, new SqlParameter("@Nick", nick));
        foreach (DataRow dr in dt.Rows)
        {
            GoodsInfo info = new GoodsInfo();
            info.GoodsId = dr["GoodsId"].ToString();
            info.GoodsName = dr["GoodsName"].ToString();
            info.GoodsPrice = decimal.Parse(dr["GoodsPrice"].ToString());
            info.GoodsPic = dr["GoodsPic"].ToString();
            info.GoodsCount = int.Parse(dr["GoodsCount"].ToString());
            info.Modified = DateTime.Parse(dr["Modified"].ToString());
            info.CateId = dr["CateId"].ToString();
            info.TaoBaoCId = dr["TaoBaoCId"].ToString();

            list.Add(info);
        }

        return list;
    }

    public int InsertGoodsInfo(TaoBaoGoodsInfo info, string nick)
    {
        return DBHelper.ExecuteNonQuery(SQL_INSERT, CreateParameter(info, nick));
    }

    private static SqlParameter[] CreateParameter(TaoBaoGoodsInfo info, string nick)
    {
        SqlParameter[] param = new[]
        {
            new SqlParameter("@GoodsId",info.num_iid),
            new SqlParameter("@GoodsName",info.title),
            new SqlParameter("@GoodsPrice",info.price),
            new SqlParameter("@GoodsCount",info.num),
            new SqlParameter("@GoodsPic",info.pic_url),
            new SqlParameter("@Modified",info.modified),
            new SqlParameter("@CateId",info.seller_cids),
            new SqlParameter("@TaoBaoCId",info.cid),
            new SqlParameter("@Nick",nick)
        };

        return param;
    }
}
