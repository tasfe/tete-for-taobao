using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Threading;
using TeteTopApi.DataContract;
using TeteTopApi.Entity;

namespace TeteTopApi
{
    public class WebPost
    {
        //private static object padlock = new object();

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
                    postData.Append(Uri.EscapeDataString(value));
                    hasParam = true;
                }
            }
            return postData.ToString();
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
        public static string CommonPost(string url, string appkey, string appSecret, string method, string session, IDictionary<string, string> param)
        {
            try
            {
                #region -----API系统参数----
                param.Add("app_key", appkey);
                param.Add("method", method);
                param.Add("session", session);
                param.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                param.Add("format", "json");
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
            catch (Exception e)
            {
                return e.StackTrace;
            }
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
        public static string CommonPostXml(string url, string appkey, string appSecret, string method, string session, IDictionary<string, string> param)
        {
            try
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
            catch (Exception e)
            {
                return e.StackTrace;
            }
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
        public string Post(string url, string appkey, string appSecret, string method, string session, IDictionary<string, string> param)
        {
            #region -----API系统参数----
            param.Add("app_key", appkey);
            param.Add("timestamp", DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss"));
            //param.Add("timestamp", "2011-11-02 09:21:32");
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

            Stream stream = null;
            StreamReader reader = null;
            HttpWebResponse rsp = null;
            rsp = (HttpWebResponse)req.GetResponse();
            Encoding encoding = Encoding.UTF8;
            stream = rsp.GetResponseStream();

            // 获取线程池的最大线程数和维护的最小空闲线程数 　　
            ThreadPool.SetMaxThreads(200, 200);
            ThreadPool.SetMinThreads(1, 1);

            int i = 0;
            string resultOld = string.Empty;

            while (true)
            {
                //如果读取报错
                try
                {
                    reader = new StreamReader(stream, encoding);
                    result = reader.ReadLine();
                }
                catch(Exception e)
                {
                    Console.Write("通讯中断，重新启动");
                    return "";
                }

                //逻辑中的错误判定处理
                try
                {
                    if (!string.IsNullOrEmpty(result))
                    {
                        if (resultOld != result)
                        {
                            //如果是服务器中断则退出
                            if (result.IndexOf("code\":10") != -1)
                            {
                                return "";
                            }

                            resultOld = result;
                            string resultNew = result;
                            Console.Write("\r\n[" + DateTime.Now.ToString() + "]-[" + i.ToString() + "]--[" + resultNew + "]\r\n");

                            LogData dbLog = new LogData();
                            Trade trade = utils.GetTrade(resultNew);
                            dbLog.InsertMsgLogInfo(trade.Nick, trade.Status, resultNew);

                            //ThreadPool.QueueUserWorkItem(new WaitCallback(StartReceiveMessage), resultNew);
                        }
                    }
                }
                catch(Exception ex)
                {
                    Console.Write(ex.Message + "||" + ex.StackTrace + "\r\n");
                }

                i++;
            }

            if (reader != null) reader.Close();
            if (stream != null) stream.Close();
            if (rsp != null) rsp.Close();

            #endregion
            return Regex.Replace(result, @"[\x00-\x08\x0b-\x0c\x0e-\x1f]", "");
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
        public string PostFree(string url, string appkey, string appSecret, string method, string session, IDictionary<string, string> param)
        {
            #region -----API系统参数----
            param.Add("app_key", appkey);
            param.Add("timestamp", DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss"));
            //param.Add("timestamp", "2011-11-02 09:21:32");
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

            Stream stream = null;
            StreamReader reader = null;
            HttpWebResponse rsp = null;
            rsp = (HttpWebResponse)req.GetResponse();
            Encoding encoding = Encoding.UTF8;
            stream = rsp.GetResponseStream();

            // 获取线程池的最大线程数和维护的最小空闲线程数 　　
            ThreadPool.SetMaxThreads(200, 200);
            ThreadPool.SetMinThreads(1, 1);

            int i = 0;
            string resultOld = string.Empty;

            while (true)
            {
                //如果读取报错
                try
                {
                    reader = new StreamReader(stream, encoding);
                    result = reader.ReadLine();
                }
                catch
                {
                    Console.Write("通讯中断，重新启动");
                    return "";
                }

                //逻辑中的错误判定处理
                try
                {
                    if (!string.IsNullOrEmpty(result))
                    {
                        if (resultOld != result)
                        {
                            //如果是服务器中断则退出
                            if (result.IndexOf("code\":10") != -1)
                            {
                                return "";
                            }

                            resultOld = result;
                            string resultNew = result;
                            Console.Write("\r\n[" + DateTime.Now.ToString() + "]-[" + i.ToString() + "]--[" + resultNew + "]\r\n");
                            LogData dbLog = new LogData();
                            string status = utils.GetValueByProperty(resultNew, "status");
                            string nick = utils.GetValueByProperty(resultNew, "nick");
                            if (nick.Length != 0)
                            {
                                dbLog.InsertMsgLogInfo(nick, status, resultNew);
                            }

                            ThreadPool.QueueUserWorkItem(new WaitCallback(StartReceiveMessageFree), resultNew);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message + "||" + ex.StackTrace + "\r\n");
                }

                i++;
            }

            if (reader != null) reader.Close();
            if (stream != null) stream.Close();
            if (rsp != null) rsp.Close();

            #endregion
            return Regex.Replace(result, @"[\x00-\x08\x0b-\x0c\x0e-\x1f]", "");
        }



        private void StartReceiveMessage(object result)
        {
            try
            {
                ReceiveMessage msg = new ReceiveMessage(result.ToString());
                msg.ActData();
            }
            catch (Exception ex)
            {
                LogData dbLog = new LogData();
                Trade trade = utils.GetTrade(result.ToString());
                dbLog.InsertErrorLog(trade.Nick, "ThreadError", "", result.ToString(), ex.Message + "||" + ex.StackTrace + "\r\n");
                Console.Write(ex.Message + "||" + ex.StackTrace + "\r\n");
            }
        }

        private void StartReceiveMessageFree(object result)
        {
            try
            {
                ReceiveMessageFree msg = new ReceiveMessageFree(result.ToString());
                msg.ActData();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message + "||" + ex.StackTrace + "\r\n");
            }
        }
    }
}
