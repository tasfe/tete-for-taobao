using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBHelp;

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
	        [TalkContent] [varchar](500) NULL,
         CONSTRAINT @pk PRIMARY KEY CLUSTERED 
        (
	        [ContentId] ASC
        )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
        ) ON [PRIMARY]
         CREATE INDEX index_@tableName_FromId ON @tableName([FromId])
         CREATE INDEX index_@tableName_ToId ON @tableName([ToId])
         CREATE INDEX index_@tableName_TalkTime ON @tableName([TalkTime])
        ";

        /// <summary>
        /// 用户订购获取代码时生成一张表
        /// </summary>
        /// <param name="nickNo"></param>
        public void CreateTable(string nickNo)
        {
            string sql = SQL_SELECT_TABLE_EXISTS.Replace("@tableName", DBHelper.GetRealTable("TalkContent", nickNo));
            int drow = DBHelper.ExecuteScalar(sql);
            if (drow == 0)
            {
                sql = SQL_CREATE_TABLE.Replace("@tableName", DBHelper.GetRealTable("TalkContent", nickNo)).Replace("@pk", "PK_TopVisitInfo_" + nickNo);

                DBHelper.ExecuteNonQuery(sql);
            }
        }

        public static bool CheckTable(string nickNo)
        {
            string sql = SQL_SELECT_TABLE_EXISTS.Replace("@tableName", DBHelper.GetRealTable("TalkContent", nickNo));
            int drow = DBHelper.ExecuteScalar(sql);
            return drow == 0 ? false : true;
        }
    }
}
