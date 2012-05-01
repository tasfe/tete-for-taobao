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
namespace teteactivitystart
{
    public partial class Form1 : Form
    {
        public static string logUrl = "D:/svngroupbuy/website/ErrLog";
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;

             

            //刷新已开始的服务
            Thread newThread3 = new Thread(activityStart);
            newThread3.Start();

          
        }

         

        


        /// <summary>
        /// 删除活动（更新结束活动）
        /// </summary>
        /// <param name="promotion_id"></param>
        /// <param name="session"></param>
        /// <param name="actionId"></param>
        /// <param name="iid"></param>
        public void delactivity(string promotion_id, string session, string actionId, string iid)
        {

            string appkey = "12287381";
            string secret = "d3486dac8198ef01000e7bd4504601a4";
            IDictionary<string, string> param = new Dictionary<string, string>();

            //删除活动
            param = new Dictionary<string, string>();
            param.Add("promotion_id", promotion_id);
            string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.marketing.promotion.delete", session, param);

            //更新结束活动
            string sql = "update  tete_activitylist set Status=2 ,isok=1  WHERE ActivityID = " + actionId + " and  ProductID=" + iid;
            DBSql.getInstance().ExecSql(sql);


        }
 
 

        /// <summary>
        /// 刷新已开始的服务(判断结束时间)
        /// </summary>
        private void activityStart()
        {
            try
            {
                string session = string.Empty;

                DBSql db = DBSql.getInstance();

                string sql1 = "select * from tete_activity where status=1 and  DATEDIFF(s,GETDATE(),endDate) < 0 "; //活动到期的活动
                DataTable dt1 = DBSql.getInstance().GetTable(sql1);
                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        string sql2 = " select * from tete_activitylist where  ActivityID=" + dt1.Rows[i]["ID"].ToString();//更新该活动下的商品
                        DataTable dt1s2 = DBSql.getInstance().GetTable(sql2);
                        if (dt1s2 != null && dt1s2.Rows.Count > 0)
                        {
                            for (int j = 0; j < dt1s2.Rows.Count; j++)
                            {
                                string sqlstr1 = "SELECT session FROM TopTaobaoShop WHERE nick = '" + dt1s2.Rows[j]["nick"].ToString() + "'";

                                DataTable dtnick = db.GetTable(sqlstr1);
                                if (dtnick.Rows.Count != 0)
                                {
                                    session = dtnick.Rows[0]["session"].ToString();
                                }
                                //删除活动
                                delactivity(dt1s2.Rows[j]["promotionID"].ToString(), session, dt1s2.Rows[j]["ActivityID"].ToString(), dt1s2.Rows[j]["ProductID"].ToString());
                            }
                        }
                        sql1 = "update tete_activity set Status=2, isok=1 where id=" + dt1.Rows[i]["ID"].ToString(); //更新活动到期状态
                        DBSql.getInstance().ExecSql(sql1);
                    }
                }
                dt1.Dispose();

                string sqls3 = " select * from tete_activitylist where status=1 and   DATEDIFF(s,GETDATE(),endDate) < 0";//未开始的活动列表

                DataTable dt1s3 = DBSql.getInstance().GetTable(sqls3);
                if (dt1s3 != null && dt1s3.Rows.Count > 0)
                {
                    for (int j = 0; j < dt1s3.Rows.Count; j++)
                    {
                        string sqlstr1 = "SELECT session FROM TopTaobaoShop WHERE nick = '" + dt1s3.Rows[j]["nick"].ToString() + "'";

                        DataTable dtnick = db.GetTable(sqlstr1);
                        if (dtnick.Rows.Count != 0)
                        {
                            session = dtnick.Rows[0]["session"].ToString();
                        }

                        //删除活动
                        delactivity(dt1s3.Rows[j]["promotionID"].ToString(), session, dt1s3.Rows[j]["ActivityID"].ToString(), dt1s3.Rows[j]["ProductID"].ToString());
                    }
                }
                dt1s3.Dispose();
                //休息后继续循环-默认1分半钟一次
                Thread.Sleep(300000);

                Thread newThread33 = new Thread(activityStart);
                newThread33.Start();
            }
            catch (Exception e)
            {

                WriteLog2("自动取消活动运行错误*****************************************" + e.StackTrace + e.Message + "----error!!!", "1");
                //MessageBox.Show("\r\n" + e.StackTrace);
                Thread newThread3 = new Thread(activityStart);
                //休息后继续循环-默认1分半钟一次 
                Thread.Sleep(300000);
                newThread3.Start();
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
            string tempFile = tempStr + "/promotionID" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (type == "1")
            {
                tempFile = tempStr + "/ErrpromotionID" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
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
        public static void WriteLog2(string message, string type)
        {
            string tempStr = logUrl + "/Write" + DateTime.Now.ToString("yyyyMMdd");//文件夹路径
            string tempFile = tempStr + "/stapromotionID" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (type == "1")
            {
                tempFile = tempStr + "/ErrstapromotionID" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
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
        public static void WriteLog3(string message, string type)
        {
            string tempStr = logUrl + "/Write" + DateTime.Now.ToString("yyyyMMdd");//文件夹路径
            string tempFile = tempStr + "/promotionID" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (type == "1")
            {
                tempFile = tempStr + "/ErrpromotionID" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
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
        public static void WriteLog4(string message, string type)
        {
            string tempStr = logUrl + "/Write" + DateTime.Now.ToString("yyyyMMdd");//文件夹路径
            string tempFile = tempStr + "/promotionID" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (type == "1")
            {
                tempFile = tempStr + "/ErrpromotionID" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
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
            string tempFile = tempStr + "/DeletepromotionID" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (type == "1")
            {
                tempFile = tempStr + "/DeleteErrpromotionID" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
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
        /// 更新宝贝详细日志
        /// </summary>
        /// <param name="message">返回结果</param>
        /// <param name="type">成功、错误</param>
        /// <param name="groupbuyID">活动ID</param>
        public static void WriteLog(string message, string type, string groupbuyID)
        {
            string tempStr = logUrl + "/Write" + DateTime.Now.ToString("yyyyMMdd");//文件夹路径
            string tempFile = tempStr + "/stapromotionID" + groupbuyID + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (type == "1")
            {
                tempFile = tempStr + "/ErrstapromotionID" + groupbuyID + DateTime.Now.ToString("yyyyMMdd") + ".txt";
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
        /// <param name="groupbuyID">活动ID</param>
        /// <returns></returns>
        public static void WriteDeleteLog(string message, string type, string groupbuyID)
        {
            string tempStr = logUrl + "/Write" + DateTime.Now.ToString("yyyyMMdd");//文件夹路径
            string tempFile = tempStr + "/DeletepromotionID" + groupbuyID + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (type == "1")
            {
                tempFile = tempStr + "/DeleteErrpromotionID" + groupbuyID + DateTime.Now.ToString("yyyyMMdd") + ".txt";
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
                sr.WriteLine("Encoding: {0}", sr.Encoding.ToString());

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
