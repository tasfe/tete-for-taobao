using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

/// <summary>
/// 存取用户在售商品信息
/// </summary>
public class TaoBaoGoodsServive
{
    const string SQL_INSERT = "INSERT TopTaoBaoGoodsInfo(GoodsID,GoosName,NickNo) VALUES(@GoodsID,@GoosName,@NickNo)";

    const string SQL_SELECT_TOP_VISIT = @"SELECT * FROM(  
select COUNT(*) as gcount,goodsid,ROW_NUMBER() OVER(ORDER BY COUNT(*) DESC) as RowNum
from (select goodsid from @tableName
where visittime between @start and @end and GoodsId<>''
) a group by goodsid) B WHERE  RowNum between @snum and @enum";

    const string SQL_SELECT_TOP_VISIT_COUNT = @"SELECT COUNT(*) FROM(  
select COUNT(*) as gcount,goodsid
from (select goodsid from @tableName where visittime between @start and @end and GoodsId<>''
) a group by goodsid) b";

    const string SQL_SELECT_TOP_BUY= @"select *  from
(
select num_iid,SUM(num) as bcount,ROW_NUMBER() OVER(ORDER BY sum(num) DESC) as rownum
from(
  select od.num_iid,od.num from [TopTaoBaoGoodsOrderInfo] o 
  inner join TopTaoBaoOrderGoodsList od on o.tid=od.tid and o.created between @start
   and @end and od.status in('TRADE_FINISHED','TRADE_BUYER_SIGNED','WAIT_BUYER_CONFIRM_GOODS','WAIT_SELLER_SEND_GOODS') and 
   o.seller_nick=@nick) a group by num_iid
)  a where rownum between @snum and @enum";

    const string SQL_SELECT_TOP_BUY_COUNT = @"select COUNT(*) from
    (select SUM(num) as buycount,num_iid
from(select od.num_iid,od.num from [TopTaoBaoGoodsOrderInfo] o 
  inner join TopTaoBaoOrderGoodsList od on o.tid=od.tid and o.created between @start
   and @end and od.status in('TRADE_FINISHED','TRADE_BUYER_SIGNED','WAIT_BUYER_CONFIRM_GOODS','WAIT_SELLER_SEND_GOODS') and 
   o.seller_nick=@nick) a group by num_iid
   ) b";

    const string SQL_SELECT_TOP_SEE_BUY = @"select num_iid,SUM(num) as bcount
