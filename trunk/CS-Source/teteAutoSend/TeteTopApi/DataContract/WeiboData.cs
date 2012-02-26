using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TeteTopApi.Entity;

namespace TeteTopApi.DataContract
{
    public class WeiboData
    {
        /// <summary>
        /// 获取需要发送的微博信息
        /// </summary>
        /// <param name="nick"></param>
        /// <returns></returns>
        public Weibo GetUserWeiboData(string nick)
        {
            Weibo weibo = new Weibo();
            string sql = "SELECT content1,content2,content3,content4 FROM TopMicroBlogAuto WHERE nick = '" + nick + "'";

            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                weibo.ContentUp = dt.Rows[0][0].ToString();
                weibo.ContentSell = dt.Rows[0][1].ToString();
                weibo.ContentReview = dt.Rows[0][2].ToString();
                weibo.ContentRecommend = dt.Rows[0][3].ToString();
            }

            return weibo;
        }

        /// <summary>
        /// 获取需要发送的微博帐号
        /// </summary>
        /// <param name="nick"></param>
        /// <returns></returns>
        public List<WeiboID> GetUserWeiboIDS(string nick)
        {
            List<WeiboID> weiboids = new List<WeiboID>();
            string sql = "SELECT uid,typ,tokenKey,tokenSecrect FROM TopMicroBlogAccount WHERE nick = '" + nick + "'";

            DataTable dt = utils.ExecuteDataTable(sql);
            for (int i = 0; i < dt.Rows.Count;i++ )
            {
                WeiboID weibo = new WeiboID();
                weibo.ID = dt.Rows[i][0].ToString();
                weibo.Typ = dt.Rows[i][1].ToString();
                weibo.Key = dt.Rows[i][2].ToString();
                weibo.Secret = dt.Rows[i][3].ToString();
                weiboids.Add(weibo);
            }

            return weiboids;
        }

        /// <summary>
        /// 更新微博发送数量
        /// </summary>
        /// <param name="nick"></param>
        /// <param name="index"></param>
        public void UpdateWeiboNum(string nick, string index)
        {
            string sql = "UPDATE TopMicroBlogAuto SET num{0} = num{0} + 1 WHERE nick = '{1}'";
            sql = sql.Replace("{0}", index);
            sql = sql.Replace("{1}", nick);

            utils.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 记录微博发送日志
        /// </summary>
        /// <param name="trade"></param>
        public void InsertWeiboSendLog(string nick, WeiboID weibo, string content, string index)
        {
            string sql = "INSERT INTO [TopMicroBlogSendLog] (" +
                                "nick, " +
                                "uid, " +
                                "typ, " +
                                "auto, " +
                                "content " +
                            " ) VALUES ( " +
                                " '" + nick + "', " +
                                " '" + weibo.ID + "', " +
                                " '" + weibo.Typ + "', " +
                                " '" + index + "', " +
                                " '" + content + "' " +
                            ") ";
            Console.Write(sql + "\r\n");
            utils.ExecuteNonQuery(sql);
        }


        /// <summary>
        /// 判断同类型微博每8小时内最多发一条
        /// </summary>
        /// <param name="trade"></param>
        public bool IsCanSendMsg(string nick, WeiboID weibo, string index)
        {
            string sql = "SELECT * FROM TopMicroBlogSendLog WHERE DATEDIFF(hh, adddate, GETDATE()) < 8 AND nick = '" + nick + "' AND uid = '" + weibo.ID + "' AND typ = '" + weibo.Typ + "' AND auto = '" + index + "'";
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
