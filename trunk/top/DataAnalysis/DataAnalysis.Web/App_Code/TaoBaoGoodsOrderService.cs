using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;

/// <summary>
/// Summary description for TaoBaoGoodsOrderService
/// </summary>
public class TaoBaoGoodsOrderService
{

    const string SQL_SELECT_BACK_TOTAL = @"select *, (select count(*) from TopTaoBaoGoodsOrderInfo
  c where CONVERT(varchar(12),c.created,112)=b.pdate and UsePromotion=1 and seller_nick=@nick) 
  as pcount from 
 (  
 select COUNT(*) allcount,CONVERT(varchar(12),created,112) pdate
 from
 (
 select * from TopTaoBaoGoodsOrderInfo where created between 
@start and @end and seller_nick=@nick
  ) a
 group by CONVERT(varchar(12),created,112)
) b";

    const string SQL_ORDER_INSERT = "INSERT TopTaoBaoGoodsOrderInfo(tid,seller_nick,total_fee,post_fee,payment,cod_fee,commission_fee,created,pay_time,end_time,receiver_state,receiver_city,UsePromotion,pingjiacreated,result,pingjiacontent,buy_nick) VALUES(@tid,@seller_nick,@total_fee,@post_fee,@payment,@cod_fee,@commission_fee,@created,@pay_time,@end_time,@receiver_state,@receiver_city,@UsePromotion,@pingjiacreated,@result,@pingjiacontent,@buy_nick)";

    const string SQL_ORDERGOODS_INSERT = "INSERT TopTaoBaoOrderGoodsList(id,tid,status,num_iid,num) VALUES(@id,@tid,@status,@num_iid,@num)";

    const string SQL_SELECT_ORDERPINGJIATOTAL = @"select COUNT(*) allcount,CONVERT(varchar(12),created,112) pdate,result from
 (
 select * from TopTaoBaoGoodsOrderInfo where created between 
@start and @end and seller_nick=@nick
  ) a
 group by CONVERT(varchar(12),created,112),result";

    const string SQL_SELECT_SELLCITY = @"select SUM(payment) as paytotal,COUNT(*) as ototal,receiver_state,receiver_city                from
            (
            select * from TopTaoBaoGoodsOrderInfo where seller_nick=@nick 
            and created between @start and @end
            ) a 
            group by receiver_state,receiver_city";

    const string SQL_SELECT_ORDERTOTAL_BYHOUR = @"SELECT Count(*) AS OrderCount,DatePart(hh,created) AS chour FROM 
                                     (
                                     SELECT created FROM 
                                     TopTaoBaoGoodsOrderInfo WHERE 
                                     seller_nick=@nick and
                                     created BETWEEN @start AND @end 
                                     ) a
                                     GROUP BY DatePart(hh,created)";

    const string SQL_SELECT_ORDER = "SELECT tid FROM TopTaoBaoGoodsOrderInfo WHERE seller_nick=@seller_nick AND created BETWEEN @start AND @end";

    const string SQL_SELECT_ORDER_LIST = "select * from(select tid,buy_nick,payment,post_fee,total_fee,receiver_state,receiver_city,ROW_NUMBER() OVER(ORDER BY created DESC) as RowNum  from TopTaoBaoGoodsOrderInfo where seller_nick=@nick and created between @start and @end) a where RowNum between @snum and @enum";

    const string SQL_SELECT_ORDER_LIST_COUNT = "select count(*) from TopTaoBaoGoodsOrderInfo where seller_nick=@nick and created between @start and @end";

    const string SQL_SELECT_GOODSORDER = "SELECT created,pay_time FROM TopTaoBaoGoodsOrderInfo WHERE tid=@tid";

    public IList<BackTotalInfo> GetAllBackTotalList(DateTime start, DateTime end, string nick)
    {
        SqlParameter[] param = new[]
            {
                new SqlParameter("@start", start),
                new SqlParameter("@end",end),
                new SqlParameter("@nick",nick)
            };

        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_BACK_TOTAL, param);
        IList<BackTotalInfo> list = new List<BackTotalInfo>();
        foreach (DataRow dr in dt.Rows)
        {
            BackTotalInfo info = new BackTotalInfo();
            info.AllCount = int.Parse(dr["allcount"].ToString());
            info.PCount = int.Parse(dr["pcount"].ToString());
            info.PDate = dr["pdate"].ToString();
            list.Add(info);
        }
        IList<BackTotalInfo> rlist = list.OrderBy(o => o.PDate).ToList();

        return rlist;
    }

    public int InsertTaoBaoGoodsOrder(GoodsOrderInfo info)
    {
        SqlParameter[] param = new[]
              {
                    new SqlParameter("@tid",info.tid),
                    new SqlParameter("@seller_nick",info.seller_nick),
                    new SqlParameter("@total_fee",info.total_fee),
                    new SqlParameter("@post_fee",info.post_fee),
                    new SqlParameter("@payment",info.payment),
                    new SqlParameter("@cod_fee",info.cod_fee),
                    new SqlParameter("@commission_fee",info.commission_fee),
                    new SqlParameter("@created",info.created),
                    new SqlParameter("@pay_time",info.pay_time==DateTime.MinValue?DateTime.Parse("1990-1-1"):info.pay_time),
                    new SqlParameter("@end_time",info.end_time==DateTime.MinValue?DateTime.Parse("1990-1-1"):info.end_time),
                    new SqlParameter("@receiver_state",info.receiver_state),
                    new SqlParameter("@receiver_city",info.receiver_city),
                    new SqlParameter("@UsePromotion",info.UsePromotion),
                    new SqlParameter("@pingjiacreated",info.PingInfo.created),
                    new SqlParameter("@result",info.PingInfo.result),
                    new SqlParameter("@pingjiacontent",info.PingInfo.content),
                    new SqlParameter("@buy_nick",info.buyer_nick)
              };
        return DBHelper.ExecuteNonQuery(SQL_ORDER_INSERT, param);
    }

    public void InsertChildOrderInfo(IList<ChildOrderInfo> list, string tid)
    {
        foreach (ChildOrderInfo info in list)
        {
            SqlParameter[] param = new[]
              {
                    new SqlParameter("@id",Guid.NewGuid()),
                    new SqlParameter("@tid",tid),
                    new SqlParameter("@status",info.status),
                    new SqlParameter("@num_iid",info.num_iid),
                    new SqlParameter("@num",info.num)
              };
            DBHelper.ExecuteNonQuery(SQL_ORDERGOODS_INSERT, param);
        }
    }

    public IList<PingJiaInfo> GetPingjiaTotal(DateTime start, DateTime end, string nick)
    {
        SqlParameter[] param = new[]
            {
                new SqlParameter("@start", start),
                new SqlParameter("@end",end),
                new SqlParameter("@nick",nick)
            };

        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_ORDERPINGJIATOTAL, param);
        IList<PingJiaInfo> list = new List<PingJiaInfo>();
        foreach (DataRow dr in dt.Rows)
        {
            PingJiaInfo info = new PingJiaInfo();
            info.pcount = int.Parse(dr["allcount"].ToString());
            info.pdate = dr["pdate"].ToString();
            info.result = dr["result"].ToString();
            list.Add(info);
        }
        IList<PingJiaInfo> rlist = list.OrderBy(o => o.created).ToList();

        return rlist;
    }

    public IList<GoodsOrderInfo> GetSellCityTop(DateTime start, DateTime end, string nick)
    {
        SqlParameter[] param = new[]
            {
                new SqlParameter("@start", start),
                new SqlParameter("@end",end),
                new SqlParameter("@nick",nick)
            };
        IList<GoodsOrderInfo> list = new List<GoodsOrderInfo>();
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_SELLCITY, param);
        foreach (DataRow dr in dt.Rows)
        {
            GoodsOrderInfo info = new GoodsOrderInfo();
            info.payment = decimal.Parse(dr["paytotal"].ToString());
            info.OrderTotal = int.Parse(dr["ototal"].ToString());
            info.receiver_state = dr["receiver_state"].ToString();
            info.receiver_city = dr["receiver_city"].ToString();
            list.Add(info);
        }
        IList<GoodsOrderInfo> rlist = list.OrderByDescending(o => o.OrderTotal).ToList();
        return rlist;
    }

    public List<GoodsOrderInfo> GetHourOrderTotal(DateTime start, DateTime end, string nick)
    {
        SqlParameter[] param = new[]
            {
                new SqlParameter("@start", start),
                new SqlParameter("@end",end),
                new SqlParameter("@nick",nick)
            };
        List<GoodsOrderInfo> list = new List<GoodsOrderInfo>();
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_ORDERTOTAL_BYHOUR, param);
        foreach (DataRow dr in dt.Rows)
        {
            GoodsOrderInfo info = new GoodsOrderInfo();
            info.payment = decimal.Parse(dr["OrderCount"].ToString());
            info.OrderTotal = int.Parse(dr["chour"].ToString());
            list.Add(info);
        }

        return list;
    }

    public GoodsOrderInfo GetGoodsOrderInfo(string tid)
    {
        SqlParameter param = new SqlParameter("@tid", tid);
        GoodsOrderInfo info = null;

        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_GOODSORDER, param);
        foreach (DataRow dr in dt.Rows)
        {
            info = new GoodsOrderInfo();
            info.created = DateTime.Parse(dr["created"].ToString());
        }
        return info;
    }

    /// <summary>
    /// 获取指定时间数据库中的所有订单
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="nick"></param>
    /// <returns></returns>
    public List<string> GetAllOrderId(DateTime start, DateTime end, string nick)
    {
        SqlParameter[] param = new[]
            {
                new SqlParameter("@start",start),
                new SqlParameter("@end",end),
                new SqlParameter("@seller_nick",nick)
            };
        List<string> list = new List<string>();
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_ORDER, param);
        foreach (DataRow dr in dt.Rows)
        {
            list.Add(dr["tid"].ToString());
        }
        return list;
    }

    public IList<GoodsOrderInfo> GetOrderList(string nick, DateTime start, DateTime end, int page, int count)
    {
        int snum = 1;
        if (page != 1)
            snum = (page - 1) * count + 1;
        int endnum = page * count;

        List<GoodsOrderInfo> list = new List<GoodsOrderInfo>();
        SqlParameter[] param = new[]
        {
            new SqlParameter("@nick",nick),
            new SqlParameter("@start",start),
            new SqlParameter("@end",end),
            new SqlParameter("@snum",snum),
            new SqlParameter("@enum",endnum)
        };

        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_ORDER_LIST, param);

        foreach (DataRow dr in dt.Rows)
        {
            GoodsOrderInfo info = new GoodsOrderInfo();
            info.buyer_nick = dr["buy_nick"].ToString();
            info.payment = decimal.Parse(dr["payment"].ToString());
            info.receiver_city = dr["receiver_city"].ToString();
            info.receiver_state = dr["receiver_state"].ToString();
            info.tid = dr["tid"].ToString();
            info.post_fee = decimal.Parse(dr["post_fee"].ToString());
            info.total_fee = decimal.Parse(dr["total_fee"].ToString());

            list.Add(info);
        }

        return list;
    }

    public int GetOrderListCount(string nick, DateTime start, DateTime end)
    {
        SqlParameter[] param = new[]
        {
            new SqlParameter("@start",start),
            new SqlParameter("@end",end),
            new SqlParameter("@nick",nick)
        };
        return DBHelper.ExecuteScalar(SQL_SELECT_ORDER_LIST_COUNT, param);
    }
}


