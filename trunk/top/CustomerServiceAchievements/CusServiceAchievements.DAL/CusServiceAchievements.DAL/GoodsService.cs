using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaoBaoAPIHelper;
using System.Data.SqlClient;
using DBHelp;
using System.Data;

namespace CusServiceAchievements.DAL
{
    public class GoodsService
    {
        const string SQL_INSERT = "INSERT TopTaoBaoGoodsInfo(GoodsId,CId,GoodsName,Pic_Url,GoodsPrice,NickNo) VALUES(@GoodsId,@CId,@GoodsName,@Pic_Url,@GoodsPrice,@NickNo)";

        const string SQL_SELECT_EXISTSTABLE = "SELECT COUNT(*) FROM sysobjects WHERE id=object_id('@tableName') AND OBJECTPROPERTY(id,'IsUserTable')=1";

        const string SQL_DROP_TABLE = "DROP TABLE @tableName";

        const string SQL_CREATE_TABLE = @"CREATE TABLE  [TopTaoBaoGoodsInfo](
                                                        [GoodsId] [varchar](50) NOT NULL,
                                                        [CId] [varchar](50) NULL,
                                                        [GoodsName] [varchar](150) NULL,
                                                        [Pic_Url] [varchar](150) NULL,
                                                        [GoodsPrice] [decimal](9, 2) NULL,
                                                        [NickNo] [varchar](50) NULL,
                                                     CONSTRAINT [PK_TopTaoBaoGoodsInfo] PRIMARY KEY CLUSTERED 
                                                    (
                                                        [GoodsId] ASC
                                                    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
                                                    ) ON [PRIMARY]
                                    CREATE INDEX index_TopTaoBaoGoodsInfo_NickNo ON TopTaoBaoGoodsInfo([NickNo])";

        const string SQL_SELECT_GOODS_BYNICK = "SELECT GoodsId FROM TopTaoBaoGoodsInfo WHERE NickNo=@nickno";

        public IList<string> GetGoodsIds(string nickNo)
        {
            IList<string> list = new List<string>();
            SqlParameter param = new SqlParameter("@nickno", nickNo);
            DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_GOODS_BYNICK, param);

            foreach (DataRow dr in dt.Rows)
            {
                list.Add(dr[0].ToString());
            }
            return list;
        }

        public int InsertGoods(GoodsInfo info)
        {
            SqlParameter[] param = new[]
            {
                new SqlParameter("@GoodsId",info.num_iid),
                new SqlParameter("@CId",info.cid),
                new SqlParameter("@GoodsName",info.title),
                new SqlParameter("@Pic_Url",info.pic_url),
                new SqlParameter("@GoodsPrice",info.price),
                new SqlParameter("@NickNo",info.nick)
            };

            return DBHelper.ExecuteNonQuery(SQL_INSERT, param);
        }

        public void DropTable(string tableName)
        {
            if (CheckTable(tableName))
            {
                string sql = SQL_DROP_TABLE.Replace("@tableName", tableName);
                DBHelper.ExecuteNonQuery(sql);
            }

            DBHelper.ExecuteNonQuery(SQL_CREATE_TABLE);
        }

        public static bool CheckTable(string tableName)
        {
            string sql = SQL_SELECT_EXISTSTABLE.Replace("@tableName", tableName);
            int drow = DBHelper.ExecuteScalar(sql);
            return drow == 0 ? false : true;
        }

    }
}
