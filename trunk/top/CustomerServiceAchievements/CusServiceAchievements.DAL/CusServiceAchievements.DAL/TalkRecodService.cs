using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBHelp;
using TaoBaoAPIHelper;
using System.Data.SqlClient;
using Model;
using System.Data;
using Enum;

namespace CusServiceAchievements.DAL
{
    public class TalkRecodService
    {
        //查询该表是否存在
        const string SQL_SELECT_TABLE_EXISTS = "SELECT COUNT(*) FROM sysobjects WHERE id=object_id('@tableName') AND OBJECTPROPERTY(id,'IsUserTable')=1";

        //建表
        const string SQL_CREATE_TABLE = @"CREATE TABLE @tableName(
	        [ContentId] [uniqueidentifier] NOT NULL,
	        [FromId] [varchar](50) NULL,
	        [ToId] [varchar](50) NULL,
	        [TalkTime] [datetime] NULL,
	        [TalkContent] [nvarchar](1000) NULL,
            [direction] [int],
         CONSTRAINT @pk PRIMARY KEY CLUSTERED 
        (
	        [ContentId] ASC
        )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
        ) ON [PRIMARY]
         CREATE INDEX index_@tableName_FromId ON @tableName([FromId])
         CREATE INDEX index_@tableName_ToId ON @tableName([ToId])
         CREATE INDEX index_@tableName_TalkTime ON @tableName([TalkTime])
        ";

        const string SQL_INSERT = "INSERT @tableName(ContentId,FromId,ToId,TalkTime,TalkContent,direction) VALUES(@ContentId,@FromId,@ToId,@TalkTime,@TalkContent,@direction)";

        const string SQL_SELECT_CUSTOMEN_LIST_SERVICE = @"select COUNT(*) as talkcount,fromid,toid,
                  MIN(talktime) as start,MAX(talktime) as endt 
                  from 
                  (
                  select * from @tableName
                  where talktime between @start and @end and toid<>@nick and 
                  SUBSTRING(ToId,0,CHARINDEX(':',ToId))<>@nick and direction=1
                  ) a 
                  group by fromid,toid";

           const string SQL_SELECT_RECEIVE_COUNT = @"select COUNT(*) cuscount,SUM(talkcount) tccount,fromid 
                      from(
                      select COUNT(*) as talkcount,fromid,toid,MIN(talktime) as start,MAX(talktime) as endt                           from 
                      (
                      select * from @tableName
                      where talktime between @start and @end and direction=0 and
                      toid<>@nick and SUBSTRING(ToId,0,CHARINDEX(':',ToId))<>@nick
                      ) a 
                      group by fromid,toid  
                      ) b group by fromid ";

           const string SQL_SELECT_MAXTIME = "select max(TalkTime) from @tableName";

           const string SQL_SELECT_UNTALKCUSTOMER = @"select count(distinct ToId) as untalkcount,FromId from 
            (
                select * from @tableName      
                where talktime between @start and @end and direction=1 and 
                toid<>@nick and SUBSTRING(ToId,0,CHARINDEX(':',ToId))<>@nick  
                and ToId not in ( select ToId from @tableName      
                where talktime between @start and @end and direction=0)
             )a group by FromId ";

           const string SQL_SELECT_ALL_CONTENT_BYDATE = "SELECT FromId,ToId,TalkTime,direction FROM @tableName WHERE TalkTime BETWEEN @start AND @end";

        /// <summary>
        /// 用户订购获取代码时生成一张表
        /// </summary>
        /// <param name="nickNo">加密后的</param>
        public void CreateTable(string nickNo)
        {
            string sql = SQL_SELECT_TABLE_EXISTS.Replace("@tableName", DBHelper.GetRealTable("TalkContent", nickNo));
            int drow = ServiceDBHelper.GetScaleSql(sql);
            if (drow == 0)
            {
                sql = SQL_CREATE_TABLE.Replace("@tableName", DBHelper.GetRealTable("TalkContent", nickNo)).Replace("@pk", "PK_TalkContent_" + nickNo);

                ServiceDBHelper.ExcuteSql(sql);
            }
        }

        public static bool CheckTable(string tableName)
        {
            string sql = SQL_SELECT_TABLE_EXISTS.Replace("@tableName", DBHelper.GetRealTable("TalkContent", tableName));
            int drow = ServiceDBHelper.GetScaleSql(sql);
            return drow == 0 ? false : true;
        }

