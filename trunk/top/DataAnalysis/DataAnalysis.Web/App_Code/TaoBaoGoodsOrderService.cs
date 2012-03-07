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

        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_BACK_TOTAL, param);
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
}

