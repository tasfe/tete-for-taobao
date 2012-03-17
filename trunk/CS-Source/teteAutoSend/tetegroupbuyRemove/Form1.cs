using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace tetegroupbuyRemove
{
    public partial class Form1 : Form
    {
        public static string logUrl = "D:/svngroupbuy/website/ErrLog";
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;

            //获取到期团购,创建取消团购任务
            Thread newThread1 = new Thread(DelGroupbuy);
            newThread1.Start();
        }

 
        /// <summary>
        ///  获取到期团购,创建取消团购任务
        /// </summary>
        private void DelGroupbuy()
        {
            try
            {
                //获取正在进行中的团购项目        
                string appkey = "12287381";//"12287381";
                string secret = "d3486dac8198ef01000e7bd4504601a4";//d3486dac8198ef01000e7bd4504601a4";
                string session = string.Empty;

                //获取已经结束的团购活动并取消
                DBSql db = DBSql.getInstance();
                string sql = "SELECT * FROM TopGroupBuy WHERE DATEDIFF(s,GETDATE(),endtime) < 0  AND isdelete = 0";

                #region 取消活动 创建取消任务 删除该活动关联的用户群 将该团购标志为已结束 AND promotionid <> 0
                //WriteLog(sql, "");
                DataTable enddt = db.GetTable(sql);
                //通过接口将该用户加入人群
                for (int y = 0; y < enddt.Rows.Count; y++)
                {
                    //活动id为空
                    if (enddt.Rows[y]["promotionid"].ToString().Trim() == "0")
                    {
                        sql = "UPDATE TopGroupBuy SET isdelete = 1 WHERE id = " + enddt.Rows[y]["id"].ToString();
                        db.ExecSql(sql);
                        continue;
                    }
                    //店铺id为空
                    if (enddt.Rows[y]["nick"].ToString().Trim() == "")
                    {
                        sql = "UPDATE TopGroupBuy SET isdelete = 1 WHERE id = " + enddt.Rows[y]["id"].ToString();
                        db.ExecSql(sql);
                        continue;
                    }
                    sql = "SELECT session FROM TopTaobaoShop WHERE nick = '" + enddt.Rows[y]["nick"].ToString() + "'";

                    WriteLog("清除代码:" + sql, "");
                    DataTable dtnick = db.GetTable(sql);
                    if (dtnick.Rows.Count != 0)
                    {
                        session = db.GetTable(sql).Rows[0][0].ToString();
                    }
                    //店铺session为空
                    if (session == "")
                    {
                        sql = "UPDATE TopGroupBuy SET isdelete = 1 WHERE id = " + enddt.Rows[y]["id"].ToString();
                        db.ExecSql(sql);
                        continue;
                    }
                    //取消该活动
                    IDictionary<string, string> paramnew = new Dictionary<string, string>();
                    paramnew.Add("promotion_id", enddt.Rows[y]["promotionid"].ToString());
                    string resultnew = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.marketing.promotion.delete", session, paramnew);

                    WriteLog("清除代码:" + resultnew, "");

                    //创建删除活动关联的宝贝描述
                    Delete(enddt.Rows[y]["nick"].ToString(), enddt.Rows[y]["id"].ToString());
                    //将该团购标志为已结束
                    sql = "UPDATE TopGroupBuy SET isdelete = 1 WHERE id = " + enddt.Rows[y]["id"].ToString();
                    
                    db.ExecSql(sql);
                }
                #endregion


                enddt.Dispose();

                WriteLog("**********************************************************", "");
                //休息后继续循环-默认15分钟一次
                Thread.Sleep(600000);

                Thread newThread = new Thread(DelGroupbuy);
                newThread.Start();
            }
            catch (Exception e)
            {

                WriteLog("**********************************************************" + e.StackTrace + "----error!!!", "1");
                //MessageBox.Show("\r\n" + e.StackTrace);
                Thread newThread = new Thread(DelGroupbuy);
                //休息后继续循环-默认10分钟一次
                Thread.Sleep(900000);
                newThread.Start();
            }
        }


        /// <summary>
        /// 创建删除活动关联的宝贝描述
        /// </summary>
        /// <param name="taobaoNick"></param>
        /// <param name="id">活动ID</param>
        private void Delete(string taobaoNick, string id)
        {

            string sql = "SELECT COUNT(*) as count FROM TopMission WHERE groupbuyid = " + id + " AND typ='delete' AND isok = 0 AND nick<>''";

            DBSql db = DBSql.getInstance();
            DataTable dt3 = db.GetTable(sql);
            if (dt3 != null && dt3.Rows.Count > 0)
            {
                if (dt3.Rows[0]["count"].ToString().Trim() != "0")
                {
                    return;
                }
            }

            sql = "INSERT INTO TopMission (typ, nick, groupbuyid) VALUES ('delete', '" + taobaoNick + "', '" + id + "')";
            db.ExecSql(sql);


            sql = "SELECT TOP 1 ID FROM TopMission ORDER BY ID DESC";
            dt3 = db.GetTable(sql);
            if (dt3 != null && dt3.Rows.Count > 0)
            {
                string missionid = dt3.Rows[0]["ID"].ToString();

                //获取团购信息并更新
                sql = "SELECT name,productimg,productid FROM TopGroupBuy WHERE id = '" + id + "'";
                DataTable dt = db.GetTable(sql);
                if (dt.Rows.Count != 0)
                {
                    sql = "UPDATE TopMission SET groupbuyname = '" + dt.Rows[0]["name"].ToString() + "',groupbuypic = '" + dt.Rows[0]["productimg"].ToString() + "',itemid = '" + dt.Rows[0]["productid"].ToString() + "' WHERE id = " + missionid;
                    db.ExecSql(sql);
                }

                //更新任务总数
                sql = "SELECT COUNT(*) as count  FROM TopWriteContent WHERE groupbuyid = '" + id + "' AND isok = 1";
                dt3 = db.GetTable(sql);
                if (dt3 != null && dt3.Rows.Count > 0)
                {
                    sql = "UPDATE TopMission SET total = '" + dt3.Rows[0]["count"] + "' WHERE id = " + missionid;
                    db.ExecSql(sql);
                }

            }

        }


        #region TOP API
        /// <summary> 
        /// 给TOP请求签名 API v2.0 
        /// </summary> 
        /// <param name="parameters">所有字符型的TOP请求参数</param> 
        /// <param name="secret">签名密钥</param> 
        /// <returns>签名</returns> 
        protected static string CreateSign(IDictionary<string, string> parameters, string secret)
        {
            parameters.Remove("sign");
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();
            StringBuilder query = new StringBuilder(secret);
            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    query.Append(key).Append(value);
                }
            }
            query.Append(secret);
            MD5 md5 = MD5.Create();
            byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(query.ToString()));
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                string hex = bytes[i].ToString("X");
                if (hex.Length == 1)
                {
                    result.Append("0");
                }
                result.Append(hex);
            }
            return result.ToString();
        }
        /// <summary> 
        /// 组装普通文本请求参数。 
        /// </summary> 
        /// <param name="parameters">Key-Value形式请求参数字典</param> 
        /// <returns>URL编码后的请求数据</returns> 
        protected static string PostData(IDictionary<string, string> parameters)
        {
            StringBuilder postData = new StringBuilder();
            bool hasParam = false;
            IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                string value = dem.Current.Value;
                // 忽略参数名或参数值为空的参数 
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                {
                    if (hasParam)
                    {
                        postData.Append("&");
                    }
                    postData.Append(name);
                    postData.Append("=");
                    // postData.Append(Uri.EscapeDataString(value));
                    postData.Append(GetUriFormate(value));
                    hasParam = true;
                }
            }
            return postData.ToString();
        }

        /// <summary>
        /// 将参数转换成 uri 格式
        /// </summary>
        /// <param name="inputString">string类型的字符串</param>
        /// <returns>编码后的string</returns>
        private static string GetUriFormate(string inputString)
        {
            StringBuilder strBuilder = new StringBuilder();
            string sourceStr = inputString;
            int len = sourceStr.Length;
            do
            {
                if (len - 21766 <= 0)
                {
                    strBuilder.Append(Uri.EscapeDataString(sourceStr));
                }
                else
                {
                    strBuilder.Append(Uri.EscapeDataString(sourceStr.Substring(0, 21766)));

                    sourceStr = sourceStr.Substring(21766);
                    len = sourceStr.Length;
                    if (len - 21766 < 0)
                    {
                        strBuilder.Append(Uri.EscapeDataString(sourceStr));
                    }
                }
            }
            while (len - 21766 > 0);

            return strBuilder.ToString();
        }
        /// <summary> 
        /// TOP API POST 请求 
        /// </summary> 
        /// <param name="url">请求容器URL</param> 
        /// <param name="appkey">AppKey</param> 
        /// <param name="appSecret">AppSecret</param> 
        /// <param name="method">API接口方法名</param> 
        /// <param name="session">调用私有的sessionkey</param> 
        /// <param name="param">请求参数</param> 
        /// <returns>返回字符串</returns> 
        public static string Post(string url, string appkey, string appSecret, string method, string session,
        IDictionary<string, string> param)
        {
            #region -----API系统参数----
            param.Add("app_key", appkey);
            param.Add("method", method);
            param.Add("session", session);
            param.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            param.Add("format", "xml");
            param.Add("v", "2.0");
            param.Add("sign_method", "md5");
            param.Add("sign", CreateSign(param, appSecret));
            #endregion
            string result = string.Empty;
            #region ---- 完成 HTTP POST 请求----
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.KeepAlive = true;
            req.Timeout = 300000;
            req.ContentType = "application/x-www-form-urlencoded;charset=gb2312";
            byte[] postData = Encoding.UTF8.GetBytes(PostData(param));
            Stream reqStream = req.GetRequestStream();
            reqStream.Write(postData, 0, postData.Length);
            reqStream.Close();
            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
            Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
            Stream stream = null;
            StreamReader reader = null;
            stream = rsp.GetResponseStream();
            reader = new StreamReader(stream, encoding);
            result = reader.ReadToEnd();
            if (reader != null) reader.Close();
            if (stream != null) stream.Close();
            if (rsp != null) rsp.Close();
            #endregion
            return Regex.Replace(result, @"[\x00-\x08\x0b-\x0c\x0e-\x1f]", "");
        }


        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="value">日志内容</param>
        /// <param name="type">类型 0(成功日志),1(错误日志) 可传空文本默认为0</param>
        /// <returns></returns>
        public static void WriteLog(string message, string type)
        {
            string tempStr = logUrl + "/Write" + DateTime.Now.ToString("yyyyMMdd");//文件夹路径
            string tempFile = tempStr + "/" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (type == "1")
            {
                tempFile = tempStr + "/Err" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            }
            if (!Directory.Exists(tempStr))
            {
                Directory.CreateDirectory(tempStr);
            }

            if (System.IO.File.Exists(tempFile))
            {
                ///如果日志文件已经存在，则直接写入日志文件
                StreamWriter sr = System.IO.File.AppendText(tempFile);
                sr.WriteLine("\n");
                sr.WriteLine(DateTime.Now + "\n" + message);
                sr.Close();
            }
            else
            {
                ///创建日志文件
                StreamWriter sr = System.IO.File.CreateText(tempFile);
                sr.WriteLine(DateTime.Now + "\n" + message);
                sr.Close();
            }

        }


        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="value">日志内容</param>
        /// <param name="type">类型 0(成功日志),1(错误日志) 可传空文本默认为0</param>
        /// <returns></returns>
        public static void WriteDeleteLog(string message, string type)
        {
            string tempStr = logUrl + "/Write" + DateTime.Now.ToString("yyyyMMdd");//文件夹路径
            string tempFile = tempStr + "/Delete" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (type == "1")
            {
                tempFile = tempStr + "/DeleteErr" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            }
            if (!Directory.Exists(tempStr))
            {
                Directory.CreateDirectory(tempStr);
            }

            if (System.IO.File.Exists(tempFile))
            {
                ///如果日志文件已经存在，则直接写入日志文件
                StreamWriter sr = System.IO.File.AppendText(tempFile);
                sr.WriteLine("\n");
                sr.WriteLine(DateTime.Now + "\n" + message);
                sr.Close();
            }
            else
            {
                ///创建日志文件
                StreamWriter sr = System.IO.File.CreateText(tempFile);
                sr.WriteLine(DateTime.Now + "\n" + message);
                sr.Close();
            }

        }
        #endregion

    }
}