        public int InsertContent(TalkContent tc,string nick)
        {
            SqlParameter[] param = CreateParameter(tc);
            string sql = SQL_INSERT.Replace("@tableName", DBHelper.GetRealTable("TalkContent", DataHelper.Encrypt(nick)));
            return ServiceDBHelper.ExcuteSql(sql, param);
        }

        private static SqlParameter[] CreateParameter(TalkContent tc)
        {
            SqlParameter[] param = new[]
            {
                new SqlParameter("@TalkTime",tc.time),
                new SqlParameter("@ContentId",Guid.NewGuid()),
                new SqlParameter("@FromId",tc.FromNick),
                new SqlParameter("@ToId",tc.ToNick),
                new SqlParameter("@TalkContent",tc.content),
                new SqlParameter("@direction",tc.direction)
            };

            return param;
        }

        public IList<CustomerInfo> GetCustomerList(DateTime start, DateTime end, string nick)
        {
            string sql = SQL_SELECT_CUSTOMEN_LIST_SERVICE.Replace("@tableName", DBHelper.GetRealTable("TalkContent", DataHelper.Encrypt(nick)));
            IList<CustomerInfo> list = new List<CustomerInfo>();

            SqlParameter[] param = new[]
                {
                    new SqlParameter("@start",start),
                    new SqlParameter("@end",end),
                    new SqlParameter("@nick",nick)
                };

            using (SqlDataReader dr = ServiceDBHelper.CreateReader(sql, param))
            {
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        CustomerInfo info = new CustomerInfo();
                        info.StartTime = DateTime.Parse(dr["start"].ToString());
                        info.EndTime = DateTime.Parse(dr["endt"].ToString());
                        info.FromNick = dr["fromid"].ToString();
                        info.CustomerNick = dr["toid"].ToString();
                        info.TalkCount = dr["talkcount"] == DBNull.Value ? 0 : int.Parse(dr["talkcount"].ToString());

                        IList<CustomerInfo> owerList = list.Where(o => o.CustomerNick == info.CustomerNick).ToList();
                        if (owerList.Count > 0)
                        {
                            //按聊天次数最多的算
                            if (owerList[0].TalkCount > info.TalkCount)
                                continue;
                            list.Remove(owerList[0]);
                        }

                        list.Add(info);
                    }
                }
            }

            return list;
        }

        public List<CustomerInfo> GetReceiveList(string nick,DateTime start,DateTime end)
        {
            string sql = SQL_SELECT_RECEIVE_COUNT.Replace("@tableName", DBHelper.GetRealTable("TalkContent", DataHelper.Encrypt(nick)));
            List<CustomerInfo> list = new List<CustomerInfo>();

            SqlParameter[] param = new[]
                {
                    new SqlParameter("@start",start),
                    new SqlParameter("@end",end),
                    new SqlParameter("@nick",nick)
                };

            using (SqlDataReader dr = ServiceDBHelper.CreateReader(sql, param))
            {
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        CustomerInfo info = new CustomerInfo();
                        info.FromNick = dr["fromid"].ToString();
                        info.CustomerNick = dr["cuscount"].ToString(); //用客服ID装下客户总数
                        info.TalkCount = int.Parse(dr["tccount"].ToString());

                        list.Add(info);
                    }
                }
            }

            return list;
        }

        public DateTime GetMaxTime(string nick)
        {
            string sql = SQL_SELECT_MAXTIME.Replace("@tableName", DBHelper.GetRealTable("TalkContent", DataHelper.Encrypt(nick)));

            using (SqlDataReader dr = ServiceDBHelper.CreateReader(sql))
            {
                if (dr != null)
                {
                    if (dr.Read())
                    {
                        return dr[0] == DBNull.Value ? DateTime.Now.AddDays(-1) : dr.GetDateTime(0);
                    }
                }
            }
            return DateTime.Parse(DateTime.Now.AddDays(-1).ToShortDateString());
        }

        /// <summary>
        /// 获取客服及客服未回复客户的数量
        /// </summary>
        /// <param name="nick"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<TopKefuTotalInfo> GetUnTalkCustomerList(string nick, DateTime start, DateTime end)
        {
            SqlParameter[] param = new[]
                {
                    new SqlParameter("@start",start),
                    new SqlParameter("@end",end),
                    new SqlParameter("@nick",nick)
                };
            List<TopKefuTotalInfo> list = new List<TopKefuTotalInfo>();

            string sql = SQL_SELECT_UNTALKCUSTOMER.Replace("@tableName", DBHelper.GetRealTable("TalkContent", DataHelper.Encrypt(nick)));

            using (SqlDataReader dr = ServiceDBHelper.CreateReader(sql, param))
            {
                if (dr != null)
                {
                    if (dr.Read())
                    {
                        TopKefuTotalInfo info = new TopKefuTotalInfo();
                        info.Nick = dr["FromId"].ToString();
                        info.UnTalkCustomerCount = dr["untalkcount"] == DBNull.Value ? 0 : int.Parse(dr["untalkcount"].ToString());
                        list.Add(info);
                    }
                }
            }
            return list;
        }

        public List<TalkContent> GetAllContent(DateTime start, DateTime end, string nick)
        {
            List<TalkContent> list = new List<TalkContent>();

            string sql = SQL_SELECT_ALL_CONTENT_BYDATE.Replace("@tableName", DBHelper.GetRealTable("TalkContent", DataHelper.Encrypt(nick)));
            SqlParameter[] param = new[]
                {
                    new SqlParameter("@start",start),
                    new SqlParameter("@end",end)
                };
            using (SqlDataReader dr = ServiceDBHelper.CreateReader(sql, param))
            {
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        TalkContent info = new TalkContent();
                        info.FromNick = dr["FromId"].ToString();
                        info.direction = dr["direction"] == DBNull.Value ? 0 : int.Parse(dr["direction"].ToString());
                        info.ToNick = dr["ToId"].ToString();
                        info.time = dr["TalkTime"].ToString();

                        list.Add(info);
                    }
                }
            }

            return list;
        }

        #region 网站用

        const string SQL_SELECT_CUSTOMER = @"select * from(
                  select COUNT(*) as talkcount,fromid,toid,MIN(talktime) as start,MAX(talktime) as endt
                  ,ROW_NUMBER() OVER(ORDER BY toid) as RowNum
                   from 
                  (
                  select * from @tableName
                  where talktime between @start and @end
                  ) a 
                  group by fromid,toid 
                  ) b where rownum between @snum and @enum";

        const string SQL_SELECT_CUSTOMER_COUNT = @"select COUNT(*) from
                  (
                   select COUNT(*) as talkcount,fromid,toid,MIN(talktime) as start,MAX(talktime) as endt
                  ,ROW_NUMBER() OVER(ORDER BY toid) as RowNum
                   from 
                  (
                  select * from @tableName
                  where talktime between @start and @end
                  ) a 
                  group by fromid,toid) b";

        const string SQL_SELECT_R_CUSTOMER = @"select COUNT(*),toid,DatePart(hh,TalkTime) as mhour from
                      (
                      select * from @tableName 
                      where TalkTime between @start and @end and 
                      toid<>@nick and SUBSTRING(ToId,0,CHARINDEX(':',ToId))<>@nick
                      ) a group by ToId,DatePart(hh,TalkTime)";

        const string SQL_SELECT_TALKCONTENT_USER = "select distinct FromId from @tableName where TalkTime between @start and @end";

        const string SQL_SELECT_TALKCONTENT_CUSTOMER = "select distinct ToId from @tableName where TalkTime between @start and @end";

        const string SQL_SELECT_TALKCONTENT = "select TalkContent,TalkTime,direction from @tableName  where FromId=@fnick and ToId=@tnick and TalkTime between @start and @end order by TalkTime";

        public IList<CustomerInfo> GetCustomerList(DateTime start, DateTime end, string nick, int page, int count)
        {
            string sql = SQL_SELECT_CUSTOMER.Replace("@tableName", DBHelper.GetRealTable("TalkContent", DataHelper.Encrypt(nick)));
            IList<CustomerInfo> list = new List<CustomerInfo>();

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

            foreach (DataRow dr in dt.Rows)
            {
                CustomerInfo info = new CustomerInfo();
                info.StartTime = DateTime.Parse(dr["start"].ToString());
                info.EndTime = DateTime.Parse(dr["endt"].ToString());
                info.FromNick = dr["fromid"].ToString();
                info.CustomerNick = dr["toid"].ToString();
                info.TalkCount = int.Parse(dr["talkcount"].ToString());

                IList<CustomerInfo> owerList = list.Where(o=>o.CustomerNick ==info.CustomerNick).ToList();
                if (owerList.Count > 0)
                {
                    //按聊天次数最多的算
                    if (owerList[0].TalkCount > info.TalkCount)
                        continue;
                    list.Remove(owerList[0]);
                }

                list.Add(info);
            }

            return list;
        }

        public int GetCustomerListCount(DateTime start, DateTime end, string nick)
        {
            string sql = SQL_SELECT_CUSTOMER_COUNT.Replace("@tableName", DBHelper.GetRealTable("TalkContent", DataHelper.Encrypt(nick)));

            SqlParameter[] param = new[]
                {
                    new SqlParameter("@start",start),
                    new SqlParameter("@end",end)
                };

            return DBHelper.ExecuteScalar(sql, param);
        }

        public IList<TalkContent> GetTalkTotalHour(string nick, DateTime start, DateTime end)
        {
            string sql = SQL_SELECT_R_CUSTOMER.Replace("@tableName", DBHelper.GetRealTable("TalkContent", DataHelper.Encrypt(nick)));
            SqlParameter[] param = new[]
                {
                    new SqlParameter("@start",start.ToShortDateString()),
                    new SqlParameter("@end",end.ToShortDateString()),
                    new SqlParameter("@nick",nick)
                };
            IList<TalkContent> list = new List<TalkContent>();
            DataTable dt = DBHelper.ExecuteDataTable(sql, param);

            foreach (DataRow dr in dt.Rows)
            {
                TalkContent info = new TalkContent();
                info.ToNick = dr["toid"].ToString();
                info.time = dr["mhour"].ToString();
                list.Add(info);
            }

            return list;
        }

        public List<TalkContent> GetTalkUser(DateTime start, DateTime end, string nick, TalkObjType otype)
        {
            string sql = SQL_SELECT_TALKCONTENT_USER.Replace("@tableName", DBHelper.GetRealTable("TalkContent", DataHelper.Encrypt(nick)));
            List<TalkContent> list = new List<TalkContent>();
            SqlParameter[] param = new[]
                {
                    new SqlParameter("@start",start.ToShortDateString()),
                    new SqlParameter("@end",end.ToShortDateString())
                };
            DataTable dt = DBHelper.ExecuteDataTable(sql, param);

            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new TalkContent { FromNick = dr["FromId"].ToString() });
            }

            return list;
        }

        public List<TalkContent> GetTalkCustomer(DateTime start, DateTime end, string fnick, TalkObjType otype)
        {
            string nick = fnick.IndexOf(":") >= 0 ? fnick.Substring(0, fnick.IndexOf(':')) : fnick;
            string sql = SQL_SELECT_TALKCONTENT_CUSTOMER.Replace("@tableName", DBHelper.GetRealTable("TalkContent", DataHelper.Encrypt(nick)));

            if (otype == TalkObjType.Out)
            {
                sql += " AND FromId='" + fnick + "'";
            }

            List<TalkContent> list = new List<TalkContent>();
            SqlParameter[] param = new[]
                {
                    new SqlParameter("@start",start.ToShortDateString()),
                    new SqlParameter("@end",end.ToShortDateString())
                };
            DataTable dt = DBHelper.ExecuteDataTable(sql, param);

            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new TalkContent { ToNick = dr["ToId"].ToString() });
            }

            return list;
        }

        public List<TalkContent> GetAllContentByFromId(string fnick, string tnick, DateTime start, DateTime end)
        {
            string nick = fnick.IndexOf(":") >= 0 ? fnick.Substring(0, fnick.IndexOf(':')) : fnick;
            string sql = SQL_SELECT_TALKCONTENT.Replace("@tableName", DBHelper.GetRealTable("TalkContent", DataHelper.Encrypt(nick)));
            
            List<TalkContent> list = new List<TalkContent>();
            SqlParameter[] param = new[]
                {
                    new SqlParameter("@start",start.ToShortDateString()),
                    new SqlParameter("@end",end.ToShortDateString()),
                    new SqlParameter("@fnick",fnick),
                    new SqlParameter("@tnick",tnick)
                };
            DataTable dt = DBHelper.ExecuteDataTable(sql, param);

            foreach (DataRow dr in dt.Rows)
            {
                TalkContent info = new TalkContent();
                info.content = dr["TalkContent"].ToString();
                info.time = dr["TalkTime"].ToString();
                info.direction = int.Parse(dr["direction"].ToString());

                info.FromNick = fnick;
                info.ToNick = tnick;

                list.Add(info);
            }

            return list;
        }

        #endregion
    }
}
