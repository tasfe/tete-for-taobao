using System;
using System.Data;
using Model;
using System.Collections.Generic;
using System.Data.SqlClient;

/// <summary>
/// 退款订单存取
/// </summary>
public class RefundService
{
    const string SQL_SELECT = "SELECT tid,total_fee,payment,buyer_nick,num,created,modified FROM GoodsOrderRefunds WHERE seller_nick=@nick AND modified BETWEEN @start AND @end";

    const string SQL_INSERT = "INSERT GoodsOrderRefunds(tid,seller_nick,total_fee,payment,buyer_nick,num,created,modified,OrderTime) VALUES(@tid,@seller_nick,@total_fee,@payment,@buyer_nick,@num,@created,@modified,@OrderTime)";

    const string SQL_SELECT_SUM = "select COUNT(*) as ocount,SUM(total_fee-payment) as opay from GoodsOrderRefunds where seller_nick=@nick and OrderTime between @start and @end";

    public List<RefundInfo> GetAllRefund(string nick, DateTime start, DateTime end)
    {
        List<RefundInfo> list = new List<RefundInfo>();
        SqlParameter[] param = new[]
        {
            new SqlParameter("@nick",nick),
            new SqlParameter("@start",start),
            new SqlParameter("@end",end)
        };

        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT, param);
        foreach (DataRow dr in dt.Rows)
        {
            RefundInfo info = new RefundInfo();
            info.tid = dr["tid"].ToString();
            info.total_fee = decimal.Parse(dr["total_fee"].ToString());
            info.payment = decimal.Parse(dr["payment"].ToString());
            info.modified = dr["modified"] == DBNull.Value ? DateTime.MinValue : DateTime.Parse(dr["modified"].ToString());

            info.buyer_nick = dr["buyer_nick"].ToString();
            info.created = dr["created"] == DBNull.Value ? DateTime.MinValue : DateTime.Parse(dr["created"].ToString());
            info.num = int.Parse(dr["num"].ToString());

            list.Add(info);
        }

        return list;
    }

    public RefundInfo GetRefundTotal(string nick, DateTime start, DateTime end)
    {
        RefundInfo info = null;
        SqlParameter[] param = new[]
            {
                new SqlParameter("@nick",nick),
                new SqlParameter("@start",start),
                new SqlParameter("@end",end)
            };

        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_SUM, param);
        foreach (DataRow dr in dt.Rows)
        {
            info = new RefundInfo();
            info.total_fee = dr["opay"] == DBNull.Value ? 0 : decimal.Parse(dr["opay"].ToString());
            info.num = dr["ocount"] == DBNull.Value ? 0 : int.Parse(dr["ocount"].ToString());
        }

        return info;
    }

    public int AddRefund(RefundInfo info)
    {
        SqlParameter[] param = GetParameter(info);
        return DBHelper.ExecuteNonQuery(SQL_INSERT, param);
    }

    private static SqlParameter[] GetParameter(RefundInfo info)
    {
        return new SqlParameter[]
        {
            new SqlParameter("@tid",info.tid),
            new SqlParameter("@seller_nick",info.seller_nick),
            new SqlParameter("@total_fee",info.total_fee),
            new SqlParameter("@payment",info.payment),
            new SqlParameter("@buyer_nick",info.buyer_nick),
            new SqlParameter("@num",info.num),
            new SqlParameter("@modified",info.modified),
            new SqlParameter("@created",info.created),
            new SqlParameter("@OrderTime",info.OrderTime)
        };
    }
}
