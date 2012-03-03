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
/// Summary description for TaoBaoGoodsServivr
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
}
