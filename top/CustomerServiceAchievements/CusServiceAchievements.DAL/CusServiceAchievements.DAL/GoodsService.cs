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

        const string SQL_SELECT_ALLGOODS_NICK = "SELECT GoodsId,GoodsPrice,GoodsName,CId,Pic_Url FROM TopTaoBaoGoodsInfo WHERE NickNo=@nickno";

        const string SQL_UPDATE = "UPDATE TopTaoBaoGoodsInfo SET GoodsPrice=@GoodsPrice,GoodsName=@GoodsName,CId=@CId,Pic_Url=@Pic_Url WHERE GoodsId=@GoodsId";

        public IList<string> GetGoodsIds(string nickNo)
        {
            IList<string> list = new List<string>();
            SqlParameter param = new SqlParameter("@nickno", nickNo);

            using (SqlDataReader dr = ServiceDBHelper.CreateReader(SQL_SELECT_GOODS_BYNICK, param))
            {
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        list.Add(dr[0].ToString());
                    }
                }
            }
            return list;
        }

        public int InsertGoods(GoodsInfo info, string nick)
        {
            SqlParameter[] param = new[]
            {
                new SqlParameter("@GoodsId",info.num_iid),
                new SqlParameter("@CId",info.cid),
                new SqlParameter("@GoodsName",info.title),
                new SqlParameter("@Pic_Url",string.IsNullOrEmpty(info.pic_url)?"": info.pic_url),
                new SqlParameter("@GoodsPrice",info.price),
                new SqlParameter("@NickNo",nick)
            };

            return ServiceDBHelper.ExcuteSql(SQL_INSERT, param);
        }

        public void DropTable(string tableName)
        {
            if (CheckTable(tableName))
            {
                string sql = SQL_DROP_TABLE.Replace("@tableName", tableName);
                ServiceDBHelper.ExcuteSql(sql);
            }

            ServiceDBHelper.ExcuteSql(SQL_CREATE_TABLE);
        }

        public static bool CheckTable(string tableName)
        {
            string sql = SQL_SELECT_EXISTSTABLE.Replace("@tableName", tableName);
            int drow = ServiceDBHelper.ExcuteSql(sql);
            return drow == 0 ? false : true;
        }

        public int UpdateGoodsInfo(GoodsInfo info)
        {
            SqlParameter[] param = new[]
            {
                new SqlParameter("@GoodsId",info.num_iid),
                new SqlParameter("@CId",info.cid),
                new SqlParameter("@GoodsName",info.title),
                new SqlParameter("@Pic_Url",info.pic_url),
                new SqlParameter("@GoodsPrice",info.price)
            };

            return ServiceDBHelper.ExcuteSql(SQL_UPDATE, param);
        }

        public List<GoodsInfo> GetAllGoods(string nick)
        {
            SqlParameter param = new SqlParameter("@nickno", nick);

            List<GoodsInfo> list = new List<GoodsInfo>();

            using (SqlDataReader dr = ServiceDBHelper.CreateReader(SQL_SELECT_ALLGOODS_NICK, param))
            {
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        GoodsInfo info = new GoodsInfo();
                        info.num_iid = dr["GoodsId"].ToString();
                        info.pic_url = dr["Pic_Url"].ToString();
                        info.cid = dr["CId"].ToString();
                        info.price = decimal.Parse(dr["GoodsPrice"].ToString());
                        info.title = dr["GoodsName"].ToString();

                        list.Add(info);
                    }
                }
            }

            return list;
        }

        #region 网站用

        const string SQL_SELECT_BY_ID = "SELECT Pic_Url,GoodsName FROM TopTaoBaoGoodsInfo WHERE NickNo=@nickno AND GoodsId=@GoodsId";

        public GoodsInfo GetGoodsInfo(string goodsId,string nick)
        {
            SqlParameter[] param = new[]
            {
                new SqlParameter("@GoodsId",goodsId),
                new SqlParameter("@nickno",nick)
            };

            DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_BY_ID, param);
            GoodsInfo info = new GoodsInfo();
            foreach (DataRow dr in dt.Rows)
            {

                info.pic_url = dr["Pic_Url"].ToString();
                info.title = dr["GoodsName"].ToString();
            }

            return info;
        }

        #endregion
    }
}