from(
  select od.num_iid,od.num from [TopTaoBaoGoodsOrderInfo] o 
  inner join TopTaoBaoOrderGoodsList od on o.tid=od.tid and o.created between @start
   and @end and od.status 
in('TRADE_FINISHED','TRADE_BUYER_SIGNED','WAIT_BUYER_CONFIRM_GOODS','WAIT_SELLER_SEND_GOODS')
 and 
   o.seller_nick=@nick) a group by num_iid having num_iid in(@pids)";

    const string SQL_UPDATE_PRICE = "UPDATE TopTaoBaoGoodsInfo SET PurchasePrice=@PurchasePrice WHERE GoodsId=@GoodsId AND NickNo=@NickNo";

    const string SQL_SELECT_GOODS_PRICE = "select * from(select *,ROW_NUMBER() OVER(ORDER BY GoodsId DESC) as  RowNum from TopTaoBaoGoodsInfo where NickNo=@nick and CHARINDEX(@goodsName,GoodsName)>0) a where RowNum between @snum and @enum";

    const string SQL_SELECT_GOODS_PRICE_COUNT = "select COUNT(*) from TopTaoBaoGoodsInfo where NickNo=@nick";

    public void InsertTaoBaoGoodsInfo(GoodsInfo info)
    {
        SqlParameter[] param = new[]
        {
            new SqlParameter("GoodsID",info.num_iid),
            new SqlParameter("GoosName",info.title),
            new SqlParameter("NickNo",info.nick)
        };
        DBHelper.ExecuteNonQuery(SQL_INSERT, param);
    }

    public int GetTopGoodsCount(string nick, DateTime start, DateTime end)
    {
        string sql = SQL_SELECT_TOP_VISIT_COUNT.Replace("@tableName", DBHelper.GetRealTable(nick));
        SqlParameter[] param = new[]
        {
            new SqlParameter("@start",start),
            new SqlParameter("@end",end)
        };
        return DBHelper.ExecuteScalar(sql, param);
    }

    public IList<GoodsInfo> GetTopGoods(string nick, DateTime start, DateTime end, int page, int count)
    {
        string sql = SQL_SELECT_TOP_VISIT.Replace("@tableName", DBHelper.GetRealTable(nick));
        int snum = 1;
        if (page != 1)
            snum = (page - 1) * count + 1;
        int endnum = page * count;
        SqlParameter[] param = new[]
        {
            new SqlParameter("@start",start),
            new SqlParameter("@end",end),
            new SqlParameter("@snum",snum),
            new SqlParameter("@enum",endnum)
        };

        DataTable dt = DBHelper.ExecuteDataTable(sql, param);
        IList<GoodsInfo> list = new List<GoodsInfo>();
        foreach (DataRow dr in dt.Rows)
        {
            GoodsInfo info = new GoodsInfo();
            info.num_iid = dr["goodsid"].ToString();
            info.Count = int.Parse(dr["gcount"].ToString());
            list.Add(info);
        }
        return list;
    }

    public IList<GoodsInfo> GetTopBuyGoods(string nick, DateTime start, DateTime end, int page, int count)
    {
        int snum = 1;
        if (page != 1)
            snum = (page - 1) * count + 1;
        int endnum = page * count;
        SqlParameter[] param = new[]
        {
            new SqlParameter("@start",start),
            new SqlParameter("@end",end),
            new SqlParameter("@snum",snum),
            new SqlParameter("@enum",endnum),
            new SqlParameter("@nick",nick)
        };

        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_TOP_BUY, param);
        IList<GoodsInfo> list = new List<GoodsInfo>();
        foreach (DataRow dr in dt.Rows)
        {
            GoodsInfo info = new GoodsInfo();
            info.num_iid = dr["num_iid"].ToString();
            info.Count = int.Parse(dr["bcount"].ToString());
            list.Add(info);
        }
        return list;
    }

    public IList<GoodsInfo> GetTopSeeBuyList(string nick, DateTime start, DateTime end, string pids)
    {
        string sql = SQL_SELECT_TOP_SEE_BUY.Replace("@pids", pids);

        SqlParameter[] param = new[]
        {
            new SqlParameter("@start",start),
            new SqlParameter("@end",end),
            new SqlParameter("@nick",nick)
        };

        DataTable dt = DBHelper.ExecuteDataTable(sql, param);
        IList<GoodsInfo> list = new List<GoodsInfo>();
        foreach (DataRow dr in dt.Rows)
        {
            GoodsInfo info = new GoodsInfo();
            info.num_iid = dr["num_iid"].ToString();
            info.Count = int.Parse(dr["bcount"].ToString());
            list.Add(info);
        }
        return list;
    }

    public int GetTopGoodsBuyCount(string nick, DateTime start, DateTime end)
    {
        SqlParameter[] param = new[]
        {
            new SqlParameter("@start",start),
            new SqlParameter("@end",end),
            new SqlParameter("@nick",nick)
        };
        return DBHelper.ExecuteScalar(SQL_SELECT_TOP_BUY_COUNT, param);
    }

    public int UpdatePrice(GoodsInfo info)
    {
        SqlParameter[] param = new[]
        {
            new SqlParameter("@GoodsId",info.num_iid),
            new SqlParameter("@NickNo",info.nick),
            new SqlParameter("@PurchasePrice",info.PurchasePrice)
        };

        return DBHelper.ExecuteNonQuery(SQL_UPDATE_PRICE, param);
    }

    public int GetAllGoodsCount(string nick, string goodsName)
    {
        SqlParameter param = new SqlParameter("@nick", nick);
        string sql = SQL_SELECT_GOODS_PRICE_COUNT;
        if (!string.IsNullOrEmpty(goodsName))
            sql += " and CHARINDEX('"+goodsName+"',GoodsName)>0";

        return DBHelper.ExecuteScalar(sql, param);
    }

    public IList<GoodsInfo> GetAllGoods(string nick,string goodsName, int page, int count)
    {
        int snum = 1;
        if (page != 1)
            snum = (page - 1) * count + 1;
        int endnum = page * count;
        SqlParameter[] param = new[]
        {
            new SqlParameter("@nick",nick),
            new SqlParameter("@snum",snum),
            new SqlParameter("@enum",endnum)
        };

        string sql = SQL_SELECT_GOODS_PRICE;

        if (string.IsNullOrEmpty(goodsName))
            sql = sql.Replace("and CHARINDEX(@goodsName,GoodsName)>0", "");
        else
        {
            sql = sql.Replace("@goodsName", "'" + goodsName + "'");
        }

        DataTable dt = DBHelper.ExecuteDataTable(sql, param);
        IList<GoodsInfo> list = new List<GoodsInfo>();
        foreach (DataRow dr in dt.Rows)
        {
            GoodsInfo info = new GoodsInfo();
            info.num_iid = dr["GoodsId"].ToString();
            info.title = dr["GoodsName"].ToString();
            info.pic_url = dr["Pic_Url"].ToString();
            info.price = decimal.Parse(dr["GoodsPrice"].ToString());
            info.PurchasePrice = dr["PurchasePrice"] == DBNull.Value ? 0 : decimal.Parse(dr["PurchasePrice"].ToString());
            list.Add(info);
        }
        return list;
    }
}
