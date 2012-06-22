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
}
