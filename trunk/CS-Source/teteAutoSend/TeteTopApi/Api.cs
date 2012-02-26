using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeteTopApi.DataContract;

namespace TeteTopApi
{
    public class Api
    {
        /// <summary>
        /// 初始化函数
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="secret"></param>
        /// <param name="session"></param>
        /// <param name="url"></param>
        public Api(string appKey, string secret, string session, string url) 
        {
            this.AppKey = appKey;
            this.Secret = secret;
            this.Session = session;
            this.Url = url;
        }

        public string CommonTopApi(string method, IDictionary<string, string> param, string session)
        {
            string result = WebPost.CommonPost(Url, AppKey, Secret, method, session, param);

            if (result.IndexOf("Missing session") != -1)
            { 
                //错误记录
                LogData dbLog = new LogData();
                dbLog.InsertErrorLog("system", "MissingSession", "", result.ToString(), "");
            }

            return result;
        }

        public string CommonTopApiXml(string method, IDictionary<string, string> param, string session)
        {
            string result = WebPost.CommonPostXml(Url, AppKey, Secret, method, session, param);

            if (result.IndexOf("Missing session") != -1)
            {
                //错误记录
                LogData dbLog = new LogData();
                dbLog.InsertErrorLog("system", "MissingSession", "", result.ToString(), "");
            }

            return result;
        }

        /// <summary>
        /// 连接通知服务器
        /// </summary>
        public string ConnectServer()
        {
            WebPost post = new WebPost();

            IDictionary<string, string> param = new Dictionary<string, string>();
            string result = post.Post(Url, AppKey, Secret, "", Session, param);

            return "";
        }

        /// <summary>
        /// 连接通知服务器
        /// </summary>
        public string ConnectServerFree()
        {
            WebPost post = new WebPost();

            IDictionary<string, string> param = new Dictionary<string, string>();
            string result = post.PostFree(Url, AppKey, Secret, "", Session, param);

            return "";
        }

        public string AppKey { get; set; }
        public string Secret { get; set; }
        public string Session { get; set; }
        public string Url { get; set; }
    }
}
